using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SyncProvisionTool
{
    public class SyncConfigurationSection
    {
        public string ScopeName;
        public List<string> Tables;
    }

    public enum SyncProvisionMode
    {
        Full = 1,
        FullWithoutFile = 2,
        FileOnly = 3,
    }

    public class SyncConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public SyncConfigurationSectionHandler()
        { }

        public static SyncProvisionMode SyncProvisionMode { get; set; }

        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            List<SyncConfigurationSection> items = new List<SyncConfigurationSection>();

            System.Xml.XmlNodeList processesNodes = section.SelectNodes("SyncSection");
            foreach (XmlNode processNode in processesNodes)
            {
                // only keep section that has the correct mode
                if (processNode.Attributes["Mode"].InnerText != SyncProvisionMode.ToString())
                    continue;

                var syncScopeNodes = processNode.SelectNodes("SyncScope");
                foreach (XmlNode syncScopeNode in syncScopeNodes)
                {
                    SyncConfigurationSection item = new SyncConfigurationSection();
                    item.ScopeName = syncScopeNode.Attributes["ScopeName"].InnerText;
                    var tableNames = syncScopeNode.Attributes["Tables"].InnerText;
                    if (String.IsNullOrEmpty(tableNames))
                        throw new InvalidOperationException("Tables Attribute Missing");
                    item.Tables = tableNames.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    items.Add(item);
                }
                break;  // we are done
            }
            return items;
        }
    }
}