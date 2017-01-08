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
    
    public partial class tblWOlabor
    {
        public int ID { get; set; }
        public string WO { get; set; }
        public string Labor { get; set; }
        public string LaborID { get; set; }
        public string Type { get; set; }
        public float EstimatedTimeLabor { get; set; }
        public Nullable<System.DateTime> DateWork { get; set; }
        public float HoursRegular { get; set; }
        public float HoursOvertime { get; set; }
        public float HoursDouble { get; set; }
        public float HoursOther { get; set; }
        public float HoursExtended { get; set; }
        public decimal CostRegular { get; set; }
        public decimal CostOvertime { get; set; }
        public decimal CostDouble { get; set; }
        public decimal CostOther { get; set; }
        public decimal CostExtendedEstimated { get; set; }
        public decimal CostExtendedActual { get; set; }
        public int ChargeAuto { get; set; }
        public Nullable<decimal> Charge { get; set; }
        public float ChargePercentage { get; set; }
        public decimal ChargeExtended { get; set; }
        public string MaintenanceCategory { get; set; }
        public string Account { get; set; }
        public int CostAuto { get; set; }
        public Nullable<System.DateTime> Recorded { get; set; }
        public byte[] TimeStamp { get; set; }
    
        public virtual tblLabor tblLabor { get; set; }
        public virtual tblWO tblWO { get; set; }
    }
}