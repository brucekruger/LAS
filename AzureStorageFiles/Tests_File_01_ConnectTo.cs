using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace AzureStorageFiles
{
    [TestClass]
    public class Tests_File_01_ConnectTo
    {
        [TestMethod]
        public async Task Test_01_Connect()
        {
            string saName = ConfigurationProvider.saName;
            saName.Should().NotBeEmpty();

            string saKey = ConfigurationProvider.saKey;
            saKey.Should().NotBeEmpty();

            var saCreds = new StorageCredentials(saName, saKey);
            var saConfig = new CloudStorageAccount(saCreds, true);
            var client = saConfig.CreateCloudFileClient();

            var share = client.GetShareReference("demo01");
            bool shareExists = await share.ExistsAsync();

            shareExists.Should().BeTrue();
        }

        [TestMethod]
        public void Test_02_ClientConnectionLimit()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit.Should().Be(2);
        }
    }
}
