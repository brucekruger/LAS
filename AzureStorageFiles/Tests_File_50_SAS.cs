using System;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.File;

namespace AzureStorageFiles
{
    [TestClass]
    public class Tests_File_50_SAS
    {
        private static CloudFileClient client = null;

        public Tests_File_50_SAS()
        {
            string saName = ConfigurationProvider.saName;
            string saKey = ConfigurationProvider.saKey;

            var saCreds = new StorageCredentials(saName, saKey);
            var saConfig = new CloudStorageAccount(saCreds, true);
            client = saConfig.CreateCloudFileClient();
        }

        [TestMethod]
        public async Task Test_50_SAS()
        {
            var share = client.GetShareReference("photos");
            var root = share.GetRootDirectoryReference();
            var file = root.GetFileReference("cookies.jpg");

            var sasToken = file.GetSharedAccessSignature(
                new SharedAccessFilePolicy
                {
                    Permissions = SharedAccessFilePermissions.Read,
                    SharedAccessExpiryTime = new DateTimeOffset(DateTime.UtcNow.AddMinutes(5))
                });

            var sasCreds = new StorageCredentials(sasToken);
            var sasCF = new CloudFile(
                new Uri($"https://{ConfigurationProvider.saName}.file.core.windows.net/photos/cookies.jpg"),
                sasCreds);

            bool fileExists = await sasCF.ExistsAsync();
            fileExists.Should().BeTrue();

            var ms = new MemoryStream();
            await sasCF.DownloadToStreamAsync(ms);
            ms.Length.Should().BeGreaterThan(0);
        }
    }
}
