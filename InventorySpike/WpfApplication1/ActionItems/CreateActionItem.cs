using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using Client.Classes;
using Client.Framework;
using Client.ViewModels;
using Inventory.Business;
using Inventory.Business.Services;

namespace Client.ActionItems
{
    public class CreateActionItem : ActionItem
    {
        public CreateActionItem() : base("Create Facility")
        {
            InitItems();
        }

        private void InitItems()
        {
            Items.Add(new ActionItem("Add Electrical System", () =>
            {
                var settings = new Dictionary<string, object>
                               {
                                   {"ResizeMode", ResizeMode.NoResize},
                                   {"WindowStartupLocation", WindowStartupLocation.CenterScreen}
                               };

                var facility = CreateNewElectricalSystem();
                var eventAggreggor = IoC.Get<IEventAggregator>();
                var windowManager = IoC.Get<IInvWindowManager>();
                var facilityService = IoC.Get<IFacilitiesService>();
                var applicationContext = IoC.Get<IApplicationContext>();
                var facilityInfoVm = new FacilityInfoViewModel(facility, applicationContext, eventAggreggor);
                var facilityVM = new FacilityDetailViewModel(facility, facilityInfoVm, windowManager, eventAggreggor, applicationContext, facilityService);
                var vm = new FacilityCreateViewModel(facilityVM, eventAggreggor, windowManager);
                windowManager.ShowDialog(vm, null, settings);
            }));

            Items.Add(new ActionItem("Add Mechanical System", null));
            Items.Add(new ActionItem("Add Electrical Equipment", null));
            Items.Add(new ActionItem("Add Electrical Equipment", null));
        }

        private InvFacility CreateNewElectricalSystem()
        {
            return new InvFacility()
            {
                FacilityName = String.Empty,
                Facility_ = "T00000-0",
                FacilityGroup = "Electrical System",
            }
                ;
        }
    }
}
