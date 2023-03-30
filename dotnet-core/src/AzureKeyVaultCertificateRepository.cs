using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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

        public X509Certificate2 GetCertificate()
        {
            return GetPfx();
        }
        
        public ECDsa GetPrivateKey()
        {
            return GetPfx().GetECDsaPrivateKey();
        }
        
        private X509Certificate2 GetPfx()
        {
            var base64Value = _secretClient.GetSecret(_certificateName).Value.Value;
            var bytes = Convert.FromBase64String(base64Value);
            return new X509Certificate2(bytes, "", X509KeyStorageFlags.Exportable);
        }
    }
}