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
    
    public partial class tblWOinspection
    {
        public int ID { get; set; }
        public string WO { get; set; }
        public string Point { get; set; }
        public string GroupName { get; set; }
        public string Action { get; set; }
        public int Fail { get; set; }
        public Nullable<int> Rate { get; set; }
        public Nullable<float> Measurement { get; set; }
        public Nullable<int> WOnumber { get; set; }
        public int AssignWO { get; set; }
        public int Spec { get; set; }
        public string Comments { get; set; }
        public byte[] TimeStamp { get; set; }
        public Nullable<System.DateTime> Recorded { get; set; }
        public Nullable<int> Passed { get; set; }
        public Nullable<int> NotApplicable { get; set; }
    
        public virtual tblWO tblWO { get; set; }
    }
}
