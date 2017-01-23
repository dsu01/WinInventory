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
using Client.ActionItems;
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
    public class MenuViewModel : ScreenViewModelBase, IShell
    {
        private static ILog logger = LogManager.GetLogger(typeof(MenuViewModel));

        // Fields

        public string Title { get; set; }
        private readonly IInvWindowManager _windowManager;
        private readonly Settings Settings;

        #region Constructors

        protected MenuViewModel()
        {
            Init();
        }

        public MenuViewModel(IInvWindowManager windowManager,
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

        #region Properties

        public ObservableCollection<IActionItem> Items { get; private set; }

        #endregion

        private void Init()
        {
            Items = new ObservableCollection<IActionItem>()
            {
                new CreateActionItem(),
                new AdminActionItem(),
                new SyncActionItem(),
            };
        }
    }
}
