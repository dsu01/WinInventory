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
    public class FacilityAttachmentViewModel : EntityViewModel<InvFacilityAttachment>
    {
        private static ILog logger = LogManager.GetLogger(typeof(FacilityAttachmentViewModel));

        // Fields
        private readonly IApplicationContext _applicationContext;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        protected FacilityAttachmentViewModel()
        {
            Init();
        }

        public FacilityAttachmentViewModel(InvFacilityAttachment attachment,
                        IApplicationContext applicationContext,
                        IEventAggregator eventAggregator
            )
            : base(attachment, eventAggregator)
        {
            _applicationContext = applicationContext;
            DisplayName = attachment.C__ID.ToString();

            this.SubscribeToEvents();

            Init();
        }

        private void Init()
        {
        }

        #endregion

        #region Properties


        #endregion

        #region Public Methods


        #endregion

        #region Private Methods

        #endregion
    }
}
