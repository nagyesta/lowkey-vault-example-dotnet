using System;
using System.Text;
using Azure.Core;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;

namespace src
{
    public class AzureKeyVaultKeyRepository
    {
        private readonly KeyClient _keyClient;
        private readonly string _keyName;
        private readonly TokenCredential _credential;
        private readonly CryptographyClientOptions _cryptographyClientOptions;

        public AzureKeyVaultKeyRepository(KeyClient keyClient, TokenCredential credential, string keyName, CryptographyClientOptions cryptographyClientOptions)
        {
            _keyClient = keyClient;
            _keyName = keyName;
            _credential = credential;
            _cryptographyClientOptions = cryptographyClientOptions;
        }

        public byte[] Encrypt(string clearText)
        {
            var key = _keyClient.GetKey(_keyName).Value;
            return new CryptographyClient(new Uri(key.Key.Id), _credential, _cryptographyClientOptions)
                .Encrypt(EncryptParameters.RsaOaep256Parameters(Encoding.UTF8.GetBytes(clearText)))
                .Ciphertext;
        }
        
        public string Decrypt(byte[] cipherText)
        {
            var key = _keyClient.GetKey(_keyName).Value;
            var decrypted = new CryptographyClient(new Uri(key.Key.Id), _credential, _cryptographyClientOptions)
                .Decrypt(DecryptParameters.RsaOaep256Parameters(cipherText))
                .Plaintext;
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
