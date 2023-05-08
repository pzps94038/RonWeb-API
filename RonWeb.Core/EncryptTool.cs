using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace RonWeb.Core
{
    public class AESConvertDataException : Exception
    {
        public AESConvertDataException() : base("AES轉換資料失敗") { }
    }

    public class RsaKey
    {
        public string PublicKey { get; set; } = string.Empty;
        public string PrivateKey { get; set; } = string.Empty;
        public RsaKey(string publicKey, string privateKey)
        {
            this.PublicKey = publicKey;
            this.PrivateKey = privateKey;
        }
    }

    public class EncryptTool
	{
        /// <summary>
        /// 產生RsaKey
        /// </summary>
        /// <returns></returns>
        public static RsaKey GenerateRsaKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                // 生成public key
                var publicKeyParam = rsa.ExportParameters(false);
                string publicKey = Convert.ToBase64String(publicKeyParam.Modulus!);
                var privateKeyParam = rsa.ExportParameters(true);
                string privateKey = Convert.ToBase64String(privateKeyParam.D!);
                return new RsaKey(publicKey, privateKey);
            }
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rsaKey"></param>
        /// <returns></returns>
        public static string RsaEncrypt(string data, RsaKey rsaKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = Convert.FromBase64String(rsaKey.PublicKey),
            });
            rsa.ImportParameters(new RSAParameters
            {
                D = Convert.FromBase64String(rsaKey.PrivateKey),
            });
            var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(data), false);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// RSA加密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rsaKey"></param>
        /// <returns></returns>
        public static string RsaEncrypt<T>(T data, RsaKey rsaKey)
        {
            string plainText = JsonConvert.SerializeObject(data);
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = Convert.FromBase64String(rsaKey.PublicKey),
            });
            rsa.ImportParameters(new RSAParameters
            {
                D = Convert.FromBase64String(rsaKey.PrivateKey),
            });
            var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), false);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rsaKey"></param>
        /// <returns></returns>
        public static string RsaDecrypt(string data, RsaKey rsaKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = Convert.FromBase64String(rsaKey.PublicKey),
            });
            rsa.ImportParameters(new RSAParameters
            {
                D = Convert.FromBase64String(rsaKey.PrivateKey),
            });
            var encryptedData = rsa.Decrypt(Encoding.UTF8.GetBytes(data), false);
            return Encoding.UTF8.GetString(encryptedData);
        }

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="data"></param>
        /// <param name="rsaKey"></param>
        /// <returns></returns>
        public static T RsaDecrypt<T>(string data, RsaKey rsaKey)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(new RSAParameters
            {
                Modulus = Convert.FromBase64String(rsaKey.PublicKey),
            });
            rsa.ImportParameters(new RSAParameters
            {
                D = Convert.FromBase64String(rsaKey.PrivateKey),
            });
            var encryptedData = rsa.Decrypt(Encoding.UTF8.GetBytes(data), false);
            var result = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(encryptedData));
            if (result == null)
                throw new AESConvertDataException();
            else
                return result;
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SHA256Encrypt(string text)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(text);
            var result = SHA256.Create().ComputeHash(plainBytes);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="plainText">資料文字</param>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <returns>加密後文字</returns>
        /// <exception cref="Exception"></exception>
        public static string AESEncrypt(string plainText, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                aesAlg.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            swEncrypt.Write(plainText);
                        string encrypt = Convert.ToBase64String(msEncrypt.ToArray());
                        return encrypt;
                    }
                }
            }
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="cipherText">資料文字</param>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <returns></returns>
        public static string AESDecrypt(string cipherText, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                var cipherByte = Convert.FromBase64String(cipherText);
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                aesAlg.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherByte))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// ModelAES加密
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="data">資料</param>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <returns>加密後文字</returns>
        public static string AESEncrypt<T>(T data, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                string plainText = JsonConvert.SerializeObject(data);
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                aesAlg.Mode = CipherMode.CBC;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            swEncrypt.Write(plainText);
                        string encrypt = Convert.ToBase64String(msEncrypt.ToArray());
                        return encrypt;
                    }
                }
            }
        }

        /// <summary>
        /// AES轉換為Model
        /// </summary>
        /// <typeparam name="T">Model類型</typeparam>
        /// <param name="cipherText">加密文字</param>
        /// <param name="key">key</param>
        /// <param name="iv">iv</param>
        /// <returns>Model</returns>
        /// <exception cref="AESConvertDataException">轉換失敗</exception>
        public static T AESDecrypt<T>(string cipherText, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                var cipherByte = Convert.FromBase64String(cipherText);
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);
                aesAlg.Mode = CipherMode.CBC;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(cipherByte))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            var result = JsonConvert.DeserializeObject<T>(srDecrypt.ReadToEnd());
                            if (result == null)
                                throw new AESConvertDataException();
                            else
                                return result;
                        }

                    }
                }
            }
        }
    }
}

