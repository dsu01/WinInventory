using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Client.Framework
{
    public abstract class ScreenViewModelBase : ViewModelBase, IScreen
    {
        protected ScreenViewModelBase()
        {
        }

        protected ScreenViewModelBase(IEventAggregator eventAggregator) : base(eventAggregator)
        {
        }

        #region Properties and Fields

        #endregion


        #region Methods

        #endregion

        #region IScreen Implementation

        #endregion
        public virtual string DisplayName
        {
            get
            {
                return String.Empty;
            }
            set
            {
            }
        }

        public virtual void Activate()
        {
        }

        public event EventHandler<ActivationEventArgs> Activated;

        public virtual bool IsActive
        {
            get { return true; }
        }

        public event EventHandler<DeactivationEventArgs> AttemptingDeactivation;

        public virtual void Deactivate(bool close)
        {
        }

        public event EventHandler<DeactivationEventArgs> Deactivated;

        public virtual void CanClose(Action<bool> callback)
        {
            callback(true);
        }

        public void TryClose(bool? dialogResult = null)
        {
        }
    }
}
