using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DVRMobile.Base
{
    public class CryptoAesAPI
    {
        private static string EncryptionKey = "asdwedsfw12234fdgaasd123123asdsa";
        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        #region "Convert"
        public static String ConvertUTF8ToAscII(String utf8Text)
        {
            if (string.IsNullOrWhiteSpace(utf8Text))
                return string.Empty;

            StringBuilder sb = new StringBuilder(utf8Text);
            sb.Replace("á", "za1a1zv");
            sb.Replace("à", "za1a2zv");
            sb.Replace("ả", "za1a3zv");
            sb.Replace("ã", "za1a4zv");
            sb.Replace("ạ", "za1a5zv");
            sb.Replace("â", "za1a6zv");
            sb.Replace("ấ", "za1a61zv");
            sb.Replace("ầ", "za1a62zv");
            sb.Replace("ẩ", "za1a63zv");
            sb.Replace("ẫ", "za1a64zv");
            sb.Replace("ậ", "za1a65zv");
            sb.Replace("ă", "za1a8zv");
            sb.Replace("ắ", "za1a81zv");
            sb.Replace("ằ", "za1a82zv");
            sb.Replace("ẳ", "za1a83zv");
            sb.Replace("ẵ", "za1a84zv");
            sb.Replace("ặ", "za1a85zv");
            sb.Replace("đ", "za2d9zv");
            sb.Replace("é", "za3e1zv");
            sb.Replace("è", "za3e2zv");
            sb.Replace("ẻ", "za3e3zv");
            sb.Replace("ẽ", "za3e4zv");
            sb.Replace("ẹ", "za3e5zv");
            sb.Replace("ê", "za3e6zv");
            sb.Replace("ế", "za3e61zv");
            sb.Replace("ề", "za3e62zv");
            sb.Replace("ể", "za3e63zv");
            sb.Replace("ễ", "za3e64zv");
            sb.Replace("ệ", "za3e65zv");
            sb.Replace("ó", "za4o1zv");
            sb.Replace("ò", "za4o2zv");
            sb.Replace("ỏ", "za4o3zv");
            sb.Replace("õ", "za4o4zv");
            sb.Replace("ọ", "za4o5zv");
            sb.Replace("ô", "za4o6zv");
            sb.Replace("ố", "za4o61zv");
            sb.Replace("ồ", "za4o62zv");
            sb.Replace("ổ", "za4o63zv");
            sb.Replace("ỗ", "za4o64zv");
            sb.Replace("ộ", "za4o65zv");
            sb.Replace("ơ", "za4o7zv");
            sb.Replace("ớ", "za4o71zv");
            sb.Replace("ờ", "za4o72zv");
            sb.Replace("ở", "za4o73zv");
            sb.Replace("ỡ", "za4o74zv");
            sb.Replace("ợ", "za4o75zv");
            sb.Replace("ú", "za5u1zv");
            sb.Replace("ù", "za5u2zv");
            sb.Replace("ủ", "za5u3zv");
            sb.Replace("ũ", "za5u4zv");
            sb.Replace("ụ", "za5u5zv");
            sb.Replace("ư", "za5u7zv");
            sb.Replace("ứ", "za5u71zv");
            sb.Replace("ừ", "za5u72zv");
            sb.Replace("ử", "za5u73zv");
            sb.Replace("ữ", "za5u74zv");
            sb.Replace("ự", "za5u75zv");
            sb.Replace("í", "za6i1zv");
            sb.Replace("ì", "za6i2zv");
            sb.Replace("ỉ", "za6i3zv");
            sb.Replace("ĩ", "za6i4zv");
            sb.Replace("ị", "za6i5zv");
            sb.Replace("ý", "za7y1zv");
            sb.Replace("ỳ", "za7y2zv");
            sb.Replace("ỷ", "za7y3zv");
            sb.Replace("ỹ", "za7y4zv");
            sb.Replace("ỵ", "za7y5zv");

            sb.Replace("Á", "za8a1zv");
            sb.Replace("À", "za8a2zv");
            sb.Replace("Ả", "za8a3zv");
            sb.Replace("Ã", "za8a4zv");
            sb.Replace("Ạ", "za8a5zv");
            sb.Replace("Â", "za8a6zv");
            sb.Replace("Ấ", "za8a61zv");
            sb.Replace("Ầ", "za8a62zv");
            sb.Replace("Ẩ", "za8a63zv");
            sb.Replace("Ẫ", "za8a64zv");
            sb.Replace("Ậ", "za8a65zv");
            sb.Replace("Ă", "za8a8zv");
            sb.Replace("Ắ", "za8a81zv");
            sb.Replace("Ằ", "za8a82zv");
            sb.Replace("Ẳ", "za8a83zv");
            sb.Replace("Ẵ", "za8a84zv");
            sb.Replace("Ặ", "za8a85zv");
            sb.Replace("Đ", "za9d9zv");
            sb.Replace("É", "za10e1zv");
            sb.Replace("È", "za10e2zv");
            sb.Replace("Ẻ", "za10e3zv");
            sb.Replace("Ẽ", "za10e4zv");
            sb.Replace("Ẹ", "za10e5zv");
            sb.Replace("Ê", "za10e6zv");
            sb.Replace("Ế", "za10e61zv");
            sb.Replace("Ề", "za10e62zv");
            sb.Replace("Ể", "za10e63zv");
            sb.Replace("Ễ", "za10e64zv");
            sb.Replace("Ệ", "za10e65zv");
            sb.Replace("Ó", "za11o1zv");
            sb.Replace("Ò", "za11o2zv");
            sb.Replace("Ỏ", "za11o3zv");
            sb.Replace("Õ", "za11o4zv");
            sb.Replace("Ọ", "za11o5zv");
            sb.Replace("Ô", "za11o6zv");
            sb.Replace("Ố", "za11o61zv");
            sb.Replace("Ồ", "za11o62zv");
            sb.Replace("Ổ", "za11o63zv");
            sb.Replace("Ỗ", "za11o64zv");
            sb.Replace("Ộ", "za11o65zv");
            sb.Replace("Ơ", "za11o7zv");
            sb.Replace("Ớ", "za11o71zv");
            sb.Replace("Ờ", "za11o72zv");
            sb.Replace("Ở", "za11o73zv");
            sb.Replace("Ỡ", "za11o74zv");
            sb.Replace("Ợ", "za11o75zv");
            sb.Replace("Ú", "za12u1zv");
            sb.Replace("Ù", "za12u2zv");
            sb.Replace("Ủ", "za12u3zv");
            sb.Replace("Ũ", "za12u4zv");
            sb.Replace("Ụ", "za12u5zv");
            sb.Replace("Ư", "za12u7zv");
            sb.Replace("Ứ", "za12u71zv");
            sb.Replace("Ừ", "za12u72zv");
            sb.Replace("Ử", "za12u73zv");
            sb.Replace("Ữ", "za12u74zv");
            sb.Replace("Ự", "za12u75zv");
            sb.Replace("Í", "za13i1zv");
            sb.Replace("Ì", "za13i2zv");
            sb.Replace("Ỉ", "za13i3zv");
            sb.Replace("Ĩ", "za13i4zv");
            sb.Replace("Ị", "za13i5zv");
            sb.Replace("Ý", "za14y1zv");
            sb.Replace("Ỳ", "za14y2zv");
            sb.Replace("Ỷ", "za14y3zv");
            sb.Replace("Ỹ", "za14y4zv");
            sb.Replace("Ỵ", "za14y5zv");

            return sb.ToString();
        }

        public static String ConvertAscIIToUTF8(String asciiText)
        {
            if (string.IsNullOrWhiteSpace(asciiText))
                return string.Empty;

            StringBuilder sb = new StringBuilder(asciiText);
            sb.Replace("za1a1zv", "á");
            sb.Replace("za1a2zv", "à");
            sb.Replace("za1a3zv", "ả");
            sb.Replace("za1a4zv", "ã");
            sb.Replace("za1a5zv", "ạ");
            sb.Replace("za1a6zv", "â");
            sb.Replace("za1a61zv", "ấ");
            sb.Replace("za1a62zv", "ầ");
            sb.Replace("za1a63zv", "ẩ");
            sb.Replace("za1a64zv", "ẫ");
            sb.Replace("za1a65zv", "ậ");
            sb.Replace("za1a8zv", "ă");
            sb.Replace("za1a81zv", "ắ");
            sb.Replace("za1a82zv", "ằ");
            sb.Replace("za1a83zv", "ẳ");
            sb.Replace("za1a84zv", "ẵ");
            sb.Replace("za1a85zv", "ặ");
            sb.Replace("za2d9zv", "đ");
            sb.Replace("za3e1zv", "é");
            sb.Replace("za3e2zv", "è");
            sb.Replace("za3e3zv", "ẻ");
            sb.Replace("za3e4zv", "ẽ");
            sb.Replace("za3e5zv", "ẹ");
            sb.Replace("za3e6zv", "ê");
            sb.Replace("za3e61zv", "ế");
            sb.Replace("za3e62zv", "ề");
            sb.Replace("za3e63zv", "ể");
            sb.Replace("za3e64zv", "ễ");
            sb.Replace("za3e65zv", "ệ");
            sb.Replace("za4o1zv", "ó");
            sb.Replace("za4o2zv", "ò");
            sb.Replace("za4o3zv", "ỏ");
            sb.Replace("za4o4zv", "õ");
            sb.Replace("za4o5zv", "ọ");
            sb.Replace("za4o6zv", "ô");
            sb.Replace("za4o61zv", "ố");
            sb.Replace("za4o62zv", "ồ");
            sb.Replace("za4o63zv", "ổ");
            sb.Replace("za4o64zv", "ỗ");
            sb.Replace("za4o65zv", "ộ");
            sb.Replace("za4o7zv", "ơ");
            sb.Replace("za4o71zv", "ớ");
            sb.Replace("za4o72zv", "ờ");
            sb.Replace("za4o73zv", "ở");
            sb.Replace("za4o74zv", "ỡ");
            sb.Replace("za4o75zv", "ợ");
            sb.Replace("za5u1zv", "ú");
            sb.Replace("za5u2zv", "ù");
            sb.Replace("za5u3zv", "ủ");
            sb.Replace("za5u4zv", "ũ");
            sb.Replace("za5u5zv", "ụ");
            sb.Replace("za5u7zv", "ư");
            sb.Replace("za5u71zv", "ứ");
            sb.Replace("za5u72zv", "ừ");
            sb.Replace("za5u73zv", "ử");
            sb.Replace("za5u74zv", "ữ");
            sb.Replace("za5u75zv", "ự");
            sb.Replace("za6i1zv", "í");
            sb.Replace("za6i2zv", "ì");
            sb.Replace("za6i3zv", "ỉ");
            sb.Replace("za6i4zv", "ĩ");
            sb.Replace("za6i5zv", "ị");
            sb.Replace("za7y1zv", "ý");
            sb.Replace("za7y2zv", "ỳ");
            sb.Replace("za7y3zv", "ỷ");
            sb.Replace("za7y4zv", "ỹ");
            sb.Replace("za7y5zv", "ỵ");

            sb.Replace("za8a1zv", "Á");
            sb.Replace("za8a2zv", "À");
            sb.Replace("za8a3zv", "Ả");
            sb.Replace("za8a4zv", "Ã");
            sb.Replace("za8a5zv", "Ạ");
            sb.Replace("za8a6zv", "Â");
            sb.Replace("za8a61zv", "Ấ");
            sb.Replace("za8a62zv", "Ầ");
            sb.Replace("za8a63zv", "Ẩ");
            sb.Replace("za8a64zv", "Ẫ");
            sb.Replace("za8a65zv", "Ậ");
            sb.Replace("za8a8zv", "Ă");
            sb.Replace("za8a81zv", "Ắ");
            sb.Replace("za8a82zv", "Ằ");
            sb.Replace("za8a83zv", "Ẳ");
            sb.Replace("za8a84zv", "Ẵ");
            sb.Replace("za8a85zv", "Ặ");
            sb.Replace("za9d9zv", "Đ");
            sb.Replace("za10e1zv", "É");
            sb.Replace("za10e2zv", "È");
            sb.Replace("za10e3zv", "Ẻ");
            sb.Replace("za10e4zv", "Ẽ");
            sb.Replace("za10e5zv", "Ẹ");
            sb.Replace("za10e6zv", "Ê");
            sb.Replace("za10e61zv", "Ế");
            sb.Replace("za10e62zv", "Ề");
            sb.Replace("za10e63zv", "Ể");
            sb.Replace("za10e64zv", "Ễ");
            sb.Replace("za10e65zv", "Ệ");
            sb.Replace("za11o1zv", "Ó");
            sb.Replace("za11o2zv", "Ò");
            sb.Replace("za11o3zv", "Ỏ");
            sb.Replace("za11o4zv", "Õ");
            sb.Replace("za11o5zv", "Ọ");
            sb.Replace("za11o6zv", "Ô");
            sb.Replace("za11o61zv", "Ố");
            sb.Replace("za11o62zv", "Ồ");
            sb.Replace("za11o63zv", "Ổ");
            sb.Replace("za11o64zv", "Ỗ");
            sb.Replace("za11o65zv", "Ộ");
            sb.Replace("za11o7zv", "Ơ");
            sb.Replace("za11o71zv", "Ớ");
            sb.Replace("za11o72zv", "Ờ");
            sb.Replace("za11o73zv", "Ở");
            sb.Replace("za11o74zv", "Ỡ");
            sb.Replace("za11o75zv", "Ợ");
            sb.Replace("za12u1zv", "Ú");
            sb.Replace("za12u2zv", "Ù");
            sb.Replace("za12u3zv", "Ủ");
            sb.Replace("za12u4zv", "Ũ");
            sb.Replace("za12u5zv", "Ụ");
            sb.Replace("za12u7zv", "Ư");
            sb.Replace("za12u71zv", "Ứ");
            sb.Replace("za12u72zv", "Ừ");
            sb.Replace("za12u73zv", "Ử");
            sb.Replace("za12u74zv", "Ữ");
            sb.Replace("za12u75zv", "Ự");
            sb.Replace("za13i1zv", "Í");
            sb.Replace("za13i2zv", "Ì");
            sb.Replace("za13i3zv", "Ỉ");
            sb.Replace("za13i4zv", "Ĩ");
            sb.Replace("za13i5zv", "Ị");
            sb.Replace("za14y1zv", "Ý");
            sb.Replace("za14y2zv", "Ỳ");
            sb.Replace("za14y3zv", "Ỷ");
            sb.Replace("za14y4zv", "Ỹ");
            sb.Replace("za14y5zv", "Ỵ");

            return sb.ToString();
        }
        #endregion
    }
}
