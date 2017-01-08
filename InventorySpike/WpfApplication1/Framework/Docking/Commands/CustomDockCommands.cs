using System.Windows.Input;

namespace Client.Framework.Docking.Commands
{
    public class CustomDockCommands
    {
        public static RoutedUICommand CloseAllDocumentPane { get; private set; }

        public static RoutedUICommand CloseThisDocumentPane { get; private set; }

        public static RoutedUICommand CloseAllButThisDocumentPane { get; private set; }

        public static RoutedUICommand CloseAllDockablePane { get; private set; }

        public static RoutedUICommand CloseDockablePane { get; private set; }

        static CustomDockCommands()
        {
            CloseAllDocumentPane = new RoutedUICommand("Close All", "CloseAllDocumentPane", typeof(CustomDockCommands));
            CloseThisDocumentPane = new RoutedUICommand("Close This", "CloseThisDocumentPane", typeof(CustomDockCommands));
            CloseAllButThisDocumentPane = new RoutedUICommand("Close All But This", "CloseAllButThisDocumentPane", typeof(CustomDockCommands));
            CloseAllDockablePane = new RoutedUICommand("Close All", "CloseAllDockablePane", typeof(CustomDockCommands));
            CloseDockablePane = new RoutedUICommand("Close", "CloseDockablePane", typeof(CustomDockCommands));
        }
    }
}