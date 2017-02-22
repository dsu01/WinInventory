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
using Client.ViewModels.Messages;
using Inventory.Business;
using Inventory.Business.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
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
        private const string _TempImagePath = @"C:\temp";

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

            LoadFacility(facility);

            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
            if (!Directory.Exists(_TempImagePath))
                Directory.CreateDirectory(_TempImagePath);
        }

        #endregion

        #region Properties

        public int SelectedTabIndex { get; set; }

        public FacilityInfoViewModel FacilityInfoViewModel { get; set; }

        public ObservableCollection<EquipmentDetailViewModel> Equipments { get; private set; }

        public EquipmentDetailViewModel SelectedEquipment { get; set; }

        public ObservableCollection<AttachmentDetailViewModel> Attachments { get; private set; }

        public EquipmentDetailViewModel SelecteAttachment { get; set; }

        public bool IsDetailVisible { get { return SelectedEquipment != null; } }

        public bool CanSave { get { return true; } }

        public bool CanCancel { get { return true; } }

        #endregion

        #region Public Methods

        public void Save()
        {
            // validation

            CursorHelper.ExecuteWithWaitCursor(() =>
            {
                SaveFacility(false,
                    delegate (InvFacility facility)
                    {
                        LoadFacility(facility);

                        _windowManager.Inform("Save Facility", "Facility saved successfully");

                        EventAggregator.PublishOnUIThread(new FacilityUpdatedMessage()
                        {
                            FacilityUpdateType = FacilityUpdateType.Updated,
                            Facility = facility,
                        });
                    },
                    delegate
                    {
                        _windowManager.ShowError("Save Facility", "Facility save failed");
                    });
            });
        }

        public void Cancel()
        {
            if (_windowManager.Confirm("Facility Details", "Cancel all unsaved changes for this facility?"))
            {
                CursorHelper.ExecuteWithWaitCursor(() =>
                {
                    this.ReLoadFacility();
                });
            }
        }

        public void SaveFacility(bool addOrUpdate, Action<InvFacility> successAction, System.Action failedAction)
        {
            // convert components
            this.Model.InvEquipments = new List<InvEquipment>(this.Equipments.Select(x => x.Model));

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

        public void AddComponent()
        {
            var equipment = CreateNewEquipment(Model);
            Equipments.Add(equipment);
            SelectedEquipment = equipment;
        }

        public void DeleteComponent()
        {

        }

        #endregion

        #region Attachments

        public void AddAttachment()
        {
            var attachment = InternalAddFacilityAttachment(Model);
            if (attachment != null)
            {
                LoadAttachment(attachment);
                Attachments.Add(attachment);
                SelecteAttachment = null;
            }
        }

        public void DeleteAttachment()
        {

        }

        public void SelectAttachment(object dataContext)
        {
            var vm = dataContext as AttachmentDetailViewModel;
            if (vm == null) return;


        }

        #endregion

        #region Private Methods

        private void LoadFacility(InvFacility savedFacility)
        {
            FacilityInfoViewModel.Model = savedFacility;
            DisplayName = savedFacility.Facility_;
            this.Equipments = new ObservableCollection<EquipmentDetailViewModel>(savedFacility.InvEquipments.OrderBy(x => x.EquipmentName).Select(x => new EquipmentDetailViewModel(x, _applicationContext, EventAggregator)));
            this.Attachments = new ObservableCollection<AttachmentDetailViewModel>(savedFacility.InvFacilityAttachments.OrderBy(x => x.Title).Select(x => new AttachmentDetailViewModel(x, _applicationContext, EventAggregator, _windowManager, _facilitiesService)));
            SelectedTabIndex = 0;

            SelectedEquipment = null;
            SelecteAttachment = null;

            LoadAttachments();
        }

        private void LoadAttachments()
        {
            Attachments.ForEach(x => LoadAttachment(x));
        }

        private void LoadAttachment(AttachmentDetailViewModel facilityAttachment)
        {
            try
            {
                if (facilityAttachment.Model.Data == null) return;

                var fileName = Path.Combine(_TempImagePath, Guid.NewGuid().ToString() + facilityAttachment.Model.ContentType);
                if (File.Exists(fileName))
                    File.Delete(fileName);

                File.WriteAllBytes(fileName, facilityAttachment.Model.Data);

                facilityAttachment.ImageSource = fileName;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private void ReLoadFacility()
        {
            var savedFacility = _facilitiesService.GetFacility(Model.SYNC_ID);

            LoadFacility(savedFacility);
        }


        private EquipmentDetailViewModel CreateNewEquipment(InvFacility facility)
        {

            var equipment = new InvEquipment()
            {
                SYNC_ID = Guid.NewGuid(),
                EquipmentID = "Equipment-0000",
                InvFacilityId = facility.SYNC_ID,
            }
                ;

            return new EquipmentDetailViewModel(equipment, this._applicationContext, EventAggregator);
        }

        private AttachmentDetailViewModel InternalAddFacilityAttachment(InvFacility facility)
        {
            var attahcment = new InvFacilityAttachment()
            {
                ID = Guid.NewGuid(),
                IsActive = true,
                Title = "Attachment-00",
                InvFacilityID = Model.SYNC_ID,
                //CreatedBy = ApplicationContext.,
                CreatedOn = DateTime.Now,
            }
            ;

            var eventAggreggor = IoC.Get<IEventAggregator>();
            var windowManager = IoC.Get<IInvWindowManager>();
            var facilityService = IoC.Get<IFacilitiesService>();
            var applicationContext = IoC.Get<IApplicationContext>();
            var attachmentVm = new AttachmentDetailViewModel(attahcment, applicationContext, eventAggreggor, _windowManager, _facilitiesService);
            var vm = new AttachmentCreateViewModel(attachmentVm, eventAggreggor, windowManager);
            var settings = new Dictionary<string, object>
                               {
                                   {"ResizeMode", ResizeMode.NoResize},
                                   {"WindowStartupLocation", WindowStartupLocation.CenterScreen}
                               };
            if (windowManager.ShowDialog(vm, null, settings) ?? false)
                return attachmentVm;
            else
                return null;
        }


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
