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
    
    public partial class tblTaskScheduleWOactivity
    {
        public int ID { get; set; }
        public int TaskScheduleID { get; set; }
        public string Activity { get; set; }
        public byte[] TimeStamp { get; set; }
    
        public virtual tblTaskScheduleWO tblTaskScheduleWO { get; set; }
    }
}
