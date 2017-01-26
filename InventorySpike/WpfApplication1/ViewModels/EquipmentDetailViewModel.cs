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
    public class EquipmentDetailViewModel : EntityViewModel<InvEquipment>
    {
        private static ILog logger = LogManager.GetLogger(typeof(EquipmentDetailViewModel));

        // Fields
        private readonly IApplicationContext _applicationContext;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected EquipmentDetailViewModel()
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public EquipmentDetailViewModel(InvEquipment equipment,
                        IApplicationContext applicationContext,
                        IEventAggregator eventAggregator
            )
            : base(equipment, eventAggregator)
        {
            _applicationContext = applicationContext;
            DisplayName = equipment.EquipmentID;

            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
        }

        #endregion

        #region Properties


        #endregion

        #region Public Methods

        //public async void Save()
        //{
        //    // validation

        //    await SaveFacility(false,
        //    delegate (InvFacility facility)
        //    {
        //        this.Model = facility;

        //        _windowManager.Inform("Save Facility", "Facility saved successfully");
        //    },
        //    delegate
        //    {
        //        _windowManager.ShowError("Save Facility", "Facility save failed");
        //    });
        //}

        //public void Cancel()
        //{
        //}

        //public async Task SaveFacility(bool addOrUpdate, Action<InvFacility> successAction, System.Action failedAction)
        //{
        //    // TODO - masage 

        //    var saved = _facilitiesService.AddOrUpdateInvFacility(this.Model, addOrUpdate);
        //    if (saved != null && successAction != null)
        //    {
        //        successAction(saved);
        //    }
        //    else if (saved == null && failedAction != null)
        //    {
        //        failedAction();
        //    }
        //}

        #endregion

        #region Private Methods

        #endregion
    }
}
