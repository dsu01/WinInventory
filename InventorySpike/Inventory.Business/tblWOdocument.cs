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
    
    public partial class tblWOdocument
    {
        public int ID { get; set; }
        public string WO { get; set; }
        public string Document { get; set; }
        public string Description { get; set; }
        public int PrintOnWO { get; set; }
        public string Source { get; set; }
        public byte[] TimeStamp { get; set; }
    
        public virtual tblDocument tblDocument { get; set; }
        public virtual tblWO tblWO { get; set; }
    }
}
