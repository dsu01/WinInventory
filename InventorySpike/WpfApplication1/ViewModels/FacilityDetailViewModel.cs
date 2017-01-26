﻿using System;
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
using Client.ViewModels.Messages;
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
    public class FacilityDetailViewModel : EntityViewModel<InvFacility>, IHandle<EquipmentSelectedMessage>
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
            DisplayName = facility.Facility_;

            this.Equipments = new ObservableCollection<EquipmentDetailViewModel>(facility.InvEquipments.Select(x => new EquipmentDetailViewModel(x, _windowManager, EventAggregator, _applicationContext, _facilitiesService)));

            SelectedTabIndex = 0;

            SelectedEquipment = null;

            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
        }

        #endregion

        #region Properties

        public int SelectedTabIndex { get; set; }

        public FacilityInfoViewModel FacilityInfoViewModel { get; set; }

        public ObservableCollection<EquipmentDetailViewModel> Equipments { get; private set; }

        public EquipmentDetailViewModel SelectedEquipment { get; set; }

        public bool IsDetailVisible { get { return SelectedEquipment != null;  } }

        public bool CanSave { get { return true; } }

        public bool CanCancel { get { return true; } }

        #endregion

        #region Public Methods

        public async void Save()
        {
            // validation

            await SaveFacility(false,
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

        public async Task SaveFacility(bool addOrUpdate, Action<InvFacility> successAction, System.Action failedAction)
        {
            // TODO - masage 

            var saved = _facilitiesService.AddOrUpdateInvFacility(this.Model, addOrUpdate);
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

        #region Event Hanlding

        public void Handle(EquipmentSelectedMessage message)
        {
            if (Model.SYNC_ID == message.Equipment.InvFacilityId)
            {
                SelectedEquipment = Equipments.FirstOrDefault(x => x.Model.SYNC_ID == message.Equipment.SYNC_ID);
                SelectedTabIndex = 1;
            }
        }

        #endregion
    }
}
