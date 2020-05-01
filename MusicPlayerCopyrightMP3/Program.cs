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
namespace MusicPlayerCopyright
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
                    firewallRule.ApplicationName = Application.StartupPath + "\\MusicPlayerCopyright.exe";
                    firewallRule.Enabled = true;
                    firewallRule.InterfaceTypes = "All";
                    firewallRule.Name = "MusicPlayer";

                    INetFwPolicy2 firewallPolicy = (INetFwPolicy2)Activator.CreateInstance(
                        Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                    firewallPolicy.Rules.Add(firewallRule);

                    INetFwRule firewallRuleUpdate = (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FWRule"));
                    firewallRuleUpdate.Action = NET_FW_ACTION_.NET_FW_ACTION_ALLOW;
                    firewallRuleUpdate.Description = "Allow notepad";
                    firewallRuleUpdate.ApplicationName = Application.StartupPath + "\\UpdateMusicPlayerCopyright.exe";
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
                
                
                //StaticClass.constr = new SqlConnection("Data Source=85.195.82.94;database=EuforyTal;uid=sa;password=phoh7Aiheeki");
            StaticClass.constr = new SqlConnection("Data Source=134.119.178.26;database=nusign;uid=sa;password=Jan@Server007;Connect Timeout=5000");
                StaticClass.constrOldDB = new SqlConnection("Data Source=134.119.178.26;database=nusign;uid=sa;password=Jan@Server007;Connect Timeout=5000");

                //StaticClass.LocalCon.ConnectionString= string.Format("Provider={0}; Data Source={1}; Jet OLEDB:Engine Type={2}", "Microsoft.Jet.OLEDB.4.0", "db.mdb", 5);
                StaticClass.LocalCon.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath +"\\db.mdb;User Id=admin;Password=;";
                string str = "";
                string localCode = "";
                string filename = Application.StartupPath + "\\tid.amp";
                string textline = "";
                string strOpt = "";
                string ExpiryCopyrightStatus = "";
                string ExpiryFitnessStatus = "";
                Int32 IsBlock = 0;
                Int32 IsSuspend = 0;
                Int32 LeftCopyrightDays = 0;
                Int32 LeftFitnessDays = 0;

                string proc = Process.GetCurrentProcess().ProcessName;
                Process[] processes = Process.GetProcessesByName(proc);
                if (processes.Length > 1)
                {
                    MessageBox.Show("Application is already running","Music player");
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
                        MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                        return;
                    }


                    if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                    StaticClass.constr.Open();


                    strOpt = "select ISNULL(IsCopyright,0) as Copyright, ISNULL(IsFitness,0) as Fitness, isnull(IsStream,0) as Stream,dealerCode, ISNULL(IsAdvt,0) as Advt, ISNULL(IsAdvtManual,0) as AdvtManual, ISNULL(IsBlockAdvt,0) as IsBlockAdvt , isnull(serviceid,0) as serviceid, isnull(cityid,0) as CityId , isnull(StateId,0) as StateId, isnull(CountryId,0) as CountryId from AMPlayerTokens where TokenID=" + textline;
                    DataSet dsOption = new DataSet();
                    dsOption = objMainClass.fnFillDataSet(strOpt);

                    if (dsOption.Tables[0].Rows.Count > 0)
                    {
                        StaticClass.TokenServiceId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["serviceid"]);

                        StaticClass.DealerCode = dsOption.Tables[0].Rows[0]["DealerCode"].ToString();

                        StaticClass.IsAdvtManual = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["AdvtManual"]);
                        StaticClass.IsAdvt = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["Advt"]);
                        StaticClass.IsBlockAdvt = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["IsBlockAdvt"]);
                        StaticClass.AdvtCityId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["CityId"]);
                        StaticClass.Stateid = Convert.ToInt32(dsOption.Tables[0].Rows[0]["Stateid"]);
                        StaticClass.CountryId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["CountryId"]);

                        str = "spGetTokenExpiryStatus_Copyright " + Convert.ToInt32(textline) + ", " + dsOption.Tables[0].Rows[0]["Copyright"] + ", " + dsOption.Tables[0].Rows[0]["Fitness"] + ", " + dsOption.Tables[0].Rows[0]["Stream"];
                        DataSet dsExpire = new DataSet();
                        dsExpire = objMainClass.fnFillDataSet(str);


                        ExpiryCopyrightStatus = dsExpire.Tables[0].Rows[0]["ExpiryCopyrightStatus"].ToString();
                        LeftCopyrightDays = Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftCopyrightDays"]);

                        ExpiryFitnessStatus = dsExpire.Tables[0].Rows[0]["ExpiryFitnessStatus"].ToString();
                        LeftFitnessDays = Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftFitnessDays"]);

                        StaticClass.StreamExpiryMessage = dsExpire.Tables[0].Rows[0]["ExpiryStreamStatus"].ToString();
                        StaticClass.LeftStreamtDays = Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftStreamDays"]);

                        if (ExpiryCopyrightStatus == "NoLic" && ExpiryFitnessStatus == "NoLic")
                        {
                            MessageBox.Show("!! Purchase the subscription of music player  !!", "Copyright Player");
                            Application.Exit();
                            return;
                        }
                        if (ExpiryCopyrightStatus == "Yes" && ExpiryFitnessStatus == "Yes")
                        {
                            MessageBox.Show("!! The license of your music player is expired. Please contact your dealer. !!" + Environment.NewLine + "Your player id: " + textline + "", "Copyright Player");
                            Application.Exit();
                            return;
                        }

                        if (ExpiryCopyrightStatus != "NoLic" && LeftCopyrightDays <= 10 && LeftCopyrightDays != 0 && LeftCopyrightDays > 0)
                        {
                            StaticClass.PlayerExpiryMessage = Convert.ToString(LeftCopyrightDays) + " days left to renewal of subscription." + Environment.NewLine + "Please pay for your subscription to keep your music online.";
                            StaticClass.IsCopyright = true;
                        }
                        else if (ExpiryCopyrightStatus != "NoLic" && LeftCopyrightDays == 0)
                        {
                            StaticClass.PlayerExpiryMessage = "Last day to renewal of subscription." + Environment.NewLine + "Please pay for your subscription to keep your music online.";
                            StaticClass.IsCopyright = true;
                        }
                        else
                        {
                            StaticClass.IsCopyright = true;
                        }


                        if (ExpiryCopyrightStatus == "Yes" && ExpiryFitnessStatus == "NoLic")
                        {
                            MessageBox.Show("!! Subscription is Expired.Please connect your vendor !! " + Environment.NewLine + "Your player id: " + textline + "", "Copyright Player");
                            Application.Exit();
                            return;
                        }
                        else if (ExpiryCopyrightStatus == "Yes" && ExpiryFitnessStatus == "No")
                        {
                            StaticClass.PlayerExpiryMessage = "Subscription is Expired.Please connect your vendor";
                            StaticClass.IsCopyright = false;
                        }
                        else if (ExpiryCopyrightStatus == "NoLic" && ExpiryFitnessStatus == "No")
                        {
                            StaticClass.PlayerExpiryMessage = "You do not have license";
                            StaticClass.IsCopyright = false;
                        }


                        if (ExpiryFitnessStatus != "NoLic" && LeftFitnessDays <= 10)
                        {

                            StaticClass.FitnessExpiryMessage = Convert.ToString(LeftFitnessDays) + " days left to renewal of Fitness subscription" + Environment.NewLine + "Please pay for your subscription to keep your music online.";
                            StaticClass.IsFitness = true;
                        }
                        else if (ExpiryFitnessStatus != "NoLic" && LeftFitnessDays == 0)
                        {
                            StaticClass.FitnessExpiryMessage = "Last day to renewal of Fitness subscription";
                            StaticClass.IsFitness = true;
                        }
                        else
                        {
                            StaticClass.IsFitness = true;
                        }

                        if (ExpiryFitnessStatus == "Yes" && ExpiryCopyrightStatus == "NoLic")
                        {
                            MessageBox.Show("!! Subscription is Expired.Please connect your vendor !!", "Copyright Player");
                            Application.Exit();
                            return;
                        }
                        else if (ExpiryFitnessStatus == "Yes" && ExpiryCopyrightStatus == "No")
                        {
                            StaticClass.FitnessExpiryMessage = "Subscription is Expired.Please connect your vendor";
                            StaticClass.IsFitness = false;
                        }
                        else if (ExpiryFitnessStatus == "NoLic" && ExpiryCopyrightStatus == "No")
                        {
                            StaticClass.FitnessExpiryMessage = "You do not have license";
                            StaticClass.IsFitness = false;
                        }






                        str = "select *, ISNULL(IsBlock,0) as Is_Block, ISNULL(IsSuspend,0) as Is_Suspend   from AMPlayerTokens where tokenid=" + textline;
                        DataSet ds = new DataSet();
                        ds = objMainClass.fnFillDataSet(str);
                        StaticClass.TokenId = textline;
                        localCode = GenerateId.getKey(GenerateId._wvpaudi);
                        
                        if (ds.Tables[0].Rows[0]["code"].ToString() == localCode)
                        {
                            StaticClass.UserId = ds.Tables[0].Rows[0]["ClientID"].ToString();
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
                    //  Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new frmStart());
                    return;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            
        }
    }
}
