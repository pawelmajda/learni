using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Learni.API.Helpers
{
    public class Sha1Helper
    {
        public static string CalculateHash(string text, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            byte[] buffer = encoding.GetBytes(text);
            SHA1CryptoServiceProvider cryptoTransformSha1 = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }
    }
}