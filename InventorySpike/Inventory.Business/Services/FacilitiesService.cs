﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.Services
{
    public interface IFacilitiesService
    {
        List<InvFacility> GetFacilities();

        List<InvFacility> GetElectricalSystems();

        InvFacility GetFacility(Guid id);

        InvFacility AddOrUpdateInvFacility(InvFacility facility, bool addOrUpdate);

        InvFacilityAttachment AddOrUpdateInvFacilityAttachment(InvFacilityAttachment facilityAttachment, bool addOrUpdate);

        bool DeleteFacilityAttachment(InvFacilityAttachment facilityAttachment);

        List<InvBuilding> GetBuildings();

        List<InvFacilitySystem> GetInvFacilitySystems();
    }

    public class FacilitiesService : IFacilitiesService
    {
        public FacilitiesService() { }

        public List<InvFacility> GetFacilities()
        {
            try
            {
                using (var dbContext = new InventoryEntities())
                {
                    var list = dbContext.InvFacilities
                        .Include(x => x.InvEquipments)
                        .OrderBy(x => x.Facility_)
                        .ToList();

                    return list;
                }
            }
            catch (Exception e)
            {
            }

            return null;
        }

        public List<InvFacility> GetElectricalSystems()
        {
            try
            {
                using (var dbContext = new InventoryEntities())
                {
                    var list = dbContext.InvFacilities.Where(x => x.FacilityGroup == "Electrical System")
                        .Include(x => x.InvEquipments)
                        //.Include(x => x.InvFacilityAttachments)
                        .OrderBy(x => x.Facility_)
                        .ToList();

                    return list;
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public InvFacility GetFacility(Guid id)
        {
            try
            {
                using (var dbContext = new InventoryEntities())
                {
                    return dbContext.InvFacilities
                        .Where(x => x.SYNC_ID == id)
                        .Include(x => x.InvEquipments)
                        .Include(x => x.InvFacilityAttachments)
                        .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
            }

            return null;
        }

        public InvFacility AddOrUpdateInvFacility(InvFacility facility, bool addOrUpdate)
        {
            var success = true;
            InvFacility saved = null;

            try
            {
                using (var dbContext = new InventoryEntities())
                {
                    // components
                    List<InvEquipment> listToSave = new List<InvEquipment>();
                    foreach (var equipment in facility.InvEquipments)
                    {
                        var existingEquipment = dbContext.InvEquipments
                            .Where(x => x.SYNC_ID == equipment.SYNC_ID)
                            .FirstOrDefault();
                        if (existingEquipment != null)
                        {
                            // existing
                            existingEquipment.EquipmentID = equipment.EquipmentID;
                            existingEquipment.TypeorUse = equipment.TypeorUse;
                            existingEquipment.Manufacturer = equipment.Manufacturer;
                            existingEquipment.Location = equipment.Location;
                            existingEquipment.Model = equipment.Model;
                            existingEquipment.SerialNo = equipment.SerialNo;
                            existingEquipment.MotorType = equipment.MotorType;
                            existingEquipment.Size = equipment.Size;
                            existingEquipment.InstallDate = equipment.InstallDate;
                            existingEquipment.Capacity = equipment.Capacity;
                            existingEquipment.CapacityHeadTest = equipment.CapacityHeadTest;
                            existingEquipment.FuelRefrigeration = equipment.FuelRefrigeration;
                            existingEquipment.MotorModel = equipment.MotorModel;
                            existingEquipment.MotorManufacturer = equipment.MotorManufacturer;
                            existingEquipment.HP = equipment.HP;
                            existingEquipment.SerialNo = equipment.SerialNo;
                            existingEquipment.MotorInstallDate = equipment.MotorInstallDate;
                            existingEquipment.Frame = equipment.Frame;
                            existingEquipment.TJCValue = equipment.TJCValue;
                            existingEquipment.RPM = equipment.RPM;
                            existingEquipment.Voltage = equipment.Voltage;
                            existingEquipment.Amperes = equipment.Amperes;
                            existingEquipment.PhaseCycle = equipment.PhaseCycle;
                            existingEquipment.PMSchedule = equipment.PMSchedule;
                            existingEquipment.BSLClassification = equipment.BSLClassification;

                            listToSave.Add(existingEquipment);
                        }
                        else
                        {
                            // new
                            dbContext.InvEquipments.Add(equipment);
                            listToSave.Add(equipment);
                        }
                    }

                    // attachments
                    foreach (var attachment in facility.InvFacilityAttachments)
                    {
                        var existingAttachment = dbContext.InvFacilityAttachments
                            .Where(x => x.ID == attachment.ID)
                            .FirstOrDefault();
                        if (existingAttachment != null)
                        {
                            // existing
                            existingAttachment.FileName = attachment.FileName;
                            existingAttachment.ContentType = attachment.ContentType;
                            existingAttachment.Data = attachment.Data;
                            existingAttachment.IsActive = attachment.IsActive;
                            existingAttachment.Title = attachment.Title;
                        }
                        else
                        {
                            // shouldn't be here
                        }
                    }

                    if (addOrUpdate) // add
                    {
                        facility.InvEquipments = listToSave;
                        dbContext.InvFacilities.Add(facility);
                    }
                    else    // update
                    {
                        var existing = dbContext.InvFacilities
                            .Where(x => x.SYNC_ID == facility.SYNC_ID)
                            .FirstOrDefault();
                        if (existing == null)
                            throw new ArgumentException(String.Format("Facility does not exist:{0}", facility.SYNC_ID));

                        existing.FacilitySystem = facility.FacilitySystem;
                        existing.FacilityID = facility.FacilityID;
                        existing.FacilityFunction = facility.FacilityFunction;
                        existing.AAALAC = facility.AAALAC;
                        existing.BSL = facility.BSL;
                        existing.TJC = facility.TJC;
                        existing.Building = facility.Building;
                        existing.Floor = facility.Floor;
                        existing.Location = facility.Location;
                        existing.WorkRequest_ = facility.WorkRequest_;

                        existing.InvEquipments = listToSave;
                    }

                    dbContext.SaveChanges();
                    saved = dbContext.InvFacilities
                        .Where(x => x.SYNC_ID == facility.SYNC_ID)
                        .Include(x => x.InvEquipments)
                        .Include(x => x.InvFacilityAttachments)
                        .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
            }

            return saved;
        }

        public InvFacilityAttachment AddOrUpdateInvFacilityAttachment(InvFacilityAttachment facilityAttachment,
            bool addOrUpdate)
        {
            var success = true;
            InvFacilityAttachment saved = null;

            try
            {

                using (var dbContext = new InventoryEntities())
                {
                    if (addOrUpdate) // add
                    {

                        dbContext.InvFacilityAttachments.Add(facilityAttachment);
                    }
                    else // update
                    {
                        // shouldn't be here
                    }

                    dbContext.SaveChanges();
                    saved = dbContext.InvFacilityAttachments
                        .Where(x => x.ID == facilityAttachment.ID)
                        .FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                saved = null;
            }

            return saved;
        }

        public bool DeleteFacilityAttachment(InvFacilityAttachment facilityAttachment)
        {
            var success = true;
            try
            {

                using (var dbContext = new InventoryEntities())
                {
                    var existing = dbContext.InvFacilityAttachments
                            .Where(x => x.ID == facilityAttachment.ID)
                            .FirstOrDefault();
                    if (existing == null)
                        return false;

                    dbContext.InvFacilityAttachments.Remove(existing);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        public List<InvBuilding> GetBuildings()
        {
            try
            {
                using (var dbContext = new InventoryEntities())
                {
                    var list = dbContext.InvBuildings
                        .OrderBy(x => x.Building)
                        .ToList();

                    return list;
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }

        public List<InvFacilitySystem> GetInvFacilitySystems()
        {
            try
            {
                using (var dbContext = new InventoryEntities())
                {
                    var list = dbContext.InvFacilitySystems
                        .OrderBy(x => x.SystemGroup)
                        .OrderBy(x => x.SystemTitle)
                        .ToList();

                    return list;
                }
            }
            catch (Exception e)
            {

            }

            return null;
        }
    }
}
