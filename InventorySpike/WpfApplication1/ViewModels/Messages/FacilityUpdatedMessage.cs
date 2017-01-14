using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventory.Business;

namespace Client.ViewModels.Messages
{
    public enum FacilityUpdateType
    {
        Create = 0,
        Updated = 1,
        Delete = 2,
    }

    public class FacilityUpdatedMessage
    {
        public FacilityUpdateType FacilityUpdateType { get; set; }

        public InvFacility Facility { get; set; }
    }
}
