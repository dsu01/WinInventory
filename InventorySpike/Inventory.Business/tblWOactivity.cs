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
    
    public partial class tblWOactivity
    {
        public int ID { get; set; }
        public string WO { get; set; }
        public string Activity { get; set; }
    
        public virtual tblWO tblWO { get; set; }
    }
}
