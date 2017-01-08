using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AvalonDock;

namespace Client.Framework.Docking
{
    public class ContentArea
    {
        public DockingManager DockingManager { get; set; }

        public List<DockableContent> DockableContentList { get; set; }

        //public DockableContent DockableContentPane { get; set; }

        public DockablePane DockablePane { get; set; }

        public DockArea DockArea { get; set; }

        public ContentArea(DockingManager manager, DockArea dockArea)
            : this(dockArea)
        {
            DockingManager = manager;
        }

        public ContentArea(DockArea dockArea)
        {
            DockArea = dockArea;
            //DockingManager = new DockingManager { Name = dockArea.ToString() };

            DockableContentList = new List<DockableContent>();

            DockablePane = new DockablePane
            {
                Name = dockArea.ToString(),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                HorizontalContentAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                // ItemsSource = DockableContentList
            };

            //DockableContentPane = new DockableContent { Content = DockablePane, Name = dockArea.ToString() };

            //DockableContent = new ContainerDockableContent
            //{
            //    //Name = DockingManager.Name,
            //    //Title = DockingManager.Name,
            //    //Content = DockingManager,
            //    Focusable = true,
            //    DockableStyle = DockableStyle.Dockable,
            //    HideOnClose = false,
            //    DockArea = dockArea,
            //    HorizontalAlignment = HorizontalAlignment.Stretch,
            //    HorizontalContentAlignment = HorizontalAlignment.Stretch,
            //    VerticalAlignment = VerticalAlignment.Stretch,
            //    VerticalContentAlignment = VerticalAlignment.Stretch
            //};

            DockablePane.SetValue(DockablePane.AnchorPropertyKey, AnchorStyle.Bottom);
            //DockablePane.Items.Add(DockableContent);
        }

        public BaseDockableContent HasView(object rootModel)
        {
            return DockableContentList.FirstOrDefault(dc => dc.DataContext == rootModel) as BaseDockableContent;
            //return DockingManager.DockableContents.FirstOrDefault(dc => dc.DataContext == rootModel) as BaseDockableContent;
        }
    }
}