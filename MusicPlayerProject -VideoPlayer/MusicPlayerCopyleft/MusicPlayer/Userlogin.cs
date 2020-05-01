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
namespace SFVideoPlayer
{
    
    public partial class userlogin : Form
    {
        gblClass ObjMainClass = new gblClass();
        string ExpiryCopyrightStatus = "";
        Int64 LeftCopyrightDays = 0;
        Thread t2;
        string strOpt = "";
        string str = "";
        string TokenDealerCode = "AlenkaMyclaud";
        public userlogin()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
           
            lblWait.Visible = true;
            lblWait.Height = 220;
            Userlogin();
        }
        private void Userlogin()
        {
            DataSet dsDefault = new DataSet();
            DataSet ds = new DataSet();
            Int32 dfId = 0;
            TokenDealerCode = "AlenkaMyclaud";


            if (txtUsername.Text == "" || txtTokenNo.Text == "")
            {
                lblWait.Visible = false;
                MessageBox.Show("Username and Token no cannot be blank", "Player");
                return;
            }
            if (txtDealerCode.Text.Trim() != "" && txtDealerCode.Text != "AlenkaMyclaud")
            {
                str = "";
                str = "select * from DFClients where isdealer=1 and  ClientName='" + txtUsername.Text.Trim() + "'";
                dsDefault = ObjMainClass.fnFillDataSet(str);
                if (dsDefault.Tables[0].Rows.Count > 0)
                {
                    if (txtDealerCode.Text.ToUpper() != dsDefault.Tables[0].Rows[0]["DealerCode"].ToString())
                    {
                        lblWait.Visible = false;
                        MessageBox.Show("The dealer code you have inserted is wrong.", "Player");
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
            if (TokenDealerCode == "AlenkaMyclaud")
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
                if (GetDealerPendingTokens(dfId) == 0)
                {
                    //MessageBox.Show("You have reached the maximum of your tokens. Please contact your administrator.", "Player");
                    //txtUsername.Focus();
                    //return;
                }
            }
            str = "spGetTokenRights '" + txtUsername.Text + "', '" + txtTokenNo.Text + "' ";
            ds = new DataSet();
            ds = ObjMainClass.fnFillDataSet(str);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                lblWait.Visible = false;
                MessageBox.Show("Your username or token is wrong.", "Player");
                return;
            }
            else if (ds.Tables[0].Rows[0]["PlayerType"].ToString() != "Desktop")
            {
                lblWait.Visible = false;
                MessageBox.Show("This token is not matching with this license type.", "Player");
                return;
            }
            //else if ((ds.Tables[0].Rows[0]["MusicType"].ToString() != "Copyright") && (ds.Tables[0].Rows[0]["MusicType"].ToString() != "NativeCR"))
            //{
            //    lblWait.Visible = false;
            //    MessageBox.Show("This token is not matching with this music type.", "Player");
            //    return;
            //}
            StaticClass.dfClientId = ds.Tables[0].Rows[0]["ClientID"].ToString();
            StaticClass.TokenId = ds.Tables[0].Rows[0]["tokenid"].ToString();
            str = "";
            str = "Update AMPlayerTokens set Code='" + GenerateId.getKey(GenerateId._wvpaudi) + "'  , " +
                "  pVersion='NativeCR',  DateTokenUsed=getdate()   , Token='used' ,DealerCode='" + TokenDealerCode + "' " +
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
            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            OleDbCommand cmdLocal = new OleDbCommand();
            cmdLocal.Connection = StaticClass.LocalCon;
            cmdLocal.CommandText = "insert into tbMisc(DealerCode,IsStore,DfClientId,IsAdvt,IsLock,ScheduleType)  values('" + StaticClass.DealerCode + "'," + Convert.ToByte(StaticClass.IsStore) + "," + StaticClass.dfClientId + "," + Convert.ToByte(StaticClass.IsAdvt) + "," + Convert.ToByte(StaticClass.IsLock) + ",'" + StaticClass.ScheduleType + "')";
            cmdLocal.ExecuteNonQuery();
            StaticClass.LocalCon.Close();
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

            sQr = "select count(*) as UsedToken from AMPlayerTokens where clientid=" + dfclientid + "  and iscopyright=1 and (pversion='NativeCR' or iif(pversion is null,'Normal',pVersion)='Normal') and token='used'";
            ds = ObjMainClass.fnFillDataSet(sQr);
            if (ds.Tables[0].Rows.Count > 0)
            {
                PendingTokens = (Convert.ToInt32(dsOption.Tables[0].Rows[0]["CopyrightTotalToken"])) - (Convert.ToInt32(ds.Tables[0].Rows[0]["UsedToken"]));
                return PendingTokens;
            }
            else
            {

                PendingTokens = 0;
                return PendingTokens;
            }

        }
        private void SetExpiryDate(Int64 Copyright)
        {
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("spSetTokenUseExpiryDate_Copyright", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
            cmd.Parameters["@TokenId"].Value = Convert.ToInt32(StaticClass.TokenId);
            cmd.Parameters.Add(new SqlParameter("@IsCopyright", SqlDbType.Int));
            cmd.Parameters["@IsCopyright"].Value = Copyright;
            cmd.ExecuteNonQuery();
            StaticClass.constr.Close();
        }
        private void CheckClientsRights()
        {
            string strOpt = "";
            string str = "";
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();


            strOpt = "select ISNULL(IsCopyright,0) as Copyright, ISNULL(IsFitness,0) as Fitness, isnull(IsStream,0) as Stream,dealerCode, ISNULL(IsAdvt,0) as Advt, ISNULL(IsAdvtManual,0) as AdvtManual, ISNULL(IsBlockAdvt,0) as IsBlockAdvt, isnull(serviceid,0) as serviceid , isnull(cityid,0) as CityId  , isnull(StateId,0) as StateId, isnull(CountryId,0) as CountryId, UserId , isnull(isStore,0) as isStore ,isnull(IsStopControl,0) as IsLock, isnull(IsVedioActive,0) as VedioActive , isnull(AMPlayerTokens.ScheduleType,'0') as ScheduleType from AMPlayerTokens where TokenID=" + StaticClass.TokenId;
            DataSet dsOption = new DataSet();
            dsOption = ObjMainClass.fnFillDataSet(strOpt);

            StaticClass.TokenServiceId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["serviceid"]);
            StaticClass.DealerCode = dsOption.Tables[0].Rows[0]["DealerCode"].ToString();

            StaticClass.IsAdvtManual = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["AdvtManual"]);
            StaticClass.IsAdvt = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["Advt"]);
            StaticClass.IsBlockAdvt = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["IsBlockAdvt"]);
            StaticClass.AdvtCityId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["CityId"]);
            StaticClass.Stateid = Convert.ToInt32(dsOption.Tables[0].Rows[0]["Stateid"]);
            StaticClass.CountryId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["CountryId"]);
            StaticClass.TokenUserId = Convert.ToInt32(dsOption.Tables[0].Rows[0]["userid"]);
            StaticClass.IsStore = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["IsStore"]);
            StaticClass.IsLock = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["IsLock"]);
            StaticClass.IsVedioActive = Convert.ToBoolean(dsOption.Tables[0].Rows[0]["VedioActive"]);
            StaticClass.ScheduleType = dsOption.Tables[0].Rows[0]["ScheduleType"].ToString();

            SetExpiryDate(Convert.ToInt32(dsOption.Tables[0].Rows[0]["Copyright"]));


            str = "spGetTokenExpiryStatus_Copyright " + Convert.ToInt32(StaticClass.TokenId) + ", " + dsOption.Tables[0].Rows[0]["Copyright"] + ", " + dsOption.Tables[0].Rows[0]["Fitness"] + ", " + dsOption.Tables[0].Rows[0]["Stream"];
            DataSet dsExpire = new DataSet();
            dsExpire = ObjMainClass.fnFillDataSet(str);


            ExpiryCopyrightStatus = dsExpire.Tables[0].Rows[0]["ExpiryCopyrightStatus"].ToString();
            LeftCopyrightDays = Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftCopyrightDays"]);

            StaticClass.StreamExpiryMessage = dsExpire.Tables[0].Rows[0]["ExpiryStreamStatus"].ToString();
            StaticClass.LeftStreamtDays = Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftStreamDays"]);

            if (ExpiryCopyrightStatus == "NoLic" )
            {
                MessageBox.Show("!! Purchase the subscription of music player  !!", "Player");
                Application.Exit();
                return;
            }
            if (ExpiryCopyrightStatus == "Yes" )
            {
                MessageBox.Show("!! The license of your music player is expired. Please contact your dealer. !!", "Player");
                Application.Exit();
                return;
            }

            if (ExpiryCopyrightStatus != "NoLic" && LeftCopyrightDays <= 10)
            {
                StaticClass.PlayerExpiryMessage = Convert.ToString(LeftCopyrightDays) + " days left to renewal of Copyright subscription";
                StaticClass.IsCopyright = true;
            }
            else if (ExpiryCopyrightStatus != "NoLic" && LeftCopyrightDays == 0)
            {
                StaticClass.PlayerExpiryMessage = "Last day to renewal of Copyright subscription";
                StaticClass.IsCopyright = true;
            }
            else
            {
                StaticClass.IsCopyright = true;
            }
            if (ExpiryCopyrightStatus == "Yes")
            {
                MessageBox.Show("!! Subscription is Expired.Please connect your vendor !!", "Player");
                Application.Exit();
                return;
            }
            else if (ExpiryCopyrightStatus == "Yes")
            {
                StaticClass.PlayerExpiryMessage = "Subscription is Expired.Please connect your vendor";
                StaticClass.IsCopyright = false;
            }
            else if (ExpiryCopyrightStatus == "NoLic")
            {
                StaticClass.PlayerExpiryMessage = "You do not have license";
                StaticClass.IsCopyright = false;
            }
           
            StaticClass.StreamExpiryMessage = dsExpire.Tables[0].Rows[0]["ExpiryStreamStatus"].ToString();
            StaticClass.LeftStreamtDays = Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftStreamDays"]);
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
            this.Icon = Properties.Resources.more;
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
            if (TableExists("tbLastPosition") == false)
            {
                strInsert = "CREATE TABLE tbLastPosition([tokenid] number NULL, [LastPostion] text NULL )";
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
            if (TableExists("tbSplPlaylistSchedule") == false)
            {
                strInsert = "CREATE TABLE tbSplPlaylistSchedule([SchId] number NULL , [splPlaylistId] number NULL,	[StartTime] DateTime NULL,	[EndTime] DateTime NULL, [splName] Text NULL,[PlaylistId] number NULL )";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (TableExists("tbSplPlaylistSchedule_Weekday") == false)
            {
                strInsert = "CREATE TABLE tbSplPlaylistSchedule_Weekday([SchId] number NULL , 	[wId] number NULL,	[IsAllWeek] number NULL )";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (TableExists("tbSpecialPlaylists_Titles") == false)
            {
                strInsert = "CREATE TABLE tbSpecialPlaylists_Titles([SchId] number NULL , 	[titleId] number NULL, [isDownload] number NULL,[Title] text null,[AlbumID] number NULL,[ArtistID] number NULL,[Time] text NULL,[arName] text NULL,[alName] text NULL)";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (DoesFieldExist("Playlists", "IsSpl", StaticClass.LocalCon.ConnectionString.ToString()) == false)
            {
                strInsert = "ALTER TABLE Playlists ADD IsSpl number";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (TableExists("tbMisc") == false)
            {
                strInsert = "CREATE TABLE tbMisc([DealerCode] text NULL, 	[IsStore] number NULL, 	[DfClientId] number NULL, [IsAdvt] number NULL )";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }

            if (DoesFieldExist("tbMisc", "IsVideoPlayer", StaticClass.LocalCon.ConnectionString.ToString()) == false)
            {
                strInsert = "ALTER TABLE tbMisc ADD IsVideoPlayer number";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }

            ObjMainClass.UpdateLocalDatabase();
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

        private void userlogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    } 
    
}


