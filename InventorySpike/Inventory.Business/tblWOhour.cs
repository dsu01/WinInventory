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
    
    public partial class tblWOhour
    {
        public int ID { get; set; }
        public string Labor { get; set; }
        public Nullable<int> WOnumber { get; set; }
        public Nullable<System.DateTime> DateWork { get; set; }
        public Nullable<float> RegularHours { get; set; }
        public Nullable<float> OverTimeHours { get; set; }
        public Nullable<float> DoubleHours { get; set; }
        public Nullable<float> OtherHours { get; set; }
        public string Category { get; set; }
        public string Account { get; set; }
        public string Comments { get; set; }
        public Nullable<int> CloseOut { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public string InputUserName { get; set; }
        public string ChangeShop { get; set; }
        public Nullable<int> OpenOut { get; set; }
        public Nullable<int> AppUserID { get; set; }
        public Nullable<int> HoldOut { get; set; }
        public string ChangeSubstatus { get; set; }
    }
}
