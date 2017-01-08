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
using Microsoft.Practices.ServiceLocation;
using PropertyChanged;
using Action = System.Action;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

namespace Client.ViewModels
{
    [ImplementPropertyChanged]
    public class StatusBarViewModel : ScreenViewModelBase, IShell
    {
        private static ILog logger = LogManager.GetLogger(typeof(StatusBarViewModel));

        // Fields

        public string Title { get; set; }
        private readonly IInvWindowManager _windowManager;
        private readonly Settings Settings;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected StatusBarViewModel()
        {
            Init();
        }

        private void Init()
        {
        }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public StatusBarViewModel(IInvWindowManager windowManager,
                        IEventAggregator eventAggregator,
                        Settings settings)
            : base(eventAggregator)
        {
            Init();

            this._windowManager = windowManager;
            this.Settings = settings;

            this.SubscribeToEvents();
        }

        #endregion
    }
}
