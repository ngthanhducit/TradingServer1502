using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace TradingServer.Model
{
    public static class ValidateCheck
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetEncodedString(string input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string strReturn = "";
            byte[] hash;
            ASCIIEncoding enc = new ASCIIEncoding();
            byte[] buffer = enc.GetBytes(input);
            hash = md5.ComputeHash(buffer);

            foreach (byte b in hash)
            {
                strReturn += b.ToString();
            }
            string password = strReturn.Substring(0, 20);
            return password;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Encrypt(string password)
        {
            byte[] utfData = UTF8Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes("Element5");
            string encryptedString = string.Empty;
            using (AesManaged aes = new AesManaged())
            {
                Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, saltBytes);

                aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
                aes.KeySize = aes.LegalKeySizes[0].MaxSize;
                aes.Key = rfc.GetBytes(aes.KeySize / 8);
                aes.IV = rfc.GetBytes(aes.BlockSize / 8);

                using (ICryptoTransform encryptTransform = aes.CreateEncryptor())
                {
                    using (MemoryStream encryptedStream = new MemoryStream())
                    {
                        using (CryptoStream encryptor =
                            new CryptoStream(encryptedStream, encryptTransform, CryptoStreamMode.Write))
                        {
                            encryptor.Write(utfData, 0, utfData.Length);
                            encryptor.Flush();
                            encryptor.Close();

                            byte[] encryptBytes = encryptedStream.ToArray();
                            encryptedString = Convert.ToBase64String(encryptBytes);
                            encryptedString = encryptedString.Substring(0, 20);
                        }
                    }
                }
            }
            return encryptedString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Decrypt(string password)
        {
            //byte[] encryptBytes = UTF8Encoding.UTF8.GetBytes(input);
            byte[] encryptBytes = Convert.FromBase64String(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes("Element5");

            // Our symmetric encryption algorithm
            AesManaged aes = new AesManaged();

            // We're using the PBKDF2 standard for password-based key generation
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes("Element5", saltBytes);

            // Setting our parameters
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;

            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);

            // Now, decryption
            ICryptoTransform decryptTrans = aes.CreateDecryptor();

            // Output stream, can be also a FileStream
            MemoryStream decryptStream = new MemoryStream();
            CryptoStream decryptor = new CryptoStream(decryptStream, decryptTrans, CryptoStreamMode.Write);

            decryptor.Write(encryptBytes, 0, encryptBytes.Length);
            decryptor.Flush();
            decryptor.Close();

            // Showing our decrypted content
            byte[] decryptBytes = decryptStream.ToArray();
            string decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);

            return decryptedString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Lenght"></param>
        /// <returns></returns>
        public static string RandomString(int Length)
        {
            Char[] charArray = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'u', 'v', 
                                            'w', 'x', 'y', 'z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

            String result = "";
            Random r = new Random();
            for (int i = 0; i < Length; i++)
            {
                int rand = r.Next(i, charArray.Length - i);
                result += "" + charArray[rand];
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomKeyLogin(int length)
        {
            string result = string.Empty;
            bool isExist = true;
            while (isExist)
            {
                isExist = false;
                result = Model.ValidateCheck.RandomString(length);
                if (Business.Market.InvestorOnline != null && Business.Market.InvestorOnline.Count > 0)
                {
                    for (int i = 0; i < Business.Market.InvestorOnline.Count; i++)
                    {
                        if (Business.Market.InvestorOnline[i].LoginKey == result)
                        {
                            result = Model.ValidateCheck.RandomString(length);
                            isExist = true;

                            break;
                        }
                    }
                }
                else
                {
                    isExist = false;
                }
            }

            return result;
        }
    }
}
