using System;
using System.Text;
using System.Security.Cryptography;
namespace Encryptor
{
    class FileEncryption
    {
        public static string ReadFile(string path)
        {
            string text = File.ReadAllText(path, new UTF8Encoding(true));
            return text;
        }


        public static string Encrypt(string contents, string key)
        {
            byte[] iv = new byte[16];
            byte[] array;

            UTF8Encoding encoding = new UTF8Encoding(true);
            using (Aes aes = Aes.Create())
            {
                aes.Key = encoding.GetBytes(key);
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(contents);
                        }
                        array = memoryStream.ToArray();
                    }
                }

            }
            return Convert.ToBase64String(array);
        }

        public static string Decrypt(string contents, string key){
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(contents);

            UTF8Encoding encoding = new UTF8Encoding(true);
            using (Aes aes = Aes.Create())
            {
                aes.Key = encoding.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, transform, CryptoStreamMode.Read))
                    {
                        using (StreamReader stream = new StreamReader((Stream)cryptoStream)){
                            return stream.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}

