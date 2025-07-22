using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decrypt_GB
{
    internal class EscureCryptoHelper
    {
        private readonly byte[] key;
        private readonly byte[] iv;

        public EscureCryptoHelper(string secretKey)
        {
            // Gera uma chave e IV seguros a partir do secretKey (pode ser armazenado em configurações seguras)
            using (SHA256 sha256 = SHA256.Create())
            {
                key = sha256.ComputeHash(Encoding.UTF8.GetBytes(secretKey));
                iv = new byte[16]; // Tamanho padrão para AES IV
                Array.Copy(key, iv, iv.Length); // Deriva o IV dos primeiros bytes da chave
            }
        }

        public string Encrypt(string plainText) //Método para criptografar!
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC; // Usar CBC é mais seguro que ECB
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream memoryStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedText) // Método para descriptografar!
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText)))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
    }
}
