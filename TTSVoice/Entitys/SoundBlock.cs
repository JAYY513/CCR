using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TTSVoice.Entitys
{
    public  class SoundBlock
    {
        public int Volume { get; set; } = 100;
        public int Rate { get; set; } = -3;
        public string Content { get; set; }
        public string Voice { get; set; }
        public override string ToString()
        {
            return System.Web.HttpUtility.UrlEncode(string.Format("{0}{1}{2}{3}", MD5Encrypt(this.Content), this.Rate, this.Voice, this.Volume));
        }
        public string MD5Encrypt(string password)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] hashedDataBytes;
            hashedDataBytes = md5Hasher.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(password));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            return tmp.ToString();//默认情况 
        }
    }
}
