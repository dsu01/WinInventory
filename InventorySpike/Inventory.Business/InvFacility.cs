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
    
    public partial class InvFacility
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvFacility()
        {
            this.InvEquipments = new HashSet<InvEquipment>();
            this.InvFacilityAttachments = new HashSet<InvFacilityAttachment>();
        }
    
        public string FacilityName { get; set; }
        public string FacilityNameTemp { get; set; }
        public string WorkRequest_ { get; set; }
        public string Facility_ { get; set; }
        public string Facility_Temp { get; set; }
        public string FacilitySystem { get; set; }
        public string FacilityGroup { get; set; }
        public string FacilityFunction { get; set; }
        public string FacilityID { get; set; }
        public string Comments { get; set; }
        public string Property { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string Wing { get; set; }
        public string Location { get; set; }
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public Nullable<System.DateTime> InstallDate { get; set; }
        public string Manufacturer { get; set; }
        public string Capacity { get; set; }
        public int IsNew { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string InputBy { get; set; }
        public string UpdateBy { get; set; }
        public int BSL { get; set; }
        public int AAALAC { get; set; }
        public int TJC { get; set; }
        public Nullable<System.DateTime> InventoryDate { get; set; }
        public string InventoryBy { get; set; }
        public string Status { get; set; }
        public string TypeorUse { get; set; }
        public string Size { get; set; }
        public string CapacityHeadTest { get; set; }
        public string FuelRefrigeration { get; set; }
        public string MotorManufacturer { get; set; }
        public string HP { get; set; }
        public string MotorType { get; set; }
        public string MotorSerialNo { get; set; }
        public Nullable<System.DateTime> MotorInstallDate { get; set; }
        public string MotorModel { get; set; }
        public string Frame { get; set; }
        public string RPM { get; set; }
        public string Voltage { get; set; }
        public string Amperes { get; set; }
        public string PhaseCycle { get; set; }
        public string BSLClassification { get; set; }
        public Nullable<int> TJCValue { get; set; }
        public string PMSchedule { get; set; }
        public string VOLTS { get; set; }
        public string AMP { get; set; }
        public string KVA { get; set; }
        public string VOLTSPRIMARY { get; set; }
        public string VOLTSSECONDARY { get; set; }
        public string PH { get; set; }
        public string W { get; set; }
        public string NOofCKTS { get; set; }
        public string CKTSUsed { get; set; }
        public string ElectricalOther { get; set; }
        public Nullable<int> ID { get; set; }
        public System.Guid SYNC_ID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvEquipment> InvEquipments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvFacilityAttachment> InvFacilityAttachments { get; set; }
    }
}
