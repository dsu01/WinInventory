using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Inventory.Business;
using PropertyChanged;

namespace Client.Classes
{
    public interface IApplicationContext : INotifyPropertyChangedEx, INotifyPropertyChanged
    {
        string ActiveUser { get; set; }

        List<InvBuilding> Buildings { get; set; }

        List<InvFacilitySystem> InvFacilitySystems { get; set; }
    }

    [ImplementPropertyChanged]
    public class ApplicationContext : PropertyChangedBase, IApplicationContext, INotifyPropertyChangedEx, INotifyPropertyChanged
    {
        public ApplicationContext()
        {
            ActiveUser = "David Su";
        }

        public string ActiveUser { get; set; }

        public List<InvBuilding> Buildings { get; set; }

        public List<InvFacilitySystem> InvFacilitySystems { get; set; }
    }
}