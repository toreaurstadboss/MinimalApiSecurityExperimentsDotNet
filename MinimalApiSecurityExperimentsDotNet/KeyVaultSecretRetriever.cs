using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace MinimalApiSecurityExperimentsDotNet
{
    
    public class KeyVaultSecretRetriever
    {
        private readonly IConfiguration _configuration;
        public KeyVaultSecretRetriever(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<KeyVaultSecret>? GetSecretAsync(string secretName)
        {
            try
            {
                string keyVaultUri = _configuration["AzureKeyVault:VaultUri"] ?? throw new ArgumentNullException(nameof(keyVaultUri));
                var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
                KeyVaultSecret secret = await client.GetSecretAsync(secretName);
                return secret;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving the secret {secretName}: {ex.Message}");
                return null;
            }
        }     

    }

}
