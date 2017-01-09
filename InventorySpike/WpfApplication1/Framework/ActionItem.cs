using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using PropertyChanged;

namespace Client.Framework
{
    public interface IActionItem : IHaveDisplayName
    {
        /// <summary>
        /// Gets or sets the name used internally for grouping and finding the VM. If not set explicitly is the same as <see cref="DisplayName"/>
        /// </summary>
        string Name { get; set; }

        string DisplayNameShort { get; set; }

        string ToolTip { get; set; }

        bool CanExecute { get; }

        void Execute();
    }

    [ImplementPropertyChanged]
    public class ActionItem : IActionItem
    {
        protected ActionItem(string displayName)
        {
            DisplayName = displayName;
            DisplayNameShort = displayName;
            Name = displayName;
            _execute = (() => { });
        }

        public ActionItem(string displayName, System.Action execute)
            : this(displayName)
        {
            _execute = execute ?? (() => { });
        }

        public string Name { get; set; }
        
        public string DisplayName { get; set; }

        public string DisplayNameShort { get; set; }
        
        public string ToolTip { get; set; }

        private readonly ObservableCollection<IActionItem> _items = new ObservableCollection<IActionItem>();

        public ObservableCollection<IActionItem> Items
        {
            get { return _items; }
        }

        #region Execution

        private readonly System.Action _execute;

        /// <summary>
        /// The action associated to the ActionItem
        /// </summary>
        public virtual void Execute()
        {
            _execute();
        }

        public virtual bool CanExecute
        {
            get { return true; }
        }

        #endregion Execution

        public bool IsItemOpen { get; set; }
        
        public bool IsActive
        {
            get { return true; }
        }
    }
}