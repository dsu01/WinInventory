using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Framework;

namespace Client.ActionItems
{
    public class AdminActionItem:  ActionItem
    {
        public AdminActionItem(): base("Administration")
        {
            InitItems();
        }

        private void InitItems()
        {
            Items.Add(new ActionItem("Manager Users", null));
        }
    }
}
