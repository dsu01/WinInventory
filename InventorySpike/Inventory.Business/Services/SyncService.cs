using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Synchronization;
using Microsoft.Synchronization.Data;
using Microsoft.Synchronization.Data.SqlServer;
using WebSyncContract;

namespace Inventory.Business.Services
{
    public interface ISyncService
    {
        bool Synchronize(string scopeName, string localConnectionString, string remoteConnectionString);
    }

    public class SyncService : ISyncService
    {
        private const int _batchSize = 2147483647;
        private const string _batchFolder = @"E:\temp\_syncBatch";

        private string _scopeName;
        private string _localConnectionString;
        private string _remoteConnectionString;

        private SqlSyncProvider _localSqlSyncProvider;
        private SqlSyncProviderProxy _remoteSqlSyncProvider;

        public SyncService()
        {
        }

        public bool Synchronize(string scopeName, string localConnectionString, string remoteConnectionString)
        {
            try
            {
                _localSqlSyncProvider = new SqlSyncProvider()
                    {
                        ScopeName = scopeName,
                        Connection = new SqlConnection(localConnectionString),
                        MemoryDataCacheSize = _batchSize,
                        BatchingDirectory = _batchFolder,
                    }
                    ;

                _remoteSqlSyncProvider = new SqlSyncProviderProxy(scopeName, remoteConnectionString)
                {
                    BatchingDirectory = _batchFolder,
                };

                var stats = SynchronizeProviders(_localSqlSyncProvider, _remoteSqlSyncProvider);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private SyncOperationStatistics SynchronizeProviders(KnowledgeSyncProvider localProvider, KnowledgeSyncProvider remoteProvider)
        {
            SyncOrchestrator orchestrator = new SyncOrchestrator();
            orchestrator.LocalProvider = localProvider;
            orchestrator.RemoteProvider = remoteProvider;
            orchestrator.Direction = SyncDirectionOrder.UploadAndDownload;

            var stats = orchestrator.Synchronize();

            return stats;
        }

        /// <summary>
        /// Called whenever an enumerating provider spools a batch file to the disk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void provider_BatchSpooled(object sender, DbBatchSpooledEventArgs e)
        {

        }

        /// <summary>
        /// Calls when the destination provider successfully applies a batch file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void provider_BatchApplied(object sender, DbBatchAppliedEventArgs e)
        {

        }
    }
}
