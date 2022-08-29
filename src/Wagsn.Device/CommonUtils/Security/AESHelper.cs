using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DW.CQMS.Common
{
    public class AESHelper
    {
        public const string DEFAULT_KEY = "erp2.0xbjs&!_WC1";

        /// <summary>AES加密</summary>
        /// <param name="text">明文</param>
        /// <param name="key">密钥,长度为16的字符串</param>
        /// <returns>密文</returns>
        public static string EncryptAES(string text, string key)
        {
            using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
            {
                rijndaelCipher.Mode = CipherMode.ECB;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                rijndaelCipher.KeySize = 128;
                rijndaelCipher.BlockSize = 128;
                byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[16];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                    len = keyBytes.Length;
                System.Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                //rijndaelCipher.IV = Encoding.UTF8.GetBytes(iv);
                ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(text);
                byte[] cipherBytes = transform.TransformFinalBlock(plainText, 0, plainText.Length);

                //return Encoding.UTF8.GetString(cipherBytes);
                return Convert.ToBase64String(cipherBytes);
            }
        }

        /// <summary>AES解密</summary>
        /// <param name="text">密文</param>
        /// <param name="key">密钥,长度为16的字符串</param>
        /// <returns>明文</returns>
        public static string DecryptAES(string text, string key)
        {
            using (RijndaelManaged rijndaelCipher = new RijndaelManaged())
            {
                rijndaelCipher.Mode = CipherMode.ECB;
                rijndaelCipher.Padding = PaddingMode.PKCS7;
                rijndaelCipher.KeySize = 128;
                rijndaelCipher.BlockSize = 128;
                byte[] encryptedData = Convert.FromBase64String(text);
                //byte[] encryptedData = Encoding.UTF8.GetBytes(text);
                byte[] pwdBytes = System.Text.Encoding.UTF8.GetBytes(key);
                byte[] keyBytes = new byte[16];
                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                    len = keyBytes.Length;
                System.Array.Copy(pwdBytes, keyBytes, len);
                rijndaelCipher.Key = keyBytes;
                //rijndaelCipher.IV = Encoding.UTF8.GetBytes(iv);
                ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
                byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                return Encoding.UTF8.GetString(plainText);
            }
        }

    }
}
