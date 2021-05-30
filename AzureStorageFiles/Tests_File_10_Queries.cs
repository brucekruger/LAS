using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;

namespace AzureStorageFiles
{
    [TestClass]
    public class Tests_File_10_Queries
    {
        private static CloudFileClient client = null;

        public Tests_File_10_Queries()
        {
            string saName = ConfigurationProvider.saName;
            string saKey = ConfigurationProvider.saKey;

            var saCreds = new StorageCredentials(saName, saKey);
            var saConfig = new CloudStorageAccount(saCreds, true);
            client = saConfig.CreateCloudFileClient();
        }

        [TestMethod]
        public async Task Test_10_QueryForShares()
        {
            var shares = new List<CloudFileShare>();
            FileContinuationToken fct = null;
            do
            {
                var srs = await client.ListSharesSegmentedAsync(fct);
                fct = srs.ContinuationToken;
                shares.AddRange(srs.Results);
            } while (fct != null);

            shares.Count.Should().BeGreaterOrEqualTo(1);
        }

        [TestMethod]
        public async Task Test_11_QueryForFilesAndDirectories()
        {
            var share = client.GetShareReference("demo01");
            bool shareExists = await share.ExistsAsync();
            shareExists.Should().BeTrue();

            var rootDir = share.GetRootDirectoryReference();
            bool rootExists = await rootDir.ExistsAsync();
            rootExists.Should().BeTrue();

            var queryResults = new List<IListFileItem>();
            FileContinuationToken fct = null;

            do
            {
                var queryResult = await rootDir.ListFilesAndDirectoriesSegmentedAsync(fct);
                queryResults.AddRange(queryResult.Results);
                fct = queryResult.ContinuationToken;
            } while (fct != null);

            queryResults.Count.Should().BeGreaterThan(0);
        }
    }
}
