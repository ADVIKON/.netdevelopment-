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
using System.Globalization;

namespace SFVideoPlayer
{
    public partial class Clientlogin : Form
    {
        DateTimeFormatInfo fi = new DateTimeFormatInfo();
        string TempAdvtFileName;
        byte[] photo_aray;
        gblClass ObjMainClass = new gblClass();
        string mAction;
        string SubmitValidate;
        Int64 client_Rights_Id;
        public Clientlogin()
        {
            InitializeComponent();
            CheckIfRememberedUser();
        }


        private void picDisplay_Click(object sender, EventArgs e)
        {


            //string str = "";
            //str = "select * from tbluser_client_rights where userid= " + StaticClass.UserId;
            //DataSet ds = new DataSet();
            //ds = ObjMainClass.fnFillDataSet(str);
            //if (ds.Tables[0].Rows.Count == 0)
            //{
            //    panUserDetail.Visible = true;
            //    picDisplay.Visible = false;
            //    chkRemember.Visible = false;
            //    Clear_Controls();
            //    return;
            //}

            //if (txtloginUserName.Text == "")
            //{
            //    MessageBox.Show("Login user name cannot be blank","Player");
            //    return;
            //}
            //if (txtLoginPassword.Text == "")
            //{
            //    MessageBox.Show("Login password cannot be blank", "Player");
            //    return;
            //}
            //    str = "select * from tbluser_client_rights where userid=" + StaticClass.UserId + " and isAdmin=1 and clientname='"+ txtloginUserName.Text +"' and Clientpassword = '" + txtLoginPassword.Text + "'";
            //    ds = ObjMainClass.fnFillDataSet(str);
            //    if (ds.Tables[0].Rows.Count <= 0)
            //    {
            //        MessageBox.Show("You are not a administrator", "Player");
            //        return;
            //    }
            //panUserDetail.Visible = true;
            //picDisplay.Visible = false;
            //chkRemember.Visible = false;
            //Clear_Controls();
            //PopulateInputFileTypeDetail();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void Clientlogin_Load(object sender, EventArgs e)
        {
            ObjMainClass.UpdateLocalDatabase();
            ObjMainClass.CompactLocaldb();
            try {
                #region Update TitleInPlaylists Table
                string str = "";
                str = "select * from tbSplPlaylistSchedule";
                DataTable dtP = new DataTable();
                dtP = ObjMainClass.fnFillDataTable_Local(str);
                if (dtP.Rows.Count > 0)
                {
                    for (int iCtr = 0; iCtr < dtP.Rows.Count; iCtr++)
                    {
                        str = "";
                        str = "select titleId from tbSpecialPlaylists_Titles where isDownload=1 and SchId= " + dtP.Rows[iCtr]["SchId"];
                        str = str + " and titleid not in (select titleid from TitlesInPlaylists where PlaylistID= " + dtP.Rows[iCtr]["PlaylistID"] + " )";
                        DataTable dtT = new DataTable();
                        dtT = ObjMainClass.fnFillDataTable_Local(str);
                        for (int iT = 0; iT < dtT.Rows.Count; iT++)
                        {
                            str = "";
                            str = "insert into TitlesInPlaylists(PlaylistID,TitleID,SrNo) values(" + dtP.Rows[iCtr]["PlaylistID"] + "," + dtT.Rows[iT]["titleId"] + ",1)";
                            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                            OleDbCommand cmdUpdateAll = new OleDbCommand();
                            cmdUpdateAll.Connection = StaticClass.LocalCon;
                            cmdUpdateAll.CommandText = str;
                            cmdUpdateAll.ExecuteNonQuery();
                        }
                    }
                }

                str = "";
                str = "select * from tbSpecialPlaylists_Titles where ";
                str = str + " titleid not in (select titleid from Titles)";
                DataTable dtMisTitle = new DataTable();
                dtMisTitle = ObjMainClass.fnFillDataTable_Local(str);
                for (int iTitle = 0; iTitle < dtMisTitle.Rows.Count; iTitle++)
                {
                    string Special_Name = "";
                    string Special_Change = "";
                    str = "";
                    str = "select * from Albums where AlbumID =" + dtMisTitle.Rows[iTitle]["AlbumID"];
                    DataTable dtAlbum = new DataTable();
                    dtAlbum = ObjMainClass.fnFillDataTable_Local(str);
                    if (dtAlbum.Rows.Count == 0)
                    {
                        Special_Name = dtMisTitle.Rows[iTitle]["alName"].ToString();
                        Special_Change = Special_Name.Replace("'", "??$$$??");
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        str = "";
                        str = "insert into Albums values (" + Convert.ToInt32(dtMisTitle.Rows[iTitle]["AlbumID"]) + " , ";
                        str = str + Convert.ToInt32(dtMisTitle.Rows[iTitle]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                        OleDbCommand cmdAlbum = new OleDbCommand();
                        cmdAlbum.Connection = StaticClass.LocalCon;
                        cmdAlbum.CommandText = str;
                        cmdAlbum.ExecuteNonQuery();
                    }
                    str = "";
                    str = "select * from Artists where ArtistID =" + dtMisTitle.Rows[iTitle]["ArtistID"];
                    DataTable dtArtist = new DataTable();
                    dtArtist = ObjMainClass.fnFillDataTable_Local(str);
                    if (dtArtist.Rows.Count == 0)
                    {
                        Special_Name = dtMisTitle.Rows[iTitle]["arName"].ToString();
                        Special_Change = Special_Name.Replace("'", "??$$$??");
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        str = "";
                        str = "insert into Artists values ( ";
                        str = str + Convert.ToInt32(dtMisTitle.Rows[iTitle]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                        OleDbCommand cmdAlbum = new OleDbCommand();
                        cmdAlbum.Connection = StaticClass.LocalCon;
                        cmdAlbum.CommandText = str;
                        cmdAlbum.ExecuteNonQuery();
                    }


                    Special_Name = dtMisTitle.Rows[iTitle]["title"].ToString();
                    Special_Change = Special_Name.Replace("'", "??$$$??");

                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    string sWr = "insert into Titles (TitleID,AlbumID,ArtistID,Title,Gain,[Time],TitleYear,HttpUrl,LocalUrl) values (" + Convert.ToInt32(dtMisTitle.Rows[iTitle]["TitleID"]) + " , " + Convert.ToInt32(dtMisTitle.Rows[iTitle]["AlbumID"]) + " , ";
                    sWr = sWr + Convert.ToInt32(dtMisTitle.Rows[iTitle]["ArtistID"]) + ", '" + Special_Change + "' , ";
                    sWr = sWr + " '0' , '" + dtMisTitle.Rows[iTitle]["Time"] + "' ,0, ";
                    sWr = sWr + " '" + dtMisTitle.Rows[iTitle]["HttpUrl"] + "', ";
                    sWr = sWr + " '" + dtMisTitle.Rows[iTitle]["LocalUrl"] + "' )";

                    OleDbCommand cmdTitle = new OleDbCommand();
                    cmdTitle.Connection = StaticClass.LocalCon;
                    cmdTitle.CommandText = sWr;
                    cmdTitle.ExecuteNonQuery();


                }



                #endregion
            }
            catch (Exception ex)
            {
                goto Nex;
            }
            Nex:






            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";

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

                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdUpdateAll = new OleDbCommand();
                cmdUpdateAll.Connection = StaticClass.LocalCon;
                cmdUpdateAll.CommandText = "delete from  tbTokenOverDueStatus where  IsUpload=1 ";
                cmdUpdateAll.ExecuteNonQuery();

                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                cmdUpdateAll = new OleDbCommand();
                cmdUpdateAll.Connection = StaticClass.LocalCon;
                cmdUpdateAll.CommandText = "delete from  tbTokenPlayedSongs where  IsUpload=1 ";
                cmdUpdateAll.ExecuteNonQuery();

                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                cmdUpdateAll = new OleDbCommand();
                cmdUpdateAll.Connection = StaticClass.LocalCon;
                cmdUpdateAll.CommandText = "delete from  tbTokenAdvtStatus where  IsUpload=1 ";
                cmdUpdateAll.ExecuteNonQuery();

                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                cmdUpdateAll = new OleDbCommand();
                cmdUpdateAll.Connection = StaticClass.LocalCon;
                cmdUpdateAll.CommandText = "delete from  tbTokenLoginStatus where  IsUpload=1 ";
                cmdUpdateAll.ExecuteNonQuery();


                string strLogin = "";
                strLogin = "";
                strLogin = strLogin + " insert into tbTokenLoginStatus(TokenId,StatusDate,StatusTime,IsUpload) values( " + StaticClass.TokenId + ", ";
                strLogin = strLogin + "  '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "', ";
                strLogin = strLogin + " '" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "',0)";

                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdLoginStatus = new OleDbCommand();
                cmdLoginStatus.Connection = StaticClass.LocalCon;
                cmdLoginStatus.CommandText = strLogin;
                cmdLoginStatus.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                goto GL;
            }
            GL:

            lblPlayerId.Text = " Your player id is: " + StaticClass.TokenId + "";
            string IsLogoGet = "No";
            string IsDealerLogoGet = "No";

            if (ObjMainClass.CheckForInternetConnection() == true)
            {

                // this.Icon = Properties.Resources.Eufory;
                // pictureBox1.Location = new Point(  9464313764 
                //this.Width / 2 - pictureBox1.Size.Width / 2,12);
                this.Icon = Properties.Resources.more;
                //  lblName.Text = "Jan Rooijakkers hold the copyrights";
                string strOpt = "";
                strOpt = "select * from dfclients where dfclientid=" + StaticClass.dfClientId;
                DataSet dsOption = new DataSet();
                dsOption = ObjMainClass.fnFillDataSet(strOpt);
                StaticClass.MainwindowMessage = dsOption.Tables[0].Rows[0]["ClientName"].ToString();
                StaticClass.CountryCode = dsOption.Tables[0].Rows[0]["CountryCode"].ToString();

                this.Text = StaticClass.MainwindowMessage;
                /////////////// Show Logo ////////////////////////////
                // pictureBox1.Image = Properties.Resources.Euforylogo;
                // pictureBox1.Location = new Point(361, 31);
                // if (StaticClass.TokenServiceId == 1) //Health & care
                // {
                //     pictureBox3.Visible = true;
                //     pictureBox3.Image = Properties.Resources.Max_Profit;
                //     pictureBox2.Visible = false;
                //     pictureBox3.Location = new Point(463, 31);
                //     IsLogoGet = "Yes";
                // }
                // if (StaticClass.TokenServiceId == 2) //Hospitality
                // {
                //     IsLogoGet = "Yes";
                //     pictureBox1.Location = new Point(285, 29);
                //     pictureBox3.Visible = false;
                //     pictureBox2.Image = Properties.Resources.ManageMedia;
                //     pictureBox2.Visible = true;
                //     pictureBox2.Location = new Point(382, 29);
                // }
                #region "Get Dealer Logo"
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
                //                picDealer.Image = Image.FromStream(ms);
                //                picDealer.Visible = true;
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
                //                    picDealer.Image = Image.FromStream(ms);
                //                    picDealer.Visible = true;
                //                }
                //                if (dsDealerLogo.Tables[0].Rows[0]["DealerServiceName"] != System.DBNull.Value)
                //                {
                //                    lblLicensedName23.Text = dsDealerLogo.Tables[0].Rows[0]["DealerServiceName"].ToString();
                //                }
                //            }
                //        }
                //    }
                //}
                #endregion
            }
            else
            {
                this.Text = "Offline Mode";
            }
            //if (IsDealerLogoGet == "No")
            //{
            //    picEpidemic.Location = new Point(426, 12);
            //   // picManage.Location = new Point(473, 12);
            //  //  picManage.Visible = true;
            //    picDealer.Visible = false;
            //    lblLicensedName23.Text = "";
            //}
            //else
            //{
            //    picEpidemic.Location = new Point(355, 12);
            //    picDealer.Location = new Point(473, 12);
            //    picDealer.Visible = true;

            //}
            //////////////////////////////////////////////////////


 

            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            OleDbCommand cmdLocal = new OleDbCommand();
            cmdLocal.Connection = StaticClass.LocalCon;
            cmdLocal.CommandText = "delete from  tbMisc ";
            cmdLocal.ExecuteNonQuery();
            StaticClass.LocalCon.Close();

            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            cmdLocal = new OleDbCommand();
            cmdLocal.Connection = StaticClass.LocalCon;
            cmdLocal.CommandText = "insert into tbMisc(DealerCode,IsStore,DfClientId,IsAdvt,IsLock,ScheduleType) values('" + StaticClass.DealerCode + "'," + Convert.ToByte(StaticClass.IsStore) + "," + StaticClass.dfClientId + "," + Convert.ToByte(StaticClass.IsAdvt) + "," + Convert.ToByte(StaticClass.IsLock) + ",'" + StaticClass.ScheduleType + "')";
            cmdLocal.ExecuteNonQuery();
            StaticClass.LocalCon.Close();
            //if (ObjMainClass.CheckForInternetConnection() == true)
            //{
            //    if (StaticClass.IsStore == true)
            //    {
            //        var weekNo = (int)DateTime.Now.DayOfWeek;
            //        DataTable dtDetailNew = new DataTable();
            //          string strNew = "GetSpecialPlaylistSchedule " + weekNo + ", " + StaticClass.TokenId + " ," + StaticClass.dfClientId + "";
            //        dtDetailNew = ObjMainClass.fnFillDataTable(strNew);
            //        if ((dtDetailNew.Rows.Count <= 0))
            //        {
            //            //lblError.Text = "The player is not activated. Please contact support. ";
            //            //lblError.Visible = true;
            //            //Application.Exit();
            //        }
            //    }
            //}


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
         
        



        private void SubmitValidation()
        {
            string str = "";
            str = "select * from tbluser_client_rights where userid=" + StaticClass.dfClientId + " and clientname='" + txtloginUserName.Text + "' and clientpassword='" + txtLoginPassword.Text + "'";
            DataSet ds = new DataSet();
            ds = ObjMainClass.fnFillDataSet(str);
            if (txtloginUserName.Text == "")
            {
                MessageBox.Show("Login user name cannot be blank", "Player");
                SubmitValidate = "False";
            }
            else if (txtLoginPassword.Text == "")
            {
                MessageBox.Show("Login password cannot be blank", "Player");
                SubmitValidate = "False";
            }
            else if (ds.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show("Login user/password is wrong", "Player");
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


            timAutoStart.Enabled = false;

            if (ObjMainClass.CheckForInternetConnection() == true)
            {
                CheckPlayerUpdateVersion();
                SubmitValidation();
                if (SubmitValidate == "True")
                {
                    strOpt = "select * from users where userid=" + StaticClass.TokenUserId;
                    DataSet dsOption = new DataSet();
                    dsOption = ObjMainClass.fnFillDataSet(strOpt);
                    StaticClass.MainwindowMessage = StaticClass.MainwindowMessage + " (" + dsOption.Tables[0].Rows[0]["Useremail"].ToString() + ")";


                    strOpt = "";
                    strOpt = "select * from tbAdvertisementTiming";
                    DataSet dsOption1 = new DataSet();
                    dsOption1 = ObjMainClass.fnFillDataSet(strOpt);
                    StaticClass.AdvtTime = Convert.ToInt32(dsOption1.Tables[0].Rows[0]["AdvtTime"]);

                    //if (chkRemember.Checked == true)
                    //{
                    //    Properties.Settings.Default.RememberMeUsername = txtloginUserName.Text;
                    //    Properties.Settings.Default.RememberMePassword = txtLoginPassword.Text;
                    //    Properties.Settings.Default.Save();
                    //}
                    //else
                    //{
                    //    Properties.Settings.Default.RememberMeUsername = "";
                    //    Properties.Settings.Default.RememberMePassword = "";
                    //    Properties.Settings.Default.Save();
                    //}

                }

                else
                {
                    DataTable dtPlaylist = new DataTable();
                    DataTable dtSong = new DataTable();
                    string LocalSongFind = "No";
                    string str = "select * from Playlists where tokenid= " + StaticClass.TokenId;
                    dtPlaylist = ObjMainClass.fnFillDataTable_Local(str);
                    if ((dtPlaylist.Rows.Count == 0))
                    {
                        MessageBox.Show("Local are songs not found.Please connect your internet to download the songs", "Player");
                        return;
                    }
                    for (int iCtr = 0; (iCtr <= (dtPlaylist.Rows.Count - 1)); iCtr++)
                    {
                        string stSong = "select * from TitlesInPlaylists where PlaylistID= " + dtPlaylist.Rows[iCtr]["playlistId"];
                        dtSong = ObjMainClass.fnFillDataTable_Local(stSong);
                        if ((dtSong.Rows.Count > 0))
                        {
                            LocalSongFind = "Yes";
                            break;
                        }
                    }
                    if (LocalSongFind == "No")
                    {
                        MessageBox.Show("Local are songs not found.Please connect your internet to download the songs", "Player");
                        return;
                    }

                }
                frmVideoPlayer objMainWindow = new frmVideoPlayer();
                objMainWindow.Show();
                this.Hide();


            }
        }




        private bool isValidConnection(string url, string user, string password)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Credentials = new NetworkCredential(user, password);
                request.GetResponse();
            }
            catch (WebException ex)
            {
                 
                return false;
            }
            return true;
        }
        private void btnExtra_Click(object sender, EventArgs e)
        {

            String RemoteFtpPath = "http://85.195.82.94/mp3files/183881.mp4";
            String LocalDestinationPath = Application.StartupPath + "\\183881.mp4";
            //FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(FileLocation);
            //requestFileDownload.Credentials = new NetworkCredential("FtpParas", "moh!@#123");
            //requestFileDownload.UsePassive = false;
            //requestFileDownload.Method = WebRequestMethods.Ftp.DownloadFile;
            //FtpWebResponse responseFileDownload = (FtpWebResponse)requestFileDownload.GetResponse();
            //Stream responseStream = responseFileDownload.GetResponseStream();
            //FileStream writeStream = new FileStream(localPath, FileMode.Create);
            //wcDownloadSplSongs.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wcDownloadSplSongs_DownloadProgressChanged);
            //wcDownloadSplSongs.DownloadFileCompleted += new AsyncCompletedEventHandler(wcDownloadSplSongs_DownloadFileCompleted);


            WebClient client = new WebClient();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_Download);
            client.DownloadFileAsync(new Uri(RemoteFtpPath), LocalDestinationPath);
            
            
        }
        void client_Download(object sender, AsyncCompletedEventArgs e)
        {
             
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Maximum = (int)e.TotalBytesToReceive / 100;
            progressBar1.Value = (int)e.BytesReceived / 100;
        }
        public static bool TableExists(string table)
        {
            if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
            StaticClass.LocalCon.Open();
            return StaticClass.LocalCon.GetSchema("Tables", new string[4] { null, null, table, "TABLE" }).Rows.Count > 0;
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
            //string strOpt = "";
            //string VersionApplicationPath = "";
            //if (e.KeyCode == Keys.Enter)
            //{
            //    SubmitValidation();
            //    CheckPlayerUpdateVersion();
            //    if (SubmitValidate == "True")
            //    {
            //        strOpt = "select * from users where userid=" + StaticClass.TokenUserId;
            //        DataSet dsOption = new DataSet();
            //        dsOption = ObjMainClass.fnFillDataSet(strOpt);
            //        StaticClass.MainwindowMessage = StaticClass.MainwindowMessage + " (" + dsOption.Tables[0].Rows[0]["Useremail"].ToString() + ")";


            //        strOpt = "";
            //        strOpt = "select * from tbAdvertisementTiming";
            //        DataSet dsOption1 = new DataSet();
            //        dsOption1 = ObjMainClass.fnFillDataSet(strOpt);
            //        StaticClass.AdvtTime = Convert.ToInt32(dsOption1.Tables[0].Rows[0]["AdvtTime"]);

            //        if (chkRemember.Checked == true)
            //        {
            //            //  Properties.Settings.Default.RememberMeUsername = txtloginUserName.Text;
            //            ///   Properties.Settings.Default.RememberMePassword = txtLoginPassword.Text;
            //            ///     Properties.Settings.Default.Save();
            //        }
            //        else
            //        {
            //            //   Properties.Settings.Default.RememberMeUsername = "";
            //            //   Properties.Settings.Default.RememberMePassword = "";
            //            //    Properties.Settings.Default.Save();
            //        }

            //        //string proc = Process.GetCurrentProcess().ProcessName;
            //        //Process[] processes = Process.GetProcessesByName(proc);
            //        //if (processes.Length > 1)
            //        //{
            //        //    Process.GetCurrentProcess().Kill();
            //        //} 

            //        //VersionApplicationPath = Application.StartupPath + "\\MusicPlayer.exe";
            //        //System.Diagnostics.Process.Start(VersionApplicationPath);

            //        DamPlayer objMainWindow = new DamPlayer();
            //        objMainWindow.Show();

            //        this.Hide();
            //    }
            //}

        }
        private void CheckPlayerUpdateVersion()
        {
            try
            {
                string strOldVersion = "";
                string FileLocation = "";
                string strUpdateVersion = "";
                string VersionApplicationPath = "";
                DateTime VersionAvailbleDate;
                DateTime CurrentDate = DateTime.Now.Date;
                Int64 OldVersion = 0;
                Int64 UpdateVersion = 0;
                DataTable dtOldVersion = new DataTable();
                DataTable dtUpdateVersion = new DataTable();
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                strOldVersion = "select isnull(IsUpdated,0) as PlayerVersion from AMPlayerTokens where tokenid =" + StaticClass.TokenId;
                dtOldVersion = ObjMainClass.fnFillDataTable(strOldVersion);

                strUpdateVersion = "select * from tbPlayerUpdateVersion where UpdateId in(select MAX(UpdateId) from tbPlayerUpdateVersion where musictype='NativeCR') and musictype='NativeCR'";
                dtUpdateVersion = ObjMainClass.fnFillDataTable(strUpdateVersion);
                if (dtUpdateVersion.Rows.Count > 0)
                {
                    OldVersion = Convert.ToInt32(dtOldVersion.Rows[0]["PlayerVersion"]);
                    UpdateVersion = Convert.ToInt32(dtUpdateVersion.Rows[0]["UpdateId"]);
                    VersionAvailbleDate = Convert.ToDateTime(dtUpdateVersion.Rows[0]["AviableDate"]);
                    FileLocation = dtUpdateVersion.Rows[0]["FileLocation"].ToString();

                    if (VersionAvailbleDate > CurrentDate) return;
                    if (OldVersion < UpdateVersion)
                    {

                        VersionApplicationPath = Application.StartupPath + "\\UpdateSFVideoPlayer.exe";

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

                        string localPath = Application.StartupPath + "\\UpdateSFVideoPlayer.exe";
                        string UpdateFileLocation = "ftp://134.119.178.26:21/Application/UpdatePatch/UpdateSFVideoPlayer.exe";


                        try
                        {

                            FtpWebRequest requestFileDownload = (FtpWebRequest)WebRequest.Create(UpdateFileLocation);
                            requestFileDownload.Credentials = new NetworkCredential("ftpAdminAll", "Admin@All007");
                            requestFileDownload.KeepAlive = true;
//                            requestFileDownload.UseBinary = true;
  //                          requestFileDownload.UsePassive = false;
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
                        catch (Exception ex)
                        {
                             
                        }

                        #endregion

                        System.Diagnostics.Process.Start(VersionApplicationPath);
                        Process[] prs = Process.GetProcesses();
                        foreach (Process pr in prs)
                        {
                            if (pr.ProcessName == "SFVideoPlayer")
                                pr.Kill();
                        }
                    }
                }
                //}
            }
            catch (Exception ex)
            {
                 
            }

        }
        private void Clientlogin_Move(object sender, EventArgs e)
        {

        }
        private void CheckIfRememberedUser()
        {
            //if (Properties.Settings.Default.RememberMeUsername != null && Properties.Settings.Default.RememberMeUsername != "")
            //{
            //    txtloginUserName.Text = Properties.Settings.Default.RememberMeUsername;
            //    txtLoginPassword.Text = Properties.Settings.Default.RememberMePassword;
            //    chkRemember.Checked = true;
            //}
        }

        private void Clientlogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            //MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            //DialogResult result;
            //result = MessageBox.Show("Are you sure to exit ?", "Player", buttons);
            //if (result == System.Windows.Forms.DialogResult.Yes)
            //{
            Application.Exit();
            //}
            //else
            //{
            //    e.Cancel = true;
            //}
        }

        private void panAdvt_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.parastechnologies.com/");
        }



        private void timAutoStart_Tick(object sender, EventArgs e)
        {

            lblCurrentTime.Text = Convert.ToString(Convert.ToInt32(lblCurrentTime.Text) - 1);
            if (lblCurrentTime.Text == "0")
            {
                timAutoStart.Enabled = false;
                string strOpt = "";

                if (ObjMainClass.CheckForInternetConnection() == true)
                {
                    CheckPlayerUpdateVersion();
                    SubmitValidation();
                    if (SubmitValidate == "True")
                    {
                        strOpt = "select * from users where userid=" + StaticClass.TokenUserId;
                        DataSet dsOption = new DataSet();
                        dsOption = ObjMainClass.fnFillDataSet(strOpt);
                        StaticClass.MainwindowMessage = StaticClass.MainwindowMessage + " (" + dsOption.Tables[0].Rows[0]["Useremail"].ToString() + ")";


                        strOpt = "";
                        strOpt = "select * from tbAdvertisementTiming";
                        DataSet dsOption1 = new DataSet();
                        dsOption1 = ObjMainClass.fnFillDataSet(strOpt);
                        StaticClass.AdvtTime = Convert.ToInt32(dsOption1.Tables[0].Rows[0]["AdvtTime"]);


                    }
                }
                else
                {
                    DataTable dtPlaylist = new DataTable();
                    DataTable dtSong = new DataTable();
                    string LocalSongFind = "No";
                    string str = "select * from Playlists where tokenid= " + StaticClass.TokenId;
                    dtPlaylist = ObjMainClass.fnFillDataTable_Local(str);
                    if ((dtPlaylist.Rows.Count == 0))
                    {
                        lblError.Visible = true;
                        lblCurrentTime.Text = "10";
                        timAutoStart.Enabled = true;
                        return;
                    }
                    for (int iCtr = 0; (iCtr <= (dtPlaylist.Rows.Count - 1)); iCtr++)
                    {
                        string stSong = "select * from TitlesInPlaylists where PlaylistID= " + dtPlaylist.Rows[iCtr]["playlistId"];
                        dtSong = ObjMainClass.fnFillDataTable_Local(stSong);
                        if ((dtSong.Rows.Count > 0))
                        {
                            LocalSongFind = "Yes";
                            break;
                        }
                    }
                    if (LocalSongFind == "No")
                    {
                        lblError.Visible = true;
                        lblCurrentTime.Text = "10";
                        timAutoStart.Enabled = true;
                        return;
                    }

                }
                frmVideoPlayer objMainWindow = new frmVideoPlayer();
                objMainWindow.Show();
                this.Hide();



            }
        }


    }
}
