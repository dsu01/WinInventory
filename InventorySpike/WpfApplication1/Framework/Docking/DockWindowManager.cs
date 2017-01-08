using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using AvalonDock;
using Caliburn.Micro;
using Client.Classes;
using Client.Framework.Docking.Commands;
using Application = System.Windows.Application;

namespace Client.Framework.Docking
{
    public class DockWindowManager : WindowManager, IDockWindowManager
    {
        private DockingManager _masterDockingManager;
        private DockArea _documentArea;
        private readonly List<DockArea> _openMenuItems;

        private const string MyLayoutFile = @".\AvalonDock.config";

        //MEF constructor
        protected DockWindowManager()
        {
            _documentArea = DockArea.Master;
            _openMenuItems = new List<DockArea>();
        }

        private void CloseAllDocumentPaneExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var dockingManager = sender as DockingManager;
            if (dockingManager != null)
            {
                var documentPane = dockingManager.MainDocumentPane;
                var dirtyViewModels =
                    documentPane.Items.OfType<BaseDockableContent>().Where(x => x.ViewModel.IsDirty()).ToList();
                if (ConfirmDirtyViewModelClose(dirtyViewModels))
                {
                    // DS
                    //DocumentPaneCommands.CloseAll.Execute(null, executedRoutedEventArgs.Parameter as IInputElement ?? executedRoutedEventArgs.Source as IInputElement);
                    DocumentPaneCommands.CloseAllButThis.Execute(null, executedRoutedEventArgs.Parameter as IInputElement ?? executedRoutedEventArgs.Source as IInputElement);
                    DocumentPaneCommands.CloseThis.Execute(null, executedRoutedEventArgs.Parameter as IInputElement ?? executedRoutedEventArgs.Source as IInputElement);
                }
            }
            CheckAreas(dockingManager);
        }

        private void CloseThisDocumentPaneExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var dockingManager = sender as DockingManager;
            if (dockingManager != null)
            {
                var activeDocument = dockingManager.ActiveDocument as BaseDockableContent;
                if (activeDocument != null)
                {
                    var dirtyViewModels =
                        new List<BaseDockableContent> { activeDocument };
                    if (!activeDocument.ViewModel.IsDirty() || ConfirmDirtyViewModelClose(dirtyViewModels))
                    {
                        DocumentPaneCommands.CloseThis.Execute(null, executedRoutedEventArgs.Parameter as IInputElement ?? executedRoutedEventArgs.Source as IInputElement);
                    }
                }
            }
            CheckAreas(dockingManager);
        }

        private void CloseAllButThisDocumentPaneExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var dockingManager = sender as DockingManager;
            if (dockingManager != null)
            {
                var documentPane = dockingManager.MainDocumentPane;
                var dirtyViewModels =
                    documentPane.Items.OfType<BaseDockableContent>().Where(x => x != dockingManager.ActiveDocument && x.ViewModel.IsDirty()).ToList();
                if (ConfirmDirtyViewModelClose(dirtyViewModels))
                {
                    DocumentPaneCommands.CloseAllButThis.Execute(null, executedRoutedEventArgs.Parameter as IInputElement ?? executedRoutedEventArgs.Source as IInputElement);
                }
            }
            CheckAreas(dockingManager);
        }

        private static bool ConfirmDirtyViewModelClose(IList<BaseDockableContent> dirtyViewModels, string actionText = "close")
        {
            var windowman = IoC.Get<IInvWindowManager>();
            return !dirtyViewModels.Any()
                   || windowman.Confirm("Unsaved Changes",
                                        string.Format(" You have unsaved changes that will be lost if you click OK. \n Click Cancel and review your unsaved changes on {0}.",
                                        JoinStringsByComma(dirtyViewModels.Select(x => x.Title).ToList())));
        }

        private static string JoinStringsByComma(IList<string> strings)
        {
            if (strings.Count() <= 2)
                return string.Join(" and ", strings);
            return string.Format("{0}, and {1}", string.Join(", ", strings.Take(strings.Count() - 1)), strings.Last());
        }

        private void CloseAllDockablePaneExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var dockablePane = executedRoutedEventArgs.Source as DockablePane ??
                (executedRoutedEventArgs.Source is BaseDockableContent ?
                    ((BaseDockableContent)executedRoutedEventArgs.Source).Parent as DockablePane : null);
            if (dockablePane != null)
            {
                var dirtyViewModels =
                    dockablePane.Items.OfType<BaseDockableContent>().Where(x => x.ViewModel.IsDirty()).ToList();
                if (ConfirmDirtyViewModelClose(dirtyViewModels))
                    DockablePaneCommands.Close.Execute(executedRoutedEventArgs.Parameter, executedRoutedEventArgs.Source as IInputElement);
            }

            CheckAreas(null);
        }

        private void CloseDockablePaneExecuted(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
        {
            var dockablecontent = executedRoutedEventArgs.Source as BaseDockableContent;
            if (dockablecontent != null)
            {
                var dirtyViewModels = new List<BaseDockableContent>();
                if (dockablecontent.ViewModel.IsDirty())
                    dirtyViewModels.Add(dockablecontent);

                if (ConfirmDirtyViewModelClose(dirtyViewModels))
                    CloseDockWindow(dockablecontent.ViewModel);
            }

            CheckAreas(null);
        }

        #region show

        public void ShowDockedWindow(object viewModel, DockArea dockArea, object context = null,
                                     bool selectWhenShown = true, DockSide dockSide = DockSide.Left,
                                     double startupVisibleWidth = -1, bool hideOnClose = false, bool isCloseable = true)
        {
            var dockingManager = GetDockingManager();

            var anchor = GetAnchorStyle(dockSide);

            // TODO
            //if (anchor == AnchorStyle.Left && dockArea != DockArea.Master)
            //{
            //    var istview = IoC.Get<InstitutionsViewModel>();
            //    var instdockable = GetDockable(istview, null, DockArea.Master);

            //    instdockable.Show(dockingManager, anchor);
            //}

            var dockableContent = GetDockable(viewModel, context, dockArea);
            dockableContent.HideOnClose = hideOnClose;
            dockableContent.IsCloseable = isCloseable;
            dockableContent.Show(dockingManager, anchor);

            if (startupVisibleWidth >= 0)
            {
                var width = new GridLength(startupVisibleWidth);
                ResizingPanel.SetResizeWidth(dockableContent.ContainerPane, width);
            }

            if (selectWhenShown)
                dockableContent.Activate();
            //else
            //    dockableContent.ToggleAutoHide();
        }

        public void ShowFloatingWindow(object viewModel, DockArea dockArea, object context = null, bool selectWhenShown = true, double width = 0, double height = 0)
        {
            var dockableContent = GetDockable(viewModel, context, dockArea);
            if (width > 0 && height > 0)
                dockableContent.FloatingWindowSize = new Size(width, height);
            var dockingManager = GetDockingManager();
            dockableContent.ShowAsFloatingWindow(dockingManager, true);
        }

        public void ShowDocumentWindows(List<Tuple<object, DockArea>> windowsToDock)
        {
            var dockingManager = GetDockingManager();

            foreach (var dockArea in windowsToDock.Select(x => x.Item2).Distinct())
            {
                var dockableContents = new List<BaseDockableContent>();
                foreach (var viewModel in windowsToDock.Where(x => x.Item2 == dockArea).Select(x => x.Item1))
                {
                    dockableContents.Add(GetDockable(viewModel, null, dockArea, true));
                }
                ShowWindowsInLocation(dockArea, dockingManager, dockableContents);
            }
        }

        public void ShowDocumentWindow(object viewModel, object context = null, bool selectWhenShown = true, DockArea dockArea = DockArea.Master)
        {
            var dockingManager = GetDockingManager();

            var dockableContent = GetDockable(viewModel, context, dockArea, true);

            ShowWindowInLocation(dockArea, dockingManager, dockableContent);
        }

        private void ShowWindowInLocation(DockArea dockArea, DockingManager dockingManager, BaseDockableContent dockableContent)
        {
            if (dockableContent == null) return;

            //CheckAreas(dockingManager);

            if (_documentArea == DockArea.Master)
                _documentArea = dockArea;

            if (_documentArea == dockArea)
                dockableContent.ShowAsDocument(dockingManager);
            else
                dockableContent.Show(dockingManager, AnchorStyle.Bottom);

            if (!_openMenuItems.Contains(dockArea))
                _openMenuItems.Add(dockArea);

            // TODO
            //var eventAggregator = IoC.Get<IEventAggregator>();
            //eventAggregator.PublishOnUIThread(new OpenMenuItemsChangedEvent(_openMenuItems));
        }

        private void ShowWindowsInLocation(DockArea dockArea, DockingManager dockingManager, IEnumerable<BaseDockableContent> dockableContents)
        {
            if (_documentArea == DockArea.Master)
                _documentArea = dockArea;

            // TODO
            //if (_documentArea == dockArea)
            //    dockingManager.ShowManyAsDocuments(dockableContents);
            //else
            //{
            //    //TODO: handle defer refresh
            //    foreach (var dockableContent in dockableContents)
            //    {
            //        dockableContent.Show(dockingManager, AnchorStyle.Bottom);
            //    }
            //}

            if (!_openMenuItems.Contains(dockArea))
                _openMenuItems.Add(dockArea);

            // TODO
            //var eventAggregator = IoC.Get<IEventAggregator>();
            //eventAggregator.PublishOnUIThread(new OpenMenuItemsChangedEvent(_openMenuItems));
        }

        #endregion show

        public void ClearOpenMenuItems()
        {
            _openMenuItems.Clear();
            _documentArea = DockArea.Master;
        }

        public IEnumerable<ViewModelBase> FindViewModelsByType(object item)
        {
            var docMngr = GetDockingManager();
            if (docMngr == null) return null;
            var res = docMngr.DockableContents.Where(dc => dc is BaseDockableContent && (dc as BaseDockableContent).ViewModel != null && (dc as BaseDockableContent).ViewModel.GetType() == item.GetType());
            return res.Select(z =>
                                  {
                                      var baseDockableContent = z as BaseDockableContent;
                                      return baseDockableContent != null ? baseDockableContent.ViewModel : null;
                                  });
        }

        #region hide and close

        public void CloseDockWindow(ViewModelBase viewModel, bool forceClose = false)
        {
            var dockContent = FindDockableContent(viewModel);

            if (dockContent != null)
            {
                if (forceClose)
                    dockContent.IsCloseable = true;
                //dockContent.ShowAsDocument();
                dockContent.Close(true);
            }
        }

        public void HideDockWindow(ViewModelBase viewModel)
        {
            var dockContent = FindDockableContent(viewModel);

            if (dockContent != null && dockContent.ContainerPane.GetType() != typeof(FloatingDockablePane) &&
                dockContent.State != DockableContentState.AutoHide && dockContent.State != DockableContentState.Document)
            {
                dockContent.ToggleAutoHide();
            }
        }

        #endregion hide and close

        #region external activate

        public void ActivateDockableContent(object vmContent, DockArea dockArea)
        {
            var content = GetDockable(vmContent, null, dockArea);
            if (content != null)
                content.Activate();
        }

        #endregion external activate

        #region layout

        public void SaveLayout()
        {
            if (!File.Exists(MyLayoutFile))
                File.Create(MyLayoutFile).Close();

            var master = GetDockingManager();
            if (master != null)
                master.SaveLayout(MyLayoutFile);
        }

        public void RestoreLayout()
        {
            if (File.Exists(MyLayoutFile))
            {
                var master = GetDockingManager();
                if (master != null)
                    master.RestoreLayout(MyLayoutFile);
            }
        }

        #endregion layout

        #region dockable content processing

        private BaseDockableContent FindDockableContent(ViewModelBase viewModel)
        {
            var docMngr = GetDockingManager();
            if (docMngr == null) return null;
            var res = docMngr.DockableContents.FirstOrDefault(dc => dc is BaseDockableContent
                                                                    && (dc as BaseDockableContent).ViewModel != null
                                                                    && (dc as BaseDockableContent).ViewModel.GetType() == viewModel.GetType());
            var bdcres = res as BaseDockableContent;
            return bdcres;
        }

        private BaseDockableContent GetDockable(object rootModel, object context, DockArea dockArea, bool isDocument = false)
        {
            var docmanager = GetDockingManager();

            var existingView = docmanager.DockableContents.FirstOrDefault(dc => dc.Title == (rootModel as ViewModelBase).DisplayName) as BaseDockableContent;
            if (existingView != null)
                return existingView;

            var view = ViewLocator.LocateForModel(rootModel, null, context);
            ViewModelBinder.Bind(rootModel, view, context);
            var dockableView = new BaseDockableContent(rootModel, view, dockArea, isDocument);

            //set the display name (tab title)
            var haveDisplayName = rootModel as IHaveDisplayName;
            if (haveDisplayName != null && !ConventionManager.HasBinding(dockableView, ManagedContent.TitleProperty))
                dockableView.SetBinding(ManagedContent.TitleProperty, "DisplayName");

            new DockableContentConductor(rootModel, dockableView);
            return dockableView;
        }

        private static AnchorStyle GetAnchorStyle(DockSide side)
        {
            switch (side)
            {
                case DockSide.None:
                    return AnchorStyle.None;

                case DockSide.Left:
                    return AnchorStyle.Left;

                case DockSide.Top:
                    return AnchorStyle.Top;

                case DockSide.Right:
                    return AnchorStyle.Right;

                case DockSide.Bottom:
                    return AnchorStyle.Bottom;

                default:
                    return AnchorStyle.Left;
            }
        }

        #endregion dockable content processing

        #region docking manager stuff

        private DockingManager GetDockingManager(Window window = null)
        {
            var dockingManager = _masterDockingManager;

            if (dockingManager == null)
            {
                Window parentWindow = GetParentWindow(window);
                if (parentWindow != null)
                {
                    dockingManager = CreateFromView(window);
                    dockingManager.CommandBindings.Add(new CommandBinding(CustomDockCommands.CloseAllDocumentPane, CloseAllDocumentPaneExecuted));
                    dockingManager.CommandBindings.Add(new CommandBinding(CustomDockCommands.CloseThisDocumentPane, CloseThisDocumentPaneExecuted));
                    dockingManager.CommandBindings.Add(new CommandBinding(CustomDockCommands.CloseAllButThisDocumentPane, CloseAllButThisDocumentPaneExecuted));
                    dockingManager.CommandBindings.Add(new CommandBinding(CustomDockCommands.CloseAllDockablePane, CloseAllDockablePaneExecuted));
                    dockingManager.CommandBindings.Add(new CommandBinding(CustomDockCommands.CloseDockablePane, CloseDockablePaneExecuted));
                }
            }

            if (dockingManager == null)
                throw new InvalidOperationException("Unable to retrieve a proper dock site ");

            _masterDockingManager = dockingManager;

            return dockingManager;
        }

        public void CheckAreas(DockingManager dockingManager)
        {
            if (dockingManager == null)
            {
                dockingManager = GetDockingManager();
            }
            var newdocumentsarea = dockingManager.MainDocumentPane.HasItems
                                    ? dockingManager.MainDocumentPane.Items.Cast<BaseDockableContent>().First().DockArea
                                    : DockArea.Master;
            var docked =
                dockingManager.DockableContents.Cast<BaseDockableContent>()
                    .Where(d => d.DockArea != DockArea.Master && d.DockArea != _documentArea)
                    .Where(c => c.Title != "Jacket Navigation")
                    .Select(s => s.DockArea)
                    .Distinct()
                    .ToList();

            // nothing has changed
            if (_documentArea == newdocumentsarea && _openMenuItems.Count(m => m != DockArea.Master) == docked.Count) return;

            var prevjacketnav = _openMenuItems.Contains(DockArea.PrepareJackets);
            _documentArea = newdocumentsarea;

            if (_documentArea == DockArea.Master && docked.Any())
            {
                _documentArea = docked.First();

                //var tomove = dockingManager.DockableContents.Cast<BaseDockableContent>()
                //    .Where(d => d.DockArea == _documentArea &&
                //        d.Title != "Jacket Navigation").ToList();

                var tomove = dockingManager.DockableContents.Cast<BaseDockableContent>()
                    .Where(d => d.DockArea == _documentArea)
                    .ToList();

                foreach (var dockableContent in tomove)
                {
                    try
                    {
                        dockableContent.ShowAsDocument(dockingManager);
                    }
                    // see bug 3116
                    catch (InvalidOperationException ex)
                    {
                        if (ex.Message.Contains("Cannot set Visibility to Visible"))
                            continue;
                        throw;
                    }
                }
                docked.Remove(_documentArea);
            }

            _openMenuItems.Clear();
            _openMenuItems.Add(_documentArea);
            _openMenuItems.AddRange(docked);

            // TODO
            //if (prevjacketnav && !_openMenuItems.Contains(DockArea.PrepareJackets))
            //{
            //    var jacketnavview = IoC.Get<JacketNavigationViewModel>();

            //    var dockablecontent = FindDockableContent(jacketnavview);
            //    if (dockablecontent != null)
            //    {
            //        dockablecontent.IsCloseable = true;
            //        dockablecontent.Close();
            //    }

            //    var istview = IoC.Get<InstitutionsViewModel>();
            //    HideDockWindow(istview);
            //}

            //var eventAggregator = IoC.Get<IEventAggregator>();
            //eventAggregator.PublishOnUIThread(new OpenMenuItemsChangedEvent(_openMenuItems));
        }

        public bool CloseAllWindows()
        {
            var dockingManager = GetDockingManager();
            if (dockingManager != null)
            {
                var dirtyViewModels = dockingManager.DockableContents.OfType<BaseDockableContent>().Where(x => x.ViewModel.IsDirty()).ToList();
                return ConfirmDirtyViewModelClose(dirtyViewModels);
            }
            return true;
        }

        public bool CheckDirtyWindows(DockArea dockArea)
        {
            var dockingManager = GetDockingManager();
            if (dockingManager != null)
            {
                var dirtyViewModels = dockingManager.DockableContents.OfType<BaseDockableContent>().Where(x => (x.ViewModel.IsDirty()) && (x.DockArea == dockArea)).ToList();
                return ConfirmDirtyViewModelClose(dirtyViewModels, "continue");
            }
            return true;
        }

        private DockingManager CreateFromView(Window window)
        {
            Window parentWindow = GetParentWindow(window);

            return parentWindow != null ? parentWindow.FindChild<DockingManager>("MasterDockingManager") : null;
        }

        private static Window GetParentWindow(Window window)
        {
            Window parentWindow = (Application.Current != null ? Application.Current.MainWindow : null);
            return parentWindow != window ? parentWindow : null;
        }

        #endregion docking manager stuff
    }
}
