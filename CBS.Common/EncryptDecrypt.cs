using System;
using System.Data;
using System.Configuration;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace CBS.Common
{
    public class EncryptDecryptClass
    {
        private static string Phrase = "L0v3BTN43v3rp07@Pr1m@";
        private static string HashAlgorithm = "MD5";
        private static int Iterations = 2;
        private static string Vector = "@1B2c3D4e5F6g7H8";
        private static int Ukur = 256;
        private static string sEncryptKey = "==P@22m0r4isM1n3=="; //'Should be minimum 8 characters

        static EncryptDecryptClass()
        {
            if (ConfigurationManager.AppSettings["Phrase"] != null)
                Phrase = ConfigurationManager.AppSettings["Phrase"];
            if (ConfigurationManager.AppSettings["Iterations"] != null)
                try
                {
                    Iterations = int.Parse(ConfigurationManager.AppSettings["Iterations"]);
                }
                catch { Iterations = 2; }
            if (ConfigurationManager.AppSettings["Vector"] != null)
                Vector = ConfigurationManager.AppSettings["Vector"];
            if (ConfigurationManager.AppSettings["Ukur"] != null)
                try
                {
                    Ukur = int.Parse(ConfigurationManager.AppSettings["Ukur"]);
                }
                catch { Ukur = 256; }
            if (ConfigurationManager.AppSettings["sEncryptKey"] != null)
                sEncryptKey = ConfigurationManager.AppSettings["sEncryptKey"];
        }

        protected static string EncryptMD5(string Textq, string SValue)
        {
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null;
            string strReturn = null;
            try
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(Vector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(SValue);
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(Textq);

                Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(Phrase, saltValueBytes, Iterations);

                byte[] keyBytes;
                int intKeySize = Ukur / 8;
                keyBytes = password.GetBytes(intKeySize);

                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);

                memoryStream = new MemoryStream();
                cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();

                byte[] cipherTextBytes = memoryStream.ToArray();

                cryptoStream.Close();
                cryptoStream.Dispose();
                memoryStream.Close();
                memoryStream.Dispose();

                strReturn = Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception ex)
            {
                //cleans up
                try
                {
                    cryptoStream.Close();
                    cryptoStream.Dispose();
                }
                catch { }
                try
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
                catch { }

                //Inform the user that an exception was raised.
                throw ex;
            }

            return strReturn;
        }

        protected static string DecryptMD5(string cipherText, string SValue)
        {
            string strReturn = null;
            MemoryStream memoryStream = null;
            CryptoStream cryptoStream = null; 
            try
            {
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(Vector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(SValue);
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(Phrase, saltValueBytes, Iterations);

                byte[] keyBytes;
                int intKeySize = Ukur / 8;
                keyBytes = password.GetBytes(intKeySize);

                RijndaelManaged symmetricKey = new RijndaelManaged();
                symmetricKey.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                memoryStream = new MemoryStream(cipherTextBytes);
                cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                cryptoStream.Close();
                cryptoStream.Dispose();
                memoryStream.Close();
                memoryStream.Dispose();

                strReturn = Encoding.UTF8.GetString(plainTextBytes, 0, plainTextBytes.Length);
            }
            catch (Exception ex)
            {
                //cleans up
                try
                {
                    cryptoStream.Close();
                    cryptoStream.Dispose();
                }
                catch { }
                try
                {
                    memoryStream.Close();
                    memoryStream.Dispose();
                }
                catch { }

                //Inform the user that an exception was raised.
                throw ex;
            }

            return strReturn;
        }

        public static string Encrypt(string text)
        {
            return EncryptMD5(text, sEncryptKey);
        }

        public static string Decrypt(string text)
        {
            return DecryptMD5(text, sEncryptKey);
        }

        
    }

    public static class EncryptDecrypt
    {
        private static byte[] rgbIV = Encoding.ASCII.GetBytes("oiwehioplsiajsdv");
        private static byte[] key = Encoding.ASCII.GetBytes("kajsdoaioalptljsadrq2jflkasd23jd");

        public static byte[] myIV
        {
            set { rgbIV = value; }
            get { return rgbIV; }
        }

        public static byte[] myKey
        {
            set { key = value; }
            get { return key; }
        }

        public static string decryptConnStr(string encryptedConnStr)
        {
            string connStr, encpwd, decpwd = "";
            int pos1, pos2;
            pos1 = encryptedConnStr.IndexOf("pwd=");
            pos2 = encryptedConnStr.IndexOf(";", pos1 + 4);
            encpwd = encryptedConnStr.Substring(pos1 + 4, pos2 - pos1 - 4);
            decpwd = DecryptString(encpwd);
            connStr = encryptedConnStr.Replace(encpwd, decpwd);
            return connStr;
        }

        public static string EncryptString(string ClearText)
        {
            string retval;
            byte[] clearTextBytes = Encoding.UTF8.GetBytes(ClearText);

            System.Security.Cryptography.SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write))
                {
                    cs.Write(clearTextBytes, 0, clearTextBytes.Length);

                    cs.Close();
                }
                retval = Convert.ToBase64String(ms.ToArray());
            }

            return retval;
        }

        public static string DecryptString(string EncryptedText)
        {
            string retval;
            byte[] encryptedTextBytes = Convert.FromBase64String(EncryptedText);

            using (MemoryStream ms = new MemoryStream())
            {
                SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();
                using (CryptoStream cs = new CryptoStream(ms, rijn.CreateDecryptor(key, rgbIV), CryptoStreamMode.Write))
                {
                    cs.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);

                    cs.Close();
                }
                retval = Encoding.UTF8.GetString(ms.ToArray());
            }

            return retval;
        }
    }
}
