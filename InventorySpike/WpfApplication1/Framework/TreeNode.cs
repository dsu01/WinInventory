using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using PropertyChanged;

namespace Client.Framework
{
    public enum TreeNodeType
    {
        System = 0,
        Equipment = 1,
        Component = 2,
    }

    public interface ITreeNode
    {
        INotifyCollectionChanged Children { get; set; }
    }

    //Bindable TreeNode class
    public interface ITreeNode<T>
    {
        int HiearchyLevel { get; set; }

        T Value { get; set; }

        BindableCollection<ITreeNode<T>> Children { get; set; }

        ITreeNode<T> Parent { get; set; }

        ITreeNode<T> Find(ITreeNode<T> node);

        //bool IsPrimaryAi { get; set; }
    }

    public class TreeNode<T> : ViewModelBase, ITreeNode<T>, ITreeNode where T : class
    {
        public TreeNode()
        {
            Children = new BindableCollection<ITreeNode<T>>();
        }

        //protected override void RegisterDirtyCheckProperties()
        //{
        //    RegisterDirtyCheckEntity(() => Value);
        //    RegisterDirtyCheckEntity(() => Children);
        //}

        public int HiearchyLevel { get; set; }

        private T _value;

        [DoNotNotify]
        public T Value
        {
            get { return _value; }
            set
            {
                if (_value != null && _value.Equals(value)) return;
                _value = value;
                NotifyOfPropertyChange(() => Value);
                NotifyOfPropertyChange(() => TreeNodeType);
            }
        }

        public bool IsOpen { get; set; }

        public bool IsSelected { get; set; }

        public TreeNodeType TreeNodeType { get; set; }

        public BindableCollection<ITreeNode<T>> Children { get; set; }

        public ITreeNode<T> Parent { get; set; }

        public ITreeNode<T> Find(ITreeNode<T> node)
        {
            if (node.Value.Equals(Value))
            {
                return this;
            }
            return Children.Count == 0 ? null : Children.Select(child => child.Find(node)).FirstOrDefault(result => result != null);
        }

        //public bool IsPrimaryAi { get; set; }

        public override string ToString()
        {
            return Value.ToString();
        }

        INotifyCollectionChanged ITreeNode.Children
        {
            get
            {
                return Children;
            }
            set
            {
                Children = (BindableCollection<ITreeNode<T>>)value;
            }
        }
    }
}