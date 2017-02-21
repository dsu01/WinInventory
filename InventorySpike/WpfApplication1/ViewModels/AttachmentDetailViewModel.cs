using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    public class AttachmentDetailViewModel : EntityViewModel<InvFacilityAttachment>
    {
        private static ILog logger = LogManager.GetLogger(typeof(AttachmentDetailViewModel));
        private readonly IFacilitiesService _facilitiesService;
        private readonly IInvWindowManager _windowManager;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected AttachmentDetailViewModel()
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public AttachmentDetailViewModel(InvFacilityAttachment facilityAttachment,
                        IApplicationContext applicationContext,
                        IEventAggregator eventAggregator,
                        IInvWindowManager windowManager,
                        IFacilitiesService facilitiesService
            )
            : base(facilityAttachment, eventAggregator)
        {
            ApplicationContext = applicationContext;
            _windowManager = windowManager;
            _facilitiesService = facilitiesService;

            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
        }

        #endregion

        #region Properties

        public IApplicationContext ApplicationContext { get; private set; }

        public string FileName
        {
            get { return Model.FileName; }
            set { Model.FileName = value; }
        }

        #endregion

        #region Public Methods

        public void PickFile()
        {
            var result = _windowManager.GetOpenFileDialogNames("Upload File (.*)|*.*", "Select a File", false);
            if (!result.Any())
            {
                return;
            }

            FileName = result[0];
        }

        public void SaveAttachment(bool addOrUpdate, Action<InvFacilityAttachment> successAction,
            System.Action failedAction)
        {
            // get content
            var success = true;
            try
            {
                Model.Data = File.ReadAllBytes(Model.FileName);
            }
            catch (IOException ex)
            {
                success = false;
            }

            if (!success || Model.Data.Length == 0)
            {
                _windowManager.ShowError("Save Attachment", "File No Content");
                return;
            }

            var saved = _facilitiesService.AddOrUpdateInvFacilityAttachment(this.Model, addOrUpdate);
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
