using System;
using System.Management;
using System.Collections.Generic;
using System.Windows;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Management.Instrumentation;

namespace SFVideoPlayer
{
    public static class GenerateId
    {

        public const string _wvpdemo = "WebVertPlayerDemonstration";
        public const string _wvvinkr = "WebVinkRecorder";

        // WEBVERT Compound keys
        public const string _wvplite = "WebVertPlayerLite";
        public const string _wvpprof = "WebVertPlayerProfessional";
        public const string _wvpcomm = "WebVertPlayerCommercial";
        public const string _wvpcorp = "WebVertPlayerCorporate";
        public const string _wvpaudi = "WebVertPlayerAudioMate";
        public const string _amseckey = "AudioMateSecurityKey";

        // --------------------------------------------
        private static string makeKey(string forwhat)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            string md5 = (MD5SUM(enc.GetBytes(forwhat)));
            string sb = null;
            for (int itemCounter = 0; itemCounter < 20; itemCounter += 4)
            {
                sb += md5.Substring(itemCounter, 4);
                if (itemCounter < 16) sb += "-";
            }
            md5 = sb.ToString().ToUpper();
            return md5;
        }

        // -------------------------------------------
        public static string getKey(string forwhat)
        {
            switch (forwhat.ToString())
            {
                // General Product Base Key
                case _wvpdemo:
                case _wvvinkr:
                    return makeKey(forwhat + GetMACAddress());

                // WEBVERT Compound keys ( Features )
                case _wvplite:
                case _wvpprof:
                case _wvpcomm:
                case _wvpcorp:
                case _wvpaudi:
                case _amseckey:
                    return makeKey(forwhat + getKey(_wvpdemo));

                default:
                    return "E R R O R   I N   K E Y";
            }
        }

        // ------------------------------------------
        private static string GetMACAddress()
        {
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("Select * From Win32_processor");
                ManagementObjectCollection moc = mos.Get();
                string ProcessorID = "";
                foreach (ManagementObject mo in moc)
                {
                    ProcessorID = (string)mo["ProcessorID"];
                }
                ProcessorID = ProcessorID + "" + StaticClass.TokenId;
                return ProcessorID;
            }
            catch (Exception ex)
            {
                return StaticClass.TokenId;
            }
            //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //ManagementObjectCollection moc = mc.GetInstances();

            //List<string> Macs = new List<string>();

            //foreach (ManagementObject mo in moc)
            //{
            //    if ((bool)mo["IPEnabled"] == true)
            //    {
            //        Macs.Add(mo["MacAddress"].ToString().Replace(":", ""));
            //    }
            //    mo.Dispose();
            //}

            //if (Macs != null)
            //    return Macs[0];
            //else
            //    throw (new Exception("This Program Requires a Network Interface"));
        }

        // -----------------------------------------------------------------
        private static string MD5SUM(byte[] FileOrText) //Output: String<-> Input: Byte[] //
        {
            return BitConverter.ToString(new
                MD5CryptoServiceProvider().ComputeHash(FileOrText)).Replace("-", "").ToLower();
        }
    }
}
