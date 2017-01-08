﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class msDATAEntities : DbContext
    {
        public msDATAEntities()
            : base("name=msDATAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DESMEOAlertSentLog> DESMEOAlertSentLogs { get; set; }
        public virtual DbSet<DESSurveySentLog> DESSurveySentLogs { get; set; }
        public virtual DbSet<DESWorkRequestCreateLog> DESWorkRequestCreateLogs { get; set; }
        public virtual DbSet<InvBuilding> InvBuildings { get; set; }
        public virtual DbSet<InvEquipment> InvEquipments { get; set; }
        public virtual DbSet<InvFacility> InvFacilities { get; set; }
        public virtual DbSet<InvFacilitySystem> InvFacilitySystems { get; set; }
        public virtual DbSet<InvStatu> InvStatus { get; set; }
        public virtual DbSet<NEDIC> NEDICs { get; set; }
        public virtual DbSet<NEDName> NEDNames { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<tblAccount> tblAccounts { get; set; }
        public virtual DbSet<tblActivity> tblActivities { get; set; }
        public virtual DbSet<tblAdministrativeHour> tblAdministrativeHours { get; set; }
        public virtual DbSet<tblAsset> tblAssets { get; set; }
        public virtual DbSet<tblAssetActivity> tblAssetActivities { get; set; }
        public virtual DbSet<tblAssetContact> tblAssetContacts { get; set; }
        public virtual DbSet<tblAssetDocument> tblAssetDocuments { get; set; }
        public virtual DbSet<tblAssetEquipment> tblAssetEquipments { get; set; }
        public virtual DbSet<tblAssetEquipmentContact> tblAssetEquipmentContacts { get; set; }
        public virtual DbSet<tblAssetEquipmentDocument> tblAssetEquipmentDocuments { get; set; }
        public virtual DbSet<tblAssetEquipmentPart> tblAssetEquipmentParts { get; set; }
        public virtual DbSet<tblAssetEquipmentSpec> tblAssetEquipmentSpecs { get; set; }
        public virtual DbSet<tblAssetMove> tblAssetMoves { get; set; }
        public virtual DbSet<tblAssetPart> tblAssetParts { get; set; }
        public virtual DbSet<tblAssetSpec> tblAssetSpecs { get; set; }
        public virtual DbSet<tblAssetSystem> tblAssetSystems { get; set; }
        public virtual DbSet<tblBillingHistory> tblBillingHistories { get; set; }
        public virtual DbSet<tblConstruction> tblConstructions { get; set; }
        public virtual DbSet<tblConstructionEvent> tblConstructionEvents { get; set; }
        public virtual DbSet<tblCustomerSurvey> tblCustomerSurveys { get; set; }
        public virtual DbSet<tblDateRange> tblDateRanges { get; set; }
        public virtual DbSet<tblDay> tblDays { get; set; }
        public virtual DbSet<tblDocument> tblDocuments { get; set; }
        public virtual DbSet<tblEvent> tblEvents { get; set; }
        public virtual DbSet<tblEventLogin> tblEventLogins { get; set; }
        public virtual DbSet<tblEventSystem> tblEventSystems { get; set; }
        public virtual DbSet<tblFailure> tblFailures { get; set; }
        public virtual DbSet<tblForm> tblForms { get; set; }
        public virtual DbSet<tblHoliday> tblHolidays { get; set; }
        public virtual DbSet<tblLabor> tblLabors { get; set; }
        public virtual DbSet<tblLaborLogin> tblLaborLogins { get; set; }
        public virtual DbSet<tblLaborProperty> tblLaborProperties { get; set; }
        public virtual DbSet<tblLaborTraining> tblLaborTrainings { get; set; }
        public virtual DbSet<tblLaborWorkDay> tblLaborWorkDays { get; set; }
        public virtual DbSet<tblMaintenanceCategory> tblMaintenanceCategories { get; set; }
        public virtual DbSet<tblMEOClassification> tblMEOClassifications { get; set; }
        public virtual DbSet<tblMiscCost> tblMiscCosts { get; set; }
        public virtual DbSet<tblOrder> tblOrders { get; set; }
        public virtual DbSet<tblOrderDetail> tblOrderDetails { get; set; }
        public virtual DbSet<tblPart> tblParts { get; set; }
        public virtual DbSet<tblPartDocument> tblPartDocuments { get; set; }
        public virtual DbSet<tblPartLocation> tblPartLocations { get; set; }
        public virtual DbSet<tblPartSpec> tblPartSpecs { get; set; }
        public virtual DbSet<tblPartSupplier> tblPartSuppliers { get; set; }
        public virtual DbSet<tblProject> tblProjects { get; set; }
        public virtual DbSet<tblProperty> tblProperties { get; set; }
        public virtual DbSet<tblPropertySpec> tblPropertySpecs { get; set; }
        public virtual DbSet<tblService> tblServices { get; set; }
        public virtual DbSet<tblShop> tblShops { get; set; }
        public virtual DbSet<tblSite> tblSites { get; set; }
        public virtual DbSet<tblSpec> tblSpecs { get; set; }
        public virtual DbSet<tblStandard> tblStandards { get; set; }
        public virtual DbSet<tblSubService> tblSubServices { get; set; }
        public virtual DbSet<tblSubstatu> tblSubstatus { get; set; }
        public virtual DbSet<tblTask> tblTasks { get; set; }
        public virtual DbSet<tblTaskActivity> tblTaskActivities { get; set; }
        public virtual DbSet<tblTaskAsset> tblTaskAssets { get; set; }
        public virtual DbSet<tblTaskAssetActivity> tblTaskAssetActivities { get; set; }
        public virtual DbSet<tblTaskAssetInspection> tblTaskAssetInspections { get; set; }
        public virtual DbSet<tblTaskDocument> tblTaskDocuments { get; set; }
        public virtual DbSet<tblTaskFrequency> tblTaskFrequencies { get; set; }
        public virtual DbSet<tblTaskInspection> tblTaskInspections { get; set; }
        public virtual DbSet<tblTaskLabor> tblTaskLabors { get; set; }
        public virtual DbSet<tblTaskMiscCost> tblTaskMiscCosts { get; set; }
        public virtual DbSet<tblTaskPart> tblTaskParts { get; set; }
        public virtual DbSet<tblTaskSchedule> tblTaskSchedules { get; set; }
        public virtual DbSet<tblTaskScheduleDateMax> tblTaskScheduleDateMaxes { get; set; }
        public virtual DbSet<tblTaskScheduleGroup> tblTaskScheduleGroups { get; set; }
        public virtual DbSet<tblTaskScheduleLock> tblTaskScheduleLocks { get; set; }
        public virtual DbSet<tblTaskScheduleWO> tblTaskScheduleWOes { get; set; }
        public virtual DbSet<tblTaskScheduleWOactivity> tblTaskScheduleWOactivities { get; set; }
        public virtual DbSet<tblTaskScheduleWOInspectionPoint> tblTaskScheduleWOInspectionPoints { get; set; }
        public virtual DbSet<tblTaskTool> tblTaskTools { get; set; }
        public virtual DbSet<tblTEAMMAccessToken> tblTEAMMAccessTokens { get; set; }
        public virtual DbSet<tblTool> tblTools { get; set; }
        public virtual DbSet<tblTraining> tblTrainings { get; set; }
        public virtual DbSet<tblWO> tblWOes { get; set; }
        public virtual DbSet<tblWOactivity> tblWOactivities { get; set; }
        public virtual DbSet<tblWOAssetComponent> tblWOAssetComponents { get; set; }
        public virtual DbSet<tblWOdocument> tblWOdocuments { get; set; }
        public virtual DbSet<tblWOhour> tblWOhours { get; set; }
        public virtual DbSet<tblWOhoursLost> tblWOhoursLosts { get; set; }
        public virtual DbSet<tblWOinspection> tblWOinspections { get; set; }
        public virtual DbSet<tblWOlabor> tblWOlabors { get; set; }
        public virtual DbSet<tblWOmiscCost> tblWOmiscCosts { get; set; }
        public virtual DbSet<tblWOpart> tblWOparts { get; set; }
        public virtual DbSet<tblWOtool> tblWOtools { get; set; }
        public virtual DbSet<tblWOtype> tblWOtypes { get; set; }
        public virtual DbSet<tblWR> tblWRs { get; set; }
        public virtual DbSet<tblZapp> tblZapps { get; set; }
        public virtual DbSet<tblZappuser> tblZappusers { get; set; }
        public virtual DbSet<tblZReport> tblZReports { get; set; }
        public virtual DbSet<tblZReportRole> tblZReportRoles { get; set; }
        public virtual DbSet<InvEquipmentAttachment> InvEquipmentAttachments { get; set; }
        public virtual DbSet<InvFacilityAttachment> InvFacilityAttachments { get; set; }
        public virtual DbSet<tblMEOPriority> tblMEOPriorities { get; set; }
        public virtual DbSet<tblMessage> tblMessages { get; set; }
        public virtual DbSet<tblPartClass> tblPartClasses { get; set; }
        public virtual DbSet<tblPartLocationList> tblPartLocationLists { get; set; }
    }
}