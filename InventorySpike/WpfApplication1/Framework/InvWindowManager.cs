using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Caliburn.Micro;
using Microsoft.Win32;
using WpfApplication1;

namespace Client.Framework
{
    public class InvWindowManager : WindowManager, IInvWindowManager
    {
        private IDictionary<string, WeakReference> windows = new Dictionary<string, WeakReference>();

        private System.Action _closedHandler;

        #region IInvWindowManager

        public virtual void ShowOrActivateWindow(object rootModel, object context = null,
            IDictionary<string, object> settings = null, bool showDialogue = false)
        {
            NavigationWindow navWindow = null;

            if (Application.Current != null && Application.Current.MainWindow != null)
            {
                navWindow = Application.Current.MainWindow as NavigationWindow;
            }

            if (navWindow != null)
            {
                var window = CreatePage(rootModel, context, settings);
                navWindow.Navigate(window);
            }
            else
            {
                var window = GetExistingWindow(rootModel);
                if (window == null)
                {
                    window = CreateWindow(rootModel, false, context, settings);
                    window.Owner = Application.Current.MainWindow;
                    windows.Add(rootModel.GetType().Name, new WeakReference(window));
                    window.Closed += Closed;
                    if (!showDialogue)
                        window.Show();
                    else
                        window.ShowDialog();
                }
                else
                {
                    window.BringIntoView();
                    window.Activate();
                    window.Focus();
                }
            }
        }

        public void TryCloseWindow(object rootModel, System.Action closedHandler = null)
        {
            var window = GetExistingWindow(rootModel);

            // not null means active
            if (window != null)
            {
                if (closedHandler != null)
                {
                    _closedHandler = closedHandler;
                    window.Closed += (s, e) =>
                    {
                        _closedHandler();
                    };
                }
                window.Close();

                if (rootModel is IDisposable)
                {
                    (rootModel as IDisposable).Dispose();
                }
            }
        }

        private void Closed(object sender, EventArgs e)
        {
            var view = sender as Window;
            if (view != null)
            {
                view.Closed -= Closed;
                if (_closedHandler != null)
                    view.Closed -= (s, ee) =>
                    {
                        _closedHandler();
                    };

                var savedEntry = windows.FirstOrDefault(d => d.Value.IsAlive && d.Value.Target.Equals(view));
                windows.Remove(savedEntry);
            }
        }

        public virtual bool Confirm(string title, string message, Window rootWindow = null)
        {
            return MessageBox.Show(rootWindow == null ? App.Current.MainWindow : rootWindow, message, title, MessageBoxButton.OKCancel) == MessageBoxResult.OK;
        }

        public virtual bool Confirm(string title, string message, object rootModel)
        {
            var rootWindow = rootModel == null ? null : GetExistingWindow(rootModel);

            return Confirm(title, message, rootWindow);
        }

        public virtual void ShowError(string title, string message, Window rootWindow = null)
        {
            MessageBox.Show(rootWindow == null ? App.Current.MainWindow : rootWindow, message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public virtual void ShowError(string title, string message, object rootModel)
        {
            var rootWindow = rootModel == null ? null : GetExistingWindow(rootModel);

            ShowError(title, message, rootWindow);
        }

        public virtual void Warn(string title, string message, Window rootWindow = null)
        {
            MessageBox.Show(rootWindow == null ? App.Current.MainWindow : rootWindow, message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public virtual void Warn(string title, string message, object rootModel)
        {
            var rootWindow = rootModel == null ? null : GetExistingWindow(rootModel);

            Warn(title, message, rootWindow);
        }

        public virtual void Inform(string title, string message, Window rootWindow = null)
        {
            if (rootWindow == null && App.Current.MainWindow != null && App.Current.MainWindow.OwnedWindows != null && App.Current.MainWindow.OwnedWindows.Count > 0)
                rootWindow = App.Current.MainWindow.OwnedWindows[0];
            MessageBox.Show(rootWindow == null ? App.Current.MainWindow : rootWindow, message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public virtual void Inform(string title, string message, object rootModel)
        {
            var rootWindow = rootModel == null ? null : GetExistingWindow(rootModel);

            Inform(title, message, rootWindow);
        }

        public virtual List<string> PickFilesToOpen(string filter, string title, bool multiSelect)
        {
            var dialog = new OpenFileDialog { Filter = filter, Title = title, Multiselect = multiSelect };

            if (!dialog.ShowDialog().Value)
                return new List<string>();

            return dialog.FileNames.Any() ? dialog.FileNames.ToList() : new List<string>();
        }

        public virtual string PickFilesToSave(string filter, string title)
        {
            var dialog = new SaveFileDialog() { Filter = filter, Title = title };

            if (!dialog.ShowDialog().Value)
                return String.Empty;

            return dialog.FileNames.Any() ? dialog.FileNames.FirstOrDefault() : String.Empty;
        }

        public virtual List<string> GetOpenFileDialogNames(string filter, string title, bool multiSelect)
        {
            var sfd = new OpenFileDialog { Filter = filter, Title = title, Multiselect = multiSelect };

            if (!sfd.ShowDialog().Value)
                return new List<string>();
            return sfd.FileNames.Any() ? sfd.FileNames.ToList() : new List<string>();
        }

        #endregion

        #region private methods

        protected virtual Window GetExistingWindow(object model)
        {
            var existing = windows.FirstOrDefault(d => d.Key == model.GetType().Name);
            return existing.Key == null ? null : existing.Value.IsAlive ? existing.Value.Target as Window : null;
        }

        #endregion
    }
}
