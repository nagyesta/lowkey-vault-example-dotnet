using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using Azure.Core;
using Azure.Core.Pipeline;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using Azure.Security.KeyVault.Secrets;
using NUnit.Framework;
using src;

namespace test
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public void TestEncryptAndDecryptShouldCancelOutEachOther()
        {
            //given
            const string secretMessage = "a secret message";
            TokenCredential credential = new NoopCredentials();
            var options = new KeyClientOptions(KeyClientOptions.ServiceVersion.V7_3)
            {
                DisableChallengeResourceVerification = true
            };
            var keyClient = new KeyClient(new Uri("https://localhost:8443/"), credential, GetClientOptions(options));
            const string keyName = "rsa-key";
            var createRsaKeyOptions = new CreateRsaKeyOptions(keyName)
            {
                KeySize = 2048,
                KeyOperations = { KeyOperation.Encrypt, KeyOperation.Decrypt, KeyOperation.WrapKey, KeyOperation.UnwrapKey }
            };
            keyClient.CreateKey(keyName, KeyType.Rsa, createRsaKeyOptions);
            var cryptographyClientOptions = new CryptographyClientOptions(CryptographyClientOptions.ServiceVersion.V7_3);
            var underTest = new AzureKeyVaultKeyRepository(keyClient, credential, keyName, GetClientOptions(cryptographyClientOptions));
            
            //when
            var encrypted = underTest.Encrypt(secretMessage);
            var decrypted = underTest.Decrypt(encrypted);

            //then
            Assert.AreEqual(secretMessage, decrypted);
        }
        
        [Test]
        public void TestGetConnectionDetailsShouldReturnValidData()
        {
            //given
            const string serverNameSecret = "ServerName";
            const string userNameSecret = "UserName";
            const string passwordSecret = "Password";
            const string serverName = "ServerName\\InstanceName";
            const string userName = "admin";
            const string password = "secret123";
            TokenCredential credential = new NoopCredentials();
            var options = new SecretClientOptions(SecretClientOptions.ServiceVersion.V7_3)
            {
                DisableChallengeResourceVerification = true
            };
            var secretClient = new SecretClient(new Uri("https://localhost:8443/"), credential, GetClientOptions(options));
            secretClient.SetSecret(serverNameSecret, serverName);
            secretClient.SetSecret(userNameSecret, userName);
            secretClient.SetSecret(passwordSecret, password);
            var underTest = new AzureKeyVaultSecretRepository(secretClient, 
                serverNameSecret, userNameSecret, passwordSecret);
            
            //when
            var actualServerName = underTest.GetServerName();
            var actualUserName = underTest.GetUserName();
            var actualPassword = underTest.GetPassword();

            //then
            Assert.AreEqual(serverName, actualServerName);
            Assert.AreEqual(userName, actualUserName);
            Assert.AreEqual(password, actualPassword);
        }
        
        [Test]
        public void TestGetCertificateDetailsShouldReturnValidData()
        {
            //given
            const string certificateName = "certificate";
            const string subject = "CN=example.com";
            TokenCredential credential = new NoopCredentials();
            var secretClientOptions = new SecretClientOptions(SecretClientOptions.ServiceVersion.V7_3)
            {
                DisableChallengeResourceVerification = true
            };
            var certificateClientOptions = new CertificateClientOptions(CertificateClientOptions.ServiceVersion.V7_3)
            {
                DisableChallengeResourceVerification = true
            };
            var secretClient = new SecretClient(new Uri("https://localhost:8443/"), credential, GetClientOptions(secretClientOptions));
            var certificateClient = new CertificateClient(new Uri("https://localhost:8443/"), credential, GetClientOptions(certificateClientOptions));
            var certificatePolicy = new CertificatePolicy("Self", subject)
            {
                KeyType = CertificateKeyType.Ec,
                KeyCurveName = CertificateKeyCurveName.P256,
                Exportable = true,
                ContentType = CertificateContentType.Pkcs12,
                ValidityInMonths = 12
            };
            certificateClient.StartCreateCertificate(certificateName, certificatePolicy).WaitForCompletion();
            var underTest = new AzureKeyVaultCertificateRepository(secretClient, certificateName);
            
            //when
            var actualCertificate = underTest.GetCertificate();
            var actualPrivateKey = underTest.GetPrivateKey();

            //then
            Assert.AreEqual(subject, actualCertificate.Subject);
            Assert.NotNull(actualPrivateKey);
        }
        
        private T GetClientOptions<T>(T options) where T : ClientOptions
        {
            DisableSslValidationOnClientOptions(options);
            return options;
        }

        /// <summary>
        /// Disables server certification callback.
        /// <br/>
        /// <b>WARNING: Do not use in production environments.</b>
        /// </summary>
        /// <param name="options"></param>
        private void DisableSslValidationOnClientOptions(ClientOptions options)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            options.Transport = new HttpClientTransport(clientHandler);
        }
    }
}