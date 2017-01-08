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
    
    public partial class tblWOpart
    {
        public int ID { get; set; }
        public string WO { get; set; }
        public string Part { get; set; }
        public string Location { get; set; }
        public Nullable<int> LocationID { get; set; }
        public float QuantityEstimated { get; set; }
        public float QuantityActual { get; set; }
        public decimal Cost { get; set; }
        public Nullable<int> CostAuto { get; set; }
        public decimal CostExtendedActual { get; set; }
        public decimal CostExtendedEstimated { get; set; }
        public Nullable<int> ChargeAuto { get; set; }
        public decimal Charge { get; set; }
        public float ChargePercentage { get; set; }
        public decimal ChargeExtended { get; set; }
        public string Account { get; set; }
        public string MaintenanceCategory { get; set; }
        public string Comments { get; set; }
        public byte[] TimeStamp { get; set; }
        public Nullable<System.DateTime> Recorded { get; set; }
    
        public virtual tblWO tblWO { get; set; }
    }
}
