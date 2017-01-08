using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using Action = System.Action;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

namespace Client.ViewModels
{
    [ImplementPropertyChanged]
    public class MainViewModel : ScreenViewModelBase, IShell
    {
        private static ILog logger = LogManager.GetLogger(typeof(MainViewModel));

        // Fields
        private IServiceLocator _serviceLocator;

        private readonly IInvWindowManager _windowManager;
        private readonly Settings Settings;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected MainViewModel()
        {
            Init();
        }

        private void Init()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public MainViewModel(IInvWindowManager windowManager,
                        IServiceLocator serviceLocator,
                        IEventAggregator eventAggregator,
                        Settings settings,
                        IApplicationContext applicationContext,
                        StatusBarViewModel statusBarViewModel,
                        MenuViewModel menuViewModel,
                        DockViewModel dockViewModel)
            : base(eventAggregator)
        {
            Init();

            this._windowManager = windowManager;
            this._serviceLocator = serviceLocator;
            this.Settings = settings;
            this.ApplicationContext = applicationContext;

            StatusBarRegion = statusBarViewModel;
            MenuRegion = menuViewModel;
            DockRegion = dockViewModel;

            this.SubscribeToEvents();
        }

        #endregion

        #region Event Handlers

        public void UnloadEvent(CancelEventArgs args)
        {
            if (!IoC.Get<IInvWindowManager>().Confirm(string.Empty, "Are you sure you want to exit CMMS Inventory?"))
            {
                args.Cancel = true;
                return;
            }

            //var dockingWindowManager = IoC.Get<IDockWindowManager>();
            //if (dockingWindowManager != null && !dockingWindowManager.CloseAllWindows())
            //{
            //    args.Cancel = true;
            //}
        }

        /// <summary>
        /// app clean up
        /// </summary>
        public void OnClosed()
        {
        }

        #endregion

        #region Properties

        public MenuViewModel MenuRegion { get; set; }

        public DockViewModel DockRegion { get; set; }

        public StatusBarViewModel StatusBarRegion { get; set; }

        public IApplicationContext ApplicationContext { get; set; }

        public string Title
        {
            get
            {
                var title = "CMMS Inventory";
                if (ApplicationContext.ActiveUser != null)
                    title = "CMMS Inventory - " + ApplicationContext.ActiveUser;
                return title;
            }
        }

        #endregion
    }
}
