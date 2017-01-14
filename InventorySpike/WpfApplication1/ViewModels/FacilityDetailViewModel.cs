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
    public class FacilityDetailViewModel : EntityViewModel<InvFacility>
    {
        private static ILog logger = LogManager.GetLogger(typeof(FacilityDetailViewModel));

        // Fields
        private readonly IInvWindowManager _windowManager;
        private readonly IFacilitiesService _facilitiesService;
        private readonly IApplicationContext _applicationContext;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected FacilityDetailViewModel()
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public FacilityDetailViewModel(InvFacility facility,
                        FacilityInfoViewModel facilityInfoViewModel,
                        IInvWindowManager windowManager,
                        IEventAggregator eventAggregator,
                        IApplicationContext applicationContext,
                        IFacilitiesService facilitiesService
            )
            : base(facility, eventAggregator)
        {
            _windowManager = windowManager;
            _facilitiesService = facilitiesService;
            _applicationContext = applicationContext;
            FacilityInfoViewModel = facilityInfoViewModel;

            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
        }

        #endregion

        #region Properties

        public FacilityInfoViewModel FacilityInfoViewModel { get; set; }

        public bool CanSave { get { return true; } }

        public bool CanCancel { get { return true; } }

        #endregion

        #region Public Methods

        public async void Save()
        {
            // validation

            await SaveFacility(
            delegate (InvFacility facility)
            {
                this.Model = facility;
                this.FacilityInfoViewModel.Model = facility;

                _windowManager.Inform("Save Facility", "Facility saved successfully");
            },
            delegate
            {
                _windowManager.ShowError("Save Facility", "Facility save failed");
            });
        }

        public void Cancel()
        {
        }

        public async Task SaveFacility(Action<InvFacility> successAction, System.Action failedAction)
        {
            var saved = _facilitiesService.AddOrUpdateInvFacility(this.Model);
            if (saved != null && successAction != null)
            {
                successAction(saved);
            }
            else if (saved == null && failedAction != null)
            {
                failedAction();
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
