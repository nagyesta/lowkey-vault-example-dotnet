using System;
using System.Net;
using System.Net.Http;
using Azure.Core;
using Azure.Core.Pipeline;
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
            var keyClient = new KeyClient(new Uri("https://localhost:8443/"), credential, GetKeyClientOptions());
            const string keyName = "rsa-key";
            var createRsaKeyOptions = new CreateRsaKeyOptions(keyName)
            {
                KeySize = 2048,
                KeyOperations = { KeyOperation.Encrypt, KeyOperation.Decrypt, KeyOperation.WrapKey, KeyOperation.UnwrapKey }
            };
            keyClient.CreateKey(keyName, KeyType.Rsa, createRsaKeyOptions);
            var underTest = new AzureKeyVaultKeyRepository(keyClient, credential, keyName, GetCryptographyClientOptions());
            
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
            var secretClient = new SecretClient(new Uri("https://localhost:8443/"), credential, GetSecretClientOptions());
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

        private KeyClientOptions GetKeyClientOptions()
        {
            var options = new KeyClientOptions();
            DisableSslValidationOnClientOptions(options);
            return options;
        }

        private SecretClientOptions GetSecretClientOptions()
        {
            var options = new SecretClientOptions();
            DisableSslValidationOnClientOptions(options);
            return options;
        }

        private CryptographyClientOptions GetCryptographyClientOptions()
        {
            var options = new CryptographyClientOptions();
            DisableSslValidationOnClientOptions(options);
            return options;
        }

        private void DisableSslValidationOnClientOptions(ClientOptions options)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            options.Transport = new HttpClientTransport(clientHandler);
        }
    }
}