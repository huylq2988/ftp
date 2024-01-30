using FTP.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTP.TestEncrypt
{
    class Program
    {
        //static string originalPath = @"D:\FTP\Data\23565221102020_3b53d63c.txt";
        static string originalPath = @"D:\FTP\Data\23545321102020_1a2f6b62.txt";
        //static string decryptPath = @"D:\FTP\Data\23574121102020_3749a138.txt";
        static void Main(string[] args)
        {
            string originalContent = "";
            using (StreamReader sr = new StreamReader(originalPath))
            {
                originalContent = sr.ReadToEnd();
            }
            string decryptContent = Helper.EncryptRSA(Helper.PassCode, originalContent);
            string originalContent2 = Helper.DecryptRSA(Helper.PassCode, decryptContent.Replace("\0", string.Empty));
        }
    }
}
