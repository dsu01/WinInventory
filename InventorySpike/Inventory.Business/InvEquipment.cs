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
    
    public partial class InvEquipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public InvEquipment()
        {
            this.InvEquipmentAttachments = new HashSet<InvEquipmentAttachment>();
        }
    
        public string EquipmentName { get; set; }
        public string EquipmentNameTemp { get; set; }
        public string ParentFacility_ { get; set; }
        public string EquipmentFacility_ { get; set; }
        public string EquipmentSystem { get; set; }
        public string EquipmentGroup { get; set; }
        public string EquipmentID { get; set; }
        public string Location { get; set; }
        public string TypeorUse { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public string Size { get; set; }
        public Nullable<System.DateTime> InstallDate { get; set; }
        public string Capacity { get; set; }
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
        public string Status { get; set; }
        public Nullable<int> EquipmentSerialNo { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CKTSUsed { get; set; }
        public string VOLTSPRIMARY { get; set; }
        public string AMP { get; set; }
        public string KVA { get; set; }
        public string VOLTSSECONDARY { get; set; }
        public string VOLTS { get; set; }
        public string W { get; set; }
        public string PH { get; set; }
        public string NOofCKTS { get; set; }
        public string ElectricalOther { get; set; }
        public string InventoryBy { get; set; }
        public Nullable<System.DateTime> InventoryDate { get; set; }
        public string InputBy { get; set; }
        public Nullable<System.DateTime> InputDate { get; set; }
        public string EquipmentNo { get; set; }
        public System.Guid SYNC_ID { get; set; }
        public Nullable<int> ID { get; set; }
        public Nullable<System.Guid> InvFacilityId { get; set; }
    
        public virtual InvFacility InvFacility { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<InvEquipmentAttachment> InvEquipmentAttachments { get; set; }
    }
}
