using System;
using System.Web;
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
using System.Data.OleDb;
using System.Net.NetworkInformation;

namespace SFVideoPlayer
{

    class gblClass
    {

      
       public string MainMessage = "You are not authrosied user";
       



       public void fnFillComboBox(string mlsSql, ComboBox Combo, string ValMember, string DispMember, string displayTextAtZeroIndex = "")
       {
           try
           {
               DataSet ds = new DataSet();
               // Warning!!! Optional parameters not supported
               DataRow dr;
               ds = fnFillDataSet(mlsSql);
              // ds.Tables[0].DefaultView.Sort = DispMember;
               Combo.DataSource = null;
               if ((ds.Tables[0].Rows.Count > 0))
               {
                   //dr = ds.Tables[0].NewRow();
                   //dr[ValMember] = 0;
                   //dr[DispMember] = displayTextAtZeroIndex;
                  // ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   //Combo.SelectedValue = 0;
               }
               else
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = "";
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   Combo.SelectedValue = 0;
               }
           }
           catch (Exception ex)
           {
              // MessageBox.Show(ex.Message);
           }
       }
       public void fnFillAdvtComboBox(string mlsSql, ComboBox Combo, string ValMember, string DispMember, string displayTextAtZeroIndex = "")
       {
           try
           {
               DataSet ds = new DataSet();
               // Warning!!! Optional parameters not supported
               DataRow dr;
               ds = fnFillDataSet(mlsSql);
                ds.Tables[0].DefaultView.Sort = DispMember;
               Combo.DataSource = null;
               if ((ds.Tables[0].Rows.Count > 0))
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = displayTextAtZeroIndex;
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   Combo.SelectedValue = 0;
               }
               else
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = "";
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   Combo.SelectedValue = 0;
               }
           }
           catch (Exception ex)
           {
               // MessageBox.Show(ex.Message);
           }
       }

       public DataTable fnFillDataTable(string sSql)
       {
           SqlDataAdapter Adp = new SqlDataAdapter();
           DataTable mldData;
           try
           {
               Adp = new SqlDataAdapter(sSql, StaticClass.constr );
               mldData = new DataTable();
               Adp.Fill(mldData);
           }
           catch (Exception ex)
           {
                mldData = new DataTable();
              // MessageBox.Show(ex.Message);
           }
           return mldData;
       }
       public DataTable fnFillDataTable_Local(string sSql)
       {
           OleDbDataAdapter Adp = new OleDbDataAdapter();
           DataTable mldData;
           try
           {
               Adp = new OleDbDataAdapter(sSql, StaticClass.LocalCon);
               mldData = new DataTable();
               Adp.Fill(mldData);
           }
           catch (Exception ex)
           {
               mldData = new DataTable();
              // MessageBox.Show(ex.Message);
           }
           return mldData;
       }
       public DataTable fnFillDataTable_Local_Events(string sSql)
       {
           OleDbDataAdapter Adp = new OleDbDataAdapter();
           DataTable mldData;
           try
           {
               Adp = new OleDbDataAdapter(sSql, StaticClass.LocalCon);
               mldData = new DataTable();
               Adp.Fill(mldData);
           }
           catch (Exception ex)
           {
               mldData = new DataTable();
               // MessageBox.Show(ex.Message);
           }
           return mldData;
       }
       public DataSet fnFillDataSet(string sQuery)
       {
           SqlDataAdapter Adp = new SqlDataAdapter();
           DataSet mlds;
           try
           {
               Adp = new SqlDataAdapter(sQuery, StaticClass.constr);
               mlds = new DataSet();
               Adp.Fill(mlds);
           }
           catch  (Exception ex)
           {
               mlds = new DataSet();
              // MessageBox.Show(ex.Message);
                
           }
           return mlds;
       }
       public DataSet fnFillDataSet_Local(string sQuery)
       {
           OleDbDataAdapter Adp = new OleDbDataAdapter();
           DataSet mlds;
           try
           {
               Adp = new OleDbDataAdapter(sQuery, StaticClass.LocalCon);
               mlds = new DataSet();
               Adp.Fill(mlds);
           }
           catch(Exception ex)
           {
               mlds = new DataSet();
              // MessageBox.Show(ex.Message);

           }
           return mlds;
       }
       public DataSet fnFillDataSet_Local_Event(string sQuery)
       {
           OleDbDataAdapter Adp = new OleDbDataAdapter();
           DataSet mlds;
           try
           {
               Adp = new OleDbDataAdapter(sQuery, StaticClass.LocalCon);
               mlds = new DataSet();
               Adp.Fill(mlds);
           }
           catch (Exception ex)
           {
               mlds = new DataSet();
               // MessageBox.Show(ex.Message);

           }
           return mlds;
       }

       public  bool CheckForInternetConnection()
       {

           try
           {
              
               using (var client = new WebClient())

               using (var stream = client.OpenRead("http://134.119.178.26"))
               {
                   return true;
               }
           }
           catch
           {
               return false;
           }

           //try
           //{
           //    Ping myPing = new Ping();
           //    String host = "google.com";
           //    byte[] buffer = new byte[32];
           //    int timeout = 1000;
           //    PingOptions pingOptions = new PingOptions();
           //    PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
           //    return true;
           //}
           //catch (Exception)
           //{
           //    return false;
           //}
       }

       public void DeleteAllOgg1(string CurrentTitleId)
       {
           try
           {
               string d = Application.StartupPath + "\\so\\";
               foreach (string f in Directory.GetFiles(d, "*.mp3"))
               {
                   FileInfo objFile = new FileInfo(f);
                   string FileName = objFile.Name;
                   if (CurrentTitleId != FileName)
                   {
                       File.Delete(f);
                   }
               }
           }
           catch (Exception ex) { }
       }
       public void fnFillComboBox_Local(string mlsSql, ComboBox Combo, string ValMember, string DispMember, string displayTextAtZeroIndex = "")
       {
           try
           {
               DataSet ds = new DataSet();
               // Warning!!! Optional parameters not supported
               DataRow dr;
               ds = fnFillDataSet_Local(mlsSql);
               ds.Tables[0].DefaultView.Sort = DispMember;
               Combo.DataSource = null;
               if ((ds.Tables[0].Rows.Count > 0))
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = displayTextAtZeroIndex;
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   //Combo.SelectedValue = 0;
               }
               else
               {
                   dr = ds.Tables[0].NewRow();
                   dr[ValMember] = 0;
                   dr[DispMember] = "";
                   ds.Tables[0].Rows.InsertAt(dr, 0);
                   Combo.ValueMember = ValMember;
                   Combo.DisplayMember = DispMember;
                   Combo.DataSource = ds.Tables[0];
                   Combo.Refresh();
                   Combo.SelectedValue = 0;
               }
           }
           catch (Exception ex)
           {
               // MessageBox.Show(ex.Message);
           }
       }
       public static bool TableExists(string table)
       {
           if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
           StaticClass.LocalCon.Open();
           return StaticClass.LocalCon.GetSchema("Tables", new string[4] { null, null, table, "TABLE" }).Rows.Count > 0;
       }
       public void UpdateLocalDatabase()
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
           if (TableExists("tbSpecialEvent") == false)
           {
               strInsert = "CREATE TABLE tbSpecialEvent([EventId] number NULL, [EventName] text NULL  )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbSpecialEvent_Titles") == false)
           {
               strInsert = "CREATE TABLE tbSpecialEvent_Titles([EventId] number NULL, [titleId] number NULL,[IsDownload] number NULL  )";
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
           if (TableExists("tbAdvertisement") == false)
           {
               strInsert = "CREATE TABLE tbAdvertisement([AdvtId] number NULL,[AdvtDisplayName] Text(250) NULL ,[AdvtCompanyName] Text(250) NULL , ";
               strInsert = strInsert + "[AdvtStartDate] Date NULL ,[AdvtEndDate] Date NULL ,";
               strInsert = strInsert + "[AdvtFilePath] Text(250) NULL ,[AdvtPlayertype] Text(50) NULL ,";
               strInsert = strInsert + "[DfClientId] number NULL ,[CountryCode] number NULL ,";
               strInsert = strInsert + "[TokenId] number NULL ,[AdvtTypeId] number NULL ,";
               strInsert = strInsert + "[AdvtTime] Time NULL ,[StateId] number NULL ,";
               strInsert = strInsert + "[CityId] number NULL ,[Dealercode] Text(150) NULL, [AdvtTypeName] Text(100) NULL, [AdvthttpUrl] Text(250) NULL )";
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
               strInsert = "CREATE TABLE tbMisc([DealerCode] text NULL, 	[IsStore] number NULL, 	[DfClientId] number NULL )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("tbMisc", "IsAdvt", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbMisc ADD IsAdvt number";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbFormat") == false)
           {
               strInsert = "CREATE TABLE tbFormat([FormatId] number NULL, [sTime] Time NULL,[eTime] Time NULL,[Is24Hour] number NULL)";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }

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
           if (TableExists("tbSpecialEvent") == false)
           {
               strInsert = "CREATE TABLE tbSpecialEvent([EventId] number NULL, [EventName] text NULL  )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbSpecialEvent_Titles") == false)
           {
               strInsert = "CREATE TABLE tbSpecialEvent_Titles([EventId] number NULL, [titleId] number NULL,[IsDownload] number NULL  )";
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
           if (TableExists("tbAdvertisement") == false)
           {
               strInsert = "CREATE TABLE tbAdvertisement([AdvtId] number NULL,[AdvtDisplayName] Text(250) NULL ,[AdvtCompanyName] Text(250) NULL , ";
               strInsert = strInsert + "[AdvtStartDate] Date NULL ,[AdvtEndDate] Date NULL ,";
               strInsert = strInsert + "[AdvtFilePath] Text(250) NULL ,[AdvtPlayertype] Text(50) NULL ,";
               strInsert = strInsert + "[DfClientId] number NULL ,[CountryCode] number NULL ,";
               strInsert = strInsert + "[TokenId] number NULL ,[AdvtTypeId] number NULL ,";
               strInsert = strInsert + "[AdvtTime] Time NULL ,[StateId] number NULL ,";
               strInsert = strInsert + "[CityId] number NULL ,[Dealercode] Text(150) NULL, [AdvtTypeName] Text(100) NULL, [AdvthttpUrl] Text(250) NULL )";
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
               strInsert = "CREATE TABLE tbMisc([DealerCode] text NULL, 	[IsStore] number NULL, 	[DfClientId] number NULL )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("tbMisc", "IsAdvt", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbMisc ADD IsAdvt number";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbFormat") == false)
           {
               strInsert = "CREATE TABLE tbFormat([FormatId] number NULL, [sTime] Time NULL,[eTime] Time NULL,[Is24Hour] number NULL)";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("tbAdvertisement", "IsTime", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               try
               {
                   strInsert = "ALTER TABLE tbAdvertisement ADD IsTime number";
                   if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                   StaticClass.LocalCon.Open();
                   OleDbCommand cmdTitle = new OleDbCommand();
                   cmdTitle.Connection = StaticClass.LocalCon;
                   cmdTitle.CommandText = strInsert;
                   cmdTitle.ExecuteNonQuery();
                   StaticClass.LocalCon.Close();

                   strInsert = "ALTER TABLE tbAdvertisement ADD IsMinute number";
                   if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                   StaticClass.LocalCon.Open();
                   cmdTitle = new OleDbCommand();
                   cmdTitle.Connection = StaticClass.LocalCon;
                   cmdTitle.CommandText = strInsert;
                   cmdTitle.ExecuteNonQuery();
                   StaticClass.LocalCon.Close();

                   strInsert = "ALTER TABLE tbAdvertisement ADD IsSong number";
                   if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                   StaticClass.LocalCon.Open();
                   cmdTitle = new OleDbCommand();
                   cmdTitle.Connection = StaticClass.LocalCon;
                   cmdTitle.CommandText = strInsert;
                   cmdTitle.ExecuteNonQuery();
                   StaticClass.LocalCon.Close();

                   strInsert = "ALTER TABLE tbAdvertisement ADD TotalMinutes number";
                   if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                   StaticClass.LocalCon.Open();
                   cmdTitle = new OleDbCommand();
                   cmdTitle.Connection = StaticClass.LocalCon;
                   cmdTitle.CommandText = strInsert;
                   cmdTitle.ExecuteNonQuery();
                   StaticClass.LocalCon.Close();

                   strInsert = "ALTER TABLE tbAdvertisement ADD TotalSongs number";
                   if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                   StaticClass.LocalCon.Open();
                   cmdTitle = new OleDbCommand();
                   cmdTitle.Connection = StaticClass.LocalCon;
                   cmdTitle.CommandText = strInsert;
                   cmdTitle.ExecuteNonQuery();
                   StaticClass.LocalCon.Close();
               }
               catch (Exception ex) { }
           }
           if (DoesFieldExist("tbMisc", "IsLock", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbMisc ADD IsLock number";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }

           if (DoesFieldExist("tbAdvertisement", "SrNo", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {

               strInsert = "ALTER TABLE tbAdvertisement ADD SrNo number";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }


           if (DoesFieldExist("tbAdvertisement", "IsVideo", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbAdvertisement ADD IsVideo number";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("tbAdvertisement", "IsVideoMute", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbAdvertisement ADD IsVideoMute number";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }

           if (DoesFieldExist("tbAdvertisement", "IsPicture", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbAdvertisement ADD IsPicture number";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }

           if (TableExists("tbAdvtPicture") == false)
           {
               strInsert = "CREATE TABLE tbAdvtPicture([AdvtId] number NULL, [ImgPath] Text(250))";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("tbAdvertisement", "IsBetween", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbAdvertisement ADD IsBetween number";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("tbAdvertisement", "bStime", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbAdvertisement ADD bStime DateTime";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("tbAdvertisement", "bEtime", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {
               strInsert = "ALTER TABLE tbAdvertisement ADD bEtime DateTime";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (DoesFieldExist("tbAdvertisement", "playingType", StaticClass.LocalCon.ConnectionString.ToString()) == false)
           {

               strInsert = "ALTER TABLE tbAdvertisement ADD playingType text";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }

           if (TableExists("tbTokenPlayedSongs") == false)
           {
               strInsert = "CREATE TABLE tbTokenPlayedSongs([Id] AUTOINCREMENT(1,1), [TokenId] number NULL, [playDate] DateTime,[playTime] DateTime, [TitleId] number NULL, 	[Artistid] number NULL,[splplaylistid] number NULL,[IsUpload] int NULL )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
           if (TableExists("tbTokenOverDueStatus") == false)
           {
               strInsert = "CREATE TABLE tbTokenOverDueStatus([Id] AUTOINCREMENT(1,1),[TokenId] number NULL, [StatusDate] DateTime,[StatusTime] DateTime,[IsUpload] int NULL )";
               if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
               StaticClass.LocalCon.Open();
               OleDbCommand cmdTitle = new OleDbCommand();
               cmdTitle.Connection = StaticClass.LocalCon;
               cmdTitle.CommandText = strInsert;
               cmdTitle.ExecuteNonQuery();
               StaticClass.LocalCon.Close();
           }
            if (TableExists("tbTokenAdvtStatus") == false)
            {
                strInsert = "CREATE TABLE tbTokenAdvtStatus([Id] AUTOINCREMENT(1,1),[TokenId] number NULL,[AdvtId] number NULL, [StatusDate] DateTime,[StatusTime] DateTime,[IsUpload] int NULL )";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (TableExists("tbTokenLoginStatus") == false)
            {
                strInsert = "CREATE TABLE tbTokenLoginStatus([Id] AUTOINCREMENT(1,1),[TokenId] number NULL, [StatusDate] DateTime,[StatusTime] DateTime,[IsUpload] int NULL )";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (TableExists("tbTokenPrayerStatus") == false)
            {
                strInsert = "CREATE TABLE tbTokenPrayerStatus([Id] AUTOINCREMENT(1,1),[TokenId] number NULL, [StatusDate] DateTime,[StatusTime] DateTime,[IsUpload] int NULL )";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (TableExists("tbTokenLogOutStatus") == false)
            {
                strInsert = "CREATE TABLE tbTokenLogOutStatus([Id] AUTOINCREMENT(1,1),[TokenId] number NULL, [StatusDate] DateTime,[StatusTime] DateTime,[IsUpload] int NULL )";
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
            if (DoesFieldExist("tbSpecialPlaylists_Titles", "FileSize", StaticClass.LocalCon.ConnectionString.ToString()) == false)
            {
                strInsert = "ALTER TABLE tbSpecialPlaylists_Titles ADD FileSize number";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (DoesFieldExist("tbAdvertisement", "FileSize", StaticClass.LocalCon.ConnectionString.ToString()) == false)
            {
                strInsert = "ALTER TABLE tbAdvertisement ADD FileSize number";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (TableExists("tbBlockTitles") == false)
            {
                strInsert = "";
                strInsert = "CREATE TABLE tbBlockTitles([titleid] number NULL)";
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
            }
            if (DoesFieldExist("Titles", "HttpUrl", StaticClass.LocalCon.ConnectionString.ToString()) == false)
            {
                strInsert = "ALTER TABLE Titles ADD HttpUrl Text";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();

                strInsert = "ALTER TABLE Titles ADD LocalUrl Text";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }

            if (DoesFieldExist("tbSpecialPlaylists_Titles", "HttpUrl", StaticClass.LocalCon.ConnectionString.ToString()) == false)
            {
                strInsert = "ALTER TABLE tbSpecialPlaylists_Titles ADD HttpUrl Text";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();

                strInsert = "ALTER TABLE tbSpecialPlaylists_Titles ADD LocalUrl Text";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (DoesFieldExist("TitlesInPlaylists", "Id", StaticClass.LocalCon.ConnectionString.ToString()) == false)
            {
                strInsert = "ALTER TABLE TitlesInPlaylists ADD Id int IDENTITY(1,1)";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (DoesFieldExist("tbMisc", "ScheduleType", StaticClass.LocalCon.ConnectionString.ToString()) == false)
            {
                strInsert = "ALTER TABLE tbMisc ADD ScheduleType text";
                if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            if (DoesFieldExist("tbSplPlaylistSchedule", "IsVideoMute", StaticClass.LocalCon.ConnectionString.ToString()) == false)
            {
                strInsert = "ALTER TABLE tbSplPlaylistSchedule ADD IsVideoMute number";
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



        public void CompactLocaldb()
        {
            try
            {
                Int64 fileSize = new FileInfo(Application.StartupPath + "\\db.mdb").Length / 1024 / 1024;
                if (fileSize >= 5)
                {
                    StaticClass.LocalCon.Close();
                    CompactAccessDB(StaticClass.LocalCon.ConnectionString, Application.StartupPath + "\\db.mdb");
                }
            }
            catch (Exception ex)
            {
            }
        }
        public void CompactAccessDB(string connectionString, string mdwfilename)
        {
            try
            {

                object[] oParams;
                //create an inctance of a Jet Replication Object
                object objJRO =
                  Activator.CreateInstance(Type.GetTypeFromProgID("JRO.JetEngine"));
                //filling Parameters array
                //cnahge "Jet OLEDB:Engine Type=5" to an appropriate value
                // or leave it as is if you db is JET4X format (access 2000,2002)
                //(yes, jetengine5 is for JET4X, no misprint here)
                oParams = new object[] {
        connectionString,
        "Provider=Microsoft.Jet.OLEDB.4.0;Data" +
        " Source=" + Application.StartupPath + "\\tempdb.mdb;Jet OLEDB:Engine Type=5"};

                //invoke a CompactDatabase method of a JRO object
                //pass Parameters array
                objJRO.GetType().InvokeMember("CompactDatabase",
                    System.Reflection.BindingFlags.InvokeMethod,
                    null,
                    objJRO,
                    oParams);

                //database is compacted now
                //to a new file C:\\tempdb.mdw
                //let's copy it over an old one and delete it

                System.IO.File.Delete(mdwfilename);
                System.IO.File.Move("" + Application.StartupPath + "\\tempdb.mdb", mdwfilename);

                //clean up (just in case)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(objJRO);
                objJRO = null;
            }
            catch (Exception ex)
            {

            }
        }


    }
}
