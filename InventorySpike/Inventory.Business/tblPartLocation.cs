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
    
    public partial class tblPartLocation
    {
        public int ID { get; set; }
        public string Part { get; set; }
        public string Location { get; set; }
        public string Comments { get; set; }
        public Nullable<float> Available { get; set; }
        public Nullable<float> Reserved { get; set; }
        public Nullable<float> OnHand { get; set; }
        public Nullable<int> MinQty { get; set; }
        public Nullable<int> MaxQty { get; set; }
        public byte[] TimeStamp { get; set; }
    
        public virtual tblPart tblPart { get; set; }
    }
}
