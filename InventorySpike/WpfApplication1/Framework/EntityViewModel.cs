using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using PropertyChanged;

namespace Client.Framework
{
    [ImplementPropertyChanged]
    public abstract class EntityViewModel : ViewModelBase, IEntityViewModel, IDataErrorInfo, IValidatable, IHasChanges
    {
        #region Properties & Fields

        protected Dictionary<string, Validator> ValidatorList = new Dictionary<string, Validator>();

        protected object _Model;

        #endregion

        protected EntityViewModel()
        {
        }

        ~EntityViewModel()
        {
            Dispose(false);
        }

        protected EntityViewModel(object model)
        {
            Model = model;
        }

        protected EntityViewModel(object model, IEventAggregator eventAggregator) : base(eventAggregator)
        {
            Model = model;
        }

        protected virtual void OnModelChanged() { }

        #region IdentityViewModel implementations


        [DoNotNotify]
        public virtual object Model
        {
            get { return _Model; }
            set
            {
                var oldModel = _Model;
                if (_Model != value)
                {
                    _Model = value;
                    RegisterModelForDirtyTracking(_Model, oldModel);
                }

                NotifyOfPropertyChange(() => this.Model);
                NotifyOfPropertyChange("");
                //NotifyOfPropertyChange("EntityId");
            }
        }

        public virtual long? EntityId
        {
            get { return 0; }
        }

        public void RefreshModel()
        {
        }

        public bool SaveChanges()
        {
            return false;
        }

        #endregion

        #region IDataErrorInfo

        public virtual ValidationResults Validate(bool validateRelatedObjects)
        {
            var results = new ValidationResults();
            foreach (var property in this.GetType().GetProperties())
            {
                // related properties
                if (property.PropertyType.GetInterface(typeof(IEnumerable).FullName) != null && property.PropertyType.IsGenericType && validateRelatedObjects)
                {
                    // collection type
                    if (!validateRelatedObjects) continue;
                    var collection = property.GetValue(this, null) as IEnumerable;
                    if (collection != null)
                        foreach (var item in collection)
                        {
                            // only run validation if the item is an entity itself
                            if (item is IValidatable)
                            {
                                results.AddAllResults((item as IValidatable).Validate(false));
                            }
                        }
                }
                else   // simple properties on the underlying Model
                {
                    // simple type property
                    var entityProperty = Model.GetType().GetProperty(property.Name);
                    if (entityProperty == null) continue;

                    var validator = GetPropertyValidator(this, entityProperty);
                    if (validator != null)
                    {
                        validator.Validate(this.Model, results);
                    }
                }
            }
            return results;
        }

        public string Error
        {
            get
            {
                var resultingErrors = new List<string>();

                var results = this.Validate(false);
                foreach (var validationResult in results)
                    if (!string.IsNullOrEmpty(validationResult.Message))
                        resultingErrors.Add(validationResult.Message);

                return string.Join(Environment.NewLine, resultingErrors);
            }
        }

        public string this[string columnName]
        {
            get
            {
                // validate on property on the Model
                PropertyInfo property = Model.GetType().GetProperty(columnName);
                if (property == null) return string.Empty;

                var validator = GetPropertyValidator(Model, property);
                if (validator == null) return string.Empty;

                var resultingErrors = new List<string>();
                var results = new ValidationResults();
                validator.Validate(Model, results);
                foreach (var validationResult in results)
                    if (!string.IsNullOrEmpty(validationResult.Message))
                        resultingErrors.Add(validationResult.Message);

                return string.Join(Environment.NewLine, resultingErrors);
            }
        }

        private Validator GetPropertyValidator(Type entityType, PropertyInfo property)
        {
            string ruleset = string.Empty;
            var source = ValidationSpecificationSource.All;
            var builder = new ReflectionMemberValueAccessBuilder();

            return PropertyValidationFactory.GetPropertyValidator(entityType, property, ruleset, source, builder);
        }

        private Validator GetPropertyValidator(object entity, PropertyInfo property)
        {
            if (!ValidatorList.ContainsKey(property.Name))
                ValidatorList.Add(property.Name, GetPropertyValidator(entity.GetType(), property));

            return ValidatorList[property.Name];
        }

        #endregion

        #region IHasChanges

        public bool HasChanges { get; set; }

        public void AcceptChanges()
        {
            this.HasChanges = false;
        }

        #endregion

        #region protected methods

        protected void RegisterModelForDirtyTracking(object newModel, object oldModel)
        {
            if (newModel != null && newModel is INotifyPropertyChanged)
            {
                (newModel as INotifyPropertyChanged).PropertyChanged += EntityViewModel_PropertyChanged;
            }
            if (oldModel != null && oldModel is INotifyPropertyChanged)
            {
                (oldModel as INotifyPropertyChanged).PropertyChanged -= EntityViewModel_PropertyChanged;
            }
        }

        private void EntityViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.HasChanges = true;
        }

        protected void RegisterListForDirtyTracking(PropertyInfo propertyInfo, IEnumerable<string> childPropertiesToIgnore = null)
        {
            if (propertyInfo == null)
                return;

            var value = propertyInfo.GetValue(this, null);
            if (typeof(INotifyCollectionChanged).IsAssignableFrom(propertyInfo.PropertyType) && value != null)
            {
                NotifyCollectionChangedEventHandler onCollectionChanged = (sender, args) =>
                {
                    if (args.Action == NotifyCollectionChangedAction.Add || args.Action == NotifyCollectionChangedAction.Remove)
                    {
                        this.HasChanges = true;
                    }
                };
                ((INotifyCollectionChanged)value).CollectionChanged += onCollectionChanged;
            }

            // get element's PropertyChanged
            if (typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType) && value != null)
            {
                var collection = value as IEnumerable;

                foreach (var item in collection)
                {
                    if (item is INotifyPropertyChanged)
                    {
                        (item as INotifyPropertyChanged).PropertyChanged += (sender, args) =>
                        {
                            if (childPropertiesToIgnore == null || !childPropertiesToIgnore.Any(x => x == args.PropertyName))
                                this.HasChanges = true;
                        };
                    }
                }
            }
        }

        public void RegisterPropertyForDirtyTracking<TPType>(Expression<Func<TPType>> propertySelector)
        {
            var memberExpression = propertySelector.Body as MemberExpression;
            if (memberExpression == null)
                throw new Exception(string.Format("RegisterPropertyForDirtyTracking():  propertySelector.Body is null or cannot be casted to MemberExpression. PropertySelector = {0}",
                    propertySelector));

            var name = memberExpression.Member.Name;

            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName != name) return;
                this.HasChanges = true;
            };
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    // Clean up managed resources
                    if (this.Model != null && this.Model is INotifyPropertyChanged)
                    {
                        (this.Model as INotifyPropertyChanged).PropertyChanged -= EntityViewModel_PropertyChanged;
                    }
                }

                // Clean up unmanaged resources
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }

    public abstract class EntityViewModel<T> : EntityViewModel, IEntityViewModel<T> where T : class
    {
        #region Implementation of IEntityViewModel<T>

        public virtual new T Model
        {
            get
            {
                return (T)base.Model;
            }

            set
            {
                base.Model = value;
            }
        }

        #endregion Implementation of IEntityViewModel<T>

        protected EntityViewModel(T model, IEventAggregator eventAggregator)
            : base(model, eventAggregator)
        {
        }

        protected EntityViewModel(T model)
            : base(model)
        {
        }

        protected EntityViewModel()
        {
        }
    }
}
