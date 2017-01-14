using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Caliburn.Micro;
using Client.Classes;
using Client.Framework;
using Client.Properties;
using Inventory.Business;
using Inventory.Business.Services;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using Action = System.Action;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

namespace Client.ViewModels
{
    [ImplementPropertyChanged]
    public class FacilityCreateViewModel : ViewModelBase, ISaveCancelDialog
    {
        private static ILog logger = LogManager.GetLogger(typeof(FacilityCreateViewModel));

        private readonly IInvWindowManager _windowManager;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected FacilityCreateViewModel()
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public FacilityCreateViewModel(FacilityDetailViewModel facility,
                        IEventAggregator eventAggregator,
                        IInvWindowManager windowManager
                        )
            : base(eventAggregator)
        {
            Facility = facility;
            _windowManager = windowManager;

            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
        }

        #endregion

        #region Properties

        public FacilityDetailViewModel Facility { get; set; }

        #endregion

        #region Public Methods


        #endregion

        #region Private Methods

        #endregion

        #region ISaveCancelDialog

        public bool CanSave
        {
            get
            {
                // return !IsProcessing && this.FishCatch != null && this.FishCatch.HasChanges;
                return true;
            }
        }

        public bool CanCancel
        {
            get { return true; }
        }

        public async void Save()
        {
            // add to collection
            //var validationResults = this.FishCatch.Validate(true);
            //if (validationResults.Count > 0)
            //{
            //    var message = validationResults.First().Message;
            //    if (!string.IsNullOrEmpty(message))
            //        _windowManager.Warn(FishCatchViewRes.SaveFishCatch, message);
            //    else
            //        _windowManager.Warn(FishCatchViewRes.SaveFishCatch, string.Format(FishCatchViewRes.ValidationError));  // generic message

            //    return;
            //}

            await this.Facility.SaveFacility(
            delegate (InvFacility facility)
            {
                this.Facility.Model = facility;
                _windowManager.Inform("Create Facility", "Facility saved successfully");
                this.DialogResult = true;
            },
            delegate
            {
                _windowManager.ShowError("Create Facility", "Facility save failed");
            });
        }

        public void Cancel()
        {
            this.DialogResult = false;
        }

        public bool? DialogResult { get; private set; }

        #endregion
    }
}
