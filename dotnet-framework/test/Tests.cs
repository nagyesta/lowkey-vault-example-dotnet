using System;
using System.Net;
using Azure.Core;
using Azure.Security.KeyVault.Keys;
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
            DisableCertificateValidation();
            TokenCredential credential = new NoopCredentials();
            var keyClient = new KeyClient(new Uri("https://localhost:8443/"), credential);
            const string keyName = "rsa-key";
            var createRsaKeyOptions = new CreateRsaKeyOptions(keyName)
            {
                KeySize = 2048,
                KeyOperations = { KeyOperation.Encrypt, KeyOperation.Decrypt, KeyOperation.WrapKey, KeyOperation.UnwrapKey }
            };
            keyClient.CreateKey(keyName, KeyType.Rsa, createRsaKeyOptions);
            var underTest = new AzureKeyVaultKeyRepository(keyClient, credential, keyName);
            
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
            DisableCertificateValidation();
            TokenCredential credential = new NoopCredentials();
            var secretClient = new SecretClient(new Uri("https://localhost:8443/"), credential);
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

        /// <summary>
        /// Disables server certification callback.
        /// <br/>
        /// <b>WARNING: Do not use in production environments.</b>
        /// </summary>
        private static void DisableCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
    }
}