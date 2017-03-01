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
            IsFacilitiesSelected = true;
        }

        #endregion

        #region Properties

        public bool IsFacilitiesSelected { get; set; }

        public bool IsSearchSelected { get; set; }

        public bool IsElectricalSystemsSelected { get; set; }

        public BindableCollection<TreeNode<object>> ElectricalSystems { get; set; }

        public BindableCollection<TreeNode<object>> Facilities { get; set; }

        #endregion

        #region Public Methods

        public void OpenFacility(object context, EventArgs eventArgs)
        {
            var treeNode = context as TreeNode<object>;
            if (treeNode == null) return;

            if (treeNode.TreeNodeType == TreeNodeType.System)
            {
                CursorHelper.ExecuteWithWaitCursor(() =>
                {
                    //reload
                    var facility = treeNode.Value as InvFacility;
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
            }
            else if (treeNode.TreeNodeType == TreeNodeType.Component)
            {
                CursorHelper.ExecuteWithWaitCursor(() =>
                {
                    //reload
                    var equipment = treeNode.Value as InvEquipment;
                    var facility = equipment.InvFacility;
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

                    // select proper component
                    EventAggregator.PublishOnUIThread(new EquipmentSelectedMessage() { Equipment = equipment });
                });
            }

            if (eventArgs is RoutedEventArgs)
                ((RoutedEventArgs)eventArgs).Handled = true;
        }

        public void Refresh()
        {
            if (IsFacilitiesSelected)
            {
                LoadFacilities();
            }
            else if (IsElectricalSystemsSelected)
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

        private void OnIsFacilitiesSelectedChanged()
        {
            if (IsFacilitiesSelected)
            {
                LoadFacilities();
            }
        }

        private void LoadFacilities()
        {
            CursorHelper.ExecuteWithWaitCursor(LoadFacilitiesInternal);
        }

        private void LoadFacilitiesInternal()
        {
            var facilities = _facilitiesService.GetFacilities();
            var buildings = _applicationContext.Buildings.ToList();
            var sites = buildings.Where(x => x.Property != null).Select(x => x.Property).Distinct().OrderBy(x => x).ToList();
            var facilitySystems = _applicationContext.InvFacilitySystems;
            var systemGroups = facilitySystems.Select(x => x.SystemGroup).Distinct().OrderBy(x => x).ToList();

            var items = new BindableCollection<TreeNode<object>>();

            foreach (var site in sites)
            {
                var siteNode = new TreeNode<object>
                {
                    HiearchyLevel = 0,
                    Value = site,
                    TreeNodeType = TreeNodeType.Site,
                    Parent = null,
                    IsOpen = true,
                    Children = new BindableCollection<ITreeNode<object>>(),
                };

                foreach (var building in buildings.Where(x => x.Property == site).Select(x => x.Building).Distinct().OrderBy(x => x))
                {
                    var buildingNode = new TreeNode<object>
                    {
                        HiearchyLevel = 1,
                        Value = building,
                        TreeNodeType = TreeNodeType.Building,
                        Parent = siteNode,
                        IsOpen = true,
                        Children = new BindableCollection<ITreeNode<object>>(),
                    };
                    siteNode.Children.Add(buildingNode);

                    foreach (var systemGroup in systemGroups)
                    {
                        var systemGroupNode = new TreeNode<object>
                        {
                            HiearchyLevel = 2,
                            Value = systemGroup,
                            TreeNodeType = TreeNodeType.SystemGroup,
                            Parent = buildingNode,
                            IsOpen = true,
                            Children = new BindableCollection<ITreeNode<object>>(),
                        };
                        buildingNode.Children.Add(systemGroupNode);

                        foreach (
                            var systemGroupType in
                            facilitySystems.Where(x => x.SystemGroup == systemGroup)
                                .Select(x => x.SystemTitle)
                                .Distinct()
                                .OrderBy(x => x))
                        {
                            var systemGroupTypeNode = new TreeNode<object>
                            {
                                HiearchyLevel = 3,
                                Value = systemGroupType,
                                TreeNodeType = TreeNodeType.SystemGroupType,
                                Parent = systemGroupNode,
                                IsOpen = true,
                                Children = new BindableCollection<ITreeNode<object>>(),
                            };
                            systemGroupNode.Children.Add(systemGroupTypeNode);

                            foreach (var facility in facilities.Where(x => x.Property == site && x.Building == building && x.FacilityGroup == systemGroup && x.FacilitySystem == systemGroupType).OrderBy(x => x.Facility_))
                            {
                                var facilityNode = new TreeNode<object>
                                {
                                    HiearchyLevel = 4,
                                    Value = facility,
                                    TreeNodeType = TreeNodeType.System,
                                    Parent = systemGroupTypeNode,
                                    IsOpen = true,
                                    Children = new BindableCollection<ITreeNode<object>>(),
                                };
                                systemGroupTypeNode.Children.Add(facilityNode);

                                foreach (var component in facility.InvEquipments.OrderBy(x => x.EquipmentName))
                                {
                                    var equipmentNode = new TreeNode<object>
                                    {
                                        HiearchyLevel = 5,
                                        Value = component,
                                        TreeNodeType = TreeNodeType.Component,
                                        Parent = facilityNode,
                                        IsOpen = true,
                                    };
                                    facilityNode.Children.Add(equipmentNode);
                                }
                            }
                        }
                    }
                }
                items.Add(siteNode);
            }

            Facilities = items;
        }

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
            var list = _facilitiesService.GetElectricalSystems();

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
                foreach (var invEquipment in invFacility.InvEquipments.OrderBy(x => x.EquipmentName))
                {
                    var equipmentNode = new TreeNode<object>
                    {
                        HiearchyLevel = 1,
                        Value = invEquipment,
                        TreeNodeType = TreeNodeType.Component,
                        Parent = facilitySystemNode,
                        IsOpen = true,
                    };
                    facilitySystemNode.Children.Add(equipmentNode);
                }

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
                if (message.Facility.FacilityGroup != "Electrical System") return;

                if ((message.FacilityUpdateType == FacilityUpdateType.Create ||
                     message.FacilityUpdateType == FacilityUpdateType.Delete))
                {
                    LoadElectricalSystems();
                }
                else if (message.FacilityUpdateType == FacilityUpdateType.Updated)
                {
                    LoadElectricalSystems();
                }
            }
        }

        #endregion
    }
}
