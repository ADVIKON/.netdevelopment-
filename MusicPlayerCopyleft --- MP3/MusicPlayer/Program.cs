using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.Sql;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Diagnostics;
using NetFwTypeLib;
namespace MusicPlayerCopyleft
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {

            try
            {

                try
                {
                    INetFwPolicy2 firewallPolicyUpdateDel = (INetFwPolicy2)Activator.CreateInstance(
                            Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    for (int i = 0; i <= 20; i++)
                    {
                        firewallPolicyUpdateDel.Rules.Remove("UpdateMusicPlayer");
                    }

                    INetFwPolicy2 firewallPolicyDel = (INetFwPolicy2)Activator.CreateInstance(
                        Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    for (int i = 0; i <= 20; i++)
                    {
                        firewallPolicyDel.Rules.Remove("MusicPlayer");
                    }


                    INetFwRule firewallRule = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                    firewallRule.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                    firewallRule.Description = "Allow notepad";
                    firewallRule.ApplicationName = Application.StartupPath + "\\MusicPlayerCopyleft.exe";
                    firewallRule.Enabled = true;
                    firewallRule.InterfaceTypes = "All";
                    firewallRule.Name = "MusicPlayer";

                    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                        Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    firewallPolicy.Rules.Add(firewallRule);

                    INetFwRule firewallRuleUpdate = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                    firewallRuleUpdate.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                    firewallRuleUpdate.Description = "Allow notepad";
                    firewallRuleUpdate.ApplicationName = Application.StartupPath + "\\UpdateMusicPlayerCopyleft.exe";
                    firewallRuleUpdate.Enabled = true;
                    firewallRuleUpdate.InterfaceTypes = "All";
                    firewallRuleUpdate.Name = "UpdateMusicPlayer";

                    INetFwPolicy2 firewallPolicyUpdate = (INetFwPolicy2)Activator.CreateInstance(
                        Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    firewallPolicyUpdate.Rules.Add(firewallRuleUpdate);

                }
                catch (Exception ex)
                {
                    goto GL;
                }

            GL:
                gblClass objMainClass = new gblClass();
                StaticClass.constr = new SqlConnection("Data Source=134.119.178.26;database=nusign;uid=sa;password=Jan@Server007;Connect Timeout=5000");
                StaticClass.constrOldDB = new SqlConnection("Data Source=134.119.178.26;database=nusign;uid=sa;password=Jan@Server007;Connect Timeout=5000");

                StaticClass.LocalCon.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\db.mdb;User Id=admin;Password=;";
                string str = "";
                string localCode = "";
                string filename = Application.StartupPath + "\\tid.amp";
                string textline = "";
                string ExpiryDamStatus = "";
                Int32 IsBlock = 0;
                Int32 IsSuspend = 0;
                Int32 LeftDamDays = 0;

                string strOpt = "";
                string proc = Process.GetCurrentProcess().ProcessName;
                Process[] processes = Process.GetProcessesByName(proc);



                if (processes.Length > 1)
                {
                    // MessageBox.Show("Application is already running", "Music Player");
                    return;
                }
                if (File.Exists(filename))
                {
                    System.IO.StreamReader objReader;
                    objReader = new System.IO.StreamReader(filename);
                    do
                    {
                        textline = textline + objReader.ReadLine();
                    } while (objReader.Peek() != -1);
                    objReader.Close();
                    if (objMainClass.CheckForInternetConnection() == false)
                    {
                        MessageBox.Show("Please check your Internet connection.", "Music Player");
                        return;
                    }


                    if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                    StaticClass.constr.Open();

                    strOpt = "select ISNULL(IsSanjivani,0) as Sanjivani, ISNULL(IsDam,0) as Dam , isnull(IsStream,0) as Stream , dealerCode, ISNULL(IsThai,0) as Thai, ISNULL(IsChina,0) as China, ISNULL(IsAdvt,0) as Advt, ISNULL(IsAdvtManual,0) as AdvtManual, ISNULL(IsBlockAdvt,0) as IsBlockAdvt, isnull(serviceid,0) as serviceid , UserId , isnull(cityid,0) as CityId, isnull(StateId,0) as StateId, isnull(CountryId,0) as CountryId from AMPlayerTokens where TokenID=" + textline;
                    DataSet dsOption = new DataSet();
                    dsOption = objMainClass.fnFillDataSet(strOpt);
                    if (dsOption.Tables[0].Rows.Count > 0)
                    {

                        StaticClass.DealerCode = dsOption.Tables[0].Rows[0]["DealerCode"].ToString();
                        StaticClass.TokenServiceId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["serviceid"]);

                        StaticClass.IsAdvtManual = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["AdvtManual"]);
                        StaticClass.IsAdvt = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["Advt"]);
                        StaticClass.IsBlockAdvt = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["IsBlockAdvt"]);

                        StaticClass.TokenUserId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["userid"]);

                        StaticClass.AdvtCityId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["CityId"]);
                        StaticClass.Stateid = Convert.ToInt32(dsOption.Tables[0].Rows[0]["Stateid"]);
                        StaticClass.CountryId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["CountryId"]);

                       // str = "spGetTokenExpiryStatus_Copyleft_Direct " + Convert.ToInt32(textline) + ", " + dsOption.Tables[0].Rows[0]["Dam"] + ", " + dsOption.Tables[0].Rows[0]["Sanjivani"] + ", " + dsOption.Tables[0].Rows[0]["Thai"] + ", " + dsOption.Tables[0].Rows[0]["China"] + ", " + dsOption.Tables[0].Rows[0]["Stream"];

                        str = "spGetTokenExpiryStatus_Copyright " + Convert.ToInt32(textline) + ", 1,  0 , 1";

                        DataSet dsExpire = new DataSet();
                        dsExpire = objMainClass.fnFillDataSet(str);
                       

                        ExpiryDamStatus = dsExpire.Tables[0].Rows[0]["ExpiryCopyrightStatus"].ToString();
                        LeftDamDays = Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftCopyrightDays"]);

                       // StaticClass.StreamExpiryMessage = dsExpire.Tables[0].Rows[0]["ExpiryStreamStatus"].ToString();
                      //  StaticClass.LeftStreamtDays = Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftStreamDays"]);

                        if (ExpiryDamStatus == "NoLic")
                        {
                            MessageBox.Show("!! Purchase the subscription of music player  !!", "Music Player");
                            Application.Exit();
                            return;
                        }
                        if (ExpiryDamStatus == "Yes")
                        {
                            MessageBox.Show("!! The license of your music player is expired. Please contact your dealer. !!" + Environment.NewLine + "Your player id: " + textline + "", "Music Player");
                            Application.Exit();
                            return;
                        }

                        if (ExpiryDamStatus != "NoLic" && LeftDamDays <= 10)
                        {
                            StaticClass.PlayerExpiryMessage = Convert.ToString(LeftDamDays) + " days left to renewal of subscription.";
                            StaticClass.IsDam= true;
                        }
                        else if (ExpiryDamStatus != "NoLic" && LeftDamDays == 0)
                        {
                            StaticClass.PlayerExpiryMessage = "Last day to renewal of subscription.";
                            StaticClass.IsDam = true;
                        }
                        else
                        {
                            StaticClass.IsDam = true;
                        }
                        if (ExpiryDamStatus == "Yes")
                        {
                            MessageBox.Show("!! Subscription is Expired.Please connect your vendor !! " + Environment.NewLine + "Your player id: " + textline + "", "Music Player");
                            Application.Exit();
                            return;
                        }

                         

                        // str = "spGetTokenExpiryStatus_Copyleft " + Convert.ToInt32(textline) + ", " + dsOption.Tables[0].Rows[0]["Dam"] + ", " + dsOption.Tables[0].Rows[0]["Sanjivani"] + ", " + dsOption.Tables[0].Rows[0]["Stream"];
                        //  StaticClass.StreamExpiryMessage = dsExpire.Tables[0].Rows[0]["ExpiryStreamStatus"].ToString();
                        //   StaticClass.LeftStreamtDays = Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftStreamDays"]);
                        str = "select *, ISNULL(IsBlock,0) as Is_Block, ISNULL(IsSuspend,0) as Is_Suspend   from AMPlayerTokens where tokenid=" + textline;
                        DataSet ds = new DataSet();
                        ds = objMainClass.fnFillDataSet(str);
                        StaticClass.TokenId = textline;
                        localCode = GenerateId.getKey(GenerateId._wvpaudi);

                        //C85C-AD70-8CDE-BD2B-5397
                        //C85C-AD70-8CDE-BD2B-5397
                        //ADC7-1E3E-7C45-F90F-E108
                        if (ds.Tables[0].Rows[0]["code"].ToString() == localCode)
                        {
                            StaticClass.dfClientId = ds.Tables[0].Rows[0]["ClientID"].ToString();
                            StaticClass.TotalTitles = ds.Tables[0].Rows[0]["NumberofTitles"].ToString();
                            StaticClass.TokenId = ds.Tables[0].Rows[0]["TokenId"].ToString();

                            IsBlock = Convert.ToInt32(ds.Tables[0].Rows[0]["Is_Block"]);
                            IsSuspend = Convert.ToInt32(ds.Tables[0].Rows[0]["Is_Suspend"]);
                            if (IsBlock == 1)
                            {
                                MessageBox.Show("Your token is blocked by admin");
                                Application.Exit();
                                return;
                            }
                            else if (IsSuspend == 1)
                            {
                                MessageBox.Show("Your token is suspend by admin");
                                return;
                            }
                            //  Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new Clientlogin());
                            return;
                        }
                        else
                        {
                            //   Application.EnableVisualStyles();
                            Application.SetCompatibleTextRenderingDefault(false);
                            Application.Run(new frmStart());
                            return;
                        }
                    }
                    else
                    {
                        File.Delete(Application.StartupPath + "//tid.amp");
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new frmStart());
                        return;
                    }
                }
                else
                {
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmStart());
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}


/// sherry32  password :- gurtaj721
/// 49439906 Hotel 3 Login
/// miraclebrows.co.uk
/// Punjabi Online Streaming Link
/// 1. http://dhinchak4.fastcast4u.com/playlist.php?name=listen.asx
/// https://www.online.iffcotokio.co.in/itgiwebportal/


// Marky's Soft-Pop & Vocals Mix ES
//Marky's Classic & Chill Mix ES
//Marky's Non-Stop Instrumental Mix 2 ES
//Marky's Lounge & Jazz Mix ES
//from which server this streams are stop ?


//Marky's Medical Mix Noorderboog ES 
//this stream missing on Centova Cast

// update AMPlayerTokens set isSuspend=0,IsBlock=0,IsDam=1,IsSanjivani=0,DamExpirydate='15-Apr-2016', Isstream=1, 
//StreamExpiryDate='11-Apr-2016', IsAdvt=1, AdvtExpiryDate='01-Jan-2016', serviceid=3 ,userid=40531 where tokenid=44032

//http://blink.ucsd.edu/technology/security/firewall/XP-2003.html



//1-2326194147