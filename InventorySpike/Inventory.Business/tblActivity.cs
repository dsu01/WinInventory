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
    
    public partial class tblActivity
    {
        public string Activity { get; set; }
        public Nullable<decimal> CostEstimated { get; set; }
        public Nullable<float> TimeEstimated { get; set; }
        public string Category { get; set; }
        public string Account { get; set; }
        public string Comments { get; set; }
        public int DescriptionBuilder { get; set; }
        public byte[] TimeStamp { get; set; }
    }
}