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
    public class AttachmentCreateViewModel : ViewModelBase, ISaveCancelDialog
    {
        private static ILog logger = LogManager.GetLogger(typeof(AttachmentCreateViewModel));

        private readonly IInvWindowManager _windowManager;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected AttachmentCreateViewModel()
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public AttachmentCreateViewModel(FacilityAttachmentViewModel attachment,
                        IEventAggregator eventAggregator,
                        IInvWindowManager windowManager
                        )
            : base(eventAggregator)
        {
            FacilityAttachment = attachment;
            _windowManager = windowManager;

            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
        }

        #endregion

        #region Properties

        public FacilityAttachmentViewModel FacilityAttachment { get; set; }

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

        public void Save()
        {
            // validation

            //CursorHelper.ExecuteWithWaitCursor(() =>
            //{
            //    this.Facility.SaveFacility(true,
            //delegate (InvFacility facility)
            //{
            //    this.Facility.Model = facility;
            //    _windowManager.Inform("Create Facility", "Facility saved successfully");
            //    this.DialogResult = true;

            //    EventAggregator.PublishOnUIThread(new FacilityUpdatedMessage()
            //    {
            //        FacilityUpdateType = FacilityUpdateType.Create,
            //        Facility = facility,
            //    });
            //},
            //delegate
            //{
            //    _windowManager.ShowError("Create Attachment", "Facility save failed");
            //});
        }

        public void Cancel()
        {
            this.DialogResult = false;
        }

        public bool? DialogResult { get; private set; }

        #endregion
    }
}
