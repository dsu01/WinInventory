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
    
    public partial class tblTaskAssetInspection
    {
        public int ID { get; set; }
        public string Task { get; set; }
        public string InspectionPoint { get; set; }
        public string Asset { get; set; }
        public string Property { get; set; }
        public int ScheduleCounter { get; set; }
        public byte[] TimeStamp { get; set; }
    
        public virtual tblAsset tblAsset { get; set; }
        public virtual tblTask tblTask { get; set; }
    }
}