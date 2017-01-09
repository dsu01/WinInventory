using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Client.Framework;

namespace Client.ActionItems
{
    public class CreateActionItem:  ActionItem
    {
        public CreateActionItem(): base("Create Facility")
        {
            InitItems();
        }

        private void InitItems()
        {
            Items.Add(new ActionItem("Add Electrical System", null));
            Items.Add(new ActionItem("Add Mechanical System", null));
            Items.Add(new ActionItem("Add Electrical Equipment", null));
            Items.Add(new ActionItem("Add Electrical Equipment", null));
        }
    }
}
