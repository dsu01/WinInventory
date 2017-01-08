using System;
using System.Collections.Generic;
using AvalonDock;
using Caliburn.Micro;

namespace Client.Framework.Docking
{
    public enum DockSide
    {
        None,
        Left,
        Top,
        Right,
        Bottom
    }

    public enum DockArea
    {
        FormerInstitutionSetup,
        PrepareJackets,
        JacketSummary,
        CashBasisAdjustment,
        Reports,
        Administration,
        SecurityAdministration,
        ExternalUserFileViewer,
        Main,
        Master
    }

    public class DockStartupOption
    {
        public DockableContentState DockableContentState;

        public DockSide DockSide { get; set; }

        public bool SelectWhenShown { get; set; }

        public double StartupVisibleWidth { get; set; }

        public bool HideOnClose { get; set; }

        public bool IsCloseable { get; set; }

        public bool IsDefaultActivated { get; set; }

        public DockArea DockArea { get; set; }

        public Func<bool> CanDisplay { get; set; }
    }

    public interface IDockWindowManager : IWindowManager
    {
        void ShowFloatingWindow(object viewModel, DockArea dockArea, object context = null, bool selectWhenShown = true, double width = 0, double height = 0);

        void ShowDocumentWindow(object viewModel, object context = null, bool selectWhenShown = true, DockArea dockArea = DockArea.Master);

        void ShowDockedWindow(object viewModel, DockArea dockArea, object context = null, bool selectWhenShown = true, DockSide dockSide = DockSide.Left, double startupVisibleWidth = -1, bool hideOnClose = false, bool isCloseable = true);

        void ActivateDockableContent(object vmContent, DockArea docarea);

        void CloseDockWindow(ViewModelBase viewModel, bool forceClose = false);

        void HideDockWindow(ViewModelBase viewModel);

        void SaveLayout();

        void RestoreLayout();

        void CheckAreas(DockingManager dockingManager);

        bool CloseAllWindows();

        bool CheckDirtyWindows(DockArea dockArea);

        void ShowDocumentWindows(List<Tuple<object, DockArea>> windowsToDock);

        void ClearOpenMenuItems();

        IEnumerable<ViewModelBase> FindViewModelsByType(object item);
    }
}