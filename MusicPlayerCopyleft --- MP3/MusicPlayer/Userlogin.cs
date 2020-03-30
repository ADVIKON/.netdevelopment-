using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Data.OleDb;
namespace MusicPlayerCopyleft
{

    public partial class userlogin : Form
    {
        gblClass ObjMainClass = new gblClass();
        string ExpiryDamStatus = "";
        Int32 LeftDamDays = 0;
        Thread t2;
        Int32 LocalDealerDfClientId = 0;
        string IsMailSend = "";
        string strOpt = "";
        string str = "";
        string CurrentTokenDealerCode = "";
        string TokenDealerCode = "MyClaud000";
        public userlogin()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            IsMailSend = "No";
            lblWait.Visible = true;
            lblWait.Height = 220;
            Userlogin();
        }
        private void Userlogin()
        {
            DataSet dsDefault = new DataSet();
            DataSet ds = new DataSet();
            Int32 dfId = 0;
            TokenDealerCode = "MyClaud000";


            if (txtUsername.Text == "" || txtTokenNo.Text == "")
            {
                lblWait.Visible = false;
                MessageBox.Show("The user name and token cannot be empty", "CopyleftPlayer");
                return;
            }
            if (txtDealerCode.Text.Trim() != "" && txtDealerCode.Text != "MyClaud000")
            {
                str = "";
                str = "select * from DFClients where isdealer=1 and  ClientName='" + txtUsername.Text.Trim() + "'";
                dsDefault = ObjMainClass.fnFillDataSet(str);
                if (dsDefault.Tables[0].Rows.Count > 0)
                {
                    if (txtDealerCode.Text.ToUpper() != dsDefault.Tables[0].Rows[0]["DealerCode"].ToString())
                    {
                        lblWait.Visible = false;
                        MessageBox.Show("The dealer code you have inserted is wrong.", "CopyleftPlayer");
                        txtDealerCode.Focus();
                        return;
                    }
                    else if (txtDealerCode.Text.ToUpper() == dsDefault.Tables[0].Rows[0]["DealerCode"].ToString())
                    {
                        TokenDealerCode = dsDefault.Tables[0].Rows[0]["DealerCode"].ToString();
                        dfId = Convert.ToInt32(dsDefault.Tables[0].Rows[0]["DFClientId"]);
                    }
                }
            }
            if (TokenDealerCode == "MyClaud000")
            {
                str = "";
                str = "select * from DFClients where isdealer=1 and  ClientName='" + txtUsername.Text.Trim() + "'";
                dsDefault = ObjMainClass.fnFillDataSet(str);
                if (dsDefault.Tables[0].Rows.Count > 0)
                {
                    TokenDealerCode = dsDefault.Tables[0].Rows[0]["DealerCode"].ToString();
                    dfId = Convert.ToInt32(dsDefault.Tables[0].Rows[0]["DFClientId"]);
                }
            }
            if (dfId != 0)
            {
                //if (GetDealerPendingTokens(dfId) == 0)
                //{
                //    MessageBox.Show("You have reached the maximum of your tokens. Please contact your administrator.", "CopyleftPlayer");
                //    txtUsername.Focus();
                //    return;
                //}
            }
            str = "spGetTokenRights '" + txtUsername.Text + "', '" + txtTokenNo.Text + "' ";
            ds = new DataSet();
            ds = ObjMainClass.fnFillDataSet(str);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                lblWait.Visible = false;
                MessageBox.Show("Your username or token is wrong.", "CopyleftPlayer");
                return;
            }
            else if (ds.Tables[0].Rows[0]["PlayerType"].ToString() != "Desktop")
            {
                lblWait.Visible = false;
                MessageBox.Show("This token is not matching with this player type.", "CopyleftPlayer");
                return;
            }
            
            StaticClass.dfClientId = ds.Tables[0].Rows[0]["ClientID"].ToString();
            StaticClass.TotalTitles = ds.Tables[0].Rows[0]["NumberofTitles"].ToString();
            StaticClass.TokenId = ds.Tables[0].Rows[0]["tokenid"].ToString();
            str = "";
            str = "Update AMPlayerTokens set Code='" + GenerateId.getKey(GenerateId._wvpaudi) + "'  , " +
                "  pVersion='Normal',  DateTokenUsed=getdate()   , Token='used' ,DealerCode='" + TokenDealerCode + "' " +
                " where TokenId=" + StaticClass.TokenId + "";
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = StaticClass.constr;
            cmd.CommandText = str;
            cmd.ExecuteNonQuery();
            StaticClass.constr.Close();
            CreateFile(ds.Tables[0].Rows[0]["tokenid"].ToString());
            str = "";
            str = "select * from tbluser_client_rights where userid=" + StaticClass.dfClientId + " and isAdmin=1";
            dsDefault = ObjMainClass.fnFillDataSet(str);
            if (dsDefault.Tables[0].Rows.Count <= 0)
            {
                SaveDefaultUser();
            }
            CheckClientsRights();
            lblWait.Visible = false;
            Clientlogin objClientlogin = new Clientlogin();
            objClientlogin.Show();
            this.Hide();
        }

        private void DeleteSongs(string d)
        {
            foreach (string f in Directory.GetFiles(d, "*.sec"))
            {
                try
                {
                    File.Delete(Application.StartupPath + "\\" + Path.GetFileName(f));
                }
                catch (Exception ex)
                {
                }
            }

        }
        private void DeleteDBTable(string tableName)
        {
            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            OleDbCommand cmdLocal = new OleDbCommand();
            cmdLocal.Connection = StaticClass.LocalCon;
            cmdLocal.CommandText = "delete from " + tableName + " ";
            cmdLocal.ExecuteNonQuery();
            StaticClass.LocalCon.Close();
        }

        private Int64 GetDealerPendingTokens(Int32 dfclientid)
        {
            string sQr = "";
            DataSet ds = new DataSet();
            Int64 PendingTokens = 0;

            string strOpt = "select * from tbdealerlogin where dfclientid=" + dfclientid;
            DataSet dsOption = new DataSet();
            dsOption = ObjMainClass.fnFillDataSet(strOpt);

            sQr = "select count(*) as UsedToken from AMPlayerTokens where clientid=" + dfclientid + "  and isDam=1 and (pversion='NativeCL' or iif(pversion is null,'Normal',pVersion)='Normal') and token='used'";
            ds = ObjMainClass.fnFillDataSet(sQr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                PendingTokens = (Convert.ToInt32(dsOption.Tables[0].Rows[0]["DamTotalToken"])) - (Convert.ToInt32(ds.Tables[0].Rows[0]["UsedToken"]));
                return PendingTokens;
            }
            else
            {

                PendingTokens = 0;
                return PendingTokens;
            }

        }
        private void SetExpiryDate(Int32 Dam, Int32 Sanjivani, Int32 Thai)
        {
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("spSetTokenUseExpiryDate_Copyright", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
            cmd.Parameters["@TokenId"].Value = Convert.ToInt32(StaticClass.TokenId);
            cmd.Parameters.Add(new SqlParameter("@IsCopyright", SqlDbType.Int));
            cmd.Parameters["@IsCopyright"].Value = 1;
            cmd.ExecuteNonQuery();
            StaticClass.constr.Close();
        }
        private void CheckClientsRights()
        {
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();

            strOpt = "select ISNULL(IsSanjivani,0) as Sanjivani, ISNULL(IsDam,0) as Dam , isnull(IsStream,0) as Stream , dealerCode, ISNULL(IsThai,0) as Thai, ISNULL(IsChina,0) as China, ISNULL(IsAdvt,0) as Advt, ISNULL(IsAdvtManual,0) as AdvtManual, ISNULL(IsBlockAdvt,0) as IsBlockAdvt , isnull(serviceid,0) as serviceid , userid , isnull(cityid,0) as CityId , isnull(StateId,0) as StateId , isnull(CountryId,0) as CountryId from AMPlayerTokens where TokenID=" + StaticClass.TokenId;
            DataSet dsOption = new DataSet();
            dsOption = ObjMainClass.fnFillDataSet(strOpt);

            SetExpiryDate(Convert.ToInt32(dsOption.Tables[0].Rows[0]["Dam"]), Convert.ToInt32(dsOption.Tables[0].Rows[0]["Sanjivani"]), Convert.ToInt32(dsOption.Tables[0].Rows[0]["Thai"]));

            StaticClass.DealerCode = dsOption.Tables[0].Rows[0]["DealerCode"].ToString();
            StaticClass.TokenServiceId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["serviceid"]);
            StaticClass.IsAdvtManual = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["AdvtManual"]);
            StaticClass.IsAdvt = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["Advt"]);
            StaticClass.IsBlockAdvt = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["IsBlockAdvt"]);

            StaticClass.TokenUserId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["userid"]);
            StaticClass.AdvtCityId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["CityId"]);
            StaticClass.Stateid = Convert.ToInt32(dsOption.Tables[0].Rows[0]["Stateid"]);
            StaticClass.CountryId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["CountryId"]);

            str = "spGetTokenExpiryStatus_Copyright " + StaticClass.TokenId + ", 1,  0 , 1";

            DataSet dsExpire = new DataSet();
            dsExpire = ObjMainClass.fnFillDataSet(str);


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
                MessageBox.Show("!! The license of your music player is expired. Please contact your dealer. !!" + Environment.NewLine + "Your player id: " + StaticClass.TokenId + "", "Music Player");
                Application.Exit();
                return;
            }

            if (ExpiryDamStatus != "NoLic" && LeftDamDays <= 10)
            {
                StaticClass.PlayerExpiryMessage = Convert.ToString(LeftDamDays) + " days left to renewal of subscription.";
                StaticClass.IsDam = true;
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
                MessageBox.Show("!! Subscription is Expired.Please connect your vendor !! " + Environment.NewLine + "Your player id: " + StaticClass.TokenId + "", "Music Player");
                Application.Exit();
                return;
            }
        }
        private void SaveDefaultUser()
        {
            try
            {
                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand("Insert_User_Client_Rights", StaticClass.constr);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt));
                cmd.Parameters["@UserID"].Value = Convert.ToInt32(StaticClass.dfClientId);

                cmd.Parameters.Add(new SqlParameter("@clientname", SqlDbType.NVarChar));
                cmd.Parameters["@clientname"].Value = "admin";

                cmd.Parameters.Add(new SqlParameter("@clientPassword", SqlDbType.NVarChar));
                cmd.Parameters["@clientPassword"].Value = "admin";
                cmd.Parameters.Add(new SqlParameter("@isRemove", SqlDbType.Int));
                cmd.Parameters["@isRemove"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@isDownload", SqlDbType.Int));
                cmd.Parameters["@isDownload"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@isAdmin", SqlDbType.Int));
                cmd.Parameters["@isAdmin"].Value = 1;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void userlogin_Load(object sender, EventArgs e)
        {
            this.Icon = Properties.Resources.DamIcon;
            lblWait.Visible = false;
            UpdateLocalDatabase();
        }
        public static bool TableExists(string table)
        {
            if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
            StaticClass.LocalCon.Open();
            return StaticClass.LocalCon.GetSchema("Tables", new string[4] { null, null, table, "TABLE" }).Rows.Count > 0;
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

        private void CreateFile(string TokenId)
        {
            string fileName = Application.StartupPath + "\\tid.amp";

            try
            {
                // Check if file already exists. If yes, delete it. 
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file 
                using (FileStream fs = File.Create(fileName))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes(TokenId);
                    fs.Write(title, 0, title.Length);
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        private void btnExtra_Click(object sender, EventArgs e)
        {

        }

        private void btnExtra_Click_1(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void picManual_Click(object sender, EventArgs e)
        {
            string file;
            file = Application.StartupPath + "\\ICL.mht";
            if (!File.Exists(file))
            {
                #region DownloadPdfFile
                string localPath = Application.StartupPath + "\\ICL.mht";
                string UpdateFileLocation = "ftp://85.195.82.94:21//MusicPlayerSetup/pdfFile/ICL.mht";
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
                catch (Exception ex)
                { }

                #endregion
            }
            String appdir = Path.GetDirectoryName(Application.ExecutablePath);
            String myfile = Path.Combine(appdir, "ICL.mht");
            webManual.Url = new Uri("file:///" + myfile);
            this.WindowState = FormWindowState.Maximized;
            panManual.Dock = DockStyle.Fill;
            panManual.Visible = true;
        }

        private void picBack_Click(object sender, EventArgs e)
        {
            panManual.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void userlogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        //#region "Encrpt-Dcrpt PDF"
        //using System.Security.Cryptography;
        //private void EncryptFile(string inputFile, string outputFile)
        //{

        //    try
        //    {
        //        string password = @"myKey123"; // Your Key Here
        //        UnicodeEncoding UE = new UnicodeEncoding();
        //        byte[] key = UE.GetBytes(password);

        //        string cryptFile = outputFile;
        //        FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

        //        RijndaelManaged RMCrypto = new RijndaelManaged();

        //        CryptoStream cs = new CryptoStream(fsCrypt,
        //            RMCrypto.CreateEncryptor(key, key),
        //            CryptoStreamMode.Write);

        //        FileStream fsIn = new FileStream(inputFile, FileMode.Open);

        //        int data;
        //        while ((data = fsIn.ReadByte()) != -1)
        //            cs.WriteByte((byte)data);


        //        fsIn.Close();
        //        cs.Close();
        //        fsCrypt.Close();
        //    }
        //    catch
        //    {
        //        MessageBox.Show("Encryption failed!", "Error");
        //    }
        //}
        //private void DecryptFile(string inputFile, string outputFile)
        //{

        //    {
        //        string password = @"myKey123"; // Your Key Here

        //        UnicodeEncoding UE = new UnicodeEncoding();
        //        byte[] key = UE.GetBytes(password);

        //        FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

        //        RijndaelManaged RMCrypto = new RijndaelManaged();

        //        CryptoStream cs = new CryptoStream(fsCrypt,
        //            RMCrypto.CreateDecryptor(key, key),
        //            CryptoStreamMode.Read);

        //        FileStream fsOut = new FileStream(outputFile, FileMode.Create);

        //        int data;
        //        while ((data = cs.ReadByte()) != -1)
        //            fsOut.WriteByte((byte)data);

        //        fsOut.Close();
        //        cs.Close();
        //        fsCrypt.Close();

        //    }
        //}
        //#endregion
    }

}


