using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Business;

namespace Client.ViewModels.Messages
{
    public enum EquipmentUpdateType
    {
        Create = 0,
        Updated = 1,
        Delete = 2,
    }

    public class EquipmentUpdatedMessage
    {
        public EquipmentUpdateType EquipmentUpdateType { get; set; }

        public InvEquipment Equipment { get; set; }
    }
}
