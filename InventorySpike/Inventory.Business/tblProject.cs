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
    
    public partial class tblProject
    {
        public string Project { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public string Status { get; set; }
        public string Supervisor { get; set; }
        public Nullable<System.DateTime> StartEstimated { get; set; }
        public Nullable<System.DateTime> EndEstimated { get; set; }
        public Nullable<System.DateTime> StartActual { get; set; }
        public Nullable<System.DateTime> EndActual { get; set; }
        public Nullable<System.DateTime> Due { get; set; }
        public Nullable<int> HourLaborEstimated { get; set; }
        public Nullable<int> HourLaborActual { get; set; }
        public Nullable<float> HourDowntimeEstimated { get; set; }
        public Nullable<float> HourDownTimeActual { get; set; }
        public string Account { get; set; }
        public string Comments { get; set; }
        public string UDF1 { get; set; }
        public string UDF2 { get; set; }
        public string UDF3 { get; set; }
        public string UDF4 { get; set; }
        public string UDF5 { get; set; }
        public byte[] upsize_ts { get; set; }
        public string Property { get; set; }
    }
}
