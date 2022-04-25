using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace DaraSurvey.Core.Helpers
{
    public class Cryptography
    {
        static byte[] rgbIV = Encoding.ASCII.GetBytes("12s4f8hadk6fvla6");
        static byte[] key = Encoding.ASCII.GetBytes("2sc6hbs3lms5dv82sm-3dk8avn5db92c");

        // ----------------------

        public static string Encrypt(string clearText, bool urlEncode = false)
        {
            if (clearText == null)
                return null;

            byte[] clearTextBytes = Encoding.UTF8.GetBytes(clearText);

            SymmetricAlgorithm Algorithm = Aes.Create();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, Algorithm.CreateEncryptor(key, rgbIV), CryptoStreamMode.Write);

            cs.Write(clearTextBytes, 0, clearTextBytes.Length);

            cs.Close();

            var ba = ms.ToArray();
            return (urlEncode) ? HttpUtility.UrlEncode(ba) : Convert.ToBase64String(ba);
        }

        // --------------------------

        public static string Decrypt(string encryptedText, bool urlDecode = false)
        {
            if (encryptedText == null)
                return null;

            byte[] encryptedTextBytes = (urlDecode) ? HttpUtility.UrlDecodeToBytes(encryptedText) : Convert.FromBase64String(encryptedText);

            SymmetricAlgorithm Algorithm = Aes.Create();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, Algorithm.CreateDecryptor(key, rgbIV), CryptoStreamMode.Write);

            cs.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);

            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        // --------------------------

        public static string ReverseBytes(string text)
        {
            if (text == null)
                return null;
            if (text == string.Empty)
                return string.Empty;

            var chars = text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                chars[i] = (char)((chars[i] & 0xFFU) << 8 | (chars[i] & 0xFF00U) >> 8);

            return new string(chars);
        }
    }
}
