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
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using WMPLib;
using System.Data.OleDb;
using System.Runtime.InteropServices;
namespace MusicPlayerCopyright
{


    public partial class mainwindow : Form
    {
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int WM_APPCOMMAND = 0x319;
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
            IntPtr wParam, IntPtr lParam);

        int eX = 0;
        int eY = 0;
        Boolean IsVisibleSong = false;
        Int32 LastRowId = 0;
        string gblsongid = "";
        string IsLast100Working = "";
        string StopDuplicate = "Yes";
        string ShowPlaylistCounter = "";
        string LocalSecondTime = "";
        string Drop_TitleName = "";
        string DropTitleSong = "";
        string UpcomingSongPlayerOne = "";
        string UpcomingSongPlayerTwo = "";
        clsSongCrypt amcrypt = new clsSongCrypt();
        gblClass ObjMainClass = new gblClass();
        string FitnessRecordShowType = "";
        string downloadSongName = "";
        Point p1 = new Point();
        string SearchText = "";
        Point p2 = new Point();
        bool drawLine;
        Pen p;
        PaintEventArgs EventSpl;
        int TotShuffle = 0;
        Int16 ShuffleCount = 0;
        string pAction = "New";
        Int32 ModifyPlaylistId;
        string IsbtnClick = "N";
        string fileName = "";
        string temp_songid = "";
        Boolean Add_Playlist = false;
        Boolean Show_Record = false;
        Boolean Drop_Song = false;
        string SubmitValidate;
        Int32 CurrentRow;
        Boolean Is_Drop = false;
        Int32 CurrentPlaylistRow = 0;
        Boolean Song_Mute = false;
        Boolean Stop_Insert = false;
        Boolean Grid_Clear = false;
        Boolean IsDrop_Song = false;
        Boolean FirstTimeSong = false;
        Boolean FirstPlaySong = true;
        string exit = "No";
        string BestOffRecordShowType = "";
        DataGridViewButtonColumn SongDownload = new DataGridViewButtonColumn();

        

        DataGridViewImageColumn Column_Img_Stream = new DataGridViewImageColumn();
        public WindowsMediaPlayer player;

        //Advt Variables
        string TempAdvtFileName;
        double AdvtTimeResult = 0;
        double TimePlayerOne = 0;
        double TimePlayerTwo = 0;
        double TimeStreamPlayer = 0;
        double PrvTimeStreamPlayer = 0;
        Boolean IsFirtTimeStreamComplete = false;
        Int32 AdvtCurrentRow = 0;
        Int32 AdvtCurrentSongId = 0;
        Boolean PanelVisiable;
        int rowIndexFromMouseDown;
        DataGridViewRow rw;
        Boolean StopDup = false;
        double prvPlayerOneTime = 0;
        double prvPlayerTwoTime = 0;
        Boolean IsAdvtTimeGet = false;
        double GrossTotaltime = 0;
        int rCount = 0;
        string DropSongLength = "";
        Boolean IsSongDropAdvt = false;

        DataGridView dgSaveDataGrid;
        Int32 SaveDataCurrentPlaylistId = 0;
        Boolean isThreadFirstTime = false;
        string AdvtPlayTime = "";
        public mainwindow()
        {
            InitializeComponent();
        }
        private void InitilizeGrid(DataGridView dgGrid)
        {
            try
            {

                if (dgGrid.Rows.Count > 0)
                {
                    dgGrid.Rows.Clear();
                }
                if (dgGrid.Columns.Count > 0)
                {
                    dgGrid.Columns.Clear();
                }

                dgGrid.Columns.Add("songid", "song Id");
                dgGrid.Columns["songid"].Width = 0;
                dgGrid.Columns["songid"].Visible = false;
                dgGrid.Columns["songid"].ReadOnly = true;

                dgGrid.Columns.Add("songname", "Title");
                 dgGrid.Columns["songname"].Width = 280;
                dgGrid.Columns["songname"].Visible = true;
                dgGrid.Columns["songname"].ReadOnly = true;
                dgGrid.Columns["songname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                dgGrid.Columns.Add("Length", "Length");
                dgGrid.Columns["Length"].Width = 80;
                dgGrid.Columns["Length"].Visible = true;
                dgGrid.Columns["Length"].ReadOnly = true;
                dgGrid.Columns["Length"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                dgGrid.Columns.Add("Artist", "Artist");
                  dgGrid.Columns["Artist"].Width = 150;
                dgGrid.Columns["Artist"].Visible = true;
                dgGrid.Columns["Artist"].ReadOnly = true;
                dgGrid.Columns["Artist"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;


                dgGrid.Columns.Add("Album", "Album");
                //  dgGrid.Columns["Album"].Width = 0;
                dgGrid.Columns["Album"].Visible = true;
                dgGrid.Columns["Album"].ReadOnly = true;
                dgGrid.Columns["Album"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                dgGrid.Columns.Add("Year", "Year");
                dgGrid.Columns["Year"].Width = 0;
                dgGrid.Columns["Year"].Visible = false;
                dgGrid.Columns["Year"].ReadOnly = true;

                

            }
            catch (Exception ex)
            {
            }

        }
        private void InitilizeHideGrid()
        {
            if (dgHideSongs.Rows.Count > 0)
            {
                dgHideSongs.Rows.Clear();
            }
            if (dgHideSongs.Columns.Count > 0)
            {
                dgHideSongs.Columns.Clear();
            }

            dgHideSongs.Columns.Add("songid", "song Id");
            dgHideSongs.Columns["songid"].Width = 100;
            dgHideSongs.Columns["songid"].Visible = true;
            dgHideSongs.Columns["songid"].ReadOnly = true;

            dgHideSongs.Columns.Add("Status", "Status");
            dgHideSongs.Columns["Status"].Width = 100;
            dgHideSongs.Columns["Status"].Visible = true;
            dgHideSongs.Columns["Status"].ReadOnly = true;

        }

        private void InitilizeLocalGrid()
        {
            if (dgLocalPlaylist.Rows.Count > 0)
            {
                dgLocalPlaylist.Rows.Clear();
            }
            if (dgLocalPlaylist.Columns.Count > 0)
            {
                dgLocalPlaylist.Columns.Clear();
            }

            dgLocalPlaylist.Columns.Add("playlistId", "playlist Id");
            dgLocalPlaylist.Columns["playlistId"].Width = 0;
            dgLocalPlaylist.Columns["playlistId"].Visible = false;
            dgLocalPlaylist.Columns["playlistId"].ReadOnly = true;

            dgLocalPlaylist.Columns.Add("playlistname", "Playlist Name");
            // dgLocalPlaylist.Columns["playlistname"].Width = 230;
            dgLocalPlaylist.Columns["playlistname"].Visible = true;
            dgLocalPlaylist.Columns["playlistname"].ReadOnly = true;
            dgLocalPlaylist.Columns["playlistname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgLocalPlaylist.Columns.Add("Default", "Default");
            dgLocalPlaylist.Columns["Default"].Width = 0;
            dgLocalPlaylist.Columns["Default"].Visible = false;
            dgLocalPlaylist.Columns["Default"].ReadOnly = true;


            dgLocalPlaylist.Columns.Add("PlaylistColor", "");
            dgLocalPlaylist.Columns["PlaylistColor"].Width = 30;
            dgLocalPlaylist.Columns["PlaylistColor"].Visible = true;
            dgLocalPlaylist.Columns["PlaylistColor"].ReadOnly = true;
            dgLocalPlaylist.Columns["PlaylistColor"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            //dgLocalPlaylist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            DataGridViewLinkColumn EditPlaylist = new DataGridViewLinkColumn();
            EditPlaylist.HeaderText = "Edit";
            EditPlaylist.Text = "Edit";
            EditPlaylist.DataPropertyName = "Edit";
            dgLocalPlaylist.Columns.Add(EditPlaylist);
            EditPlaylist.UseColumnTextForLinkValue = true;
            EditPlaylist.Width = 0;
            EditPlaylist.Visible = false;


        }

        private void InitilizeCommanGrid()
        {
            try
            {

                if (dgCommanGrid.Rows.Count > 0)
                {
                    dgCommanGrid.Rows.Clear();
                }
                if (dgCommanGrid.Columns.Count > 0)
                {
                    dgCommanGrid.Columns.Clear();
                }

                dgCommanGrid.Columns.Add("songid", "song Id");
                dgCommanGrid.Columns["songid"].Width = 0;
                dgCommanGrid.Columns["songid"].Visible = false;
                dgCommanGrid.Columns["songid"].ReadOnly = true;

                dgCommanGrid.Columns.Add("songname", "Title");
                  dgCommanGrid.Columns["songname"].Width = 280;
                dgCommanGrid.Columns["songname"].Visible = true;
                dgCommanGrid.Columns["songname"].ReadOnly = true;
                dgCommanGrid.Columns["songname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                dgCommanGrid.Columns.Add("Length", "Length");
                dgCommanGrid.Columns["Length"].Width = 80;
                dgCommanGrid.Columns["Length"].Visible = true;
                dgCommanGrid.Columns["Length"].ReadOnly = true;
                dgCommanGrid.Columns["Length"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                dgCommanGrid.Columns.Add("aritstname", "Aritst");
                   dgCommanGrid.Columns["aritstname"].Width = 150;
                dgCommanGrid.Columns["aritstname"].Visible = true;
                dgCommanGrid.Columns["aritstname"].ReadOnly = true;
                dgCommanGrid.Columns["aritstname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

                dgCommanGrid.Columns.Add("albumname", "Album");
                //  dgCommanGrid.Columns["albumname"].Width = 130;
                dgCommanGrid.Columns["albumname"].Visible = true;
                dgCommanGrid.Columns["albumname"].ReadOnly = true;
                dgCommanGrid.Columns["albumname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                SongDownload.HeaderText = "";
                SongDownload.Text = "";
                SongDownload.DataPropertyName = "SongDownload";
                dgCommanGrid.Columns.Add(SongDownload);
                SongDownload.UseColumnTextForButtonValue = true;
                SongDownload.Width = 30;
                dgCommanGrid.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
        private void InitilizeCommanOptionGrid(DataGridView Grid_Name)
        {
            if (Grid_Name.Rows.Count > 0)
            {
                Grid_Name.Rows.Clear();
            }
            if (Grid_Name.Columns.Count > 0)
            {
                Grid_Name.Columns.Clear();
            }

            Grid_Name.Columns.Add("playlistId", "playlist Id");
            Grid_Name.Columns["playlistId"].Width = 0;
            Grid_Name.Columns["playlistId"].Visible = false;
            Grid_Name.Columns["playlistId"].ReadOnly = true;

            Grid_Name.Columns.Add("CommanName", "Comman Name");
            //            Grid_Name.Columns["CommanName"].Width = 265;
            Grid_Name.Columns["CommanName"].Visible = true;
            Grid_Name.Columns["CommanName"].ReadOnly = true;
            Grid_Name.Columns["CommanName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }
        private void InitilizeNewestGrid()
        {
            if (dgNewest.Rows.Count > 0)
            {
                dgNewest.Rows.Clear();
            }
            if (dgNewest.Columns.Count > 0)
            {
                dgNewest.Columns.Clear();
            }

            dgNewest.Columns.Add("playlistId", "playlist Id");
            dgNewest.Columns["playlistId"].Width = 0;
            dgNewest.Columns["playlistId"].Visible = false;
            dgNewest.Columns["playlistId"].ReadOnly = true;

            dgNewest.Columns.Add("Title", "Title");
            //dgNewest.Columns["Title"].Width = 190;
            dgNewest.Columns["Title"].Visible = true;
            dgNewest.Columns["Title"].ReadOnly = true;
            dgNewest.Columns["Title"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgNewest.Columns.Add("Artist", "Artist");
            //dgNewest.Columns["Artist"].Width = 80;
            dgNewest.Columns["Artist"].Visible = true;
            dgNewest.Columns["Artist"].ReadOnly = true;
            dgNewest.Columns["Artist"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        }



        private void PopulateInputFileTypeDetail(DataGridView dgGrid, Int32 currentPlayRow)
        {
            try
            {

                string mlsSql = "";
                string GetLocalPath = "";
                string TitleYear = "";
                string TitleTime = "";
                var Special_Name = "";
                string Special_Change = "";
                Int32 iCtr = 0;
                Int32 srNo = 0;
                DataTable dtDetail = new DataTable();
                //mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + Convert.ToInt32(currentPlayRow);

                mlsSql = "SELECT  Titles.TitleID, ltrim(Titles.Title) as Title, Titles.Time,ltrim(Albums.Name) AS AlbumName ,";
                mlsSql = mlsSql + " Titles.TitleYear ,  ltrim(Artists.Name) as ArtistName  FROM ((( TitlesInPlaylists  ";
                mlsSql = mlsSql + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID )  ";
                mlsSql = mlsSql + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ) ";
                mlsSql = mlsSql + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ) ";
                mlsSql = mlsSql + " where TitlesInPlaylists.PlaylistID=" + Convert.ToInt32(currentPlayRow) + " ORDER BY   Rnd(-(100000*Titles.TitleID)*Time())";


                dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                InitilizeGrid(dgGrid);
                if ((dtDetail.Rows.Count > 0))
                {
                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {
                        GetLocalPath = dtDetail.Rows[iCtr]["TitleID"] + ".mp3";
                        srNo = iCtr;
                        dgGrid.Rows.Add();
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["songid"].Value = dtDetail.Rows[iCtr]["TitleID"];

                        Special_Name = "";
                        Special_Change = "";
                        Special_Name = dtDetail.Rows[iCtr]["Title"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["songname"].Value = Special_Change;

                        string str = dtDetail.Rows[iCtr]["Time"].ToString();
                        string[] arr = str.Split(':');
                        TitleTime = arr[1] + ":" + arr[2];

                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Length"].Value = TitleTime;

                        Special_Name = "";
                        Special_Change = "";

                        Special_Name = dtDetail.Rows[iCtr]["AlbumName"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Album"].Value = Special_Change;

                        TitleYear = dtDetail.Rows[iCtr]["TitleYear"].ToString();
                        if (TitleYear == "0")
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Year"].Value = "- - -";
                        }
                        else
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Year"].Value = dtDetail.Rows[iCtr]["TitleYear"];
                        }

                        Special_Name = "";
                        Special_Change = "";

                        Special_Name = dtDetail.Rows[iCtr]["ArtistName"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Artist"].Value = Special_Change;

                        if (this.Width.ToString() == "1020")
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 11);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Style.Font = new Font("Segoe UI", 11);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                             
                        }
                    }
                }
                if (this.Width.ToString() == "1020")
                {
                    foreach (DataGridViewRow row in dgGrid.Rows)
                    {
                        row.Height = 33;
                    }
                }
                else
                {
                    dgGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11);
                    foreach (DataGridViewRow row in dgGrid.Rows)
                    {
                        row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                        row.Cells[2].Style.Font = new Font("Segoe UI", 12);
                        row.Cells[3].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                        row.Cells[4].Style.Font = new Font("Segoe UI", 12);
                        row.Cells[5].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                        
                        row.Height = 35;

                    }
                }
                RowHide();
            }
            catch (Exception ex)
            {

            }

        }


        private void FillLocalPlaylist()
        {

            Boolean IsFindDefaultPlaylist = false;
            string str = "";
            string strGetCount = "";
            int iCtr;
            DataTable dtDetail;
            DataTable dtGetCount;
            str = "select playlistId,name, PlaylistDefault from Playlists where UserID=" + StaticClass.UserId + " and tokenid= " + StaticClass.TokenId;
            dtDetail = ObjMainClass.fnFillDataTable_Local(str);

            InitilizeLocalGrid();
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgLocalPlaylist.Rows.Add();
                    dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["playlistId"];

                    strGetCount = "select Count(*) as Total from  TitlesInPlaylists where playlistId =" + dtDetail.Rows[iCtr]["playlistId"] + " ";
                    dtGetCount = ObjMainClass.fnFillDataTable_Local(strGetCount);

                    dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["name"] + "  (" + dtGetCount.Rows[0]["Total"] + ")";
                    dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["PlaylistDefault"];

                    if (this.Width.ToString() == "1020")
                    {
                        dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    }

                    if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                    StaticClass.constr.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = StaticClass.constr;
                    cmd.CommandText = "update Playlists set tokenid=" + StaticClass.TokenId + " where playlistid=" + dtDetail.Rows[iCtr]["playlistId"];
                    cmd.ExecuteNonQuery();
                    StaticClass.constr.Close();

                }
                if (this.Width.ToString() == "1020")
                {
                    foreach (DataGridViewRow row in dgLocalPlaylist.Rows)
                    {
                        row.Height = 33;
                        if (row.Cells[2].Value.ToString() == "Default")
                        {
                            IsFindDefaultPlaylist = true;
                            row.Selected = true;
                            StaticClass.DefaultPlaylistId = Convert.ToInt32(row.Cells[0].Value);
                            dgLocalPlaylist.CurrentCell = row.Cells[1];
                            StaticClass.DefaultPlaylistCurrentRow = dgLocalPlaylist.CurrentCell.RowIndex;

                            row.Cells[1].Style.ForeColor = Color.FromArgb(20, 162, 175);
                            row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            row.Cells[1].Style.SelectionForeColor = Color.White;

                            row.Cells[3].Style.SelectionBackColor = Color.LightBlue;
                            row.Cells[3].Style.BackColor = Color.LightBlue;
                        }
                        else
                        {
                            row.Cells[1].Style.ForeColor = Color.FromArgb(0, 0, 0);
                            row.Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                            row.Cells[3].Style.BackColor = Color.White;
                            row.Cells[3].Style.SelectionBackColor = Color.White;

                        }
                    }
                }
                else
                {
                    foreach (DataGridViewRow row in dgLocalPlaylist.Rows)
                    {
                        row.Height = 35;
                        if (row.Cells[2].Value.ToString() == "Default")
                        {
                            IsFindDefaultPlaylist = true;
                            row.Selected = true;
                            StaticClass.DefaultPlaylistId = Convert.ToInt32(row.Cells[0].Value);
                            dgLocalPlaylist.CurrentCell = row.Cells[1];
                            StaticClass.DefaultPlaylistCurrentRow = dgLocalPlaylist.CurrentCell.RowIndex;

                            row.Cells[1].Style.ForeColor = Color.FromArgb(20, 162, 175);
                            row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                            row.Cells[1].Style.SelectionForeColor = Color.White;

                            row.Cells[3].Style.SelectionBackColor = Color.LightBlue;
                            row.Cells[3].Style.BackColor = Color.LightBlue;
                        }
                        else
                        {
                            row.Cells[1].Style.ForeColor = Color.FromArgb(0, 0, 0);
                            row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            row.Cells[3].Style.BackColor = Color.White;
                            row.Cells[3].Style.SelectionBackColor = Color.White;

                        }
                    }
                }

                if (IsFindDefaultPlaylist == false)
                {
                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdUpdateAll = new OleDbCommand();
                    cmdUpdateAll.Connection = StaticClass.LocalCon;
                    cmdUpdateAll.CommandText = "Update Playlists set PlaylistDefault=''";
                    cmdUpdateAll.ExecuteNonQuery();
                    //    StaticClass.LocalCon.Close();

                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdUpdate = new OleDbCommand();
                    cmdUpdate.Connection = StaticClass.LocalCon;
                    cmdUpdate.CommandText = "Update Playlists set PlaylistDefault='Default' where playlistid = " + dgLocalPlaylist.Rows[0].Cells[0].Value.ToString();
                    cmdUpdate.ExecuteNonQuery();
                    //  StaticClass.LocalCon.Close();
                    FillLocalPlaylist();
                }
            }

        }
        private void mainwindow_Load(object sender, EventArgs e)
        {
            InitilizePlaylistScheduleGrid();
            lblInternetStatus.Text = "10";
            string str = "";
            this.Text = StaticClass.MainwindowMessage + "                                                                                        jan@myclaud.com";
            lblGreen.Visible = true;
            lblRed.Visible = false;
            lblGreen.Location = new Point(250, 3);
            InitilizeStreamShe();
            if (StaticClass.IsBlockAdvt == true)
            {
                picAdvertisement.Visible = false;
                timAdvt.Enabled = false;
                timGetAdvtTime.Enabled = false;
                lblRed.Visible = true;
                lblGreen.Visible = false;
            }
            else if (StaticClass.IsAdvt == false)
            {
                //lblRed.Visible = true;
                //lblGreen.Visible = false;
            }
            else
            {
                //string strAdvt = "";
                //strAdvt = "spGetAdvtExpiryStatus " + StaticClass.TokenId + ", 1 ";
                //DataSet dsExpire = new DataSet();
                //dsExpire = ObjMainClass.fnFillDataSet(strAdvt);
                //if (dsExpire.Tables[0].Rows[0]["ExpiryAdvtStatus"].ToString() == "NoLic")
                //{
                //    lblRed.Visible = true;
                //    lblGreen.Visible = false;
                //}
                //if (dsExpire.Tables[0].Rows[0]["ExpiryAdvtStatus"].ToString() == "Yes")
                //{
                //    lblRed.Visible = true;
                //    lblGreen.Visible = false;
                //}
            }
            //////////////////////////////////////////
            //////////////////////////////////////////
            delete_temp_table();
            DeleteHideSongs();
            //////////////////////////////////////////
            //////////////////////////////////////////
            musicPlayer1.enableContextMenu = false;
            musicPlayer2.enableContextMenu = false;
            StreamMusicPlayer.enableContextMenu = false;
            dgPlaylist.AllowDrop = true;
            dgOtherPlaylist.AllowDrop = true;

            dgPlaylist.Dock = DockStyle.Fill;
            musicPlayer1.uiMode = "none";
            musicPlayer2.uiMode = "none";

            InitilizeGrid(dgPlaylist);

            InitilizeHideGrid();
            InitilizeCommanGrid();

            if (StaticClass.IsCopyright == true)
            {
                lblCopyrightLicence1.Visible = false;
                lblCopyrightLicence2.Visible = false;


                InitilizeNewestGrid();
                str = "SELECT TOP (150) Titles.TitleID as IdName, ltrim(Titles.Title) as textname, Artists.Name as ArtistName, Albums.Name AS AlbumName FROM Titles INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID where titlecategoryid=4 order by Titles.TitleID desc";
                FillNewestGrid(str);

                InitilizeCommanOptionGrid(dgBestOf);
                str = "select PlaylistId as IdName, ltrim(Name) as textName from Playlists where isPredefined=1 order by Name";
                FillCommanOptionGrid(str, dgBestOf);

                str = "SELECT TOP (300) Titles.TitleID, ltrim(Titles.Title) as Title,Titles.Time, Artists.Name as ArtistName, Albums.Name AS AlbumName FROM Titles INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID where titlecategoryid=4 order by Titles.TitleID desc";
                FillGrid(str);
            }
            else
            {
                lblCopyrightLicence1.Visible = true;
                lblCopyrightLicence1.Text = StaticClass.PlayerExpiryMessage;
                lblCopyrightLicence2.Visible = true;
                lblCopyrightLicence2.Text = StaticClass.PlayerExpiryMessage;
            }


            if (StaticClass.IsCopyright == false && StaticClass.IsFitness == true)
            {
                //FillFitnessGenreTitles(Convert.ToInt32(dgFitness.Rows[0].Cells[0].Value));
            }


            FillLocalPlaylist();
            delete_temp_table();
            InitilizeMusicGrid(dgMusicPlayer1);
            InitilizeMusicGrid(dgMusicPlayer2);


            if (dgLocalPlaylist.Rows.Count > 0)
            {
                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
            }

            str = "select * from tblmusic_player_settings where DFClientId=" + StaticClass.UserId + " and localUserId=" + StaticClass.LocalUserId + " and tokenno= " + StaticClass.TokenId;
            DataSet ds = new DataSet();
            ds = ObjMainClass.fnFillDataSet(str);
            if (dgLocalPlaylist.Rows.Count > 0 && dgPlaylist.Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    //PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    PlaySongDefault();
                }
                else
                {
                    //PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    Get_Last_Settings();
                }
                DisplaySongPlayerOne();
            }
            else
            {
                panPlayerButton.Enabled = true;
            }
            musicPlayer1.settings.volume = 100;
            musicPlayer2.settings.volume = 100;

            RowHide();

            GetFirstSong();
            DownloadSong(dgPlaylist);

            // LoadGif();
            SearchText = "";
            FillStreamData();
            FillStar(dgSongRatingPlayerOne);

            if (FirstTimeSong == true)
            {
                SetRating(dgSongRatingPlayerOne);
                SetDisableRating(dgSongRatingPlayerTwo);
            }
            else
            {
                Song_Set_foucs();
                if (musicPlayer1.currentMedia != null)
                {
                    g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                    GetSavedRating(g, dgSongRatingPlayerOne);
                    SetDisableRating(dgSongRatingPlayerTwo);
                }
            }

            ///////////////////  Advertisement Code /////////////////////
            string Advttype = "select * from tbAdvertisementType";
            ObjMainClass.fnFillAdvtComboBox(Advttype, cmbAdvertisementType, "AdvtTypeId", "AdvtTypeName", "");
            ObjMainClass.fnFillAdvtComboBox(Advttype, cmbAdvertisementTypeAdd, "AdvtTypeId", "AdvtTypeName", "");
            dtpAdvtStartDate.Value = DateTime.Now.Date;
            dtpAdvtEndDate.Value = DateTime.Now.Date;
            dtpSearchAdvt.Value = DateTime.Now.Date;
            dtpScheduleDate.Value = DateTime.Now.Date;
            FillMainAdvertisement();
            dtpScheduleDate.Value = DateTime.Now.Date;

            dtpSearchClientAdvt.Value = DateTime.Now.Date;

            ClearPlaylistSch();
            ////////////////////////////////////////////////////////////

            FillPrayer(dgPrayer);
        }




        //private void LoadGif()
        //{
        //    Image img = ((System.Drawing.Image)(Properties.Resources.Loading));
        //    MemoryStream mstr = new MemoryStream();
        //    img.Save(mstr, ImageFormat.Gif);
        //    picLoading.Image = Image.FromStream(mstr);
        //}
        private void GetFirstSong()
        {
            string file = "";
            string LocalFileName = "";
            if (dgCommanGrid.Rows.Count == 0) return;

            if (dgLocalPlaylist.Rows.Count == 0 && dgPlaylist.Rows.Count == 0)
            {
                isThreadFirstTime = true;
                timResetSong.Enabled = false;
                ShowPlaylistCounter = "Yes";
                SaveDefaultPlaylist(file);
                for (int iCtr = 0; (iCtr <= 24); iCtr++)
                {
                    file = dgCommanGrid.Rows[iCtr].Cells[0].Value.ToString();
                    LocalFileName = Application.StartupPath + "\\" + file + ".sec";
                    if (System.IO.File.Exists(LocalFileName))
                    {
                        insert_Playlist_song(file, "Yes", false);
                        PlaySongDefault();
                        FirstPlaySong = false;
                    }
                    else
                    {
                        FirstTimeSong = true;
                        FirstPlaySong = true;
                        Add_Playlist = true;
                        insert_temp_data(file);
                        multi_song_download();
                    }
                }
                return;

            }
            else if (dgLocalPlaylist.Rows.Count != 0 && dgPlaylist.Rows.Count == 0)
            {
                isThreadFirstTime = true;
                timResetSong.Enabled = false;
                ShowPlaylistCounter = "Yes";
                for (int iCtr = 0; (iCtr <= 24); iCtr++)
                {
                    file = dgCommanGrid.Rows[iCtr].Cells[0].Value.ToString();
                    LocalFileName = Application.StartupPath + "\\" + file + ".sec";
                    if (System.IO.File.Exists(LocalFileName))
                    {
                        insert_Playlist_song(file, "Yes", false);
                        PlaySongDefault();
                        FirstPlaySong = false;
                    }
                    else
                    {
                        FirstTimeSong = true;
                        FirstPlaySong = true;
                        Add_Playlist = true;
                        insert_temp_data(file);
                        multi_song_download();
                    }
                }
                return;
            }
        }
        private void InitilizeMusicGrid(DataGridView dgGrid)
        {
            if (dgGrid.Rows.Count > 0)
            {
                dgGrid.Rows.Clear();
            }
            if (dgGrid.Columns.Count > 0)
            {
                dgGrid.Columns.Clear();
            }
            dgGrid.Columns.Add("songid", "Song Id");
            dgGrid.Columns["songid"].Width = 200;
            dgGrid.Columns["songid"].Visible = true;
            dgGrid.Columns["songid"].ReadOnly = true;
        }
        void delete_temp_table()
        {
            try
            {
                if (StaticClass.constr.State == ConnectionState.Open)
                {
                    StaticClass.constr.Close();
                }

                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = StaticClass.constr;
                cmd.CommandText = "delete from temp_song where tokenid=" + StaticClass.TokenId;
                cmd.ExecuteNonQuery();
                StaticClass.constr.Close();
            }
            catch
            {

            }
        }

        void delete_temp_data(string songid)
        {
            if (StaticClass.constr.State == ConnectionState.Open)
            {
                StaticClass.constr.Close();
            }

            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = StaticClass.constr;
            cmd.CommandText = "delete from temp_song where tempid=" + Convert.ToInt32(songid);
            cmd.ExecuteNonQuery();
            StaticClass.constr.Close();
        }
        void insert_temp_data(string songid)
        {
            string filePath = "";
            try
            {
                filePath = Application.StartupPath + "\\" + songid + ".mp3";
                if (StaticClass.constr.State == ConnectionState.Open)
                {
                    StaticClass.constr.Close();
                }

                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = StaticClass.constr;
                cmd.CommandText = "INSERT INTO temp_song(tempid, tempSongid,tokenid)   VALUES(@param1,@param2, @param3)";

                cmd.Parameters.AddWithValue("@param1", songid);
                cmd.Parameters.AddWithValue("@param2", songid);
                cmd.Parameters.AddWithValue("@param3", StaticClass.TokenId);
                cmd.ExecuteNonQuery();
                StaticClass.constr.Close();

            }
            catch
            {
                if (System.IO.File.Exists(filePath))
                {
                    delete_temp_data(songid);
                }
            }
        }
        private void InsertDownloadSong(string songid)
        {
            try
            {
                if (StaticClass.constr.State == ConnectionState.Open)
                {
                    StaticClass.constr.Close();
                }

                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = StaticClass.constr;
                cmd.CommandText = "INSERT INTO userDownloadTitle(DfClientId, titleId,tokenid)   VALUES(@param1,@param2,@param3)";

                cmd.Parameters.AddWithValue("@param1", StaticClass.UserId);
                cmd.Parameters.AddWithValue("@param2", songid);
                cmd.Parameters.AddWithValue("@param3", StaticClass.TokenId);
                // cmd.ExecuteNonQuery();
                StaticClass.constr.Close();
            }
            catch
            {
            }

        }
        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
        Up:
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream streamRemote = null;
            Stream streamLocal = null;
            downloadSongName = "";
            try
            {
                String RemoteFtpPath = "http://85.195.82.94/mp3files/" + fileName;
                String LocalDestinationPath = Application.StartupPath + "\\" + fileName;

                string sUrlToReadFileFrom = RemoteFtpPath;
                string sFilePathToWriteFileTo = LocalDestinationPath;

                Uri url = new Uri(sUrlToReadFileFrom);
                request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                response = (System.Net.HttpWebResponse)request.GetResponse();
                response.Close();
                Int64 iSize = response.ContentLength;
                Int64 iRunningByteTotal = 0;//
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    using (streamRemote = client.OpenRead(new Uri(sUrlToReadFileFrom)))
                    {
                        using (streamLocal = new FileStream(sFilePathToWriteFileTo, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            int iByteSize = 0;
                            byte[] byteBuffer = new byte[iSize];
                            while ((iByteSize = streamRemote.Read(byteBuffer, 0, byteBuffer.Length)) > 0)
                            {
                                streamLocal.Write(byteBuffer, 0, iByteSize);
                                iRunningByteTotal += iByteSize;
                                double dIndex = (double)(iRunningByteTotal);
                                double dTotal = (double)byteBuffer.Length;
                                double dProgressPercentage = (dIndex / dTotal);
                                int iProgressPercentage = (int)(dProgressPercentage * 100);
                                bgWorker.ReportProgress(iProgressPercentage);
                            }
                            streamLocal.Close();
                        }
                        streamRemote.Close();
                    }
                }
                downloadSongName = LocalDestinationPath;
                Stop_Insert = false;


            }
            catch (Exception ex)
            {

                if (LocalSecondTime == "1")
                {
                    LocalSecondTime = "";
                    if (bgWorker.IsBusy == true)
                    {
                        streamLocal = null;
                        streamRemote = null;
                        request = null;
                        response = null;
                        bgWorker.CancelAsync();
                        bgWorker.Dispose();
                        if (bgWorker.CancellationPending == true)
                        {
                            e.Cancel = true;
                        }
                    }
                    goto Up;
                }
                multi_song_download();
                Add_Playlist = false;
                FirstTimeSong = false;
                Drop_Song = false;
                Stop_Insert = true;
                return;
            }

        }

        void Update_NotFound_Titles(string songid)
        {
            string songid_Local = "";
            string strLocal = songid;
            string[] arr = strLocal.Split('.');
            songid_Local = arr[0];
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = StaticClass.constr;
            cmd.CommandText = "update Titles  set IsDownload=0 where titleid=" + Convert.ToInt32(songid_Local);
            cmd.ExecuteNonQuery();
            StaticClass.constr.Close();
        }
        private void AddSongGrid(string TempSongName, string file, int X, int Y)
        {
            int Index = 0;
            drawLine = false;
            dgPlaylist.Invalidate();
            if (System.IO.File.Exists(TempSongName))
            {
                insert_Playlist_song(file, "No", false);
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    Point clientPoint = dgPlaylist.PointToClient(new Point(X, Y));
                    Index = dgPlaylist.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
                    if (dgPlaylist.Rows.Count == 0 || dgPlaylist.Rows.Count == 1)
                    {
                        dgPlaylist.Rows.Add();
                        Index = 0;
                        ResetPlaylist(dgPlaylist, Index, file);
                        PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                        DownloadSong(dgPlaylist);
                        return;

                    }
                    else if (Index == -1)
                    {
                        Index = 1;
                        ResetPlaylist(dgPlaylist, Index, file);
                        DownloadSong(dgPlaylist);
                        return;
                    }
                    else if (Index != -1)
                    {
                        ResetPlaylist(dgPlaylist, Index, file);

                    }
                }
                else
                {
                    Point clientPoint = dgOtherPlaylist.PointToClient(new Point(X, Y));
                    Index = dgOtherPlaylist.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
                    if (dgOtherPlaylist.Rows.Count == 0 || dgOtherPlaylist.Rows.Count == 1)
                    {
                        dgOtherPlaylist.Rows.Add();
                        Index = 0;
                        ResetPlaylist(dgOtherPlaylist, Index, file);
                        PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                        DownloadSong(dgOtherPlaylist);
                        return;

                    }
                    else if (Index == -1)
                    {
                        Index = 1;
                        ResetPlaylist(dgOtherPlaylist, Index, file);
                        DownloadSong(dgOtherPlaylist);
                        return;
                    }
                    else if (Index != -1)
                    {
                        ResetPlaylist(dgOtherPlaylist, Index, file);

                    }
                }
                DownloadSong(dgPlaylist);
            }
        }
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lblPercentage.Text = e.ProgressPercentage + "%";
        }


        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            string sName = "";
            string TempName = "";
            string GetDownloadLocalPath = "";
            GC.Collect();
            lblPercentage.Visible = false;
            progressBar1.Value = 0;
            GetDownloadLocalPath = downloadSongName;
            if (System.IO.File.Exists(GetDownloadLocalPath))
            {
                clsSongCrypt.encrfile(new Uri(downloadSongName, UriKind.Relative));
            }

            sName = temp_songid;
            TempName = Application.StartupPath + "\\" + sName + ".sec";
            delete_temp_data(temp_songid);
            if (Stop_Insert == false)
            {
                InsertDownloadSong(temp_songid);
            }


            //Drop_Song = true;
            //IsDrop_Song = true;


            // insert_Playlist_song(sName);
            if (Add_Playlist == true)
            {
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    dgPlaylist.Invalidate();
                    if (dgPlaylist.Rows.Count >= 700)
                    {
                        MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Copyright Player");
                        return;
                    }
                }
                else
                {
                    dgOtherPlaylist.Invalidate();
                    if (dgOtherPlaylist.Rows.Count >= 700)
                    {
                        MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Copyright Player");
                        return;
                    }
                }
                if (DropTitleSong == "Yes")
                {
                    DropTitleSong = "No";
                    AddSongGrid(TempName, sName, eX, eY);
                    //  return;
                }
                else
                {
                    //DeleteHideSong(sName);
                    if (IsLast100Working == "Yes")
                    {
                        insert_Playlist_song(sName, "No", false);
                        dgPlaylist.Rows.Add();
                        dgPlaylist.Rows[dgPlaylist.Rows.Count - 1].Cells[0].Value = sName;

                    }
                    else
                    {
                        if (IsDrop_Song == true)
                        {
                            insert_Playlist_song(sName, "No", true);
                            dgPlaylist.Rows.Add();
                            dgPlaylist.Rows[dgPlaylist.Rows.Count - 1].Cells[0].Value = sName;
                        }
                        else
                        {
                            insert_Playlist_song(sName, "Yes", false);
                        }
                    }
                }
            }

            if (IsDrop_Song == true)
            {
                if (musicPlayer1.URL != "")
                {
                    Set_foucs_PayerOne();
                }
                else if (musicPlayer2.URL != "")
                {
                    Set_foucs_PayerTwo();
                }
            }

            if (FirstTimeSong == true)
            {
                if (dgPlaylist.Rows.Count > 1)
                {
                    NextSongDisplay(dgPlaylist.Rows[1].Cells[0].Value.ToString());
                    isThreadFirstTime = false;
                    //SetDisableRating(dgSongRatingPlayerTwo);
                }
                // insert_Playlist_song(sName, "Yes");

                if (FirstPlaySong == true)
                {
                    PlaySongDefault();
                    DisplaySongPlayerOne();
                    Song_Set_foucs();
                    FirstPlaySong = false;
                }
            }
            if (Drop_Song == true)
            {
                if (Drop_TitleName == sName)
                {
                    Drop_TitleName = "";
                    if (musicPlayer2.URL == "")
                    {
                        Drop_Song = false;
                        DeleteHideSongs();
                        InsertHideSong(sName);
                        RowHide();
                        NextSongDisplay(sName);
                        Song_Set_foucs3(sName);
                        GetDropSongRow(sName);
                        // DownloadSong();
                        //   return;
                    }
                    else if (musicPlayer1.URL == "")
                    {
                        Drop_Song = false;
                        DeleteHideSongs();
                        InsertHideSong(sName);
                        RowHide();
                        NextSongDisplay2(sName);
                        Song_Set_foucs3(sName);
                        GetDropSongRow(sName);
                        //  DownloadSong();
                        // return;
                    }
                }
            }
            if (musicPlayer1.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");
            }
            else if (musicPlayer2.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");
            }
            if (dgPlaylist.Visible == true)
            {
                DownloadSong(dgPlaylist);
            }
            else
            {
                DownloadSong(dgOtherPlaylist);
            }
            
            if (ShowPlaylistCounter == "Yes")
            {
                GetSongCounter();
            }
            multi_song_download();

        }
        private void Set_foucs_PayerOne()
        {
            try
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                    {

                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            DeleteParticularHideSong();
                            UpdateHideSong(g.ToString());
                        }
                        else
                        {
                            dgPlaylist.CurrentCell = dgPlaylist.Rows[i].Cells[1];
                            dgPlaylist.Rows[i].Selected = true;

                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionBackColor = Color.FromArgb(20, 162, 175);
                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.White;
                        }
                        lblSongName.ForeColor = Color.Yellow;
                        lblArtistName.ForeColor = Color.Yellow;
                        lblMusicTimeOne.ForeColor = Color.Yellow;
                        lblSongDurationOne.ForeColor = Color.Yellow;
                        pbarMusic1.ForeColor = Color.Yellow;
                        panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));
                        pbarMusic1.BackColor = Color.FromArgb(9, 130, 154);

                        lblSongName2.ForeColor = Color.Gray;
                        lblArtistName2.ForeColor = Color.Gray;
                        lblMusicTimeTwo.ForeColor = Color.Gray;
                        lblSongDurationTwo.ForeColor = Color.Gray;
                        pbarMusic2.ForeColor = Color.Gray;
                        pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
                        panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));

                    }
                }
            }
            catch
            {
            }
        }
        private void Set_foucs_PayerTwo()
        {
            try
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                    {
                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            DeleteParticularHideSong();
                            UpdateHideSong(g.ToString());
                        }
                        else
                        {
                            dgPlaylist.CurrentCell = dgPlaylist.Rows[i].Cells[1];
                            dgPlaylist.Rows[i].Selected = true;
                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionBackColor = Color.FromArgb(20, 162, 175);
                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.White;
                        }

                        lblSongName2.ForeColor = Color.Yellow;
                        lblArtistName2.ForeColor = Color.Yellow;
                        lblMusicTimeTwo.ForeColor = Color.Yellow;
                        lblSongDurationTwo.ForeColor = Color.Yellow;
                        pbarMusic2.ForeColor = Color.Yellow;
                        pbarMusic2.BackColor = Color.FromArgb(9, 130, 154);
                        panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));

                        lblSongName.ForeColor = Color.Gray;
                        lblArtistName.ForeColor = Color.Gray;
                        lblMusicTimeOne.ForeColor = Color.Gray;
                        lblSongDurationOne.ForeColor = Color.Gray;
                        pbarMusic1.ForeColor = Color.Gray;
                        pbarMusic1.BackColor = Color.FromArgb(175, 175, 175);
                        panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));

                        return;
                    }
                }
            }
            catch { }
        }
        private void GetDropSongRow(string DropSongId)
        {
            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
            {
                if (dgPlaylist.Rows.Count > 0)
                {
                    if (DropSongId == dgPlaylist.Rows[i].Cells[0].Value.ToString())
                    {
                        CurrentRow = i - 1;
                    }
                }
            }
        }
        private void DownloadSong(DataGridView dgGrid)
        {
            string GetLocalPath;
            string SongFind = "";
            for (int i = 0; i < dgCommanGrid.Rows.Count; i++)
            {
                GetLocalPath = Application.StartupPath + "\\" + dgCommanGrid.Rows[i].Cells[0].Value + ".sec";
                SongFind = "";
                if (System.IO.File.Exists(GetLocalPath))
                {
                    if (dgGrid.Rows.Count > 0)
                    {
                        for (int a = 0; a < dgGrid.Rows.Count; a++)
                        {
                            if (dgCommanGrid.Rows[i].Cells[0].Value.ToString() == dgGrid.Rows[a].Cells[0].Value.ToString())
                            {
                                dgCommanGrid.Rows[i].Cells[5].Style.ForeColor = Color.LightGreen;
                                dgCommanGrid.Rows[i].Cells[5].Style.BackColor = Color.LightGreen;
                                SongDownload.FlatStyle = FlatStyle.Popup;
                                SongFind = "Y";
                            }
                            else
                            {
                                if (SongFind != "Y")
                                {
                                    dgCommanGrid.Rows[i].Cells[5].Style.ForeColor = Color.Plum;
                                    dgCommanGrid.Rows[i].Cells[5].Style.BackColor = Color.Plum;
                                    SongDownload.FlatStyle = FlatStyle.Popup;
                                }
                            }
                        }
                    }
                    else
                    {
                        dgCommanGrid.Rows[i].Cells[5].Style.ForeColor = Color.Plum;
                        dgCommanGrid.Rows[i].Cells[5].Style.BackColor = Color.Plum;
                        SongDownload.FlatStyle = FlatStyle.Popup;
                    }

                }
                else
                {
                    dgCommanGrid.Rows[i].Cells[5].Style.ForeColor = Color.White;
                    dgCommanGrid.Rows[i].Cells[5].Style.BackColor = Color.Gainsboro;
                    SongDownload.FlatStyle = FlatStyle.Popup;
                }
            }
        }

        private void RecordAdd(DataGridView dgGrid, string songTitle)
        {
            string IsExist = "No";

            for (int i = 0; i < dgGrid.Rows.Count; i++)
            {
                if (Convert.ToString(dgGrid.Rows[i].Cells[0].Value) == songTitle)
                {
                    IsExist = "Yes";
                }

            }
            if (IsExist == "No")
            {
                dgGrid.Rows.Add();
                dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[0].Value = songTitle;
            }
        }

        void multi_song_download()
        {

            string mlsSql = "";
            string songId = "";
            int i;

            mlsSql = "select * from temp_song where tokenid=" + StaticClass.TokenId;
            DataSet ds = new DataSet();
            ds = ObjMainClass.fnFillDataSet(mlsSql);
            if (bgWorker.IsBusy == false)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    songId = ds.Tables[0].Rows[i]["tempSongid"].ToString();
                    temp_songid = songId;
                    fileName = songId + ".mp3";
                    //lblSongDown.Text = "Song is downloading";
                    string filePath = Application.StartupPath + "\\" + fileName;

                    if (System.IO.File.Exists(filePath))
                    {


                    }
                    else
                    {

                        if (bgWorker.IsBusy == false)
                        {
                            if (isThreadFirstTime == true)
                            {
                                if (dgLocalPlaylist.Rows.Count == 1)
                                {
                                    if (dgPlaylist.Rows.Count == 0)
                                    {
                                        FirstTimeSong = true;
                                        FirstPlaySong = true;
                                        Add_Playlist = true;
                                    }
                                }
                            }
                            LocalSecondTime = "1";
                            lblPercentage.Visible = true;
                            bgWorker.RunWorkerAsync();
                            break;
                        }
                    }

                }
            }

        }
        private void musicPlayer1_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            try
            {


                TimerEventProcessorPlayerTwo();
                //Song_Set_foucs();


                if (Song_Mute == true)
                {
                    musicPlayer1.settings.mute = true;
                }
                else
                {
                    musicPlayer1.settings.mute = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            musicPlayer1.Ctlcontrols.play();
            timer1.Enabled = false;
        }
        private void PlaylistSave()
        {
            string msQl = "";
            Int32 Playlist_Id = 0;
            if (StaticClass.constr.State == ConnectionState.Open)
            {
                StaticClass.constr.Close();
            }

            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("InsertPlayListsNew", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt));
            cmd.Parameters["@UserID"].Value = StaticClass.UserId;

            cmd.Parameters.Add(new SqlParameter("@IsPredefined", SqlDbType.Bit));
            cmd.Parameters["@IsPredefined"].Value = 0;

            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
            cmd.Parameters["@Name"].Value = txtPlaylistName.Text;

            cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.VarChar, 50));
            cmd.Parameters["@Summary"].Value = " ";

            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 50));
            cmd.Parameters["@Description"].Value = " ";

            cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
            cmd.Parameters["@TokenId"].Value = StaticClass.TokenId;

            try
            {
                Playlist_Id = Convert.ToInt32(cmd.ExecuteScalar());
                ModifyPlaylistId = Playlist_Id;
                string sQr = "";

                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                sQr = "insert into PlayLists values(" + Convert.ToInt32(Playlist_Id) + ", ";
                sQr = sQr + StaticClass.UserId + " , '" + txtPlaylistName.Text + "', " + StaticClass.TokenId + ",'' )";
                StaticClass.LocalCon.Open();
                OleDbCommand cmdSaveLocal = new OleDbCommand();
                cmdSaveLocal.Connection = StaticClass.LocalCon;
                cmdSaveLocal.CommandText = sQr;
                cmdSaveLocal.ExecuteNonQuery();
                StaticClass.LocalCon.Close();



                // MessageBox.Show("Saved");
            }
            catch (Exception ex)
            {
                // throw new ApplicationException ("Data error.");
                // MessageBox.Show(ex.Message);
            }
            finally
            {
                StaticClass.constr.Close();
            }
        }

        private void PlaylistModify()
        {
            if (StaticClass.constr.State == ConnectionState.Open)
            {
                StaticClass.constr.Close();
            }

            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("UpdateUserPlayLists", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@PlayListID", SqlDbType.BigInt));
            cmd.Parameters["@PlayListID"].Value = ModifyPlaylistId;

            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
            cmd.Parameters["@Name"].Value = txtPlaylistName.Text;
            try
            {
                cmd.ExecuteNonQuery();
                string sQr = "";
                if (StaticClass.LocalCon.State == ConnectionState.Open)
                {
                    StaticClass.LocalCon.Close();
                }
                sQr = "update PlayLists set Name= '" + txtPlaylistName.Text + "' ";
                sQr = sQr + " where PlayListID= " + Convert.ToInt32(ModifyPlaylistId);
                StaticClass.LocalCon.Open();
                OleDbCommand cmdSaveLocal = new OleDbCommand();
                cmdSaveLocal.Connection = StaticClass.LocalCon;
                cmdSaveLocal.CommandText = sQr;
                cmdSaveLocal.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
            finally
            {
                StaticClass.constr.Close();
            }
        }

        private void FillCommanOptionGrid(string str, DataGridView Grid_Name)
        {
            int iCtr;
            try
            {
                InitilizeCommanOptionGrid(Grid_Name);
                DataTable dtDetail;
                dtDetail = ObjMainClass.fnFillDataTable_OldDB(str);
                if ((dtDetail.Rows.Count > 0))
                {
                    BestOffRecordShowType = "MainAlbum";
                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {

                        Grid_Name.Rows.Add();
                        Grid_Name.Rows[Grid_Name.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["IdName"];
                        Grid_Name.Rows[Grid_Name.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["textname"];
                    }
                    foreach (DataGridViewRow row in Grid_Name.Rows)
                    {
                        row.Height = 33;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        private void FillNewestGrid(string str)
        {
            int iCtr;
            try
            {
                InitilizeNewestGrid();
                DataTable dtDetail;
                dtDetail = ObjMainClass.fnFillDataTable_OldDB(str);
                if ((dtDetail.Rows.Count > 0))
                {
                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {

                        dgNewest.Rows.Add();
                        dgNewest.Rows[dgNewest.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["IdName"];
                        dgNewest.Rows[dgNewest.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["textname"];
                        dgNewest.Rows[dgNewest.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["ArtistName"];

                        dgNewest.Rows[dgNewest.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                        dgNewest.Rows[dgNewest.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);

                    }
                    foreach (DataGridViewRow row in dgNewest.Rows)
                    {
                        row.Height = 33;
                    }

                    //dgNewest.Sort(dgNewest.Columns[1], ListSortDirection.Ascending);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void FillGrid(string str)
        {
            int iCtr;

            string TitleTime = "";
            try
            {
                InitilizeCommanGrid();
                DataTable dtDetail;
                string GetLocalPath;
                dtDetail = ObjMainClass.fnFillDataTable_OldDB(str);
                if ((dtDetail.Rows.Count > 0))
                {

                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {

                        dgCommanGrid.Rows.Add();
                        dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["TitleID"];
                        dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["Title"];

                        string strTime = dtDetail.Rows[iCtr]["Time"].ToString();
                        string[] arr = strTime.Split(':');
                        TitleTime = arr[1] + ":" + arr[2];

                        dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[2].Value = TitleTime;
                        dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[3].Value = dtDetail.Rows[iCtr]["ArtistName"];

                        dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[4].Value = dtDetail.Rows[iCtr]["AlbumName"];

                        if (this.Width.ToString() == "1020")
                        {
                            dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 11);
                            dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[4].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            SongDownload.Width = 30;
                        }
                        GetLocalPath = Application.StartupPath + "\\" + dtDetail.Rows[iCtr]["TitleID"] + ".sec";
                        if (System.IO.File.Exists(GetLocalPath))
                        {
                            dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[5].Style.ForeColor = Color.LightGreen;
                            dgCommanGrid.Rows[dgCommanGrid.Rows.Count - 1].Cells[5].Style.BackColor = Color.LightGreen;
                            SongDownload.FlatStyle = FlatStyle.Popup;
                        }
                    }
                    if (this.Width.ToString() == "1020")
                    {
                        foreach (DataGridViewRow row in dgCommanGrid.Rows)
                        {
                            row.Height = 33;
                        }
                    }
                    else
                    {
                        dgCommanGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12);
                        foreach (DataGridViewRow row in dgCommanGrid.Rows)
                        {
                            row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                            row.Cells[2].Style.Font = new Font("Segoe UI", 12);
                            row.Cells[3].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                            row.Cells[4].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                            row.Height = 35;
                            SongDownload.Width = 50;
                        }
                    }
                    //dgCommanGrid.Sort(dgCommanGrid.Columns[1], ListSortDirection.Ascending);
                }

            }

            catch
            {

                return;
            }
        }
        void insert_Playlist_song_LocalDatabase(string song_id, Boolean IsComeDropSong)
        {
            string sWr = "";
            var Special_Name = "";
            string Special_Change = "";
            int Playlist_Id = 0;
            if (IsComeDropSong == true)
            {
                Playlist_Id = StaticClass.DefaultPlaylistId;
            }
            else
            {
                if (IsLast100Working == "Yes")
                {
                    Playlist_Id = StaticClass.Last100PlaylistId;
                }
                else
                {
                    Playlist_Id = Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);
                }
            }
            Int32 AlbumID = 0;
            Int32 ArtistID = 0;
            string sQr = "";
            DataSet dsAlbum = new DataSet();
            try
            {
                sQr = "select * from Titles where TitleID=" + song_id;
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet_Local(sQr);
                if (ds.Tables[0].Rows.Count <= 0)
                {

                    sQr = "select TitleID,AlbumID,ArtistID,Title,Gain,isnull(TitleYear,0) as TitleYear,Time from Titles where TitleID=" + song_id;
                    DataSet dsTitle = new DataSet();
                    dsTitle = ObjMainClass.fnFillDataSet_OldDB(sQr);
                    AlbumID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["AlbumID"]);
                    ArtistID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["ArtistID"]);
                    Special_Name = dsTitle.Tables[0].Rows[0]["Title"].ToString().Trim().Replace("\0","");
                    Special_Change = Special_Name.Replace("'", "??$$$??");
                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    sWr = "insert into Titles values (" + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["TitleID"]) + " , " + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["AlbumID"]) + " , ";
                    sWr = sWr + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["ArtistID"]) + ", '" + Special_Change + "' , ";
                    sWr = sWr + "'" + dsTitle.Tables[0].Rows[0]["Gain"] + "' , '" + dsTitle.Tables[0].Rows[0]["Time"] + "' ,";
                    sWr = sWr + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["TitleYear"]) + ")";
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdTitle = new OleDbCommand();
                    cmdTitle.Connection = StaticClass.LocalCon;
                    cmdTitle.CommandText = sWr;
                    cmdTitle.ExecuteNonQuery();

                }
                else
                {
                    sQr = "select TitleID,AlbumID,ArtistID,Title,Gain,isnull(TitleYear,0) as TitleYear,Time from Titles where TitleID=" + song_id;
                    DataSet dsTitle = new DataSet();
                    dsTitle = ObjMainClass.fnFillDataSet_OldDB(sQr);
                    AlbumID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["AlbumID"]);
                    ArtistID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["ArtistID"]);

                }
                Special_Name = "";
                Special_Change = "";
                sQr = "select * from Albums where albumid=" + Convert.ToInt32(AlbumID);
                DataSet dsAlbumLocal = new DataSet();
                dsAlbumLocal = ObjMainClass.fnFillDataSet_Local(sQr);
                if (dsAlbumLocal.Tables[0].Rows.Count <= 0)
                {
                    sQr = "select * from Albums where albumid=" + Convert.ToInt32(AlbumID);
                    dsAlbum = ObjMainClass.fnFillDataSet_OldDB(sQr);

                    Special_Name = dsAlbum.Tables[0].Rows[0]["Name"].ToString().Replace("\0", "");



                    Special_Change = Special_Name.Replace("'", "??$$$??");

                    if (Special_Change.Length > 50)
                    {
                        Special_Change = Special_Change.Substring(0, 49);
                    }

                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    sWr = "insert into Albums values (" + Convert.ToInt32(dsAlbum.Tables[0].Rows[0]["AlbumID"]) + " , ";
                    sWr = sWr + Convert.ToInt32(dsAlbum.Tables[0].Rows[0]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdAlbum = new OleDbCommand();
                    cmdAlbum.Connection = StaticClass.LocalCon;
                    cmdAlbum.CommandText = sWr;
                    cmdAlbum.ExecuteNonQuery();

                }
                Special_Name = "";
                Special_Change = "";

                sQr = "select * from Artists where ArtistID=" + Convert.ToInt32(ArtistID);
                DataSet dsArtistLocal = new DataSet();
                dsArtistLocal = ObjMainClass.fnFillDataSet_Local(sQr);
                if (dsArtistLocal.Tables[0].Rows.Count <= 0)
                {
                    sQr = "select * from Artists where ArtistID=" + Convert.ToInt32(ArtistID);
                    DataSet dsArtist = new DataSet();
                    dsArtist = ObjMainClass.fnFillDataSet_OldDB(sQr);
                    Special_Name = dsArtist.Tables[0].Rows[0]["Name"].ToString().Trim().Replace("\0","");
                    Special_Change = Special_Name.Replace("'", "??$$$??");

                    if (Special_Change.Length > 50)
                    {
                        Special_Change = Special_Change.Substring(0, 49);
                    }

                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    sWr = "";
                    sWr = "insert into Artists values (" + Convert.ToInt32(dsArtist.Tables[0].Rows[0]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdAlbum = new OleDbCommand();
                    cmdAlbum.Connection = StaticClass.LocalCon;
                    cmdAlbum.CommandText = sWr;
                    cmdAlbum.ExecuteNonQuery();
                }




                //(Convert.ToInt32(ds.Tables[0].Rows[0]["lastPlaylistId"]));



                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    sWr = "insert into TitlesInPlaylists values (" + Playlist_Id + " , " + Convert.ToInt32(song_id) + ", " + Convert.ToInt32(dgPlaylist.Rows.Count - 1) + ")";
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = StaticClass.LocalCon;
                    cmd.CommandText = sWr;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    sWr = "insert into TitlesInPlaylists values (" + Playlist_Id + " , " + Convert.ToInt32(song_id) + ", " + Convert.ToInt32(dgOtherPlaylist.Rows.Count - 1) + ")";
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = StaticClass.LocalCon;
                    cmd.CommandText = sWr;
                    cmd.ExecuteNonQuery();
                }
                // DownloadSong();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }


        }

        void insert_Playlist_song(string songid, string GridReset, Boolean IsComeDropSong)
        {
            try
            {
                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand("InsertTitlesInPlayLists", StaticClass.constr);
                cmd.CommandType = CommandType.StoredProcedure;

                if (IsLast100Working == "Yes")
                {
                    cmd.Parameters.Add(new SqlParameter("@PlayListID", SqlDbType.BigInt));
                    cmd.Parameters["@PlayListID"].Value = StaticClass.Last100PlaylistId;
                }
                else
                {
                    cmd.Parameters.Add(new SqlParameter("@PlayListID", SqlDbType.BigInt));
                    if (IsComeDropSong == false)
                    {
                        cmd.Parameters["@PlayListID"].Value = Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);
                    }
                    else
                    {
                        cmd.Parameters["@PlayListID"].Value = StaticClass.DefaultPlaylistId;
                    }
                }
                cmd.Parameters.Add(new SqlParameter("@TitleID", SqlDbType.BigInt));
                cmd.Parameters["@TitleID"].Value = songid;
                cmd.ExecuteNonQuery();
                insert_Playlist_song_LocalDatabase(songid, IsComeDropSong);
                if ((GridReset == "Yes") && IsComeDropSong == true)
                {
                    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));
                }
                else if ((GridReset == "Yes") && IsComeDropSong == false)
                {

                    if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                    {
                        PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    }
                    else
                    {
                        PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    }

                }
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    rCount = 0;
                    //DropSongLength = "";
                    //IsSongDropAdvt = false;
                    label7.Text = "0";
                    label8.Text = "0";
                    label14.Text = "0";
                    IsAdvtTimeGet = false;
                    GrossTotaltime = 0;
                    timGetRemainAdvtTime.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // throw new ApplicationException("Data error.");
            }
            finally
            {
                // StaticClass.constr.Close();
            }

        }
        private void PlaySongDefault()
        {
            string MusicFileName = "";
            string TempMusicFileName = "";
            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
            {
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value));
                    musicPlayer1.URL = MusicFileName;
                    if (btnStreamMute.Text == "")
                    {
                        musicPlayer1.settings.volume = 100;
                    }
                    else
                    {
                        if (btnMute.Text == "")
                        {
                            btnMute.Text = ".";
                            musicPlayer1.settings.mute = true;
                            Song_Mute = true;
                            btnMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_red));
                        }
                    }
                    CurrentRow = i;
                    CurrentPlaylistRow = dgLocalPlaylist.CurrentCell.RowIndex;
                    if (CurrentRow == dgPlaylist.Rows.Count - 1)
                    {
                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                    }
                    else
                    {
                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                    }
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label3.Text = musicPlayer1.settings.volume.ToString();
            //Form1 objform1 = new Form1();
            //objform1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            musicPlayer1.settings.volume = 40;
            musicPlayer1.Ctlcontrols.currentPosition = 15;
            //Form2 objform2 = new Form2();
            //objform2.Show();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            musicPlayer2.Ctlcontrols.play();
            timer2.Enabled = false;
        }
        private void TimerEventProcessorPlayerTwo()
        {
            if (IsbtnClick == "N")
            {
                timer4.Enabled = false;
                timer5.Enabled = false;
            }
            else
            {
                timer4.Enabled = true;
                timer5.Enabled = true;
            }
            timAutoFadePlayerOne.Enabled = true;
            timAutoFadePlayerTwo.Enabled = true;
            //            timer3.Interval = 1000;
            //           timer3.Enabled = true;
            timMusicTimeOne.Enabled = true;
            timMusicTimeTwo.Enabled = true;

        }

        private void TimerEventProcessorPlayerOne()
        {

            if (IsbtnClick == "N")
            {
                timer4.Enabled = false;
                timer5.Enabled = false;
            }
            else
            {
                timer4.Enabled = true;
                timer5.Enabled = true;
            }
            timAutoFadePlayerOne.Enabled = true;
            timAutoFadePlayerTwo.Enabled = true;
            //            timer3.Interval = 1000;
            //           timer3.Enabled = true;
            timMusicTimeOne.Enabled = true;
            timMusicTimeTwo.Enabled = true;

        }

        private void timer3_Tick(object sender, EventArgs e)
        {

            double t = Math.Floor(musicPlayer1.currentMedia.duration - musicPlayer1.Ctlcontrols.currentPosition);
            double a = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
            timeRemaining.Text = (t.ToString());
            //            lblCurrentTiming.Text =  a.ToString();

        }


        private void PlaylistFadeSong()
        {
            string MusicFileName = "";
            string TempMusicFileName = "";
            string mlsSql = "";
            string FindSong = "";
            DataTable dtDetail;

            GetOldSongIdPlayer1();
            if (CurrentRow >= dgPlaylist.Rows.Count - 1)
            {

                CurrentRow = LastRowId;
                //if (LastRowId == dgPlaylist.Rows.Count - 1)
                //{
                //    CurrentRow = 0;
                //}
                //else
                //{
                //    CurrentRow = LastRowId;
                //}
            }
            if (dgPlaylist.Rows.Count == 0)
            {
                IsLast100Working = "No";
            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
            //{
            //    CurrentPlaylistRow = 0;

            //}
            //else
            //{
            //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
            //}
            GHTE:
                for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count > 0))
                    {
                        //CurrentPlaylistRow = i;
                        FindSong = "True";
                        break;
                    }
                    else
                    {
                        FindSong = "false";
                        //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                        //{
                        //    CurrentPlaylistRow = 0;
                        //}
                        //else
                        //{
                        //    CurrentPlaylistRow = i;
                        //}
                    }
                }
                if (FindSong == "false")
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count == 0))
                    {
                        // CurrentPlaylistRow = 0;
                        goto GHTE;
                    }
                }
                // dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[1];
                // dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Selected = true;

                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));
                CurrentRow = 0;
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer2.URL = MusicFileName;
                    musicPlayer2.settings.volume = 0;

                    //if (CurrentRow == dgPlaylist.Rows.Count - 1)
                    //{
                    //    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                    //}
                    //else
                    //{
                    //    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                    //}

                    timer2.Enabled = true;
                    DisplaySongPlayerTwo();
                    return;
                }
            }

            if (dgPlaylist.Rows.Count - 1 == 0)
            {
                IsLast100Working = "No";
            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
            //{
            //    CurrentPlaylistRow = 0;

            //}
            //else
            //{
            //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
            //}
            GHT:
                for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count > 0))
                    {
                        // CurrentPlaylistRow = i;
                        FindSong = "True";
                        break;
                    }
                    else
                    {
                        FindSong = "false";
                        //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                        //{
                        //    CurrentPlaylistRow = 0;
                        //}
                        //else
                        //{
                        //    CurrentPlaylistRow = i;
                        //}
                    }
                }

                if (FindSong == "false")
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count == 0))
                    {
                        //CurrentPlaylistRow = 0;
                        goto GHT;
                    }
                }
                //dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];
                //                dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;
                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));
                CurrentRow = 0;
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer2.URL = MusicFileName;
                    musicPlayer2.settings.volume = 0;

                    timer2.Enabled = true;
                    DisplaySongPlayerTwo();
                    return;
                }
            }




        gg:
            if (CurrentRow == dgPlaylist.Rows.Count - 1)
            {
                if (IsDrop_Song == false)
                {
                    //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                    //{
                    //    CurrentPlaylistRow = 0;

                    //}
                    //else
                    //{
                    //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
                    //}

                    for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                    {
                        mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                        dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                        if ((dtDetail.Rows.Count > 0))
                        {
                            //  CurrentPlaylistRow = i;
                            break;
                        }
                        else
                        {
                            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                            //{
                            //    CurrentPlaylistRow = 0;
                            //}
                            //else
                            //{
                            //    CurrentPlaylistRow = i;
                            //}
                        }
                    }

                    dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[1];
                    //dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = false;
                    dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Selected = true;
                    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));

                    CurrentRow = 0;
                }
                else
                {
                    IsDrop_Song = false;
                }
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer2.URL = MusicFileName;
                    musicPlayer2.settings.volume = 0;

                    //if (CurrentRow == dgPlaylist.Rows.Count - 1)
                    //{
                    //    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                    //}
                    //else
                    //{
                    //    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                    //}

                    timer2.Enabled = true;
                    DisplaySongPlayerTwo();
                    return;
                }


            }
            //if (chkShuffleSong.Checked == true)
            //{
            //    CurrentRow = CurrentRow + 3;
            //}
            //else
            //{
            if (CurrentRow >= dgPlaylist.Rows.Count)
            {
                CurrentRow = 0;
            }
            else
            {
                CurrentRow = CurrentRow + 1;
            }
            //}


            if (CurrentRow == dgPlaylist.Rows.Count)
            {
                CurrentRow = dgPlaylist.Rows.Count - 1;
                goto gg;
            }
            TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
            MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
            if (System.IO.File.Exists(TempMusicFileName))
            {
                DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                musicPlayer2.URL = MusicFileName;
                musicPlayer2.settings.volume = 0;

                //if (CurrentRow == dgPlaylist.Rows.Count - 1)
                //{
                //    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                //}
                //else
                //{
                //    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                //}

                timer2.Enabled = true;
                DisplaySongPlayerTwo();
                return;
            }
            for (int i = Convert.ToInt16(CurrentRow); i < dgPlaylist.Rows.Count; i++)
            {
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value));
                    musicPlayer2.URL = MusicFileName;
                    musicPlayer2.settings.volume = 0;

                    //if (i == dgPlaylist.Rows.Count - 1)
                    //{
                    //    NextSongDisplay2(dgPlaylist.Rows[0].Cells[0].Value.ToString());
                    //}
                    //else
                    //{
                    //    NextSongDisplay2(dgPlaylist.Rows[i + 1].Cells[0].Value.ToString());
                    //}

                    timer2.Enabled = true;

                    //if (chkShuffleSong.Checked == true)
                    //{
                    //    if (CurrentRow == 0)
                    //    {
                    //        CurrentRow = i + 2;
                    //    }
                    //    else if (CurrentRow == 1)
                    //    {
                    //        CurrentRow = i + 4;
                    //    }
                    //    else
                    //    {
                    //        CurrentRow = i - 1;
                    //    }
                    //}
                    //else
                    //{
                    CurrentRow = i;
                    // }

                    timer2.Enabled = true;
                    DisplaySongPlayerTwo();
                    return;
                }

            }
        }
        private void dgLocalPlaylist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex == 1 || e.ColumnIndex == 3)
            {
                if (e.RowIndex >= 0)
                {
                    IsLast100Working = "No";
                    StaticClass.Last100PlaylistId = 0;

                    //CurrentPlaylistRow = dgLocalPlaylist.CurrentCell.RowIndex;
                    if (dgLocalPlaylist.Rows[e.RowIndex].Cells[2].Value.ToString() == "Default")
                    {
                        //ColorHere

                        dgLocalPlaylist.Rows[e.RowIndex].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        dgLocalPlaylist.Rows[e.RowIndex].Cells[1].Style.SelectionForeColor = Color.White;

                        dgLocalPlaylist.Rows[e.RowIndex].Cells[3].Style.SelectionBackColor = Color.LightBlue;


                        dgPlaylist.Visible = true;
                        dgPlaylist.Dock = DockStyle.Fill;
                        dgOtherPlaylist.Visible = false;
                        PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[e.RowIndex].Cells[0].Value));
                        DownloadSong(dgPlaylist);
                    }
                    else
                    {
                        dgLocalPlaylist.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.FromArgb(0, 0, 0);
                        dgLocalPlaylist.Rows[e.RowIndex].Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);

                        dgLocalPlaylist.Rows[e.RowIndex].Cells[3].Style.SelectionBackColor = Color.White;

                        dgOtherPlaylist.Visible = true;
                        dgOtherPlaylist.Dock = DockStyle.Fill;
                        dgPlaylist.Visible = false;
                        PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[e.RowIndex].Cells[0].Value));
                        DownloadSong(dgOtherPlaylist);
                    }
                    //Song_Set_foucs();
                    
                }
            }
            //if (e.ColumnIndex == 2)
            //{
            //    txtPlaylistName.Text = dgLocalPlaylist.Rows[e.RowIndex].Cells[1].Value.ToString();
            //    ModifyPlaylistId = Convert.ToInt32(dgLocalPlaylist.Rows[e.RowIndex].Cells[0].Value);
            //    pAction = "Modify";
            //    txtPlaylistName.Focus();
            //}
        }


        private void dgPlaylist_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex == -1)
            {
                return;
            }
            StopDuplicate = "Yes";
            //int rowindex = dgPlaylist.CurrentCell.RowIndex;
            //int columnindex = dgPlaylist.CurrentCell.ColumnIndex;
            //string localfilename;
            //try
            //{
            //    localfilename = dgPlaylist.Rows[rowindex].Cells[0].Value.ToString() + ".mp3";
            //    string localfilePath = Application.StartupPath + "\\" + localfilename;

            //    if (columnindex == 2)
            //    {

            //        if (System.IO.File.Exists(localfilePath))
            //        {
            //            musicPlayer1.URL = localfilePath;
            //            musicPlayer1.settings.volume = 70;
            //            musicPlayer2.URL = "";
            //            musicPlayer2.Ctlcontrols.stop();
            //            CurrentRow = rowindex;

            //            if (CurrentRow == dgPlaylist.Rows.Count - 1)
            //            {
            //                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
            //            }
            //            else
            //            {
            //                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
            //            }
            //            return;
            //        }
            //    }

            //    if (columnindex == 3)
            //    {

            //        if (StaticClass.isDownload != "1" || StaticClass.isRemove != "1")
            //        {
            //            MessageBox.Show(ObjMainClass.MainMessage, "Copyright Player");
            //            return;
            //        }
            //        if (musicPlayer1.URL != "")
            //        {
            //            if (dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() == musicPlayer1.currentMedia.name.ToString())
            //            {
            //                MessageBox.Show("It is not possible to delete the current song.", "Copyright Player");
            //                return;
            //            }
            //        }
            //        if (musicPlayer2.URL != "")
            //        {
            //            if (dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() == musicPlayer2.currentMedia.name.ToString())
            //            {
            //                MessageBox.Show("It is not possible to delete the current song.", "Copyright Player");
            //                return;
            //            }
            //        }

            //        if (Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) != 0)
            //        {
            //            StaticClass.constr.Open();
            //            SqlCommand cmd = new SqlCommand();
            //            cmd.Connection = StaticClass.constr;
            //            cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgPlaylist.Rows[rowindex].Cells[0].Value;
            //            cmd.ExecuteNonQuery();
            //            StaticClass.constr.Close();

            //            delete_temp_data(dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());

            //           // System.IO.File.Delete(localfilePath);
            //            if (chkShuffleSong.Checked == true)
            //            {
            //                PopulateShuffleSong(Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value), ShuffleCount);
            //            }
            //            else
            //            {
            //                PopulateInputFileTypeDetail(Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
            //            }
            //           // DownloadSong();

            //            //if (musicPlayer1.URL != "")
            //            //{
            //            //    Song_Set_foucs();
            //            //}
            //            //if (musicPlayer2.URL != "")
            //            //{
            //            //    Song_Set_foucs2();
            //            //}

            //            if (CurrentRow == dgPlaylist.Rows.Count - 1)
            //            {
            //                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
            //            }
            //            else
            //            {
            //                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
            //            }

            //        }

            //        else
            //        {
            //            MessageBox.Show("Please select a playlist.", "Copyright Player");
            //        }
            //    }
            //}
            //catch {}
        }
        private void Music_Player_Settings()
        {
            string str = "";
            string Song_Name = "";
            string GetName = "";
            DataTable dtDetail;
            string mlsSql = "";
            if (dgLocalPlaylist.Rows.Count == 0) return;
            if (dgPlaylist.Rows.Count == 0) return;
            double LastSongDuration = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
            if (StaticClass.constr.State == ConnectionState.Open)
            {
                StaticClass.constr.Close();
            }
            str = "delete from tblmusic_player_settings where DFClientId=" + StaticClass.UserId + " and localUserId= " + StaticClass.LocalUserId + " and tokenNo=" + StaticClass.TokenId;
            StaticClass.constr.Open();
            SqlCommand cmdDel = new SqlCommand();
            cmdDel.Connection = StaticClass.constr;
            cmdDel.CommandText = str;
            cmdDel.ExecuteNonQuery();
            StaticClass.constr.Close();

            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("Insert_music_player_settings", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@DFClientId", SqlDbType.BigInt));
            cmd.Parameters["@DFClientId"].Value = Convert.ToInt32(StaticClass.UserId);

            cmd.Parameters.Add(new SqlParameter("@localUserId", SqlDbType.BigInt));
            cmd.Parameters["@localUserId"].Value = Convert.ToInt32(StaticClass.LocalUserId); ;

            cmd.Parameters.Add(new SqlParameter("@lastPlaylistId", SqlDbType.BigInt));

            if (musicPlayer1.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                cmd.Parameters.Add(new SqlParameter("@lastTileId", SqlDbType.BigInt));
                cmd.Parameters["@lastTileId"].Value = Convert.ToInt32(g);
                Song_Name = g.ToString();
            }
            else if (musicPlayer2.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                cmd.Parameters.Add(new SqlParameter("@lastTileId", SqlDbType.BigInt));
                cmd.Parameters["@lastTileId"].Value = Convert.ToInt32(g);
                Song_Name = g.ToString();
            }


            for (int i = 0; i < dgLocalPlaylist.Rows.Count; i++)
            {
                mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value);
                dtDetail = ObjMainClass.fnFillDataTable(mlsSql);
                if ((dtDetail.Rows.Count > 0))
                {
                    for (int iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {
                        if (dtDetail.Rows[iCtr]["TitleID"].ToString() == Song_Name)
                        {
                            cmd.Parameters["@lastPlaylistId"].Value = Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value);
                            GetName = "Yes";
                            break;
                        }
                    }
                }
                if (GetName == "Yes")
                {
                    break;
                }
            }



            cmd.Parameters.Add(new SqlParameter("@lastVolume", SqlDbType.Int));
            cmd.Parameters["@lastVolume"].Value = Convert.ToInt16(musicPlayer1.settings.volume);

            cmd.Parameters.Add(new SqlParameter("@lastSongDuration", SqlDbType.Int));
            cmd.Parameters["@lastSongDuration"].Value = Convert.ToInt16(LastSongDuration);

            cmd.Parameters.Add(new SqlParameter("@IsFade", SqlDbType.Int));
            cmd.Parameters["@IsFade"].Value = 0;

            cmd.Parameters.Add(new SqlParameter("@IsShuffle", SqlDbType.Int));
            cmd.Parameters["@IsShuffle"].Value = 0;

            cmd.Parameters.Add(new SqlParameter("@TokenNo", SqlDbType.BigInt));
            cmd.Parameters["@TokenNo"].Value = StaticClass.TokenId;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Application.Exit();
                return;
            }
            finally
            {
                StaticClass.constr.Close();
            }
        }
        private void Get_Last_Settings()
        {
            string tempSongName = "";
            try
            {
                string str = "";
                string SongName = "";
                str = "select * from tblmusic_player_settings where DFClientId=" + StaticClass.UserId + " and localUserId=" + StaticClass.LocalUserId + " and tokenNo=" + StaticClass.TokenId;
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet(str);

                //if (Convert.ToInt32(ds.Tables[0].Rows[0]["lastPlaylistId"])== Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value))
                //PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(ds.Tables[0].Rows[0]["lastPlaylistId"]));

                for (int i = 0; i < dgLocalPlaylist.Rows.Count; i++)
                {
                    if (dgLocalPlaylist.Rows[i].Cells[2].Value.ToString() == "Default")
                    {
                        //dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[i].Cells[1];
                        CurrentPlaylistRow = i;
                        break;
                    }
                }

                //for (int i = 0; i < dgLocalPlaylist.Rows.Count; i++)
                //{
                //    if (dgLocalPlaylist.Rows[i].Cells[0].Value.ToString() == ds.Tables[0].Rows[0]["lastPlaylistId"].ToString())
                //    {
                //        dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[i].Cells[1];
                //        CurrentPlaylistRow = i;
                //    }
                //}

                int tempRow = 0;
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {

                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == ds.Tables[0].Rows[0]["lastTileId"].ToString())
                    {
                        tempRow = i + 1;
                        if (tempRow >= dgPlaylist.Rows.Count)
                        {
                            tempRow = 0;
                        }
                        tempSongName = Application.StartupPath + "\\" + dgPlaylist.Rows[tempRow].Cells[0].Value.ToString() + ".sec";
                        if (System.IO.File.Exists(tempSongName))
                        {
                            SongName = Application.StartupPath + "\\" + dgPlaylist.Rows[tempRow].Cells[0].Value.ToString() + ".mp3";
                            DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[tempRow].Cells[0].Value));
                            musicPlayer1.URL = SongName;

                            CurrentRow = tempRow;


                            dgPlaylist.CurrentCell = dgPlaylist.Rows[i].Cells[1];
                            if (CurrentRow == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                            }

                        }
                    }
                }
                musicPlayer1.settings.volume = 100;
                if (musicPlayer1.URL == "")
                {
                    PlaySongDefault();
                }
                //            musicPlayer1.settings.volume = Convert.ToInt16(ds.Tables[0].Rows[0]["lastVolume"]);




                //if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsShuffle"]) == 1)
                //{
                //    chkShuffleSong.Checked = true;
                //   PopulateShuffleSong(dgPlaylist, Convert.ToInt32(ds.Tables[0].Rows[0]["lastPlaylistId"]), ShuffleCount);
                //}
                //else
                //{
                //    chkShuffleSong.Checked = false;
                //    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(ds.Tables[0].Rows[0]["lastPlaylistId"]));
                //}




                //            musicPlayer1.Ctlcontrols.currentPosition = Convert.ToInt16(ds.Tables[0].Rows[0]["lastSongDuration"]);
                //          timer1.Enabled = true;

            }
            catch { }
        }
        private void Song_Set_foucs()
        {
            try
            {

                drawLine = false;
                dgPlaylist.Invalidate();
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                    {
                        CurrentRow = i;

                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            IsVisibleSong = true;
                            UpdateHideSong(g.ToString());
                        }
                        else
                        {
                            IsVisibleSong = false;
                            IsSongDropAdvt = false;
                            dgPlaylist.CurrentCell = dgPlaylist.Rows[i].Cells[1];
                            dgPlaylist.Rows[i].Selected = true;

                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionBackColor = Color.FromArgb(20, 162, 175);
                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.White;
                        }
                        lblSongName.ForeColor = Color.Yellow;
                        lblArtistName.ForeColor = Color.Yellow;
                        lblMusicTimeOne.ForeColor = Color.Yellow;
                        lblSongDurationOne.ForeColor = Color.Yellow;
                        pbarMusic1.ForeColor = Color.Yellow;
                        panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));
                        pbarMusic1.BackColor = Color.FromArgb(9, 130, 154);

                        lblSongName2.ForeColor = Color.Gray;
                        lblArtistName2.ForeColor = Color.Gray;
                        lblMusicTimeTwo.ForeColor = Color.Gray;
                        lblSongDurationTwo.ForeColor = Color.Gray;
                        pbarMusic2.ForeColor = Color.Gray;
                        pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
                        panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                        if (dgHideSongs.Rows.Count > 0)
                        {
                            DeleteParticularHideSong();
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void Song_Set_foucs2()
        {
            try
            {

                drawLine = false;
                dgPlaylist.Invalidate();
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                    {
                        CurrentRow = i;
                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            IsVisibleSong = true;
                            UpdateHideSong(g.ToString());
                        }
                        else
                        {
                            IsVisibleSong = false;
                            IsSongDropAdvt = false;
                            dgPlaylist.CurrentCell = dgPlaylist.Rows[i].Cells[1];
                            dgPlaylist.Rows[i].Selected = true;
                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionBackColor = Color.FromArgb(20, 162, 175);
                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.White;
                        }

                        lblSongName2.ForeColor = Color.Yellow;
                        lblArtistName2.ForeColor = Color.Yellow;
                        lblMusicTimeTwo.ForeColor = Color.Yellow;
                        lblSongDurationTwo.ForeColor = Color.Yellow;
                        pbarMusic2.ForeColor = Color.Yellow;
                        pbarMusic2.BackColor = Color.FromArgb(9, 130, 154);
                        panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));

                        lblSongName.ForeColor = Color.Gray;
                        lblArtistName.ForeColor = Color.Gray;
                        lblMusicTimeOne.ForeColor = Color.Gray;
                        lblSongDurationOne.ForeColor = Color.Gray;
                        pbarMusic1.ForeColor = Color.Gray;
                        pbarMusic1.BackColor = Color.FromArgb(175, 175, 175);
                        panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                        if (dgHideSongs.Rows.Count > 0)
                        {
                            DeleteParticularHideSong();
                        }

                        return;
                    }
                }
            }
            catch { }
        }








        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            string str = "";

            if (e.KeyCode == Keys.Return)
            {
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                    return;
                }
                if (txtSearch.Text == "")
                {
                    str = "SELECT TOP (200) Titles.TitleID, ltrim(Titles.Title) as Title, Titles.Time, ltrim(Artists.Name) as ArtistName, ltrim(Albums.Name) AS AlbumName FROM Titles INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID where titlecategoryid=4 order by TitleID desc";
                    FillGrid(str);
                    if (dgPlaylist.Visible == true)
                    {
                        DownloadSong(dgPlaylist);
                    }
                    else
                    {
                        DownloadSong(dgOtherPlaylist);
                    }
                    
                    return;

                }
                if (txtSearch.Text.Length < 2)
                {
                    MessageBox.Show("Enter minimum 2 characters for a search function.", "Copyright Player");
                    return;
                }
                SearchText = txtSearch.Text.Replace("\r\n", string.Empty);


                CommanSearch();
                //stSearch = "spOverallSearch'" + txtSearch.Text + "',300";
                //FillGrid(stSearch);
                txtSearch.TextAlign = HorizontalAlignment.Left;
                txtSearch.ForeColor = Color.White;
                txtSearch.Text = "";
                if (dgPlaylist.Visible == true)
                {
                    DownloadSong(dgPlaylist);
                }
                else
                {
                    DownloadSong(dgOtherPlaylist);
                }
                // picLoading.Visible = false;

            }

        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search")
            {
                txtSearch.Text = "";
            }
            txtSearch.TextAlign = HorizontalAlignment.Left;
            txtSearch.ForeColor = Color.White;
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                txtSearch.Text = "Search";
                txtSearch.TextAlign = HorizontalAlignment.Center;
                txtSearch.ForeColor = Color.White;
            }

        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 39 || Convert.ToInt32(e.KeyChar) == 37)
            {
                e.Handled = true;
                return;
            }
            // MessageBox.Show(Convert.ToInt32(e.KeyChar).ToString());

        }



        private void btnFade_Click(object sender, EventArgs e)
        {
            try
            {
                if (musicPlayer1.URL == "" && musicPlayer2.URL == "" && dgPlaylist.Rows.Count == 0)
                {
                    MessageBox.Show("Drag & drop the song in the player.", "Copyright Player");
                    return;
                }
                drawLine = false;
                dgPlaylist.Invalidate();

                picFade.Location = new Point(6, 45);
                picFade.Visible = true;
                btnFade.Visible = false;

                if (musicPlayer1.URL == "")
                {
                    IsbtnClick = "Y";
                    panPlayerButton.Enabled = true;
                    panComman.Enabled = false;
                    panel8.Enabled = false;
                    panPlaylist.Enabled = false;
                    PlaylistFadeSongPlayerOne();

                    timAutoFadePlayerOne.Enabled = false;
                    timAutoFadePlayerTwo.Enabled = false;
                    timer5.Enabled = true;
                    return;
                }
                if (musicPlayer2.URL == "")
                {
                    IsbtnClick = "Y";
                    panPlayerButton.Enabled = true;
                    panComman.Enabled = false;
                    panel8.Enabled = false;
                    panPlaylist.Enabled = false;
                    PlaylistFadeSong();

                    timAutoFadePlayerOne.Enabled = false;
                    timAutoFadePlayerTwo.Enabled = false;
                    timer4.Enabled = true;
                    return;

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                panPlayerButton.Enabled = true;
                panComman.Enabled = true;
                panel8.Enabled = true;
                panPlaylist.Enabled = true;
            }
        }
        private void PlaylistFadeSongPlayerOne()
        {
            string MusicFileName = "";
            string TempMusicFileName = "";
            string mlsSql = "";
            string FindSong = "";
            DataTable dtDetail;

            GetOldSongIdPlayer2();
            if (CurrentRow >= dgPlaylist.Rows.Count - 1)
            {

                CurrentRow = LastRowId;
                //if (LastRowId == dgPlaylist.Rows.Count - 1)
                //{
                //    CurrentRow = 0;
                //}
                //else
                //{
                //    CurrentRow = LastRowId;
                //}
            }
            if (dgPlaylist.Rows.Count == 0)
            {
                IsLast100Working = "No";
            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
            //{
            //    CurrentPlaylistRow = 0;
            //}
            //else
            //{
            //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
            //}
            GHTE:
                for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count > 0))
                    {
                        // CurrentPlaylistRow = i;
                        FindSong = "True";
                        break;
                    }
                    else
                    {
                        FindSong = "false";
                        //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                        //{
                        //    CurrentPlaylistRow = 0;
                        //}
                        //else
                        //{
                        //    CurrentPlaylistRow = i;
                        //}
                    }
                }
                if (FindSong == "false")
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count == 0))
                    {
                        // CurrentPlaylistRow = 0;
                        goto GHTE;
                    }
                }
                //  dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];
                //   dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;
                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));

                CurrentRow = 0;
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer1.URL = MusicFileName;
                    musicPlayer1.settings.volume = 0;

                    //if (CurrentRow == dgPlaylist.Rows.Count - 1)
                    //{
                    //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                    //}
                    //else
                    //{
                    //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                    //}

                    timer1.Enabled = true;
                    DisplaySongPlayerOne();
                    return;
                }
            }



            if (dgPlaylist.Rows.Count - 1 == 0)
            {
                IsLast100Working = "No";
            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
            //{
            //    CurrentPlaylistRow = 0;

            //}
            //else
            //{
            //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
            //}
            GHT:
                for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count > 0))
                    {
                        //  CurrentPlaylistRow = i;
                        FindSong = "True";
                        break;
                    }
                    else
                    {
                        FindSong = "false";
                        //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                        //{
                        //    CurrentPlaylistRow = 0;
                        //}
                        //else
                        //{
                        //    CurrentPlaylistRow = i;
                        //}
                    }
                }
                if (FindSong == "false")
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count == 0))
                    {
                        //  CurrentPlaylistRow = 0;
                        goto GHT;
                    }
                }

                //dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];
                //dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;
                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));

                CurrentRow = 0;
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer1.URL = MusicFileName;
                    musicPlayer1.settings.volume = 0;

                    //if (CurrentRow == dgPlaylist.Rows.Count - 1)
                    //{
                    //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                    //}
                    //else
                    //{
                    //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                    //}

                    timer1.Enabled = true;
                    DisplaySongPlayerOne();
                    return;
                }
            }


        gg:
            if (CurrentRow == dgPlaylist.Rows.Count - 1)
            {
                if (IsDrop_Song == false)
                {
                    //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                    //{
                    //    CurrentPlaylistRow = 0;

                    //}
                    //else
                    //{
                    //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
                    //}

                    for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                    {
                        mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                        dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                        if ((dtDetail.Rows.Count > 0))
                        {
                            //   CurrentPlaylistRow = i;
                            break;
                        }
                        else
                        {
                            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                            //{
                            //    CurrentPlaylistRow = 0;
                            //}
                            //else
                            //{
                            //    CurrentPlaylistRow = i;
                            //}
                        }
                    }

                    //dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];
                    // dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;

                    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));
                    CurrentRow = 0;
                }
                else
                {
                    IsDrop_Song = false;
                }
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer1.URL = MusicFileName;
                    musicPlayer1.settings.volume = 0;

                    //if (CurrentRow == dgPlaylist.Rows.Count - 1)
                    //{
                    //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                    //}
                    //else
                    //{
                    //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                    //}

                    timer1.Enabled = true;
                    DisplaySongPlayerOne();
                    return;
                }


            }
            //if (chkShuffleSong.Checked == true)
            //{
            //    if (CurrentRow == 0)
            //    {
            //        CurrentRow = CurrentRow + 1;
            //    }
            //    else
            //    {
            //        CurrentRow = CurrentRow - 2;
            //    }
            //}
            //else
            //{
            if (CurrentRow >= dgPlaylist.Rows.Count)
            {
                CurrentRow = 0;
            }
            else
            {
                CurrentRow = CurrentRow + 1;
            }
            // }
            if (CurrentRow == dgPlaylist.Rows.Count)
            {
                CurrentRow = dgPlaylist.Rows.Count - 1;
                goto gg;
            }

            TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
            MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
            if (System.IO.File.Exists(TempMusicFileName))
            {
                DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                musicPlayer1.URL = MusicFileName;
                musicPlayer1.settings.volume = 0;

                //if (CurrentRow == dgPlaylist.Rows.Count - 1)
                //{
                //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                //}
                //else
                //{
                //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                //}

                timer1.Enabled = true;
                DisplaySongPlayerOne();
                return;
            }
            for (int i = Convert.ToInt16(CurrentRow); i < dgPlaylist.Rows.Count; i++)
            {
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value));
                    musicPlayer1.URL = MusicFileName;
                    musicPlayer1.settings.volume = 0;

                    //if (i == dgPlaylist.Rows.Count - 1)
                    //{
                    //    NextSongDisplay(dgPlaylist.Rows[0].Cells[0].Value.ToString());
                    //}
                    //else
                    //{
                    //    NextSongDisplay(dgPlaylist.Rows[i + 1].Cells[0].Value.ToString());
                    //}

                    timer1.Enabled = true;

                    //if (chkShuffleSong.Checked == true)
                    //{
                    //    CurrentRow = i + 2;
                    //}
                    //else
                    //{
                    CurrentRow = i;
                    //}

                    timer1.Enabled = true;
                    DisplaySongPlayerOne();
                    return;
                }

            }
        }


        private void NextSongDisplay2(string NextSongId)
        {
            try
            {
                string mlsSql;
                var Special_Name = "";
                string Special_Change = "";
                mlsSql = "SELECT  Titles.Title as songname, Albums.Name as AlbumsName, Artists.Name AS ArtistsName, Titles.Time FROM ( Albums INNER JOIN Titles ON Albums.AlbumID = Titles.AlbumID ) INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID where Titles.titleid=" + Convert.ToInt32(NextSongId);
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet_Local(mlsSql);

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["songname"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblSongName.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["ArtistsName"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblArtistName.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["AlbumsName"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                //lblalbumName.Text = Special_Change;
                UpcomingSongPlayerOne = NextSongId;
                UpcomingSongPlayerTwo = "";

                string str = ds.Tables[0].Rows[0]["Time"].ToString();
                string[] arr = str.Split(':');
                DropSongLength = arr[1] + ":" + arr[2];
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            try
            {
                double a = Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);
                lblCurrentTiming.Text = a.ToString();
                double t = Math.Floor(musicPlayer2.currentMedia.duration - musicPlayer2.Ctlcontrols.currentPosition);
                lblMusic2Timeremaing.Text = (t.ToString());
                PlayFadeSong();
            }
            catch
            {
            }
        }

        private void PlayFadeSong()
        {

            if (lblCurrentTiming.Text == "1")
            {
                TimePlayerOne = TimePlayerOne + Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.mute = false;
                    musicPlayer1.settings.mute = false;
                    musicPlayer1.settings.volume = 75;
                }
                else if (btnMute.Text == ".")
                {
                    musicPlayer2.settings.mute = true;
                    musicPlayer1.settings.mute = true;
                }


                if (lblSongCount.Text == "2")
                {
                    timGetRemainAdvtTime.Enabled = false;
                    lblAdvtTimeRemain.Text = "00:10";
                    musicPlayer2.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 25;
                }
            }

            else if (lblCurrentTiming.Text == "2")
            {
                int musicVolume;
                musicVolume = musicPlayer1.settings.volume;
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:07";
                    musicPlayer2.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 50;
                }
                if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 50;
                }
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                GetSavedRating(g, dgSongRatingPlayerTwo);
                SetDisableRating(dgSongRatingPlayerOne);
                Song_Set_foucs2();
            }

            else if (lblCurrentTiming.Text == "4")
            {
                int musicVolume;
                musicVolume = musicPlayer1.settings.volume;
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:05";
                    musicPlayer2.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 75;
                }
                if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 25;
                }
            }

            else if (lblCurrentTiming.Text == "6")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:02";
                    musicPlayer2.settings.volume = 0;
                }
                else
                {
                    if (btnMute.Text == "")
                    {
                        musicPlayer2.settings.volume = 85;
                    }
                    if (IsVisibleSong == true)
                    {
                        if (LastRowId == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }
                    }
                    else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                    {

                        if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }

                    }
                    else
                    {
                        if (CurrentRow == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                        }
                    }
                    panPlayerButton.Enabled = true;
                    panComman.Enabled = true;
                    panel8.Enabled = true;
                    panPlaylist.Enabled = true;
                }
                if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 15;
                }
            }

            else if (lblCurrentTiming.Text == "8")
            {
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;


                musicPlayer1.settings.volume = 0;
                musicPlayer1.Ctlcontrols.stop();
                musicPlayer1.URL = "";
                SaveLast100();
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    musicPlayer2.settings.volume = 0;
                    lblPlayerName.Text = "One";
                    musicPlayer1.Ctlcontrols.pause();
                    musicPlayer2.Ctlcontrols.pause();
                    panAdvt.Height = 125;
                    panAdvt.Visible = true;
                    if (panStreamShedule.Visible == false & panAdvtDetail.Visible == false & panAllStreamShedule.Visible == false & panManual.Visible == false & panPrayer.Visible == false)
                    {
                        panAdvt.BringToFront();
                    }
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 100;
                }
                panPlayerButton.Enabled = true;
                panComman.Enabled = true;
                panel8.Enabled = true;
                panPlaylist.Enabled = true;
                IsbtnClick = "N";


                lblMusicTimeOne.Text = "00:00";
                lblSongDurationOne.Text = "00:00";

            }
            else if (lblCurrentTiming.Text == "10")
            {
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;


                musicPlayer1.settings.volume = 0;
                musicPlayer1.Ctlcontrols.stop();
                musicPlayer1.URL = "";
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 100;
                }
                panPlayerButton.Enabled = true;
                panComman.Enabled = true;
                panel8.Enabled = true;
                panPlaylist.Enabled = true;
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");
            }
        }

        private void dgPlaylist_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                drawLine = false;

                RowDeselect(dgPlaylist);
                dgPlaylist.Rows[e.RowIndex].Selected = true;
                dgPlaylist.DoDragDrop(dgPlaylist.Rows[e.RowIndex].Cells[0].Value.ToString(), DragDropEffects.Copy);
            }


        }

        private void GetNextSong(string RunningPlayer)
        {
            string currentFileName;
            if (RunningPlayer == "1")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                currentFileName = g;
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (currentFileName == dgPlaylist.Rows[i].Cells[0].Value.ToString())
                    {
                        CurrentRow = i;
                        if (CurrentRow == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                        }
                        return;
                    }
                }
            }
            else if (RunningPlayer == "2")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                currentFileName = g;
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (currentFileName == dgPlaylist.Rows[i].Cells[0].Value.ToString())
                    {
                        CurrentRow = i;
                        if (CurrentRow == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                        }
                        return;
                    }
                }

            }

        }




        private void Song_Clear_foucs()
        {
            try
            {
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    dgPlaylist.Rows[i].Cells[1].Style.SelectionBackColor = Color.White;
                    dgPlaylist.Rows[i].Cells[1].Style.SelectionForeColor = Color.Black;

                    dgPlaylist.Rows[i].Cells[2].Style.SelectionBackColor = Color.White;
                    dgPlaylist.Rows[i].Cells[2].Style.SelectionForeColor = Color.Black;

                    dgPlaylist.Rows[i].Cells[3].Style.SelectionBackColor = Color.White;
                    dgPlaylist.Rows[i].Cells[3].Style.SelectionForeColor = Color.Black;

                }
            }
            catch
            {
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            try
            {
                double a = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
                lblCurrentTimingPlayerOne.Text = a.ToString();
                double t = Math.Floor(musicPlayer1.currentMedia.duration - musicPlayer1.Ctlcontrols.currentPosition);
                lblMusic1Timeremaing.Text = (t.ToString());
                PlayFadeSongPlayerOne();
            }
            catch { }
        }
        private void PlayFadeSongPlayerOne()
        {
            if (lblCurrentTimingPlayerOne.Text == "1")
            {
                TimePlayerTwo = TimePlayerTwo + Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);

                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.mute = false;
                    musicPlayer1.settings.mute = false;
                    musicPlayer2.settings.volume = 75;
                }
                else if (btnMute.Text == ".")
                {
                    musicPlayer2.settings.mute = true;
                    musicPlayer1.settings.mute = true;
                }

                if (lblSongCount.Text == "2")
                {
                    timGetRemainAdvtTime.Enabled = false;
                    lblAdvtTimeRemain.Text = "00:10";
                    musicPlayer1.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 25;
                }
            }

            else if (lblCurrentTimingPlayerOne.Text == "2")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:07";
                    musicPlayer1.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 50;
                }
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 50;
                }
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                GetSavedRating(g, dgSongRatingPlayerOne);
                SetDisableRating(dgSongRatingPlayerTwo);
                Song_Set_foucs();
            }

            else if (lblCurrentTimingPlayerOne.Text == "4")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:05";
                    musicPlayer1.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 75;
                }
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 25;
                }
            }

            else if (lblCurrentTimingPlayerOne.Text == "6")
            {
                int musicVolume;
                musicVolume = musicPlayer2.settings.volume;
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:02";
                    musicPlayer1.settings.volume = 0;
                }
                else
                {
                    if (btnMute.Text == "")
                    {
                        musicPlayer1.settings.volume = 85;
                    }
                    if (IsVisibleSong == true)
                    {
                        if (LastRowId == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }
                    }
                    else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                    {

                        if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }

                    }

                    else
                    {
                        if (CurrentRow == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                        }
                    }
                    panPlayerButton.Enabled = true;
                    panComman.Enabled = true;
                    panel8.Enabled = true;
                    panPlaylist.Enabled = true;
                }
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 15;
                }
            }

            else if (lblCurrentTimingPlayerOne.Text == "8")
            {
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;


                musicPlayer2.settings.volume = 0;
                musicPlayer2.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                SaveLast100();
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    panAdvt.Height = 125;

                    musicPlayer1.settings.volume = 0;
                    lblPlayerName.Text = "Two";
                    musicPlayer2.Ctlcontrols.pause();
                    musicPlayer1.Ctlcontrols.pause();
                    panAdvt.Visible = true;
                    if (panStreamShedule.Visible == false & panAdvtDetail.Visible == false & panAllStreamShedule.Visible == false & panManual.Visible == false & panPrayer.Visible == false)
                    {
                        panAdvt.BringToFront();
                    }
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 100;
                }
                IsbtnClick = "N";
                panPlayerButton.Enabled = true;
                panComman.Enabled = true;
                panel8.Enabled = true;
                panPlaylist.Enabled = true;
                lblMusicTimeTwo.Text = "00:00";
                lblSongDurationTwo.Text = "00:00";



            }
            else if (lblCurrentTimingPlayerOne.Text == "10")
            {
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;


                musicPlayer2.settings.volume = 0;
                musicPlayer2.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 100;
                }
                panPlayerButton.Enabled = true;
                panComman.Enabled = true;
                panel8.Enabled = true;
                panPlaylist.Enabled = true;
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");
            }

        }

        private void musicPlayer2_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            try
            {


                TimerEventProcessorPlayerOne();
                //Song_Set_foucs2();


                if (Song_Mute == true)
                {
                    musicPlayer2.settings.mute = true;
                }
                else
                {
                    musicPlayer2.settings.mute = false;
                }

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }




        private void timAutoFadePlayerTwo_Tick(object sender, EventArgs e)
        {
            try
            {
                //drawLine = false;
                //dgPlaylist.Invalidate();
                double t = Math.Floor(musicPlayer1.currentMedia.duration - musicPlayer1.Ctlcontrols.currentPosition);
                lblMusicTimeremaingPlayerOne.Text = (t.ToString());
                TimePlayerOne = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
                PlayAutoFadeSongPlayerTwo();
            }
            catch { }
        }

        private void PlayAutoFadeSongPlayerTwo()
        {
            if ((Convert.ToInt32(lblMusicTimeremaingPlayerOne.Text) <= 60) && (Convert.ToInt32(lblMusicTimeremaingPlayerOne.Text) >= 21))
            {
                if (lblSongCount.Text == "2")
                {
                    timGetRemainAdvtTime.Enabled = false;
                    lblAdvtTimeRemain.Text = "00:" + lblMusicTimeremaingPlayerOne.Text;
                }
            }
            if (lblMusicTimeremaingPlayerOne.Text == "20")
            {
                SaveLast100();
                if (lblSongCount.Text == "2")
                {
                    timGetRemainAdvtTime.Enabled = false;
                    lblAdvtTimeRemain.Text = "00:13";
                }
                // TimePlayerOne = TimePlayerOne + Math.Floor(musicPlayer1.currentMedia.duration);
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "19")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:12";
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "18")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:11";
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "17")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:10";
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "16")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:09";
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "15")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:08";
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "14")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:07";
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "13")
            {
                label1.Text = "Player Two----13";
                panPlayerButton.Enabled = true;
                panComman.Enabled = false;
                panel8.Enabled = false;
                panPlaylist.Enabled = false;

                picFade.Location = new Point(6, 45);
                picFade.Visible = true;
                btnFade.Visible = false;

                PlayAutoFadingSongPlayerTwo();
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.mute = false;
                    musicPlayer1.settings.mute = false;
                    musicPlayer1.settings.volume = 75;
                }
                else if (btnMute.Text == ".")
                {
                    musicPlayer2.settings.mute = true;
                    musicPlayer1.settings.mute = true;
                }
                DisplaySongPlayerTwo();
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:06";
                    musicPlayer2.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 25;
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "12")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:05";
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "11")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:04";
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "10")
            {
                label1.Text = "Player Two----8";
                prvPlayerOneTime = prvPlayerOneTime + Math.Floor(musicPlayer1.currentMedia.duration);
                if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 50;
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:03";
                    timGetRemainAdvtTime.Enabled = false;
                    musicPlayer2.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 50;
                }
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                GetSavedRating(g, dgSongRatingPlayerTwo);
                SetDisableRating(dgSongRatingPlayerOne);
                Song_Set_foucs2();
            }

            else if (lblMusicTimeremaingPlayerOne.Text == "8")
            {
                label1.Text = "Player Two----6";
                if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 25;
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:02";
                    timGetRemainAdvtTime.Enabled = false;
                    musicPlayer2.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 75;
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "6")
            {
                label1.Text = "Player Two----6";
                if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 25;
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    timGetRemainAdvtTime.Enabled = false;
                    musicPlayer2.settings.volume = 0;
                }
                else
                {
                    if (btnMute.Text == "")
                    {
                        musicPlayer2.settings.volume = 85;
                    }
                    if (IsVisibleSong == true)
                    {
                        if (LastRowId == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }
                    }
                    else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                    {
                        if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }
                    }
                    else
                    {
                        if (CurrentRow == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                        }

                    }
                    panPlayerButton.Enabled = true;
                    panComman.Enabled = true;
                    panel8.Enabled = true;
                    panPlaylist.Enabled = true;
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "5")
            {
                label1.Text = "Player Two----2";

                panPlayerButton.Enabled = true;
                panComman.Enabled = true;
                panel8.Enabled = true;
                panPlaylist.Enabled = true;

                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;


                musicPlayer1.settings.volume = 0;
                musicPlayer1.Ctlcontrols.stop();
                musicPlayer1.URL = "";
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    timGetAdvtTime.Enabled = false;
                    musicPlayer2.settings.volume = 0;
                    lblPlayerName.Text = "One";
                    musicPlayer1.Ctlcontrols.pause();
                    musicPlayer2.Ctlcontrols.pause();
                    panAdvt.Height = 125;
                    panAdvt.Visible = true;
                    if (panStreamShedule.Visible == false & panAdvtDetail.Visible == false & panAllStreamShedule.Visible == false & panManual.Visible == false & panPrayer.Visible == false)
                    {
                        panAdvt.BringToFront();
                    }
                }
                else
                {
                    if (btnMute.Text == "")
                    {
                        musicPlayer2.settings.volume = 100;
                    }
                    g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                    ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");
                }


            }
            else if (lblMusicTimeremaingPlayerOne.Text == "2")
            {
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;


                label1.Text = "Player Two----2";
                panPlayerButton.Enabled = true;
                panComman.Enabled = true;
                panel8.Enabled = true;
                panPlaylist.Enabled = true;
                musicPlayer1.settings.volume = 0;
                musicPlayer1.Ctlcontrols.stop();
                musicPlayer1.URL = "";
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 100;
                }
                Song_Set_foucs2();
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");

            }
        }
        private void PlayAutoFadingSongPlayerTwo()
        {

            string MusicFileName = "";
            string TempMusicFileName = "";
            string mlsSql = "";
            string FindSong = "";
            DataTable dtDetail;
            GetOldSongIdPlayer1();
            if (CurrentRow >= dgPlaylist.Rows.Count - 1)
            {
                CurrentRow = LastRowId;
            }
            if (dgPlaylist.Rows.Count == 0)
            {
            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
            //{
            //    CurrentPlaylistRow = 0;
            //}
            //else
            //{
            //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
            //}
            GHTE:
                for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count > 0))
                    {
                        FindSong = "True";
                        //  CurrentPlaylistRow = i;
                        break;

                    }
                    else
                    {
                        FindSong = "false";
                        //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                        //{
                        //    CurrentPlaylistRow = 0;
                        //}
                        //else
                        //{
                        //    CurrentPlaylistRow = i;
                        //}
                    }
                }
                if (FindSong == "false")
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count == 0))
                    {
                        // CurrentPlaylistRow = 0;
                        goto GHTE;
                    }
                }
                // dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];

                // dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;

                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));

                CurrentRow = 0;
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer2.URL = MusicFileName;
                    musicPlayer2.settings.volume = 0;
                    timer2.Enabled = true;
                    return;
                }
            }



            if (dgPlaylist.Rows.Count - 1 == 0)
            {
            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
            //{
            //    CurrentPlaylistRow = 0;
            //}
            //else
            //{
            //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
            //}
            GHT:
                for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count > 0))
                    {
                        FindSong = "True";
                        //  CurrentPlaylistRow = i;
                        break;

                    }
                    else
                    {
                        FindSong = "false";
                        //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                        //{
                        //    CurrentPlaylistRow = 0;
                        //}
                        //else
                        //{
                        //    CurrentPlaylistRow = i;
                        //}
                    }
                }
                if (FindSong == "false")
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count == 0))
                    {
                        // CurrentPlaylistRow = 0;
                        goto GHT;
                    }
                }
                // dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];

                // dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;

                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));

                CurrentRow = 0;
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer2.URL = MusicFileName;
                    musicPlayer2.settings.volume = 0;
                    timer2.Enabled = true;
                    return;
                }
            }



        gg:
            if (CurrentRow == dgPlaylist.Rows.Count - 1)
            {
                if (IsDrop_Song == false)
                {
                    //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                    //{
                    //    CurrentPlaylistRow = 0;
                    //}
                    //else
                    //{
                    //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
                    //}

                    for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                    {
                        mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                        dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                        if ((dtDetail.Rows.Count > 0))
                        {
                            // CurrentPlaylistRow = i;
                            break;
                        }
                        else
                        {
                            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                            //{
                            //    CurrentPlaylistRow = 0;
                            //}
                            //else
                            //{
                            //    CurrentPlaylistRow = i;
                            //}
                            //return;
                        }
                    }
                    // dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];

                    //  dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;

                    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));
                    CurrentRow = 0;
                }
                else
                {
                    IsDrop_Song = false;
                }
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer2.URL = MusicFileName;
                    musicPlayer2.settings.volume = 0;
                    timer2.Enabled = true;
                    return;
                }


            }
            //if (chkShuffleSong.Checked == true)
            //{
            //    if (CurrentRow == 0)
            //    {
            //        CurrentRow = CurrentRow + 3;
            //    }
            //    else if (CurrentRow == 1)
            //    {
            //        CurrentRow = CurrentRow + 2;
            //    }
            //    else
            //    {
            //        CurrentRow = CurrentRow - 2;
            //    }
            //}
            //else
            //{
            if (CurrentRow >= dgPlaylist.Rows.Count)
            {
                CurrentRow = 0;
            }
            else
            {
                CurrentRow = CurrentRow + 1;
            }
            //}
            if (CurrentRow == dgPlaylist.Rows.Count)
            {
                CurrentRow = dgPlaylist.Rows.Count - 1;
                goto gg;
            }
            TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
            MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
            if (System.IO.File.Exists(TempMusicFileName))
            {
                DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                musicPlayer2.URL = MusicFileName;
                musicPlayer2.settings.volume = 0;

                timer2.Enabled = true;
                return;
            }
            for (int i = Convert.ToInt16(CurrentRow); i < dgPlaylist.Rows.Count; i++)
            {
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value));
                    musicPlayer2.URL = MusicFileName;
                    musicPlayer2.settings.volume = 0;

                    timer2.Enabled = true;

                    //if (chkShuffleSong.Checked == true)
                    //{
                    //    CurrentRow = i + 1;
                    //}
                    //else
                    //{
                    CurrentRow = i;
                    //}

                    timer2.Enabled = true;
                    return;
                }

            }
        }


        private void timAutoFadePlayerOne_Tick(object sender, EventArgs e)
        {

            try
            {
                //drawLine = false;
                //dgPlaylist.Invalidate();
                double t = Math.Floor(musicPlayer2.currentMedia.duration - musicPlayer2.Ctlcontrols.currentPosition);
                lblMusicTimeremaingPlayerTwo.Text = (t.ToString());
                TimePlayerTwo = Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);
                PlayAutoFadeSongPlayerOne();
            }
            catch { }
        }

        private void PlayAutoFadeSongPlayerOne()
        {
            if ((Convert.ToInt32(lblMusicTimeremaingPlayerTwo.Text) <= 60) && (Convert.ToInt32(lblMusicTimeremaingPlayerTwo.Text) >= 21))
            {
                if (lblSongCount.Text == "2")
                {
                    timGetRemainAdvtTime.Enabled = false;
                    lblAdvtTimeRemain.Text = "00:" + lblMusicTimeremaingPlayerTwo.Text;
                }
            }
            if (lblMusicTimeremaingPlayerTwo.Text == "20")
            {
                SaveLast100();
                if (lblSongCount.Text == "2")
                {
                    timGetRemainAdvtTime.Enabled = false;
                    lblAdvtTimeRemain.Text = "00:13";
                }
                //TimePlayerTwo = TimePlayerTwo + Math.Floor(musicPlayer2.currentMedia.duration);
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "19")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:12";
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "18")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:11";
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "17")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:10";
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "16")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:09";
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "15")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:08";
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "14")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:07";
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "13")
            {
                label6.Text = "Player One----12";
                panPlayerButton.Enabled = true;
                panComman.Enabled = false;
                panel8.Enabled = false;
                panPlaylist.Enabled = false;

                picFade.Location = new Point(6, 45);
                picFade.Visible = true;
                btnFade.Visible = false;

                PlayAutoFadingSongPlayerOne();

                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.mute = false;
                    musicPlayer1.settings.mute = false;
                    musicPlayer2.settings.volume = 75;
                }
                else if (btnMute.Text == ".")
                {
                    musicPlayer2.settings.mute = true;
                    musicPlayer1.settings.mute = true;
                }
                DisplaySongPlayerOne();
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:06";
                    musicPlayer1.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 25;
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "12")
            {
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:05";
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "11")
            {

                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:04";
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "10")
            {
                label6.Text = "Player One----8";
                prvPlayerTwoTime = prvPlayerTwoTime + Math.Floor(musicPlayer2.currentMedia.duration);
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 50;
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:03";
                    musicPlayer1.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 50;
                }
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                GetSavedRating(g, dgSongRatingPlayerOne);
                SetDisableRating(dgSongRatingPlayerTwo);
                Song_Set_foucs();
            }

            else if (lblMusicTimeremaingPlayerTwo.Text == "8")
            {
                label6.Text = "Player One----6";
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 25;
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:02";
                    musicPlayer1.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 75;
                }
            }

            else if (lblMusicTimeremaingPlayerTwo.Text == "6")
            {
                label6.Text = "Player One----6";
                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.volume = 25;
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    musicPlayer1.settings.volume = 0;
                }
                else
                {
                    if (btnMute.Text == "")
                    {
                        musicPlayer1.settings.volume = 85;
                    }
                    if (IsVisibleSong == true)
                    {
                        if (LastRowId == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }
                    }
                    else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                    {
                        if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }

                    }
                    else
                    {
                        if (CurrentRow == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                        }

                    }

                    panPlayerButton.Enabled = true;
                    panComman.Enabled = true;
                    panel8.Enabled = true;
                    panPlaylist.Enabled = true;
                }
            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "5")
            {
                label6.Text = "Player One----5";

                panPlayerButton.Enabled = true;
                panComman.Enabled = true;
                panel8.Enabled = true;
                panPlaylist.Enabled = true;

                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;


                musicPlayer2.settings.volume = 0;
                musicPlayer2.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    panAdvt.Height = 125;
                    musicPlayer1.settings.volume = 0;
                    lblPlayerName.Text = "Two";
                    musicPlayer2.Ctlcontrols.pause();
                    musicPlayer1.Ctlcontrols.pause();
                    panAdvt.Visible = true;
                    if (panStreamShedule.Visible == false & panAdvtDetail.Visible == false & panAllStreamShedule.Visible == false & panManual.Visible == false & panPrayer.Visible == false)
                    {
                        panAdvt.BringToFront();
                    }
                }
                else
                {
                    if (btnMute.Text == "")
                    {
                        musicPlayer1.settings.volume = 100;
                    }
                    g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                    ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");
                }
            }

            else if (lblMusicTimeremaingPlayerTwo.Text == "2")
            {

                label6.Text = "Player One----2";
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;

                panPlayerButton.Enabled = true;
                panComman.Enabled = true;
                panel8.Enabled = true;
                panPlaylist.Enabled = true;
                musicPlayer2.settings.volume = 0;
                musicPlayer2.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                if (btnMute.Text == "")
                {
                    musicPlayer1.settings.volume = 100;
                }
                Song_Set_foucs();
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");
            }

        }
        private void PlayAutoFadingSongPlayerOne()
        {
            string MusicFileName = "";
            string TempMusicFileName = "";
            string mlsSql = "";
            string FindSong = "";
            DataTable dtDetail;
            GetOldSongIdPlayer2();
            if (CurrentRow >= dgPlaylist.Rows.Count - 1)
            {
                CurrentRow = LastRowId;
            }
            if (dgPlaylist.Rows.Count == 0)
            {
            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
            //{
            //    CurrentPlaylistRow = 0;

            //}
            //else
            //{
            //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
            //}
            GHTE:
                for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count > 0))
                    {
                        FindSong = "True";
                        //  CurrentPlaylistRow = i;
                        break;
                    }
                    else
                    {
                        FindSong = "false";
                        //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                        //{
                        //    CurrentPlaylistRow = 0;
                        //}
                        //else
                        //{
                        //    CurrentPlaylistRow = i;
                        //}
                    }
                }
                if (FindSong == "false")
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count == 0))
                    {
                        // CurrentPlaylistRow = 0;
                        goto GHTE;
                    }
                }
                //  dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];

                //  dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;
                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));
                CurrentRow = 0;
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer1.URL = MusicFileName;
                    musicPlayer1.settings.volume = 0;
                    timer1.Enabled = true;
                    return;
                }
            }



            if (dgPlaylist.Rows.Count - 1 == 0)
            {
            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
            //{
            //    CurrentPlaylistRow = 0;

            //}
            //else
            //{
            //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
            //}
            GHT:
                for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count > 0))
                    {
                        FindSong = "True";
                        //    CurrentPlaylistRow = i;
                        break;
                    }
                    else
                    {
                        FindSong = "false";
                        //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                        //{
                        //    CurrentPlaylistRow = 0;
                        //}
                        //else
                        //{
                        //    CurrentPlaylistRow = i;
                        //}
                    }
                }
                if (FindSong == "false")
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count == 0))
                    {
                        //  CurrentPlaylistRow = 0;
                        goto GHT;
                    }
                }
                // dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];

                //  dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;
                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));
                CurrentRow = 0;
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer1.URL = MusicFileName;
                    musicPlayer1.settings.volume = 0;
                    timer1.Enabled = true;
                    return;
                }
            }




        gg:
            if (CurrentRow == dgPlaylist.Rows.Count - 1)
            {
                if (IsDrop_Song == false)
                {
                    //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                    //{
                    //    CurrentPlaylistRow = 0;
                    //}
                    //else
                    //{
                    //    CurrentPlaylistRow = CurrentPlaylistRow + 1;
                    //}

                    for (int i = Convert.ToInt16(CurrentPlaylistRow); i < dgLocalPlaylist.Rows.Count; i++)
                    {
                        mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + StaticClass.DefaultPlaylistId;
                        dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                        if ((dtDetail.Rows.Count > 0))
                        {
                            // CurrentPlaylistRow = i;
                            break;
                        }
                        else
                        {
                            //if (CurrentPlaylistRow == dgLocalPlaylist.Rows.Count - 1)
                            //{
                            //    CurrentPlaylistRow = 0;
                            //}
                            //else
                            //{
                            //    CurrentPlaylistRow = i;
                            //}
                        }
                    }

                    // dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1];

                    //  dgLocalPlaylist.Rows[CurrentPlaylistRow].Selected = true;

                    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));
                    CurrentRow = 0;
                }
                else
                {
                    IsDrop_Song = false;
                }
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                    musicPlayer1.URL = MusicFileName;
                    musicPlayer1.settings.volume = 0;
                    timer1.Enabled = true;
                    return;
                }


            }
            //if (chkShuffleSong.Checked == true)
            //{
            //    CurrentRow = CurrentRow + 1;
            //}
            //else
            //{
            if (CurrentRow >= dgPlaylist.Rows.Count)
            {
                CurrentRow = 0;
            }
            else
            {
                CurrentRow = CurrentRow + 1;
            }
            //}
            if (CurrentRow == dgPlaylist.Rows.Count)
            {
                CurrentRow = dgPlaylist.Rows.Count - 1;
                goto gg;
            }

            TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".sec";
            MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value + ".mp3";
            if (System.IO.File.Exists(TempMusicFileName))
            {
                DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells[0].Value));
                musicPlayer1.URL = MusicFileName;
                musicPlayer1.settings.volume = 0;
                timer1.Enabled = true;
                return;
            }
            for (int i = Convert.ToInt16(CurrentRow); i < dgPlaylist.Rows.Count; i++)
            {
                TempMusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".sec";
                MusicFileName = Application.StartupPath + "\\" + dgPlaylist.Rows[i].Cells[0].Value + ".mp3";
                if (System.IO.File.Exists(TempMusicFileName))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value));
                    musicPlayer1.URL = MusicFileName;
                    musicPlayer1.settings.volume = 0;

                    timer1.Enabled = true;

                    //if (chkShuffleSong.Checked == true)
                    //{
                    //    CurrentRow = i + 1;
                    //}
                    //else
                    //{
                    CurrentRow = i;
                    //}

                    timer1.Enabled = true;
                    return;
                }

            }
        }
        private void NextSongDisplay(string NextSongId)
        {
            try
            {
                string mlsSql = "";
                var Special_Name = "";
                string Special_Change = "";

                mlsSql = "SELECT  Titles.Title as songname, Albums.Name as AlbumsName, Artists.Name AS ArtistsName, Titles.Time FROM (Albums INNER JOIN Titles ON Albums.AlbumID = Titles.AlbumID) INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID where Titles.titleid=" + Convert.ToInt32(NextSongId);
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet_Local(mlsSql);

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["songname"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblSongName2.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["ArtistsName"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblArtistName2.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["AlbumsName"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                //lblAlbumName2.Text = Special_Change;
                UpcomingSongPlayerOne = "";
                UpcomingSongPlayerTwo = NextSongId;

                string str = ds.Tables[0].Rows[0]["Time"].ToString();
                string[] arr = str.Split(':');
                DropSongLength = arr[1] + ":" + arr[2];
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }

        private void panWmp2_DragDrop(object sender, DragEventArgs e)
        {
            string file;
            string LocalFileName;
            string TempLocalFileName;
            Boolean SongFind;
            SongFind = false;
            Grid_Clear = false;
            file = (string)e.Data.GetData(DataFormats.Text);
            Drop_Song = false;
            StopDuplicate = "Yes";
            LocalFileName = Application.StartupPath + "\\" + file + ".mp3";
            TempLocalFileName = Application.StartupPath + "\\" + file + ".sec";
            ShowPlaylistCounter = "No";
            IsSongDropAdvt = true;
            if (dgLocalPlaylist.Rows.Count == 0 && dgPlaylist.Rows.Count == 0)
            {
                SaveDefaultPlaylist(file);
                PlaySongDefault();
                btnPlay.Text = "";
                return;
            }
            else if (dgLocalPlaylist.Rows.Count != 0 && dgPlaylist.Rows.Count == 0)
            {
                insert_Playlist_song(file, "Yes", true);
                PlaySongDefault();
                btnPlay.Text = "";
                return;
            }
            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
            {
                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == file)
                {
                    SongFind = true;
                }
            }
            if (SongFind == false)
            {
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    MessageBox.Show("Please check your Internet connection.", "Music Player");
                    return;
                }
                if (System.IO.File.Exists(TempLocalFileName))
                {
                    IsDrop_Song = true;
                    if (IsLast100Working == "Yes")
                    {
                        insert_Playlist_song(file, "No", true);
                        dgPlaylist.Rows.Add();
                        dgPlaylist.Rows[dgPlaylist.Rows.Count - 1].Cells[0].Value = file;
                    }
                    else
                    {
                        insert_Playlist_song(file, "No", true);
                        dgPlaylist.Rows.Add();
                        dgPlaylist.Rows[dgPlaylist.Rows.Count - 1].Cells[0].Value = file;
                    }
                    DeleteHideSongs();
                    InsertHideSong(file);
                    RowHide();
                    if (dgPlaylist.Visible == true)
                    {
                        DownloadSong(dgPlaylist);
                    }
                    else
                    {
                        DownloadSong(dgOtherPlaylist);
                    }
                    if (musicPlayer1.URL != "")
                    {
                        Set_foucs_PayerOne();
                    }
                    else if (musicPlayer2.URL != "")
                    {
                        Set_foucs_PayerTwo();
                    }
                }
                else
                {
                    string sQr = "";
                    sQr = "select COUNT(dfclientid) as TotalDownload from UserDownloadTitle where DfClientId=" + StaticClass.UserId + " and TokenId= " + StaticClass.TokenId;
                    DataSet dsTitle = new DataSet();
                    dsTitle = ObjMainClass.fnFillDataSet(sQr);
                    if (StaticClass.TotalTitles == dsTitle.Tables[0].Rows[0]["TotalDownload"].ToString())
                    {
                        MessageBox.Show("Your songs downloading limit is over." + Environment.NewLine + "Please contact vendor to resume the service.", "Music Player");
                        return;
                    }
                    IsDrop_Song = true;
                    Add_Playlist = true;
                    Drop_Song = true;
                    FirstTimeSong = false;
                    insert_temp_data(file);
                    Drop_TitleName = file;
                    multi_song_download();
                    return;
                }


            }

            if (musicPlayer2.URL == "")
            {
                NextSongDisplay(file);
                Song_Set_foucs3(file);
                return;
            }
            else if (musicPlayer1.URL == "")
            {
                NextSongDisplay2(file);
                Song_Set_foucs3(file);
                return;

            }
        }
        private void Song_Set_foucs3(string fileName)
        {
            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
            {
                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == fileName)
                {
                    CurrentRow = i - 1;
                    break;
                }
            }
        }

        private void panWmp2_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        private void SaveDefaultPlaylist(string DefaultSongId)
        {
            string lStr = "";
            lStr = "select * from PlayLists where Name='Default' and userid=" + StaticClass.UserId;
            DataSet ds = new DataSet();
            ds = ObjMainClass.fnFillDataSet(lStr);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DefaultPlaylistSave("Default");
                FillLocalPlaylist();
                // MessageBox.Show("This playlist name is already used.", "Copyright Player");
                return;
            }
            else if (StaticClass.Is_Admin != "1")
            {
                MessageBox.Show(ObjMainClass.MainMessage, "Copyright Player");
                return;
            }
            DefaultPlaylistSave("Default");

            FillLocalPlaylist();

        }

        private void DefaultPlaylistSave(string PlaylistName)
        {
            Int32 Playlist_Id = 0;
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("InsertPlayLists", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt));
            cmd.Parameters["@UserID"].Value = StaticClass.UserId;

            cmd.Parameters.Add(new SqlParameter("@IsPredefined", SqlDbType.Bit));
            cmd.Parameters["@IsPredefined"].Value = 0;

            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
            cmd.Parameters["@Name"].Value = PlaylistName;

            cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.VarChar, 50));
            cmd.Parameters["@Summary"].Value = " ";

            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 50));
            cmd.Parameters["@Description"].Value = " ";

            try
            {
                Playlist_Id = Convert.ToInt32(cmd.ExecuteScalar());
                StaticClass.Last100PlaylistId = Playlist_Id;
                string sQr = "";
                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                sQr = "insert into PlayLists values(" + Playlist_Id + ", ";
                sQr = sQr + StaticClass.UserId + " , '" + PlaylistName + "', " + StaticClass.TokenId + ",'' )";
                StaticClass.LocalCon.Open();
                OleDbCommand cmdSaveLocal = new OleDbCommand();
                cmdSaveLocal.Connection = StaticClass.LocalCon;
                cmdSaveLocal.CommandText = sQr;
                cmdSaveLocal.ExecuteNonQuery();
                StaticClass.LocalCon.Close();

                // MessageBox.Show("Saved");
            }
            catch (Exception ex)
            {
                // throw new ApplicationException ("Data error.");
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                StaticClass.constr.Close();
            }
        }

        private void panWmp1_DragDrop(object sender, DragEventArgs e)
        {
            string file;
            string LocalFileName;
            string TempLocalFileName;
            Boolean SongFind;
            SongFind = false;
            Grid_Clear = false;
            file = (string)e.Data.GetData(DataFormats.Text);
            Drop_Song = false;
            StopDuplicate = "Yes";
            LocalFileName = Application.StartupPath + "\\" + file + ".mp3";
            TempLocalFileName = Application.StartupPath + "\\" + file + ".sec";
            ShowPlaylistCounter = "No";
            IsSongDropAdvt = true;
            if (dgLocalPlaylist.Rows.Count == 0 && dgPlaylist.Rows.Count == 0)
            {
                SaveDefaultPlaylist(file);
                PlaySongDefault();
                btnPlay.Text = "";
                return;
            }
            else if (dgLocalPlaylist.Rows.Count != 0 && dgPlaylist.Rows.Count == 0)
            {
                insert_Playlist_song(file, "Yes", true);
                PlaySongDefault();
                btnPlay.Text = "";
                return;
            }

            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
            {
                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == file)
                {
                    SongFind = true;
                }
            }
            if (SongFind == false)
            {
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    MessageBox.Show("Please check your Internet connection.", "Music player");
                    return;
                }
                if (System.IO.File.Exists(TempLocalFileName))
                {
                    IsDrop_Song = true;

                    if (IsLast100Working == "Yes")
                    {
                        insert_Playlist_song(file, "No", true);
                        dgPlaylist.Rows.Add();
                        dgPlaylist.Rows[dgPlaylist.Rows.Count - 1].Cells[0].Value = file;

                    }
                    else
                    {
                        insert_Playlist_song(file, "No", true);
                        dgPlaylist.Rows.Add();
                        dgPlaylist.Rows[dgPlaylist.Rows.Count - 1].Cells[0].Value = file;
                    }
                    DeleteHideSongs();
                    InsertHideSong(file);
                    RowHide();
                    if (dgPlaylist.Visible == true)
                    {
                        DownloadSong(dgPlaylist);
                    }
                    else
                    {
                        DownloadSong(dgOtherPlaylist);
                    }
                    if (musicPlayer1.URL != "")
                    {
                        Set_foucs_PayerOne();
                    }
                    else if (musicPlayer2.URL != "")
                    {
                        Set_foucs_PayerTwo();
                    }

                }
                else
                {
                    string sQr = "";
                    sQr = "select COUNT(dfclientid) as TotalDownload from UserDownloadTitle where DfClientId=" + StaticClass.UserId + " and TokenId= " + StaticClass.TokenId;
                    DataSet dsTitle = new DataSet();
                    dsTitle = ObjMainClass.fnFillDataSet(sQr);
                    if (StaticClass.TotalTitles == dsTitle.Tables[0].Rows[0]["TotalDownload"].ToString())
                    {
                        MessageBox.Show("Your songs downloading limit is over." + Environment.NewLine + "Please contact vendor to resume the service.", "Music Player");
                        return;
                    }

                    Add_Playlist = true;
                    Drop_Song = true;
                    IsDrop_Song = true;
                    FirstTimeSong = false;
                    insert_temp_data(file);
                    Drop_TitleName = file;
                    multi_song_download();
                    return;
                }


            }
            if (musicPlayer2.URL == "")
            {

                NextSongDisplay(file);
                Song_Set_foucs3(file);

                return;

            }
            else if (musicPlayer1.URL == "")
            {
                NextSongDisplay2(file);
                Song_Set_foucs3(file);

                return;

            }

        }

        private void panWmp1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void timMusicTimeOne_Tick(object sender, EventArgs e)
        {
            try
            {

                double t1 = Math.Floor(musicPlayer1.currentMedia.duration - musicPlayer1.Ctlcontrols.currentPosition);
                double w1 = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
                double mint1 = Math.Floor(t1 / 60);
                double s1;
                int r1;
                s1 = Convert.ToInt16(Math.Abs(t1 / 60));
                r1 = Convert.ToInt16(t1 % 60);
                //--------------------------------------------//
                //--------------------------------------------//

                double fd;
                fd = Math.Floor(musicPlayer1.currentMedia.duration);
                double zh;
                zh = fd / 60;
                double left = System.Math.Floor(zh);
                double sec2 = fd % 60;
                //--------------------------------------------//
                //--------------------------------------------//

                if (musicPlayer1.status == "Ready")
                {
                    lblMusicTimeOne.Text = "00:00";
                    lblSongDurationOne.Text = "00:00";
                }
                else
                {
                    lblMusicTimeOne.Text = mint1.ToString("00") + ":" + r1.ToString("00");
                    lblSongDurationOne.Text = left.ToString("00") + ":" + sec2.ToString("00");
                }
                double w = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
                pbarMusic1.Maximum = Convert.ToInt16(musicPlayer1.currentMedia.duration);
                pbarMusic1.Value = Convert.ToInt16(w);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void timMusicTimeTwo_Tick(object sender, EventArgs e)
        {
            try
            {
                double t1 = Math.Floor(musicPlayer2.currentMedia.duration - musicPlayer2.Ctlcontrols.currentPosition);
                double w1 = Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);
                double mint1 = Math.Floor(t1 / 60);
                double s1;
                int r1;
                s1 = Convert.ToInt16(Math.Abs(t1 / 60));
                r1 = Convert.ToInt16(t1 % 60);
                //--------------------------------------------//
                //--------------------------------------------//

                double fd;
                fd = Math.Floor(musicPlayer2.currentMedia.duration);
                double zh;
                zh = fd / 60;
                double left = System.Math.Floor(zh);
                double sec2 = fd % 60;
                //--------------------------------------------//
                //--------------------------------------------//

                if (musicPlayer2.status == "Ready")
                {
                    lblMusicTimeTwo.Text = "00:00";
                    lblSongDurationTwo.Text = "00:00";
                }
                else
                {
                    lblMusicTimeTwo.Text = mint1.ToString("00") + ":" + r1.ToString("00");
                    lblSongDurationTwo.Text = left.ToString("00") + ":" + sec2.ToString("00");
                }
                double w = Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);
                pbarMusic2.Maximum = Convert.ToInt16(musicPlayer2.currentMedia.duration);
                pbarMusic2.Value = Convert.ToInt16(w);
            }
            catch
            {
            }
        }

        private void dgCommanGrid_SelectionChanged(object sender, EventArgs e)
        {
            //dgCommanGrid.ReadOnly = false;
            dgCommanGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            //dgCommanGrid.MultiSelect = true ;
        }

        private void dgLocalPlaylist_SelectionChanged(object sender, EventArgs e)
        {
            dgLocalPlaylist.ReadOnly = true;
            dgLocalPlaylist.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgLocalPlaylist.MultiSelect = false;
        }
        private void Get_Current_Song(string fileName)
        {
            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
            {
                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == fileName)
                {
                    CurrentRow = i;
                    return;
                }
            }
        }
        private void PopulateShuffleSong(DataGridView dgGrid, Int32 currentPlayRow)
        {
            try
            {
                string mlsSql = "";
                string GetLocalPath = "";
                string TitleYear = "";
                string TitleTime = "";
                var Special_Name = "";
                string Special_Change = "";
                Int32 iCtr = 0;
                Int32 srNo = 0;
                DataTable dtDetail;
                mlsSql = "SELECT  Titles.TitleID, ltrim(Titles.Title) as Title, Titles.Time,Albums.Name AS AlbumName ,";
                mlsSql = mlsSql + " Titles.TitleYear as TitleYear ,  ltrim(Artists.Name) as ArtistName  FROM ((( TitlesInPlaylists  ";
                mlsSql = mlsSql + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID )  ";
                mlsSql = mlsSql + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ) ";
                mlsSql = mlsSql + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ) ";
                mlsSql = mlsSql + " where TitlesInPlaylists.PlaylistID=" + Convert.ToInt32(currentPlayRow) + " ORDER BY   Rnd(-(100000*Titles.TitleID)*Time())";

                dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                InitilizeGrid(dgGrid);
                if ((dtDetail.Rows.Count > 0))
                {
                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {
                        GetLocalPath = dtDetail.Rows[iCtr]["TitleID"] + ".mp3";
                        srNo = iCtr;
                        dgGrid.Rows.Add();
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["songid"].Value = dtDetail.Rows[iCtr]["TitleID"];

                        Special_Name = "";
                        Special_Change = "";
                        Special_Name = dtDetail.Rows[iCtr]["Title"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["songname"].Value = Special_Change;

                        string str = dtDetail.Rows[iCtr]["Time"].ToString();
                        string[] arr = str.Split(':');
                        TitleTime = arr[1] + ":" + arr[2];

                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Length"].Value = TitleTime;

                        Special_Name = "";
                        Special_Change = "";

                        Special_Name = dtDetail.Rows[iCtr]["AlbumName"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Album"].Value = Special_Change;

                        TitleYear = dtDetail.Rows[iCtr]["TitleYear"].ToString();
                        if (TitleYear == "0")
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Year"].Value = "- - -";
                        }
                        else
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Year"].Value = dtDetail.Rows[iCtr]["TitleYear"];
                        }

                        Special_Name = "";
                        Special_Change = "";

                        Special_Name = dtDetail.Rows[iCtr]["ArtistName"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Artist"].Value = Special_Change;

                        if (this.Width.ToString() == "1020")
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 11);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Style.Font = new Font("Segoe UI", 11);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        }
                    }
                    foreach (DataGridViewRow row in dgGrid.Rows)
                    {
                        row.Height = 33;
                    }
                    RowHide();
                }
                else
                {
                    MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                    PopulateInputFileTypeDetail(dgGrid, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                }

            }
            catch
            {

            }
        }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (musicPlayer1.URL != "")
            {
                if (btnPlay.Text == ".")
                {
                    btnPlay.Text = "";
                    musicPlayer1.Ctlcontrols.play();
                    musicPlayer1.settings.volume = 100;
                    btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
                    timGetRemainAdvtTime.Enabled = true;
                }
                else if (btnPlay.Text == "")
                {
                    btnPlay.Text = ".";
                    musicPlayer1.Ctlcontrols.pause();
                    btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
                    timGetRemainAdvtTime.Enabled = false;
                }
            }
            else if (musicPlayer2.URL != "")
            {
                if (btnPlay.Text == ".")
                {
                    btnPlay.Text = "";
                    musicPlayer2.Ctlcontrols.play();
                    musicPlayer2.settings.volume = 100;
                    btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
                    timGetRemainAdvtTime.Enabled = true;
                }
                else if (btnPlay.Text == "")
                {
                    btnPlay.Text = ".";
                    musicPlayer2.Ctlcontrols.pause();
                    btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
                    timGetRemainAdvtTime.Enabled = false;
                }
            }

        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            if (musicPlayer1.URL != "")
            {
                if (btnMute.Text == "")
                {
                    btnMute.Text = ".";
                    musicPlayer1.settings.mute = true;
                    Song_Mute = true;
                    btnMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_red));
                }
                else if (btnMute.Text == ".")
                {
                    btnMute.Text = "";
                    musicPlayer1.settings.volume = 100;
                    musicPlayer1.settings.mute = false;
                    Song_Mute = false;
                    btnMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_blue));
                }
            }
            else if (musicPlayer2.URL != "")
            {
                if (btnMute.Text == "")
                {
                    btnMute.Text = ".";
                    musicPlayer2.settings.mute = true;
                    Song_Mute = true;
                    btnMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_red));
                }
                else if (btnMute.Text == ".")
                {
                    btnMute.Text = "";
                    musicPlayer2.settings.volume = 100;
                    musicPlayer2.settings.mute = false;
                    Song_Mute = false;
                    btnMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_blue));
                }
            }
        }
        private void btnShop_Click(object sender, EventArgs e)
        {
            musicPlayer1.Ctlcontrols.stop();
            musicPlayer2.Ctlcontrols.stop();
            btnPlay.Text = "Play";
        }


        private void button3_Click(object sender, EventArgs e)
        {
            Int32 CurrrentPos;
            if (musicPlayer1.URL != "")
            {
                CurrrentPos = Convert.ToInt32(musicPlayer1.Ctlcontrols.currentPosition);
                musicPlayer1.Ctlcontrols.currentPosition = CurrrentPos + 10;
            }
            if (musicPlayer2.URL != "")
            {
                CurrrentPos = Convert.ToInt32(musicPlayer2.Ctlcontrols.currentPosition);
                musicPlayer2.Ctlcontrols.currentPosition = CurrrentPos + 10;
            }

        }


        private void txtPlaylistName_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (ObjMainClass.CheckForInternetConnection() == false)
                    {
                        MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                        return;
                    }
                    string lStr = "";
                    lStr = "select * from PlayLists where Name='" + txtPlaylistName.Text + "' and userid=" + StaticClass.UserId + " and tokenid=" + StaticClass.TokenId;
                    DataSet ds = new DataSet();
                    ds = ObjMainClass.fnFillDataSet(lStr);

                    if (txtPlaylistName.Text == "")
                    {
                        MessageBox.Show("The playlist cannot be empty or without a name.", "Copyright Player");
                        return;
                    }
                    else if (pAction == "New")
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("This playlist name is already used.", "Copyright Player");
                            return;
                        }
                    }
                    else if (StaticClass.Is_Admin != "1")
                    {
                        MessageBox.Show(ObjMainClass.MainMessage, "Copyright Player");
                        return;
                    }
                    if (pAction == "New")
                    {
                        PlaylistSave();
                        txtPlaylistName.Text = "";
                        pAction = "New";
                        //  ModifyPlaylistId = 0;
                    }
                    else
                    {
                        PlaylistModify();
                        txtPlaylistName.Text = "";
                        pAction = "New";

                    }
                    FillLocalPlaylist();
                    Set_Playlist_foucs();
                    ModifyPlaylistId = 0;
                }

            }
            catch
            {
                // MessageBox.Show("Please check your Internet connection.","Copyright Player");
            }

        }

        private void dgPlaylist_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;

        }

        private void dgPlaylist_DragDrop(object sender, DragEventArgs e)
        {
            string SongName;
            string TempSongName;
            string lStr;
            string file;
            int Index;
            try
            {

                drawLine = false;
                dgPlaylist.Invalidate();
                Grid_Clear = false;
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    dgPlaylist.Invalidate();
                    if (dgPlaylist.Rows.Count >= 700)
                    {
                        MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Copyright Player");
                        return;
                    }
                }
                else
                {
                    dgOtherPlaylist.Invalidate();
                    if (dgOtherPlaylist.Rows.Count >= 700)
                    {
                        MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Copyright Player");
                        return;
                    }
                }
                if (StopDuplicate == "Yes") return;
                file = (string)e.Data.GetData(DataFormats.Text);
                SongName = Application.StartupPath + "\\" + file + ".mp3";
                TempSongName = Application.StartupPath + "\\" + file + ".sec";
                if (IsLast100Working == "Yes")
                {
                    lStr = "select * from TitlesInPlaylists where PlaylistID=" + StaticClass.Last100PlaylistId + " and TitleID=" + file;
                }
                else
                {
                    lStr = "select * from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID=" + file;
                }
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet_Local(lStr);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Show_Record == true)
                    {
                        DeleteHideSong(file);
                        PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    }
                }
                else
                {
                    if (ObjMainClass.CheckForInternetConnection() == false)
                    {
                        MessageBox.Show("Please check your Internet connection.", "Music Player");
                        return;
                    }
                    if (System.IO.File.Exists(TempSongName))
                    {
                        if (dgPlaylist.Rows.Count == 700)
                        {
                            MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Music Player");
                            return;
                        }
                        else
                        {

                            insert_Playlist_song(file, "No", false);
                            Point clientPoint = dgPlaylist.PointToClient(new Point(e.X, e.Y));
                            Index = dgPlaylist.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                            if (dgPlaylist.Rows.Count == 0 || dgPlaylist.Rows.Count == 1)
                            {
                                dgPlaylist.Rows.Add();
                                Index = 0;
                                ResetPlaylist(dgPlaylist, Index, file);
                                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                                DownloadSong(dgPlaylist);
                                return;

                            }
                            else if (Index == -1)
                            {
                                Index = 1;
                                ResetPlaylist(dgPlaylist, Index, file);
                                DownloadSong(dgPlaylist);
                                return;
                            }
                            else if (Index != -1)
                            {
                                ResetPlaylist(dgPlaylist, Index, file);

                            }
                            GetSongCounter();
                            DownloadSong(dgPlaylist);
                            if (musicPlayer1.URL != "")
                            {
                                Song_Set_foucs();
                            }
                            else if (musicPlayer2.URL != "")
                            {
                                Song_Set_foucs2();
                            }
                        }

                    }
                    else
                    {
                        string sQr = "";
                        sQr = "select COUNT(dfclientid) as TotalDownload from UserDownloadTitle where DfClientId=" + StaticClass.UserId + " and TokenId= " + StaticClass.TokenId;
                        DataSet dsTitle = new DataSet();
                        dsTitle = ObjMainClass.fnFillDataSet(sQr);
                        if (StaticClass.TotalTitles == dsTitle.Tables[0].Rows[0]["TotalDownload"].ToString())
                        {
                            MessageBox.Show("Your songs downloading limit is over." + Environment.NewLine + "Please contact vendor to resume the service.", "Music Player");
                            return;
                        }
                        eX = e.X;
                        eY = e.Y;
                        DropTitleSong = "Yes";
                        Add_Playlist = true;
                        FirstTimeSong = false;
                        insert_temp_data(file);
                        ShowPlaylistCounter = "Yes";
                        multi_song_download();
                    }
                }
                Show_Record = false;
                StopDuplicate = "Yes";
                Is_Drop = false;
            }
            catch { }
        }
        private void ResetPlaylist(DataGridView dgGrid, int RowIndex, string New_Song_Id)
        {
            string mlsSql = "";
            string TitleYear = "";
            string TitleTime = "";
            Int32 iCtr = 0;
            Int32 srNo = 0;
            string Title_id = "";
            string sr_No = "";
            string Title = "";
            string AlbumName = "";
            string Title_Year = "";
            string ArtistName = "";
            var Special_Name = "";
            string Special_Change = "";
            DataTable dtDetail = new DataTable();
            mlsSql = "SELECT distinct  Titles.TitleID, Titles.Title, Titles.Time,Albums.Name AS AlbumName ,";
            mlsSql = mlsSql + " Titles.TitleYear ,  Artists.Name as ArtistName  FROM ((( TitlesInPlaylists  ";
            mlsSql = mlsSql + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID )  ";
            mlsSql = mlsSql + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ) ";
            mlsSql = mlsSql + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ) ";
            mlsSql = mlsSql + " where Titles.TitleID=" + New_Song_Id;
            dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
            if ((dtDetail.Rows.Count > 0))
            {
                srNo = iCtr;
                Title_id = dtDetail.Rows[iCtr]["TitleID"].ToString();
                sr_No = srNo + 1 + ".";

                Special_Name = "";
                Special_Change = "";
                Special_Name = dtDetail.Rows[iCtr]["Title"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                Title = Special_Change;

                string str = dtDetail.Rows[iCtr]["Time"].ToString();
                string[] arr = str.Split(':');
                TitleTime = arr[1] + ":" + arr[2];
                AlbumName = dtDetail.Rows[iCtr]["AlbumName"].ToString();
                TitleYear = dtDetail.Rows[iCtr]["TitleYear"].ToString();
                if (TitleYear == "0")
                {
                    Title_Year = "- - -";
                }
                else
                {
                    Title_Year = dtDetail.Rows[iCtr]["TitleYear"].ToString();
                }

                ArtistName = dtDetail.Rows[iCtr]["ArtistName"].ToString();
                ArtistName = ArtistName.Replace("??$$$??", "'");
                var addedRow = dgGrid.Rows[RowIndex];
                dgGrid.Rows.Insert(RowIndex, Title_id, Title, TitleTime, ArtistName, AlbumName, Title_Year);

            }
            for (iCtr = 0; iCtr < dgGrid.Rows.Count; iCtr++)
            {
                dgGrid.Rows[iCtr].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                dgGrid.Rows[iCtr].Cells[2].Style.Font = new Font("Segoe UI", 11);
                dgGrid.Rows[iCtr].Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                dgGrid.Rows[iCtr].Cells[4].Style.Font = new Font("Segoe UI", 11);
                dgGrid.Rows[iCtr].Cells[5].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
            }

            foreach (DataGridViewRow row in dgGrid.Rows)
            {
                row.Height = 33;
            }

        }

        private void dgCommanGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string SongName = "";
            string sQr = "";
            try
            {
                if (e.ColumnIndex == 5 && e.RowIndex == -1)
                {
                    return;
                }

                if (e.ColumnIndex == 5)
                {
                    if (ObjMainClass.CheckForInternetConnection() == false)
                    {
                        MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                        return;
                    }
                    Add_Playlist = false;
                    FirstTimeSong = false;
                    panPlayerButton.Enabled = true;

                    SongName = Application.StartupPath + "\\" + dgCommanGrid.Rows[e.RowIndex].Cells[0].Value + ".sec";
                    if (System.IO.File.Exists(SongName))
                    {
                        return;
                    }
                    sQr = "select COUNT(dfclientid) as TotalDownload from UserDownloadTitle where DfClientId=" + StaticClass.UserId + " and TokenId= " + StaticClass.TokenId;
                    DataSet ds = new DataSet();
                    ds = ObjMainClass.fnFillDataSet(sQr);
                    if (StaticClass.TotalTitles == ds.Tables[0].Rows[0]["TotalDownload"].ToString())
                    {
                        MessageBox.Show("Your songs downloading limit is over." + Environment.NewLine + "Please contact vendor to resume the service.", "Copyright Player");
                        return;
                    }

                    insert_temp_data(dgCommanGrid.Rows[e.RowIndex].Cells[0].Value.ToString());
                    multi_song_download();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }

        private void dgCommanGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //string SongName;
            //string lStr;
            //string file;
            //file = dgCommanGrid.Rows[e.RowIndex].Cells[0].Value.ToString();
            //SongName = Application.StartupPath + "\\" + file + ".mp3";
            //lStr = "select * from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID=" + file;
            //DataSet ds = new DataSet();
            //ds = objClass1.fnFillDataSet(lStr);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //}
            //else
            //{
            //    if (System.IO.File.Exists(SongName))
            //    {
            //        insert_Playlist_song(file);

            //    }
            //    else
            //    {
            //        insert_temp_data(file);
            //        multi_song_download();
            //        insert_Playlist_song(file);
            //    }
            //}

        }

        private void dgPlaylist_KeyDown(object sender, KeyEventArgs e)
        {
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Music Player");
                return;
            }
            try
            {
                string localfilename = "";
                if (e.KeyCode == Keys.Delete)
                {
                    localfilename = dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() + ".mp3";
                    string localfilePath = Application.StartupPath + "\\" + localfilename;
                    if ((dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default") && (Convert.ToInt32(dgPlaylist.Rows.Count) <= 1))
                    {
                        MessageBox.Show("It is not possible to delete songs from the default playlist.", "Music Player");
                        return;
                    }
                    if (StaticClass.isDownload != "1" || StaticClass.isRemove != "1")
                    {
                        MessageBox.Show(ObjMainClass.MainMessage, "Music Player");
                        return;
                    }
                    if (musicPlayer1.URL != "")
                    {
                        g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                        if (dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() == g.ToString())
                        {
                            MessageBox.Show("It is not possible to delete the current song.", "Music Player");
                            return;
                        }
                    }
                    if (musicPlayer2.URL != "")
                    {
                        g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                        if (dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() == g.ToString())
                        {
                            MessageBox.Show("It is not possible to delete the current song.", "Music Player");
                            return;
                        }
                    }

                    if (Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) != 0)
                    {

                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                        StaticClass.LocalCon.Open();
                        OleDbCommand cmdLocal = new OleDbCommand();
                        cmdLocal.Connection = StaticClass.LocalCon;
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value;
                        cmdLocal.ExecuteNonQuery();
                        StaticClass.LocalCon.Close();

                        delete_temp_data(dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());
                        dgPlaylist.Rows.RemoveAt(dgPlaylist.CurrentCell.RowIndex);
                        GetSongCounter();


                        if (IsSongDropAdvt == false)
                        {
                            GetCurrentRow();
                            if (musicPlayer1.URL == "")
                            {
                                if (IsVisibleSong == true)
                                {
                                    if (LastRowId == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                                    }
                                }
                                else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                                {
                                    if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                                    }
                                }
                                else
                                {
                                    if (CurrentRow == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                                    }
                                }
                            }
                            else if (musicPlayer2.URL == "")
                            {
                                if (IsVisibleSong == true)
                                {
                                    if (LastRowId == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                                    }
                                }
                                else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                                {
                                    if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                                    }

                                }
                                else
                                {
                                    if (CurrentRow == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                                    }
                                }
                            }

                        }

                    }

                    else
                    {
                        MessageBox.Show("Please select a playlist.", "Music Player");
                    }
                    if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                    {
                        rCount = 0;
                        // DropSongLength = "";
                        // IsSongDropAdvt = false;
                        label7.Text = "0";
                        label8.Text = "0";
                        label14.Text = "0";
                        IsAdvtTimeGet = false;
                        GrossTotaltime = 0;
                        timGetRemainAdvtTime.Enabled = true;
                    }
                }
                if (e.KeyCode.Equals(Keys.Up))
                {
                    moveUp(dgPlaylist);
                }
                if (e.KeyCode.Equals(Keys.Down))
                {
                    moveDown(dgPlaylist);
                }
                e.Handled = true;
            }
            catch
            {

                return;
            }
        }

        private void dgLocalPlaylist_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    if (ObjMainClass.CheckForInternetConnection() == false)
                    {
                        MessageBox.Show("Please check your Internet connection.", "Music Player");
                        return;
                    }
                    if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                    {
                        MessageBox.Show("It is not possible to delete the default playlist.", "Music Player");
                        return;
                    }
                    string sgr = "";
                    if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                    {
                        if (musicPlayer1.URL != "")
                        {
                            g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                            sgr = "select * from TitlesInPlaylists where PlaylistID=" + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value + " and TitleId=" + g.ToString();
                            DataSet ds = new DataSet();
                            ds = ObjMainClass.fnFillDataSet(sgr);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                                {
                                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                                    {
                                        MessageBox.Show("It is not possible to delete the current playlist while playing.", "Music Player");
                                        return;
                                    }
                                }
                            }
                        }
                        if (musicPlayer2.URL != "")
                        {
                            g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                            sgr = "select * from TitlesInPlaylists where PlaylistID=" + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value + " and TitleId=" + g.ToString();
                            DataSet ds = new DataSet();
                            ds = ObjMainClass.fnFillDataSet(sgr);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                                {
                                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                                    {
                                        MessageBox.Show("It is not possible to delete the current playlist while playing.", "Music Player");
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                    StaticClass.constr.Open();
                    SqlCommand cmd = new SqlCommand("Delete_PlayList", StaticClass.constr);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@PlaylistID", SqlDbType.BigInt));
                    cmd.Parameters["@PlaylistID"].Value = Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        string sQr = "";
                        if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                        sQr = "delete from TitlesInPlaylists where PlaylistID =" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);
                        StaticClass.LocalCon.Open();
                        OleDbCommand cmdDelPlaylistLocal = new OleDbCommand();
                        cmdDelPlaylistLocal.Connection = StaticClass.LocalCon;
                        cmdDelPlaylistLocal.CommandText = sQr;
                        cmdDelPlaylistLocal.ExecuteNonQuery();
                        StaticClass.LocalCon.Close();

                        sQr = "";
                        if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                        sQr = "delete from Playlists where PlaylistID =" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);
                        StaticClass.LocalCon.Open();
                        OleDbCommand cmdDelLocal = new OleDbCommand();
                        cmdDelLocal.Connection = StaticClass.LocalCon;
                        cmdDelLocal.CommandText = sQr;
                        cmdDelLocal.ExecuteNonQuery();
                        StaticClass.LocalCon.Close();

                        string sdr = "";
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        sdr = "delete from tblmusic_player_settings where DFClientId=" + StaticClass.UserId + " and localUserId= " + StaticClass.LocalUserId + " and LastPlaylistId= " + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value;
                        StaticClass.constr.Open();
                        SqlCommand cmdDel = new SqlCommand();
                        cmdDel.Connection = StaticClass.constr;
                        cmdDel.CommandText = sdr;
                        cmdDel.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        FillLocalPlaylist();
                        if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                        {
                            dgPlaylist.Visible = true;
                            dgPlaylist.Dock = DockStyle.Fill;
                            dgOtherPlaylist.Visible = false;
                            PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                        }
                        else
                        {
                            dgOtherPlaylist.Visible = true;
                            dgOtherPlaylist.Dock = DockStyle.Fill;
                            dgPlaylist.Visible = false;
                            PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                        }

                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
            }
            catch
            {

                return;
            }
        }

        private void dgCommanGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                if (Grid_Clear == false)
                {
                    RowDeselect(dgCommanGrid);
                    dgCommanGrid.Rows[e.RowIndex].Selected = true;
                }
                else
                {
                    RowSelect(dgCommanGrid, dgCommanGrid.Rows[e.RowIndex].Cells[0].Value.ToString());
                }
                drawLine = true;
                dgCommanGrid.DoDragDrop(dgCommanGrid.Rows[e.RowIndex].Cells[0].Value.ToString(), DragDropEffects.Copy);
                Is_Drop = true;
            }
        }
        private void RowSelect(DataGridView Grid_Name, string Current_Value)
        {
            foreach (DataGridViewRow dr in Grid_Name.Rows)
            {
                if (dr.Cells[0].Value.ToString() == Current_Value)
                {
                    dr.Visible = true;
                }
            }
        }
        private void RowDeselect(DataGridView Grid_Name)
        {
            foreach (DataGridViewRow dr in Grid_Name.Rows)
            {
                dr.Selected = false;
            }
        }
        private void picSongPlay_Click(object sender, EventArgs e)
        {
            string TempSongName = "";
            string TempSongPath = "";
            if (dgPlaylist.CurrentCell.RowIndex == -1)
            {
                return;
            }
            timResetSong.Stop();
            btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
            btnPlay.Text = "";
            // int rowindex = dgPlaylist.CurrentCell.RowIndex;
            // int columnindex = dgPlaylist.CurrentCell.ColumnIndex;
            string localfilename = ""; ;
            try
            {
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() != "Default")
                {
                    insert_Playlist_song(dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString(), "Yes", true);
                    DeleteHideSongs();
                    InsertHideSong(dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());
                    RowHide();
                    Set_foucs_PayerOne();
                    for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                    {
                        if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString())
                        {
                            CurrentRow = i;
                            break;
                        }
                    }
                    localfilename = dgPlaylist.Rows[CurrentRow].Cells[0].Value.ToString() + ".mp3";
                    TempSongName = dgPlaylist.Rows[CurrentRow].Cells[0].Value.ToString() + ".sec";
                }
                else
                {
                    CurrentRow = dgPlaylist.CurrentCell.RowIndex;
                    localfilename = dgPlaylist.Rows[CurrentRow].Cells[0].Value.ToString() + ".mp3";
                    TempSongName = dgPlaylist.Rows[CurrentRow].Cells[0].Value.ToString() + ".sec";
                }

                TempSongPath = Application.StartupPath + "\\" + TempSongName;
                string localfilePath = Application.StartupPath + "\\" + localfilename;
                if (System.IO.File.Exists(TempSongPath))
                {
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value));
                    musicPlayer1.URL = localfilePath;
                    musicPlayer1.settings.volume = 100;
                    musicPlayer2.URL = "";
                    musicPlayer2.Ctlcontrols.stop();

                    DisplaySongPlayerOne();
                    Song_Set_foucs();


                    if (IsVisibleSong == true)
                    {
                        if (LastRowId == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }
                    }
                    else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                    {
                        if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                        }
                    }
                    else
                    {
                        if (CurrentRow == dgPlaylist.Rows.Count - 1)
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        }
                        else
                        {
                            NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                        }

                    }
                    g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                    GetSavedRating(g, dgSongRatingPlayerOne);
                    SetDisableRating(dgSongRatingPlayerTwo);
                }
                rCount = 0;
                DropSongLength = "";
                IsSongDropAdvt = false;
                label7.Text = "0";
                label8.Text = "0";
                label14.Text = "0";
                IsAdvtTimeGet = false;
                GrossTotaltime = 0;
                timGetRemainAdvtTime.Enabled = true;

                timResetSong.Start();
            }

            catch { }
        }

        private void picSongRemove_Click(object sender, EventArgs e)
        {

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;


            result = MessageBox.Show("Are you sure want to delete selected song ?", "Copyright Player", buttons);
            if (result == System.Windows.Forms.DialogResult.No)
            {
                return;

            }

            string MusicFileName = "";
            string mlsSql = "";
            DataTable dtDetail;
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Music Player");
                return;
            }
            try
            {
                if (dgPlaylist.CurrentCell.RowIndex == -1)
                {
                    return;
                }
                int rowindex = dgPlaylist.CurrentCell.RowIndex;
                int columnindex = dgPlaylist.CurrentCell.ColumnIndex;
                string localfilename;
                localfilename = dgPlaylist.Rows[rowindex].Cells[0].Value.ToString() + ".mp3";
                string localfilePath = Application.StartupPath + "\\" + localfilename;

                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    if ((dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default") && (Convert.ToInt32(dgPlaylist.Rows.Count) <= 1))
                    {
                        MessageBox.Show("It is not possible to delete songs from the default playlist.", "Music Player");
                        return;
                    }

                    if (StaticClass.isDownload != "1" || StaticClass.isRemove != "1")
                    {
                        MessageBox.Show(ObjMainClass.MainMessage, "Music Player");
                        return;
                    }
                    if (musicPlayer1.URL != "")
                    {
                        g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                        if (dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() == g.ToString())
                        {
                            MessageBox.Show("It is not possible to delete the current song.", "Music Player");
                            return;
                        }
                    }
                    if (musicPlayer2.URL != "")
                    {
                        g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                        if (dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() == g.ToString())
                        {
                            MessageBox.Show("It is not possible to delete the current song.", "Music Player");
                            return;
                        }
                    }
                }
                if (Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) != 0)
                {

                    if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                    {
                        rCount = 0;
                        // DropSongLength = "";
                        // IsSongDropAdvt = false;
                        label7.Text = "0";
                        label8.Text = "0";
                        label14.Text = "0";
                        IsAdvtTimeGet = false;
                        GrossTotaltime = 0;
                        timGetRemainAdvtTime.Enabled = true;

                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgPlaylist.Rows[rowindex].Cells[0].Value;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                        StaticClass.LocalCon.Open();
                        OleDbCommand cmdLocal = new OleDbCommand();
                        cmdLocal.Connection = StaticClass.LocalCon;
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgPlaylist.Rows[rowindex].Cells[0].Value;
                        cmdLocal.ExecuteNonQuery();
                        StaticClass.LocalCon.Close();
                        delete_temp_data(dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());

                        dgPlaylist.Rows.RemoveAt(dgPlaylist.CurrentCell.RowIndex);
                        if (IsSongDropAdvt == false)
                        {
                            GetCurrentRow();
                            if (musicPlayer1.URL == "")
                            {
                                if (IsVisibleSong == true)
                                {
                                    if (LastRowId == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                                    }
                                }
                                else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                                {
                                    if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                                    }
                                }
                                else
                                {
                                    if (CurrentRow == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                                    }
                                }
                            }
                            else if (musicPlayer2.URL == "")
                            {
                                if (IsVisibleSong == true)
                                {
                                    if (LastRowId == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                                    }
                                }
                                else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                                {
                                    if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                                    }

                                }
                                else
                                {
                                    if (CurrentRow == dgPlaylist.Rows.Count - 1)
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                                    }
                                    else
                                    {
                                        NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                        StaticClass.LocalCon.Open();
                        OleDbCommand cmdLocal = new OleDbCommand();
                        cmdLocal.Connection = StaticClass.LocalCon;
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value;
                        cmdLocal.ExecuteNonQuery();
                        StaticClass.LocalCon.Close();
                        delete_temp_data(dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());
                        delete_temp_data(dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());
                        PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    }

                    GetSongCounter();



                }

                else
                {
                    MessageBox.Show("Please select a playlist.", "Music Player");
                }
            }
            catch
            {
                return;
            }
        }


        private void picAddtoPlaylist_Click(object sender, EventArgs e)
        {
            string SongName;
            string TempSongName;
            string lStr;
            int TotalSelectSongs;
            TotalSelectSongs = 0;
            Grid_Clear = false;
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Music Player");
                return;
            }
            try
            {
                drawLine = false;
                dgPlaylist.Invalidate();
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    dgPlaylist.Invalidate();
                    if (dgPlaylist.Rows.Count >= 700)
                    {
                        MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Copyright Player");
                        return;
                    }
                }
                else
                {
                    dgOtherPlaylist.Invalidate();
                    if (dgOtherPlaylist.Rows.Count >= 700)
                    {
                        MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Copyright Player");
                        return;
                    }
                }

                for (int i = 0; i < dgCommanGrid.Rows.Count; i++)
                {
                    if (dgCommanGrid.Rows[i].Selected == true)
                    {
                        if (TotalSelectSongs == 0)
                        {
                            TotalSelectSongs = 1;
                        }
                        else
                        {
                            TotalSelectSongs = TotalSelectSongs + 1;
                        }
                    }
                }

                if (TotalSelectSongs > 10)
                {
                    MessageBox.Show("It is not possible to add more than 10 songs at the same time.", "Music Player");
                    return;
                }

                Add_Playlist = true;
                FirstTimeSong = false;
                for (int i = 0; i < dgCommanGrid.Rows.Count; i++)
                {
                    if (dgCommanGrid.Rows[i].Selected == true)
                    {
                        SongName = Application.StartupPath + "\\" + dgCommanGrid.Rows[i].Cells[0].Value + ".mp3";
                        TempSongName = Application.StartupPath + "\\" + dgCommanGrid.Rows[i].Cells[0].Value + ".sec";

                        lStr = "select * from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID=" + dgCommanGrid.Rows[i].Cells[0].Value;
                        DataSet ds = new DataSet();
                        ds = ObjMainClass.fnFillDataSet_Local(lStr);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DeleteHideSong(dgCommanGrid.Rows[i].Cells[0].Value.ToString());



                            if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                            {
                                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                            }
                            else
                            {
                                PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                            }


                        }
                        else
                        {
                            if (System.IO.File.Exists(TempSongName))
                            {
                                if (dgPlaylist.Rows.Count == 700)
                                {
                                    MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Music Player");
                                    return;
                                }
                                else
                                {
                                    DeleteHideSong(dgCommanGrid.Rows[i].Cells[0].Value.ToString());
                                    insert_Playlist_song(dgCommanGrid.Rows[i].Cells[0].Value.ToString(), "Yes", false);
                                    GetSongCounter();
                                    if (dgPlaylist.Visible == true)
                                    {
                                        DownloadSong(dgPlaylist);
                                    }
                                    else
                                    {
                                        DownloadSong(dgOtherPlaylist);
                                    }
                                   
                                }


                            }
                            else
                            {
                                string sQr = "";
                                sQr = "select COUNT(dfclientid) as TotalDownload from UserDownloadTitle where DfClientId=" + StaticClass.UserId + " and TokenId= " + StaticClass.TokenId;
                                DataSet dsTitle = new DataSet();
                                dsTitle = ObjMainClass.fnFillDataSet(sQr);
                                if (StaticClass.TotalTitles == dsTitle.Tables[0].Rows[0]["TotalDownload"].ToString())
                                {
                                    MessageBox.Show("Your songs downloading limit is over." + Environment.NewLine + "Please contact vendor to resume the service.", "Music Player");
                                    return;
                                }

                                insert_temp_data(dgCommanGrid.Rows[i].Cells[0].Value.ToString());
                                ShowPlaylistCounter = "Yes";
                                multi_song_download();
                            }
                        }

                        // rowSel = rowSel + ", " + dgCommanGrid.Rows[i].Cells[0].Value.ToString();
                    }
                }
            }
            catch
            {


            }

        }

        private void picPlaylistRemove_Click(object sender, EventArgs e)
        {

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
             DialogResult result;
            

              result = MessageBox.Show("Are you sure want to delete selected playlist ?", "Copyright Player", buttons);
             if (result == System.Windows.Forms.DialogResult.No)
            {
                return;

            }

            string sgr = "";
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Music Player");
                return;
            }
            try
            {
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    MessageBox.Show("It is not possible to delete the default playlist.", "Music Player");
                    return;
                }
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    if (musicPlayer1.URL != "")
                    {
                        g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                        sgr = "select * from TitlesInPlaylists where PlaylistID=" + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value + " and TitleId=" + g.ToString();
                        DataSet ds = new DataSet();
                        ds = ObjMainClass.fnFillDataSet_Local(sgr);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                            {
                                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                                {
                                    MessageBox.Show("It is not possible to delete the current playlist while playing.", "Music Player");
                                    return;
                                }
                            }
                        }
                    }
                    if (musicPlayer2.URL != "")
                    {
                        g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                        sgr = "select * from TitlesInPlaylists where PlaylistID=" + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value + " and TitleId=" + g.ToString();
                        DataSet ds = new DataSet();
                        ds = ObjMainClass.fnFillDataSet_Local(sgr);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                            {
                                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                                {
                                    MessageBox.Show("It is not possible to delete the current playlist while playing.", "Music Player");
                                    return;
                                }
                            }
                        }
                    }
                }


                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand("Delete_PlayList", StaticClass.constr);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@PlaylistID", SqlDbType.BigInt));
                cmd.Parameters["@PlaylistID"].Value = Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);
                try
                {
                    cmd.ExecuteNonQuery();

                    string sQr = "";
                    if (StaticClass.LocalCon.State == ConnectionState.Open)
                    {
                        StaticClass.LocalCon.Close();
                    }
                    sQr = "delete from Playlists where PlaylistID =" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdDelLocal = new OleDbCommand();
                    cmdDelLocal.Connection = StaticClass.LocalCon;
                    cmdDelLocal.CommandText = sQr;
                    cmdDelLocal.ExecuteNonQuery();
                    StaticClass.LocalCon.Close();


                    string sdr = "";
                    if (StaticClass.constr.State == ConnectionState.Open)
                    {
                        StaticClass.constr.Close();
                    }
                    sdr = "delete from tblmusic_player_settings where DFClientId=" + StaticClass.UserId + " and localUserId= " + StaticClass.LocalUserId + " and LastPlaylistId= " + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value + " and tokenno= " + StaticClass.TokenId;
                    StaticClass.constr.Open();
                    SqlCommand cmdDel = new SqlCommand();
                    cmdDel.Connection = StaticClass.constr;
                    cmdDel.CommandText = sdr;
                    cmdDel.ExecuteNonQuery();
                    StaticClass.constr.Close();

                    FillLocalPlaylist();
                    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                }
                catch (Exception ex)
                {
                    // MessageBox.Show(ex.Message);
                }
                finally
                {
                    StaticClass.constr.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                return;
            }

        }


        private void picSavePlaylist_Click(object sender, EventArgs e)
        {
            string lStr = "";
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            try
            {
                lStr = "select * from PlayLists where Name='" + txtPlaylistName.Text + "' and userid=" + StaticClass.UserId;
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet(lStr);

                if (txtPlaylistName.Text == "")
                {
                    MessageBox.Show("The playlist cannot be empty or without a name.", "Copyright Player");
                    return;
                }
                else if (ds.Tables[0].Rows.Count > 0)
                {
                    if (pAction == "New")
                    {
                        // MessageBox.Show("This playlist name is already used.", "Copyright Player");
                        //  return;
                    }
                    else
                    {
                        txtPlaylistName.Text = "";
                        return;
                    }
                }

                else if (StaticClass.Is_Admin != "1")
                {
                    MessageBox.Show(ObjMainClass.MainMessage, "Copyright Player");
                    return;
                }
                if (pAction == "New")
                {
                    PlaylistSave();
                    txtPlaylistName.Text = "";
                    pAction = "New";
                    // ModifyPlaylistId = 0;
                }
                else
                {
                    PlaylistModify();
                    txtPlaylistName.Text = "";
                    pAction = "New";
                }
                FillLocalPlaylist();
                Set_Playlist_foucs();
                ModifyPlaylistId = 0;

            }
            catch
            {

                return;
            }

        }
        private void Set_Playlist_foucs()
        {

            try
            {
                for (int i = 0; i < dgLocalPlaylist.Rows.Count; i++)
                {
                    if (dgLocalPlaylist.Rows[i].Cells[0].Value.ToString() == ModifyPlaylistId.ToString())
                    {
                        dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[i].Cells[1];
                        dgLocalPlaylist.Rows[i].Selected = true;

                        if (dgLocalPlaylist.Rows[i].Cells[2].Value.ToString() == "Default")
                        {
                            dgPlaylist.Visible = true;
                            dgPlaylist.Dock = DockStyle.Fill;
                            dgOtherPlaylist.Visible = false;

                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.White;

                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[3].Style.BackColor = Color.LightBlue;
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[3].Style.SelectionBackColor = Color.LightBlue;
                            PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value));
                        }
                        else
                        {
                            dgOtherPlaylist.Visible = true;
                            dgPlaylist.Visible = false;

                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[3].Style.BackColor = Color.White;
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.ForeColor = Color.Black;
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[3].Style.SelectionBackColor = Color.White;
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);

                            dgOtherPlaylist.Dock = DockStyle.Fill;
                            PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value));
                        }
                        break;
                    }
                }
            }
            catch
            {
            }
        }


        private void dgBestOf_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string str = "";
            try
            {
                str = "SELECT Titles.TitleID, ltrim(Titles.Title) as Title,Titles.Time, ltrim(Artists.Name) AS ArtistName, ltrim(Albums.Name) AS AlbumName ";
                str = str + " FROM Titles INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ";
                str = str + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID  ";
                str = str + " INNER JOIN TitlesInPlaylists ON Titles.TitleID = TitlesInPlaylists.TitleID ";
                str = str + " where TitlesInPlaylists.PlaylistID= " + dgBestOf.Rows[e.RowIndex].Cells[0].Value + " order by Titles.Title ";
                FillGrid(str);
                if (dgPlaylist.Visible == true)
                {
                    DownloadSong(dgPlaylist);
                }
                else
                {
                    DownloadSong(dgOtherPlaylist);
                }
                //if (dgBestOf.Rows[e.RowIndex].Cells[0].Value.ToString() == "-50")
                //{
                //    str = "select PlaylistId as IdName, Name as textName from Playlists where isPredefined=1 order by Name";
                //    FillCommanOptionGrid(str, dgBestOf);
                //}
                //else if (BestOffRecordShowType == "MainAlbum")
                //{
                //    FillBestOffData(Convert.ToInt32(dgBestOf.Rows[e.RowIndex].Cells[0].Value));
                //}
            }
            catch (Exception ex)
            {

            }

        }
        private void FillBestOffData(Int32 CurrentBestOffAlbumId)
        {
            string str = "";
            int iCtr;
            DataTable dtDetail;
            str = "SELECT Titles.TitleID, Titles.Title,Titles.Time, Artists.Name AS ArtistName, Albums.Name AS AlbumName ";
            str = str + " FROM Titles INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ";
            str = str + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID  ";
            str = str + " INNER JOIN TitlesInPlaylists ON Titles.TitleID = TitlesInPlaylists.TitleID ";
            str = str + " where TitlesInPlaylists.PlaylistID= " + CurrentBestOffAlbumId + " order by Titles.TitleID desc";


            dtDetail = ObjMainClass.fnFillDataTable_OldDB(str);

            InitilizeBestOffTitleGrid();
            BestOffRecordShowType = "BestOffTitle";
            dgBestOf.Rows.Add();
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[0].Value = "-50";
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[1].Value = "<< BACK";
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[1].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[1].Style.ForeColor = Color.White;
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[1].Style.BackColor = Color.FromArgb(55, 51, 45);
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[1].Style.SelectionForeColor = Color.White;
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[1].Style.SelectionBackColor = Color.FromArgb(55, 51, 45);

            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[2].Style.BackColor = Color.FromArgb(55, 51, 45);
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[2].Style.SelectionForeColor = Color.White;
            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[2].Style.SelectionBackColor = Color.FromArgb(55, 51, 45);



            dgBestOf.Rows[dgBestOf.Rows.Count - 1].Frozen = true;

            if ((dtDetail.Rows.Count > 0))
            {

                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgBestOf.Rows.Add();
                    dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["titleId"];
                    dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["Title"];
                    dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["ArtistName"];
                    dgBestOf.Rows[dgBestOf.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 8, System.Drawing.FontStyle.Regular);

                }


            }
            foreach (DataGridViewRow row in dgBestOf.Rows)
            {
                row.Height = 30;
            }



        }
        private void InitilizeBestOffTitleGrid()
        {
            if (dgBestOf.Rows.Count > 0)
            {
                dgBestOf.Rows.Clear();
            }
            if (dgBestOf.Columns.Count > 0)
            {
                dgBestOf.Columns.Clear();
            }

            dgBestOf.Columns.Add("playlistId", "playlist Id");
            dgBestOf.Columns["playlistId"].Width = 0;
            dgBestOf.Columns["playlistId"].Visible = false;
            dgBestOf.Columns["playlistId"].ReadOnly = true;

            dgBestOf.Columns.Add("Title", "Title");
            dgBestOf.Columns["Title"].Width = 190;
            dgBestOf.Columns["Title"].Visible = true;
            dgBestOf.Columns["Title"].ReadOnly = true;

            dgBestOf.Columns.Add("Artist", "Artist");
            dgBestOf.Columns["Artist"].Width = 80;
            dgBestOf.Columns["Artist"].Visible = true;
            dgBestOf.Columns["Artist"].ReadOnly = true;

            dgBestOf.ColumnHeadersVisible = true;

        }
        private void RowHide()
        {
            for (int i = 0; i < dgHideSongs.Rows.Count; i++)
            {
                foreach (DataGridViewRow dr in dgPlaylist.Rows)
                {
                    if (dr.Cells[0].Value.ToString() == dgHideSongs.Rows[i].Cells[0].Value.ToString())
                    {
                        dr.Visible = false;
                    }
                }
            }
        }
        private void UpdateHideSong(string Song_id)
        {
            for (int i = 0; i < dgHideSongs.Rows.Count; i++)
            {
                if (Convert.ToString(dgHideSongs.Rows[i].Cells[0].Value) == Song_id)
                {
                    dgHideSongs.Rows[i].Cells[1].Value = "Done";
                }

            }
        }
        private void InsertHideSong(string Song_id)
        {
            //string IsExist = "No";

            //for (int i = 0; i < dgHideSongs.Rows.Count; i++)
            //{
            //    if (Convert.ToString(dgHideSongs.Rows[i].Cells[0].Value) == Song_id)
            //    {
            //        IsExist = "Yes";
            //    }

            //}
            //if (IsExist == "No")
            //{
            InitilizeHideGrid();
            dgHideSongs.Rows.Add();
            dgHideSongs.Rows[dgHideSongs.Rows.Count - 1].Cells[0].Value = Song_id;
            //}
        }
        private void DeleteHideSongs()
        {
            try
            {
                for (int i = 0; i < dgHideSongs.Rows.Count; i++)
                {
                    if (StaticClass.constr.State == ConnectionState.Open)
                    {
                        StaticClass.constr.Close();
                    }
                    StaticClass.constr.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = StaticClass.constr;
                    if (IsLast100Working == "Yes")
                    {
                        cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + StaticClass.Last100PlaylistId + " and TitleID =" + dgHideSongs.Rows[i].Cells[0].Value;
                    }
                    else
                    {
                        cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + StaticClass.DefaultPlaylistId + " and TitleID =" + dgHideSongs.Rows[i].Cells[0].Value;
                    }
                    cmd.ExecuteNonQuery();
                    StaticClass.constr.Close();
                    //----------------------------- Local Database ------------------------//
                    if (StaticClass.LocalCon.State == ConnectionState.Open)
                    {
                        StaticClass.LocalCon.Close();
                    }
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdLocal = new OleDbCommand();
                    cmdLocal.Connection = StaticClass.LocalCon;
                    if (IsLast100Working == "Yes")
                    {
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + StaticClass.Last100PlaylistId + " and TitleID =" + dgHideSongs.Rows[i].Cells[0].Value;
                    }
                    else
                    {
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + StaticClass.DefaultPlaylistId + " and TitleID =" + dgHideSongs.Rows[i].Cells[0].Value;
                    }
                    cmdLocal.ExecuteNonQuery();
                    StaticClass.LocalCon.Close();

                }
                //if (chkShuffleSong.Checked == true)
                //{
                //    PopulateShuffleSong(Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value), ShuffleCount);
                //}
                //else
                //{
                //    PopulateInputFileTypeDetail(Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                //}
            }
            catch { }
        }
        private void DeleteHideSong(string Song_id)
        {
            for (int i = 0; i < dgHideSongs.Rows.Count; i++)
            {
                if (Convert.ToString(dgHideSongs.Rows[i].Cells[0].Value) == Song_id)
                {
                    dgHideSongs.Rows.RemoveAt(i);
                    Show_Record = true;
                    break;
                }
                Show_Record = false;

            }
            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
            {
                if (Convert.ToString(dgPlaylist.Rows[i].Cells[0].Value) == Song_id)
                {
                    dgPlaylist.Rows.RemoveAt(i);
                    break;
                }
            }
            IsDrop_Song = false;

        }

        private void DeleteParticularHideSong()
        {
            for (int i = 0; i < dgHideSongs.Rows.Count; i++)
            {
                if (Convert.ToString(dgHideSongs.Rows[i].Cells[1].Value) == "Done")
                {

                    if (StaticClass.constr.State == ConnectionState.Open)
                    {
                        StaticClass.constr.Close();
                    }
                    StaticClass.constr.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = StaticClass.constr;
                    if (IsLast100Working == "Yes")
                    {
                        cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + StaticClass.Last100PlaylistId + " and TitleID =" + dgHideSongs.Rows[i].Cells[0].Value;
                    }
                    else
                    {
                        cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + StaticClass.DefaultPlaylistId + " and TitleID =" + dgHideSongs.Rows[i].Cells[0].Value;
                    }
                    cmd.ExecuteNonQuery();
                    StaticClass.constr.Close();

                    if (StaticClass.LocalCon.State == ConnectionState.Open)
                    {
                        StaticClass.LocalCon.Close();
                    }
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdLocal = new OleDbCommand();
                    cmdLocal.Connection = StaticClass.LocalCon;
                    if (IsLast100Working == "Yes")
                    {
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + StaticClass.Last100PlaylistId + " and TitleID =" + dgHideSongs.Rows[i].Cells[0].Value;
                    }
                    else
                    {
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + StaticClass.DefaultPlaylistId + " and TitleID =" + dgHideSongs.Rows[i].Cells[0].Value;
                    }
                    cmdLocal.ExecuteNonQuery();
                    StaticClass.LocalCon.Close();

                    DeleteHideSong(dgHideSongs.Rows[i].Cells[0].Value.ToString());
                }
            }
            //if (IsLast100Working != "Yes")
            //{
            //    if (chkShuffleSong.Checked == true)
            //    {
            //        PopulateShuffleSong(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value), ShuffleCount);
            //    }
            //    else
            //    {
            //        PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[StaticClass.DefaultPlaylistCurrentRow].Cells[0].Value));
            //    }
            //}
        }

        private void btnGreenDownload_Click(object sender, EventArgs e)
        {
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            try
            {
                string sZr = "";
                sZr = "";
                sZr = " SELECT   distinct  Titles.TitleID, Titles.Title, Titles.Time, Artists.Name AS ArtistName, Albums.Name AS AlbumName";
                sZr = sZr + " FROM Titles ";
                sZr = sZr + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ";
                sZr = sZr + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ";
                sZr = sZr + " INNER JOIN TitlesInPlaylists ON Titles.TitleID = TitlesInPlaylists.TitleID ";
                sZr = sZr + " INNER JOIN Playlists ON TitlesInPlaylists.PlaylistID = Playlists.PlaylistID ";
                sZr = sZr + " where Playlists.UserID= " + StaticClass.UserId + " ";
                sZr = sZr + " order by Titles.TitleID desc";
                FillGrid(sZr);
            }
            catch { }
        }

        private void btnPurple_Click(object sender, EventArgs e)
        {
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            try
            {
                string sZr = "";
                sZr = "SELECT  Titles.TitleID, Titles.Title, Titles.Time, Artists.Name AS ArtistName, Albums.Name AS AlbumName";
                sZr = sZr + " FROM         Titles INNER JOIN";
                sZr = sZr + " Albums ON Titles.AlbumID = Albums.AlbumID INNER JOIN ";
                sZr = sZr + "  Artists ON Titles.ArtistID = Artists.ArtistID ";
                sZr = sZr + " INNER JOIN UserDownloadTitle ON Titles.TitleID = UserDownloadTitle.TitleId ";
                sZr = sZr + " where UserDownloadTitle.DFClientID=" + StaticClass.UserId + " ";
                sZr = sZr + " and UserDownloadTitle.TitleId not in ";
                sZr = sZr + " (SELECT   distinct  Titles.TitleID ";
                sZr = sZr + " FROM Titles ";
                sZr = sZr + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ";
                sZr = sZr + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID  ";
                sZr = sZr + " INNER JOIN TitlesInPlaylists ON Titles.TitleID = TitlesInPlaylists.TitleID ";
                sZr = sZr + " INNER JOIN Playlists ON TitlesInPlaylists.PlaylistID = Playlists.PlaylistID ";
                sZr = sZr + " where Playlists.UserID=" + StaticClass.UserId + ")";
                FillGrid(sZr);
                if (dgPlaylist.Visible == true)
                {
                    DownloadSong(dgPlaylist);
                }
                else
                {
                    DownloadSong(dgOtherPlaylist);
                }
            }
            catch
            {

                return;
            }
        }




        private void dgCommanGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ShiftKey || e.KeyCode == Keys.ControlKey)
            {
                Grid_Clear = true;
            }
            else
            {
                Grid_Clear = false;
            }
        }

        private void dgNewest_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                drawLine = true;
                RowSelect(dgNewest, dgNewest.Rows[e.RowIndex].Cells[0].Value.ToString());
                dgNewest.DoDragDrop(dgNewest.Rows[e.RowIndex].Cells[0].Value.ToString(), DragDropEffects.Copy);
                Is_Drop = true;
            }
        }



        private void picSearch_Click(object sender, EventArgs e)
        {
            string str = "";
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            if (txtSearch.Text == "" || txtSearch.Text == "Search")
            {
                str = "SELECT TOP (200) Titles.TitleID, ltrim(Titles.Title) as Title, Titles.Time, ltrim(Artists.Name) as ArtistName, ltrim(Albums.Name) AS AlbumName FROM Titles INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID where titlecategoryid=4 order by TitleID desc";
                FillGrid(str);
                if (dgPlaylist.Visible == true)
                {
                    DownloadSong(dgPlaylist);
                }
                else
                {
                    DownloadSong(dgOtherPlaylist);
                }
                return;

            }
            if (txtSearch.Text.Length < 2)
            {
                MessageBox.Show("Enter minimum 2 characters for a search function.", "Copyright Player");
                return;
            }

            SearchText = txtSearch.Text;
            CommanSearch();

            //stSearch = "spOverallSearch'" + txtSearch.Text + "',300";
            //FillGrid(stSearch);

            txtSearch.TextAlign = HorizontalAlignment.Left;
            txtSearch.ForeColor = Color.White;
            txtSearch.Text = "";
            if (dgPlaylist.Visible == true)
            {
                DownloadSong(dgPlaylist);
            }
            else
            {
                DownloadSong(dgOtherPlaylist);
            }

        }

        private void CommanSearch()
        {
            string stSearch = "";
            string strAlbum = "";
            if (rdoTitle.Checked == true)
            {
                stSearch = "spSearch_Title '" + SearchText + "'";
                FillGrid(stSearch);
            }
            else if (rdoArtist.Checked == true)
            {
                stSearch = "spSearch_Artist '" + SearchText + "'";
                FillGrid(stSearch);
            }
            else if (rdoAlbum.Checked == true)
            {
                //strAlbum = "select AlbumID,Name from Albums where name like '%" + SearchText + "%' and AlbumID <> 22558 and AlbumID <> 22557  order by AlbumID desc";

                strAlbum = "spSearch_Album_Copyright '" + SearchText + "'";
                ObjMainClass.fnFillComboBox_OldDB(strAlbum, cmbAlbum, "AlbumID", "Name", "");
                cmbAlbum.Visible = true;
                txtSearch.Visible = false;
                //stSearch = "spSearch_Album " + cmbAlbum.SelectedValue  ;
                //FillGrid(stSearch);
            }
        }

        private void mainwindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            //MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            //DialogResult result;
            //try
            //{

            //    result = MessageBox.Show("Are you sure to exit ?", "Copyright Player", buttons);
            //    if (result == System.Windows.Forms.DialogResult.Yes)
            //    {
            //        if (bgWorker.IsBusy == true)
            //        {
            //            MessageBox.Show("Song downloading in process" + Environment.NewLine + "Application is not exit ?", "Copyright Player");
            //            return;
            //        }
            //        if (ObjMainClass.CheckForInternetConnection() == false)
            //        {
            //            Application.Exit();
            //            return;
            //        }
            //        delete_temp_table();
            //        DeleteHideSongs();
            //        Music_Player_Settings();
            //        Application.Exit();
            //        return;
            //    }
            //    else if (result == System.Windows.Forms.DialogResult.No)
            //    {

            //        MessageBox.Show("No");
            //        return;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

        }


        private void dgPlaylist_DragOver(object sender, DragEventArgs e)
        {
            try
            {

                DataGridView.HitTestInfo info = this.dgPlaylist.HitTest(e.X, e.Y);
                label5.Text = e.Y.ToString();
                if (drawLine == true)
                {
                    StopDuplicate = "No";
                    if (Convert.ToInt32(label5.Text) <= Convert.ToInt32(217))
                    {
                        info = this.dgPlaylist.HitTest(20, 20);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(217) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(247))
                    {
                        info = this.dgPlaylist.HitTest(50, 50);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(247) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(277))
                    {
                        info = this.dgPlaylist.HitTest(80, 80);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(277) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(307))
                    {
                        info = this.dgPlaylist.HitTest(110, 110);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(307) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(337))
                    {
                        info = this.dgPlaylist.HitTest(140, 140);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(337) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(367))
                    {
                        info = this.dgPlaylist.HitTest(170, 170);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(367) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(397))
                    {
                        info = this.dgPlaylist.HitTest(200, 200);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(397) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(427))
                    {
                        info = this.dgPlaylist.HitTest(230, 230);
                    }
                    else
                    {
                        info = this.dgPlaylist.HitTest(240, 240);
                    }



                    if (info.ColumnIndex != -1)
                    {
                        Rectangle rect = this.dgPlaylist.GetRowDisplayRectangle(
                            info.RowIndex, true);
                        this.p1.X = rect.Left;
                        this.p1.Y = rect.Bottom;
                        this.p2.X = rect.Right;
                        this.p2.Y = rect.Bottom;
                        this.drawLine = true;
                        this.dgPlaylist.Invalidate();
                    }
                }
                else
                {
                    this.drawLine = false;
                    this.dgPlaylist.Invalidate();

                }
            }
            catch (Exception ex)
            {
            }

        }

        private void dgPlaylist_Paint(object sender, PaintEventArgs e)
        {
            if (this.drawLine)
            {
                using (p = new Pen(Color.Red, 3))
                {
                    EventSpl = e;
                    e.Graphics.DrawLine(p, p1, p2);
                }
            }
            else
            {
                //using (p = new Pen(Color.White, 0))
                //{
                //    EventSpl = e;
                //    e.Graphics.DrawLine(p, p1, p2);
                //}
            }
        }

        private void dgPlaylist_DragLeave(object sender, EventArgs e)
        {
            drawLine = false;
            dgPlaylist.Invalidate();
        }

        private void tbcMain_Click(object sender, EventArgs e)
        {

        }

        private void dgPlaylist_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgPlaylist_MouseLeave(object sender, EventArgs e)
        {
            drawLine = false;
            dgPlaylist.Invalidate();
        }

        private void timResetSong_Tick(object sender, EventArgs e)
        {
            string LocalUpcomingSong = "";
            if (dgPlaylist.Rows.Count == 0) return;
            if (panAdvt.Visible == true) return;
            if (pbarMusic1.Value == 0 && pbarMusic2.Value == 0)
            {
                if (UpcomingSongPlayerOne != "" && UpcomingSongPlayerTwo == "")
                {
                    DecrpetSec(Convert.ToInt32(UpcomingSongPlayerOne));
                    musicPlayer1.URL = Application.StartupPath + "\\" + UpcomingSongPlayerOne + ".mp3";
                    musicPlayer1.settings.volume = 100;
                    musicPlayer1.Ctlcontrols.play();
                    musicPlayer2.URL = "";
                    musicPlayer2.Ctlcontrols.stop();

                    lblSongName.ForeColor = Color.White;
                    lblArtistName.ForeColor = Color.White;
                    lblMusicTimeOne.ForeColor = Color.White;
                    lblSongDurationOne.ForeColor = Color.White;
                    pbarMusic1.ForeColor = Color.White;
                    panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));
                    pbarMusic1.BackColor = Color.FromArgb(9, 130, 154);

                    lblSongName2.ForeColor = Color.Gray;
                    lblArtistName2.ForeColor = Color.Gray;
                    lblMusicTimeTwo.ForeColor = Color.Gray;
                    lblSongDurationTwo.ForeColor = Color.Gray;
                    pbarMusic2.ForeColor = Color.Gray;
                    pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
                    panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                    g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                    GetSavedRating(g, dgSongRatingPlayerOne);
                    SetDisableRating(dgSongRatingPlayerTwo);

                    return;
                }
                else if (UpcomingSongPlayerOne == "" && UpcomingSongPlayerTwo != "")
                {
                    DecrpetSec(Convert.ToInt32(UpcomingSongPlayerTwo));
                    musicPlayer2.URL = Application.StartupPath + "\\" + UpcomingSongPlayerTwo + ".mp3";
                    musicPlayer2.settings.volume = 100;
                    musicPlayer2.Ctlcontrols.play();
                    musicPlayer1.URL = "";
                    musicPlayer1.Ctlcontrols.stop();

                    lblSongName2.ForeColor = Color.White;
                    lblArtistName2.ForeColor = Color.White;
                    lblMusicTimeTwo.ForeColor = Color.White;
                    lblSongDurationTwo.ForeColor = Color.White;
                    pbarMusic2.ForeColor = Color.White;
                    pbarMusic2.BackColor = Color.FromArgb(9, 130, 154);
                    panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));

                    lblSongName.ForeColor = Color.Gray;
                    lblArtistName.ForeColor = Color.Gray;
                    lblMusicTimeOne.ForeColor = Color.Gray;
                    lblSongDurationOne.ForeColor = Color.Gray;
                    pbarMusic1.ForeColor = Color.Gray;
                    pbarMusic1.BackColor = Color.FromArgb(175, 175, 175);
                    panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                    g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                    GetSavedRating(g, dgSongRatingPlayerTwo);
                    SetDisableRating(dgSongRatingPlayerOne);
                    return;
                }
                else if (UpcomingSongPlayerOne == "" && UpcomingSongPlayerTwo == "")
                {
                    LocalUpcomingSong = dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString();
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value));
                    musicPlayer1.URL = Application.StartupPath + "\\" + LocalUpcomingSong + ".mp3";
                    musicPlayer1.settings.volume = 100;
                    musicPlayer1.Ctlcontrols.play();
                    musicPlayer2.URL = "";
                    musicPlayer2.Ctlcontrols.stop();

                    lblSongName.ForeColor = Color.White;
                    lblArtistName.ForeColor = Color.White;
                    lblMusicTimeOne.ForeColor = Color.White;
                    lblSongDurationOne.ForeColor = Color.White;
                    pbarMusic1.ForeColor = Color.White;
                    panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));
                    pbarMusic1.BackColor = Color.FromArgb(9, 130, 154);

                    lblSongName2.ForeColor = Color.Gray;
                    lblArtistName2.ForeColor = Color.Gray;
                    lblMusicTimeTwo.ForeColor = Color.Gray;
                    lblSongDurationTwo.ForeColor = Color.Gray;
                    pbarMusic2.ForeColor = Color.Gray;
                    pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
                    panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                    g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                    GetSavedRating(g, dgSongRatingPlayerOne);
                    SetDisableRating(dgSongRatingPlayerTwo);
                    return;
                }
                else if (UpcomingSongPlayerOne != "" && UpcomingSongPlayerTwo != "")
                {
                    LocalUpcomingSong = dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString();
                    DecrpetSec(Convert.ToInt32(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value));
                    musicPlayer1.URL = Application.StartupPath + "\\" + LocalUpcomingSong + ".mp3";
                    musicPlayer1.settings.volume = 100;
                    musicPlayer1.Ctlcontrols.play();
                    musicPlayer2.URL = "";
                    musicPlayer2.Ctlcontrols.stop();
                    lblSongName.ForeColor = Color.White;
                    lblArtistName.ForeColor = Color.White;
                    lblMusicTimeOne.ForeColor = Color.White;
                    lblSongDurationOne.ForeColor = Color.White;
                    pbarMusic1.ForeColor = Color.White;
                    panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));
                    pbarMusic1.BackColor = Color.FromArgb(9, 130, 154);

                    lblSongName2.ForeColor = Color.Gray;
                    lblArtistName2.ForeColor = Color.Gray;
                    lblMusicTimeTwo.ForeColor = Color.Gray;
                    lblSongDurationTwo.ForeColor = Color.Gray;
                    pbarMusic2.ForeColor = Color.Gray;
                    pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
                    panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                    g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                    GetSavedRating(g, dgSongRatingPlayerOne);
                    SetDisableRating(dgSongRatingPlayerTwo);
                    return;
                }
            }
        }

        private void mainwindow_Move(object sender, EventArgs e)
        {
            // this.Location = new Point(121, 19);
        }

        private void mainwindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            try
            {

                if (e.CloseReason == CloseReason.UserClosing)
                {
                    if (bgWorker.IsBusy == true)
                    {
                        MessageBox.Show("Song downloading in process" + Environment.NewLine + "Application is not exit ?", "Copyright Player");
                        e.Cancel = true;
                        return;
                    }
                    result = MessageBox.Show("Are you sure to exit ?", "Copyright Player", buttons);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {

                        if (musicPlayer1.URL != "")
                        {
                            musicPlayer1.Ctlcontrols.stop();

                        }
                        if (musicPlayer2.URL != "")
                        {
                            musicPlayer2.Ctlcontrols.stop();

                        }
                        if (StreamMusicPlayer.URL != "")
                        {

                            StreamMusicPlayer.Ctlcontrols.stop();
                        }

                        //if (ObjMainClass.CheckForInternetConnection() == false)
                        //{
                        //    Application.Exit();
                        //    return;
                        //} 
                        //delete_temp_table();
                        //DeleteHideSongs();

                        //    INetFwPolicy2 firewallPolicyDel = (INetFwPolicy2)Activator.CreateInstance(
                        //Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                        //    firewallPolicyDel.Rules.Remove("MusicPlayer");

                        ObjMainClass.DeleteAllOgg("0");
                        Music_Player_Settings();
                        musicPlayer1.URL = "";
                        musicPlayer2.URL = "";
                        StreamMusicPlayer.URL = "";
                        e.Cancel = false;
                        Application.Exit();
                        return;
                    }
                    else if (result == System.Windows.Forms.DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        private void txtPlaylistName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 39 || Convert.ToInt32(e.KeyChar) == 37)
            {
                e.Handled = true;
                return;
            }
        }

        private void rdoTitle_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoTitle.Checked == true)
            {
                cmbAlbum.Visible = false;
                txtSearch.Visible = true;
            }
        }

        private void rdoArtist_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoArtist.Checked == true)
            {
                cmbAlbum.Visible = false;
                txtSearch.Visible = true;
            }

        }

        private void rdoAlbum_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAlbum.Checked == true)
            {
                cmbAlbum.Visible = false;
                txtSearch.Visible = true;
            }
        }

        private void cmbAlbum_SelectedIndexChanged(object sender, EventArgs e)
        {
            string stSearch = "";
            stSearch = "spSearch_Album " + cmbAlbum.SelectedValue;
            FillGrid(stSearch);
            if (dgPlaylist.Visible == true)
            {
                DownloadSong(dgPlaylist);
            }
            else
            {
                DownloadSong(dgOtherPlaylist);
            }
        }

        private void mainwindow_SizeChanged(object sender, EventArgs e)
        {
            return;
            try
            {

            panStream.Height = this.Height;
            panStream.Width = this.Width;

            panPrayer.Height = this.Height;
            panPrayer.Width = this.Width;

            pbarMusic1.Width = panPbarOne.Width;
            pbarMusic2.Width = panPbarTwo.Width;

            panStreamControls.Location = new Point(
         this.panStreamMainControls.Width / 2 - panStreamControls.Size.Width / 2,
         this.panStreamMainControls.Height / 2 - panStreamControls.Size.Height / 2);

            picStreaming.Location = new Point(
           this.panStreamUpper.Width / 2 - picStreaming.Size.Width / 2,
           this.panStreamUpper.Height / 2 - picStreaming.Size.Height / 2);


            dgSongRatingPlayerOne.Location = new Point(
               this.panMusicOne.Width - dgSongRatingPlayerOne.Size.Width - 10,
               29);

            dgSongRatingPlayerTwo.Location = new Point(
               this.panMusicTwo.Width - dgSongRatingPlayerTwo.Size.Width - 10,
               29);

            //Advt Code
            panAdvt.Width = this.Width;
            panAdvtPlayer.Location = new Point(
        this.panAdvt.Width / 2 - panAdvtPlayer.Size.Width / 2,
        this.panAdvt.Height / 2 - panAdvtPlayer.Size.Height / 2);


            panAdvt.Location = new Point(0, 0);


            panAdvtDetail.Location = new Point(0, 0);
            panAdvtDetail.Width = this.Width;
            panAdvtDetail.Height = this.Height;





            panAdvtSchedule.Location = new Point(0, 0);
            panAdvtSchedule.Width = panAdvtDetail.Width;
            panAdvtSchedule.Height = panAdvtDetail.Height;
            panAdvtGrid.Height = panAdvtSchedule.Height - panButtonSettings.Height - panScheduleDate.Height - 40;


            panAdvtInnerDetail.Location = new Point(
           this.panAdvtCenter.Width / 2 - panAdvtInnerDetail.Size.Width / 2,
           this.panAdvtCenter.Height / 2 - panAdvtInnerDetail.Size.Height / 2);


            dgClientAdvt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (panPrayerDetail.Visible == true)
            {
                panPrayerDetail.Size = new Size(this.Width, this.Height);
                panPrayerDetail.Location = new Point(0, 0);
            }

            //////////////////////////

            #region Font Size
            dgPlaylist.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12);
            if (dgOtherPlaylist.Rows.Count > 0)
            {
                dgOtherPlaylist.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12);
            }
            dgCommanGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12);
            foreach (DataGridViewRow row in dgPlaylist.Rows)
            {
                row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[3].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[4].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[5].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Height = 35;
            }
            if (dgOtherPlaylist.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgOtherPlaylist.Rows)
                {
                    row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    row.Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    row.Cells[3].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    row.Cells[4].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    row.Cells[5].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    row.Height = 35;
                }
            }

            foreach (DataGridViewRow row in dgCommanGrid.Rows)
            {
                row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[2].Style.Font = new Font("Segoe UI", 12);
                row.Cells[3].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[4].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Height = 35;
            }

            foreach (DataGridViewRow row in dgLocalPlaylist.Rows)
            {
                row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Height = 35;
            }
            foreach (DataGridViewRow row in dgNewest.Rows)
            {
                row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Height = 35;
            }
            foreach (DataGridViewRow row in dgBestOf.Rows)
            {
                row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Height = 35;
            }
            foreach (DataGridViewRow row in dgDailyAdvt.Rows)
            {
                row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[8].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Height = 35;
            }
            foreach (DataGridViewRow row in dgTop250.Rows)
            {
                row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Height = 35;
            }
            foreach (DataGridViewRow row in dgLast100.Rows)
            {
                row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Height = 35;
            }
            foreach (DataGridViewRow row in dgStream.Rows)
            {
                row.Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                row.Height = 35;
            }
            panel27.Width = 415;
            panel28.Width = 470;
            panel8.Height = 47;
            picAdvertisement.Size = new Size(47, 37);
            picPrayer.Size = new Size(47, 37);
            picStream.Size = new Size(47, 37);
            picStreamShedule.Size = new Size(47, 37);
            picManual.Size = new Size(47, 37);
            picPlaylistSchedule.Size = new Size(47, 37);
            picSongPlay.Size = new Size(47, 37);
            picSongRemove.Size = new Size(47, 37);


            picManual.Location = new Point(5, 2);
            picAdvertisement.Location = new Point(56, 2);
            picPlaylistSchedule.Location = new Point(107, 2);
            picStreamShedule.Location = new Point(158, 2);
            picStream.Location = new Point(208, 2);
            picPrayer.Location = new Point(259, 2);
            picSongPlay.Location = new Point(310, 2);
            picSongRemove.Location = new Point(360, 2);

            panel16.Height = 49;

            txtSearch.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            cmbAlbum.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);

            picAddtoPlaylist.Size = new Size(47, 37);
            picAddtoPlaylist.Location = new Point(414, 7);

            picSearch.Size = new Size(45, 34);
            picSearch.Location = new Point(4, 7);
            rdoTitle.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            rdoTitle.Size = new Size(75, 35);
            rdoTitle.Location = new Point(56, 6);
            rdoArtist.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            rdoArtist.Size = new Size(85, 35);
            rdoArtist.Location = new Point(135, 6);

            rdoAlbum.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            rdoAlbum.Size = new Size(92, 35);
            rdoAlbum.Location = new Point(225, 6);

            progressBar1.Location = new Point(319, 13);
            lblPercentage.Location = new Point(344, 17);

            txtPlaylistName.Size = new Size(175, 29);
            txtPlaylistName.Location = new Point(10, 21);

            lblGreen.Location = new Point(145, 4);
            lblRed.Location = new Point(145, 4);
            label9.Location = new Point(167, 4);

            picSavePlaylist.Size = new Size(47, 37);
            picSavePlaylist.Location = new Point(191, 13);

            picPlaylistRemove.Size = new Size(47, 37);
            picPlaylistRemove.Location = new Point(244, 13);

            txtLastPlaylistName.Size = new Size(229, 29);
            picSaveLast100Playlist.Size = new Size(47, 37);
            picSaveLast100Playlist.Location = new Point(242, 12);
            #endregion

            panPlaylistSchedule.Width = this.Width;
            panPlaylistSchedule.Height = this.Height;
            panPlaylistSchedule.Location = new Point(0, 0);
            dgPlaylistSch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (this.Width.ToString() == "1020")
            {
                #region Return Font Size
                dgPlaylist.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11);
                if (dgOtherPlaylist.Rows.Count > 0)
                {
                    dgOtherPlaylist.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11);
                }
                dgCommanGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 11);

                foreach (DataGridViewRow row in dgPlaylist.Rows)
                {
                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[2].Style.Font = new Font("Segoe UI", 11);
                    row.Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[4].Style.Font = new Font("Segoe UI", 11);
                    row.Cells[5].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Height = 33;
                }
                if (dgOtherPlaylist.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in dgOtherPlaylist.Rows)
                    {
                        row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        row.Cells[2].Style.Font = new Font("Segoe UI", 11);
                        row.Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        row.Cells[4].Style.Font = new Font("Segoe UI", 11);
                        row.Cells[5].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        row.Height = 33;
                    }
                }

                foreach (DataGridViewRow row in dgCommanGrid.Rows)
                {
                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[2].Style.Font = new Font("Segoe UI", 11);
                    row.Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[4].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Height = 33;
                }
                foreach (DataGridViewRow row in dgLocalPlaylist.Rows)
                {
                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Height = 30;
                }
                foreach (DataGridViewRow row in dgNewest.Rows)
                {
                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[2].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Height = 30;
                }
                foreach (DataGridViewRow row in dgBestOf.Rows)
                {
                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Height = 30;
                }
                foreach (DataGridViewRow row in dgDailyAdvt.Rows)
                {
                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[8].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Height = 30;
                }
                foreach (DataGridViewRow row in dgTop250.Rows)
                {
                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[2].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Height = 30;
                }
                foreach (DataGridViewRow row in dgLast100.Rows)
                {
                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[2].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Height = 30;
                }
                foreach (DataGridViewRow row in dgStream.Rows)
                {
                    row.Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    row.Height = 33;
                }
                panel27.Width = 328;
                panel8.Height = 34;
                panel28.Width = 440;
                picAdvertisement.Size = new Size(35, 28);
                picPrayer.Size = new Size(35, 28);
                picStream.Size = new Size(35, 28);
                picStreamShedule.Size = new Size(35, 28);
                picManual.Size = new Size(35, 28);
                picPlaylistSchedule.Size = new Size(35, 28);
                picSongPlay.Size = new Size(35, 28);
                picSongRemove.Size = new Size(35, 28);


                picManual.Location = new Point(5, 1);
                picAdvertisement.Location = new Point(44, 1);
                picPlaylistSchedule.Location = new Point(83, 1);
                picStreamShedule.Location = new Point(122, 1);
                picStream.Location = new Point(160, 1);
                picPrayer.Location = new Point(200, 1);
                picSongPlay.Location = new Point(238, 1);
                picSongRemove.Location = new Point(276, 1);

                panel16.Height = 41;


                txtSearch.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                cmbAlbum.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                picAddtoPlaylist.Size = new Size(40, 28);
                picAddtoPlaylist.Location = new Point(391, 7);

                picSearch.Size = new Size(33, 27);
                picSearch.Location = new Point(3, 7);
                rdoTitle.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                rdoTitle.Size = new Size(79, 25);
                rdoTitle.Location = new Point(41, 8);
                rdoArtist.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                rdoArtist.Size = new Size(83, 25);
                rdoArtist.Location = new Point(123, 8);

                rdoAlbum.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                rdoAlbum.Size = new Size(85, 25);
                rdoAlbum.Location = new Point(209, 8);

                progressBar1.Location = new Point(298, 9);
                lblPercentage.Location = new Point(327, 14);

                txtPlaylistName.Size = new Size(215, 24);
                txtPlaylistName.Location = new Point(10, 21);

                lblGreen.Location = new Point(253, 4);
                lblRed.Location = new Point(253, 4);
                label9.Location = new Point(275, 4);

                picSavePlaylist.Size = new Size(30, 28);
                picSavePlaylist.Location = new Point(231, 18);

                picPlaylistRemove.Size = new Size(30, 28);
                picPlaylistRemove.Location = new Point(263, 18);

                txtLastPlaylistName.Size = new Size(239, 24);
                picSaveLast100Playlist.Size = new Size(35, 30);
                picSaveLast100Playlist.Location = new Point(252, 14);
                    #endregion

                     
                dgPlaylistSch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //dgPlaylistSch.Columns["sName"].Width = 400;
                //dgPlaylistSch.Columns["StartDate"].Width = 120;
                //dgPlaylistSch.Columns["EndDate"].Width = 120;
                //dgPlaylistSch.Columns["Time"].Width = 100;
                //dgPlaylistSch.Columns["wDay"].Width = 100;
                EditPlaylistShedule.Width = 70;
                DeletePlaylistShedule.Width = 70;

                //dgPlaylist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                 //dgPlaylist.Columns["songname"].Width = 280;
                //dgPlaylist.Columns["Length"].Width = 80;
                //dgPlaylist.Columns["Album"].Width = 0;
                 //dgPlaylist.Columns["Artist"].Width = 150;

               if (dgOtherPlaylist.Rows.Count > 0)
                 {
                //    dgOtherPlaylist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                   //dgOtherPlaylist.Columns["songname"].Width = 280;
                //    dgOtherPlaylist.Columns["Length"].Width = 80;
                //    dgOtherPlaylist.Columns["Album"].Width = 0;
                 // dgOtherPlaylist.Columns["Artist"].Width = 150;
                 }

                //dgCommanGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
               //  dgCommanGrid.Columns["songname"].Width = 280;
                //dgCommanGrid.Columns["Length"].Width = 80;
               //  dgCommanGrid.Columns["aritstname"].Width = 150;
                //dgCommanGrid.Columns["Albumname"].Width = 130;
                SongDownload.Width = 30;
                 
                //dgStream.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                //dgStream.Columns["StreamName"].Width = 940;
                //Column_Img_Stream.Width = 40;

                dgSongRatingPlayerOne.Location = new Point(316, 29);
                dgSongRatingPlayerTwo.Location = new Point(316, 29);

                ////Advt Code/////////////

                panAdvtSchedule.Location = new Point(0, 0);
                panAdvtSchedule.Width = panAdvtDetail.Width;
                panAdvtSchedule.Height = panAdvtDetail.Height;

                panAdvtGrid.Height = panAdvtSchedule.Height - panButtonSettings.Height - panScheduleDate.Height - 40;
                dgAdvt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //////////////////////////
            }
            else
            {
                // dgPlaylist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //  dgOtherPlaylist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // dgCommanGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //dgStream.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                if (dgPlaylist.Rows.Count > 0)
                {
                    dgPlaylist.Columns["songname"].Width = 380;
                    dgPlaylist.Columns["Artist"].Width = 200;
                }
                if (dgOtherPlaylist.Rows.Count > 0)
                {
                    dgOtherPlaylist.Columns["songname"].Width = 380;
                    dgOtherPlaylist.Columns["Artist"].Width = 200;
                }
                if (dgCommanGrid.Rows.Count > 0)
                {
                    dgCommanGrid.Columns["songname"].Width = 380;
                    dgCommanGrid.Columns["aritstname"].Width = 200;
                }
                   

                SongDownload.Width = 50;
                 
                //Column_Img_Stream.Width = 40;
                ////Advt Code/////////////
                dgAdvt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                //////////////////////////


            }
            if (panManual.Visible == true)
            {
                panManual.Width = this.Width;
                panManual.Height = this.Height;
                panManual.Location = new Point(0, 0);
                webManual.Location = new Point(0, 0);
                webManual.Width = panPdf.Width - 10;
                webManual.Height = panPdf.Height - 30;
            }
            if (panManual.Visible == true)
            {
                panManual.Width = this.Width;
                panManual.Height = this.Height;
                panManual.Location = new Point(0, 0);
                webManual.Location = new Point(0, 0);
                webManual.Width = panPdf.Width - 10;
                webManual.Height = panPdf.Height - 30;
            }

            panAllStreamShedule.Width = this.Width;
            panAllStreamShedule.Height = this.Height;
            panAllStreamShedule.Location = new Point(0, 0);

            if (panStreamShedule.Visible == true)
            {
                panStreamShedule.Width = this.Width;
                panStreamShedule.Height = this.Height;
                panStreamShedule.Location = new Point(0, 0);
                if (this.Width.ToString() == "1020")
                {
                    dgStreamShedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                    dgStreamShedule.Columns["sName"].Width = 510;
                    dgStreamShedule.Columns["StartDate"].Width = 120;
                    dgStreamShedule.Columns["EndDate"].Width = 120;
                    dgStreamShedule.Columns["Time"].Width = 100;
                    EditStreamShedule.Width = 70;
                    DeleteStreamShedule.Width = 70;
                }
                else
                {
                    dgStreamShedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
            }
            }
            catch (Exception ex)
            {
            }

        }
        private void dgLocalPlaylist_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            if (e.ColumnIndex == 1)
            {
                if (e.RowIndex >= 0)
                {
                    string str45 = dgLocalPlaylist.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string[] arr = str45.Split('(');
                    txtPlaylistName.Text = arr[0].Trim();
                    ModifyPlaylistId = Convert.ToInt32(dgLocalPlaylist.Rows[e.RowIndex].Cells[0].Value);
                    pAction = "Modify";
                    txtPlaylistName.Focus();
                }
            }
        }
        #region StreamCode
        private void picStream_Click(object sender, EventArgs e)
        {
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            if ((musicPlayer1.URL != "") && (musicPlayer2.URL != "")) return;
            //FillStreamData();
            if (ClientsRightsValidation() == false) return;
            if (dgStream.Rows.Count == 0)
            {
                MessageBox.Show("The selected stream is not available at the moment.", "Copyright Player");
                return;
            }
            lblInternetStatus.Text = "10";
            lblStreamTime.Text = "";
            string strStream = "";
            DataTable dtGetSaveStream = new DataTable();


            if (musicPlayer1.URL != "")
            {
                musicPlayer1.Ctlcontrols.pause();
            }
            else if (musicPlayer2.URL != "")
            {
                musicPlayer2.Ctlcontrols.pause();
            }

            strStream = "select * from tbLastStream where tokenid= " + StaticClass.TokenId;
            dtGetSaveStream = ObjMainClass.fnFillDataTable_Local(strStream);
            if (dtGetSaveStream.Rows.Count > 0)
            {
                for (int i = 0; i <= dgStream.Rows.Count - 1; i++)
                {
                    if (Convert.ToInt32(dtGetSaveStream.Rows[0]["StreamId"]) == Convert.ToInt32(dgStream.Rows[i].Cells["streamId"].Value))
                    {
                        lblStreamName.Text = dgStream.Rows[i].Cells["StreamName"].Value.ToString();
                        StreamMusicPlayer.URL = dgStream.Rows[i].Cells["StreamLink"].Value.ToString();
                        dgStream.CurrentCell = dgStream.Rows[i].Cells["StreamName"];
                        dgStream.Rows[i].Selected = true;
                        dgStream.Rows[i].Cells["Column_Img_Stream"].Value = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
                        goto Done;
                    }
                }
            }
            lblStreamName.Text = dgStream.Rows[0].Cells["StreamName"].Value.ToString();
            StreamMusicPlayer.URL = dgStream.Rows[0].Cells["StreamLink"].Value.ToString();
            dgStream.Rows[0].Selected = true;
            dgStream.Rows[0].Cells["Column_Img_Stream"].Value = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
        Done:
            panStream.Visible = true;
            StreamMusicPlayer.Ctlcontrols.play();
            panStream.Location = new Point(0, 0);

            panStream.Height = this.Height;
            panStream.Width = this.Width;

            panStream.BringToFront();
            btnStreamPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
            btnStreamPlay.Text = "";
            //btnStreamMute.Text = "";
            // dgStream.Rows[0].Selected = true;
            GetCurrentStream();
            timStream.Enabled = true;
            timStreamMusicTime.Enabled = true;
        }
        private Boolean ClientsRightsValidation()
        {
            //if (StaticClass.StreamExpiryMessage == "NoLic")
            //{
            //    MessageBox.Show("!! Purchase the subscription of Online Streaming !!", "Copyright Player");
            //    return false;
            //}
            //else if (StaticClass.StreamExpiryMessage == "Yes")
            //{
            //    MessageBox.Show("!! Subscription is expire !!", "Copyright Player");
            //    return false;
            //}
            //else if (StaticClass.StreamExpiryMessage != "NoLic" && StaticClass.LeftStreamtDays == 0)
            //{
            //    lblStreamExpiry.Text = "Last day to renewal of subscription";
            //    return true;
            //}
            //else if (StaticClass.StreamExpiryMessage != "NoLic" && StaticClass.LeftStreamtDays <= 10)
            //{
            //    lblStreamExpiry.Text = Convert.ToString(StaticClass.LeftStreamtDays) + " days left to renewal of subscription";
            //    return true;
            //}

            return true;
        }
        private void InitilizeStreamGrid()
        {
            if (dgStream.Rows.Count > 0)
            {
                dgStream.Rows.Clear();
            }
            if (dgStream.Columns.Count > 0)
            {
                dgStream.Columns.Clear();
            }

            dgStream.Columns.Add("Streamid", "Stream Id");
            dgStream.Columns["Streamid"].Width = 0;
            dgStream.Columns["Streamid"].Visible = false;
            dgStream.Columns["Streamid"].ReadOnly = true;

            dgStream.Columns.Add("StreamLink", "Stream Link");
            dgStream.Columns["StreamLink"].Width = 0;
            dgStream.Columns["StreamLink"].Visible = false;
            dgStream.Columns["StreamLink"].ReadOnly = true;

            dgStream.Columns.Add("StreamName", "Stream Name");
            //dgStream.Columns["StreamName"].Width = 900;
            dgStream.Columns["StreamName"].Visible = true;
            dgStream.Columns["StreamName"].ReadOnly = true;
            dgStream.Columns["StreamName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgStream.Columns.Add(Column_Img_Stream);
            Column_Img_Stream.HeaderText = "";
            Column_Img_Stream.Name = "Column_Img_Stream";
            Column_Img_Stream.Width = 40;
            Column_Img_Stream.ImageLayout = DataGridViewImageCellLayout.Zoom;
            Column_Img_Stream.Visible = false;

            dgStream.Columns["Column_Img_Stream"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgStream.Columns["Column_Img_Stream"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
        }

        private void InitilizeStreamShe()
        {
            if (dgAllStreamShedule.Rows.Count > 0)
            {
                dgAllStreamShedule.Rows.Clear();
            }
            if (dgAllStreamShedule.Columns.Count > 0)
            {
                dgAllStreamShedule.Columns.Clear();
            }

            dgAllStreamShedule.Columns.Add("StreamName", "Stream Name");
            dgAllStreamShedule.Columns["StreamName"].Width = 200;
            dgAllStreamShedule.Columns["StreamName"].Visible = false;
            dgAllStreamShedule.Columns["StreamName"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("sDate", "sDate");
            dgAllStreamShedule.Columns["sDate"].Width = 100;
            dgAllStreamShedule.Columns["sDate"].Visible = false;
            dgAllStreamShedule.Columns["sDate"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("eDate", "eDate");
            dgAllStreamShedule.Columns["eDate"].Width = 100;
            dgAllStreamShedule.Columns["eDate"].Visible = true;
            dgAllStreamShedule.Columns["eDate"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("sTime", "sTime");
            dgAllStreamShedule.Columns["sTime"].Width = 100;
            dgAllStreamShedule.Columns["sTime"].Visible = true;
            dgAllStreamShedule.Columns["sTime"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("SheduleId", "SheduleId");
            dgAllStreamShedule.Columns["SheduleId"].Width = 0;
            dgAllStreamShedule.Columns["SheduleId"].Visible = false;
            dgAllStreamShedule.Columns["SheduleId"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("titlecategoryid", "titlecategoryid");
            dgAllStreamShedule.Columns["titlecategoryid"].Width = 0;
            dgAllStreamShedule.Columns["titlecategoryid"].Visible = false;
            dgAllStreamShedule.Columns["titlecategoryid"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("AdminCode", "AdminCode");
            dgAllStreamShedule.Columns["AdminCode"].Width = 0;
            dgAllStreamShedule.Columns["AdminCode"].Visible = false;
            dgAllStreamShedule.Columns["AdminCode"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("StreamId", "StreamId");
            dgAllStreamShedule.Columns["StreamId"].Width = 0;
            dgAllStreamShedule.Columns["StreamId"].Visible = false;
            dgAllStreamShedule.Columns["StreamId"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("dCode", "dCode");
            dgAllStreamShedule.Columns["dCode"].Width = 0;
            dgAllStreamShedule.Columns["dCode"].Visible = false;
            dgAllStreamShedule.Columns["dCode"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("CountryId", "CountryId");
            dgAllStreamShedule.Columns["CountryId"].Width = 0;
            dgAllStreamShedule.Columns["CountryId"].Visible = false;
            dgAllStreamShedule.Columns["CountryId"].ReadOnly = true;


            dgAllStreamShedule.Columns.Add("Stateid", "Stateid");
            dgAllStreamShedule.Columns["Stateid"].Width = 0;
            dgAllStreamShedule.Columns["Stateid"].Visible = false;
            dgAllStreamShedule.Columns["Stateid"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("CityId", "CityId");
            dgAllStreamShedule.Columns["CityId"].Width = 0;
            dgAllStreamShedule.Columns["CityId"].Visible = false;
            dgAllStreamShedule.Columns["CityId"].ReadOnly = true;

            dgAllStreamShedule.Columns.Add("TokenId", "TokenId");
            dgAllStreamShedule.Columns["TokenId"].Width = 0;
            dgAllStreamShedule.Columns["TokenId"].Visible = false;
            dgAllStreamShedule.Columns["TokenId"].ReadOnly = true;

            dgAllStreamShedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void FillStreamData()
        {
            string str;
            int iCtr;
            DataTable dtDetail;
          //  str = "Select * from  tblOnlineStreaming where dealercode='Copyright'    order by   streamName";
            str = "GetTokenWiseStreams " + StaticClass.UserId + " , " + StaticClass.TokenId + " ,'Copyright'";
            dtDetail = ObjMainClass.fnFillDataTable(str);

            //DataTable dtGetStreamShedule;
            //str = "GetStreamShedule " + StaticClass.TokenId + ", '" + StaticClass.DealerCode + "',4,'More000'," + StaticClass.AdvtCityId + ",'" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "'";
            //dtGetStreamShedule = ObjMainClass.fnFillDataTable(str);
            //InitilizeStreamShe();

            //if ((dtGetStreamShedule.Rows.Count > 0))
            //{
            //    for (iCtr = 0; (iCtr <= (dtGetStreamShedule.Rows.Count - 1)); iCtr++)
            //    {
            //        dgAllStreamShedule.Rows.Add();
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["StreamName"].Value = dtGetStreamShedule.Rows[iCtr]["StreamName"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["sDate"].Value = dtGetStreamShedule.Rows[iCtr]["sDate"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["eDate"].Value = dtGetStreamShedule.Rows[iCtr]["eDate"];

            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["sTime"].Value = dtGetStreamShedule.Rows[iCtr]["sTime"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["SheduleId"].Value = dtGetStreamShedule.Rows[iCtr]["SheduleId"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["titlecategoryid"].Value = dtGetStreamShedule.Rows[iCtr]["titlecategoryid"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["AdminCode"].Value = dtGetStreamShedule.Rows[iCtr]["AdminCode"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["StreamId"].Value = dtGetStreamShedule.Rows[iCtr]["StreamId"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["dCode"].Value = dtGetStreamShedule.Rows[iCtr]["dCode"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["CountryId"].Value = dtGetStreamShedule.Rows[iCtr]["CountryId"];

            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["Stateid"].Value = dtGetStreamShedule.Rows[iCtr]["Stateid"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["CityId"].Value = dtGetStreamShedule.Rows[iCtr]["CityId"];
            //        dgAllStreamShedule.Rows[dgAllStreamShedule.Rows.Count - 1].Cells["TokenId"].Value = dtGetStreamShedule.Rows[iCtr]["TokenId"];
            //    }
            //}

            //dgAllStreamShedule.DataSource = dtGetStreamShedule;
            //dgAllStreamShedule.Columns[4].Visible = false;
            //dgAllStreamShedule.Columns[5].Visible = false;
            //dgAllStreamShedule.Columns[6].Visible = false;
            //dgAllStreamShedule.Columns[7].Visible = false;
            //dgAllStreamShedule.Columns[8].Visible = false;
            //dgAllStreamShedule.Columns[9].Visible = false;
            //dgAllStreamShedule.Columns[10].Visible = false;
            //dgAllStreamShedule.Columns[11].Visible = false;
            //dgAllStreamShedule.Columns[12].Visible = false;


            InitilizeStreamGrid();
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgStream.Rows.Add();
                    dgStream.Rows[dgStream.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["StreamId"];
                    dgStream.Rows[dgStream.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["StreamLink"];
                    dgStream.Rows[dgStream.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["StreamName"];
                    dgStream.Rows[dgStream.Rows.Count - 1].Cells["Column_Img_Stream"].Value = ((System.Drawing.Image)(Properties.Resources.NextData_Hover));
                    //       dgStream.Rows[dgStream.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                }

            }
            foreach (DataGridViewRow row in dgStream.Rows)
            {
                if (this.Width.ToString() == "1020")
                {
                    row.Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    row.Height = 33;
                }
                else
                {
                    row.Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    row.Height = 35;
                }
            }

        }


        private void dgStream_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            PrvTimeStreamPlayer = 0;
            IsFirtTimeStreamComplete = false;

            lblStreamName.Text = dgStream.Rows[e.RowIndex].Cells[2].Value.ToString();
            StreamMusicPlayer.URL = dgStream.Rows[e.RowIndex].Cells[1].Value.ToString();
            StreamMusicPlayer.Ctlcontrols.play();
            timStream.Enabled = true;
            timStreamMusicTime.Enabled = true;
            dgStream.Rows[e.RowIndex].Selected = true;
            dgStream.Rows[e.RowIndex].Cells["Column_Img_Stream"].Value = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
            if (btnStreamPlay.Text == ".")
            {
                btnStreamPlay.Text = "";
                btnStreamPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
            }
            string sWr = "";
            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            sWr = "Delete from tbLastStream where tokenid= " + Convert.ToInt32(StaticClass.TokenId);
            OleDbCommand cmdDel = new OleDbCommand();
            cmdDel.Connection = StaticClass.LocalCon;
            cmdDel.CommandText = sWr;
            cmdDel.ExecuteNonQuery();

            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            sWr = "insert into tbLastStream values (" + Convert.ToInt32(StaticClass.TokenId) + " , ";
            sWr = sWr + Convert.ToInt32(dgStream.Rows[dgStream.CurrentCell.RowIndex].Cells["streamId"].Value) + ")";
            OleDbCommand cmdSave = new OleDbCommand();
            cmdSave.Connection = StaticClass.LocalCon;
            cmdSave.CommandText = sWr;
            cmdSave.ExecuteNonQuery();
            GetCurrentStream();
        }


        private void btnStreamPlay_Click(object sender, EventArgs e)
        {
            if (StreamMusicPlayer.URL != "")
            {
                if (btnStreamPlay.Text == ".")
                {
                    btnStreamPlay.Text = "";
                    StreamMusicPlayer.Ctlcontrols.play();
                    StreamMusicPlayer.settings.volume = 100;
                    btnStreamPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
                    timStreamMusicTime.Enabled = true;
                    picWaveOne.Visible = true;
                    picWaveTwo.Visible = true;
                }
                else if (btnStreamPlay.Text == "")
                {
                    btnStreamPlay.Text = ".";
                    StreamMusicPlayer.Ctlcontrols.pause();
                    btnStreamPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
                    timStreamMusicTime.Enabled = false;
                    picWaveOne.Visible = false;
                    picWaveTwo.Visible = false;
                }
            }
        }



        private void btnStreamMute_Click(object sender, EventArgs e)
        {
            if (StreamMusicPlayer.URL != "")
            {
                if (btnStreamMute.Text == "")
                {
                    btnStreamMute.Text = ".";
                    StreamMusicPlayer.settings.mute = true;
                    btnStreamMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_red));
                }
                else if (btnStreamMute.Text == ".")
                {
                    btnStreamMute.Text = "";
                    StreamMusicPlayer.settings.mute = false;
                    btnStreamMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_blue));
                }
            }
        }
        private void GetCurrentStream()
        {
            for (int i = 0; i < dgStream.Rows.Count; i++)
            {
                if (dgStream.Rows[i].Cells[2].Value.ToString() != lblStreamName.Text.Trim())
                {
                    dgStream.Rows[i].Selected = false;
                    dgStream.Rows[i].Cells["Column_Img_Stream"].Value = ((System.Drawing.Image)(Properties.Resources.NextData_Hover));
                }
            }
        }


        private void picSwitchOff_Click(object sender, EventArgs e)
        {
            //string sWr = "";
            //if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            //StaticClass.LocalCon.Open();
            //sWr = "Delete from tbLastStream where tokenid= " + Convert.ToInt32(StaticClass.TokenId);
            //OleDbCommand cmdDel = new OleDbCommand();
            //cmdDel.Connection = StaticClass.LocalCon;
            //cmdDel.CommandText = sWr;
            //cmdDel.ExecuteNonQuery();

            //if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            //StaticClass.LocalCon.Open();
            //sWr = "insert into tbLastStream values (" + Convert.ToInt32(StaticClass.TokenId) + " , ";
            //sWr = sWr + Convert.ToInt32(dgStream.Rows[dgStream.CurrentCell.RowIndex].Cells["streamId"].Value) + ")";
            //OleDbCommand cmdSave = new OleDbCommand();
            //cmdSave.Connection = StaticClass.LocalCon;
            //cmdSave.CommandText = sWr;
            //cmdSave.ExecuteNonQuery();

            if (musicPlayer1.URL != "")
            {
                if (btnPlay.Text == "")
                {
                    musicPlayer1.Ctlcontrols.play();
                }
            }
            else if (musicPlayer2.URL != "")
            {
                if (btnPlay.Text == "")
                {
                    musicPlayer2.Ctlcontrols.play();
                }
            }
            panStream.Visible = false;
            StreamMusicPlayer.Ctlcontrols.stop();
            StreamMusicPlayer.URL = "";
            timStreamMusicTime.Enabled = false;
            timGetNetState.Enabled = false;
        }


        private void timStream_Tick(object sender, EventArgs e)
        {
            pictureBox2.Visible = true;
            picWaveOne.Visible = false;
            picWaveTwo.Visible = false;
        }

        private void timStreamMusicTime_Tick(object sender, EventArgs e)
        {
            lblStreamTime.Text = StreamMusicPlayer.Ctlcontrols.currentPositionString;
            if (musicPlayer1.URL != "")
            {
                musicPlayer1.Ctlcontrols.pause();
            }
            else if (musicPlayer2.URL != "")
            {
                musicPlayer2.Ctlcontrols.pause();
            }
            if (IsFirtTimeStreamComplete == true)
            {
                TimeStreamPlayer = Math.Floor(StreamMusicPlayer.Ctlcontrols.currentPosition) - PrvTimeStreamPlayer;
            }
            else
            {
                PrvTimeStreamPlayer = Math.Floor(StreamMusicPlayer.Ctlcontrols.currentPosition);
                TimeStreamPlayer = Math.Floor(StreamMusicPlayer.Ctlcontrols.currentPosition);
            }
            if (lblStreamTime.Text != "")
            {
                pictureBox2.Visible = false;
                picWaveOne.Visible = true;
                picWaveTwo.Visible = true;
                timStream.Enabled = false;
            }
            if (lblSongCount.Text == "2")
            {
                PrvTimeStreamPlayer = Math.Floor(StreamMusicPlayer.Ctlcontrols.currentPosition);
                IsFirtTimeStreamComplete = true;
                lblPlayerName.Text = "Stream";
                StreamMusicPlayer.Ctlcontrols.pause();
                panAdvt.Height = 152;
                panAdvt.Visible = true;
                if (panStreamShedule.Visible == false & panAdvtDetail.Visible == false & panAllStreamShedule.Visible == false & panManual.Visible == false & panPrayer.Visible == false)
                {
                    panAdvt.BringToFront();
                }

            }
            if (btnStreamMute.Text == "")
            {
                StreamMusicPlayer.settings.mute = false;
            }
            else if (btnStreamMute.Text == ".")
            {
                StreamMusicPlayer.settings.mute = true;
            }
            lblInternetStatus.Text = (Convert.ToInt32(lblInternetStatus.Text) - 1).ToString();

            if (lblInternetStatus.Text == "0")
            {


                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                     
                    
                    if (musicPlayer1.URL != "")
                    {
                        musicPlayer1.settings.volume = 100;
                        if (btnPlay.Text == "")
                        {
                            if (panAdvt.Visible == true)
                            {
                                lblPlayerName.Text = "Two";
                                musicPlayer1.Ctlcontrols.pause();
                            }
                            else
                            {
                                musicPlayer1.Ctlcontrols.play();
                            }
                        }
                    }
                    else if (musicPlayer2.URL != "")
                    {
                        musicPlayer2.settings.volume = 100;
                        if (btnPlay.Text == "")
                        {
                            if (panAdvt.Visible == true)
                            {
                                lblPlayerName.Text = "One";
                                musicPlayer2.Ctlcontrols.pause();
                            }
                            else
                            {
                                musicPlayer2.Ctlcontrols.play();
                            }
                        }
                    }

                     

                    panStream.Visible = false;
                    StreamMusicPlayer.Ctlcontrols.stop();
                    StreamMusicPlayer.URL = "";
                    timStreamMusicTime.Enabled = false;
                    timResetSong.Enabled = true;

                    timGetNetState.Enabled = true;
                    ReNetStateTime = 180;
                    return;
                }
                else if (StreamMusicPlayer.playState == WMPPlayState.wmppsReady)
                {
                     
                    musicPlayer1.settings.volume = 100;
                    if (musicPlayer1.URL != "")
                    {
                        if (btnPlay.Text == "")
                        {
                            musicPlayer1.Ctlcontrols.play();
                        }
                    }
                    else if (musicPlayer2.URL != "")
                    {
                        if (btnPlay.Text == "")
                        {
                            musicPlayer2.Ctlcontrols.play();
                        }
                    }

                   
                    panStream.Visible = false;
                    StreamMusicPlayer.Ctlcontrols.stop();
                    StreamMusicPlayer.URL = "";
                    timStreamMusicTime.Enabled = false;
                    timResetSong.Enabled = true;

                    timGetNetState.Enabled = true;
                    ReNetStateTime = 180;
                    return;
                }
                else
                {
                    lblInternetStatus.Text = "10";
                }
            }
        }
        #endregion


        private void DecrpetSec(Int32 Title_Song_Id)
        {
            try
            {
                using (MemoryStream st = amcrypt.getStream(Title_Song_Id))
                {
                    long length = st.Length;
                    byte[] data = new byte[length];
                    st.Read(data, 0, (int)length);
                    FileStream fs = new FileStream(Application.StartupPath + "//" + Title_Song_Id + ".mp3", FileMode.Create);
                    fs.Write(data, 0, (int)length);
                    fs.Flush();
                    fs.Close();
                }
            }
            catch (Exception ex) { }
        }

        private void mainwindow_Validating(object sender, CancelEventArgs e)
        {
            drawLine = false;
            dgPlaylist.Invalidate();
        }

        #region SongRating
        private void FillStar(DataGridView GridName)
        {
            if (GridName.Rows.Count > 0)
            {
                GridName.Rows.Clear();
            }
            if (GridName.Columns.Count > 0)
            {
                GridName.Columns.Clear();
            }

            DataGridViewImageColumn Star1 = new DataGridViewImageColumn();
            Star1.HeaderText = "Star1";
            Star1.Name = "Star1";
            GridName.Columns.Add(Star1);
            Star1.Width = 20;
            Star1.ImageLayout = DataGridViewImageCellLayout.Stretch;

            DataGridViewImageColumn Star2 = new DataGridViewImageColumn();
            Star2.HeaderText = "Star2";
            Star2.Name = "Star2";
            GridName.Columns.Add(Star2);
            Star2.Width = 20;
            Star2.ImageLayout = DataGridViewImageCellLayout.Stretch;

            DataGridViewImageColumn Star3 = new DataGridViewImageColumn();
            Star3.HeaderText = "Star3";
            Star3.Name = "Star3";
            GridName.Columns.Add(Star3);
            Star3.Width = 20;
            Star3.ImageLayout = DataGridViewImageCellLayout.Stretch;

            DataGridViewImageColumn Star4 = new DataGridViewImageColumn();
            Star4.HeaderText = "Star4";
            Star4.Name = "Star4";
            GridName.Columns.Add(Star4);
            Star4.Width = 20;
            Star4.ImageLayout = DataGridViewImageCellLayout.Stretch;

            DataGridViewImageColumn Star5 = new DataGridViewImageColumn();
            GridName.Columns.Add(Star5);
            Star5.HeaderText = "Star5";
            Star5.Name = "Star5";
            Star5.Width = 20;
            Star5.ImageLayout = DataGridViewImageCellLayout.Stretch;
        }
        private void dgSongRatingPlayerOne_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (musicPlayer1.URL != "")
            {
                Int32 TotalStar = e.ColumnIndex;
                Image StarON;
                StarON = Properties.Resources.starON;
                Image OffStar;
                OffStar = Properties.Resources.starOFF;
                for (int i = 0; i <= 4; i++)
                {
                    if (i <= TotalStar)
                    {
                        dgSongRatingPlayerOne.Rows[0].Cells[i].Value = StarON;
                    }
                    else
                    {
                        dgSongRatingPlayerOne.Rows[0].Cells[i].Value = OffStar;
                    }
                }
            }
        }
        private void GetSavedRating(string titleID, DataGridView GridName)
        {

            try
            {
                DataTable dtRating = new DataTable();
                string str = "";

                Image StarON;
                StarON = Properties.Resources.starON;

                Image OffStar;
                OffStar = Properties.Resources.starOFF;
                FillStar(GridName);
                GridName.Rows.Add();
                str = "select * from tbTitleRating where tokenid=" + StaticClass.TokenId + "  and titleid= " + titleID;


                dtRating = ObjMainClass.fnFillDataTable_Local(str);

                if (dtRating.Rows.Count > 0)
                {
                    GridName.GridColor = Color.FromArgb(25, 146, 166);
                    Int32 TotalStar = Convert.ToInt32(dtRating.Rows[0]["TitleRating"]);

                    for (int i = 0; i <= 4; i++)
                    {
                        if (i <= TotalStar)
                        {
                            GridName.Rows[0].Cells[i].Value = StarON;
                        }
                        else
                        {
                            GridName.Rows[0].Cells[i].Value = OffStar;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= 4; i++)
                    {
                        GridName.Rows[0].Cells[i].Value = OffStar;
                        GridName.GridColor = Color.FromArgb(25, 146, 166);
                    }
                }
            }
            catch (Exception ex) { }
        }
        private void SetDisableRating(DataGridView GridName)
        {
            try
            {
                Image StarDisable;
                StarDisable = Properties.Resources.StarDisable;
                FillStar(GridName);
                GridName.Rows.Add();
                GridName.GridColor = Color.FromArgb(175, 175, 175);
                for (int i = 0; i <= 4; i++)
                {
                    GridName.Rows[0].Cells[i].Value = StarDisable;
                }
            }
            catch (Exception ex) { }
        }
        private void SetRating(DataGridView GridName)
        {
            try
            {
                Image StarDisable;
                StarDisable = Properties.Resources.starOFF;
                FillStar(GridName);
                GridName.Rows.Add();
                GridName.GridColor = Color.FromArgb(25, 146, 166);
                for (int i = 0; i <= 4; i++)
                {
                    GridName.Rows[0].Cells[i].Value = StarDisable;
                }
            }
            catch (Exception ex) { }
        }
        private void dgSongRatingPlayerOne_MouseLeave(object sender, EventArgs e)
        {
            string g = musicPlayer1.URL;
            if (musicPlayer1.URL.ToString() != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                GetSavedRating(g, dgSongRatingPlayerOne);
            }

        }

        private void dgSongRatingPlayerOne_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string g = musicPlayer1.URL;
            if (musicPlayer1.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                SaveRating(e.ColumnIndex, Convert.ToInt32(g));
            }

        }
        private void SaveRating(Int32 TitleRating, Int32 RatingTitleId)
        {
            string strInsertrating = "";
            try
            {
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    MessageBox.Show("Please check your Internet connection.", "music player");
                    return;
                }
                ////////////// Save Online Data ////////////////
                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand("spTitleRating", StaticClass.constr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
                cmd.Parameters["@TokenId"].Value = StaticClass.TokenId;
                cmd.Parameters.Add(new SqlParameter("@TitleId", SqlDbType.BigInt));
                cmd.Parameters["@TitleId"].Value = RatingTitleId;
                cmd.Parameters.Add(new SqlParameter("@TitleRating", SqlDbType.BigInt));
                cmd.Parameters["@TitleRating"].Value = TitleRating;
               // cmd.ExecuteNonQuery();

                ////////////// Save Local Data ////////////////

                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                strInsertrating = "delete from tbTitleRating where tokenid=" + StaticClass.TokenId + " and titleId= " + RatingTitleId + "";
                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitleDelete = new OleDbCommand();
                cmdTitleDelete.Connection = StaticClass.LocalCon;
                cmdTitleDelete.CommandText = strInsertrating;
                cmdTitleDelete.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
                /////////////////////////////////////////////////////////////
                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                strInsertrating = "insert into tbTitleRating values (" + StaticClass.TokenId + " , " + RatingTitleId + " , ";
                strInsertrating = strInsertrating + TitleRating + ")";

                StaticClass.LocalCon.Open();
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsertrating;
                cmdTitle.ExecuteNonQuery();
                StaticClass.LocalCon.Close();

                string g = "";
                if (musicPlayer1.URL != "")
                {
                    g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                    GetSavedRating(g, dgSongRatingPlayerOne);
                }
                if (musicPlayer2.URL != "")
                {
                    g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                    GetSavedRating(g, dgSongRatingPlayerTwo);
                }
            }
            catch (Exception ex) { }
        }

        private void dgSongRatingPlayerTwo_MouseLeave(object sender, EventArgs e)
        {
            if (musicPlayer2.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                GetSavedRating(g, dgSongRatingPlayerTwo);
            }
        }

        private void dgSongRatingPlayerTwo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (musicPlayer2.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                SaveRating(e.ColumnIndex, Convert.ToInt32(g));
            }
        }

        private void dgSongRatingPlayerTwo_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (musicPlayer2.URL != "")
            {
                Int32 TotalStar = e.ColumnIndex;
                Image StarON;
                StarON = Properties.Resources.starON;
                Image OffStar;
                OffStar = Properties.Resources.starOFF;
                for (int i = 0; i <= 4; i++)
                {
                    if (i <= TotalStar)
                    {
                        dgSongRatingPlayerTwo.Rows[0].Cells[i].Value = StarON;
                    }
                    else
                    {
                        dgSongRatingPlayerTwo.Rows[0].Cells[i].Value = OffStar;
                    }
                }
            }
        }
        private void InitilizeTop250Grid()
        {
            if (dgTop250.Rows.Count > 0)
            {
                dgTop250.Rows.Clear();
            }
            if (dgTop250.Columns.Count > 0)
            {
                dgTop250.Columns.Clear();
            }

            dgTop250.Columns.Add("songid", "song Id");
            dgTop250.Columns["songid"].Width = 0;
            dgTop250.Columns["songid"].Visible = false;
            dgTop250.Columns["songid"].ReadOnly = true;

            dgTop250.Columns.Add("songname", "Title");
            dgTop250.Columns["songname"].Width = 170;
            dgTop250.Columns["songname"].Visible = true;
            dgTop250.Columns["songname"].ReadOnly = true;

            dgTop250.Columns.Add("Artist", "Artist");
            dgTop250.Columns["Artist"].Width = 103;
            dgTop250.Columns["Artist"].Visible = true;
            dgTop250.Columns["Artist"].ReadOnly = true;
        }
        private void FillTop250Grid()
        {
            int iCtr;
            string str = "SELECT TOP 250 Titles.TitleID , Titles.Title as Title, Artists.Name as ArtistName ";
            str = str + " FROM (Titles INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID) INNER JOIN tbTitleRating ON Titles.TitleID = tbTitleRating.TitleId  ";
            str = str + " where  tbTitleRating.tokenid=" + StaticClass.TokenId + " order by tbTitleRating.titlerating desc ";
            try
            {
                InitilizeTop250Grid();
                DataTable dtDetail;
                dtDetail = ObjMainClass.fnFillDataTable_Local(str);
                if ((dtDetail.Rows.Count > 0))
                {
                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {

                        dgTop250.Rows.Add();
                        dgTop250.Rows[dgTop250.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["TitleID"];
                        dgTop250.Rows[dgTop250.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["Title"];
                        dgTop250.Rows[dgTop250.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["ArtistName"];

                        dgTop250.Rows[dgTop250.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        dgTop250.Rows[dgTop250.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);

                    }
                    foreach (DataGridViewRow row in dgTop250.Rows)
                    {
                        row.Height = 33;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }




        private void dgTop250_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                drawLine = true;
                RowSelect(dgTop250, dgTop250.Rows[e.RowIndex].Cells[0].Value.ToString());
                dgTop250.DoDragDrop(dgTop250.Rows[e.RowIndex].Cells[0].Value.ToString(), DragDropEffects.Copy);
                Is_Drop = true;
            }
        }
        #endregion

        private void DisplaySongPlayerOne()
        {
            try
            {
                string mlsSql = "";
                string currentFileName;
                var Special_Name = "";
                string Special_Change = "";
               string g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                currentFileName = g;
                mlsSql = "SELECT  Titles.Title as songname, Albums.Name as AlbumsName, Artists.Name AS ArtistsName FROM ( Albums INNER JOIN Titles ON Albums.AlbumID = Titles.AlbumID ) INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID where Titles.titleid=" + Convert.ToInt32(currentFileName);
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet_Local(mlsSql);

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["songname"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblSongName.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["ArtistsName"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblArtistName.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["AlbumsName"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                //lblalbumName.Text = Special_Change;
            }
            catch (Exception ex)
            {
            }
        }
        private void DisplaySongPlayerTwo()
        {
            try
            {

                string mlsSql = "";
                string currentFileName;
                var Special_Name = "";
                string Special_Change = "";
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                currentFileName = g;
                mlsSql = "SELECT  Titles.Title as songname, Albums.Name as AlbumsName, Artists.Name AS ArtistsName FROM ( Albums INNER JOIN Titles ON Albums.AlbumID = Titles.AlbumID ) INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID where Titles.titleid=" + Convert.ToInt32(currentFileName);
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet_Local(mlsSql);

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["songname"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblSongName2.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["ArtistsName"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblArtistName2.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = ds.Tables[0].Rows[0]["AlbumsName"].ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                //lblAlbumName2.Text = Special_Change;
                //30-08,06-09,13-09,27-09
            }
            catch (Exception ex)
            {
            }

        }


        #region "Last100"


        private void InitilizeLast100Grid()
        {
            if (dgLast100.Rows.Count > 0)
            {
                dgLast100.Rows.Clear();
            }
            if (dgLast100.Columns.Count > 0)
            {
                dgLast100.Columns.Clear();
            }

            dgLast100.Columns.Add("songid", "song Id");
            dgLast100.Columns["songid"].Width = 0;
            dgLast100.Columns["songid"].Visible = false;
            dgLast100.Columns["songid"].ReadOnly = true;

            dgLast100.Columns.Add("songname", "Title");
            dgLast100.Columns["songname"].Width = 170;
            dgLast100.Columns["songname"].Visible = true;
            dgLast100.Columns["songname"].ReadOnly = true;
            dgLast100.Columns["songname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgLast100.Columns.Add("Artist", "Artist");
            dgLast100.Columns["Artist"].Width = 103;
            dgLast100.Columns["Artist"].Visible = true;
            dgLast100.Columns["Artist"].ReadOnly = true;
            dgLast100.Columns["Artist"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgLast100.Columns.Add("Length", "Length");
            dgLast100.Columns["Length"].Width = 0;
            dgLast100.Columns["Length"].Visible = false;
            dgLast100.Columns["Length"].ReadOnly = true;
        }
        private void FillLast100Grid()
        {
            string Special_Name = "";
            string Special_Change = "";
            string TitleTime = "";
            int iCtr;
            string str = "SELECT Titles.TitleID , ltrim(Titles.Title) as Title, Artists.Name as ArtistName, Titles.Time ";
            str = str + " FROM ((Titles  INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID )";
            str = str + " INNER JOIN tbLast100 ON Titles.titleid = tbLast100.titleid )";
            str = str + " order by tbLast100.SrNo desc";
            try
            {
                InitilizeLast100Grid();
                DataTable dtDetail;
                dtDetail = ObjMainClass.fnFillDataTable_Local(str);
                if ((dtDetail.Rows.Count > 0))
                {
                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {

                        dgLast100.Rows.Add();
                        dgLast100.Rows[dgLast100.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["TitleID"];

                        Special_Name = "";
                        Special_Change = "";
                        Special_Name = dtDetail.Rows[iCtr]["Title"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgLast100.Rows[dgLast100.Rows.Count - 1].Cells[1].Value = Special_Change;


                        Special_Name = "";
                        Special_Change = "";
                        Special_Name = dtDetail.Rows[iCtr]["ArtistName"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgLast100.Rows[dgLast100.Rows.Count - 1].Cells[2].Value = Special_Change;


                        string strTime = dtDetail.Rows[iCtr]["Time"].ToString();
                        string[] arr = strTime.Split(':');
                        TitleTime = arr[1] + ":" + arr[2];

                        dgLast100.Rows[dgLast100.Rows.Count - 1].Cells[3].Value = TitleTime;

                        dgLast100.Rows[dgLast100.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        dgLast100.Rows[dgLast100.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);

                    }
                    foreach (DataGridViewRow row in dgLast100.Rows)
                    {
                        row.Height = 33;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SaveLast100()
        {
            Int32 LocalTitleId = 0;
            Int32 MaxTableID = 0;
            Int32 TotalRecords = 0;
            Int32 OverRecords = 0;
            try
            {

                string strTotal = "SELECT count(*) from tbLast100";
                DataTable dtGetTotal;
                dtGetTotal = ObjMainClass.fnFillDataTable_Local(strTotal);
                if ((dtGetTotal.Rows.Count > 0))
                {
                    TotalRecords = Convert.ToInt32(dtGetTotal.Rows[0][0]);
                }

                if (TotalRecords >= 100)
                {
                    OverRecords = TotalRecords - 99;
                    if (OverRecords > 0)
                    {

                        string strGetRec = "SELECT top " + OverRecords + " * from tbLast100 order by srno";
                        DataTable dtGetOver;
                        dtGetOver = ObjMainClass.fnFillDataTable_Local(strGetRec);
                        if ((dtGetOver.Rows.Count > 0))
                        {
                            for (int iCtr = 0; (iCtr <= (dtGetOver.Rows.Count - 1)); iCtr++)
                            {

                                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                                StaticClass.LocalCon.Open();
                                OleDbCommand cmdLast = new OleDbCommand();
                                cmdLast.Connection = StaticClass.LocalCon;
                                cmdLast.CommandText = "delete from tbLast100 where TitleId =" + Convert.ToInt32(dtGetOver.Rows[iCtr][1]); ;
                                cmdLast.ExecuteNonQuery();
                                StaticClass.LocalCon.Close();
                            }
                        }
                    }


                }
                string g = "";
                if (musicPlayer1.URL != "")
                {
                    g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                    LocalTitleId = Convert.ToInt32(g);
                }
                else if (musicPlayer2.URL != "")
                {
                    g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                    LocalTitleId = Convert.ToInt32(g);
                }




                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                StaticClass.LocalCon.Open();
                OleDbCommand cmdDelLast = new OleDbCommand();
                cmdDelLast.Connection = StaticClass.LocalCon;
                cmdDelLast.CommandText = "delete from tbLast100 where TitleId =" + LocalTitleId;
                cmdDelLast.ExecuteNonQuery();
                StaticClass.LocalCon.Close();

                DataTable dtGetMaxId;
                dtGetMaxId = ObjMainClass.fnFillDataTable_Local("SELECT iif(IsNull(max(srNo)),0,(max(srNo))) + 1 FROM tbLast100");
                MaxTableID = Convert.ToInt32(dtGetMaxId.Rows[0][0]);

                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                StaticClass.LocalCon.Open();
                OleDbCommand cmdSaveLast = new OleDbCommand();
                cmdSaveLast.Connection = StaticClass.LocalCon;
                cmdSaveLast.CommandText = "insert into tbLast100 (Srno, Titleid) values (" + MaxTableID + ", " + LocalTitleId + ")";
                cmdSaveLast.ExecuteNonQuery();
                StaticClass.LocalCon.Close();


                FillLast100Grid();
            }
            catch (Exception ex) { }
        }
        private void dgLast100_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2)
            {
                drawLine = false;
                RowSelect(dgLast100, dgLast100.Rows[e.RowIndex].Cells[0].Value.ToString());
                dgLast100.DoDragDrop(dgLast100.Rows[e.RowIndex].Cells[0].Value.ToString(), DragDropEffects.Copy);
                Is_Drop = true;
            }
        }

        private void dgLast100_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                StaticClass.LocalCon.Open();
                OleDbCommand cmdDelLast = new OleDbCommand();
                cmdDelLast.Connection = StaticClass.LocalCon;
                cmdDelLast.CommandText = "delete from tbLast100 where TitleId =" + dgLast100.Rows[dgLast100.CurrentCell.RowIndex].Cells[0].Value;
                cmdDelLast.ExecuteNonQuery();

                FillLast100Grid();
            }
        }
        private void picSaveLast100Playlist_Click(object sender, EventArgs e)
        {
            timResetSong.Stop();
            IsLast100Working = "Yes";
            if (txtLastPlaylistName.Text.Trim() == "")
            {
                MessageBox.Show("Playlist name cannot be blank", "Music Player");
                txtLastPlaylistName.Focus();
                return;
            }
            DefaultPlaylistSave(txtLastPlaylistName.Text.Trim());
            txtLastPlaylistName.Text = "";
            gblsongid = "";
            for (int i = 0; i < dgLast100.Rows.Count; i++)
            {
                insert_Playlist_song(dgLast100.Rows[i].Cells[0].Value.ToString(), "No", false);
                //gblsongid = dgLast100.Rows[i].Cells[0].Value.ToString();
                //Thread t2 = new Thread(Insert_Last100Playlist_songs);
                //t2.IsBackground = true;
                //t2.Start();
            }
            FillLocalPlaylist();
            IsLast100Working = "No";
            timResetSong.Start();
        }
        void Insert_Last100Playlist_songs()
        {
            try
            {
                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand("InsertTitlesInPlayLists", StaticClass.constr);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@PlayListID", SqlDbType.BigInt));
                cmd.Parameters["@PlayListID"].Value = StaticClass.Last100PlaylistId;
                cmd.Parameters.Add(new SqlParameter("@TitleID", SqlDbType.BigInt));
                cmd.Parameters["@TitleID"].Value = gblsongid;
                cmd.ExecuteNonQuery();
                StaticClass.constr.Close();
                Insert_Last100Playlist_song_LocalDatabase();
            }
            catch (Exception ex)
            {
            }
        }
        void Insert_Last100Playlist_song_LocalDatabase()
        {
            string sWr = "";
            var Special_Name = "";
            string Special_Change = "";
            int Playlist_Id = 0;
            Playlist_Id = StaticClass.Last100PlaylistId;
            Int32 AlbumID = 0;
            Int32 ArtistID = 0;
            string sQr = "";
            DataSet dsAlbum = new DataSet();
            try
            {
                sQr = "select * from Titles where TitleID=" + gblsongid;
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet_Local(sQr);
                if (ds.Tables[0].Rows.Count <= 0)
                {

                    sQr = "select TitleID,AlbumID,ArtistID,Title,Gain,isnull(TitleYear,0) as TitleYear,Time from Titles where TitleID=" + gblsongid;
                    DataSet dsTitle = new DataSet();
                    dsTitle = ObjMainClass.fnFillDataSet(sQr);
                    AlbumID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["AlbumID"]);
                    ArtistID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["ArtistID"]);
                    Special_Name = dsTitle.Tables[0].Rows[0]["Title"].ToString();
                    Special_Change = Special_Name.Replace("'", "??$$$??");

                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    StaticClass.LocalCon.Open();
                    sWr = "insert into Titles values (" + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["TitleID"]) + " , " + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["AlbumID"]) + " , ";
                    sWr = sWr + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["ArtistID"]) + ", '" + Special_Change + "' , ";
                    sWr = sWr + "'" + dsTitle.Tables[0].Rows[0]["Gain"] + "' , '" + dsTitle.Tables[0].Rows[0]["Time"] + "' ,";
                    sWr = sWr + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["TitleYear"]) + ")";
                    OleDbCommand cmdTitle = new OleDbCommand();
                    cmdTitle.Connection = StaticClass.LocalCon;
                    cmdTitle.CommandText = sWr;
                    cmdTitle.ExecuteNonQuery();
                    StaticClass.LocalCon.Close();
                }
                else
                {
                    sQr = "select TitleID,AlbumID,ArtistID,Title,Gain,isnull(TitleYear,0) as TitleYear,Time from Titles where TitleID=" + gblsongid;
                    DataSet dsTitle = new DataSet();
                    dsTitle = ObjMainClass.fnFillDataSet(sQr);
                    AlbumID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["AlbumID"]);
                    ArtistID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["ArtistID"]);

                }
                Special_Name = "";
                Special_Change = "";
                sQr = "select * from Albums where albumid=" + Convert.ToInt32(AlbumID);
                DataSet dsAlbumLocal = new DataSet();
                dsAlbumLocal = ObjMainClass.fnFillDataSet_Local(sQr);
                if (dsAlbumLocal.Tables[0].Rows.Count <= 0)
                {
                    sQr = "select * from Albums where albumid=" + Convert.ToInt32(AlbumID);
                    dsAlbum = ObjMainClass.fnFillDataSet(sQr);

                    Special_Name = dsAlbum.Tables[0].Rows[0]["Name"].ToString();
                    Special_Change = Special_Name.Replace("'", "??$$$??");

                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    StaticClass.LocalCon.Open();
                    sWr = "insert into Albums values (" + Convert.ToInt32(dsAlbum.Tables[0].Rows[0]["AlbumID"]) + " , ";
                    sWr = sWr + Convert.ToInt32(dsAlbum.Tables[0].Rows[0]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                    OleDbCommand cmdAlbum = new OleDbCommand();
                    cmdAlbum.Connection = StaticClass.LocalCon;
                    cmdAlbum.CommandText = sWr;
                    cmdAlbum.ExecuteNonQuery();
                    StaticClass.LocalCon.Close();
                }
                Special_Name = "";
                Special_Change = "";

                sQr = "select * from Artists where ArtistID=" + Convert.ToInt32(ArtistID);
                DataSet dsArtistLocal = new DataSet();
                dsArtistLocal = ObjMainClass.fnFillDataSet_Local(sQr);
                if (dsArtistLocal.Tables[0].Rows.Count <= 0)
                {
                    sQr = "select * from Artists where ArtistID=" + Convert.ToInt32(ArtistID);
                    DataSet dsArtist = new DataSet();
                    dsArtist = ObjMainClass.fnFillDataSet(sQr);
                    Special_Name = dsArtist.Tables[0].Rows[0]["Name"].ToString();
                    Special_Change = Special_Name.Replace("'", "??$$$??");
                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    StaticClass.LocalCon.Open();
                    sWr = "insert into Artists values (" + Convert.ToInt32(dsArtist.Tables[0].Rows[0]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                    OleDbCommand cmdAlbum = new OleDbCommand();
                    cmdAlbum.Connection = StaticClass.LocalCon;
                    cmdAlbum.CommandText = sWr;
                    cmdAlbum.ExecuteNonQuery();
                    StaticClass.LocalCon.Close();
                }

                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                StaticClass.LocalCon.Open();
                sWr = "insert into TitlesInPlaylists values (" + Playlist_Id + " , " + Convert.ToInt32(gblsongid) + ")";
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = StaticClass.LocalCon;
                cmd.CommandText = sWr;
                cmd.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

        }
        private void GetOldSongIdPlayer1()
        {
            Int32 Locali = 0;
            Boolean SongFind = true;
            try
            {
               string g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                drawLine = false;
                dgPlaylist.Invalidate();
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() != g.ToString())
                    {
                        SongFind = false;
                    }
                    else
                    {
                        SongFind = true;
                        Locali = i;
                        break;
                    }

                }
                if (SongFind == true)
                {
                    LastRowId = Locali;

                }
            }
            catch
            {
            }
        }
        private void GetOldSongIdPlayer2()
        {
            Int32 Locali = 0;
            Boolean SongFind = true;
            try
            {


                drawLine = false;
                dgPlaylist.Invalidate();
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() != g.ToString())
                    {
                        SongFind = false;
                    }
                    else
                    {
                        SongFind = true;
                        Locali = i;
                        break;
                    }

                }
                if (SongFind == true)
                {
                    LastRowId = Locali;
                }
            }

            catch { }
        }
        private void txtLastPlaylistName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                timResetSong.Stop();
                IsLast100Working = "Yes";
                if (txtLastPlaylistName.Text.Trim() == "")
                {
                    MessageBox.Show("Playlist name cannot be blank", "Music Player");
                    txtLastPlaylistName.Focus();
                    return;
                }
                DefaultPlaylistSave(txtLastPlaylistName.Text.Trim());
                txtLastPlaylistName.Text = "";
                gblsongid = "";
                for (int i = 0; i < dgLast100.Rows.Count; i++)
                {
                    insert_Playlist_song(dgLast100.Rows[i].Cells[0].Value.ToString(), "No", false);
                    //gblsongid = dgLast100.Rows[i].Cells[0].Value.ToString();
                    //Thread t2 = new Thread(Insert_Last100Playlist_songs);
                    //t2.IsBackground = true;
                    //t2.Start();
                }
                FillLocalPlaylist();
                IsLast100Working = "No";
                timResetSong.Start();
            }
        }

        private void txtLastPlaylistName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToInt32(e.KeyChar) == 39 || Convert.ToInt32(e.KeyChar) == 37)
            {
                e.Handled = true;
                return;
            }
        }
        #endregion
        #region "SetDefaultPlaylist"

        DataGridViewCell ActiveCell = null;
        private void dgLocalPlaylist_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Set as defalut", SetDefault));
                int currentMouseOverRow = dgLocalPlaylist.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    ActiveCell = dgLocalPlaylist[0, currentMouseOverRow];

                    dgLocalPlaylist.Rows[currentMouseOverRow].Selected = true;
                    dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[currentMouseOverRow].Cells[1];
                }
                m.Show(dgLocalPlaylist, new Point(e.X, e.Y));
            }
        }
        private void SetDefault(object sender, EventArgs e)
        {
            string mlsSql = "";
            DataTable dtGetRecords = new DataTable();
            if (ActiveCell != null && ActiveCell.Value != null)
                //Clipboard.SetText(ActiveCell.Value.ToString());

                mlsSql = "SELECT  count(*) from TitlesInPlaylists";
            mlsSql = mlsSql + " where PlaylistID=" + Convert.ToInt32(ActiveCell.Value.ToString());
            dtGetRecords = ObjMainClass.fnFillDataTable_Local(mlsSql);
            if (Convert.ToInt32(dtGetRecords.Rows[0][0]) <= 0)
            {
                MessageBox.Show("To set for default playlist you need to add minimum 1 song in this playlist.", "Music Player");
                return;
            }



            for (int i = 0; i < dgLocalPlaylist.Rows.Count; i++)
            {
                if (dgLocalPlaylist.Rows[i].Cells[0].Value.ToString() == ActiveCell.Value.ToString())
                {
                    dgLocalPlaylist.Rows[i].Cells[2].Value = "Default";
                }
                else
                {
                    dgLocalPlaylist.Rows[i].Cells[2].Value = "";

                }
            }
            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            OleDbCommand cmdUpdateAll = new OleDbCommand();
            cmdUpdateAll.Connection = StaticClass.LocalCon;
            cmdUpdateAll.CommandText = "Update Playlists set PlaylistDefault=''";
            cmdUpdateAll.ExecuteNonQuery();
            StaticClass.LocalCon.Close();

            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            OleDbCommand cmdUpdate = new OleDbCommand();
            cmdUpdate.Connection = StaticClass.LocalCon;
            cmdUpdate.CommandText = "Update Playlists set PlaylistDefault='Default' where playlistid = " + ActiveCell.Value.ToString();
            cmdUpdate.ExecuteNonQuery();
            StaticClass.LocalCon.Close();
            PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(ActiveCell.Value.ToString()));
            dgPlaylist.Visible = true;
            dgPlaylist.Dock = DockStyle.Fill;
            dgOtherPlaylist.Visible = false;


            for (int i = 0; i < dgLocalPlaylist.Rows.Count; i++)
            {
                if (dgLocalPlaylist.Rows[i].Cells[2].Value.ToString() == "Default")
                {
                    CurrentPlaylistRow = i;
                    break;
                }
            }
            CurrentRow = -1;
            LastRowId = -1;
            StaticClass.DefaultPlaylistCurrentRow = CurrentPlaylistRow;
            string g = "";
            if (musicPlayer1.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                if (g.ToString() == dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString())
                {
                    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(1)].Cells[0].Value.ToString());
                    CurrentRow = 0;
                }
                else
                {
                    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                }

            }
            else if (musicPlayer2.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                if (g.ToString() == dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString())
                {
                    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(1)].Cells[0].Value.ToString());
                    CurrentRow = 0;
                }
                else
                {
                    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                }
            }
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1].Style.SelectionBackColor = Color.White;
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1].Style.SelectionForeColor = Color.Black;
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[2].Style.SelectionBackColor = Color.White;
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[2].Style.SelectionForeColor = Color.Black;

            // MessageBox.Show(CurrentPlaylistRow.ToString());
            SetGridLayout();
        }

        private void SetGridLayout()
        {
            foreach (DataGridViewRow row in dgLocalPlaylist.Rows)
            {
                row.Height = 30;
                if (row.Cells[2].Value.ToString() == "Default")
                {

                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[1].Style.SelectionForeColor = Color.White;
                    row.Cells[1].Style.ForeColor = Color.FromArgb(20, 162, 175);
                    row.Cells[3].Style.SelectionBackColor = Color.LightBlue;
                    row.Cells[3].Style.BackColor = Color.LightBlue;
                }
                else
                {
                    row.Cells[1].Style.ForeColor = Color.FromArgb(0, 0, 0);
                    row.Cells[1].Style.SelectionForeColor = Color.White;
                    row.Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    row.Cells[3].Style.BackColor = Color.White;
                    row.Cells[3].Style.SelectionBackColor = Color.White;
                }
            }
        }


        private void dgOtherPlaylist_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }
            if (e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5)
            {
                drawLine = false;

                RowDeselect(dgOtherPlaylist);
                dgOtherPlaylist.Rows[e.RowIndex].Selected = true;
                dgOtherPlaylist.DoDragDrop(dgOtherPlaylist.Rows[e.RowIndex].Cells[0].Value.ToString(), DragDropEffects.Copy);
            }
        }



        private void dgOtherPlaylist_DragDrop(object sender, DragEventArgs e)
        {
            string SongName;
            string TempSongName;
            string lStr;
            string file;
            int Index;
            try
            {

                drawLine = false;
                dgOtherPlaylist.Invalidate();
                Grid_Clear = false;
                if (dgOtherPlaylist.Rows.Count >= 700)
                {
                    MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Music Player");
                    return;
                }
                if (StopDuplicate == "Yes") return;
                file = (string)e.Data.GetData(DataFormats.Text);
                SongName = Application.StartupPath + "\\" + file + ".mp3";
                TempSongName = Application.StartupPath + "\\" + file + ".sec";
                if (IsLast100Working == "Yes")
                {
                    lStr = "select * from TitlesInPlaylists where PlaylistID=" + StaticClass.Last100PlaylistId + " and TitleID=" + file;
                }
                else
                {
                    lStr = "select * from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID=" + file;
                }
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet_Local(lStr);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Show_Record == true)
                    {
                        DeleteHideSong(file);
                        PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    }
                }
                else
                {
                    if (ObjMainClass.CheckForInternetConnection() == false)
                    {
                        MessageBox.Show("Please check your Internet connection.", "Music Player");
                        return;
                    }
                    if (System.IO.File.Exists(TempSongName))
                    {
                        if (dgOtherPlaylist.Rows.Count == 700)
                        {
                            MessageBox.Show("The playlist has reached the maximum of songs. Please create a new playlist.", "Music Player");
                            return;
                        }
                        else
                        {

                            insert_Playlist_song(file, "No", false);
                            Point clientPoint = dgOtherPlaylist.PointToClient(new Point(e.X, e.Y));
                            Index = dgOtherPlaylist.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                            if (dgOtherPlaylist.Rows.Count == 0 || dgOtherPlaylist.Rows.Count == 1)
                            {
                                dgOtherPlaylist.Rows.Add();
                                Index = 0;
                                ResetPlaylist(dgOtherPlaylist, Index, file);
                                PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                                DownloadSong(dgOtherPlaylist);
                                return;

                            }
                            else if (Index == -1)
                            {
                                Index = 1;
                                ResetPlaylist(dgOtherPlaylist, Index, file);
                                DownloadSong(dgOtherPlaylist);
                                return;
                            }
                            else if (Index != -1)
                            {
                                ResetPlaylist(dgOtherPlaylist, Index, file);

                            }
                            GetSongCounter();
                            DownloadSong(dgOtherPlaylist);

                        }

                    }
                    else
                    {
                        string sQr = "";
                        sQr = "select COUNT(dfclientid) as TotalDownload from UserDownloadTitle where DfClientId=" + StaticClass.UserId + " and TokenId= " + StaticClass.TokenId;
                        DataSet dsTitle = new DataSet();
                        dsTitle = ObjMainClass.fnFillDataSet(sQr);
                        if (StaticClass.TotalTitles == dsTitle.Tables[0].Rows[0]["TotalDownload"].ToString())
                        {
                            MessageBox.Show("Your songs downloading limit is over." + Environment.NewLine + "Please contact vendor to resume the service.", "Music Player");
                            return;
                        }
                        eX = e.X;
                        eY = e.Y;
                        DropTitleSong = "Yes";
                        Add_Playlist = true;
                        FirstTimeSong = false;
                        insert_temp_data(file);
                        ShowPlaylistCounter = "Yes";
                        multi_song_download();
                    }
                }
                Show_Record = false;
                StopDuplicate = "Yes";
                Is_Drop = false;
            }
            catch { }
        }

        private void dgOtherPlaylist_DragLeave(object sender, EventArgs e)
        {
            drawLine = false;
            dgOtherPlaylist.Invalidate();
        }

        private void dgOtherPlaylist_DragOver(object sender, DragEventArgs e)
        {
            try
            {

                DataGridView.HitTestInfo info = this.dgOtherPlaylist.HitTest(e.X, e.Y);
                label5.Text = e.Y.ToString();
                if (drawLine == true)
                {
                    StopDuplicate = "No";
                    if (Convert.ToInt32(label5.Text) <= Convert.ToInt32(217))
                    {
                        info = this.dgOtherPlaylist.HitTest(20, 20);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(217) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(247))
                    {
                        info = this.dgOtherPlaylist.HitTest(50, 50);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(247) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(277))
                    {
                        info = this.dgOtherPlaylist.HitTest(80, 80);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(277) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(307))
                    {
                        info = this.dgOtherPlaylist.HitTest(110, 110);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(307) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(337))
                    {
                        info = this.dgOtherPlaylist.HitTest(140, 140);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(337) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(367))
                    {
                        info = this.dgOtherPlaylist.HitTest(170, 170);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(367) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(397))
                    {
                        info = this.dgOtherPlaylist.HitTest(200, 200);
                    }
                    else if (Convert.ToInt32(label5.Text) > Convert.ToInt32(397) && Convert.ToInt32(label5.Text) <= Convert.ToInt32(427))
                    {
                        info = this.dgOtherPlaylist.HitTest(230, 230);
                    }
                    else
                    {
                        info = this.dgOtherPlaylist.HitTest(240, 240);
                    }



                    if (info.ColumnIndex != -1)
                    {
                        Rectangle rect = this.dgOtherPlaylist.GetRowDisplayRectangle(
                            info.RowIndex, true);
                        this.p1.X = rect.Left;
                        this.p1.Y = rect.Bottom;
                        this.p2.X = rect.Right;
                        this.p2.Y = rect.Bottom;
                        this.drawLine = true;
                        this.dgOtherPlaylist.Invalidate();
                    }
                }
                else
                {
                    this.drawLine = false;
                    this.dgOtherPlaylist.Invalidate();

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgOtherPlaylist_KeyDown(object sender, KeyEventArgs e)
        {
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Music Player");
                return;
            }
            try
            {
                string localfilename = "";
                if (e.KeyCode == Keys.Delete)
                {
                    localfilename = dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() + ".mp3";
                    string localfilePath = Application.StartupPath + "\\" + localfilename;

                    if (Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) != 0)
                    {

                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                        StaticClass.LocalCon.Open();
                        OleDbCommand cmdLocal = new OleDbCommand();
                        cmdLocal.Connection = StaticClass.LocalCon;
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value;
                        cmdLocal.ExecuteNonQuery();
                        StaticClass.LocalCon.Close();

                        delete_temp_data(dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());

                        dgOtherPlaylist.Rows.RemoveAt(dgOtherPlaylist.CurrentCell.RowIndex);
                        GetSongCounter();
                        // DownloadSong();
                    }

                    else
                    {
                        MessageBox.Show("Please select a playlist.", "Music Player");
                    }
                }
                if (e.KeyCode.Equals(Keys.Up))
                {
                    moveUp(dgOtherPlaylist);
                }
                if (e.KeyCode.Equals(Keys.Down))
                {
                    moveDown(dgOtherPlaylist);
                }
                e.Handled = true;
            }
            catch
            {

                return;
            }
        }

        private void dgOtherPlaylist_MouseLeave(object sender, EventArgs e)
        {
            drawLine = false;
            dgOtherPlaylist.Invalidate();
        }

        private void dgOtherPlaylist_Paint(object sender, PaintEventArgs e)
        {
            if (this.drawLine)
            {
                using (p = new Pen(Color.Red, 3))
                {
                    EventSpl = e;
                    e.Graphics.DrawLine(p, p1, p2);
                }
            }
            else
            {
                //using (p = new Pen(Color.White, 0))
                //{
                //    EventSpl = e;
                //    e.Graphics.DrawLine(p, p1, p2);
                //}
            }
        }
        #endregion

        private void GetSongCounter()
        {
            string strNew = "";
            DataTable dtDetailNew = new DataTable();
            strNew = "select TitlesInPlaylists.playlistId, Count(*) as Total  from TitlesInPlaylists ";
            strNew = strNew + " where TitlesInPlaylists.playlistId = " + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " ";
            strNew = strNew + " group by TitlesInPlaylists.playlistId ";
            dtDetailNew = ObjMainClass.fnFillDataTable_Local(strNew);
            if ((dtDetailNew.Rows.Count > 0))
            {
                for (int iCtr = 0; (iCtr <= (dgLocalPlaylist.Rows.Count - 1)); iCtr++)
                {
                    if (Convert.ToInt32(dgLocalPlaylist.Rows[iCtr].Cells[0].Value) == Convert.ToInt32(dtDetailNew.Rows[0]["playlistId"]))
                    {
                        string strGetName = dgLocalPlaylist.Rows[iCtr].Cells[1].Value.ToString();
                        string[] arr = strGetName.Split('(');
                        dgLocalPlaylist.Rows[iCtr].Cells[1].Value = arr[0].Trim() + "  (" + dtDetailNew.Rows[0]["Total"] + ")";
                    }
                    // dtDetail.Rows[iCtr]["playlistId"];
                    //  
                }
            }

        }

        private void dgOtherPlaylist_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }
        #region "Advertisement"

        private void picAdvertisement_Click(object sender, EventArgs e)
        {
            string strAdvt = "";
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            if (StaticClass.IsAdvt == false)
            {
              //  MessageBox.Show("!! Please buy a subscription to use our advertisement option or contact your dealer. !!", "Music Player");
              //  return;
            }
            //strAdvt = "spGetAdvtExpiryStatus " + StaticClass.TokenId + ", 1 ";
            //DataSet dsExpire = new DataSet();
            //dsExpire = ObjMainClass.fnFillDataSet(strAdvt);

            //if (dsExpire.Tables[0].Rows[0]["ExpiryAdvtStatus"].ToString() == "NoLic")
            //{
            //    MessageBox.Show("!! Please buy a subscription to use our advertisement option or contact your dealer. !!", "Music Player");
            //    return;
            //}
            //if (dsExpire.Tables[0].Rows[0]["ExpiryAdvtStatus"].ToString() == "Yes")
            //{
            //    MessageBox.Show("!! Subscription is expire !!", "Music Player");
            //    return;
            //}
            //if (dsExpire.Tables[0].Rows[0]["ExpiryAdvtStatus"].ToString() != "NoLic" && Convert.ToInt32(dsExpire.Tables[0].Rows[0]["LeftAdvtDays"]) == 0)
            //{
            //    MessageBox.Show("This is your last day to renew your advertisement subscription.", "Music Player");
            //}
            panAdvtDetail.Location = new Point(0, 0);
            panAdvtDetail.Width = this.Width;
            panAdvtDetail.Height = this.Height;
            panAdvtDetail.Visible = true;
            panAdvtDetail.BringToFront();
            panAdvtSchedule.Location = new Point(0, 0);
            panAdvtSchedule.Width = panAdvtDetail.Width;
            panAdvtSchedule.Height = panAdvtDetail.Height;
            //panel11.Visible = false;
            panAdvtSchedule.BringToFront();
            panAdvtSchedule.Visible = true;
            panAdvtGrid.Height = panAdvtSchedule.Height - panButtonSettings.Height - panScheduleDate.Height - 40;
        }
        private void FillMainAdvertisement()
        {
            string PlayerType = "";
            PlayerType = "Copyright";
            string str = "";
            int iCtr;
            DataTable dtDetail;
            DataTable dtDetailLocal;
            dtDetailLocal = ObjMainClass.fnFillDataTable_Local("select * from tbAdvt where ScheduleDate=#" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "#");
            //if (StaticClass.IsAdvtManual == true)
            //{
            //    str = "spGetAdvertisementClientManual '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','" + PlayerType + "','" + StaticClass.DealerCode + "', " + StaticClass.AdvtCityId + "," + StaticClass.TokenId + "";
            //}
            //else
            //{
            //    str = "spGetAdvertisementClient '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','" + PlayerType + "','" + StaticClass.DealerCode + "', " + StaticClass.AdvtCityId + "," + StaticClass.TokenId + "";
            //}

            if (StaticClass.IsAdvtManual == true)
            {
                str = "spGetAdvtManual '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','" + PlayerType + "'," + StaticClass.UserId + "," + ReturnWeekId(DateTime.Now.DayOfWeek.ToString()) + ", " + StaticClass.AdvtCityId + "," + StaticClass.UserId + " , " + StaticClass.CountryId + ", " + StaticClass.Stateid + ", " + StaticClass.TokenId;
            }
            else
            {
                str = "spGetAdvtAdmin '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','" + PlayerType + "'," + StaticClass.UserId + "," + ReturnWeekId(DateTime.Now.DayOfWeek.ToString()) + ", " + StaticClass.AdvtCityId + "," + StaticClass.UserId + " , " + StaticClass.CountryId + ", " + StaticClass.Stateid + ", " + StaticClass.TokenId;
            }
            dtDetail = ObjMainClass.fnFillDataTable(str);
            InitilizeMainAdvertisement();
            if ((dtDetail.Rows.Count > 0))
            {
                //panUpcomingAdvt.Visible = true;
                timGetRemainAdvtTime.Enabled = true;
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgAdvt.Rows.Add();
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["AdvtId"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["AdvtDisplayName"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["AdvtCompanyName"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[3].Value = dtDetail.Rows[iCtr]["AdvtFilePath"];

                    bool exists = dtDetailLocal.Select().ToList().Exists(row => row["AdvtId"].ToString() == dtDetail.Rows[iCtr]["AdvtId"].ToString());
                    if (exists == true)
                    {
                        dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[4].Value = "Done";
                    }
                    else
                    {
                        dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[4].Value = "";
                    }

                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[5].Value = dtDetail.Rows[iCtr]["AdvtTypeName"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[6].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtStartDate"]);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[7].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtEndDate"]);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[8].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["AdvtTime"]);


                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[6].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[7].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[8].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                }
                foreach (DataGridViewRow row in dgAdvt.Rows)
                {
                    row.Height = 30;
                }
            }
            else
            {
                panUpcomingAdvt.Visible = false;
                timGetRemainAdvtTime.Enabled = false;
            }
        }
        private void InitilizeMainAdvertisement()
        {
            if (dgAdvt.Rows.Count > 0)
            {
                dgAdvt.Rows.Clear();
            }
            if (dgAdvt.Columns.Count > 0)
            {
                dgAdvt.Columns.Clear();
            }

            dgAdvt.Columns.Add("Advtid", "Advt Id");
            dgAdvt.Columns["Advtid"].Width = 0;
            dgAdvt.Columns["Advtid"].Visible = false;
            dgAdvt.Columns["Advtid"].ReadOnly = true;

            dgAdvt.Columns.Add("Advt", "Advertisement Name");
            dgAdvt.Columns["Advt"].Width = 245;
            dgAdvt.Columns["Advt"].Visible = true;
            dgAdvt.Columns["Advt"].ReadOnly = true;

            dgAdvt.Columns.Add("AdvtComp", "Advt Comp");
            dgAdvt.Columns["AdvtComp"].Width = 0;
            dgAdvt.Columns["AdvtComp"].Visible = false;
            dgAdvt.Columns["AdvtComp"].ReadOnly = true;

            dgAdvt.Columns.Add("AdvtLink", "AdvtLink");
            dgAdvt.Columns["AdvtLink"].Width = 0;
            dgAdvt.Columns["AdvtLink"].Visible = false;
            dgAdvt.Columns["AdvtLink"].ReadOnly = true;


            dgAdvt.Columns.Add("Play", "Play");
            dgAdvt.Columns["Play"].Width = 0;
            dgAdvt.Columns["Play"].Visible = false;
            dgAdvt.Columns["Play"].ReadOnly = true;


            dgAdvt.Columns.Add("Type", "Type");
            dgAdvt.Columns["Type"].Width = 200;
            dgAdvt.Columns["Type"].Visible = true;
            dgAdvt.Columns["Type"].ReadOnly = true;

            dgAdvt.Columns.Add("StartDate", "Start Date");
            dgAdvt.Columns["StartDate"].Width = 200;
            dgAdvt.Columns["StartDate"].Visible = true;
            dgAdvt.Columns["StartDate"].ReadOnly = true;

            dgAdvt.Columns.Add("EndDate", "End Date");
            dgAdvt.Columns["EndDate"].Width = 200;
            dgAdvt.Columns["EndDate"].Visible = true;
            dgAdvt.Columns["EndDate"].ReadOnly = true;

            dgAdvt.Columns.Add("AdvtTime", "Time");
            dgAdvt.Columns["AdvtTime"].Width = 200;
            dgAdvt.Columns["AdvtTime"].Visible = true;
            dgAdvt.Columns["AdvtTime"].ReadOnly = true;

            dgAdvt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }


        private void picMainPlayer_Click(object sender, EventArgs e)
        {
            panAdvtDetail.Visible = false;
        }


        private void picAdvtSchedule_Click(object sender, EventArgs e)
        {
            panAdvtSchedule.Location = new Point(0, 0);
            panAdvtSchedule.Width = panAdvtDetail.Width;
            panAdvtSchedule.Height = panAdvtDetail.Height;
            //panel11.Visible = false;
            panAdvtSchedule.Visible = true;
            panAdvtGrid.Height = panAdvtSchedule.Height - panButtonSettings.Height - panScheduleDate.Height - 40;
        }


        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                    return;
                }
                string strDel = "";
                if (dgAdvtComman.Rows.Count == 0) return;
                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                StaticClass.LocalCon.Open();
                OleDbCommand cmdDelAdvt = new OleDbCommand();
                cmdDelAdvt.Connection = StaticClass.LocalCon;
                cmdDelAdvt.CommandText = "delete from tbAdvt  where ScheduleDate= #" + string.Format("{0:dd/MMM/yyyy}", dtpScheduleDate.Value) + "#";
                cmdDelAdvt.ExecuteNonQuery();
                StaticClass.LocalCon.Close();

                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmdDel = new SqlCommand();
                cmdDel.Connection = StaticClass.constr;
                strDel = "delete from tbAdvertisementManualSettings where tokenid=" + Convert.ToInt32(StaticClass.TokenId) + " and ScheduleDate= '" + string.Format("{0:dd/MMM/yyyy}", dtpScheduleDate.Value) + "'";
                cmdDel.CommandText = strDel;
                cmdDel.ExecuteNonQuery();
                StaticClass.constr.Close();


                for (int i = 0; i <= dgAdvtComman.Rows.Count - 1; i++)
                {
                    if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                    StaticClass.constr.Open();
                    SqlCommand cmd = new SqlCommand("spAdvertisementManualSettingsSave", StaticClass.constr);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
                    cmd.Parameters["@TokenId"].Value = Convert.ToInt32(StaticClass.TokenId);

                    cmd.Parameters.Add(new SqlParameter("@SrNo", SqlDbType.Int));
                    cmd.Parameters["@SrNo"].Value = i + 1;

                    cmd.Parameters.Add(new SqlParameter("@AdvtId", SqlDbType.BigInt));
                    cmd.Parameters["@AdvtId"].Value = Convert.ToInt32(dgAdvtComman.Rows[i].Cells[0].Value);

                    cmd.Parameters.Add(new SqlParameter("@ScheduleDate", SqlDbType.DateTime));
                    cmd.Parameters["@ScheduleDate"].Value = dtpScheduleDate.Value;

                    cmd.ExecuteNonQuery();
                }
                StaticClass.constr.Close();

                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmdUpdate = new SqlCommand();
                cmdUpdate.Connection = StaticClass.constr;
                cmdUpdate.CommandText = "Update AMPlayerTokens set IsAdvtManual=1 where tokenid=" + Convert.ToInt32(StaticClass.TokenId);
                cmdUpdate.ExecuteNonQuery();
                StaticClass.constr.Close();



                if (Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dtpScheduleDate.Value)) == Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date)))
                {
                    StaticClass.IsAdvtManual = true;
                    string strOpt = "";
                    strOpt = "select * from tbAdvertisementTiming";
                    DataSet dsOption1 = new DataSet();
                    dsOption1 = ObjMainClass.fnFillDataSet(strOpt);
                    StaticClass.AdvtTime = Convert.ToInt32(dsOption1.Tables[0].Rows[0]["AdvtTime"]);

                    MessageBox.Show("The manual advertisement modus is ON and the advertisements are playing according your selection.", "Music Player");
                    FillAllAdvertisement();
                    FillMainAdvertisement();
                    timGetAdvtTime.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Your settings are save", "Music Player");
                }
                dtpScheduleDate.Value = DateTime.Now.Date;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnDefaultSettings_Click(object sender, EventArgs e)
        {
            try
            {
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                    return;
                }
                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmdDel = new SqlCommand();
                cmdDel.Connection = StaticClass.constr;
                cmdDel.CommandText = "delete from tbAdvertisementManualSettings where tokenid=" + Convert.ToInt32(StaticClass.TokenId);
                cmdDel.ExecuteNonQuery();
                StaticClass.constr.Close();

                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmdUpdate = new SqlCommand();
                cmdUpdate.Connection = StaticClass.constr;
                cmdUpdate.CommandText = "Update AMPlayerTokens set IsAdvtManual=0 where tokenid=" + Convert.ToInt32(StaticClass.TokenId);
                cmdUpdate.ExecuteNonQuery();
                StaticClass.constr.Close();

                StaticClass.IsAdvtManual = false;
                MessageBox.Show("The administrator advertisement modus is ON and the advertisements are playing according a pre scheduled time setting.", "Music Player");
                FillAllAdvertisement();
                FillMainAdvertisement();
                timGetAdvtTime.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void timAdvt_Tick(object sender, EventArgs e)
        {
            try
            {
                double t1 = Math.Floor(AdvtPlayer.currentMedia.duration - AdvtPlayer.Ctlcontrols.currentPosition);
                double w1 = Math.Floor(AdvtPlayer.Ctlcontrols.currentPosition);
                double mint1 = Math.Floor(t1 / 60);
                double s1;
                int r1;
                s1 = Convert.ToInt16(Math.Abs(t1 / 60));
                r1 = Convert.ToInt16(t1 % 60);
                //--------------------------------------------//
                //--------------------------------------------//

                double fd;
                fd = Math.Floor(AdvtPlayer.currentMedia.duration);
                double zh;
                zh = fd / 60;
                double left = System.Math.Floor(zh);
                double sec2 = fd % 60;
                //--------------------------------------------//
                //--------------------------------------------//

                if (AdvtPlayer.status == "Ready")
                {
                    lblAdvtMusic.Text = "00:00";
                    lblAdvtDuration.Text = "00:00";
                }
                else
                {
                    lblAdvtMusic.Text = mint1.ToString("00") + ":" + r1.ToString("00");
                    lblAdvtDuration.Text = left.ToString("00") + ":" + sec2.ToString("00");
                }
                double w = Math.Floor(AdvtPlayer.Ctlcontrols.currentPosition);
                AdvtProBar.Maximum = Convert.ToInt16(AdvtPlayer.currentMedia.duration);
                AdvtProBar.Value = Convert.ToInt16(w);

                if (AdvtPlayer.playState == WMPPlayState.wmppsStopped || AdvtPlayer.playState == WMPPlayState.wmppsReady)
                {
                    panPlayerButton.Enabled = true;
                    panComman.Enabled = true;
                    panel8.Enabled = true;
                    panPlaylist.Enabled = true;
                    //panUpcomingAdvt.Visible = false;
                    AdvtCurrentRow = AdvtCurrentRow + 1;
                    panAdvt.Visible = false;

                    if (PanelVisiable == true)
                    {
                        panAdvtDetail.Visible = true;
                    }
                    else
                    {
                        panAdvtDetail.Visible = false;
                    }
                    AdvtTimeResult = 0;
                    TimePlayerOne = 0;
                    TimePlayerTwo = 0;
                    prvPlayerOneTime = 0;
                    prvPlayerTwoTime = 0;
                    TimeStreamPlayer = 0;
                    AdvtCurrentSongId = 0;
                    lblSongCount.Text = "1";

                    if (lblPlayerName.Text == "Stream")
                    {
                        StreamMusicPlayer.Ctlcontrols.play();
                        if (btnStreamMute.Text == "")
                        {
                            StreamMusicPlayer.settings.volume = 100;
                        }
                        else
                        {
                            StreamMusicPlayer.settings.mute = true;
                        }
                    }
                    else if (lblPlayerName.Text == "One")
                    {
                        musicPlayer1.settings.volume = 0;
                        musicPlayer1.Ctlcontrols.stop();
                        musicPlayer1.URL = "";
                        musicPlayer2.Ctlcontrols.play();
                        if (btnMute.Text == "")
                        {
                            musicPlayer2.settings.mute = false;
                            musicPlayer1.settings.mute = false;
                            musicPlayer2.settings.volume = 100;
                        }
                        else if (btnMute.Text == ".")
                        {
                            musicPlayer2.settings.mute = true;
                            musicPlayer1.settings.mute = true;
                        }
                        Song_Set_foucs2();
                        //if (CurrentRow == dgPlaylist.Rows.Count - 1)
                        //{
                        //    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        //}
                        //else
                        //{
                        //    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                        //}
                        if (IsVisibleSong == true)
                        {
                            if (LastRowId == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                            }
                        }
                        else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                        {
                            if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                            }
                        }
                        else
                        {
                            if (CurrentRow == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                            }
                        }
                        g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                        ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");
                    }
                    else
                    {
                        musicPlayer2.settings.volume = 0;
                        musicPlayer2.Ctlcontrols.stop();
                        musicPlayer2.URL = "";
                        musicPlayer1.Ctlcontrols.play();
                        if (btnMute.Text == "")
                        {
                            musicPlayer2.settings.mute = false;
                            musicPlayer1.settings.mute = false;
                            musicPlayer1.settings.volume = 100;
                        }
                        else if (btnMute.Text == ".")
                        {
                            musicPlayer2.settings.mute = true;
                            musicPlayer1.settings.mute = true;
                        }
                        Song_Set_foucs();
                        //if (CurrentRow == dgPlaylist.Rows.Count - 1)
                        //{
                        //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                        //}
                        //else
                        //{
                        //    NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                        //}
                        if (IsVisibleSong == true)
                        {
                            if (LastRowId == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                            }
                        }
                        else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                        {
                            if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                            }

                        }
                        else
                        {
                            if (CurrentRow == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                            }
                        }
                        string g = "";
                        g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                        ObjMainClass.DeleteAllOgg(g.ToString() + ".mp3");
                    }
                    FillMainAdvertisement();
                    timGetAdvtTime.Enabled = true;
                    timAdvt.Enabled = false;

                    rCount = 0;
                    DropSongLength = "";
                    IsSongDropAdvt = false;
                    label7.Text = "0";
                    label8.Text = "0";
                    label14.Text = "0";
                    IsAdvtTimeGet = false;
                    GrossTotaltime = 0;
                    timGetRemainAdvtTime.Enabled = true;
                }
            }

            catch
            {
            }
        }

        private void timGetAdvtTime_Tick(object sender, EventArgs e)
        {
            AdvtTimeResult = AdvtTimeResult + 1;
            if (StaticClass.IsAdvtManual == true)
            {
                // AdvtTimeResult = TimePlayerOne + TimePlayerTwo + TimeStreamPlayer;
                //if (TimePlayerOne == 0)
                //{
                //    AdvtTimeResult = prvPlayerOneTime + prvPlayerTwoTime + TimePlayerTwo + TimeStreamPlayer;
                //}
                //else if (TimePlayerTwo == 0)
                //{
                //    AdvtTimeResult = TimePlayerOne + prvPlayerOneTime + prvPlayerTwoTime + TimeStreamPlayer;
                //}
                //else
                //{
                //    if (TimePlayerOne > 10)
                //    {
                //        AdvtTimeResult = TimePlayerTwo + TimeStreamPlayer + prvPlayerOneTime + prvPlayerTwoTime;
                //    }
                //    else if (TimePlayerTwo > 10)
                //    {
                //        AdvtTimeResult = TimePlayerOne + TimeStreamPlayer + prvPlayerOneTime + prvPlayerTwoTime;
                //    }

                //}
                lblAdvtMainTime.Text = AdvtTimeResult.ToString();
                if (dgAdvt.Rows.Count > 0)
                {
                    GetUpcomingAdvtName();
                    panUpcomingAdvt.Visible = true;
                    if (AdvtTimeResult >= StaticClass.AdvtTime)
                    {
                        AdvtTimeResult = 0;
                        lblSongCount.Text = "2";
                        timGetAdvtTime.Enabled = false;
                    }
                }
                else
                {
                    if (AdvtTimeResult >= 900)
                    {
                        AdvtTimeResult = 0;
                        TimePlayerOne = 0;
                        TimePlayerTwo = 0;
                        TimeStreamPlayer = 0;
                        prvPlayerOneTime = 0;
                        prvPlayerTwoTime = 0;
                        lblSongCount.Text = "1";
                        FillMainAdvertisement();
                    }
                }
            }
            else
            {
                //panUpcomingAdvt.Visible = false;
                lblCurrentTime.Text = DateTime.Now.ToString("hh:mm tt");
                if (AdvtPlayTime == lblCurrentTime.Text) return;
                for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
                {
                    if (lblCurrentTime.Text == dgAdvt.Rows[iRow].Cells[8].Value.ToString())
                    {
                        AdvtTimeResult = 0;
                        AdvtPlayTime = lblCurrentTime.Text;
                        AdvtCurrentRow = iRow;
                        lblSongCount.Text = "2";
                        GetUpcomingAdvtName();
                        panUpcomingAdvt.Visible = true;
                        timGetAdvtTime.Enabled = false;
                        break;
                    }
                }
                if (dgAdvt.Rows.Count == 0)
                {
                    if (AdvtTimeResult >= 900)
                    {
                        AdvtTimeResult = 0;
                        TimePlayerOne = 0;
                        TimePlayerTwo = 0;
                        TimeStreamPlayer = 0;
                        prvPlayerOneTime = 0;
                        prvPlayerTwoTime = 0;
                        lblSongCount.Text = "1";
                        FillMainAdvertisement();
                        FillAllAdvertisement();
                    }
                }
            }
        }
        private void GetUpcomingAdvtName()
        {
            if (AdvtCurrentRow != 0)
            {
                if (AdvtCurrentRow >= dgAdvt.Rows.Count)
                {
                    AdvtCurrentRow = 0;
                }
            }
            if (StaticClass.IsAdvtManual == true)
            {
                if (AdvtCurrentSongId != 0)
                {
                    for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
                    {
                        if (AdvtCurrentSongId == Convert.ToInt32(dgAdvt.Rows[iRow].Cells[0].Value))
                        {
                            AdvtCurrentRow = iRow;
                            break;
                        }
                    }
                }
                else
                {
                    for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
                    {
                        if (dgAdvt.Rows[iRow].Cells[4].Value.ToString() != "Done")
                        {
                            AdvtCurrentRow = iRow;
                            break;
                        }
                    }
                }
            }
            lblUpcomingAdvtName.Text = "Next advertisement:- " + dgAdvt.Rows[AdvtCurrentRow].Cells[1].Value.ToString();

        }
        private void panAdvt_VisibleChanged(object sender, EventArgs e)
        {
            string LocalCheckAdvt = "No";
            if (panAdvt.Visible == true)
            {
                panUpcomingAdvt.Visible = false;
                if (AdvtCurrentRow != 0)
                {
                    if (AdvtCurrentRow >= dgAdvt.Rows.Count)
                    {
                        AdvtCurrentRow = 0;
                    }
                }
                if (StaticClass.IsAdvtManual == true)
                {
                    if (AdvtCurrentSongId != 0)
                    {
                        for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
                        {
                            if (AdvtCurrentSongId == Convert.ToInt32(dgAdvt.Rows[iRow].Cells[0].Value))
                            {
                                AdvtCurrentRow = iRow;
                                LocalCheckAdvt = "Yes";
                                break;
                            }
                        }
                    }
                    else
                    {
                        for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
                        {
                            if (dgAdvt.Rows[iRow].Cells[4].Value.ToString() != "Done")
                            {
                                AdvtCurrentRow = iRow;
                                LocalCheckAdvt = "Yes";
                                break;
                            }
                        }
                    }

                    if (LocalCheckAdvt == "No")
                    {
                        AdvtCurrentRow = 0;
                        if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                        StaticClass.LocalCon.Open();
                        OleDbCommand cmdDelAdvt = new OleDbCommand();
                        cmdDelAdvt.Connection = StaticClass.LocalCon;
                        cmdDelAdvt.CommandText = "delete from tbAdvt  where ScheduleDate=#" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "#";
                        cmdDelAdvt.ExecuteNonQuery();
                        StaticClass.LocalCon.Close();
                    }

                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdInsertVal = new OleDbCommand();
                    cmdInsertVal.Connection = StaticClass.LocalCon;
                    cmdInsertVal.CommandText = "insert into tbAdvt(AdvtId,ScheduleDate) values(" + dgAdvt.Rows[AdvtCurrentRow].Cells[0].Value.ToString() + ",'" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "')";
                    cmdInsertVal.ExecuteNonQuery();
                    StaticClass.LocalCon.Close();

                    dgAdvt.Rows[AdvtCurrentRow].Cells[4].Value = "Done";
                }
                if (panAdvtDetail.Visible == true)
                {
                    PanelVisiable = true;
                    // panAdvtDetail.Visible = false;
                }
                else
                {
                    PanelVisiable = false;
                }

                panAdvt.Location = new Point(0, 0);
                panAdvt.Width = this.Width;
                lblAdvtName.Text = dgAdvt.Rows[AdvtCurrentRow].Cells[1].Value.ToString();
                lblAdvtCompany.Text = dgAdvt.Rows[AdvtCurrentRow].Cells[2].Value.ToString();
                AdvtPlayer.URL = dgAdvt.Rows[AdvtCurrentRow].Cells[3].Value.ToString();
                AdvtPlayer.Ctlcontrols.play();
                if (btnMute.Text == "." || btnStreamMute.Text == ".")
                {
                    AdvtPlayer.settings.volume = 0;
                }
                else
                {
                    AdvtPlayer.settings.volume = 100;
                }
                timAdvt.Enabled = true;
            }
        }




        private void dgAdvt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StopDup = true;
        }

        private void dgAdvt_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1) return;
            drawLine = true;
            RowDeselect(dgAdvt);
            dgAdvt.Rows[e.RowIndex].Selected = true;
            //dgAdvt.DoDragDrop(dgAdvt.Rows[e.RowIndex].Cells[0].Value.ToString(), DragDropEffects.Copy);

            StopDup = false;
            rw = dgAdvt.SelectedRows[0];
            rowIndexFromMouseDown = dgAdvt.SelectedRows[0].Index;
            dgAdvt.DoDragDrop(rw, DragDropEffects.Move);
        }

        private void dgAdvt_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgAdvt_DragDrop(object sender, DragEventArgs e)
        {
            int Index = 0;
            drawLine = false;
            dgAdvt.Invalidate();
            if (StopDup == true) return;
            int rowIndexOfItemUnderMouseToDrop;
            Point clientPoint = dgAdvt.PointToClient(new Point(e.X, e.Y));
            rowIndexOfItemUnderMouseToDrop = dgAdvt.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (rowIndexOfItemUnderMouseToDrop == -1)
            {
                rowIndexOfItemUnderMouseToDrop = dgAdvt.Rows.Count - 1;
            }
            if (e.Effect == DragDropEffects.Move)
            {
                dgAdvt.Rows.RemoveAt(rowIndexFromMouseDown);
                dgAdvt.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rw);
            }
        }

        private void dgAdvt_DragEnter(object sender, DragEventArgs e)
        {
            if (dgAdvt.SelectedRows.Count > 0)
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void dgAdvt_DragLeave(object sender, EventArgs e)
        {

        }

        private void dgAdvt_DragOver(object sender, DragEventArgs e)
        {
            try
            {

                DataGridView.HitTestInfo info = this.dgAdvt.HitTest(e.X, e.Y);
                label24.Text = e.Y.ToString();
                if (drawLine == true)
                {
                    // StopDuplicate = "No";
                    if (Convert.ToInt32(label24.Text) <= Convert.ToInt32(96))
                    {
                        info = this.dgAdvt.HitTest(30, 30);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(96) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(126))
                    {
                        info = this.dgAdvt.HitTest(60, 60);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(126) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(156))
                    {
                        info = this.dgAdvt.HitTest(90, 90);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(156) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(186))
                    {
                        info = this.dgAdvt.HitTest(120, 120);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(186) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(216))
                    {
                        info = this.dgAdvt.HitTest(150, 150);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(216) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(246))
                    {
                        info = this.dgAdvt.HitTest(180, 180);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(246) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(276))
                    {
                        info = this.dgAdvt.HitTest(210, 210);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(276) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(306))
                    {
                        info = this.dgAdvt.HitTest(240, 240);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(306) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(336))
                    {
                        info = this.dgAdvt.HitTest(270, 270);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(336) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(366))
                    {
                        info = this.dgAdvt.HitTest(300, 300);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(366) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(396))
                    {
                        info = this.dgAdvt.HitTest(330, 330);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(396) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(426))
                    {
                        info = this.dgAdvt.HitTest(360, 360);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(426) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(456))
                    {
                        info = this.dgAdvt.HitTest(390, 390);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(456) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(486))
                    {
                        info = this.dgAdvt.HitTest(420, 420);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(486) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(516))
                    {
                        info = this.dgAdvt.HitTest(450, 450);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(516) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(546))
                    {
                        info = this.dgAdvt.HitTest(480, 480);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(546) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(576))
                    {
                        info = this.dgAdvt.HitTest(510, 510);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(576) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(606))
                    {
                        info = this.dgAdvt.HitTest(540, 540);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(606) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(636))
                    {
                        info = this.dgAdvt.HitTest(570, 570);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(636) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(666))
                    {
                        info = this.dgAdvt.HitTest(600, 600);
                    }

                    else
                    {
                        info = this.dgAdvt.HitTest(630, 630);
                    }
                    if (info.ColumnIndex != -1)
                    {
                        Rectangle rect = this.dgAdvt.GetRowDisplayRectangle(
                            info.RowIndex, true);
                        this.p1.X = rect.Left;
                        this.p1.Y = rect.Bottom;
                        this.p2.X = rect.Right;
                        this.p2.Y = rect.Bottom;
                        this.drawLine = true;
                        this.dgAdvt.Invalidate();
                    }
                }
                else
                {
                    this.drawLine = false;
                    this.dgAdvt.Invalidate();

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgAdvt_MouseLeave(object sender, EventArgs e)
        {

        }

        private void dgAdvt_Paint(object sender, PaintEventArgs e)
        {
            if (this.drawLine)
            {
                using (p = new Pen(Color.Red, 3))
                {
                    EventSpl = e;
                    e.Graphics.DrawLine(p, p1, p2);
                }
            }
            else
            {
                //using (p = new Pen(Color.White, 0))
                //{
                //    EventSpl = e;
                //    e.Graphics.DrawLine(p, p1, p2);
                //}
            }
        }



        private void dgAdvtComman_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StopDup = true;
        }
        private void dgAdvtComman_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1) return;
            drawLine = true;
            RowDeselect(dgAdvtComman);
            dgAdvtComman.Rows[e.RowIndex].Selected = true;
            //dgAdvt.DoDragDrop(dgAdvt.Rows[e.RowIndex].Cells[0].Value.ToString(), DragDropEffects.Copy);

            StopDup = false;
            rw = dgAdvtComman.SelectedRows[0];
            rowIndexFromMouseDown = dgAdvtComman.SelectedRows[0].Index;
            dgAdvtComman.DoDragDrop(rw, DragDropEffects.Move);
        }
        private void dgAdvtComman_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dgAdvtComman_DragDrop(object sender, DragEventArgs e)
        {
            int Index = 0;
            drawLine = false;
            dgAdvtComman.Invalidate();
            if (StopDup == true) return;
            int rowIndexOfItemUnderMouseToDrop;
            Point clientPoint = dgAdvtComman.PointToClient(new Point(e.X, e.Y));
            rowIndexOfItemUnderMouseToDrop = dgAdvtComman.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (rowIndexOfItemUnderMouseToDrop == -1)
            {
                rowIndexOfItemUnderMouseToDrop = dgAdvtComman.Rows.Count - 1;
            }
            if (e.Effect == DragDropEffects.Move)
            {
                dgAdvtComman.Rows.RemoveAt(rowIndexFromMouseDown);
                dgAdvtComman.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rw);
            }
        }
        private void dgAdvtComman_DragEnter(object sender, DragEventArgs e)
        {
            if (dgAdvtComman.SelectedRows.Count > 0)
            {
                e.Effect = DragDropEffects.Move;
            }
        }
        private void dgAdvtComman_DragLeave(object sender, EventArgs e)
        {

        }
        private void dgAdvtComman_DragOver(object sender, DragEventArgs e)
        {
            try
            {

                DataGridView.HitTestInfo info = this.dgAdvtComman.HitTest(e.X, e.Y);
                label24.Text = e.Y.ToString();
                if (drawLine == true)
                {
                    // StopDuplicate = "No";
                    if (Convert.ToInt32(label24.Text) <= Convert.ToInt32(136))
                    {
                        info = this.dgAdvtComman.HitTest(30, 30);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(136) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(166))
                    {
                        info = this.dgAdvtComman.HitTest(60, 60);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(166) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(196))
                    {
                        info = this.dgAdvtComman.HitTest(90, 90);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(196) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(226))
                    {
                        info = this.dgAdvtComman.HitTest(120, 120);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(226) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(256))
                    {
                        info = this.dgAdvtComman.HitTest(150, 150);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(256) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(286))
                    {
                        info = this.dgAdvtComman.HitTest(180, 180);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(286) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(316))
                    {
                        info = this.dgAdvtComman.HitTest(210, 210);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(316) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(346))
                    {
                        info = this.dgAdvtComman.HitTest(240, 240);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(346) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(376))
                    {
                        info = this.dgAdvtComman.HitTest(270, 270);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(376) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(406))
                    {
                        info = this.dgAdvtComman.HitTest(300, 300);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(406) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(436))
                    {
                        info = this.dgAdvtComman.HitTest(330, 330);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(436) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(466))
                    {
                        info = this.dgAdvtComman.HitTest(360, 360);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(466) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(496))
                    {
                        info = this.dgAdvtComman.HitTest(390, 390);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(496) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(526))
                    {
                        info = this.dgAdvtComman.HitTest(420, 420);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(526) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(556))
                    {
                        info = this.dgAdvtComman.HitTest(450, 450);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(556) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(586))
                    {
                        info = this.dgAdvtComman.HitTest(480, 480);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(586) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(616))
                    {
                        info = this.dgAdvtComman.HitTest(510, 510);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(616) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(646))
                    {
                        info = this.dgAdvtComman.HitTest(540, 540);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(646) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(676))
                    {
                        info = this.dgAdvtComman.HitTest(570, 570);
                    }
                    else if (Convert.ToInt32(label24.Text) > Convert.ToInt32(676) && Convert.ToInt32(label24.Text) <= Convert.ToInt32(706))
                    {
                        info = this.dgAdvtComman.HitTest(600, 600);
                    }

                    else
                    {
                        info = this.dgAdvtComman.HitTest(630, 630);
                    }
                    if (info.ColumnIndex != -1)
                    {
                        Rectangle rect = this.dgAdvtComman.GetRowDisplayRectangle(
                            info.RowIndex, true);
                        this.p1.X = rect.Left;
                        this.p1.Y = rect.Bottom;
                        this.p2.X = rect.Right;
                        this.p2.Y = rect.Bottom;
                        this.drawLine = true;
                        this.dgAdvtComman.Invalidate();
                    }
                }
                else
                {
                    this.drawLine = false;
                    this.dgAdvtComman.Invalidate();

                }
            }
            catch (Exception ex)
            {
            }
        }
        private void dgAdvtComman_MouseLeave(object sender, EventArgs e)
        {

        }
        private void dgAdvtComman_Paint(object sender, PaintEventArgs e)
        {
            if (this.drawLine)
            {
                using (p = new Pen(Color.Red, 3))
                {
                    EventSpl = e;
                    e.Graphics.DrawLine(p, p1, p2);
                }
            }
            else
            {
                //using (p = new Pen(Color.White, 0))
                //{
                //    EventSpl = e;
                //    e.Graphics.DrawLine(p, p1, p2);
                //}
            }
        }
        private void FillAllAdvertisement()
        {
            string PlayerType = "";
            PlayerType = "Copyright";
            string str = "";
            int iCtr;
            DataTable dtDetail;


            if (StaticClass.IsAdvtManual == true)
            {
                str = "spGetAdvtManual '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','" + PlayerType + "'," + StaticClass.UserId + "," + ReturnWeekId(DateTime.Now.DayOfWeek.ToString()) + ", " + StaticClass.AdvtCityId + "," + StaticClass.UserId + " , " + StaticClass.CountryId + ", " + StaticClass.Stateid + ", " + StaticClass.TokenId;
            }
            else
            {
                str = "spGetAdvtAdmin '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','" + PlayerType + "'," + StaticClass.UserId + "," + ReturnWeekId(DateTime.Now.DayOfWeek.ToString()) + ", " + StaticClass.AdvtCityId + "," + StaticClass.UserId + " , " + StaticClass.CountryId + ", " + StaticClass.Stateid + ", " + StaticClass.TokenId;
            }
            dtDetail = ObjMainClass.fnFillDataTable(str);
            InitilizeAdvertisementGrid();
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgAdvtComman.Rows.Add();
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["AdvtId"];
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["AdvtDisplayName"];
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["AdvtCompanyName"];
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[3].Value = dtDetail.Rows[iCtr]["AdvtFilePath"];
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[4].Value = "";
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[5].Value = dtDetail.Rows[iCtr]["AdvtTypeName"];
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[6].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtStartDate"]);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[7].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtEndDate"]);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[8].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["AdvtTime"]);


                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[6].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[7].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells[8].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                }
                foreach (DataGridViewRow row in dgAdvtComman.Rows)
                {
                    row.Height = 30;
                }
            }
        }
        private void InitilizeAdvertisementGrid()
        {
            if (dgAdvtComman.Rows.Count > 0)
            {
                dgAdvtComman.Rows.Clear();
            }
            if (dgAdvtComman.Columns.Count > 0)
            {
                dgAdvtComman.Columns.Clear();
            }

            dgAdvtComman.Columns.Add("Advtid", "Advt Id");
            dgAdvtComman.Columns["Advtid"].Width = 0;
            dgAdvtComman.Columns["Advtid"].Visible = false;
            dgAdvtComman.Columns["Advtid"].ReadOnly = true;

            dgAdvtComman.Columns.Add("Advt", "Advertisement Name");
            dgAdvtComman.Columns["Advt"].Width = 245;
            dgAdvtComman.Columns["Advt"].Visible = true;
            dgAdvtComman.Columns["Advt"].ReadOnly = true;

            dgAdvtComman.Columns.Add("AdvtComp", "Advt Comp");
            dgAdvtComman.Columns["AdvtComp"].Width = 0;
            dgAdvtComman.Columns["AdvtComp"].Visible = false;
            dgAdvtComman.Columns["AdvtComp"].ReadOnly = true;

            dgAdvtComman.Columns.Add("AdvtLink", "AdvtLink");
            dgAdvtComman.Columns["AdvtLink"].Width = 0;
            dgAdvtComman.Columns["AdvtLink"].Visible = false;
            dgAdvtComman.Columns["AdvtLink"].ReadOnly = true;


            dgAdvtComman.Columns.Add("Play", "Play");
            dgAdvtComman.Columns["Play"].Width = 0;
            dgAdvtComman.Columns["Play"].Visible = false;
            dgAdvtComman.Columns["Play"].ReadOnly = true;


            dgAdvtComman.Columns.Add("Type", "Type");
            dgAdvtComman.Columns["Type"].Width = 200;
            dgAdvtComman.Columns["Type"].Visible = true;
            dgAdvtComman.Columns["Type"].ReadOnly = true;

            dgAdvtComman.Columns.Add("StartDate", "Start Date");
            dgAdvtComman.Columns["StartDate"].Width = 200;
            dgAdvtComman.Columns["StartDate"].Visible = true;
            dgAdvtComman.Columns["StartDate"].ReadOnly = true;

            dgAdvtComman.Columns.Add("EndDate", "End Date");
            dgAdvtComman.Columns["EndDate"].Width = 200;
            dgAdvtComman.Columns["EndDate"].Visible = true;
            dgAdvtComman.Columns["EndDate"].ReadOnly = true;

            dgAdvtComman.Columns.Add("AdvtTime", "Time");
            dgAdvtComman.Columns["AdvtTime"].Width = 200;
            dgAdvtComman.Columns["AdvtTime"].Visible = true;
            dgAdvtComman.Columns["AdvtTime"].ReadOnly = true;

            dgAdvtComman.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        private void dtpScheduleDate_ValueChanged(object sender, EventArgs e)
        {
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            FillAllAdvertisement();
        }

        #endregion
        private void moveUp(DataGridView dgGrid)
        {
            if (dgGrid.RowCount > 0)
            {
                if (dgGrid.SelectedRows.Count > 0)
                {
                    int rowCount = dgGrid.Rows.Count;
                    int index = dgGrid.SelectedCells[0].OwningRow.Index;

                    if (index == 0)
                    {
                        return;
                    }
                    DataGridViewRowCollection rows = dgGrid.Rows;

                    // remove the previous row and add it behind the selected row.
                    DataGridViewRow prevRow = rows[index - 1];
                    rows.Remove(prevRow);
                    prevRow.Frozen = false;
                    rows.Insert(index, prevRow);
                    dgGrid.ClearSelection();

                    SaveSongSequence(dgGrid);
                    dgGrid.Rows[index - 1].Selected = true;
                }
            }
        }
        private void moveDown(DataGridView dgGrid)
        {
            if (dgGrid.RowCount > 0)
            {
                if (dgGrid.SelectedRows.Count > 0)
                {
                    int rowCount = dgGrid.Rows.Count;
                    int index = dgGrid.SelectedCells[0].OwningRow.Index;

                    if (index == (rowCount - 2)) // include the header row
                    {
                        return;
                    }
                    DataGridViewRowCollection rows = dgGrid.Rows;

                    // remove the next row and add it in front of the selected row.
                    DataGridViewRow nextRow = rows[index + 1];
                    rows.Remove(nextRow);
                    nextRow.Frozen = false;
                    rows.Insert(index, nextRow);
                    dgGrid.ClearSelection();

                    SaveSongSequence(dgGrid);
                    dgGrid.Rows[index + 1].Selected = true;
                }
            }
        }
        private void SaveSongSequence(DataGridView dgGrid)
        {
            string sWr = "";
            if (dgGrid.Rows.Count == 0) return;
            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            OleDbCommand cmdDelAdvt = new OleDbCommand();
            cmdDelAdvt.Connection = StaticClass.LocalCon;
            cmdDelAdvt.CommandText = "delete from TitlesInPlaylists  where PlaylistID= " + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + "";
            cmdDelAdvt.ExecuteNonQuery();
            StaticClass.LocalCon.Close();
            int Srno = 0;
            for (int i = 0; i <= dgGrid.Rows.Count - 1; i++)
            {
                Srno = Srno + 1;
                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                sWr = "insert into TitlesInPlaylists values (" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " , ";
                sWr = sWr + Convert.ToInt32(dgGrid.Rows[i].Cells[0].Value) + " , " + Srno + ")";
                StaticClass.LocalCon.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = StaticClass.LocalCon;
                cmd.CommandText = sWr;
                cmd.ExecuteNonQuery();
            }
            //if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
            //{
            //    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
            //}
            //else
            //{
            //    PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
            //}
            if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
            {
                rCount = 0;
                //DropSongLength = "";
                //IsSongDropAdvt = false;
                label7.Text = "0";
                label8.Text = "0";
                label14.Text = "0";
                IsAdvtTimeGet = false;
                GrossTotaltime = 0;
                timGetRemainAdvtTime.Enabled = true;
                if (IsSongDropAdvt == false)
                {

                    if (musicPlayer2.URL != "")
                    {
                        Song_Set_foucsPlayer2();
                        if (IsVisibleSong == true)
                        {
                            if (LastRowId == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                            }
                        }
                        else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                        {
                            if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId)].Cells[0].Value.ToString());
                            }
                        }
                        else
                        {
                            if (CurrentRow == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                            }
                        }
                    }
                    else
                    {
                        Song_Set_foucsPlayer();
                        if (IsVisibleSong == true)
                        {
                            if (LastRowId == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                            }
                        }
                        else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                        {
                            if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId)].Cells[0].Value.ToString());
                            }

                        }
                        else
                        {
                            if (CurrentRow == dgPlaylist.Rows.Count - 1)
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells[0].Value.ToString());
                            }
                        }
                    }
                }
            }
        }
        private void Song_Set_foucsPlayer2()
        {
            try
            {
                drawLine = false;
                dgPlaylist.Invalidate();
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                    {
                        CurrentRow = i;
                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            IsVisibleSong = true;
                            UpdateHideSong(g.ToString());
                        }
                        else
                        {
                            IsVisibleSong = false;
                            dgPlaylist.CurrentCell = dgPlaylist.Rows[i].Cells[1];
                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionBackColor = Color.FromArgb(20, 162, 175);
                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.White;
                        }

                        lblSongName2.ForeColor = Color.Yellow;
                        lblArtistName2.ForeColor = Color.Yellow;
                        lblMusicTimeTwo.ForeColor = Color.Yellow;
                        lblSongDurationTwo.ForeColor = Color.Yellow;
                        pbarMusic2.ForeColor = Color.Yellow;
                        pbarMusic2.BackColor = Color.FromArgb(9, 130, 154);
                        panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));

                        lblSongName.ForeColor = Color.Gray;
                        lblArtistName.ForeColor = Color.Gray;
                        lblMusicTimeOne.ForeColor = Color.Gray;
                        lblSongDurationOne.ForeColor = Color.Gray;
                        pbarMusic1.ForeColor = Color.Gray;
                        pbarMusic1.BackColor = Color.FromArgb(175, 175, 175);
                        panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                        if (dgHideSongs.Rows.Count > 0)
                        {
                            DeleteParticularHideSong();
                        }
                        dgPlaylist.ClearSelection();
                    }
                }
            }
            catch { }
        }
        private void Song_Set_foucsPlayer()
        {
            try
            {
                drawLine = false;
                dgPlaylist.Invalidate();
                string g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == g.ToString())
                    {
                        CurrentRow = i;
                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            IsVisibleSong = true;
                            UpdateHideSong(g.ToString());
                        }
                        else
                        {
                            IsVisibleSong = false;
                            dgPlaylist.CurrentCell = dgPlaylist.Rows[i].Cells[1];

                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionBackColor = Color.FromArgb(20, 162, 175);
                            dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.White;
                        }
                        lblSongName.ForeColor = Color.Yellow;
                        lblArtistName.ForeColor = Color.Yellow;
                        lblMusicTimeOne.ForeColor = Color.Yellow;
                        lblSongDurationOne.ForeColor = Color.Yellow;
                        pbarMusic1.ForeColor = Color.Yellow;
                        panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));
                        pbarMusic1.BackColor = Color.FromArgb(9, 130, 154);

                        lblSongName2.ForeColor = Color.Gray;
                        lblArtistName2.ForeColor = Color.Gray;
                        lblMusicTimeTwo.ForeColor = Color.Gray;
                        lblSongDurationTwo.ForeColor = Color.Gray;
                        pbarMusic2.ForeColor = Color.Gray;
                        pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
                        panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                        if (dgHideSongs.Rows.Count > 0)
                        {
                            DeleteParticularHideSong();
                        }
                        dgPlaylist.ClearSelection();
                    }
                }

            }
            catch
            {
            }
        }
        string g = "";
        private void GetCurrentRow()
        {
            string SongName = "";
            if (musicPlayer2.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);

                SongName = g.ToString();
            }
            else if (musicPlayer1.URL != "")
            {
                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                SongName = g.ToString();
            }
            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
            {
                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == SongName)
                {
                    CurrentRow = i;
                    break;
                }
            }
        }

        private void timGetRemainAdvtTime_Tick(object sender, EventArgs e)
        {
            try
            {
                if (StaticClass.IsAdvtManual == true)
                {

                    if (panAdvt.Visible == false)
                    {
                        if (IsAdvtTimeGet == false)
                        {
                            Int32 TempTime = 0;
                            TempTime = GetTotalAdvtTime(rCount);
                            if (TempTime < StaticClass.AdvtTime)
                            {
                                rCount = rCount + 1;
                                TempTime = GetTotalAdvtTime(rCount);
                            }
                            if (TempTime >= StaticClass.AdvtTime)
                            {
                                rCount = 0;
                                IsAdvtTimeGet = true;
                            }
                            GrossTotaltime = TempTime;
                            if (musicPlayer1.URL != "")
                            {
                                GrossTotaltime = GrossTotaltime - Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
                            }
                            else if (musicPlayer2.URL != "")
                            {
                                GrossTotaltime = GrossTotaltime - Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);
                            }
                        }
                    }


                    GrossTotaltime = GrossTotaltime - 1;
                    double t1 = Math.Floor(GrossTotaltime);
                    //double w1 = Math.Floor(timeLeft);
                    double mint1 = Math.Floor(t1 / 60);
                    double s1;
                    int r1;
                    s1 = Convert.ToInt16(Math.Abs(t1 / 60));
                    r1 = Convert.ToInt16(t1 % 60);
                    //--------------------------------------------//
                    //--------------------------------------------//

                    double fd;
                    fd = Math.Floor(GrossTotaltime);
                    double zh;
                    zh = fd / 60;
                    double left = System.Math.Floor(zh);
                    double sec2 = fd % 60;
                    //--------------------------------------------//
                    //--------------------------------------------//

                    lblAdvtTimeRemain.Text = mint1.ToString("00") + ":" + r1.ToString("00");
                    //lblAdvtTimeRemain.Text = GrossTotaltime.ToString() ;
                }
                else
                {
                    if (musicPlayer1.URL != "")
                    {
                        lblAdvtTimeRemain.Text = lblMusicTimeOne.Text;
                    }
                    if (musicPlayer2.URL != "")
                    {
                        lblAdvtTimeRemain.Text = lblMusicTimeTwo.Text;
                    }
                    //panUpcomingAdvt.Visible = false;

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        public Int32 GetTotalAdvtTime(int numberTotal)
        {
            //label14.Text = "0";
            label7.Text = "0";
            label8.Text = "0";
            Int32 LastRowIdAdvt = 0;

            //if (IsVisibleSong == true)
            //{
            //    if (LastRowId == dgPlaylist.Rows.Count - 1)
            //    {
            //        label7.Text = dgPlaylist.Rows[Convert.ToInt32(0)].Cells["Length"].Value.ToString();
            //    }
            //    else
            //    {
            //        label7.Text = dgPlaylist.Rows[Convert.ToInt32(LastRowId + numberTotal)].Cells["Length"].Value.ToString();
            //    }
            //}
            //else
            if (CurrentRow >= dgPlaylist.Rows.Count - 1)
            {
                if (LastRowId == 0)
                {
                    string SongName = "";
                    if (musicPlayer2.URL != "")
                    {
                        g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                        SongName = g.ToString();
                    }
                    else if (musicPlayer1.URL != "")
                    {
                        g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                        SongName = g.ToString();
                    }
                    for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                    {
                        if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == SongName)
                        {
                            LastRowIdAdvt = i;
                            break;
                        }
                    }


                    if ((IsSongDropAdvt == true) && (numberTotal == 0))
                    {
                        label7.Text = DropSongLength;
                    }
                    else if (LastRowIdAdvt + 1 + numberTotal <= dgPlaylist.Rows.Count - 1)
                    {
                        if (numberTotal == 0)
                        {
                            label7.Text = dgPlaylist.Rows[Convert.ToInt32(LastRowIdAdvt + 1 + numberTotal)].Cells["Length"].Value.ToString();
                        }
                        else
                        {
                            label7.Text = dgPlaylist.Rows[Convert.ToInt32(LastRowIdAdvt + numberTotal)].Cells["Length"].Value.ToString();
                        }
                    }
                    else if (LastRowIdAdvt + 1 + numberTotal >= dgPlaylist.Rows.Count - 1)
                    {
                        if ((IsSongDropAdvt == true) && (numberTotal != 0))
                        {
                            label7.Text = dgPlaylist.Rows[Convert.ToInt32(numberTotal - 1)].Cells["Length"].Value.ToString();
                        }
                        else
                        {
                            label7.Text = dgPlaylist.Rows[Convert.ToInt32(numberTotal)].Cells["Length"].Value.ToString();
                        }
                    }
                    else
                    {
                        label7.Text = dgPlaylist.Rows[Convert.ToInt32(LastRowIdAdvt + 1 + numberTotal)].Cells["Length"].Value.ToString();
                    }

                }
                else
                {
                    if (LastRowId + numberTotal == dgPlaylist.Rows.Count - 1)
                    {
                        label7.Text = dgPlaylist.Rows[Convert.ToInt32(0)].Cells["Length"].Value.ToString();
                    }
                    else
                    {
                        label7.Text = dgPlaylist.Rows[Convert.ToInt32(LastRowId + numberTotal)].Cells["Length"].Value.ToString();
                    }
                }
            }
            else
            {
                if (numberTotal != 0)
                {
                    if (IsSongDropAdvt == false)
                    {
                        if (CurrentRow + 1 + numberTotal >= dgPlaylist.Rows.Count)
                        {
                            label7.Text = dgPlaylist.Rows[Convert.ToInt32(0)].Cells["Length"].Value.ToString();
                        }
                        else
                        {
                            label7.Text = dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1 + numberTotal)].Cells["Length"].Value.ToString();
                        }
                    }
                    else
                    {
                        if (LastRowId == 0)
                        {
                            string SongName = "";
                            if (musicPlayer2.URL != "")
                            {
                                g = Path.GetFileNameWithoutExtension(musicPlayer2.URL);
                                SongName = g.ToString();
                            }
                            else if (musicPlayer1.URL != "")
                            {
                                g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);

                                SongName = g.ToString();
                            }
                            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                            {
                                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == SongName)
                                {
                                    LastRowIdAdvt = i;
                                    break;
                                }
                            }
                            if (LastRowIdAdvt + numberTotal >= dgPlaylist.Rows.Count - 1)
                            {
                                label7.Text = dgPlaylist.Rows[Convert.ToInt32(0)].Cells["Length"].Value.ToString();
                            }
                            else
                            {
                                label7.Text = dgPlaylist.Rows[Convert.ToInt32(LastRowIdAdvt + numberTotal)].Cells["Length"].Value.ToString();
                            }

                        }
                        else
                        {
                            if (LastRowId + 1 + numberTotal >= dgPlaylist.Rows.Count - 1)
                            {
                                label7.Text = dgPlaylist.Rows[Convert.ToInt32(numberTotal)].Cells["Length"].Value.ToString();
                            }
                            else
                            {
                                label7.Text = dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1 + numberTotal)].Cells["Length"].Value.ToString();
                            }
                        }
                    }
                }
                else if (CurrentRow == dgPlaylist.Rows.Count - 1)
                {
                    label7.Text = dgPlaylist.Rows[Convert.ToInt32(0)].Cells["Length"].Value.ToString();
                }
                else
                {
                    if (numberTotal == 0)
                    {
                        if (IsSongDropAdvt == true)
                        {
                            label7.Text = DropSongLength;
                        }
                        else if (CurrentRow + 1 <= dgPlaylist.Rows.Count - 1)
                        {
                            label7.Text = dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells["Length"].Value.ToString();
                        }
                        else if (CurrentRow + 1 >= dgPlaylist.Rows.Count - 1)
                        {
                            label7.Text = dgPlaylist.Rows[Convert.ToInt32(0)].Cells["Length"].Value.ToString();
                        }

                        else
                        {
                            label7.Text = dgPlaylist.Rows[Convert.ToInt32(CurrentRow + 1)].Cells["Length"].Value.ToString();
                        }
                    }
                }
            }

            if (numberTotal == 0)
            {
                if (IsSongDropAdvt == true)
                {
                    if (musicPlayer1.URL != "")
                    {
                        label8.Text = (Math.Floor(musicPlayer1.currentMedia.duration)).ToString();
                    }
                    else if (musicPlayer2.URL != "")
                    {
                        label8.Text = (Math.Floor(musicPlayer2.currentMedia.duration)).ToString();
                    }
                    label14.Text = (Convert.ToInt32(label14.Text) + ((Convert.ToInt32(label8.Text)))).ToString();
                }
                else
                {
                    label8.Text = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["Length"].Value.ToString();
                    string strCurrent = label8.Text.ToString();
                    string[] arr2 = strCurrent.Split(':');
                    label14.Text = (Convert.ToInt32(label14.Text) + ((Convert.ToInt32(arr2[0]) * 60)) + Convert.ToInt32(arr2[1])).ToString();
                }
                if (Convert.ToInt32(label14.Text) >= 300)
                {
                    return Convert.ToInt32(label14.Text);
                }

            }
            string strNext = label7.Text.ToString();
            string[] arr = strNext.Split(':');
            label14.Text = (Convert.ToInt32(label14.Text) + ((Convert.ToInt32(arr[0]) * 60)) + Convert.ToInt32(arr[1])).ToString();
            return Convert.ToInt32(label14.Text);
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    if (dgPlaylist.CurrentCell.RowIndex == -1) return;
                    //PopulateShuffleSong(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    // SaveSongSequence(dgPlaylist);
                    PopulateNewShuffle(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                }
                else
                {
                    if (dgOtherPlaylist.CurrentCell.RowIndex == -1) return;
                    //PopulateShuffleSong(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    //SaveSongSequence(dgOtherPlaylist);
                    PopulateNewShuffle(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                }
            }
            catch { }
        }






        #region NewShuffle
        private void PopulateNewShuffle(DataGridView dgGrid, Int32 currentPlayRow)
        {
            try
            {
                string mlsSql = "";
                string GetLocalPath = "";
                string TitleYear = "";
                string TitleTime = "";
                var Special_Name = "";
                string Special_Change = "";
                Int32 iCtr = 0;
                Int32 srNo = 0;
                DataTable dtDetail;
                mlsSql = "SELECT  Titles.TitleID, ltrim(Titles.Title) as Title, Titles.Time,Albums.Name AS AlbumName ,";
                mlsSql = mlsSql + " Titles.TitleYear as TitleYear ,  ltrim(Artists.Name) as ArtistName  FROM ((( TitlesInPlaylists  ";
                mlsSql = mlsSql + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID )  ";
                mlsSql = mlsSql + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ) ";
                mlsSql = mlsSql + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ) ";
                mlsSql = mlsSql + " where TitlesInPlaylists.PlaylistID=" + Convert.ToInt32(currentPlayRow) + " ORDER BY   Rnd(-(100000*Titles.TitleID)*Time())";
                dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                InitilizeGrid(dgGrid);


                if ((dtDetail.Rows.Count > 0))
                {
                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {
                        GetLocalPath = dtDetail.Rows[iCtr]["TitleID"] + ".mp3";
                        srNo = iCtr;
                        dgGrid.Rows.Add();
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["songid"].Value = dtDetail.Rows[iCtr]["TitleID"];

                        Special_Name = "";
                        Special_Change = "";
                        Special_Name = dtDetail.Rows[iCtr]["Title"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["songname"].Value = Special_Change;

                        string str = dtDetail.Rows[iCtr]["Time"].ToString();
                        string[] arr = str.Split(':');
                        TitleTime = arr[1] + ":" + arr[2];

                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Length"].Value = TitleTime;

                        Special_Name = "";
                        Special_Change = "";

                        Special_Name = dtDetail.Rows[iCtr]["AlbumName"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Album"].Value = Special_Change;

                        TitleYear = dtDetail.Rows[iCtr]["TitleYear"].ToString();
                        if (TitleYear == "0")
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Year"].Value = "- - -";
                        }
                        else
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Year"].Value = dtDetail.Rows[iCtr]["TitleYear"];
                        }

                        Special_Name = "";
                        Special_Change = "";

                        Special_Name = dtDetail.Rows[iCtr]["ArtistName"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Artist"].Value = Special_Change;

                        if (this.Width.ToString() == "1020")
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 11);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Style.Font = new Font("Segoe UI", 11);
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        }
                    }
                }


                #region "Next Song"
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    rCount = 0;
                    label7.Text = "0";
                    label8.Text = "0";
                    label14.Text = "0";
                    IsAdvtTimeGet = false;
                    GrossTotaltime = 0;
                    timGetRemainAdvtTime.Enabled = true;
                    if (IsSongDropAdvt == false)
                    {
                        if (musicPlayer2.URL != "")
                        {
                            Song_Set_foucsPlayer2();
                            if (IsVisibleSong == true)
                            {
                                if (LastRowId == dgPlaylist.Rows.Count - 1)
                                {
                                    NextSongShow2(0);
                                }
                                else
                                {
                                    NextSongShow2(LastRowId + 1);
                                }
                            }
                            else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                            {
                                if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                                {
                                    NextSongShow2(0);
                                }
                                else
                                {
                                    NextSongShow2(LastRowId);
                                }
                            }
                            else
                            {
                                if (CurrentRow == dgPlaylist.Rows.Count - 1)
                                {
                                    NextSongShow2(0);
                                }
                                else
                                {
                                    NextSongShow2(CurrentRow + 1);
                                }
                            }
                        }
                        else
                        {
                            Song_Set_foucsPlayer();
                            if (IsVisibleSong == true)
                            {
                                if (LastRowId == dgPlaylist.Rows.Count - 1)
                                {
                                    NextSongShow(0);
                                }
                                else
                                {
                                    NextSongShow(LastRowId + 1);
                                }
                            }
                            else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                            {
                                if (LastRowId + 1 == dgPlaylist.Rows.Count - 1)
                                {
                                    NextSongShow(0);
                                }
                                else
                                {
                                    NextSongShow(LastRowId);
                                }

                            }
                            else
                            {
                                if (CurrentRow == dgPlaylist.Rows.Count - 1)
                                {
                                    NextSongShow(0);
                                }
                                else
                                {
                                    NextSongShow(CurrentRow + 1);
                                }
                            }
                        }
                    }
                }
                #endregion



                foreach (DataGridViewRow row in dgGrid.Rows)
                {
                    row.Height = 33;
                }



                RowHide();

                dgSaveDataGrid = dgGrid;
                SaveDataCurrentPlaylistId = Convert.ToInt32(currentPlayRow);
                if (bgSaveDataWorker.IsBusy == false)
                {
                    btnShuffle.Enabled = false;
                    bgSaveDataWorker.RunWorkerAsync();
                }
            }



            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void NextSongShow2(Int32 NextCurrentRow)
        {
            try
            {
                var Special_Name = "";
                string Special_Change = "";

                Special_Name = "";
                Special_Change = "";
                Special_Name = dgPlaylist.Rows[NextCurrentRow].Cells["songname"].Value.ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblSongName.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = dgPlaylist.Rows[NextCurrentRow].Cells["Artist"].Value.ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblArtistName.Text = Special_Change;


                string str = dgPlaylist.Rows[NextCurrentRow].Cells["Length"].Value.ToString();
                string[] arr = str.Split(':');
                DropSongLength = arr[1] + ":" + arr[2];

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void NextSongShow(Int32 NextCurrentRow)
        {
            try
            {
                var Special_Name = "";
                string Special_Change = "";

                Special_Name = "";
                Special_Change = "";
                Special_Name = dgPlaylist.Rows[NextCurrentRow].Cells["songname"].Value.ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblSongName2.Text = Special_Change;

                Special_Name = "";
                Special_Change = "";
                Special_Name = dgPlaylist.Rows[NextCurrentRow].Cells["Artist"].Value.ToString();
                Special_Change = Special_Name.Replace("??$$$??", "'");
                lblArtistName2.Text = Special_Change;

                UpcomingSongPlayerOne = "";
                UpcomingSongPlayerTwo = dgPlaylist.Rows[NextCurrentRow].Cells["TitleId"].Value.ToString();

                string str = dgPlaylist.Rows[NextCurrentRow].Cells["Length"].Value.ToString();
                string[] arr = str.Split(':');
                DropSongLength = arr[1] + ":" + arr[2];

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        private void bgSaveDataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string sWr = "";
            if (dgSaveDataGrid.Rows.Count == 0) return;
            if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
            StaticClass.LocalCon.Open();
            OleDbCommand cmdDelAdvt = new OleDbCommand();
            cmdDelAdvt.Connection = StaticClass.LocalCon;
            cmdDelAdvt.CommandText = "delete from TitlesInPlaylists  where PlaylistID= " + Convert.ToInt32(SaveDataCurrentPlaylistId) + "";
            cmdDelAdvt.ExecuteNonQuery();
            StaticClass.LocalCon.Close();
            int Srno = 0;
            for (int i = 0; i <= dgSaveDataGrid.Rows.Count - 1; i++)
            {
                Srno = Srno + 1;
                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                sWr = "insert into TitlesInPlaylists values (" + Convert.ToInt32(SaveDataCurrentPlaylistId) + " , ";
                sWr = sWr + Convert.ToInt32(dgSaveDataGrid.Rows[i].Cells[0].Value) + " , " + Srno + ")";
                StaticClass.LocalCon.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = StaticClass.LocalCon;
                cmd.CommandText = sWr;
                cmd.ExecuteNonQuery();
            }

        }

        private void bgSaveDataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bgSaveDataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GC.Collect();
            btnShuffle.Enabled = true;
        }
        #endregion





        #region "AdvtMainPage"
        private void tbcMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (tbcMain.SelectedIndex == 2)
                {
                    string strAdvt = "";
                    if (StaticClass.IsAdvt == false)
                    {
                        lblFitnessLicence.Dock = DockStyle.Fill;
                        lblFitnessLicence.Visible = true;
                        dgDailyAdvt.Visible = false;
                        return;
                    }
                    strAdvt = "spGetAdvtExpiryStatus " + StaticClass.TokenId + ", 1 ";
                    DataSet dsExpire = new DataSet();
                    dsExpire = ObjMainClass.fnFillDataSet(strAdvt);

                    if (dsExpire.Tables[0].Rows[0]["ExpiryAdvtStatus"].ToString() == "NoLic")
                    {
                        lblFitnessLicence.Dock = DockStyle.Fill;
                        lblFitnessLicence.Visible = true;
                        dgDailyAdvt.Visible = false;
                        return;
                    }
                    if (dsExpire.Tables[0].Rows[0]["ExpiryAdvtStatus"].ToString() == "Yes")
                    {
                        lblFitnessLicence.Dock = DockStyle.Fill;
                        lblFitnessLicence.Visible = true;
                        dgDailyAdvt.Visible = false;
                        return;
                    }
                    dgDailyAdvt.Visible = true;
                    lblFitnessLicence.Dock = DockStyle.None;
                    lblFitnessLicence.Visible = false;
                    FillDailyAdvertisement();
                }
            }
            catch { return; }
        }


        private void FillDailyAdvertisement()
        {
            string PlayerType = "";
            PlayerType = "Copyright";
            string str = "";
            int iCtr;
            DataTable dtDetail;
            InitilizeDailyAdvertisement();
            if (StaticClass.IsAdvtManual == true)
            {
                str = "spGetAdvtManual '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','" + PlayerType + "'," + StaticClass.UserId + "," + ReturnWeekId(DateTime.Now.DayOfWeek.ToString()) + ", " + StaticClass.AdvtCityId + "," + StaticClass.UserId + " , " + StaticClass.CountryId + ", " + StaticClass.Stateid + ", " + StaticClass.TokenId;
            }
            else
            {
                str = "spGetAdvtAdmin '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','" + PlayerType + "'," + StaticClass.UserId + "," + ReturnWeekId(DateTime.Now.DayOfWeek.ToString()) + ", " + StaticClass.AdvtCityId + "," + StaticClass.UserId + " , " + StaticClass.CountryId + ", " + StaticClass.Stateid + ", " + StaticClass.TokenId;
            }
            dtDetail = ObjMainClass.fnFillDataTable(str);

            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgDailyAdvt.Rows.Add();
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["AdvtId"];
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["AdvtDisplayName"];
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["AdvtCompanyName"];
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[3].Value = dtDetail.Rows[iCtr]["AdvtFilePath"];
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[4].Value = "";

                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[5].Value = dtDetail.Rows[iCtr]["AdvtTypeName"];
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[6].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtStartDate"]);
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[7].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtEndDate"]);
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[8].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["AdvtTime"]);


                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[6].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[7].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgDailyAdvt.Rows[dgDailyAdvt.Rows.Count - 1].Cells[8].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                }
                foreach (DataGridViewRow row in dgDailyAdvt.Rows)
                {
                    row.Height = 30;
                }
            }
        }
        private void InitilizeDailyAdvertisement()
        {
            if (dgDailyAdvt.Rows.Count > 0)
            {
                dgDailyAdvt.Rows.Clear();
            }
            if (dgDailyAdvt.Columns.Count > 0)
            {
                dgDailyAdvt.Columns.Clear();
            }

            dgDailyAdvt.Columns.Add("Advtid", "Advt Id");
            dgDailyAdvt.Columns["Advtid"].Width = 0;
            dgDailyAdvt.Columns["Advtid"].Visible = false;
            dgDailyAdvt.Columns["Advtid"].ReadOnly = true;

            dgDailyAdvt.Columns.Add("Advt", "Name");
            dgDailyAdvt.Columns["Advt"].Width = 260;
            dgDailyAdvt.Columns["Advt"].Visible = true;
            dgDailyAdvt.Columns["Advt"].ReadOnly = true;

            dgDailyAdvt.Columns.Add("AdvtComp", "Advt Comp");
            dgDailyAdvt.Columns["AdvtComp"].Width = 0;
            dgDailyAdvt.Columns["AdvtComp"].Visible = false;
            dgDailyAdvt.Columns["AdvtComp"].ReadOnly = true;

            dgDailyAdvt.Columns.Add("AdvtLink", "AdvtLink");
            dgDailyAdvt.Columns["AdvtLink"].Width = 0;
            dgDailyAdvt.Columns["AdvtLink"].Visible = false;
            dgDailyAdvt.Columns["AdvtLink"].ReadOnly = true;


            dgDailyAdvt.Columns.Add("Play", "Play");
            dgDailyAdvt.Columns["Play"].Width = 0;
            dgDailyAdvt.Columns["Play"].Visible = false;
            dgDailyAdvt.Columns["Play"].ReadOnly = true;


            dgDailyAdvt.Columns.Add("Type", "Type");
            dgDailyAdvt.Columns["Type"].Width = 0;
            dgDailyAdvt.Columns["Type"].Visible = false;
            dgDailyAdvt.Columns["Type"].ReadOnly = true;

            dgDailyAdvt.Columns.Add("StartDate", "Start Date");
            dgDailyAdvt.Columns["StartDate"].Width = 0;
            dgDailyAdvt.Columns["StartDate"].Visible = false;
            dgDailyAdvt.Columns["StartDate"].ReadOnly = true;

            dgDailyAdvt.Columns.Add("EndDate", "End Date");
            dgDailyAdvt.Columns["EndDate"].Width = 0;
            dgDailyAdvt.Columns["EndDate"].Visible = false;
            dgDailyAdvt.Columns["EndDate"].ReadOnly = true;

            dgDailyAdvt.Columns.Add("AdvtTime", "Time");
            dgDailyAdvt.Columns["AdvtTime"].Width = 100;
            dgDailyAdvt.Columns["AdvtTime"].Visible = true;
            dgDailyAdvt.Columns["AdvtTime"].ReadOnly = true;

            dgDailyAdvt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        #endregion

        private void tbcPlaylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbcPlaylist.SelectedIndex == 0)// Playlist
            {
            }
            if (tbcPlaylist.SelectedIndex == 1)// Top250
            {
                FillTop250Grid();
            }
            if (tbcPlaylist.SelectedIndex == 2)// Last100
            {
                FillLast100Grid();
            }
        }

        private void picManual_Click(object sender, EventArgs e)
        {
            string file;
            file = Application.StartupPath + "\\MCR.mht";
            if (!File.Exists(file))
            {
                #region DownloadPdfFile
                string localPath = Application.StartupPath + "\\MCR.mht";
                string UpdateFileLocation = "ftp://85.195.82.94:21//MusicPlayerSetup/pdfFile/MCR.mht";
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
            panManual.Width = this.Width;
            panManual.Height = this.Height;
            panManual.Location = new Point(0, 0);
            webManual.Location = new Point(0, 0);
            webManual.Width = panPdf.Width - 10;
            webManual.Height = panPdf.Height - 30;
            panManual.Visible = true;
            String appdir = Path.GetDirectoryName(Application.ExecutablePath);
            String myfile = Path.Combine(appdir, "MCR.mht");
            webManual.Url = new Uri("file:///" + myfile);
        }

        private void picBack_Click(object sender, EventArgs e)
        {
            panManual.Visible = false;
        }

        string StreamTime = "";
        int StreamRow = 0;
        int StartStream = 0;
        private void timGetStreamCurrenttime_Tick(object sender, EventArgs e)
        {
            if (StreamTime != string.Format("{0:h:mm tt}", DateTime.Now))
            {
                StreamTime = string.Format("{0:h:mm tt}", DateTime.Now);
                for (int iRow = 0; iRow < dgAllStreamShedule.Rows.Count; iRow++)
                {
                    if (StaticClass.AdvtCityId == Convert.ToInt32(dgAllStreamShedule.Rows[iRow].Cells["cityid"].Value))
                    {
                        if (StaticClass.DealerCode == dgAllStreamShedule.Rows[iRow].Cells["dCode"].Value.ToString())
                        {
                            if (Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date)) >= Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dgAllStreamShedule.Rows[iRow].Cells["sDate"].Value)))
                            {
                                if (Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date)) <= Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dgAllStreamShedule.Rows[iRow].Cells["eDate"].Value)))
                                {
                                    if (Convert.ToDateTime(string.Format("{0:h:mm tt}", DateTime.Now)) == Convert.ToDateTime(string.Format("{0:h:mm tt}", dgAllStreamShedule.Rows[iRow].Cells["sTime"].Value)))
                                    {
                                        StreamRow = iRow;
                                        StartStream = 0;
                                        timStartStreamShedule.Enabled = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        private void timStartStreamShedule_Tick(object sender, EventArgs e)
        {
        RunAg:
            if (StartStream != 0)
            {
                StartStream = StartStream - 1;
            }
            if (StartStream == 0)
            {
                if ((musicPlayer1.URL != "") && (musicPlayer2.URL != ""))
                {
                    StartStream = 20;
                    goto RunAg;
                }
                if (panAdvt.Visible == true)
                {
                    StartStream = 10;
                }
                //                if (ClientsRightsValidation() == false) return;
                if (dgStream.Rows.Count == 0)
                {
                    //MessageBox.Show("The selected stream is not available at the moment.", "Copyright Player");
                    return;
                }
                lblStreamTime.Text = "";

                if (musicPlayer1.URL != "")
                {
                    musicPlayer1.Ctlcontrols.pause();
                }
                else if (musicPlayer2.URL != "")
                {
                    musicPlayer2.Ctlcontrols.pause();
                }
                if (dgAllStreamShedule.Rows.Count > 0)
                {
                    for (int i = 0; i <= dgStream.Rows.Count - 1; i++)
                    {

                        if (Convert.ToInt32(dgAllStreamShedule.Rows[StreamRow].Cells["streamId"].Value) == Convert.ToInt32(dgStream.Rows[i].Cells["streamId"].Value))
                        {
                            lblStreamName.Text = dgStream.Rows[i].Cells["StreamName"].Value.ToString();
                            StreamMusicPlayer.URL = dgStream.Rows[i].Cells["StreamLink"].Value.ToString();
                            if (btnMute.Text == ".")
                            {
                                if (btnStreamMute.Text == "")
                                {
                                    btnStreamMute.Text = ".";
                                    StreamMusicPlayer.settings.mute = true;
                                    btnStreamMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_red));
                                }
                            }

                            dgStream.CurrentCell = dgStream.Rows[i].Cells["StreamName"];
                            dgStream.Rows[i].Selected = true;
                            dgStream.Rows[i].Cells["Column_Img_Stream"].Value = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
                            goto Done;
                        }

                    }
                }

                lblStreamName.Text = dgStream.Rows[0].Cells["StreamName"].Value.ToString();
                StreamMusicPlayer.URL = dgStream.Rows[0].Cells["StreamLink"].Value.ToString();
                if (btnMute.Text == ".")
                {
                    if (btnStreamMute.Text == "")
                    {
                        btnStreamMute.Text = ".";
                        StreamMusicPlayer.settings.mute = true;
                        btnStreamMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_red));
                    }
                }
                dgStream.Rows[0].Selected = true;
                dgStream.Rows[0].Cells["Column_Img_Stream"].Value = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
            Done:
                panStream.Visible = true;
                if (panStreamShedule.Visible == false & panAdvtDetail.Visible == false & panAllStreamShedule.Visible == false & panManual.Visible == false & panPrayer.Visible == false)
                {
                    panStream.BringToFront();
                }
                StreamMusicPlayer.Ctlcontrols.play();
                panStream.Location = new Point(0, 0);
                panStream.Height = this.Height;
                panStream.Width = this.Width;
                btnStreamPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
                btnStreamPlay.Text = "";
                GetCurrentStream();
                timStream.Enabled = true;
                timStreamMusicTime.Enabled = true;
                timStartStreamShedule.Enabled = false;

            }
        }

        #region "Save Stream Shedule"
        Int32 ModifyStreamSheduleId = 0;
        DataGridViewLinkColumn EditStreamShedule = new DataGridViewLinkColumn();
        DataGridViewLinkColumn DeleteStreamShedule = new DataGridViewLinkColumn();
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SubmitValidationStream() == false) return;
            SaveRecord();
        }
        private void btnRefersh_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void dtpFromDate_ValueChanged(object sender, EventArgs e)
        {
            FillClientStreamShedule("GetClientStreamSheduling '" + string.Format("{0:dd/MMM/yyyy}", dtpFromDate.Value) + "'," + StaticClass.TokenId);
        }
        private Boolean SubmitValidationStream()
        {

            if (StaticClass.AdvtCityId == 0)
            {
                MessageBox.Show("Your token information is not found. Please contact your dealer.", "Copyright Player");
                cmbStreamName.Focus();
                return false;
            }
            if (dtpEndDate.Value < dtpStartDate.Value)
            {
                MessageBox.Show("Please select proper date's", "Copyright Player");
                dtpEndDate.Focus();
                return false;
            }
            if (Convert.ToInt32(cmbStreamName.SelectedValue) == 0)
            {
                MessageBox.Show("Stream name cannot be balnk", "Copyright Player");
                cmbStreamName.Focus();
                return false;
            }
            if (cmbHour.Text == "")
            {
                MessageBox.Show("Please select the hour", "Copyright Player");
                cmbHour.Focus();
                return false;
            }
            if (cmbMin.Text == "")
            {
                MessageBox.Show("Please select minute", "Copyright Player");
                cmbMin.Focus();
                return false;
            }
            if (cmbAMPM.Text == "")
            {
                MessageBox.Show("Please select time type", "Copyright Player");
                cmbAMPM.Focus();
                return false;
            }
            string GetComboTimeString = "";
            GetComboTimeString = cmbHour.Text + ":" + cmbMin.Text + " " + cmbAMPM.Text;
            DateTime GetComboTime = DateTime.ParseExact(GetComboTimeString, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);

            //string strDealerTimeValid = "";
            //if (btnSave.Text == "Save")
            //{
            //    strDealerTimeValid = "select * from tbStreamScheduling where tokenid=0 and  cityid=" + StaticClass.AdvtCityId + " and  dealercode='" + StaticClass.DealerCode + "' and ";
            //    strDealerTimeValid = strDealerTimeValid + " StartTime='" + GetComboTime.ToString("hh:mm tt") + "' ";
            //    strDealerTimeValid = strDealerTimeValid + " and ('" + dtpStartDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate";
            //    strDealerTimeValid = strDealerTimeValid + " or '" + dtpEndDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate )";
            //}
            //else
            //{
            //    strDealerTimeValid = "select * from tbStreamScheduling where tokenid=0 and cityid=" + StaticClass.AdvtCityId + " and  dealercode='" + StaticClass.DealerCode + "' and ";
            //    strDealerTimeValid = strDealerTimeValid + " StartTime='" + GetComboTime.ToString("hh:mm tt") + "' ";
            //    strDealerTimeValid = strDealerTimeValid + " and ('" + dtpStartDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate";
            //    strDealerTimeValid = strDealerTimeValid + " or '" + dtpEndDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate )";
            //    strDealerTimeValid = strDealerTimeValid + " and SheduleId   <> " + ModifyStreamSheduleId;
            //}
            //DataTable dtDealerTimeValid = new DataTable();
            //dtDealerTimeValid = ObjMainClass.fnFillDataTable(strDealerTimeValid);
            //if (dtDealerTimeValid.Rows.Count > 0)
            //{
            //    MessageBox.Show("This time is already used", "Copyright Player");
            //    cmbMin.Focus();
            //    return false;
            //}

            //strDealerTimeValid = "";
            //if (btnSave.Text == "Save")
            //{
            //    strDealerTimeValid = "select * from tbStreamScheduling where tokenid=0 and cityid=" + StaticClass.AdvtCityId + " and  dealercode='" + StaticClass.DealerCode + "' and ";
            //    strDealerTimeValid = strDealerTimeValid + " StreamId=" + Convert.ToInt32(cmbStreamName.SelectedValue) + " ";
            //    strDealerTimeValid = strDealerTimeValid + " and ('" + dtpStartDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate";
            //    strDealerTimeValid = strDealerTimeValid + " or '" + dtpEndDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate )";
            //}
            //else
            //{
            //    strDealerTimeValid = "select * from tbStreamScheduling where tokenid=0 and cityid=" + StaticClass.AdvtCityId + " and  dealercode='" + StaticClass.DealerCode + "' and ";
            //    strDealerTimeValid = strDealerTimeValid + " StreamId=" + Convert.ToInt32(cmbStreamName.SelectedValue) + " ";
            //    strDealerTimeValid = strDealerTimeValid + " and ('" + dtpStartDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate";
            //    strDealerTimeValid = strDealerTimeValid + " or '" + dtpEndDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate )";
            //    strDealerTimeValid = strDealerTimeValid + " and SheduleId   <> " + ModifyStreamSheduleId;
            //}
            //dtDealerTimeValid = new DataTable();
            //dtDealerTimeValid = ObjMainClass.fnFillDataTable(strDealerTimeValid);
            //if (dtDealerTimeValid.Rows.Count > 0)
            //{
            //    MessageBox.Show("This stream is already used in between date's", "Copyright Player");
            //    dtpStartDate.Focus();
            //    return false;
            //}


            string strClientTimeValid = "";
            if (btnSave.Text == "Save")
            {
                strClientTimeValid = "select * from tbStreamSchedulingClient where tokenid=" + StaticClass.TokenId + " and  dealercode='" + StaticClass.DealerCode + "' and ";
                strClientTimeValid = strClientTimeValid + " StartTime='" + GetComboTime.ToString("hh:mm tt") + "' ";
                strClientTimeValid = strClientTimeValid + " and ('" + dtpStartDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate";
                strClientTimeValid = strClientTimeValid + " or '" + dtpEndDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate )";
            }
            else
            {
                strClientTimeValid = "select * from tbStreamSchedulingClient where tokenid=" + StaticClass.TokenId + " and  dealercode='" + StaticClass.DealerCode + "' and ";
                strClientTimeValid = strClientTimeValid + " StartTime='" + GetComboTime.ToString("hh:mm tt") + "' ";
                strClientTimeValid = strClientTimeValid + " and ('" + dtpStartDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate";
                strClientTimeValid = strClientTimeValid + " or '" + dtpEndDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate )";
                strClientTimeValid = strClientTimeValid + " and SheduleId   <> " + ModifyStreamSheduleId;
            }
            DataTable dtClientTimeValid = new DataTable();
            dtClientTimeValid = ObjMainClass.fnFillDataTable(strClientTimeValid);
            if (dtClientTimeValid.Rows.Count > 0)
            {
                MessageBox.Show("This time is already used", "Copyright Player");
                cmbMin.Focus();
                return false;
            }

            //strClientTimeValid = "";
            //if (btnSave.Text == "Save")
            //{
            //    strClientTimeValid = "select * from tbStreamSchedulingClient where tokenid=" + StaticClass.TokenId + " and  dealercode='" + StaticClass.DealerCode + "' and ";
            //    strClientTimeValid = strClientTimeValid + " StreamId=" + Convert.ToInt32(cmbStreamName.SelectedValue) + " ";
            //    strClientTimeValid = strClientTimeValid + " and ('" + dtpStartDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate";
            //    strClientTimeValid = strClientTimeValid + " or '" + dtpEndDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate )";
            //}
            //else
            //{
            //    strClientTimeValid = "select * from tbStreamSchedulingClient where tokenid=" + StaticClass.TokenId + " and  dealercode='" + StaticClass.DealerCode + "' and ";
            //    strClientTimeValid = strClientTimeValid + " StreamId=" + Convert.ToInt32(cmbStreamName.SelectedValue) + " ";
            //    strClientTimeValid = strClientTimeValid + " and ('" + dtpStartDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate";
            //    strClientTimeValid = strClientTimeValid + " or '" + dtpEndDate.Value.ToString("dd/MMM/yyyy") + "' between StartDate and  EndDate )";
            //    strClientTimeValid = strClientTimeValid + " and SheduleId   <> " + ModifyStreamSheduleId;
            //}
            //dtClientTimeValid = new DataTable();
            //dtClientTimeValid = ObjMainClass.fnFillDataTable(strClientTimeValid);
            //if (dtClientTimeValid.Rows.Count > 0)
            //{
            //    MessageBox.Show("This stream is already used in between date's", "Copyright Player");
            //    dtpStartDate.Focus();
            //    return false;
            //}

            return true;
        }
        private void SaveRecord()
        {
            string GetComboTimeString = "";
            GetComboTimeString = cmbHour.Text + ":" + cmbMin.Text + " " + cmbAMPM.Text;
            DateTime GetComboTime = DateTime.ParseExact(GetComboTimeString, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("spStreamSchedulingClient", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@SheduleId", SqlDbType.BigInt));
            cmd.Parameters["@SheduleId"].Value = Convert.ToInt32(ModifyStreamSheduleId);

            cmd.Parameters.Add(new SqlParameter("@TitleCategoryId", SqlDbType.BigInt));
            cmd.Parameters["@TitleCategoryId"].Value = "4";

            cmd.Parameters.Add(new SqlParameter("@AdminCode", SqlDbType.VarChar));
            cmd.Parameters["@AdminCode"].Value = "More000";

            cmd.Parameters.Add(new SqlParameter("@StreamId", SqlDbType.BigInt));
            cmd.Parameters["@StreamId"].Value = Convert.ToInt32(cmbStreamName.SelectedValue);

            cmd.Parameters.Add(new SqlParameter("@StartDate", SqlDbType.DateTime));
            cmd.Parameters["@StartDate"].Value = dtpStartDate.Value.ToString("dd/MMM/yyyy");

            cmd.Parameters.Add(new SqlParameter("@EndDate", SqlDbType.DateTime));
            cmd.Parameters["@EndDate"].Value = dtpEndDate.Value.ToString("dd/MMM/yyyy");

            cmd.Parameters.Add(new SqlParameter("@StartTime", SqlDbType.DateTime));
            cmd.Parameters["@StartTime"].Value = GetComboTime.ToString("hh:mm tt");

            cmd.Parameters.Add(new SqlParameter("@Dealercode", SqlDbType.VarChar));
            cmd.Parameters["@Dealercode"].Value = StaticClass.DealerCode;

            cmd.Parameters.Add(new SqlParameter("@CountryId", SqlDbType.BigInt));
            cmd.Parameters["@CountryId"].Value = StaticClass.CountryId;

            cmd.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
            cmd.Parameters["@StateId"].Value = StaticClass.Stateid;

            cmd.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
            cmd.Parameters["@CityId"].Value = StaticClass.AdvtCityId;

            cmd.Parameters.Add(new SqlParameter("@Tokenid", SqlDbType.BigInt));
            cmd.Parameters["@Tokenid"].Value = StaticClass.TokenId;
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record is save", "Copyright Player");
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Record not save", "!! Problem !!");
                Clear();
                return;
            }
            finally
            {
                StaticClass.constr.Close();
            }
        }
        private void Clear()
        {
            ModifyStreamSheduleId = 0;
            btnSave.Text = "Save";
            cmbHour.Text = "";
            cmbMin.Text = "";
            cmbAMPM.Text = "";
            dtpStartDate.Value = DateTime.Now.Date;
            dtpEndDate.Value = DateTime.Now.Date;
            dtpFromDate.Value = DateTime.Now.Date;
            FillStreamCombo();
            FillClientStreamShedule("GetClientStreamSheduling '" + string.Format("{0:dd/MMM/yyyy}", dtpFromDate.Value) + "'," + StaticClass.TokenId);
            cmbStreamName.Focus();
            FillStreamData();
        }
        private void FillStreamCombo()
        {
            string str = "";
            str = "GetTokenWiseStreams " + StaticClass.UserId + " , " + StaticClass.TokenId + " ,'Copyright'";
            
            ObjMainClass.fnFillAdvtComboBox(str, cmbStreamName, "StreamId", "StreamName", "");
        }
        private void InitilizeGrid()
        {
            if (dgStreamShedule.Rows.Count > 0)
            {
                dgStreamShedule.Rows.Clear();
            }
            if (dgStreamShedule.Columns.Count > 0)
            {
                dgStreamShedule.Columns.Clear();
            }

            dgStreamShedule.Columns.Add("ScheduleId", "Schedule Id");
            dgStreamShedule.Columns["ScheduleId"].Width = 0;
            dgStreamShedule.Columns["ScheduleId"].Visible = false;
            dgStreamShedule.Columns["ScheduleId"].ReadOnly = true;

            dgStreamShedule.Columns.Add("cName", "Category Name");
            dgStreamShedule.Columns["cName"].Width = 140;
            dgStreamShedule.Columns["cName"].Visible = false;
            dgStreamShedule.Columns["cName"].ReadOnly = true;

            dgStreamShedule.Columns.Add("sName", "Stream Name");
            dgStreamShedule.Columns["sName"].Width = 510;
            dgStreamShedule.Columns["sName"].Visible = true;
            dgStreamShedule.Columns["sName"].ReadOnly = true;


            dgStreamShedule.Columns.Add("aCode", "Admin Code");
            dgStreamShedule.Columns["aCode"].Width = 110;
            dgStreamShedule.Columns["aCode"].Visible = false;
            dgStreamShedule.Columns["aCode"].ReadOnly = true;

            dgStreamShedule.Columns.Add("StartDate", "Start Date");
            dgStreamShedule.Columns["StartDate"].Width = 120;
            dgStreamShedule.Columns["StartDate"].Visible = true;
            dgStreamShedule.Columns["StartDate"].ReadOnly = true;

            dgStreamShedule.Columns.Add("EndDate", "End Date");
            dgStreamShedule.Columns["EndDate"].Width = 120;
            dgStreamShedule.Columns["EndDate"].Visible = true;
            dgStreamShedule.Columns["EndDate"].ReadOnly = true;

            dgStreamShedule.Columns.Add("Time", "Time");
            dgStreamShedule.Columns["Time"].Width = 100;
            dgStreamShedule.Columns["Time"].Visible = true;
            dgStreamShedule.Columns["Time"].ReadOnly = true;



            dgStreamShedule.Columns.Add("CiName", "City Name");
            dgStreamShedule.Columns["CiName"].Width = 140;
            dgStreamShedule.Columns["CiName"].Visible = false;
            dgStreamShedule.Columns["CiName"].ReadOnly = true;


            EditStreamShedule.HeaderText = "Edit";
            EditStreamShedule.Text = "Edit";
            EditStreamShedule.DataPropertyName = "Edit";
            dgStreamShedule.Columns.Add(EditStreamShedule);
            EditStreamShedule.UseColumnTextForLinkValue = true;
            EditStreamShedule.Width = 70;
            EditStreamShedule.Visible = true;

            DeleteStreamShedule.HeaderText = "Delete";
            DeleteStreamShedule.Text = "Delete";
            DeleteStreamShedule.DataPropertyName = "Delete";
            dgStreamShedule.Columns.Add(DeleteStreamShedule);
            DeleteStreamShedule.UseColumnTextForLinkValue = true;
            DeleteStreamShedule.Width = 70;
            DeleteStreamShedule.Visible = true;
            //dgGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;

        }
        private void FillClientStreamShedule(string Query)
        {
            int iCtr;
            DataTable dtDetail;
            dtDetail = ObjMainClass.fnFillDataTable(Query);
            InitilizeGrid();
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgStreamShedule.Rows.Add();
                    dgStreamShedule.Rows[dgStreamShedule.Rows.Count - 1].Cells["ScheduleId"].Value = dtDetail.Rows[iCtr]["SheduleId"];
                    dgStreamShedule.Rows[dgStreamShedule.Rows.Count - 1].Cells["cName"].Value = dtDetail.Rows[iCtr]["titlecategoryname"];
                    dgStreamShedule.Rows[dgStreamShedule.Rows.Count - 1].Cells["sName"].Value = dtDetail.Rows[iCtr]["StreamName"];
                    dgStreamShedule.Rows[dgStreamShedule.Rows.Count - 1].Cells["aCode"].Value = dtDetail.Rows[iCtr]["AdminCode"];
                    dgStreamShedule.Rows[dgStreamShedule.Rows.Count - 1].Cells["StartDate"].Value = string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["StartDate"]);
                    dgStreamShedule.Rows[dgStreamShedule.Rows.Count - 1].Cells["EndDate"].Value = string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["EndDate"]);
                    dgStreamShedule.Rows[dgStreamShedule.Rows.Count - 1].Cells["Time"].Value = string.Format("{0:h:mm tt}", dtDetail.Rows[iCtr]["StartTime"]);
                    dgStreamShedule.Rows[dgStreamShedule.Rows.Count - 1].Cells["CiName"].Value = dtDetail.Rows[iCtr]["CityName"];
                }


            }
        }
        private void dgStreamShedule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == 8)
            {
                string Localstr = "";
                Localstr = " select * from tbStreamSchedulingClient ";
                Localstr = Localstr + "	where SheduleId = " + Convert.ToInt32(dgStreamShedule.Rows[dgStreamShedule.CurrentCell.RowIndex].Cells[0].Value);
                DataTable dtDetail;
                dtDetail = ObjMainClass.fnFillDataTable(Localstr);
                if ((dtDetail.Rows.Count > 0))
                {
                    btnSave.Text = "Update";
                    ModifyStreamSheduleId = Convert.ToInt32(dtDetail.Rows[0]["SheduleId"]);
                    cmbStreamName.SelectedValue = Convert.ToInt32(dtDetail.Rows[0]["StreamId"]);
                    dtpStartDate.Value = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[0]["StartDate"]));
                    dtpEndDate.Value = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[0]["EndDate"]));
                    cmbHour.Text = string.Format("{0:hh}", dtDetail.Rows[0]["StartTime"]);
                    cmbMin.Text = string.Format("{0:mm}", dtDetail.Rows[0]["StartTime"]);
                    cmbAMPM.Text = string.Format("{0:tt}", dtDetail.Rows[0]["StartTime"]);
                }

            }
            if (e.ColumnIndex == 9)
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                if (dgStreamShedule.Rows.Count <= 0) return;
                if (Convert.ToInt32(dgStreamShedule.Rows[dgStreamShedule.CurrentCell.RowIndex].Cells[0].Value) != 0)
                {
                    result = MessageBox.Show("Are you sure delete selected shedule?", "Music Player", buttons);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = "delete from tbStreamScheduling where SheduleId=" + Convert.ToInt32(dgStreamShedule.Rows[dgStreamShedule.CurrentCell.RowIndex].Cells[0].Value);
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();
                        FillClientStreamShedule("GetClientStreamSheduling '" + string.Format("{0:dd/MMM/yyyy}", dtpFromDate.Value) + "'," + StaticClass.TokenId);
                    }
                }
            }
        }
        private void picStreamShedule_Click(object sender, EventArgs e)
        {
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            Clear();
            if (this.Width.ToString() != "1020")
            {
                dgStreamShedule.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            panStreamShedule.Width = this.Width;
            panStreamShedule.Height = this.Height;
            panStreamShedule.Location = new Point(0, 0);

            panStreamShedule.BringToFront();
            panStreamShedule.Visible = true;
        }
        private void picHideShedule_Click(object sender, EventArgs e)
        {
            panStreamShedule.Visible = false;
        }
        private void picAllStreamShedule_Click(object sender, EventArgs e)
        {
            panAllStreamShedule.Width = this.Width;
            panAllStreamShedule.Height = this.Height;
            panAllStreamShedule.Location = new Point(0, 0);
            panAllStreamShedule.BringToFront();
            panAllStreamShedule.Visible = true;
        }
        private void picBackPlayer_Click(object sender, EventArgs e)
        {
            panAllStreamShedule.Visible = false;
        }
        #endregion

        private void picAdvtSchedule_Click_1(object sender, EventArgs e)
        {
            panAdvtSchedule.Location = new Point(0, 0);
            panAdvtSchedule.Width = panAdvtDetail.Width;
            panAdvtSchedule.Height = panAdvtDetail.Height;
            //panel11.Visible = false;
            panAdvtSchedule.BringToFront();
            panAdvtSchedule.Visible = true;
            panAdvtGrid.Height = panAdvtSchedule.Height - panButtonSettings.Height - panScheduleDate.Height - 40;
        }

        private void picAdvtNew_Click(object sender, EventArgs e)
        {
            panAdvtDetail.Location = new Point(0, 0);
            panAdvtDetail.Width = this.Width;
            panAdvtDetail.Height = this.Height;
            panel42.Visible = false;
            panAdvtSchedule.Visible = false;
            panAdvtNew.Width = panAdvtDetail.Width - 20;
            panAdvtNew.Dock = DockStyle.Top;
            panAdvtCenter.Dock = DockStyle.Top;

            tlpAdvtMain.Visible = false;
            panAdvtNew.Height = panAdvtDetail.Height - 30;
            panAdvtInnerDetail.Location = new Point(
           this.panAdvtCenter.Width / 2 - panAdvtInnerDetail.Size.Width / 2,
           this.panAdvtCenter.Height / 2 - panAdvtInnerDetail.Size.Height / 2);
            dgClientAdvt.Dock = DockStyle.Fill;


            panAdvtNew.Visible = true;
            txtAdvtName.Focus();
        }

        private void picMainPlayer_Click_1(object sender, EventArgs e)
        {
            panAdvtDetail.Visible = false;
        }

        private void picSearchAdvtPanel_Click(object sender, EventArgs e)
        {
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            panel42.Visible = true;
            tlpAdvtMain.BringToFront();
            tlpAdvtMain.Visible = true;
            tlpAdvtMain.Dock = DockStyle.Fill;
            panAdvtSchedule.Dock = DockStyle.None;
            panAdvtNew.Visible = false;
            panAdvtSchedule.Visible = false;

            string Localstr = "";
            string PlayerType = "";
            PlayerType = "Copyright";
            //ClearData();
            dgAdvtDealerDetail.Visible = true;
            panAdvtNew.Dock = DockStyle.None;
            panAdvtNew.Visible = false;
            panAdvtSchedule.Visible = false;

            Localstr = "spGetAdvertisementClientDateWise '" + string.Format("{0:dd/MMM/yyyy}", dtpSearchAdvt.Value) + "','" + PlayerType + "','" + StaticClass.DealerCode + "', " + StaticClass.CountryId + "," + StaticClass.TokenId + "," + Convert.ToInt32(cmbAdvertisementType.SelectedValue);
            FillDealerAdvtDetail(dgAdvtClientDetail, Localstr);

            Localstr = "spGetAdvertisementDealerDateWise '" + string.Format("{0:dd/MMM/yyyy}", dtpSearchAdvt.Value) + "','" + PlayerType + "','" + StaticClass.DealerCode + "', " + Convert.ToInt32(cmbAdvertisementType.SelectedValue) + "," + StaticClass.AdvtCityId + "";
            FillDealerAdvtDetail(dgAdvtDealerDetail, Localstr);
        }
        private void FillDealerAdvtDetail(DataGridView dgGrid, string Query)
        {

            int iCtr;
            DataTable dtDetail;


            dtDetail = ObjMainClass.fnFillDataTable(Query);
            InitilizeAdvertisementDetail(dgGrid);
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgGrid.Rows.Add();
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["AdvtId"];
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["AdvtDisplayName"];
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["AdvtTypeName"];
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[3].Value = string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["AdvtStartDate"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Value = string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["AdvtEndDate"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[5].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["AdvtTime"]);

                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[1].Style.ForeColor = Color.Black;
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[2].Style.ForeColor = Color.Black;
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[3].Style.ForeColor = Color.Black;
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Style.ForeColor = Color.Black;
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[5].Style.ForeColor = Color.Black;

                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[3].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);

                }
                foreach (DataGridViewRow row in dgGrid.Rows)
                {
                    row.Height = 30;
                }
            }
        }
        private void InitilizeAdvertisementDetail(DataGridView dgGrid)
        {
            if (dgGrid.Rows.Count > 0)
            {
                dgGrid.Rows.Clear();
            }
            if (dgGrid.Columns.Count > 0)
            {
                dgGrid.Columns.Clear();
            }

            dgGrid.Columns.Add("Advtid", "Advt Id");
            dgGrid.Columns["Advtid"].Width = 0;
            dgGrid.Columns["Advtid"].Visible = false;
            dgGrid.Columns["Advtid"].ReadOnly = true;

            dgGrid.Columns.Add("Advt", "Advt Name");
            dgGrid.Columns["Advt"].Width = 100;
            dgGrid.Columns["Advt"].Visible = true;
            dgGrid.Columns["Advt"].ReadOnly = true;

            dgGrid.Columns.Add("Type", "Type");
            dgGrid.Columns["Type"].Width = 100;
            dgGrid.Columns["Type"].Visible = true;
            dgGrid.Columns["Type"].ReadOnly = true;

            dgGrid.Columns.Add("EndDate", "Start Date");
            dgGrid.Columns["EndDate"].Width = 100;
            dgGrid.Columns["EndDate"].Visible = true;
            dgGrid.Columns["EndDate"].ReadOnly = true;

            dgGrid.Columns.Add("EndDate", "End Date");
            dgGrid.Columns["EndDate"].Width = 100;
            dgGrid.Columns["EndDate"].Visible = true;
            dgGrid.Columns["EndDate"].ReadOnly = true;

            dgGrid.Columns.Add("Time", "Time");
            dgGrid.Columns["Time"].Width = 100;
            dgGrid.Columns["Time"].Visible = true;
            dgGrid.Columns["Time"].ReadOnly = true;

            if (dgGrid.Name == "dgClientAdvt")
            {
                DataGridViewLinkColumn EditAdvt = new DataGridViewLinkColumn();
                EditAdvt.HeaderText = "Edit";
                EditAdvt.Text = "Edit";
                EditAdvt.DataPropertyName = "Edit";
                dgGrid.Columns.Add(EditAdvt);
                EditAdvt.UseColumnTextForLinkValue = true;
                EditAdvt.Width = 70;
                EditAdvt.Visible = true;

                DataGridViewLinkColumn DeleteAdvt = new DataGridViewLinkColumn();
                DeleteAdvt.HeaderText = "Delete";
                DeleteAdvt.Text = "Delete";
                DeleteAdvt.DataPropertyName = "Delete";
                dgGrid.Columns.Add(DeleteAdvt);
                DeleteAdvt.UseColumnTextForLinkValue = true;
                DeleteAdvt.Width = 70;
                DeleteAdvt.Visible = true;
            }
            //dgGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;
            dgGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


        }

        private void bgAdvtWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {

                string filename = txtPath.Text;
                FileInfo objFile = new FileInfo(filename);
                string ftpServerIP = "ftp://85.195.82.94:21/AdvtSongs/Client/" + TempAdvtFileName + ".mp3";
                string ftpUserName = "harish";
                string ftpPassword = "Mohali123";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpServerIP));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);
                request.UseBinary = true;
                request.UsePassive = false;
                byte[] fileStream = System.IO.File.ReadAllBytes(filename);
                System.IO.Stream requestStream = request.GetRequestStream();
                for (int offset = 0; offset <= fileStream.Length; offset += 1024)
                {
                    bgAdvtWorker.ReportProgress(Convert.ToInt32(offset * progressBar3.Maximum / fileStream.Length));
                    int chSize = fileStream.Length - offset;
                    if (chSize > 1024)
                        chSize = 1024;
                    requestStream.Write(fileStream, offset, chSize);
                }
                requestStream.Close();
                requestStream.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = StaticClass.constr;
                cmd.CommandText = "delete from tbAdvertisement where advtid=" + TempAdvtFileName;
                cmd.ExecuteNonQuery();
                StaticClass.constr.Close();
                return;
            }
        }

        private void bgAdvtWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar3.Value = e.ProgressPercentage;
            lblAdvtPercentage.Text = e.ProgressPercentage + "%";
        }

        private void bgAdvtWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GC.Collect();
            string GetComboTimeString = "";
            GetComboTimeString = cmbAdvtHour.Text + ":" + cmbAdvtMin.Text + " " + cmbAdvtAMPM.Text;
            DateTime GetComboTime = DateTime.ParseExact(GetComboTimeString, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);

            lblAdvtPercentage.Text = "";
            progressBar3.Value = 0;
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("spUpdateAdvertisement_Client", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@AdvtId", SqlDbType.BigInt));
            cmd.Parameters["@AdvtId"].Value = Convert.ToInt32(TempAdvtFileName);

            cmd.Parameters.Add(new SqlParameter("@AdvtDisplayName", SqlDbType.VarChar));
            cmd.Parameters["@AdvtDisplayName"].Value = txtAdvtName.Text.Trim();

            cmd.Parameters.Add(new SqlParameter("@AdvtCompanyName", SqlDbType.VarChar));
            cmd.Parameters["@AdvtCompanyName"].Value = txtCompanyName.Text.Trim();

            cmd.Parameters.Add(new SqlParameter("@AdvtStartDate", SqlDbType.DateTime));
            cmd.Parameters["@AdvtStartDate"].Value = dtpAdvtStartDate.Value.ToString("dd/MMM/yyyy");

            cmd.Parameters.Add(new SqlParameter("@AdvtEndDate", SqlDbType.DateTime));
            cmd.Parameters["@AdvtEndDate"].Value = dtpAdvtEndDate.Value.ToString("dd/MMM/yyyy");

            cmd.Parameters.Add(new SqlParameter("@AdvtFilePath", SqlDbType.VarChar));
            cmd.Parameters["@AdvtFilePath"].Value = "http://85.195.82.94/AdvtSongs/Client/" + TempAdvtFileName + ".mp3";

            cmd.Parameters.Add(new SqlParameter("@AdvtPlayertype", SqlDbType.VarChar));
            cmd.Parameters["@AdvtPlayertype"].Value = "Copyright";

            //error
            cmd.Parameters.Add(new SqlParameter("@DfClientId", SqlDbType.BigInt));
            cmd.Parameters["@DfClientId"].Value = Convert.ToInt32(StaticClass.UserId);

            cmd.Parameters.Add(new SqlParameter("@CountryCode", SqlDbType.BigInt));
            cmd.Parameters["@CountryCode"].Value = Convert.ToInt32(StaticClass.CountryId);

            cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
            cmd.Parameters["@TokenId"].Value = Convert.ToInt32(StaticClass.TokenId);

            cmd.Parameters.Add(new SqlParameter("@Dealercode", SqlDbType.VarChar));
            cmd.Parameters["@Dealercode"].Value = StaticClass.DealerCode;

            cmd.Parameters.Add(new SqlParameter("@AdvtTypeId", SqlDbType.Int));
            cmd.Parameters["@AdvtTypeId"].Value = Convert.ToInt32(cmbAdvertisementTypeAdd.SelectedValue);

            cmd.Parameters.Add(new SqlParameter("@AdvtTime", SqlDbType.DateTime));
            cmd.Parameters["@AdvtTime"].Value = GetComboTime.ToString("hh:mm tt");

            cmd.Parameters.Add(new SqlParameter("@StateId", SqlDbType.BigInt));
            cmd.Parameters["@StateId"].Value = StaticClass.Stateid;

            cmd.Parameters.Add(new SqlParameter("@CityId", SqlDbType.BigInt));
            cmd.Parameters["@CityId"].Value = StaticClass.AdvtCityId;
            try
            {
                cmd.ExecuteNonQuery();
                FillAllAdvertisement();
                FillMainAdvertisement();
                string Localstr = "spGetAdvertisementClientDateWise '" + string.Format("{0:dd/MMM/yyyy}", dtpSearchClientAdvt.Value) + "','Copyright','" + StaticClass.DealerCode + "', " + StaticClass.CountryId + "," + StaticClass.TokenId + "," + Convert.ToInt32(cmbAdvertisementType.SelectedValue);
                FillDealerAdvtDetail(dgClientAdvt, Localstr);
                MessageBox.Show("Advertisement is upload", "Copyright Player");
                ClearData();
                // panAdvtNew.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Record not save", "!! Problem !!");
                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = StaticClass.constr;
                cmd1.CommandText = "delete from tbAdvertisement where advtid=" + TempAdvtFileName;
                cmd1.ExecuteNonQuery();
                StaticClass.constr.Close();

                ClearData();
                Console.WriteLine(ex.Message);
                return;
            }
            finally
            {
                StaticClass.constr.Close();
            }
        }

        private void btnDialog_Click(object sender, EventArgs e)
        {
            OpenDialog.FileName = "";
            OpenDialog.DefaultExt = ".mp3";
            OpenDialog.Filter = "|*.mp3";
            if (OpenDialog.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = OpenDialog.FileName;
            }
        }

        private void btnUnload_Click(object sender, EventArgs e)
        {
            if (bgAdvtWorker.IsBusy == true)
            {
                MessageBox.Show("There is an upload of a file in progress, please wait for exit.", "Copyright Player");
                return;
            }
            ClearData();
            panel42.Visible = false;
            panAdvtSchedule.BringToFront();
            panAdvtSchedule.Visible = true;
            panAdvtSchedule.Dock = DockStyle.Fill;
            panAdvtNew.Visible = false;
        }
        private void ClearData()
        {
            lblAdvtId.Text = "0";
            btnDialog.Visible = true;
            btnUpload.Text = "Upload";
            txtAdvtName.Text = "";
            txtCompanyName.Text = "";
            dtpAdvtStartDate.Value = DateTime.Now.Date;
            dtpAdvtEndDate.Value = DateTime.Now.Date;
            cmbAdvtHour.Text = "";
            cmbAdvtMin.Text = "";
            cmbAdvtAMPM.Text = "";
            txtPath.Text = "";
            cmbAdvertisementTypeAdd.SelectedValue = 0;
            cmbAdvertisementTypeAdd.Text = "";

        }

        private void picSearchAdvt_Click(object sender, EventArgs e)
        {
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Copyright Player");
                return;
            }
            tlpAdvtMain.BringToFront();
            tlpAdvtMain.Dock = DockStyle.Fill;
            tlpAdvtMain.Visible = true;
            panAdvtSchedule.Dock = DockStyle.None;
            panAdvtNew.Visible = false;
            panAdvtSchedule.Visible = false;

            string Localstr = "";
            string PlayerType = "";
            PlayerType = "Copyright";
            //ClearData();
            dgAdvtDealerDetail.Visible = true;
            panAdvtNew.Dock = DockStyle.None;
            panAdvtNew.Visible = false;
            panAdvtSchedule.Visible = false;

            Localstr = "spGetAdvertisementClientDateWise '" + string.Format("{0:dd/MMM/yyyy}", dtpSearchAdvt.Value) + "','" + PlayerType + "','" + StaticClass.DealerCode + "', " + StaticClass.CountryId + "," + StaticClass.TokenId + "," + Convert.ToInt32(cmbAdvertisementType.SelectedValue);
            FillDealerAdvtDetail(dgAdvtClientDetail, Localstr);

            Localstr = "spGetAdvertisementDealerDateWise '" + string.Format("{0:dd/MMM/yyyy}", dtpSearchAdvt.Value) + "','" + PlayerType + "','" + StaticClass.DealerCode + "', " + Convert.ToInt32(cmbAdvertisementType.SelectedValue) + "," + StaticClass.AdvtCityId + "";
            FillDealerAdvtDetail(dgAdvtDealerDetail, Localstr);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (SubmitValidationAdvt() == false) return;
            if (bgAdvtWorker.IsBusy == true)
            {
                MessageBox.Show("Advertisement uploading", "Token Administrator");
                return;
            }
            if (btnUpload.Text == "Upload")
            {
                if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                StaticClass.constr.Open();
                SqlCommand cmd = new SqlCommand("spTempSaveAdvertisement", StaticClass.constr);
                cmd.CommandType = CommandType.StoredProcedure;
                TempAdvtFileName = cmd.ExecuteScalar().ToString();
                StaticClass.constr.Close();
                bgAdvtWorker.RunWorkerAsync();
            }
            else
            {
                UpdateAdvt();
                string Localstr = "spGetAdvertisementClientDateWise '" + string.Format("{0:dd/MMM/yyyy}", dtpSearchClientAdvt.Value) + "','Copyright','" + StaticClass.DealerCode + "', " + StaticClass.CountryId + "," + StaticClass.TokenId + "," + Convert.ToInt32(cmbAdvertisementType.SelectedValue);
                FillDealerAdvtDetail(dgClientAdvt, Localstr);
                FillAllAdvertisement();
                FillMainAdvertisement();
            }
        }
        private void UpdateAdvt()
        {
            string GetComboTimeString = "";
            GetComboTimeString = cmbAdvtHour.Text + ":" + cmbAdvtMin.Text + " " + cmbAdvtAMPM.Text;
            DateTime GetComboTime = DateTime.ParseExact(GetComboTimeString, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);

            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("spUpdateAdvertisementClient", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@AdvtId", SqlDbType.BigInt));
            cmd.Parameters["@AdvtId"].Value = Convert.ToInt32(lblAdvtId.Text);

            cmd.Parameters.Add(new SqlParameter("@AdvtDisplayName", SqlDbType.VarChar));
            cmd.Parameters["@AdvtDisplayName"].Value = txtAdvtName.Text.Trim();

            cmd.Parameters.Add(new SqlParameter("@AdvtCompanyName", SqlDbType.VarChar));
            cmd.Parameters["@AdvtCompanyName"].Value = txtCompanyName.Text.Trim();

            cmd.Parameters.Add(new SqlParameter("@AdvtStartDate", SqlDbType.DateTime));
            cmd.Parameters["@AdvtStartDate"].Value = dtpAdvtStartDate.Value.ToString("dd/MMM/yyyy");

            cmd.Parameters.Add(new SqlParameter("@AdvtEndDate", SqlDbType.DateTime));
            cmd.Parameters["@AdvtEndDate"].Value = dtpAdvtEndDate.Value.ToString("dd/MMM/yyyy");

            cmd.Parameters.Add(new SqlParameter("@AdvtTypeId", SqlDbType.Int));
            cmd.Parameters["@AdvtTypeId"].Value = Convert.ToInt32(cmbAdvertisementTypeAdd.SelectedValue);

            cmd.Parameters.Add(new SqlParameter("@AdvtTime", SqlDbType.DateTime));
            cmd.Parameters["@AdvtTime"].Value = GetComboTime.ToString("hh:mm tt");

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Advertisement is modify", "Copyright Player");
                ClearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Record not modify", "!! Problem !!");
                ClearData();
                return;
            }
            finally
            {
                StaticClass.constr.Close();
            }
        }
        private Boolean SubmitValidationAdvt()
        {
            if (txtAdvtName.Text == "")
            {
                MessageBox.Show("Advertisement name cannot be blank", "Copyright Player");
                txtAdvtName.Focus();
                return false;
            }
            else if (txtCompanyName.Text == "")
            {
                MessageBox.Show("Company name cannot be blank", "Copyright Player");
                txtCompanyName.Focus();
                return false;
            }
            else if (dtpAdvtEndDate.Value < dtpAdvtStartDate.Value)
            {
                MessageBox.Show("Please select proper date's", "Copyright Player");
                dtpEndDate.Focus();
                return false;
            }
            else if (Convert.ToInt32(cmbAdvertisementTypeAdd.SelectedValue) == 0)
            {
                MessageBox.Show("Please select advertisement type", "Copyright Player");
                cmbAdvertisementTypeAdd.Focus();
                return false;
            }
            else if (txtPath.Text == "")
            {
                MessageBox.Show("File path cannot be blank", "Copyright Player");
                txtPath.Focus();
                return false;
            }
            else if (cmbAdvtHour.Text == "")
            {
                MessageBox.Show("Please select the hour", "Copyright Player");
                cmbAdvtHour.Focus();
                return false;
            }
            else if (cmbAdvtMin.Text == "")
            {
                MessageBox.Show("Please select minute", "Copyright Player");
                cmbAdvtMin.Focus();
                return false;
            }
            else if (cmbAdvtAMPM.Text == "")
            {
                MessageBox.Show("Please select time type", "Copyright Player");
                cmbAdvtAMPM.Focus();
                return false;
            }

            string GetComboTimeString = "";
            GetComboTimeString = cmbAdvtHour.Text + ":" + cmbAdvtMin.Text + " " + cmbAdvtAMPM.Text;
            DateTime GetComboTime = DateTime.ParseExact(GetComboTimeString, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);

            string strDealerTimeValid = "select * from tbAdvertisement where AdvtPlayertype='Copyright' and  (tokenid=0 or tokenid= " + StaticClass.TokenId + ")   and dealercode='" + StaticClass.DealerCode + "' and ";
            strDealerTimeValid = strDealerTimeValid + " Advttime='" + GetComboTime.ToString("hh:mm tt") + "' ";
            strDealerTimeValid = strDealerTimeValid + " and ('" + dtpAdvtStartDate.Value.ToString("dd/MMM/yyyy") + "' between advtstartdate and  advtenddate";
            strDealerTimeValid = strDealerTimeValid + " or '" + dtpAdvtEndDate.Value.ToString("dd/MMM/yyyy") + "' between advtstartdate and  advtenddate )";
            DataTable dtDealerTimeValid = new DataTable();
            dtDealerTimeValid = ObjMainClass.fnFillDataTable(strDealerTimeValid);

            //string strClientTimeValid = "select * from tbAdvertisement where tokenid=" + StaticClass.TokenId + " and ";
            //strClientTimeValid = strClientTimeValid + " Advttime='" + GetComboTime.ToString("hh:mm tt") + "' ";
            //strClientTimeValid = strClientTimeValid + "  and ('" + dtpStartDate.Value.ToString("dd/MMM/yyyy") + "' between advtstartdate and  advtenddate";
            //strClientTimeValid = strClientTimeValid + "  or '" + dtpEndDate.Value.ToString("dd/MMM/yyyy") + "' between advtstartdate and  advtenddate )";
            //DataTable dtClientTimeValid = new DataTable();
            //dtClientTimeValid = ObjMainClass.fnFillDataTable(strClientTimeValid);

            if (dtDealerTimeValid.Rows.Count > 0)
            {
                MessageBox.Show("This time is already used", "Copyright Player");
                cmbMin.Focus();
                return false;
            }
            string strMain = "";
            strMain = "select * from tbAdvtAdmin where   ";
            strMain = strMain + " Starttime='" + GetComboTime.ToString("hh:mm tt") + "' ";
            strMain = strMain + " and (Playertype='Copyright'   or Playertype='All')  ";
            strMain = strMain + " and ('" + dtpAdvtStartDate.Value.ToString("dd/MMM/yyyy") + "' between startdate and  enddate";
            strMain = strMain + " or '" + dtpAdvtEndDate.Value.ToString("dd/MMM/yyyy") + "' between startdate and  enddate )";
            dtDealerTimeValid = new DataTable();
            dtDealerTimeValid = ObjMainClass.fnFillDataTable(strMain);
            if (dtDealerTimeValid.Rows.Count > 0)
            {
                MessageBox.Show("This time is already used", "Copyright Player");
                cmbMin.Focus();
                return false;
            }
            return true;
        }

        private void dgClientAdvt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                string Localstr = "";
                //Localstr = "spGetAdvertisementDealer '" + string.Format("{0:dd/MMM/yyyy}", dtpFromDate.Value) + "','" + cmbSearchPlayerCode.Text + "', " + Convert.ToInt32(dgAdvtDealerDetail.Rows[dgAdvtDealerDetail.CurrentCell.RowIndex].Cells[0].Value);
                Localstr = " select * from tbAdvertisement ";
                Localstr = Localstr + "	where advtid = " + Convert.ToInt32(dgClientAdvt.Rows[dgClientAdvt.CurrentCell.RowIndex].Cells[0].Value);
                DataTable dtDetail;
                dtDetail = ObjMainClass.fnFillDataTable(Localstr);
                if ((dtDetail.Rows.Count > 0))
                {
                    btnDialog.Visible = false;
                    btnUpload.Text = "Update";
                    lblAdvtId.Text = Convert.ToString(dtDetail.Rows[0]["AdvtId"]);
                    txtAdvtName.Text = dtDetail.Rows[0]["AdvtDisplayName"].ToString();
                    txtCompanyName.Text = dtDetail.Rows[0]["AdvtCompanyName"].ToString();
                    cmbAdvertisementTypeAdd.SelectedValue = Convert.ToInt32(dtDetail.Rows[0]["AdvtTypeId"]);
                    dtpAdvtStartDate.Value = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[0]["AdvtStartDate"]));
                    dtpAdvtEndDate.Value = Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[0]["AdvtEndDate"]));
                    cmbAdvtHour.Text = string.Format("{0:hh}", dtDetail.Rows[0]["AdvtTime"]);
                    cmbAdvtMin.Text = string.Format("{0:mm}", dtDetail.Rows[0]["AdvtTime"]);
                    cmbAdvtAMPM.Text = string.Format("{0:tt}", dtDetail.Rows[0]["AdvtTime"]);
                    txtPath.Text = dtDetail.Rows[0]["AdvtFilePath"].ToString();
                }
            }
            if (e.ColumnIndex == 7)
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                if (dgClientAdvt.Rows.Count <= 0) return;
                if (Convert.ToInt32(dgClientAdvt.Rows[dgClientAdvt.CurrentCell.RowIndex].Cells[0].Value) != 0)
                {
                    result = MessageBox.Show("Are you sure to delete this advertisement?", "Copyright Player", buttons);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        string ftpServerIP = "85.195.82.94:21/AdvtSongs/Client/";
                        string ftpUserName = "harish";
                        string ftpPassword = "Mohali123";

                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://" + ftpServerIP + "/" + dgClientAdvt.Rows[dgClientAdvt.CurrentCell.RowIndex].Cells[0].Value + ".mp3");

                        request.Credentials = new NetworkCredential(ftpUserName, ftpPassword);

                        request.Method = WebRequestMethods.Ftp.DeleteFile;
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        Console.WriteLine("Delete status: {0}", response.StatusDescription);
                        response.Close();


                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = "delete from tbAdvertisementDetail where advtid=" + Convert.ToInt32(dgClientAdvt.Rows[dgClientAdvt.CurrentCell.RowIndex].Cells[0].Value);
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd2 = new SqlCommand();
                        cmd2.Connection = StaticClass.constr;
                        cmd2.CommandText = "delete from tbAdvertisement where advtid=" + Convert.ToInt32(dgClientAdvt.Rows[dgClientAdvt.CurrentCell.RowIndex].Cells[0].Value);
                        cmd2.ExecuteNonQuery();
                        StaticClass.constr.Close();
                        dgClientAdvt.Rows.RemoveAt(dgClientAdvt.CurrentCell.RowIndex);


                        string Localstr = "spGetAdvertisementClientDateWise '" + string.Format("{0:dd/MMM/yyyy}", dtpSearchClientAdvt.Value) + "','Copyright','" + StaticClass.DealerCode + "', " + StaticClass.CountryId + "," + StaticClass.TokenId + "," + Convert.ToInt32(cmbAdvertisementType.SelectedValue);
                        FillDealerAdvtDetail(dgClientAdvt, Localstr);
                    }
                }
            }
        }

        private void cmbAdvertisementType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpSearchClientAdvt_ValueChanged(object sender, EventArgs e)
        {
            string Localstr = "spGetAdvertisementClientDateWise '" + string.Format("{0:dd/MMM/yyyy}", dtpSearchClientAdvt.Value) + "','Copyright','" + StaticClass.DealerCode + "', " + StaticClass.CountryId + "," + StaticClass.TokenId + "," + Convert.ToInt32(cmbAdvertisementType.SelectedValue);
            FillDealerAdvtDetail(dgClientAdvt, Localstr);
        }


        //Start Work on Playlist Schedule

        Int32 ModifyPlaylistSheduleId = 0;
        string PlaylistTime = "";
        int PlaylistRow = 0;
        int StartPlaylist = 0;
        DataGridViewLinkColumn EditPlaylistShedule = new DataGridViewLinkColumn();
        DataGridViewLinkColumn DeletePlaylistShedule = new DataGridViewLinkColumn();
        private void picPlaylistSchedule_Click(object sender, EventArgs e)
        {
            if (this.Width.ToString() != "1020")
            {
                dgPlaylistSch.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            panPlaylistSchedule.Width = this.Width;
            panPlaylistSchedule.Height = this.Height;
            panPlaylistSchedule.Location = new Point(0, 0);

            panPlaylistSchedule.BringToFront();
            panPlaylistSchedule.Visible = true;
        }
        private void picPanHide_Click(object sender, EventArgs e)
        {
            panPlaylistSchedule.Visible = false;
        }
        private void panPlaylistSchedule_VisibleChanged(object sender, EventArgs e)
        {
            if (panPlaylistSchedule.Visible == false) return;
            ClearPlaylistSch();

        }
        private void InitilizePlaylistScheduleGrid()
        {
            if (dgPlaylistSch.Rows.Count > 0)
            {
                dgPlaylistSch.Rows.Clear();
            }
            if (dgPlaylistSch.Columns.Count > 0)
            {
                dgPlaylistSch.Columns.Clear();
            }

            dgPlaylistSch.Columns.Add("ScheduleId", "Schedule Id");
            dgPlaylistSch.Columns["ScheduleId"].Width = 0;
            dgPlaylistSch.Columns["ScheduleId"].Visible = false;
            dgPlaylistSch.Columns["ScheduleId"].ReadOnly = true;

            dgPlaylistSch.Columns.Add("sName", "Playlist Name");
            dgPlaylistSch.Columns["sName"].Width = 400;
            dgPlaylistSch.Columns["sName"].Visible = true;
            dgPlaylistSch.Columns["sName"].ReadOnly = true;

            dgPlaylistSch.Columns.Add("StartDate", "Start Date");
            dgPlaylistSch.Columns["StartDate"].Width = 120;
            dgPlaylistSch.Columns["StartDate"].Visible = true;
            dgPlaylistSch.Columns["StartDate"].ReadOnly = true;

            dgPlaylistSch.Columns.Add("EndDate", "End Date");
            dgPlaylistSch.Columns["EndDate"].Width = 120;
            dgPlaylistSch.Columns["EndDate"].Visible = true;
            dgPlaylistSch.Columns["EndDate"].ReadOnly = true;

            dgPlaylistSch.Columns.Add("Time", "Time");
            dgPlaylistSch.Columns["Time"].Width = 100;
            dgPlaylistSch.Columns["Time"].Visible = true;
            dgPlaylistSch.Columns["Time"].ReadOnly = true;

            dgPlaylistSch.Columns.Add("wDay", "WeekDays");
            dgPlaylistSch.Columns["wDay"].Width = 100;
            dgPlaylistSch.Columns["wDay"].Visible = true;
            dgPlaylistSch.Columns["wDay"].ReadOnly = true;

            EditPlaylistShedule.HeaderText = "Edit";
            EditPlaylistShedule.Text = "Edit";
            EditPlaylistShedule.DataPropertyName = "Edit";
            dgPlaylistSch.Columns.Add(EditPlaylistShedule);
            EditPlaylistShedule.UseColumnTextForLinkValue = true;
            EditPlaylistShedule.Width = 70;
            EditPlaylistShedule.Visible = true;

            DeletePlaylistShedule.HeaderText = "Delete";
            DeletePlaylistShedule.Text = "Delete";
            DeletePlaylistShedule.DataPropertyName = "Delete";
            dgPlaylistSch.Columns.Add(DeletePlaylistShedule);
            DeletePlaylistShedule.UseColumnTextForLinkValue = true;
            DeletePlaylistShedule.Width = 70;
            DeletePlaylistShedule.Visible = true;

            dgPlaylistSch.Columns.Add("pId", "pId");
            dgPlaylistSch.Columns["pId"].Width = 0;
            dgPlaylistSch.Columns["pId"].Visible = false;
            dgPlaylistSch.Columns["pId"].ReadOnly = true;

            //dgGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.ColumnHeader;

        }
        private void FillPlaylistShedule()
        {
            string str = "";
            str = "select tbPlaylistSchedule.SchId,tbPlaylistSchedule.PlaylistId,tbPlaylistSchedule.StartDate,tbPlaylistSchedule.EndDate,tbPlaylistSchedule.StartTime,tbPlaylistSchedule.WeekDay,(Playlists.Name) as pName from (tbPlaylistSchedule";
            str = str + " inner join Playlists on tbPlaylistSchedule.playlistid= Playlists.playlistid)";
            str = str + " where   #" + string.Format("{0:dd/MMM/yyyy}", dtpSearchDatePlaylist.Value) + "# between";
            str = str + " tbPlaylistSchedule.StartDate and tbPlaylistSchedule.EndDate";

            int iCtr;
            DataTable dtDetail;
            dtDetail = ObjMainClass.fnFillDataTable_Local(str);
            InitilizePlaylistScheduleGrid();
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgPlaylistSch.Rows.Add();
                    dgPlaylistSch.Rows[dgPlaylistSch.Rows.Count - 1].Cells["ScheduleId"].Value = dtDetail.Rows[iCtr]["SchId"];
                    dgPlaylistSch.Rows[dgPlaylistSch.Rows.Count - 1].Cells["sName"].Value = dtDetail.Rows[iCtr]["pName"];
                    dgPlaylistSch.Rows[dgPlaylistSch.Rows.Count - 1].Cells["StartDate"].Value = string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["StartDate"]);
                    dgPlaylistSch.Rows[dgPlaylistSch.Rows.Count - 1].Cells["EndDate"].Value = string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["EndDate"]);
                    dgPlaylistSch.Rows[dgPlaylistSch.Rows.Count - 1].Cells["Time"].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["StartTime"]);
                    dgPlaylistSch.Rows[dgPlaylistSch.Rows.Count - 1].Cells["wDay"].Value = dtDetail.Rows[iCtr]["WeekDay"];
                    dgPlaylistSch.Rows[dgPlaylistSch.Rows.Count - 1].Cells["pId"].Value = dtDetail.Rows[iCtr]["PlaylistId"];
                }


            }
        }
        private void ClearPlaylistSch()
        {
            ModifyPlaylistSheduleId = 0;
            btnSavePlaylist.Text = "Save";
            cmbHourPlaylist.Text = "";
            cmbMinPlaylist.Text = "";
            cmbAmPmPlaylist.Text = "";
            cmbWeekDay.Text = "";
            dtpStartDatePlaylist.Value = DateTime.Now.Date;
            dtpEndDatePlaylist.Value = DateTime.Now.Date;
            dtpSearchDatePlaylist.Value = DateTime.Now.Date;
            string str = "";
            str = "Select distinct Playlists.playlistid, Playlists.Name from  Playlists ";
            str = str + " inner join TitlesInPlaylists on TitlesInPlaylists.playlistid= Playlists.playlistid";
            str = str + " where tokenid = " + StaticClass.TokenId + " order by Name";
            ObjMainClass.fnFillComboBox_Local(str, cmbPlaylist, "playlistid", "Name", "");
            FillPlaylistShedule();
            cmbPlaylist.Focus();
        }
        private void btnRefershPlaylist_Click(object sender, EventArgs e)
        {
            ClearPlaylistSch();
        }
        private Boolean SubmitValidationPlaylist()
        {
            if (dtpEndDatePlaylist.Value < dtpStartDatePlaylist.Value)
            {
                MessageBox.Show("Please select proper date's", "Copyright Player");
                dtpEndDatePlaylist.Focus();
                return false;
            }
            if (Convert.ToInt32(cmbPlaylist.SelectedValue) == 0)
            {
                MessageBox.Show("Playlist name cannot be balnk", "Copyright Player");
                cmbPlaylist.Focus();
                return false;
            }
            if (cmbHourPlaylist.Text == "")
            {
                MessageBox.Show("Please select the hour", "Copyright Player");
                cmbHourPlaylist.Focus();
                return false;
            }
            if (cmbMinPlaylist.Text == "")
            {
                MessageBox.Show("Please select minute", "Copyright Player");
                cmbMinPlaylist.Focus();
                return false;
            }
            if (cmbWeekDay.Text == "")
            {
                MessageBox.Show("Please select weekdays", "Copyright Player");
                cmbMinPlaylist.Focus();
                return false;
            }
            if (cmbAmPmPlaylist.Text == "")
            {
                MessageBox.Show("Please select time type", "Copyright Player");
                cmbAmPmPlaylist.Focus();
                return false;
            }
            string GetComboTimeString = "";
            GetComboTimeString = cmbHourPlaylist.Text + ":" + cmbMinPlaylist.Text + " " + cmbAmPmPlaylist.Text;
            DateTime GetComboTime = DateTime.ParseExact(GetComboTimeString, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);

            string strClientTimeValid = "";
            if (btnSavePlaylist.Text == "Save")
            {
                if (cmbWeekDay.Text == "All")
                {
                    strClientTimeValid = "select * from tbPlaylistSchedule where  ";
                    strClientTimeValid = strClientTimeValid + " StartTime=#" + GetComboTime.ToString("hh:mm tt") + "# ";
                    strClientTimeValid = strClientTimeValid + " and (#" + dtpStartDatePlaylist.Value.ToString("dd/MMM/yyyy") + "# between StartDate and  EndDate";
                    strClientTimeValid = strClientTimeValid + " or #" + dtpEndDatePlaylist.Value.ToString("dd/MMM/yyyy") + "# between StartDate and  EndDate )";
                }
                else
                {
                    strClientTimeValid = "select * from tbPlaylistSchedule where (WeekDay= '" + cmbWeekDay.Text + "' or WeekDay='All') and ";
                    strClientTimeValid = strClientTimeValid + " StartTime=#" + GetComboTime.ToString("hh:mm tt") + "# ";
                    strClientTimeValid = strClientTimeValid + " and (#" + dtpStartDatePlaylist.Value.ToString("dd/MMM/yyyy") + "# between StartDate and  EndDate";
                    strClientTimeValid = strClientTimeValid + " or #" + dtpEndDatePlaylist.Value.ToString("dd/MMM/yyyy") + "# between StartDate and  EndDate )";
                }
            }
            else
            {
                if (cmbWeekDay.Text == "All")
                {
                    strClientTimeValid = "select * from tbPlaylistSchedule where  ";
                    strClientTimeValid = strClientTimeValid + " StartTime=#" + GetComboTime.ToString("hh:mm tt") + "# ";
                    strClientTimeValid = strClientTimeValid + " and (#" + dtpStartDatePlaylist.Value.ToString("dd/MMM/yyyy") + "# between StartDate and  EndDate";
                    strClientTimeValid = strClientTimeValid + " or #" + dtpEndDatePlaylist.Value.ToString("dd/MMM/yyyy") + "# between StartDate and  EndDate )";
                    strClientTimeValid = strClientTimeValid + " and SchId   <> " + ModifyPlaylistSheduleId;
                }
                else
                {
                    strClientTimeValid = "select * from tbPlaylistSchedule where   (WeekDay= '" + cmbWeekDay.Text + "'  or WeekDay='All') and ";
                    strClientTimeValid = strClientTimeValid + " StartTime=#" + GetComboTime.ToString("hh:mm tt") + "# ";
                    strClientTimeValid = strClientTimeValid + " and (#" + dtpStartDatePlaylist.Value.ToString("dd/MMM/yyyy") + "# between StartDate and  EndDate";
                    strClientTimeValid = strClientTimeValid + " or #" + dtpEndDatePlaylist.Value.ToString("dd/MMM/yyyy") + "# between StartDate and  EndDate )";
                    strClientTimeValid = strClientTimeValid + " and SchId   <> " + ModifyPlaylistSheduleId;
                }
            }
            DataTable dtClientTimeValid = new DataTable();
            dtClientTimeValid = ObjMainClass.fnFillDataTable_Local(strClientTimeValid);
            if (dtClientTimeValid.Rows.Count > 0)
            {
                MessageBox.Show("This time is already used", "Copyright Player");
                cmbMinPlaylist.Focus();
                return false;
            }
            return true;
        }
        private void btnSavePlaylist_Click(object sender, EventArgs e)
        {
            string sQr = "";
            if (SubmitValidationPlaylist() == false) return;
            string GetComboTimeString = "";
            GetComboTimeString = cmbHourPlaylist.Text + ":" + cmbMinPlaylist.Text + " " + cmbAmPmPlaylist.Text;
            DateTime GetComboTime = DateTime.ParseExact(GetComboTimeString, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            if (btnSavePlaylist.Text == "Save")
            {
                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                sQr = "insert into tbPlaylistSchedule(PlaylistId,StartDate,EndDate,StartTime,WeekDay) values(" + Convert.ToInt32(cmbPlaylist.SelectedValue) + ", ";
                sQr = sQr + "#" + dtpStartDatePlaylist.Value.ToString("dd/MMM/yyyy") + "# , #" + dtpEndDatePlaylist.Value.ToString("dd/MMM/yyyy") + "#, #" + string.Format("{0:hh:mm tt}", GetComboTime) + "# ,'" + cmbWeekDay.Text + "' )";
                StaticClass.LocalCon.Open();
                OleDbCommand cmdSaveLocal = new OleDbCommand();
                cmdSaveLocal.Connection = StaticClass.LocalCon;
                cmdSaveLocal.CommandText = sQr;
                cmdSaveLocal.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            else
            {
                sQr = "";
                if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                sQr = "update tbPlaylistSchedule set PlaylistId= " + Convert.ToInt32(cmbPlaylist.SelectedValue) + ",StartDate= #" + dtpStartDatePlaylist.Value.ToString("dd/MMM/yyyy") + "#, ";
                sQr = sQr + " EndDate= #" + dtpEndDatePlaylist.Value.ToString("dd/MMM/yyyy") + "#, StartTime= #" + string.Format("{0:hh:mm tt}", GetComboTime) + "# ";
                sQr = sQr + " , WeekDay = '" + cmbWeekDay.Text + "' ";
                sQr = sQr + " where SchId= " + ModifyPlaylistSheduleId;
                StaticClass.LocalCon.Open();
                OleDbCommand cmdSaveLocal = new OleDbCommand();
                cmdSaveLocal.Connection = StaticClass.LocalCon;
                cmdSaveLocal.CommandText = sQr;
                cmdSaveLocal.ExecuteNonQuery();
                StaticClass.LocalCon.Close();
            }
            ClearPlaylistSch();
        }
        private void dgPlaylistSch_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                string GetComboTimeString = "";
                GetComboTimeString = dgPlaylistSch.Rows[e.RowIndex].Cells["Time"].Value.ToString();
                DateTime GetComboTime = DateTime.ParseExact(GetComboTimeString, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                ModifyPlaylistSheduleId = Convert.ToInt32(dgPlaylistSch.Rows[e.RowIndex].Cells["ScheduleId"].Value);
                dtpStartDatePlaylist.Value = Convert.ToDateTime(dgPlaylistSch.Rows[e.RowIndex].Cells["StartDate"].Value);
                dtpEndDatePlaylist.Value = Convert.ToDateTime(dgPlaylistSch.Rows[e.RowIndex].Cells["EndDate"].Value);
                cmbHourPlaylist.Text = string.Format("{0:hh}", GetComboTime);
                cmbMinPlaylist.Text = string.Format("{0:mm}", GetComboTime);
                cmbAmPmPlaylist.Text = string.Format("{0:tt}", GetComboTime);
                cmbWeekDay.Text = dgPlaylistSch.Rows[e.RowIndex].Cells["wDay"].Value.ToString();
                cmbPlaylist.SelectedValue = Convert.ToInt32(dgPlaylistSch.Rows[e.RowIndex].Cells["pId"].Value);
                btnSavePlaylist.Text = "Update";
            }
            if (e.ColumnIndex == 7)
            {
                string sQr = "";
                DialogResult result;
                result = MessageBox.Show("Are you sure delete ?", "Copyright Player", MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    if (StaticClass.LocalCon.State == ConnectionState.Open) StaticClass.LocalCon.Close();
                    sQr = "delete from  tbPlaylistSchedule where SchId= " + Convert.ToInt32(dgPlaylistSch.Rows[e.RowIndex].Cells["ScheduleId"].Value);
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdSaveLocal = new OleDbCommand();
                    cmdSaveLocal.Connection = StaticClass.LocalCon;
                    cmdSaveLocal.CommandText = sQr;
                    cmdSaveLocal.ExecuteNonQuery();
                    StaticClass.LocalCon.Close();
                    ClearPlaylistSch();
                }

            }
        }
        private void dtpSearchDatePlaylist_ValueChanged(object sender, EventArgs e)
        {
            FillPlaylistShedule();
        }
        private void timGetPlaylistSchCurrentTime_Tick(object sender, EventArgs e)
        {
            if (PlaylistTime != string.Format("{0:hh:mm tt}", DateTime.Now))
            {
                PlaylistTime = string.Format("{0:hh:mm tt}", DateTime.Now);
                for (int iRow = 0; iRow < dgPlaylistSch.Rows.Count; iRow++)
                {
                    if (Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date)) >= Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dgPlaylistSch.Rows[iRow].Cells["StartDate"].Value)))
                    {
                        if (Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date)) <= Convert.ToDateTime(string.Format("{0:dd/MMM/yyyy}", dgPlaylistSch.Rows[iRow].Cells["EndDate"].Value)))
                        {
                            if (dgPlaylistSch.Rows[iRow].Cells["wDay"].Value.ToString() != "All")
                            {
                                if (dgPlaylistSch.Rows[iRow].Cells["wDay"].Value.ToString() == DateTime.Now.Date.DayOfWeek.ToString())
                                {
                                    if (Convert.ToDateTime(string.Format("{0:hh:mm tt}", DateTime.Now)) == Convert.ToDateTime(string.Format("{0:hh:mm tt}", dgPlaylistSch.Rows[iRow].Cells["Time"].Value)))
                                    {
                                        PlaylistRow = iRow;
                                        StartPlaylist = 0;
                                        timStartPlaylistSchedule.Enabled = true;
                                        break;
                                    }
                                }
                            }
                            else if (Convert.ToDateTime(string.Format("{0:hh:mm tt}", DateTime.Now)) == Convert.ToDateTime(string.Format("{0:hh:mm tt}", dgPlaylistSch.Rows[iRow].Cells["Time"].Value)))
                            {
                                PlaylistRow = iRow;
                                StartPlaylist = 0;
                                timStartPlaylistSchedule.Enabled = true;
                                break;
                            }
                        }
                    }
                }
            }
        }
        private void timStartPlaylistSchedule_Tick(object sender, EventArgs e)
        {
        RunAg:
            if (StartPlaylist != 0)
            {
                StartPlaylist = StartPlaylist - 1;
            }
            if (StartPlaylist == 0)
            {
                if ((musicPlayer1.URL != "") && (musicPlayer2.URL != ""))
                {
                    StartPlaylist = 20;
                    goto RunAg;
                }
                if (panAdvt.Visible == true)
                {
                    StartPlaylist = 10;
                }

                lblStreamTime.Text = "";

                if (musicPlayer1.URL != "")
                {
                    musicPlayer1.Ctlcontrols.pause();
                }
                else if (musicPlayer2.URL != "")
                {
                    musicPlayer2.Ctlcontrols.pause();
                }
                if (dgPlaylistSch.Rows.Count > 0)
                {
                    for (int i = 0; i <= dgLocalPlaylist.Rows.Count - 1; i++)
                    {
                        if (Convert.ToInt32(dgPlaylistSch.Rows[PlaylistRow].Cells["pId"].Value) == Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value))
                        {
                            panStream.Visible = false;
                            StreamMusicPlayer.Ctlcontrols.stop();
                            StreamMusicPlayer.URL = "";
                            timStreamMusicTime.Enabled = false;
                            dgLocalPlaylist.Rows[i].Selected = true;
                            PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgPlaylistSch.Rows[PlaylistRow].Cells["pId"].Value));
                            PlaySongDefault();
                            DisplaySongPlayerOne();
                            Set_foucs_PayerOne();
                            g = Path.GetFileNameWithoutExtension(musicPlayer1.URL);
                            GetSavedRating(g, dgSongRatingPlayerOne);
                            SetDisableRating(dgSongRatingPlayerTwo);
                            musicPlayer2.settings.volume = 0;
                            musicPlayer2.Ctlcontrols.stop();
                            musicPlayer2.URL = "";
                        }
                    }
                }
                timStartPlaylistSchedule.Enabled = false;
            }
        }


        private int ReturnWeekId(string CurrentWeekday)
        {
            if (CurrentWeekday == "Sunday")
            {
                return 1;
            }
            if (CurrentWeekday == "Monday")
            {
                return 2;
            }
            if (CurrentWeekday == "Tuesday")
            {
                return 3;
            }
            if (CurrentWeekday == "Wednesday")
            {
                return 4;
            }
            if (CurrentWeekday == "Thursday")
            {
                return 5;
            }
            if (CurrentWeekday == "Friday")
            {
                return 6;
            }
            if (CurrentWeekday == "Saturday")
            {
                return 7;
            }
            return 0;
        }





        #region Prayer Implement
        private void InitilizePrayerGrid(DataGridView dgGrid)
        {
            if (dgGrid.Rows.Count > 0)
            {
                dgGrid.Rows.Clear();
            }
            if (dgGrid.Columns.Count > 0)
            {
                dgGrid.Columns.Clear();
            }

            dgGrid.Columns.Add("pId", "Id");
            dgGrid.Columns["pId"].Width = 0;
            dgGrid.Columns["pId"].Visible = false;
            dgGrid.Columns["pId"].ReadOnly = true;

            dgGrid.Columns.Add("sDate", "Start Date");
            dgGrid.Columns["sDate"].Width = 200;
            dgGrid.Columns["sDate"].Visible = false;
            dgGrid.Columns["sDate"].ReadOnly = true;
            dgGrid.Columns["sDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgGrid.Columns.Add("eDate", "End Date");
            dgGrid.Columns["eDate"].Width = 200;
            dgGrid.Columns["eDate"].Visible = false;
            dgGrid.Columns["eDate"].ReadOnly = true;
            dgGrid.Columns["eDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgGrid.Columns.Add("sTime", "Start Time");
            dgGrid.Columns["sTime"].Width = 200;
            dgGrid.Columns["sTime"].Visible = true;
            dgGrid.Columns["sTime"].ReadOnly = true;
            dgGrid.Columns["sTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgGrid.Columns.Add("eTime", "End Time");
            dgGrid.Columns["eTime"].Width = 200;
            dgGrid.Columns["eTime"].Visible = true;
            dgGrid.Columns["eTime"].ReadOnly = true;
            dgGrid.Columns["eTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


        }
        private void FillPrayer(DataGridView dgGrid)
        {
            string str = "";
            int iCtr;
            DataTable dtDetail;
            str = "select * from tbPrayer where #" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "# between sdate and edate";
            dtDetail = ObjMainClass.fnFillDataTable_Local(str);
            if ((dtDetail.Rows.Count <= 0))
            {
                str = "spGetPrayerData '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','Copyright'," + StaticClass.UserId + ",  " + StaticClass.AdvtCityId + "," + StaticClass.UserId + " , " + StaticClass.CountryId + ", " + StaticClass.Stateid + ", " + StaticClass.TokenId;
                // str = "spGetPrayerData '29-Feb-2016','NativeCR',70590,3,70590,20,1,1973";
                dtDetail = ObjMainClass.fnFillDataTable(str);
            }
            InitilizePrayerGrid(dgGrid);
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgGrid.Rows.Add();
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["pId"].Value = dtDetail.Rows[iCtr]["pId"];
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["sDate"].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["sDate"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["eDate"].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["eDate"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["sTime"].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["sTime"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["eTime"].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["eTime"]);
                }
                foreach (DataGridViewRow row in dgGrid.Rows)
                {
                    row.Height = 30;
                }
                timPrayer.Enabled = true;
            }
        }
        private void panPrayer_VisibleChanged(object sender, EventArgs e)
        {
            if (panPrayer.Visible == true)
            {

                panPrayer.Width = this.Width;
                panPrayer.Height = this.Height;
                panPrayer.Location = new Point(0, 0);
            }

        }
        string PrayerTime = "";
        string IsPrayerRunning = "No";
        private void timPrayer_Tick(object sender, EventArgs e)
        {
            if (PrayerTime != string.Format("{0:hh:mm tt}", DateTime.Now))
            {
                PrayerTime = string.Format("{0:hh:mm tt}", DateTime.Now);
                for (int iRow = 0; iRow < dgPrayer.Rows.Count; iRow++)
                {
                    if (Convert.ToDateTime(string.Format("{0:hh:mm tt}", DateTime.Now)) >= Convert.ToDateTime(string.Format("{0:hh:mm tt}", dgPrayer.Rows[iRow].Cells["sTime"].Value)) && (Convert.ToDateTime(string.Format("{0:hh:mm tt}", DateTime.Now)) < Convert.ToDateTime(string.Format("{0:hh:mm tt}", dgPrayer.Rows[iRow].Cells["eTime"].Value))))
                    {
                        if (IsPrayerRunning == "No")
                        {
                            Mute();
                            IsPrayerRunning = "Yes";
                            panPrayer.BringToFront();
                            panPrayer.Visible = true;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (Convert.ToDateTime(string.Format("{0:hh:mm tt}", DateTime.Now)) == Convert.ToDateTime(string.Format("{0:hh:mm tt}", dgPrayer.Rows[iRow].Cells["eTime"].Value)))
                    {
                        UnMute();
                        IsPrayerRunning = "No";
                        panPrayer.Visible = false;
                    }
                }
            }
        }
        string IsAlreadyMute = "No";
        string IsAlreadyMuteStream = "No";
        private void Mute()
        {
            if (btnMute.Text == ".")
            {
                IsAlreadyMute = "Yes";
            }
            else
            {
                IsAlreadyMute = "No";
            }
            btnMute.Text = ".";
            musicPlayer1.settings.mute = true;
            musicPlayer2.settings.mute = true;
            Song_Mute = true;
            btnMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_red));

            if (btnStreamMute.Text == ".")
            {
                IsAlreadyMuteStream = "Yes";
            }
            else
            {
                IsAlreadyMuteStream = "No";
            }

            btnStreamMute.Text = ".";
            StreamMusicPlayer.settings.mute = true;
            btnStreamMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_red));

        }
        private void UnMute()
        {
            if (IsAlreadyMute == "No")
            {
                btnMute.Text = "";
                musicPlayer1.settings.mute = false;
                musicPlayer2.settings.mute = false;
                musicPlayer1.settings.volume = 100;
                musicPlayer2.settings.volume = 100;
                Song_Mute = false;
                btnMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_blue));
            }
            if (IsAlreadyMuteStream == "No")
            {
                btnStreamMute.Text = "";
                StreamMusicPlayer.settings.mute = false;
                StreamMusicPlayer.settings.volume = 100;
                btnStreamMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_blue));
            }
        }
        #endregion

        #region Save Prayer
        Int32 SearchPrayerId = 0;
        private void picBackMusic_Click(object sender, EventArgs e)
        {
            panPrayerDetail.Visible = false;
        }

        private void picPrayer_Click(object sender, EventArgs e)
        {
            panPrayerDetail.Visible = true;
        }

        private void panPrayerDetail_VisibleChanged(object sender, EventArgs e)
        {
            if (panPrayerDetail.Visible == false) return;
            panPrayerDetail.Size = new Size(this.Width, this.Height);
            panPrayerDetail.Location = new Point(0, 0);
            dgSearchPrayer.Dock = DockStyle.Fill;
            dgSearchPrayer.BorderStyle = BorderStyle.None;
            dtpsDatePrayer.Value = DateTime.Now.Date;
            dtpeDatePrayer.Value = DateTime.Now.Date;
            dtpsTimePrayer.Value = DateTime.Now;
            dtpeTimePrayer.Value = DateTime.Now;
            dtpSearchPrayer.Value = DateTime.Now.Date;
        }

        private Boolean SubmitValidationPrayer()
        {
            if (dtpEndDate.Value < dtpStartDate.Value)
            {
                MessageBox.Show("Please select proper date's", "Management Panel");
                dtpEndDate.Focus();
                return false;
            }
            if (dtpeTimePrayer.Value < dtpsTimePrayer.Value)
            {
                MessageBox.Show("Please select proper time", "Management Panel");
                dtpeTimePrayer.Focus();
                return false;
            }
            DataTable dtDetail;
            string str = "";
            if (SearchPrayerId == 0)
            {
                str = "select * from tbPrayer where (#" + string.Format("{0:hh:mm tt}", dtpsTimePrayer.Value) + "# between sTime and eTime";
                str = str + " or #" + string.Format("{0:hh:mm tt}", dtpeTimePrayer.Value) + "# between sTime and eTime) and ";
                str = str + " (#" + string.Format("{0:dd/MMM/yyyy}", dtpsDatePrayer.Value) + "# between sDate and eDate";
                str = str + " or #" + string.Format("{0:dd/MMM/yyyy}", dtpeDatePrayer.Value) + "# between sDate and eDate) ";
            }
            else
            {
                str = "select * from tbPrayer where (#" + string.Format("{0:hh:mm tt}", dtpsTimePrayer.Value) + "# between sTime and eTime";
                str = str + " or #" + string.Format("{0:hh:mm tt}", dtpeTimePrayer.Value) + "# between sTime and eTime) and pid <>" + SearchPrayerId;
                str = str + " and (#" + string.Format("{0:dd/MMM/yyyy}", dtpsDatePrayer.Value) + "# between sDate and eDate";
                str = str + " or #" + string.Format("{0:dd/MMM/yyyy}", dtpeDatePrayer.Value) + "# between sDate and eDate) ";
            }
            dtDetail = ObjMainClass.fnFillDataTable_Local(str);
            if ((dtDetail.Rows.Count > 0))
            {
                MessageBox.Show("Prayer time is already exixts", "Copyright Player");
                dtpsTimePrayer.Focus();
                return false;
            }
            return true;
        }
        //
        private void InitilizeSearchPrayerGrid(DataGridView dgGrid)
        {
            if (dgGrid.Rows.Count > 0)
            {
                dgGrid.Rows.Clear();
            }
            if (dgGrid.Columns.Count > 0)
            {
                dgGrid.Columns.Clear();
            }

            dgGrid.Columns.Add("pId", "Id");
            dgGrid.Columns["pId"].Width = 0;
            dgGrid.Columns["pId"].Visible = false;
            dgGrid.Columns["pId"].ReadOnly = true;

            dgGrid.Columns.Add("sDate", "Start Date");
            dgGrid.Columns["sDate"].Width = 200;
            dgGrid.Columns["sDate"].Visible = true;
            dgGrid.Columns["sDate"].ReadOnly = true;
            dgGrid.Columns["sDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgGrid.Columns.Add("eDate", "End Date");
            dgGrid.Columns["eDate"].Width = 200;
            dgGrid.Columns["eDate"].Visible = true;
            dgGrid.Columns["eDate"].ReadOnly = true;
            dgGrid.Columns["eDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgGrid.Columns.Add("sTime", "Start Time");
            dgGrid.Columns["sTime"].Width = 200;
            dgGrid.Columns["sTime"].Visible = true;
            dgGrid.Columns["sTime"].ReadOnly = true;
            dgGrid.Columns["sTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgGrid.Columns.Add("eTime", "End Time");
            dgGrid.Columns["eTime"].Width = 200;
            dgGrid.Columns["eTime"].Visible = true;
            dgGrid.Columns["eTime"].ReadOnly = true;
            dgGrid.Columns["eTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            DataGridViewLinkColumn Edit = new DataGridViewLinkColumn();
            Edit.HeaderText = "Edit";
            Edit.Text = "Edit";
            Edit.DataPropertyName = "Edit";
            dgGrid.Columns.Add(Edit);
            Edit.UseColumnTextForLinkValue = true;
            Edit.Width = 70;
            Edit.Visible = true;
            dgGrid.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            DataGridViewLinkColumn Delete = new DataGridViewLinkColumn();
            Delete.HeaderText = "Delete";
            Delete.Text = "Delete";
            Delete.DataPropertyName = "Delete";
            dgGrid.Columns.Add(Delete);
            Delete.UseColumnTextForLinkValue = true;
            Delete.Width = 70;
            Delete.Visible = true;
            dgGrid.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

        }

        private void FillSearchPrayer(DataGridView dgGrid)
        {
            string str = "";
            int iCtr;
            DataTable dtDetail;
            str = "select * from tbPrayer where #" + string.Format("{0:dd/MMM/yyyy}", dtpSearchPrayer.Value) + "# between sdate and edate";
            dtDetail = ObjMainClass.fnFillDataTable_Local(str);
            InitilizeSearchPrayerGrid(dgGrid);
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgGrid.Rows.Add();
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["pId"].Value = dtDetail.Rows[iCtr]["pId"];
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["sDate"].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["sDate"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["eDate"].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["eDate"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["sTime"].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["sTime"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["eTime"].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["eTime"]);
                }
                foreach (DataGridViewRow row in dgGrid.Rows)
                {
                    row.Height = 30;
                }
            }
        }

        private void dtpSearchPrayer_ValueChanged(object sender, EventArgs e)
        {
            FillSearchPrayer(dgSearchPrayer);
        }

        private void btnSavePrayer_Click(object sender, EventArgs e)
        {
            if (SubmitValidationPrayer() == false) return;
            string str = "";
            str = "delete from tbPrayer where pid=" + SearchPrayerId;
            if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
            StaticClass.LocalCon.Open();
            OleDbCommand cmdDel = new OleDbCommand();
            cmdDel.Connection = StaticClass.LocalCon;
            cmdDel.CommandText = str;
            cmdDel.ExecuteNonQuery();

            str = "insert into tbPrayer(sDate,eDate,sTime,eTime) values(#" + string.Format("{0:dd/MMM/yyyy}", dtpsDatePrayer.Value) + "# ,";
            str = str + " #" + string.Format("{0:dd/MMM/yyyy}", dtpeDatePrayer.Value) + "# ,#" + string.Format("{0:hh:mm tt}", dtpsTimePrayer.Value) + "#, #" + string.Format("{0:hh:mm tt}", dtpeTimePrayer.Value) + "# )";
            if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
            StaticClass.LocalCon.Open();
            OleDbCommand cmdIns = new OleDbCommand();
            cmdIns.Connection = StaticClass.LocalCon;
            cmdIns.CommandText = str;
            cmdIns.ExecuteNonQuery();
            ClearPrayerFeilds();
            FillSearchPrayer(dgSearchPrayer);
            FillPrayer(dgPrayer);
        }
        private void ClearPrayerFeilds()
        {
            dtpsDatePrayer.Value = DateTime.Now.Date;
            dtpeDatePrayer.Value = DateTime.Now.Date;
            dtpsTimePrayer.Value = DateTime.Now;
            dtpeTimePrayer.Value = DateTime.Now;
            SearchPrayerId = 0;
            btnSavePrayer.Text = "Save";
        }

        private void btnRefershPrayer_Click(object sender, EventArgs e)
        {
            ClearPrayerFeilds();
        }

        private void dgSearchPrayer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                btnSavePrayer.Text = "Update";
                SearchPrayerId = Convert.ToInt32(dgSearchPrayer.Rows[e.RowIndex].Cells["pid"].Value);
                dtpsDatePrayer.Value = Convert.ToDateTime(dgSearchPrayer.Rows[e.RowIndex].Cells["sDate"].Value);
                dtpeDatePrayer.Value = Convert.ToDateTime(dgSearchPrayer.Rows[e.RowIndex].Cells["eDate"].Value);
                dtpsTimePrayer.Value = Convert.ToDateTime(dgSearchPrayer.Rows[e.RowIndex].Cells["sTime"].Value);
                dtpeTimePrayer.Value = Convert.ToDateTime(dgSearchPrayer.Rows[e.RowIndex].Cells["eTime"].Value);
            }
            if (e.ColumnIndex == 6)
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;
                result = MessageBox.Show("Are you sure to delete this prayer?", "Copyright Player", buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    string str = "";
                    str = "delete from tbPrayer where pid=" + Convert.ToInt32(dgSearchPrayer.Rows[e.RowIndex].Cells["pid"].Value);
                    if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }
                    StaticClass.LocalCon.Open();
                    OleDbCommand cmdDel = new OleDbCommand();
                    cmdDel.Connection = StaticClass.LocalCon;
                    cmdDel.CommandText = str;
                    cmdDel.ExecuteNonQuery();
                    FillSearchPrayer(dgSearchPrayer);
                }
            }
        }
        #endregion
        Int32 ReNetStateTime = 180;
        private void timGetNetState_Tick(object sender, EventArgs e)
        {
        connectAgain:
            ReNetStateTime = ReNetStateTime - 1;
             
            if (ReNetStateTime == 0)
            {

                if (ObjMainClass.CheckForInternetConnection() == true)
                {
                    if (musicPlayer1.URL != "")
                    {
                        if (musicPlayer1.Ctlcontrols.currentPosition <= 20)
                        {
                            ReNetStateTime = 20;
                            goto connectAgain;
                        }
                    }
                    if (musicPlayer2.URL != "")
                    {
                        if (musicPlayer2.Ctlcontrols.currentPosition <= 20)
                        {
                            ReNetStateTime = 20;
                            goto connectAgain;
                        }
                    }
                    if ((musicPlayer1.URL != "") && (musicPlayer2.URL != ""))
                    {
                        ReNetStateTime = 20;
                        goto connectAgain;
                    }
                    FillStreamData();
                    if (ClientsRightsValidation() == false) return;
                    if (dgStream.Rows.Count == 0)
                    {
                        ReNetStateTime = 15;
                        goto connectAgain;
                    }
                    lblStreamTime.Text = "";
                    string strStream = "";
                    DataTable dtGetSaveStream = new DataTable();
                    if (musicPlayer1.URL != "")
                    {
                        musicPlayer1.Ctlcontrols.pause();
                    }
                    else if (musicPlayer2.URL != "")
                    {
                        musicPlayer2.Ctlcontrols.pause();
                    }

                    strStream = "select * from tbLastStream where tokenid= " + StaticClass.TokenId;
                    dtGetSaveStream = ObjMainClass.fnFillDataTable_Local(strStream);
                    if (dtGetSaveStream.Rows.Count > 0)
                    {
                        for (int i = 0; i <= dgStream.Rows.Count - 1; i++)
                        {
                            if (Convert.ToInt32(dtGetSaveStream.Rows[0]["StreamId"]) == Convert.ToInt32(dgStream.Rows[i].Cells["streamId"].Value))
                            {
                                lblStreamName.Text = dgStream.Rows[i].Cells["StreamName"].Value.ToString();
                                StreamMusicPlayer.URL = dgStream.Rows[i].Cells["StreamLink"].Value.ToString();
                                dgStream.CurrentCell = dgStream.Rows[i].Cells["StreamName"];
                                dgStream.Rows[i].Selected = true;
                                break;
                            }
                            else
                            {
                                lblStreamName.Text = dgStream.Rows[0].Cells["StreamName"].Value.ToString();
                                StreamMusicPlayer.URL = dgStream.Rows[0].Cells["StreamLink"].Value.ToString();
                            }
                        }
                    }
                    else
                    {
                        lblStreamName.Text = dgStream.Rows[0].Cells["StreamName"].Value.ToString();
                        StreamMusicPlayer.URL = dgStream.Rows[0].Cells["StreamLink"].Value.ToString();
                    }
                    panStream.Visible = true;
                    StreamMusicPlayer.Ctlcontrols.play();

                    panStream.Location = new Point(0, 0);

                    //panStream.Height = 670;
                    // panStream.Width = 1015;

                    panStream.Height = this.Height;
                    panStream.Width = this.Width;

                    // panStream.Dock = DockStyle.Fill;
                    btnStreamPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
                    btnStreamPlay.Text = "";
                    //btnStreamMute.Text = "";
                    dgStream.Rows[0].Selected = true;
                    dgStream.Rows[0].Cells["Column_Img_Stream"].Value = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
                    GetCurrentStream();
                    timStream.Enabled = true;
                    timStreamMusicTime.Enabled = true;
                     
                    timResetSong.Enabled = false;
                    timGetNetState.Enabled = false;
                    lblInternetStatus.Text = "10";
                }

                else
                {
                    ReNetStateTime = 180;
                }
            }
        }
    }
}
