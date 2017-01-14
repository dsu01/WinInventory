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
