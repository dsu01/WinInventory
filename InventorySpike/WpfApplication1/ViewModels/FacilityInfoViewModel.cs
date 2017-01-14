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
    public class FacilityInfoViewModel : EntityViewModel<InvFacility>
    {
        private static ILog logger = LogManager.GetLogger(typeof(FacilityInfoViewModel));

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected FacilityInfoViewModel()
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the MainWindowViewModel class.
        /// </summary>
        public FacilityInfoViewModel(InvFacility facility,
                        IApplicationContext applicationContext,
                        IEventAggregator eventAggregator)
            : base(facility, eventAggregator)
        {
            ApplicationContext = applicationContext;
            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
        }

        #endregion

        #region Properties

        public IApplicationContext ApplicationContext { get; private set; }

        public IEnumerable<string> FacilitySystems
        {
            get
            {
                switch (this.Model.FacilityGroup)
                {
                    case "Electrical System":
                        yield return "Emergency Light";
                        yield return "Test Elec Sys Sys";
                        break;
                    case "Mechanical System":
                        yield return "Air Handling Unit";
                        yield return "Test Mechanical Sys Sys";
                        break;
                    case "Electrical Equipment":
                        yield return "Control Panel";
                        yield return "Test Elec Sys Equip";
                        break;
                    case "Mechanical Equipment":
                        yield return "Air Dryer";
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Public Methods


        #endregion

        #region Private Methods

        #endregion
    }
}
