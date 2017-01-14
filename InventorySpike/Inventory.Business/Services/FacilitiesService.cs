using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Business.Services
{
    public interface IFacilitiesService
    {
        List<InvFacility> GetFacilities();

        InvFacility AddOrUpdateInvFacility(InvFacility facility);

        List<InvBuilding> GetBuildings();
    }

    public class FacilitiesService : IFacilitiesService
    {
        public FacilitiesService() { }

        public List<InvFacility> GetFacilities()
        {
            try
            {
                using (var dbContext = new msDATAEntities())
                {
                    var list = dbContext.InvFacilities.Where(x => x.FacilityGroup == "Electrical System")
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

        public InvFacility AddOrUpdateInvFacility(InvFacility facility)
        {
            var success = true;
            InvFacility saved = null;

            try
            {
                using (var dbContext = new msDATAEntities())
                {
                    if (facility.ID <= 0) // update
                    {
                        dbContext.InvFacilities.Add(facility);
                        var val = dbContext.SaveChanges();
                        // TODO - work on uniqueness
                        saved = dbContext.InvFacilities
                            .Where(x => x.Facility_ == facility.Facility_)
                            .SingleOrDefault();
                    }
                    else    // update
                    {
                        var existing = dbContext.InvFacilities
                            .Where(x => x.ID == facility.ID)
                            .SingleOrDefault();
                        if (existing == null)
                            throw new ArgumentException(String.Format("Facility does not exist:{0}", facility.ID));

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

                        dbContext.SaveChanges();
                        saved = dbContext.InvFacilities
                            .Where(x => x.ID == facility.ID)
                            .SingleOrDefault();
                    }
                }
            }
            catch (Exception e)
            {
            }

            return saved;
        }

        public List<InvBuilding> GetBuildings()
        {
            try
            {
                using (var dbContext = new msDATAEntities())
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

    }
}
