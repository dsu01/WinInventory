using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class SyncActionItem : ActionItem
    {
        private readonly IInvWindowManager _windowManager;

        public SyncActionItem(IInvWindowManager windowManager) : base("Sync Actions")
        {
            _windowManager = windowManager;

            InitItems();
        }

        private void InitItems()
        {
            Items.Add(new ActionItem("Sync All", () =>
            {
                var success = true;
                var syncService = IoC.Get<ISyncService>();

                var scopeName = ConfigurationManager.AppSettings["Scope"];
                var localConnectionString = ConfigurationManager.ConnectionStrings["LocalConnectionString"].ConnectionString;
                var remoteConnectionString = ConfigurationManager.ConnectionStrings["RemoteConnectionString"].ConnectionString;

                CursorHelper.ExecuteWithWaitCursor(() =>
                {
                    success = syncService.Synchronize(scopeName, localConnectionString, remoteConnectionString);
                    if (success)
                        _windowManager.Inform("Sync", "synced successfully");
                    else
                        _windowManager.ShowError("Sync", "sync failed");
                });
            }));

            Items.Add(new ActionItem("Sync Up", null));
            Items.Add(new ActionItem("Sync Down", null));
        }

        private InvFacility CreateNewElectricalSystem()
        {
            var applicationContext = IoC.Get<IApplicationContext>();

            return new InvFacility()
            {
                FacilityName = String.Empty,
                Facility_ = "T00000-0",
                FacilityGroup = "Electrical System",
                Status = "Active",
                InputBy = applicationContext.ActiveUser,
                InputDate = DateTime.Now,
                SYNC_ID = Guid.NewGuid(),
            }
            ;
        }
    }
}
