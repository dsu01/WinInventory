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
    
    public partial class NEDName
    {
        public int ID { get; set; }
        public string ned_nih_id { get; set; }
        public string ned_personal_title { get; set; }
        public string ned_first_name { get; set; }
        public string ned_middle_name { get; set; }
        public string ned_last_name { get; set; }
        public string ned_office_telephone_number { get; set; }
        public string ned_room_number { get; set; }
        public string ned_building_abbreviation { get; set; }
        public string ned_ic_abbreviation { get; set; }
        public string ned_person_status { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public string TakenBy { get; set; }
        public string Email { get; set; }
        public string ned_ic_detail { get; set; }
        public string ned_building_property { get; set; }
    }
}