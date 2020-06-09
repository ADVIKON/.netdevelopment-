using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net.Http;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Data.SqlClient;
using static WeMixApi.gbl;
using System.Net;
using System.IO;

namespace WeMixApi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string conString = "";
        DataTable dtPlaylist = new DataTable();
        
        private void Form1_Load(object sender, EventArgs e)
        {
            conString = "Data Source=134.119.178.26;database=nusign;uid=sa;password=Jan@Server007;Connect Timeout=5000";
            dtPlaylist.Columns.Add("id", typeof(int));
            dtPlaylist.Columns.Add("name", typeof(string));
            dtPlaylist.Columns.Add("type", typeof(string));
            dtPlaylist.Columns.Add("published", typeof(DateTime));
            dtPlaylist.Columns.Add("Add", typeof(string));

            wcDownload.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wcDownload_DownloadProgressChanged);
            wcDownload.DownloadFileCompleted += new AsyncCompletedEventHandler(wcDownload_DownloadFileCompleted);

        }
        private void btnRunAPI_Click(object sender, EventArgs e)
        {
            if (txtSongDownloadLocation.Text == "")
            {
                MessageBox.Show("Set path of song download location ");
                return;
            }
            string URL = "https://advikon.cnc-london.net/data/playlists?command=getplaylists&startdate=2020-01-01";
            GetPlaylistInformation(URL);

        }
        #region PlaylistInformation
        private async void GetPlaylistInformation(string url)
        {
            SqlConnection con = new SqlConnection(conString);
            try
            {

           

            List<Playlist> clsPlaylist = new List<Playlist>();
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("username", "AdvikonAPI");
                client.DefaultRequestHeaders.Add("password", "Es5CuKqV5wqtxZsg");


                using (var response = await client.GetAsync(url))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var fileJsonString = await response.Content.ReadAsStringAsync();
                        var objs = JsonConvert.DeserializeObject<Playlist>(fileJsonString);

                        if (objs.data == null)
                        {
                            MessageBox.Show("No record found for playlists");
                            return;
                        }
                        foreach (var iData in objs.data)
                        {
                            SqlCommand cmd = new SqlCommand("Save_WeMix_Playlists", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@id", SqlDbType.Int));
                            cmd.Parameters["@id"].Value = iData.id;

                            cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar));
                            cmd.Parameters["@name"].Value = iData.name.Trim();

                            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar));
                            cmd.Parameters["@type"].Value = iData.type.Trim();

                            cmd.Parameters.Add(new SqlParameter("@published", SqlDbType.DateTime));
                            cmd.Parameters["@published"].Value = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(iData.published));
                            if (con.State == ConnectionState.Closed)
                            {
                                con.Open();
                            }
                            cmd.ExecuteNonQuery();

                            DataRow nr = dtPlaylist.NewRow();
                            nr["id"] = iData.id;
                            nr["name"] = iData.name.Trim();
                            nr["type"] = iData.type.Trim();
                            nr["published"] = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(iData.published));
                                nr["Add"] = "Add";
                                dtPlaylist.Rows.Add(nr);
                        }
                        con.Close();
                        if (dtPlaylist.Rows.Count > 0)
                        {
                                dgGrid.DataSource = null;
                                dgGrid.DataSource = dtPlaylist;
                                
                            //GetPlaylistContent();
                        }
                    }
                }
            }
            }
            catch (Exception ex)
            {
                con.Close();
                MessageBox.Show("GetPlaylistInformation " + ex.Message);
            }

        }
        #endregion

        #region PlaylistContent
        private async void GetPlaylistContent()
        {
            SqlConnection con = new SqlConnection(conString);
            try
            {
                for (int iRow = 0; iRow < dtPlaylist.Rows.Count; iRow++)
                {
                    string url = "https://advikon.cnc-london.net/data/playlists?command=getlistcontent&id=" + dtPlaylist.Rows[iRow]["id"];
                    List<Playlist> clsPlaylist = new List<Playlist>();
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(
                                new MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("username", "AdvikonAPI");
                        client.DefaultRequestHeaders.Add("password", "Es5CuKqV5wqtxZsg");


                        using (var response = await client.GetAsync(url))
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                var fileJsonString = await response.Content.ReadAsStringAsync();
                                var objs = JsonConvert.DeserializeObject<PlaylistContent>(fileJsonString);

                                if (objs.data == null)
                                {
                                    return;
                                }
                                int counter = 0;
                                foreach (var iData in objs.data.content)
                                {
                                    counter++;
                                    pBar.Maximum = objs.data.content.Count;
                                    pBar.Value = counter;

                                    SqlCommand cmd = new SqlCommand("Save_WeMix_Playlist_Content", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@AssetID", SqlDbType.NVarChar));
                                    cmd.Parameters["@AssetID"].Value = iData.AssetID;
                                    cmd.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar));
                                    cmd.Parameters["@Title"].Value = iData.Title.Trim();
                                    cmd.Parameters.Add(new SqlParameter("@Artist", SqlDbType.NVarChar));
                                    cmd.Parameters["@Artist"].Value = iData.Artist.Trim();
                                    cmd.Parameters.Add(new SqlParameter("@Duration", SqlDbType.NVarChar));
                                    cmd.Parameters["@Duration"].Value = iData.Duration;
                                    cmd.Parameters.Add(new SqlParameter("@BPM", SqlDbType.NVarChar));
                                    cmd.Parameters["@BPM"].Value = iData.BPM.Trim();
                                    cmd.Parameters.Add(new SqlParameter("@Genre", SqlDbType.NVarChar));
                                    cmd.Parameters["@Genre"].Value = iData.Genre.Trim();
                                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.Int));
                                    cmd.Parameters["@Year"].Value = iData.Year;
                                    cmd.Parameters.Add(new SqlParameter("@Country", SqlDbType.NVarChar));
                                    cmd.Parameters["@Country"].Value = iData.Country.Trim();
                                    cmd.Parameters.Add(new SqlParameter("@Explicit", SqlDbType.Int));
                                    cmd.Parameters["@Explicit"].Value = iData.Explicit;
                                    cmd.Parameters.Add(new SqlParameter("@MediaType", SqlDbType.NVarChar));
                                    cmd.Parameters["@MediaType"].Value = iData.MediaType;
                                    cmd.Parameters.Add(new SqlParameter("@FileType", SqlDbType.NVarChar));
                                    cmd.Parameters["@FileType"].Value = iData.MediaType.Substring(iData.MediaType.Length - 3); ;
                                    cmd.Parameters.Add(new SqlParameter("@download", SqlDbType.NVarChar));
                                    cmd.Parameters["@download"].Value = iData.downloadLink;
                                    cmd.Parameters.Add(new SqlParameter("@playlistid", SqlDbType.Int));
                                    cmd.Parameters["@playlistid"].Value = objs.data.playlistid;
                                    long filesize = 1;
                                    cmd.Parameters.Add(new SqlParameter("@fileSize", SqlDbType.Int));
                                    cmd.Parameters["@fileSize"].Value = filesize;

                                    cmd.Parameters.Add(new SqlParameter("@language", SqlDbType.NVarChar));
                                    cmd.Parameters["@language"].Value = "English";
                                    if (con.State == ConnectionState.Closed)
                                    {
                                        con.Open();
                                    }
                                    cmd.ExecuteNonQuery();
                                }

                            }
                        }
                    }
                }
                con.Close();
                DownloadSong();
            }
            catch (Exception ex)
            {
                var k = ex.Message.ToString();
                con.Close();
                MessageBox.Show("GetPlaylistContent " + ex.Message);
            }

        }

        #endregion
        WebClient wcDownload = new WebClient();
        
        
        string DownloadAssetID = "";
        private void DownloadSong()
        {
            try
            {
                string LocalDestinationPath = "";
                string st = "select * from   tbWeMix_Playlist_Content where isDownloaded=0 and  filesize!=0 ";
                DataTable dt = new DataTable();
                dt = fnFillDataTable(st);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (wcDownload.IsBusy == true)
                    {
                        break;
                    }
                    GetFileSize = 0;
                    DownloadAssetID = dt.Rows[i]["AssetID"].ToString();
                    string url = dt.Rows[i]["download"].ToString();
                    LocalDestinationPath = txtSongDownloadLocation.Text + "//" + dt.Rows[i]["AssetID"].ToString() + "." + dt.Rows[i]["FileType"].ToString() + "";
                    wcDownload.DownloadFileAsync(new Uri(url), LocalDestinationPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DownloadSong " + ex.Message);
            }
        }
        long GetFileSize = 0;
        void wcDownload_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            pBar.Maximum = (int)e.TotalBytesToReceive / 100;
            pBar.Value = (int)e.BytesReceived / 100;
            try
            {
                SqlConnection con = new SqlConnection(conString);
            
            if (GetFileSize == 0)
            {
                    GetFileSize = (int)e.TotalBytesToReceive;
                string sQr = "update tbWeMix_Playlist_Content set filesize=" + GetFileSize + " where AssetID='" + DownloadAssetID + "'";
                if (con.State == ConnectionState.Closed) { con.Open(); }
                SqlCommand cmdFile = new SqlCommand();
                cmdFile.Connection = con;
                cmdFile.CommandText = sQr;
                cmdFile.ExecuteNonQuery();
                cmdFile.Dispose();
            }
            }
            catch (Exception ex)
            {

                
            }

        }
        int TitleCounter = 0;
        private void wcDownload_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string DownloadedFile = "";
                long DownloadedFileSize = 0;
                Int32 ActualFileSize = 0;
                Int32 GenreId = 0;
                SqlConnection con = new SqlConnection(conString);
                string st = "select tbWeMix_Playlist_Content.*, tbWeMix_Playlists.name as playlistname from tbWeMix_Playlist_Content ";
                st = st + " inner join  tbWeMix_Playlists on tbWeMix_Playlists.id= tbWeMix_Playlist_Content.playlistid ";
                st =st+ " where tbWeMix_Playlist_Content.AssetID='" + DownloadAssetID+ "'";
                DataTable dt = new DataTable();
                dt = fnFillDataTable(st);

                wcDownload.Dispose();
                pBar.Value = 0;
                if (dt.Rows.Count > 0)
                {
                    DownloadedFile = txtSongDownloadLocation.Text + "//" + dt.Rows[0]["AssetID"].ToString() + "." + dt.Rows[0]["FileType"].ToString() + "";
                    DownloadedFileSize = new FileInfo(DownloadedFile).Length;
                    ActualFileSize = Convert.ToInt32(dt.Rows[0]["filesize"]);
                    if (ActualFileSize != DownloadedFileSize)
                    {
                        TitleCounter++;
                    }
                    if (TitleCounter == 2)
                    {
                        if (txtErr.Text == "")
                        {
                            txtErr.Text = dt.Rows[0]["AssetID"].ToString();
                        }
                        else
                        {
                            txtErr.Text = txtErr.Text +","+dt.Rows[0]["AssetID"].ToString();
                        }
                        string sQr = "update tbWeMix_Playlist_Content set isDownloaded=0,filesize=0 where AssetID='" + dt.Rows[0]["AssetID"].ToString() + "'";
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        SqlCommand cmdFile = new SqlCommand();
                        cmdFile.Connection = con;
                        cmdFile.CommandText = sQr;
                        cmdFile.ExecuteNonQuery();
                    }

                    if (ActualFileSize == DownloadedFileSize)
                    {
                        TitleCounter = 0;
                        string sQr = "update tbWeMix_Playlist_Content set isDownloaded=1 where AssetID='" + dt.Rows[0]["AssetID"].ToString()+"'";
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        SqlCommand cmdUpdate = new SqlCommand();
                        cmdUpdate.Connection = con;
                        cmdUpdate.CommandText = sQr;
                        cmdUpdate.ExecuteNonQuery();

                        #region Save Data in our database

                        SqlCommand cmdGenre = new SqlCommand("spInsertGenre", con);
                        cmdGenre.CommandType = CommandType.StoredProcedure;
                        cmdGenre.Parameters.Add(new SqlParameter("@genre", SqlDbType.VarChar));
                        cmdGenre.Parameters["@genre"].Value = dt.Rows[0]["Genre"].ToString().Trim();
                        if (con.State == ConnectionState.Closed) { con.Open(); }
                        GenreId = Convert.ToInt32(cmdGenre.ExecuteScalar());


                        SqlCommand cmd = new SqlCommand("InsertArtistsAlbumsTitles_AlenkaMedia", con);
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@TiTleTiTle", SqlDbType.VarChar));
                        cmd.Parameters["@TiTleTiTle"].Value = dt.Rows[0]["Title"].ToString().Trim();

                        cmd.Parameters.Add(new SqlParameter("@TitleArtistName", SqlDbType.VarChar));
                        cmd.Parameters["@TitleArtistName"].Value = dt.Rows[0]["Artist"].ToString().Trim();

                        cmd.Parameters.Add(new SqlParameter("@AlbumName", SqlDbType.VarChar));
                        cmd.Parameters["@AlbumName"].Value = dt.Rows[0]["playlistName"].ToString().Trim();

                        cmd.Parameters.Add(new SqlParameter("@titlecategoryid", SqlDbType.BigInt));
                        cmd.Parameters["@titlecategoryid"].Value = 4;

                        cmd.Parameters.Add(new SqlParameter("@titleSubcategoryid", SqlDbType.VarChar));
                        cmd.Parameters["@titleSubcategoryid"].Value = "22";

                        cmd.Parameters.Add(new SqlParameter("@Time", SqlDbType.VarChar));
                        cmd.Parameters["@Time"].Value = dt.Rows[0]["Duration"].ToString().Trim();

                        cmd.Parameters.Add(new SqlParameter("@AlbumLabel", SqlDbType.VarChar));
                        cmd.Parameters["@AlbumLabel"].Value = "0";

                        cmd.Parameters.Add(new SqlParameter("@CatalogCode", SqlDbType.VarChar));
                        cmd.Parameters["@CatalogCode"].Value = "0";

                        cmd.Parameters.Add(new SqlParameter("@titleYear", SqlDbType.Int));
                        cmd.Parameters["@titleYear"].Value = dt.Rows[0]["Year"].ToString().Trim();

                        cmd.Parameters.Add(new SqlParameter("@GenreId", SqlDbType.Int));
                        cmd.Parameters["@GenreId"].Value = GenreId;


                        string Tempo = "";
                        if (Convert.ToInt32(dt.Rows[0]["BPM"]) <= 76)
                        {
                            Tempo = "Slow";
                        }
                        if ((Convert.ToInt32(dt.Rows[0]["BPM"]) > 76) && (Convert.ToInt32(dt.Rows[0]["BPM"]) <= 120))
                        {
                            Tempo = "Mid";
                        }
                        if (Convert.ToInt32(dt.Rows[0]["BPM"]) > 120)
                        {
                            Tempo = "Up";
                        }
                        cmd.Parameters.Add(new SqlParameter("@tempo", SqlDbType.VarChar));
                        cmd.Parameters["@tempo"].Value = Tempo;

                        string mediaType = "";
                        if (dt.Rows[0]["FileType"].ToString().Trim() == "mp3")
                        {
                            mediaType = "Audio";
                        }
                        if (dt.Rows[0]["FileType"].ToString().Trim() == "mp4")
                        {
                            mediaType = "Video";
                        }
                        cmd.Parameters.Add(new SqlParameter("@mType", SqlDbType.VarChar));
                        cmd.Parameters["@mType"].Value = mediaType;

                        string acat = "";
                        acat = dt.Rows[0]["playlistName"].ToString().Replace("Advikon -", "");
                        acat = acat.Replace("Advikon - ", "");
                        cmd.Parameters.Add(new SqlParameter("@acategory", SqlDbType.VarChar));
                        cmd.Parameters["@acategory"].Value = acat.Trim();

                        cmd.Parameters.Add(new SqlParameter("@language", SqlDbType.VarChar));
                        cmd.Parameters["@language"].Value = dt.Rows[0]["language"].ToString().Trim();

                        cmd.Parameters.Add(new SqlParameter("@isRF", SqlDbType.VarChar));
                        cmd.Parameters["@isRF"].Value = "0";

                        cmd.Parameters.Add(new SqlParameter("@isrc", SqlDbType.VarChar));
                        cmd.Parameters["@isrc"].Value = "";

                        cmd.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.VarChar));
                        cmd.Parameters["@FileSize"].Value = dt.Rows[0]["fileSize"].ToString().Trim();


                        cmd.Parameters.Add(new SqlParameter("@dfclientid", SqlDbType.Int));
                        cmd.Parameters["@dfclientid"].Value = 0;

                        cmd.Parameters.Add(new SqlParameter("@folderId", SqlDbType.Int));
                        cmd.Parameters["@folderId"].Value = 0;



                        cmd.Parameters.Add(new SqlParameter("@AssetID_WeMix", SqlDbType.VarChar));
                        cmd.Parameters["@AssetID_WeMix"].Value = dt.Rows[0]["AssetID"].ToString().Trim();


                        cmd.Parameters.Add(new SqlParameter("@Country_WeMix", SqlDbType.VarChar));
                        cmd.Parameters["@Country_WeMix"].Value = dt.Rows[0]["Country"].ToString().Trim();

                        cmd.Parameters.Add(new SqlParameter("@Explicit", SqlDbType.Int));
                        cmd.Parameters["@Explicit"].Value = dt.Rows[0]["Explicit"];

                        cmd.Parameters.Add(new SqlParameter("@label", SqlDbType.NVarChar));
                        cmd.Parameters["@label"].Value = "WeMix";

                        if (con.State == ConnectionState.Closed) { con.Open(); }

                        Int32 Title_Id = Convert.ToInt32(cmd.ExecuteScalar());

                        string NewName = txtSongDownloadLocation.Text + "//" + Title_Id.ToString() + "." + dt.Rows[0]["FileType"].ToString() + "";
                        if (!File.Exists(NewName))
                        {
                            File.Copy(DownloadedFile, NewName);
                        }
                        File.Delete(DownloadedFile);

                        #endregion

                    }
                }

                DownloadSong();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        public DataTable fnFillDataTable(string sSql)
        {
            SqlConnection con = new SqlConnection(conString);
            SqlDataAdapter Adp = new SqlDataAdapter();
            DataTable mldData;
            try
            {
                Adp = new SqlDataAdapter(sSql, con);
                mldData = new DataTable();
                Adp.Fill(mldData);
            }
            catch (Exception ex)
            {
                mldData = new DataTable();
                MessageBox.Show(ex.Message);
            }
            return mldData;
        }
        WebClient client = new WebClient();

        private void dgGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgGrid.ColumnCount-1)
                {
                    dtPlaylist = new DataTable();
                    dtPlaylist.Columns.Add("id", typeof(int));
                    dtPlaylist.Columns.Add("name", typeof(string));
                    dtPlaylist.Columns.Add("type", typeof(string));
                    dtPlaylist.Columns.Add("published", typeof(DateTime));
                    DataRow nr = dtPlaylist.NewRow();
                    nr["id"] = dgGrid.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    nr["name"] = dgGrid.Rows[e.RowIndex].Cells["name"].Value.ToString().Trim();
                    nr["type"] = dgGrid.Rows[e.RowIndex].Cells["type"].Value.ToString().Trim();
                    nr["published"] = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(dgGrid.Rows[e.RowIndex].Cells["published"].Value));
                    dtPlaylist.Rows.Add(nr);
                    txtErr.Text = txtErr.Text + "," + dgGrid.Rows[e.RowIndex].Cells["id"].Value.ToString();
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnGetContent_Click(object sender, EventArgs e)
        {
            GetPlaylistContent();
        }
    }
}
