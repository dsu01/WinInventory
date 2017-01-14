using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using AutoMapper;
using Caliburn.Micro;
using Client.Properties;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;
using Caliburn.Micro.Logging.log4net;
using Client.Classes;
using Client.Framework;
using Client.Framework.Docking;
using Client.ViewModels;
using Inventory.Business.Services;

namespace Client
{
    public class AppBootstrapper : BootstrapperBase
    {
        private static ILog logger = LogManager.GetLogger(typeof(AppBootstrapper));

        DispatcherTimer _updateTimer;
        private IEventAggregator _eventAggregator;

        /// <summary>
        /// Gets or sets the IoC container.
        /// </summary>w
        public UnityContainer Container { get; set; }

        /// <summary>
        /// Initializes the <see cref="AppBootstrapper"/> class.
        /// </summary>
        static AppBootstrapper()
        {
            Caliburn.Micro.LogManager.GetLog = type => new log4netLogger(type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Bootstrapper"/> class.
        /// </summary>
        public AppBootstrapper()
        {
            Initialize();
            InitializeSettings();
            InstallExceptionHandlers();
            this._eventAggregator = (IEventAggregator)GetInstance(typeof(IEventAggregator), null);
            _updateTimer = new DispatcherTimer();
            //_updateTimer.Interval = TimeSpan.FromSeconds(Settings.Default.UpdateIntervalSeconds);
            _updateTimer.Tick += UpdateTimer_Tick;
        }

        private void InitializeSettings()
        {
            //if (Settings.Default.UpgradeRequired)
            //{
            //    Settings.Default.Upgrade();
            //    Settings.Default.UpgradeRequired = false;
            //    Settings.Default.Save();
            //}
        }

        /// <summary>
        /// Configure the framework and setup your IoC container.
        /// </summary>
        protected override void Configure()
        {
            this.Container = new UnityContainer();
            var locator = new UnityServiceLocator(Container);
            ServiceLocator.SetLocatorProvider(() => locator);
            ConfigureUnityContainer(locator);
            ConfigureAutoMapper();

            base.Configure();
        }

        #region Caliburn overrides

        /// <summary>
        /// Get instance from IOC container.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        /// <returns>
        /// The located service.
        /// </returns>
        protected override object GetInstance(Type service, string key)
        {
            var result = default(object);
            if (key != null)
            {
                result = Container.Resolve(service, key);
            }
            else
            {
                result = Container.Resolve(service);
            }
            return result;
        }

        /// <summary>
        /// Get all instances from IOC container
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <returns>
        /// The located services.
        /// </returns>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Container.ResolveAll(service);
        }

        /// <summary>
        /// Buildup instance from IOC container.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            instance = Container.BuildUp(instance);
            base.BuildUp(instance);
        }

        /// <summary>
        /// Called when the application starts up.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="StartupEventArgs"/> instance containing the event data.</param>
        protected async override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);

            //SplashScreen splash = new SplashScreen(@"Images\Splash.png");
            //splash.Show(autoClose: false, topMost: true);
            //splash.Close(TimeSpan.FromSeconds(1));

            DisplayRootViewFor<IShell>();

            var applicationContext = IoC.Get<IApplicationContext>();
            if ((applicationContext == null) || (applicationContext.ActiveUser == null))
            {
                ShutdownApp("App cannot start due to an internal error.");
            }

            await LoadLookupData();

            var facilitiesVM = IoC.Get<FacilitiesViewModel>();
            if (facilitiesVM != null)
            {
                var manager = IoC.Get<IDockWindowManager>();
                manager.ShowDockedWindow(facilitiesVM, DockArea.Master, null, true, DockSide.Left, 400, true, false);
            }

            //FindServerAsync().ContinueWith(t =>
            //{
            //    if (t.IsFaulted)
            //    {
            //        var windowManager = Container.Resolve<ISeaStarWindowManager>();
            //        splash.Close(TimeSpan.Zero);
            //        windowManager.ShowError(AppBootstrapperRes.Orbmap, AppBootstrapperRes.StartupErrorMessage);
            //        Application.Current.Shutdown();
            //    }

            //    ConfigureVesselServerEndpoints(t.Result);
            //    Container.Resolve<IVehicleClientService>();   // Init service GPS Listener
            //    if (_eventAggregator != null)
            //    {
            //        _eventAggregator.PublishOnUIThread(new ServiceInitializationCompleteMessage());
            //    }

            //    _updateTimer.Start();
            //    splash.Close(TimeSpan.FromSeconds(2));
            //}, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async Task LoadLookupData()
        {
            var applicationContext = IoC.Get<IApplicationContext>();

            var facilityService = GetInstance(typeof(IFacilitiesService), null);
            if (facilityService == null)
                return;

            applicationContext.Buildings = (facilityService as IFacilitiesService).GetBuildings();
        }

        #endregion

        #region Other Methods

        private void InstallExceptionHandlers()
        {
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// Finds the server.
        /// </summary>
        //private Task<Uri> FindServerAsync()
        //{
        //    EndpointAddress address = Container.Resolve<EndpointAddress>("SeaStarService");
        //    logger.InfoFormat("Attempting connection to server in settings file: {0}", address.Uri);
        //    return ServiceHelper<ISeaStarService>.InvokeServiceMethodAsync<bool>((s) => s.Ping()).ContinueWith<Uri>(t =>
        //    {
        //        if (t.IsFaulted)
        //        {
        //            logger.Error(t.Exception);
        //        }
        //        else if (t.Result == true)
        //        {
        //            return new Uri(address.Uri, "/");
        //        }

        //        logger.InfoFormat("Failed to connect to server.");

        //        int attempt = 0;
        //        int maxAttempts = 5;
        //        DiscoveryClient discoveryClient = new DiscoveryClient(new UdpDiscoveryEndpoint());
        //        FindCriteria criteria = new FindCriteria(typeof(ISeaStarService));
        //        criteria.Duration = new TimeSpan(0, 0, 1);
        //        List<string> localAddresses = new List<string>()
        //            {
        //                //"localhost",
        //                //"127.0.0.1",
        //                //"::1",
        //                Environment.MachineName.ToLower()
        //            };
        //        while (attempt++ < maxAttempts)
        //        {
        //            logger.Info("Attempting to discover server with WS Discovery.");
        //            FindResponse findResponse = discoveryClient.Find(criteria);

        //            // prefer local addresses
        //            EndpointDiscoveryMetadata endpoint
        //                = findResponse.Endpoints.FirstOrDefault(e => localAddresses.Contains(e.Address.Uri.Host.ToLower()));

        //            //#if !DEBUG
        //            //                    // if not in debug mode and a local service has not been found, look on the rest of the network
        //            //                    if (endpoint == null)
        //            //                    {
        //            //                        endpoint = findResponse.Endpoints.FirstOrDefault();
        //            //                    }
        //            //#endif
        //            if (endpoint != null)
        //            {
        //                logger.InfoFormat("Found server: {0}", endpoint.Address.Uri);
        //                return new Uri(endpoint.Address.Uri, "/");
        //            }
        //        }

        //        throw new Exception("Server not found");
        //    });
        //}

        //private void ConfigureVesselServerEndpoints(Uri serverUri)
        //{
        //    Settings settings = Settings.Default;
        //    settings.VesselServer = serverUri.Host;
        //    settings.VesselServerPort = serverUri.Port;
        //    settings.Save();

        //    this.Container.RegisterInstance("SeaStarService", new EndpointAddress(new Uri(serverUri, "/seastar")));
        //    this.Container.RegisterInstance("TrackingService", new EndpointAddress(new Uri(serverUri, "/tracking")));
        //    this.Container.RegisterInstance("WeatherService", new EndpointAddress(new Uri(serverUri, "/weather")));
        //    this.Container.RegisterInstance("FrbService", new EndpointAddress(new Uri(serverUri, "/frb")));
        //    this.Container.RegisterInstance("FabService", new EndpointAddress(new Uri(serverUri, "/fab")));
        //    this.Container.RegisterInstance("OrderService", new EndpointAddress(new Uri(serverUri, "/order")));
        //    this.Container.RegisterInstance("FfmService", new EndpointAddress(new Uri(serverUri, "/ffm")));
        //    this.Container.RegisterInstance("FishCatchService", new EndpointAddress(new Uri(serverUri, "/fishcatch")));
        //    this.Container.RegisterInstance("MastercastService", new EndpointAddress(new Uri(serverUri, "/mastercast")));
        //    this.Container.RegisterInstance("NavigationService", new EndpointAddress(new Uri(serverUri, "/navigation")));
        //    this.Container.RegisterInstance("UserEmailService", new EndpointAddress(new Uri(serverUri, "/userEmail")));
        //}

        private void ConfigureUnityContainer(IServiceLocator serviceLocator)
        {
            this.Container.RegisterInstance(Settings.Default);  // Singleton
            this.Container.RegisterInstance(serviceLocator); // Singleton

            // Must initially setup localhost endpoints to avoid unity stack overflow exception
            Settings settings = Settings.Default;
            //Uri netTcpUriBase = new Uri(String.Format("net.tcp://{0}:{1}/", settings.VesselServer, settings.VesselServerPort));
            //ConfigureVesselServerEndpoints(netTcpUriBase);

            // Services
            this.Container.RegisterType<IFacilitiesService, FacilitiesService>();

            // UI services
            this.Container.RegisterType<IApplicationContext, ApplicationContext>(new ContainerControlledLifetimeManager());

            // ViewModels
            this.Container.RegisterType<IShell, MainViewModel>();
            this.Container.RegisterType<StatusBarViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<MenuViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<DockViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<StatusBarViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<FacilitiesViewModel>(new ContainerControlledLifetimeManager());

            // Caliburn types
            this.Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IWindowManager, InvWindowManager>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IInvWindowManager, InvWindowManager>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IDockWindowManager, DockWindowManager>(new ContainerControlledLifetimeManager());
        }

        //private void RegisterServiceChannel<TChannel>(string name)
        //{
        //    this.Container.RegisterType<ChannelFactory<TChannel>>(
        //        new ContainerControlledLifetimeManager(), new InjectionConstructor(name));
        //    this.Container.RegisterType<TChannel>(
        //        new InjectionFactory(c => c.Resolve<ChannelFactory<TChannel>>().CreateChannel(c.Resolve<EndpointAddress>(name))));
        //}


        private void ConfigureAutoMapper()
        {
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.ConstructServicesUsing(t => Container.Resolve(t));
            //    cfg.CreateMap<DataSetDto, ProductDataSetViewModel>().ConstructUsingServiceLocator();
            //});
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            //if (_eventAggregator != null)
            //{
            //    _eventAggregator.PublishOnUIThread(new CheckForUpdatesMessage());
            //}
        }

        #endregion

        #region error handlers

        /// <summary>
        /// Handles unhandled dispatcher exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            logger.Error("Unhandled Dispatcher Exception", e.Exception);

            // Display error message does not work when Dispatcher is hosed.  so just going straight to app shutdown
            //DisplayShutdownMessage();
            //MessageBox.Show(AppBootstrapperRes.ShutdownMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            // TODO - take this out to avoid unnecessary shutdown
            //System.Windows.Application.Current.Shutdown();

            e.Handled = true;
        }

        /// <summary>
        /// Handles the UnobservedTaskException event of the TaskScheduler.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnobservedTaskExceptionEventArgs"/> instance containing the event data.</param>
        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            logger.Error("Unobserved Task Exception" + e.Exception.Flatten().ToString());

            // do not put in any additional logic in here as they may compound the exception
            e.SetObserved();
        }

        /// <summary>
        /// Handles the UnhandledException event of the AppDomain.CurrentDomain.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="UnhandledExceptionEventArgs"/> instance containing the event data.</param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
            {
                logger.Fatal("Unhandled Fatal Exception", e.ExceptionObject as Exception);
            }
            else
            {
                logger.Fatal("Unhandled Non-Terminal Exception", e.ExceptionObject as Exception);
            }

            DisplayShutdownMessage();

            Application.Current.Shutdown();
        }

        private void HandledException(Exception e)
        {
            //var eventAggregator = GetInstance(typeof(IEventAggregator), null) as IEventAggregator;
            //if (eventAggregator != null)
            //    eventAggregator.PublishOnUIThread(new UserErrorMessage(e, "Exception"));
        }

        private void DisplayShutdownMessage()
        {
            Execute.OnUIThread(() => MessageBox.Show("Shutting down app now...", "Error", MessageBoxButton.OK, MessageBoxImage.Error));
        }

        private void ShutdownApp(string message)
        {
            Execute.OnUIThread(() =>
            {
                MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            });
        }

        #endregion
    }
}
