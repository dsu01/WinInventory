using System.Windows;
using System.Windows.Input;
using AvalonDock;
using Caliburn.Micro;

namespace Client.Framework.Docking
{
    public class BaseDockableContent : DockableContent
    {
        public BaseDockableContent()
        {
        }

        public BaseDockableContent(object rootModel, object view, DockArea dockArea, bool isDocument = false)
        {
            var dockableContent = view as BaseDockableContent;

            if (dockableContent != null)
            {
                Content = view;
                IsCloseable = dockableContent.IsCloseable;
                HideOnClose = dockableContent.HideOnClose;
                Model = dockableContent.Model;
                DockArea = dockableContent.DockArea;
                DockableStyle = DockableStyle.Dockable;
                //DockableStyle = dockableContent.DockableStyle;
                DataContext = rootModel;
            }
            else
            {
                var fe = view as FrameworkElement;
                if ((fe != null) && (fe.Parent != null))
                {
                    var parent = fe.Parent as BaseDockableContent;
                    if (parent != null)
                        parent.Content = null;
                }
                Content = view;
                IsCloseable = true;
                HideOnClose = false;
                Model = rootModel;
                DockArea = dockArea;
                DockableStyle = DockableStyle.Dockable;
                //DockableStyle = isDocument ? DockableStyle.Document : DockableStyle.Dockable;
                DataContext = rootModel;

                //dockableContent.IsCloseable = viewModel.CanClose;
                //Image img = new Image();
                //img.Source = new BitmapImage(new Uri(@"Resources/Data.png", UriKind.Relative));
                //dockableContent.Icon = img.Source;
            }
        }

        private object _model;

        public object Model
        {
            get { return _model; }
            set
            {
                if (_model == value) return;
                _model = value;
            }
        }

        public ViewModelBase ViewModel
        {
            get { return Model as ViewModelBase; }
            set { Model = value; }
        }

        public DockArea DockArea { get; set; }

        public override bool Close()
        {
            //if(State != DockableContentState.Document)
            //HideOnClose = true;

            var dockwindowmanager = IoC.Get<IDockWindowManager>();
            dockwindowmanager.CheckAreas(null);

            ViewModel.Dispose();
            var close = base.Close();

            return close;
        }

        protected override void OnMouseMove(MouseEventArgs mouseEventArgs)
        {
            //Debug.WriteLine(string.Format("{0}: Mouse Move", DateTime.Now.ToString("hh:mm:ss.fff")));
            //WindowTimer.ResetTimeoutTimer();
        }

        protected override void OnKeyDown(KeyEventArgs keyEventArgs)
        {
            //Debug.WriteLine(string.Format("{0}: Key Down", DateTime.Now.ToString("hh:mm:ss.fff")));
            //WindowTimer.ResetTimeoutTimer();
        }
    }
}