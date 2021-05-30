using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;

namespace AzureStorageFiles
{
    [TestClass]
    public class Tests_File_20_Operations
    {
        private static CloudFileClient client = null;

        public Tests_File_20_Operations()
        {
            string saName = ConfigurationProvider.saName;
            string saKey = ConfigurationProvider.saKey;

            var saCreds = new StorageCredentials(saName, saKey);
            var saConfig = new CloudStorageAccount(saCreds, true);
            client = saConfig.CreateCloudFileClient();
        }

        [TestMethod]
        public async Task Test_20_UploadFile()
        {
            var share = client.GetShareReference("photos");
            await share.CreateIfNotExistsAsync();

            var root = share.GetRootDirectoryReference();

            const string fileName = "cookies.jpg";

            var file = root.GetFileReference(fileName);

            var localFileName = $"Files\\{fileName}";
            File.Exists(localFileName).Should().BeTrue();

            await file.UploadFromFileAsync(localFileName);

            var fileExists = await file.ExistsAsync();
            fileExists.Should().BeTrue();
        }
    }
}
