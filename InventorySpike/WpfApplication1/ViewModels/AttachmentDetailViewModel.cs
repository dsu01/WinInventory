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
    public class AttachmentDetailViewModel : EntityViewModel<InvFacilityAttachment>
    {
        private static ILog logger = LogManager.GetLogger(typeof(AttachmentDetailViewModel));

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
                        IEventAggregator eventAggregator)
            : base(facilityAttachment, eventAggregator)
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

        #endregion

        #region Public Methods

        public void PickFile()
        {
            
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
