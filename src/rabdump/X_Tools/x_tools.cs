using System;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace X_Tools
{
    static class XTools
    {
        static public bool IsFilenameValid(string inputFileName)
        {
            Match m = Regex.Match(inputFileName, @"[\\\/\:\*\?\" + Convert.ToChar(34) + @"\<\>\|]");
            return !(m.Success);
        }
        static public string FormatBytes(double bytes)
        {
            string[] suffix = { "B", "KB", "MB", "GB", "TB", "PB" };
            int i;
            double dblSByte = 0;
            for (i = 0; (int)(bytes / 1024) > 0; i++, bytes /= 1024)
            {
                dblSByte = bytes / 1024.0;
            }
            return String.Format("{0:0.00} {1}", dblSByte, suffix[i]);
        }

        public static string SafeFileName(string fileName, string replacer)
        {
            return Regex.Replace(fileName, "[" + Regex.Escape(new string(Path.GetInvalidFileNameChars())) + "]", replacer, RegexOptions.Compiled);
        }

        public static string SafeFileName(string fileName)
        {
            return SafeFileName(fileName, string.Empty);
        }

        public static bool VerifyMD5(string file, string hash)
        {
            if (File.Exists(file))
            {
                string computed = GetFileMD5(file);
                if (computed == hash)
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
                return false;
            }
        }

        public static string GetFileMD5(string file)
        {
            MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();
            if (File.Exists(file))
            {
                FileStream fs = File.OpenRead(file);
                byte[] fileHash = csp.ComputeHash(fs);
                fs.Close();

                return BitConverter.ToString(fileHash).Replace("-", "").ToLower();
            }
            else
            {
                return "nofile";
            }
        }

        // readStream is the stream you need to read
        // writeStream is the stream you want to write to
/*
        static public void ReadWriteStream(Stream readStream, Stream writeStream)
        {
            const int length = 256;
            Byte[] buffer = new Byte[length];
            int bytesRead = readStream.Read(buffer, 0, length);
            // write the required bytes
            while (bytesRead > 0)
            {
                writeStream.Write(buffer, 0, bytesRead);
                bytesRead = readStream.Read(buffer, 0, length);
            }
            readStream.Close();
            writeStream.Close();
        }
*/
    }
}
