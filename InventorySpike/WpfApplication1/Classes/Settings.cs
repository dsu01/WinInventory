using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Classes
{
    public class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings) (ApplicationSettingsBase.Synchronized(new Settings())));

        public static Settings Default
        {
            get { return defaultInstance; }
        }

        public Settings()
            : base()
        {
        }
    }
}
