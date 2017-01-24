using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Business;

namespace Client.ViewModels.Messages
{
    public class EquipmentSelectedMessage
    {
        public InvEquipment Equipment { get; set; }
    }
}
