
using Azure.Security.KeyVault.Secrets;

namespace MinimalApiSecurityExperimentsDotNet.Extensions
{
    
    public class PseudonymizerHostedService : IHostedService
    {

        private readonly IServiceProvider _provider;

        public PseudonymizerHostedService(IServiceProvider provider)
        {
            _provider = provider;            
        }      

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _provider.CreateScope();
            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var keyVaultRetriever = new KeyVaultSecretRetriever(config);
            var hmacSha256SecretName = config["AzureKeyVault:HmacSha256SecretName"];
            if (hmacSha256SecretName == null)
            {
                throw new InvalidOperationException($"Config '{hmacSha256SecretName}' not is missing.");
            }
            KeyVaultSecret? secret = await keyVaultRetriever.GetSecretAsync(hmacSha256SecretName);
        
            if (secret == null)
            {
                throw new InvalidOperationException($"Secret '{hmacSha256SecretName}' not found in Key Vault.");
            }

            var pseudonymizer = new Pseudonymizer(secret.Value);

            // Register the pseudonymizer in the pseudonymizerProvider
            var pseudonymizerProvider = scope.ServiceProvider.GetRequiredService<PseudonymizerProvider>();
            pseudonymizerProvider.Set(pseudonymizer);            
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    }

}
