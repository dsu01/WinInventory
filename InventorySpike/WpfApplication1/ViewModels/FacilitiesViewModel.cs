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
using Inventory.Business.Services;
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using Action = System.Action;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

namespace Client.ViewModels
{
    [ImplementPropertyChanged]
    public class FacilitiesViewModel : ScreenViewModelBase, IShell
    {
        private static ILog logger = LogManager.GetLogger(typeof(MenuViewModel));

        // Fields

        private readonly IInvWindowManager _windowManager;
        private readonly Settings Settings;
        private readonly IFacilitiesService _facilitiesService;

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
                        IFacilitiesService facilitiesService,
                        Settings settings)
            : base(eventAggregator)
        {
            _windowManager = windowManager;
            Settings = settings;
            _facilitiesService = facilitiesService;

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
    }
}
