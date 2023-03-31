using Azure.Security.KeyVault.Secrets;

namespace src
{
    public class AzureKeyVaultCertificateRepository
    {
        private readonly SecretClient _secretClient;
        private readonly string _certificateName;

        public AzureKeyVaultCertificateRepository(SecretClient secretClient,  
            string certificateName)
        {
            _secretClient = secretClient;
            _certificateName = certificateName;
        }
        
        public string GetPfxBase64()
        {
            return _secretClient.GetSecret(_certificateName).Value.Value;
        }
    }
}