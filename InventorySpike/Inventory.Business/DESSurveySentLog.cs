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
    
    public partial class DESSurveySentLog
    {
        public int seq { get; set; }
        public Nullable<int> WRnumber { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DateTimeSent { get; set; }
        public string body { get; set; }
        public string subject { get; set; }
    }
}