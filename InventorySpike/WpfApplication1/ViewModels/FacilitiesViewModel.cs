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
using Client.Framework.Docking;
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
    public class FacilitiesViewModel : ScreenViewModelBase, IShell, IHandle<FacilityUpdatedMessage>
    {
        private static ILog logger = LogManager.GetLogger(typeof(MenuViewModel));

        // Fields

        private readonly IInvWindowManager _windowManager;
        private readonly Settings Settings;
        private readonly IFacilitiesService _facilitiesService;
        private readonly IApplicationContext _applicationContext;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected FacilitiesViewModel()
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public FacilitiesViewModel(IInvWindowManager windowManager,
                        IEventAggregator eventAggregator,
                        IApplicationContext applicationContext,
                        IFacilitiesService facilitiesService,
                        Settings settings)
            : base(eventAggregator)
        {
            _windowManager = windowManager;
            Settings = settings;
            _facilitiesService = facilitiesService;
            _applicationContext = applicationContext;

            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
            IsElectricalSystemsSelected = true;
        }

        #endregion

        #region Properties

        public bool IsElectricalSystemsSelected { get; set; }

        public BindableCollection<TreeNode<object>> ElectricalSystems { get; set; }

        #endregion

        #region Public Methods

        public void OpenFacility(object context, EventArgs eventArgs)
        {
            var treeNode = context as TreeNode<object>;
            if (treeNode == null) return;

            var facility = treeNode.Value as InvFacility;
            if (facility == null) return;

            CursorHelper.ExecuteWithWaitCursor(() =>
            {
                //reload
                facility = _facilitiesService.GetFacility(facility.SYNC_ID);
                if (facility == null)
                {
                    _windowManager.ShowError("Open Facility", "cannot load facility");
                    return;
                }

                var facilityInfoVm = new FacilityInfoViewModel(facility, _applicationContext, this.EventAggregator);
                var facilityDetailVM = new FacilityDetailViewModel(facility, facilityInfoVm, _windowManager,
                    EventAggregator, _applicationContext, _facilitiesService);
                var manager = IoC.Get<IDockWindowManager>();
                manager.ShowDocumentWindow(facilityDetailVM, null);
            });

            if (eventArgs is RoutedEventArgs)
                ((RoutedEventArgs)eventArgs).Handled = false;
        }

        public void Refresh()
        {
            if (IsElectricalSystemsSelected)
            {
                LoadElectricalSystems();
            }
        }

        public void SelectFacility(TreeNode<object> treeNode, EventArgs eventArgs)
        {
            if (treeNode == null)
                return;

            // are we in root
            if (treeNode.Value is InvFacility)
            {
                //var institutionSelected = treeNode.Value == null
                //                              ? null
                //                              : _institutionService.GetInstitutionById((treeNode.Value as InstitutionTreeItem).InstitutionId);
                //_applicationContext.SelectedInstitution = institutionSelected;
                //SelectedTreeItem = treeNode;
                //SelectedFormerInstitutionStatusValues = null;
            }

            if (eventArgs is RoutedEventArgs)
                ((RoutedEventArgs)eventArgs).Handled = true;
        }

        #endregion

        #region Private Methods

        private void OnIsElectricalSystemsSelectedChanged()
        {
            if (IsElectricalSystemsSelected)
            {
                LoadElectricalSystems();
            }
        }

        private void LoadElectricalSystems()
        {
            CursorHelper.ExecuteWithWaitCursor(LoadElectricalSystemsInternal);
        }

        private void LoadElectricalSystemsInternal()
        {
            var list = _facilitiesService.GetFacilities();

            var items = new BindableCollection<TreeNode<object>>();

            foreach (var invFacility in list)
            {
                var facilitySystemNode = new TreeNode<object>
                {
                    HiearchyLevel = 0,
                    Value = invFacility,
                    TreeNodeType = TreeNodeType.System,
                    Parent = null,
                    IsOpen = false,
                    Children = new BindableCollection<ITreeNode<object>>(),
                };

                items.Add(facilitySystemNode);
            }

            ElectricalSystems = items;
        }

        #endregion

        #region Message Handlers

        public void Handle(FacilityUpdatedMessage message)
        {
            if (IsElectricalSystemsSelected)
            {
                if ((message.FacilityUpdateType == FacilityUpdateType.Create || message.FacilityUpdateType == FacilityUpdateType.Delete)
                    && message.Facility.FacilityGroup == "Electrical System")
                    LoadElectricalSystems();
            }
        }

        #endregion
    }
}
