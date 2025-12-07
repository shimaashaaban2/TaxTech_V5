using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tax_Tech.Classes
{
    public class Encryption
    {
        private Encryption()
        {
        }
        private static Encryption _instance;
        public static Encryption GetEncryption()
        {
            if (_instance == null)
            {
                _instance = new Encryption();
            }
            return _instance;
        }
        private static readonly string StaticKey = "?D(G-KaPdSgVkYp3s6v9y$B&E)H@MbQe";

        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

        // This constant is used to determine the keysize of the encryption algorithm.
        private const int keysize = 256;
 
        public string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (PasswordDeriveBytes password = new PasswordDeriveBytes(StaticKey, null))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        } 
        public bool NullString(string ToCheck)
        {
            if (ToCheck != null)
            {
                if (ToCheck.Trim().Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }

        }
    }
}