//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventory.Business
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblService
    {
        public int ServiceID { get; set; }
        public string Service { get; set; }
        public string ServiceCategory { get; set; }
        public string EquipmentType { get; set; }
        public string Comments { get; set; }
        public Nullable<System.DateTime> RequestedDate { get; set; }
        public string RequestedBy { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
        public string WOType { get; set; }
    }
}
