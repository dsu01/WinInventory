using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServer;

namespace SyncProvisionTool
{
    internal struct ProvisionStruct
    {
        public string connectionString;
        public string databaseName;
        public string scopeName;
        public List<string> tableNames;
    }

    class Program
    {
        private static ProvisionStruct provisionStruct = new ProvisionStruct();

        static void Main(string[] args)
        {
            provisionStruct.connectionString = String.Empty;
            provisionStruct.databaseName = String.Empty;
            provisionStruct.scopeName = String.Empty;
            provisionStruct.tableNames = null;

            string scopeName = String.Empty;

            try
            {
                string database = string.Empty;

                if (args != null && args.Length == 2)
                {
                    provisionStruct.connectionString = args[0];
                    database = args[1];
                }
                else
                {
                    provisionStruct.connectionString =
                        ConfigurationManager.ConnectionStrings["SyncConnection"].ConnectionString;
                    provisionStruct.databaseName = ConfigurationManager.AppSettings["Database"];
                    database = ConfigurationManager.AppSettings["Database"];
                }

                if (String.IsNullOrEmpty(database))
                    throw new InvalidOperationException("Config Section missing: SyncConnection");

                if (String.IsNullOrEmpty(database))
                    throw new InvalidOperationException("Config Section missing: database name.");

                // get Mode param
                try
                {
                    SyncConfigurationSectionHandler.SyncProvisionMode = SyncProvisionMode.Full; // default
                    if (args.Length >= 3)
                    {
                        SyncConfigurationSectionHandler.SyncProvisionMode =
                            (SyncProvisionMode) (Enum.Parse(typeof(SyncProvisionMode), args[2]));
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SyncProvisionMode"]))
                            SyncConfigurationSectionHandler.SyncProvisionMode =
                                (SyncProvisionMode)
                                (Enum.Parse(typeof(SyncProvisionMode),
                                    ConfigurationManager.AppSettings["SyncProvisionMode"]));
                    }
                }
                catch (Exception)
                {
                    throw new ArgumentException("Config Section Wrong Format: SyncSection Mode");
                }

                List<SyncConfigurationSection> sections =
                    (List<SyncConfigurationSection>) ConfigurationManager.GetSection("SyncConfigSections");
                if (sections == null || sections.Count < 1)
                    throw new InvalidOperationException("Config Section missing: SyncConfigSections");

                bool success = true;
                foreach (var syncConfigurationSection in sections)
                {
                    provisionStruct.scopeName = syncConfigurationSection.ScopeName;
                    provisionStruct.tableNames = syncConfigurationSection.Tables;

                    success = ConfigureSqlSyncProvider(provisionStruct);
                }

                Console.WriteLine(success ? "Scope Provision Successful" : "Scope already exists");
                if (!success)
                    return;

                //var auditMode = ConfigurationManager.AppSettings["AuditMode"];
                //if (auditMode.ToLower() == "true")
                //{
                //    CreateDatabaseAudit();
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured in provision:" + ex.Message);
                Environment.Exit(1);
            }
        }

        public static bool ConfigureSqlSyncProvider(ProvisionStruct provisionStruct)
        {
            SqlSyncProvider provider = null;
            bool returnVal = false;
            try
            {
                provider = new SqlSyncProvider();
                provider.ScopeName = provisionStruct.scopeName;
                provider.Connection = new SqlConnection(provisionStruct.connectionString);

                //create a new scope description and add the appropriate tables to this scope
                DbSyncScopeDescription scopeDesc = new DbSyncScopeDescription(provider.ScopeName);
                if (provisionStruct.tableNames != null)
                    foreach (var tableName in provisionStruct.tableNames)
                    {
                        var info = SqlSyncDescriptionBuilder.GetDescriptionForTable(tableName,(SqlConnection) provider.Connection);

                        //FixPrimaryKeysForTable(info);

                        scopeDesc.Tables.Add(info);
                    }

                //class to be used to provision the scope defined above
                SqlSyncScopeProvisioning serverConfig = null;
                serverConfig = new SqlSyncScopeProvisioning((System.Data.SqlClient.SqlConnection) provider.Connection);

                if (serverConfig.ScopeExists(provisionStruct.scopeName))
                    return false;

                if (!serverConfig.ScopeExists(provisionStruct.scopeName))
                {
                    //note that it is important to call this after the tables have been added to the scope
                    serverConfig.PopulateFromScopeDescription(scopeDesc);

                    //indicate that the base table already exists and does not need to be created
                    serverConfig.SetCreateTableDefault(DbSyncCreationOption.Skip);

                    //provision the server
                    serverConfig.Apply();

                    returnVal = true;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                //provider.Dispose();
            }

            return returnVal;
        }

        private static void FixPrimaryKeysForTable(DbSyncTableDescription description)
        {
            try
            {
                IEnumerator<DbSyncColumnDescription> enumerator = description.PkColumns.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DbSyncColumnDescription colDesc = enumerator.Current;
                    if (colDesc.IsPrimaryKey && colDesc.QuotedName != "SYNC_ID")
                    {
                        colDesc.IsPrimaryKey = false;
                    }
                }
                if (enumerator != null)
                    enumerator.Dispose();

                enumerator = description.NonPkColumns.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    DbSyncColumnDescription colDesc = enumerator.Current;
                    if (colDesc.UnquotedName == "SYNC_ID")
                    {
                        colDesc.IsPrimaryKey = true;
                    }
                    //if (colDesc.UnquotedName == "ID")
                    //{
                    //    colDesc.IsPrimaryKey = true;
                    //}
                }
                if (enumerator != null)
                    enumerator.Dispose();
            }
            finally
            {
                //((IDisposable) description).Dispose();
            }
        }
    }
}
