using System;
using System.Security.Cryptography;
using System.Text;

namespace Encrypt_text_with_password
{
    class Program
    {
        static string decryptText(string Text, string Password)
        {
            byte[] data = Convert.FromBase64String(Text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Password));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    try
                    {
                        ICryptoTransform transform = tripDes.CreateDecryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        return UTF8Encoding.UTF8.GetString(results);
                    }
                    catch
                    {
                        return "Decryption error!";
                    }
                }
            }
        }

        static string encryptText(string Text, string Password)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(Text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(Password));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    try
                    {
                        ICryptoTransform transform = tripDes.CreateEncryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        return Convert.ToBase64String(results, 0, results.Length);
                    }
                    catch
                    {
                        return "Encryption failed!";
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            // A text can be encrypted with a password
            Console.WriteLine(encryptText("Can you read this?", "!pass12#"));

            // and be decrypted with the same password
            Console.WriteLine(decryptText(encryptText("Can you read this?", "!pass12#"), "!pass12#"));

            Console.ReadLine(); 
        }

    }
}
