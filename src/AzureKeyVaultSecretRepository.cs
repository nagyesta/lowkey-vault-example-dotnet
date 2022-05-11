using Azure.Security.KeyVault.Secrets;

namespace src
{
    public class AzureKeyVaultSecretRepository
    {
        private readonly SecretClient _secretClient;
        private readonly string _secretNameServer;
        private readonly string _secretNameUser;
        private readonly string _secretNamePass;

        public AzureKeyVaultSecretRepository(SecretClient secretClient,  
            string secretNameServer, string secretNameUser, string secretNamePass)
        {
            _secretClient = secretClient;
            _secretNameServer = secretNameServer;
            _secretNameUser = secretNameUser;
            _secretNamePass = secretNamePass;
        }

        public string GetServerName()
        {
            return _secretClient.GetSecret(_secretNameServer).Value.Value;
        }
        
        public string GetUserName()
        {
            return _secretClient.GetSecret(_secretNameUser).Value.Value;
        }
        
        public string GetPassword()
        {
            return _secretClient.GetSecret(_secretNamePass).Value.Value;
        }
    }
}