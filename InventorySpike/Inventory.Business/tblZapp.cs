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
    
    public partial class tblZapp
    {
        public int ID { get; set; }
        public string ApplicationTitle { get; set; }
        public Nullable<int> Version { get; set; }
        public Nullable<int> Build { get; set; }
        public string Custom { get; set; }
        public Nullable<int> CustomBuild { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string Server { get; set; }
        public string Platform { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public Nullable<int> Active { get; set; }
    }
}
