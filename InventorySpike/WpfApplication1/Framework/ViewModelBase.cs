using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using ILog = log4net.ILog;
using LogManager = log4net.LogManager;

namespace Client.Framework
{
    public abstract class ViewModelBase : PropertyChangedBase, INotifyPropertyChangedEx, INotifyPropertyChanged, IDisposable
    {
        private static ILog logger = LogManager.GetLogger(typeof(ViewModelBase));

        private bool _subscribed = false;

        protected ViewModelBase()
        {
        }

        protected ViewModelBase(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
        }

        #region Properties and Fields

        protected virtual IEventAggregator EventAggregator { get; set; }

        #endregion

        protected void SubscribeToEvents()
        {
            if (!_subscribed)
            {
                EventAggregator.Subscribe(this);
                _subscribed = true;
            }
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (EventAggregator != null)
            {
                if (_subscribed && EventAggregator != null)
                {
                    EventAggregator.Unsubscribe(this);
                }
            }
        }

        #endregion
    }
}