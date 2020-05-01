using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Security.Cryptography;
using System.Windows.Forms;
namespace SFVideoPlayer
{
    public delegate void updateProgressDelegateEve();
    public delegate void CloseProgressDelegateEve();
    class clsSongCryptEvent
    {
        public event updateProgressDelegateEve updProg;
        public event CloseProgressDelegateEve closeprog;
        public MemoryStream getStream(int titleID)
        {
            // check if already converted
            string secName = Application.StartupPath + "\\so\\" + titleID + ".sec";    //   <<<<  please fill in      
            if (File.Exists(secName))
            {
                FileInfo f2 = new FileInfo(secName);
                //if (f2.Length > 1500000)
                //{
                return decrcfile(new Uri(secName, UriKind.Relative));
                // }
            }

            string oggName = Application.StartupPath + "\\so\\" + titleID + ".mp3";   //    <<<<<<< please fill in
            if (File.Exists(oggName))
            {
                FileInfo f2 = new FileInfo(Application.StartupPath + "\\so\\" + titleID.ToString() + ".mp3");
                if (f2.Length > 1500000)
                {
                    encrfile(new Uri(oggName, UriKind.Relative));
                    return getStream(titleID);
                }

            }
            return null;
        }
        public static void encrfile(Uri file)
        {
            byte[] ba = ReadByteArrayFromFile(file.ToString());
            byte[] enc = EncryptorEvent.Encrypt(ba, myKey());
            MemoryStream ms = new MemoryStream(enc);
            string FileName = file.ToString().Replace(".mp3", ".sec");
            SaveMemoryStream(ms, FileName);

            ms.Flush();
            ms.Close();
            ms.Dispose();
        }


        //=====================================================================================
        static bool FileInUse(string path)
        {
            FileStream fs = null;
            string __message;

            try
            {
                using (fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    //If required we can check for read/write by using fs.CanRead or fs.CanWrite
                }
                return false;
            }
            catch (IOException ex)
            {
                //check if message is for a File IO

                __message = ex.Message.ToString();
                if (__message.Contains("The process cannot access the file")) // dutch systems ?? need the 
                    return true;
                else
                    throw;
            }
            finally
            {
                try
                {
                    fs.Close();

                }
                catch
                {
                }
            }
        }


        //===================================================================
        private static void SaveMemoryStream(MemoryStream ms, string FileName)
        {
            FileStream outStream = File.OpenWrite(FileName);
            ms.WriteTo(outStream);
            outStream.Flush();
            outStream.Close();
            outStream.Dispose();
            ms.Flush();
            ms.Close();
            ms.Dispose();

        }

        //=====================================
        private MemoryStream decrcfile(Uri File)
        {
            byte[] ba = ReadByteArrayFromFile(File.ToString());
            byte[] dec = EncryptorEvent.DecryptEvent(ba, myKey());
            MemoryStream ms = new MemoryStream(dec);
            return ms;
        }

        //===================================================
        private static byte[] ReadByteArrayFromFile(string fileName)
        {
            byte[] buff = null;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(fileName).Length;
            buff = br.ReadBytes((int)numBytes);
            fs.Flush();
            fs.Close();
            fs.Dispose();
            br.Close();
            br.Dispose();
            return buff;

        }

        //====================
        private static string myKey()
        {
            return StaticClass.TokenId;
        }

    }

    class EncryptorEvent
    {
        public static byte[] Encrypt(byte[] input, string password)
        {
            try
            {
                TripleDESCryptoServiceProvider service = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                byte[] key = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                byte[] iv = md5.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Transform(input, service.CreateEncryptor(key, iv));
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }

        public static byte[] DecryptEvent(byte[] input, string password)
        {
            try
            {
                TripleDESCryptoServiceProvider service = new TripleDESCryptoServiceProvider();
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                byte[] key = md5.ComputeHash(Encoding.ASCII.GetBytes(password));
                byte[] iv = md5.ComputeHash(Encoding.ASCII.GetBytes(password));

                return Transform(input, service.CreateDecryptor(key, iv));
            }
            catch (Exception)
            {
                return new byte[0];
            }
        }

        public static string Encrypt(string text, string password)
        {
            byte[] input = Encoding.UTF8.GetBytes(text);
            byte[] output = Encrypt(input, password);
            return Convert.ToBase64String(output);
        }

        public static string Decrypt(string text, string password)
        {
            byte[] input = Convert.FromBase64String(text);
            byte[] output = DecryptEvent(input, password);
            return Encoding.UTF8.GetString(output);
        }

        private static byte[] Transform(byte[] input, ICryptoTransform CryptoTransform)
        {
            MemoryStream memStream = new MemoryStream();
            CryptoStream cryptStream = new CryptoStream(memStream, CryptoTransform, CryptoStreamMode.Write);

            cryptStream.Write(input, 0, input.Length);
            cryptStream.FlushFinalBlock();

            memStream.Position = 0;
            byte[] result = new byte[Convert.ToInt32(memStream.Length)];
            memStream.Read(result, 0, Convert.ToInt32(result.Length));

            memStream.Close();
            cryptStream.Close();

            return result;
        }
    }

}
