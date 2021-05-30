using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;

namespace AzureStorageFiles
{
    public static class ConfigurationProvider
    {
        public static string saName = null;
        public static string saKey = null;

        static ConfigurationProvider()
        {
            var options = new SecretClientOptions
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };
            var client = new SecretClient(new Uri("https://learnazure-key-vault.vault.azure.net/"), new DefaultAzureCredential(), options);

            KeyVaultSecret saNameSec = client.GetSecret("AzureStorageAccountName");
            saName = saNameSec.Value;

            KeyVaultSecret saKeySec = client.GetSecret("AzureStorageAccountKey");
            saKey = saKeySec.Value;
        }
    }
}
