using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Data.OleDb;
namespace MusicPlayerCopyright
{
    public partial class Clientlogin : Form
    {
        byte[] photo_aray;
        gblClass ObjMainClass = new gblClass();
        string mAction;
        string SubmitValidate;
        Int32 client_Rights_Id;
        public Clientlogin()
        {
            InitializeComponent();
            CheckIfRememberedUser();
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


       private void Clientlogin_Load(object sender, EventArgs e)
       {
           try
           {
               #region "Clear Temp History"
               string args = "";
               args = ("InetCpl.cpl,ClearMyTracksByProcess 8");
               System.Diagnostics.Process process = null;
               System.Diagnostics.ProcessStartInfo processStartInfo;
               processStartInfo = new System.Diagnostics.ProcessStartInfo();
               processStartInfo.FileName = Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\Rundll32.exe";
               if ((System.Environment.OSVersion.Version.Major >= 6))
               {
                   //  Windows Vista or higher
                   //   processStartInfo.Verb = "runas";
               }
               processStartInfo.Arguments = args;
               processStartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
               processStartInfo.UseShellExecute = true;
               try
               {
                   process = System.Diagnostics.Process.Start(processStartInfo);
               }
               catch (Exception ex)
               {
                   // MessageBox.Show(ex.ToString());
               }
               finally
               {
                   if (!(process == null))
                   {
                       process.Dispose();
                   }
               }
               #endregion
           }
           catch (Exception ex)
           {
               goto GL;
           }
       GL: 
           string IsLogoGet = "No";
           string IsDealerLogoGet = "No";
           
           // pictureBox1.Location = new Point(
           //this.Width / 2 - pictureBox1.Size.Width / 2, 12);

           string strOpt = "";
           strOpt = "select * from dfclients where dfclientid=" + StaticClass.UserId;
           DataSet dsOption = new DataSet();
           dsOption = ObjMainClass.fnFillDataSet(strOpt);
           StaticClass.MainwindowMessage = dsOption.Tables[0].Rows[0]["ClientName"].ToString() + " (Music Player)";
           StaticClass.CountryCode = dsOption.Tables[0].Rows[0]["CountryCode"].ToString();

           lblExpiryPlayer.Text = StaticClass.PlayerExpiryMessage;
           label3.Text = "Player id: " + StaticClass.TokenId;
           /////////////// Show Logo ////////////////////////////

           //if (StaticClass.TokenServiceId == 1)
           //{
           //    lblName.Text = "Licensed by :- Jan Rooijakkers  && Max Profit";
           //}
           //else if (StaticClass.TokenServiceId == 2)
           //{
           //    lblName.Text = "Licensed by :- Jan Rooijakkers && Manage your Media";
           //}
           //else
           //{
        //   lblName.Text = "Licensed by Jan Rooijakkers for Jaroconsult";
          // }
           //pictureBox1.Image = Properties.Resources.Euforylogo1;
           //pictureBox1.Location = new Point(361, 31);
           //if (StaticClass.TokenServiceId == 1) //Health & care
           //{
           //    pictureBox3.Visible = true;
           //    pictureBox3.Image = Properties.Resources.Max_Profit;
           //    pictureBox2.Visible = false;
           //    pictureBox3.Location = new Point(463, 31);
           //    IsLogoGet = "Yes";
           //}
           //if (StaticClass.TokenServiceId == 2) //Hospitality
           //{
           //    IsLogoGet = "Yes";
           //    pictureBox1.Location = new Point(285, 29);
           //    pictureBox3.Visible = false;
           //    pictureBox2.Image = Properties.Resources.ManageMedia;
           //    pictureBox2.Visible = true;
           //    pictureBox2.Location = new Point(382, 29);
           //}
           
               //#region "Get Dealer Logo"
               //string strLogo = "";
               //DataSet dsDealerLogo = new DataSet();
               //if (StaticClass.DealerCode != "More000")
               //{
               //    if (IsDealerLogoGet == "No")
               //    {
               //        strLogo = "select * from tbDealerLogin where DealerCode='" + StaticClass.DealerCode + "'";
               //        dsDealerLogo = ObjMainClass.fnFillDataSet(strLogo);
               //        lblLicensedName23.Text = "";
               //        if (dsDealerLogo.Tables[0].Rows.Count > 0)
               //        {
               //            if (dsDealerLogo.Tables[0].Rows[0]["DealerLogo"] != System.DBNull.Value)
               //            {
               //                IsDealerLogoGet = "Yes";
               //                photo_aray = (byte[])dsDealerLogo.Tables[0].Rows[0]["DealerLogo"];
               //                MemoryStream ms = new MemoryStream(photo_aray);
               //                pictureBox3.Image = Image.FromStream(ms);
               //                pictureBox3.Visible = true;
               //            }
               //            if (dsDealerLogo.Tables[0].Rows[0]["DealerServiceName"] != System.DBNull.Value)
               //            {
               //                lblLicensedName23.Text = dsDealerLogo.Tables[0].Rows[0]["DealerServiceName"].ToString();
               //            }
               //        }
               //    }
               //    if (IsDealerLogoGet == "No")
               //    {
               //        strOpt = "select * from dfclients where isMainDealer=1 and countrycode=" + StaticClass.CountryCode;
               //        DataSet dsMainDealer = new DataSet();
               //        dsMainDealer = ObjMainClass.fnFillDataSet(strOpt);
               //        if (dsMainDealer.Tables[0].Rows.Count > 0)
               //        {
               //            strLogo = "select * from tbDealerLogin where DealerCode='" + dsMainDealer.Tables[0].Rows[0]["DealerCode"].ToString() + "'";
               //            dsDealerLogo = ObjMainClass.fnFillDataSet(strLogo);
               //            lblLicensedName23.Text = "";
               //            if (dsDealerLogo.Tables[0].Rows.Count > 0)
               //            {
               //                if (dsDealerLogo.Tables[0].Rows[0]["DealerLogo"] != System.DBNull.Value)
               //                {
               //                    IsDealerLogoGet = "Yes";
               //                    photo_aray = (byte[])dsDealerLogo.Tables[0].Rows[0]["DealerLogo"];
               //                    MemoryStream ms = new MemoryStream(photo_aray);
               //                    pictureBox3.Image = Image.FromStream(ms);
               //                    pictureBox3.Visible = true;
               //                }
               //                if (dsDealerLogo.Tables[0].Rows[0]["DealerServiceName"] != System.DBNull.Value)
               //                {
               //                    lblLicensedName23.Text = dsDealerLogo.Tables[0].Rows[0]["DealerServiceName"].ToString();
               //                }
               //            }
               //        }
               //    }
               //}
               //#endregion

               //if (IsDealerLogoGet == "No")
               //{
               //   // pictureBox2.Visible = false;
               //    pictureBox3.Visible = false;
               //    pictureBox2.Location = new Point(428, 12);
               //    pictureBox1.Location = new Point(484, 16);
               //    pictureBox1.Visible = false;
               //    lblLicensedName23.Text = "";
               //}
               //else
               //{
               //    pictureBox1.Visible = false;
               //   // pictureBox1.Location = new Point(366, 27);
               //    pictureBox3.Location = new Point(484, 16);
               //}


           if (IsDealerLogoGet == "No")
           {
               
           }
           //////////////////////////////////////////////////////

           this.Text = StaticClass.MainwindowMessage;
           EncrpetSongs();
           DeleteOgg();
           UpdateLocalDatabase();
       }
       private void UpdateLocalDatabase()
       {
           string strInsert = "";
           if (TableExists("tbTitleRating") == false)
           {
               strInsert = "CREATE TABLE tbTitleRating([TokenId] number NULL, 	[TitleId] number NULL, 	[TitleRating] int NULL )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbAdvt") == false)
           {
               strInsert = "CREATE TABLE tbAdvt([AdvtId] number NULL )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbLast100") == false)
           {
               strInsert = "CREATE TABLE tbLast100([SrNo] number NULL, [TitleId] number NULL )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("Playlists", "PlaylistDefault", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE Playlists ADD PlaylistDefault Text";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("tbAdvt", "ScheduleDate", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbAdvt ADD ScheduleDate DateTime";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbLastStream") == false)
           {
               strInsert = "CREATE TABLE tbLastStream([TokenId] number NULL, 	[StreamId] number NULL )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbLastStream") == false)
           {
               strInsert = "CREATE TABLE tbLastStream([TokenId] number NULL, 	[StreamId] number NULL )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("TitlesInPlaylists", "SrNo", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE TitlesInPlaylists ADD SrNo number";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbPlaylistSchedule") == false)
           {
               strInsert = "CREATE TABLE tbPlaylistSchedule([SchId] AUTOINCREMENT , 	[PlaylistId] number NULL, 	[StartDate] DateTime NULL,[EndDate] DateTime NULL,[StartTime] DateTime NULL , [WeekDay] Text NULL ,CONSTRAINT SchA_PK PRIMARY KEY(SchId))";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbPrayer") == false)
           {
               strInsert = "";
               strInsert = "CREATE TABLE tbPrayer([pId] AUTOINCREMENT ,[sDate] Date NULL,[eDate] Date NULL , [sTime] Time NULL, [eTime] Time NULL , CONSTRAINT RegA_PK PRIMARY KEY(pId))";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
       }
       public bool DoesFieldExist(string tblName, string fldName, string cnnStr)
       {
           bool functionReturnValue = false;
           // For Access Connection String,
           // use "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" &
           // accessFilePathAndName

           // Open connection to the database
           OleDbConnection dbConn = new OleDbConnection(cnnStr);
           dbConn.Open();
           DataTable dbTbl = new DataTable();

           // Get the table definition loaded in a table adapter
           string strSql = "Select TOP 1 * from " + tblName;
           OleDbDataAdapter dbAdapater = new OleDbDataAdapter(strSql, dbConn);
           dbAdapater.Fill(dbTbl);

           // Get the index of the field name
           int i = dbTbl.Columns.IndexOf(fldName);

           if (i == -1)
           {
               //Field is missing
               functionReturnValue = false;
           }
           else
           {
               //Field is there
               functionReturnValue = true;
           }

           dbTbl.Dispose();
           dbConn.Close();
           dbConn.Dispose();
           return functionReturnValue;
       }
       public static bool TableExists(string table)
       {
           if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
           StaticClass.LocalCon.Open();
           return StaticClass.LocalCon.GetSchema("Tables", new string[4] { null, null, table, "TABLE" }).Rows.Count > 0;
       }
       private void EncrpetSongs()
       {
           string d = Application.StartupPath;
           foreach (string f in Directory.GetFiles(d, "*.mp3"))
           {
               clsSongCrypt.encrfile(new Uri(f, UriKind.Relative));
               
           }
       }
       private void DeleteOgg()
       {
           string d = Application.StartupPath;
           try
           {
               foreach (string f in Directory.GetFiles(d, "*.mp3"))
               {

                   File.Delete(f);
               }
           }
           catch(Exception ex)
           {
           }
       }
       private void SubmitValidation()
       {
           string str = "";
           str = "select * from tbluser_client_rights where userid=" + StaticClass.UserId + " and clientname='" + txtloginUserName.Text + "' and clientpassword='" + txtLoginPassword.Text + "'";
           DataSet ds = new DataSet();
           ds = ObjMainClass.fnFillDataSet(str);
           if (txtloginUserName.Text == "")
           {
               MessageBox.Show("The login user name cannot be empty", "Copyright Player");
               SubmitValidate = "False";
           }
           else if (txtLoginPassword.Text == "")
           {
               MessageBox.Show("Login password cannot be blank", "Copyright Player");
               SubmitValidate = "False";
           }
           else if (ds.Tables[0].Rows.Count <= 0)
           {
               MessageBox.Show("Login user/password is wrong", "Copyright Player");
               SubmitValidate = "False";
           }
           else if (ds.Tables[0].Rows.Count > 0)
           {
               StaticClass.LocalUserId = ds.Tables[0].Rows[0]["clientRightsId"].ToString();
               StaticClass.Is_Admin = ds.Tables[0].Rows[0]["isAdmin"].ToString();
               StaticClass.isRemove = ds.Tables[0].Rows[0]["isRemove"].ToString();
               StaticClass.isDownload = ds.Tables[0].Rows[0]["isDownload"].ToString();
               SubmitValidate = "True";
           }
       }

       private void btnLogin_Click(object sender, EventArgs e)
       {
           string strOpt = "";
           CheckPlayerUpdateVersion();
           SubmitValidation();
           if (SubmitValidate == "True")
           {
               strOpt = "select * from dfclients where dfclientid=" + StaticClass.UserId;
               DataSet dsOption = new DataSet();
               dsOption = ObjMainClass.fnFillDataSet(strOpt);
               StaticClass.MainwindowMessage = dsOption.Tables[0].Rows[0]["ClientName"].ToString() + " (" + dsOption.Tables[0].Rows[0]["email"].ToString() + ")";
               

               strOpt = "";
               strOpt = "select * from tbAdvertisementTiming";
               DataSet dsOption1 = new DataSet();
               dsOption1 = ObjMainClass.fnFillDataSet(strOpt);
               StaticClass.AdvtTime = Convert.ToInt32(dsOption1.Tables[0].Rows[0]["AdvtTime"]);

                if (chkRemember.Checked== true)
                {
                    Properties.Settings.Default.RememberMeUsername = txtloginUserName.Text;
                    Properties.Settings.Default.RememberMePassword = txtLoginPassword.Text;
                    Properties.Settings.Default.Save();
                }
                else
                {                    
                    Properties.Settings.Default.RememberMeUsername = "";
                    Properties.Settings.Default.RememberMePassword = "";
                    Properties.Settings.Default.Save(); 
                }
                
                
                    //string proc = Process.GetCurrentProcess().ProcessName;
                    //Process[] processes = Process.GetProcessesByName(proc);
                    //if (processes.Length > 1)
                    //{
                    //    Process.GetCurrentProcess().Kill();
                    //} 

                    //VersionApplicationPath = Application.StartupPath + "\\MusicPlayer.exe";
                    //System.Diagnostics.Process.Start(VersionApplicationPath);
                

               mainwindow objMainWindow = new mainwindow();
               objMainWindow.Show();
               this.Hide();
           }
       }

       private void Clientlogin_KeyDown(object sender, KeyEventArgs e)
       {
           if (e.KeyCode == Keys.Enter)
           {
               SendKeys.Send("{TAB}");
           }
       }

       private void Clientlogin_FormClosed(object sender, FormClosedEventArgs e)
       {
           //Application.Exit();
       }

       private void txtLoginPassword_KeyDown(object sender, KeyEventArgs e)
       {
           string strOpt = "";
           string VersionApplicationPath = "";
           if (e.KeyCode == Keys.Enter)
           {
               SubmitValidation();
               if (SubmitValidate == "True")
               {
                   strOpt = "select * from dfclients where dfclientid=" + StaticClass.UserId;
                   DataSet dsOption = new DataSet();
                   dsOption = ObjMainClass.fnFillDataSet(strOpt);
                   StaticClass.MainwindowMessage = dsOption.Tables[0].Rows[0]["ClientName"].ToString() + " (" + dsOption.Tables[0].Rows[0]["email"].ToString() + ")";

                   if (chkRemember.Checked == true)
                   {
                       Properties.Settings.Default.RememberMeUsername = txtloginUserName.Text;
                       Properties.Settings.Default.RememberMePassword = txtLoginPassword.Text;
                       Properties.Settings.Default.Save();
                   }
                   else
                   {
                       Properties.Settings.Default.RememberMeUsername = "";
                       Properties.Settings.Default.RememberMePassword = "";
                       Properties.Settings.Default.Save();
                   }
                   CheckPlayerUpdateVersion();
                       //string proc = Process.GetCurrentProcess().ProcessName;
                       //Process[] processes = Process.GetProcessesByName(proc);
                       //if (processes.Length > 1)
                       //{
                       //    Process.GetCurrentProcess().Kill();
                       //} 

                       //VersionApplicationPath = Application.StartupPath + "\\MusicPlayer.exe";
                       //System.Diagnostics.Process.Start(VersionApplicationPath);
                   mainwindow objMainWindow = new mainwindow();
                   objMainWindow.Show();
                   this.Hide();
               }
           }

       }
       private void CheckPlayerUpdateVersion()
       {
           string strOldVersion = "";
           string FileLocation = "";
           string strUpdateVersion = "";
           string VersionApplicationPath = "";
           DateTime VersionAvailbleDate;
           DateTime CurrentDate= DateTime.Now.Date;
           Int32 OldVersion = 0;
           Int32 UpdateVersion = 0;
           DataTable dtOldVersion = new DataTable();
           DataTable dtUpdateVersion = new DataTable();
           MessageBoxButtons buttons = MessageBoxButtons.YesNo;
           DialogResult result;
           try
           {
               strOldVersion = "select isnull(IsUpdated,0) as PlayerVersion from AMPlayerTokens where tokenid =" + StaticClass.TokenId;
               dtOldVersion = ObjMainClass.fnFillDataTable(strOldVersion);

               strUpdateVersion = "select * from tbPlayerUpdateVersion where UpdateId in(select MAX(UpdateId) from tbPlayerUpdateVersion where musictype='Copyright') and musictype='Copyright'";
               dtUpdateVersion = ObjMainClass.fnFillDataTable(strUpdateVersion);
               if (dtUpdateVersion.Rows.Count > 0)
               {
                   OldVersion = Convert.ToInt32(dtOldVersion.Rows[0]["PlayerVersion"]);
                   UpdateVersion = Convert.ToInt32(dtUpdateVersion.Rows[0]["UpdateId"]);
                   VersionAvailbleDate = Convert.ToDateTime(dtUpdateVersion.Rows[0]["AviableDate"]);
                   FileLocation = dtUpdateVersion.Rows[0]["FileLocation"].ToString();

                   //if (VersionAvailbleDate > CurrentDate) return ;
                   if (OldVersion < UpdateVersion)
                   {

                       VersionApplicationPath = Application.StartupPath + "\\UpdateMusicPlayerCopyright.exe";
                       // result = MessageBox.Show("New version of player is availble" + Environment.NewLine + " !! You want to update ?", "Player Update", buttons);
                       //if (result == System.Windows.Forms.DialogResult.Yes)
                       //{

                       //if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                       //StaticClass.constr.Open();
                       //SqlCommand cmd = new SqlCommand();
                       //cmd.Connection = StaticClass.constr;
                       //cmd.CommandText = "update AMPlayerTokens set IsUpdated = " + UpdateVersion + " where tokenid=" + StaticClass.TokenId;
                       //cmd.ExecuteNonQuery();
                       //StaticClass.constr.Close();

                       #region Update

                       string localPath = Application.StartupPath + "\\UpdateMusicPlayerCopyright.exe";
                       string UpdateFileLocation = "ftp://85.195.82.94:21/Copyright/UpdateMusicPlayerCopyright.exe";
                       try
                       {

                           FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(UpdateFileLocation);
                           requestFileDownload.Credentials = new NetworkCredential("harish", "Mohali123");
                           requestFileDownload.KeepAlive = true;
                           requestFileDownload.UseBinary = true;
                           requestFileDownload.UsePassive = false;
                           requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;

                           FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();

                           Stream responseStream = responseFileDownload.GetResponseStream();
                           FileStream writeStream = new FileStream(localPath, FileMode.Create);

                           int Length = 2048;
                           Byte[] buffer = new Byte[Length];
                           int bytesRead = responseStream.Read(buffer, 0, Length);

                           while (bytesRead > 0)
                           {
                               writeStream.Write(buffer, 0, bytesRead);
                               bytesRead = responseStream.Read(buffer, 0, Length);


                               // calculate the progress out of a base "100"

                               double dIndex = (double)(bytesRead);

                               double dTotal = (double)Length;

                               double dProgressPercentage = (dIndex / dTotal);

                               int iProgressPercentage = (int)(dProgressPercentage * 100);



                           }
                           responseStream.Close();
                           writeStream.Close();

                           requestFileDownload = null;
                           responseFileDownload = null;
                       }
                       catch(Exception ex)
                       {
                           MessageBox.Show(ex.Message + " Inner");
                       }

                       #endregion




                       System.Diagnostics.Process.Start(VersionApplicationPath);




                       Process[] prs = Process.GetProcesses();
                       foreach (Process pr in prs)
                       {
                           if (pr.ProcessName == "MusicPlayerCopyright")
                               pr.Kill();
                       }
                   }

                   //}
               }
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message + " Outter");
           }
            
       }
       private void Clientlogin_Move(object sender, EventArgs e)
       {
          
       }
       private void CheckIfRememberedUser()
       {
           if (Properties.Settings.Default.RememberMeUsername != null && Properties.Settings.Default.RememberMeUsername != "")
           {
               txtloginUserName.Text = Properties.Settings.Default.RememberMeUsername;
               txtLoginPassword.Text = Properties.Settings.Default.RememberMePassword;
               chkRemember.Checked = true;
           }
       }

       private void Clientlogin_FormClosing(object sender, FormClosingEventArgs e)
       {
           //MessageBoxButtons buttons = MessageBoxButtons.YesNo;
           //DialogResult result;
           //result = MessageBox.Show("Are you sure to exit ?", "Copyright Player", buttons);
           //if (result == System.Windows.Forms.DialogResult.Yes)
           //{
               Application.Exit();
           //}
           //else
           //{
           //    e.Cancel = true;
           //}
       }

       private void btnExtra_Click(object sender, EventArgs e)
       {
           string fileName = "15.mp3";
           string localPath = Application.StartupPath + "\\";
           string file_song_path = Application.StartupPath + "\\" + fileName;
           FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create("ftp://85.195.103.190:21/AllSongs/oggfiles/" + fileName);
           requestFileDownload.Credentials = new NetworkCredential("ftpuser", "23789Songs");
           // requestFileDownload.KeepAlive = true;
//           requestFileDownload.UsePassive = false;
           // requestFileDownload.UseBinary = true;
           requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
           FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
           Stream responseStream = responseFileDownload.GetResponseStream();
           FileStream writeStream = new FileStream(localPath + fileName, FileMode.Create);
           FtpWebRequest requestFileSize = (FtpWebRequest)WebRequest.Create("ftp://85.195.103.190:21/AllSongs/oggfiles/" + fileName);
           requestFileSize.Credentials = new NetworkCredential("ftpuser", "23789Songs");
           requestFileSize.UsePassive = false;
           requestFileSize.Method = WebRequestMethods.Ftp.GetFileSize;
           FtpWebResponse response = (FtpWebResponse)requestFileSize.GetResponse();
           Int64 Length = response.ContentLength;
           Byte[] byteBuffer = new Byte[Length];
           int iByteSize = 0;
           Int64 iRunningByteTotal = 0;
           while ((iByteSize = responseStream.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
           {
               writeStream.Write(byteBuffer, 0, iByteSize);
               iRunningByteTotal += iByteSize;
               double dIndex = (double)(iRunningByteTotal);
               double dTotal = (double)byteBuffer.Length;
               double dProgressPercentage = (dIndex / dTotal);
               int iProgressPercentage = (int)(dProgressPercentage * 100);

           }

           responseStream.Close();
           writeStream.Close();

           requestFileDownload = null;
           responseFileDownload = null;           
       }

       private void AdminMail()
       {
           try
           {
               var fromAddress = "noreply@manageyourclaudio.eu";
               var toAddress = "talwindergur@gmail.com";
               const string fromPassword = "Claudio@123456";
               string subject = "New Dealer Register";
               string body = "Hello Admin, \n";
               body = "\n";
               body += "This is to you inform that new dealer is registred on music player and credential are \n ";
               body += "Main Username:\n";
               var smtp = new System.Net.Mail.SmtpClient();
               {
                   smtp.Host = "juniper.arvixe.com";
                   smtp.Port = 26;
                   smtp.EnableSsl = false;
                   smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                   smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                   smtp.Timeout = 999999999;
               }
               smtp.Send(fromAddress, toAddress, subject, body);
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
       }

       private void label4_Click(object sender, EventArgs e)
       {
           System.Diagnostics.Process.Start("http://www.parastechnologies.com/");
       }
    }
}
