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
using System.Threading;
using WMPLib;
using System.Data.OleDb;
using System.Drawing.Imaging;
using NetFwTypeLib;
using System.Globalization;
using System.Web.Caching;
using System.Configuration;

namespace SFVideoPlayer
{


    public partial class frmVideoPlayer : Form
    {
        DateTimeFormatInfo fi = new DateTimeFormatInfo();
        Int32 MusicPlayer1CurrentSongId = 0;
        Int32 MusicPlayer2CurrentSongId = 0;
        string FirstTimeConditation = "Yes";
        Int32 LastRowId = 0;
        Boolean IsVisibleSong = false;
        string ShowPlaylistCounter = "";
        string gblsongid = "";
        string StopDuplicate = "Yes";
        string IsLast100Working = "";
        string LocalSecondTime = "";
        string Drop_TitleName = "";
        Boolean PanelVisiable;
        string TempAdvtFileName;
        double AdvtTimeResult = 0;
        double TimePlayerOne = 0;
        double TimePlayerTwo = 0;
        double TimeStreamPlayer = 0;
        double PrvTimeStreamPlayer = 0;
        Boolean IsFirtTimeStreamComplete = false;
        Int32 AdvtCurrentRow = 0;
        Int32 AdvtCurrentSongId = 0;
        int eX = 0;
        int eY = 0;
        string DropTitleSong = "";
        string UpcomingSongPlayerOne = "";
        string UpcomingSongPlayerTwo = "";
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
        string SanjivaniRecordShowType = "";
        DataGridViewButtonColumn SongDownload = new DataGridViewButtonColumn();
        DataGridViewImageColumn Column_Img_Stream = new DataGridViewImageColumn();
        public WindowsMediaPlayer player;

        double prvPlayerOneTime = 0;
        double prvPlayerTwoTime = 0;
        Boolean IsAdvtTimeGet = false;
        double GrossTotaltime = 0;
        int rCount = 0;
        string DropSongLength = "";
        Boolean IsSongDropAdvt = false;

        DataGridView dgSaveDataGrid;
        Int32 SaveDataCurrentPlaylistId = 0;

        string IsStreamPlaying = "";
        Int32 ReNetStateTime = 180;
        Int32 LastStreamId = 0;
        string LastStreamLink = "";
        Int32 CheckStreamTime = 10;
        Boolean IsStreamUp = true;
        Boolean IsRestartDownloading = false;
        string AdvtPlayTime = "";
        string AdvtUrl = "";
        string AdvtFilePath = "";

        WebClient wcDownloadSplSongs = new WebClient();
        WebClient wcDownloadRequestSong = new WebClient();
        public frmVideoPlayer()
        {
            InitializeComponent();

        }
        private void InitilizeGrid(DataGridView dgGrid)
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
            dgGrid.Columns["songname"].Width = 300;
            dgGrid.Columns["songname"].Visible = true;
            dgGrid.Columns["songname"].ReadOnly = true;
            dgGrid.Columns["songname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgGrid.Columns.Add("Length", "Length");
            dgGrid.Columns["Length"].Width = 80;
            dgGrid.Columns["Length"].Visible = true;
            dgGrid.Columns["Length"].ReadOnly = true;
            dgGrid.Columns["Length"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;



            dgGrid.Columns.Add("Year", "Year");
            dgGrid.Columns["Year"].Width = 0;
            dgGrid.Columns["Year"].Visible = false;
            dgGrid.Columns["Year"].ReadOnly = true;

            dgGrid.Columns.Add("Artist", "Artist");
            dgGrid.Columns["Artist"].Width = 130;
            dgGrid.Columns["Artist"].Visible = true;
            dgGrid.Columns["Artist"].ReadOnly = true;
            dgGrid.Columns["Artist"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgGrid.Columns.Add("Album", "Album");
            dgGrid.Columns["Album"].Width = 130;
            dgGrid.Columns["Album"].Visible = true;
            dgGrid.Columns["Album"].ReadOnly = true;
            dgGrid.Columns["Album"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgGrid.Columns.Add("lUrl", "lUrl");
            dgGrid.Columns["lUrl"].Width = 0;
            dgGrid.Columns["lUrl"].Visible = false;
            dgGrid.Columns["lUrl"].ReadOnly = true;
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
            dgLocalPlaylist.Columns["playlistname"].Width = 240;
            dgLocalPlaylist.Columns["playlistname"].Visible = true;
            dgLocalPlaylist.Columns["playlistname"].ReadOnly = true;
            dgLocalPlaylist.Columns["playlistname"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgLocalPlaylist.Columns.Add("Default", "Default");
            //dgLocalPlaylist.Columns["playlistname"].Width = 210;
            dgLocalPlaylist.Columns["Default"].Visible = false;
            dgLocalPlaylist.Columns["Default"].ReadOnly = true;

            dgLocalPlaylist.Columns.Add("PlaylistColor", "");
            dgLocalPlaylist.Columns["PlaylistColor"].Width = 30;
            dgLocalPlaylist.Columns["PlaylistColor"].Visible = true;
            dgLocalPlaylist.Columns["PlaylistColor"].ReadOnly = true;
            //dgLocalPlaylist.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgLocalPlaylist.Columns["PlaylistColor"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            DataGridViewLinkColumn EditPlaylist = new DataGridViewLinkColumn();
            EditPlaylist.HeaderText = "Edit";
            EditPlaylist.Text = "Edit";
            EditPlaylist.DataPropertyName = "Edit";
            dgLocalPlaylist.Columns.Add(EditPlaylist);
            EditPlaylist.UseColumnTextForLinkValue = true;
            EditPlaylist.Width = 0;
            EditPlaylist.Visible = false;


            dgLocalPlaylist.Columns.Add("sTime", "sTime");
            dgLocalPlaylist.Columns["sTime"].Width = 0;
            dgLocalPlaylist.Columns["sTime"].Visible = false;
            dgLocalPlaylist.Columns["sTime"].ReadOnly = true;

            dgLocalPlaylist.Columns.Add("eTime", "eTime");
            dgLocalPlaylist.Columns["eTime"].Width = 0;
            dgLocalPlaylist.Columns["eTime"].Visible = false;
            dgLocalPlaylist.Columns["eTime"].ReadOnly = true;

            dgLocalPlaylist.Columns.Add("IsvMute", "IsvMute");
            dgLocalPlaylist.Columns["IsvMute"].Width = 0;
            dgLocalPlaylist.Columns["IsvMute"].Visible = false;
            dgLocalPlaylist.Columns["IsvMute"].ReadOnly = true;

        }


        private void PopulateInputFileTypeDetail(DataGridView dgGrid, Int32 currentPlayRow)
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
            if (StaticClass.ScheduleType == "OneToOnePlaylist")
            {
                mlsSql = " SELECT st.TitleID, ltrim(st.Title) as Title, st.Time, st.alname AS AlbumName ,";
                mlsSql = mlsSql + " 0 as TitleYear ,  ltrim(st.arname) as ArtistName,  TitlesInPlaylists.id FROM (TitlesInPlaylists ";
                mlsSql = mlsSql + " INNER JOIN tbSpecialPlaylists_Titles st ON TitlesInPlaylists.TitleID = st.TitleID )  ";
                mlsSql = mlsSql + " ORDER BY  Rnd(-(date() * TitlesInPlaylists.Id) * Time()),Rnd(-(date() * TitlesInPlaylists.titleid) * Time())";
            }
            else
            {
                mlsSql = "SELECT  Titles.TitleID, ltrim(Titles.Title) as Title, Titles.Time,Albums.Name AS AlbumName ,";
                mlsSql = mlsSql + " Titles.TitleYear ,   ltrim(Artists.Name) as ArtistName , Titles.LocalUrl FROM ((( TitlesInPlaylists  ";
                mlsSql = mlsSql + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID )  ";
                mlsSql = mlsSql + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ) ";
                mlsSql = mlsSql + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ) ";
                mlsSql = mlsSql + " where TitlesInPlaylists.PlaylistID=" + Convert.ToInt32(currentPlayRow) + "  ORDER BY Rnd(-(100000*Titles.TitleID)*Time())";
            }


            dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
            InitilizeGrid(dgGrid);
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    GetLocalPath = dtDetail.Rows[iCtr]["TitleID"] + ".mp4";
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
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["lUrl"].Value = dtDetail.Rows[iCtr]["localUrl"];

                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["songname"].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Length"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Album"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Artist"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);

                }
            }
            foreach (DataGridViewRow row in dgGrid.Rows)
            {
                row.Height = 30;
            }
            RowHide();

        }

        private void TitleCategoryRowHide()
        {
            for (int i = 0; i < dgTotalTitles.Rows.Count; i++)
            {
                foreach (DataGridViewRow dr in dgPlaylist.Rows)
                {
                    if (dr.Cells[0].Value.ToString() == dgTotalTitles.Rows[i].Cells[0].Value.ToString())
                    {
                        dgPlaylist.Rows.Remove(dr);
                        //dr.Visible = false;
                    }
                }
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
            str = "select playlistId,name, PlaylistDefault from Playlists where tokenid= " + StaticClass.TokenId;
            dtDetail = ObjMainClass.fnFillDataTable_Local(str);

            InitilizeLocalGrid();
            if ((dtDetail.Rows.Count > 0))
            {
                panTop.Visible = false;
                panBottom.Visible = false;
                picMiddleLogo.Visible = false;
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgLocalPlaylist.Rows.Add();
                    dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["playlistId"];

                    strGetCount = "select Count(*) as Total from  TitlesInPlaylists where playlistId =" + dtDetail.Rows[iCtr]["playlistId"] + " ";
                    dtGetCount = ObjMainClass.fnFillDataTable_Local(strGetCount);

                    dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["name"] + "  (" + dtGetCount.Rows[0]["Total"] + ") ";
                    dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["PlaylistDefault"];

                    strGetCount = "";
                    strGetCount = "select StartTime,EndTime, IsVideoMute, isnull(IsVideoMute) as vMute from  tbSplPlaylistSchedule where playlistId =" + dtDetail.Rows[iCtr]["playlistId"] + " ";
                    dtGetCount = ObjMainClass.fnFillDataTable_Local(strGetCount);
                    if ((dtGetCount.Rows.Count > 0))
                    {
                        dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells["sTime"].Value = dtGetCount.Rows[0]["StartTime"];
                        dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells["eTime"].Value = dtGetCount.Rows[0]["EndTime"];
                        if (dtGetCount.Rows[0]["vMute"].ToString() == "-1")
                        {
                            dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells["IsvMute"].Value = "0";
                        }
                        else
                        {
                            dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells["IsvMute"].Value = dtGetCount.Rows[0]["IsVideoMute"];
                        }
                    }
                    else
                    {
                        dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells["sTime"].Value = "Nill";
                        dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells["eTime"].Value = "Nill";
                    }

                    dgLocalPlaylist.Rows[dgLocalPlaylist.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);

                    if (ObjMainClass.CheckForInternetConnection() == true)
                    {
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = "update Playlists set tokenid=" + StaticClass.TokenId + " where playlistid=" + dtDetail.Rows[iCtr]["playlistId"];
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();
                    }

                }
                foreach (DataGridViewRow row in dgLocalPlaylist.Rows)
                {
                    row.Height = 30;
                    if (row.Cells[2].Value.ToString() == "Default")
                    {
                        IsFindDefaultPlaylist = true;
                        row.Selected = true;
                        StaticClass.DefaultPlaylistId = Convert.ToInt32(row.Cells[0].Value);
                        dgLocalPlaylist.CurrentCell = row.Cells[1];
                        StaticClass.DefaultPlaylistCurrentRow = dgLocalPlaylist.CurrentCell.RowIndex;

                        row.Cells[1].Style.ForeColor = Color.FromArgb(20, 162, 175);
                        row.Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                        row.Cells[1].Style.SelectionForeColor = Color.Yellow;

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
                if (IsFindDefaultPlaylist == false)
                {
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    OleDbCommand cmdUpdateAll = new OleDbCommand();
                    cmdUpdateAll.Connection = StaticClass.LocalCon;
                    cmdUpdateAll.CommandText = "Update Playlists set PlaylistDefault=''";
                    cmdUpdateAll.ExecuteNonQuery();


                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    OleDbCommand cmdUpdate = new OleDbCommand();
                    cmdUpdate.Connection = StaticClass.LocalCon;
                    cmdUpdate.CommandText = "Update Playlists set PlaylistDefault='Default' where playlistid = " + dgLocalPlaylist.Rows[0].Cells[0].Value.ToString();
                    cmdUpdate.ExecuteNonQuery();

                    FillLocalPlaylist();
                }
            }
            else
            {
                panTop.Dock = DockStyle.Top;
                panTop.BringToFront();
                panTop.Visible = true;

                panBottom.Dock = DockStyle.Bottom;
                panBottom.BringToFront();
                panBottom.Visible = true;

                picMiddleLogo.BringToFront();
                picMiddleLogo.Visible = true;
            }
        }
        private void GetTotalSongs()
        {
            string str = "";
            DataTable dtTotalTitles;
            str = "SELECT distinct Titles.TitleID FROM TitlesInPlaylists  ";
            str = str + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID   ";
            str = str + " where TitlesInPlaylists.PlaylistID in(select PlaylistID from Playlists where Userid=" + StaticClass.dfClientId + ") ";
            str = str + " and titles.titlecategoryid in(4) ";
            dtTotalTitles = ObjMainClass.fnFillDataTable(str);
            dgTotalTitles.DataSource = dtTotalTitles;
        }
        string IsFormatFirstTimeLoad = "Yes";
        private NotifyIcon m_notifyicon;
        private ContextMenu m_menu;
        protected void Exit_Click(Object sender, System.EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show("Are you sure to exit ?", "Music Player", buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    if (ObjMainClass.CheckForInternetConnection() == true)
                    {
                        UploadPlayerStatus();
                        #region Upload LogOut Status

                        string strZ = "insert into tbTokenLogOutStatus(TokenId,StatusDate,StatusTime) values(" + StaticClass.TokenId + " , ";
                        strZ = strZ + " '" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "','" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "') ";
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmdLog = new SqlCommand();
                        cmdLog.Connection = StaticClass.constr;
                        cmdLog.CommandText = strZ;
                        cmdLog.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        #endregion
                    }
                }
                catch (Exception ex)
                {

                }
                m_notifyicon.Dispose();


                try
                {
                    Process[] prs = Process.GetProcesses();
                    foreach (Process pr in prs)
                    {
                        if (pr.ProcessName == "SFVideoPlayer")
                            pr.Kill();
                    }
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    Application.Exit();
                }

            }

        }
        protected void Hide_Click(Object sender, System.EventArgs e)
        {
            Hide();
        }
        protected void Show_Click(Object sender, System.EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Maximized;
        }
        private void mainwindow_Load(object sender, EventArgs e)
        {
            ObjMainClass.CompactLocaldb();
            wcDownloadSplSongs.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wcDownloadSplSongs_DownloadProgressChanged);
            wcDownloadSplSongs.DownloadFileCompleted += new AsyncCompletedEventHandler(wcDownloadSplSongs_DownloadFileCompleted);


            myWebClientAdvt.DownloadProgressChanged += new DownloadProgressChangedEventHandler(myWebClientAdvt_DownloadProgressChanged);
            myWebClientAdvt.DownloadFileCompleted += new AsyncCompletedEventHandler(myWebClientAdvt_DownloadFileCompleted);


            wcDownloadRequestSong.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wcDownloadRequestSong_DownloadProgressChanged);
            wcDownloadRequestSong.DownloadFileCompleted += new AsyncCompletedEventHandler(wcDownloadRequestSong_DownloadFileCompleted);


            musicPlayer1.Dock = DockStyle.Fill;
            musicPlayer1.BringToFront();
            pnlMp3.Dock = DockStyle.Fill;

            button3.Visible = false;


            m_menu = new ContextMenu();

            m_menu.MenuItems.Add(0,
                new MenuItem("Exit", new System.EventHandler(Exit_Click)));
            m_notifyicon = new NotifyIcon();
            m_notifyicon.Text = "Right click for context menu";
            m_notifyicon.Visible = true;
            m_notifyicon.Icon = new System.Drawing.Icon(Application.StartupPath + @"\MyA.ico");
            m_notifyicon.ContextMenu = m_menu;
            m_notifyicon.ShowBalloonTip(10, "Now player is online", "Running", ToolTipIcon.Info);
            this.WindowState = FormWindowState.Maximized;










            timAdvt.Enabled = false;



            fi.AMDesignator = "AM";
            fi.PMDesignator = "PM";
            FirstTimeConditation = "Yes";
            UpdateLocalDatabase();




            this.Icon = Properties.Resources.more;
            string str = "";


            string MainDirectory = Application.StartupPath + "\\Advt";
            bool isExists = System.IO.Directory.Exists(MainDirectory);
            if (!isExists)
                System.IO.Directory.CreateDirectory(MainDirectory);

            DirectoryInfo DifA = new DirectoryInfo(MainDirectory);
            DifA.Attributes = FileAttributes.Hidden;

            MainDirectory = "";
            MainDirectory = Application.StartupPath + "\\so";
            isExists = System.IO.Directory.Exists(MainDirectory);
            if (!isExists)
                System.IO.Directory.CreateDirectory(MainDirectory);

            DirectoryInfo Dif = new DirectoryInfo(MainDirectory);
            Dif.Attributes = FileAttributes.Hidden;

            MainDirectory = "";
            MainDirectory = Application.StartupPath + "\\db.mdb";
            DirectoryInfo DifD = new DirectoryInfo(MainDirectory);
            DifD.Attributes = FileAttributes.Hidden;



            musicPlayer1.enableContextMenu = false;
            musicPlayer2.enableContextMenu = false;

            dgPlaylist.AllowDrop = true;
            dgOtherPlaylist.AllowDrop = true;
            //dgPlaylist.Dock = DockStyle.Fill;
            InitilizeGrid(dgPlaylist);
            musicPlayer1.uiMode = "none";
            musicPlayer2.uiMode = "none";
            AdvtPlayer.uiMode = "none";
            InitilizeHideGrid();

            FillLocalPlaylist();
            InitilizeMusicGrid(dgMusicPlayer1);
            InitilizeMusicGrid(dgMusicPlayer2);


            if (dgLocalPlaylist.Rows.Count > 0)
            {
                PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
            }
            DataSet ds = new DataSet();
            str = "select * from tbMusicLastSettings where tokenno=" + StaticClass.TokenId + "";
            ds = ObjMainClass.fnFillDataSet_Local(str);
            if (dgLocalPlaylist.Rows.Count > 0 && dgPlaylist.Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    PlaySongDefault();
                }
                else
                {
                    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                    Get_Last_Settings();
                }
                DisplaySongPlayerOne();
            }
            else
            {

            }
            RowHide();






            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                DataTable dtPlaylist = new DataTable();
                DataTable dtSong = new DataTable();
                string LocalSongFind = "No";
                string strNet = "select * from Playlists where tokenid= " + StaticClass.TokenId;
                dtPlaylist = ObjMainClass.fnFillDataTable_Local(strNet);
                if ((dtPlaylist.Rows.Count == 0))
                {
                    MessageBox.Show("No local songs are found, please connect to Internet.", "Player");
                    Application.Exit();
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
                    MessageBox.Show("No local songs are found, please connect to Internet.", "Player");
                    Application.Exit();
                }


                //panel2.Visible = false;
                //panPlaylist.Height = 550; tbpNewest300  tbpBestof


                // this.Text = "support.player@alenkamedia.com  +919619183373 or +919930535229";

                GetAdvtPlayingType();
                StaticClass.PlayerClosingTime = "";
                IsFormatFirstTimeLoad = "Yes";
                timResetSong.Enabled = false;
                timGetSplPlaylistScheduleTime.Enabled = true;
                //FillEvents();

            }
            else
            {


                this.Text = StaticClass.MainwindowMessage;


                GetAdvertisement();
                delete_temp_table();
                DeleteHideSongs();


                if (StaticClass.IsStore == false)
                {

                }
                else
                {
                    IsFormatFirstTimeLoad = "Yes";
                    timResetSong.Enabled = false;
                    timGetSplPlaylist.Enabled = true;
                }
                DataTable dtDetail = new DataTable();
                str = "spGetPrayerData " + DateTime.Now.Date.Month + " ," + StaticClass.AdvtCityId + "," + StaticClass.CountryId + ", " + StaticClass.Stateid + ", " + StaticClass.TokenId;
                dtDetail = ObjMainClass.fnFillDataTable(str);
                if ((dtDetail.Rows.Count > 0))
                {
                    str = "";
                    str = "delete from tbPrayer";
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    OleDbCommand cmdDel = new OleDbCommand();
                    cmdDel.Connection = StaticClass.LocalCon;
                    cmdDel.CommandText = str;
                    cmdDel.ExecuteNonQuery();
                    for (int iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {
                        str = "";
                        str = "insert into tbPrayer(pId,sDate,eDate,sTime,eTime) values(" + dtDetail.Rows[iCtr]["pId"] + ", #" + string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["sDate"]) + "# ,";
                        str = str + " #" + string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["eDate"]) + "# ,#" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["sTime"]) + "#, #" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["eTime"]) + "# )";
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdIns = new OleDbCommand();
                        cmdIns.Connection = StaticClass.LocalCon;
                        cmdIns.CommandText = str;
                        cmdIns.ExecuteNonQuery();
                    }

                }



            }

            FillMainAdvertisement();
            DownloadAdvt();
            PlayNowSQLDependency();



            SearchText = "";
            FillStar(dgSongRatingPlayerOne);
            SetRating(dgSongRatingPlayerOne);
            SetDisableRating(dgSongRatingPlayerTwo);

            if (StaticClass.IsStore == false)
            {
                if (FirstTimeSong == true)
                {
                    Song_Set_foucs();

                    if (musicPlayer1.URL.ToString() != "")
                    {
                        GetSavedRating(MusicPlayer1CurrentSongId.ToString(), dgSongRatingPlayerOne);
                    }
                    SetDisableRating(dgSongRatingPlayerTwo);
                }
            }







            FillPrayer(dgPrayer);

            MainDirectory = "";
            MainDirectory = Application.StartupPath + "\\db.ldb";
            DirectoryInfo DifDl = new DirectoryInfo(MainDirectory);
            DifDl.Attributes = FileAttributes.Hidden;
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
                cmd.CommandText = "delete from temp_song where tokenid= " + StaticClass.TokenId;
                cmd.ExecuteNonQuery();
                StaticClass.constr.Close();
            }
            catch
            {

            }
        }

        void delete_temp_data(string songid)
        {
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = StaticClass.constr;
            cmd.CommandText = "delete from temp_song where tempid=" + Convert.ToInt32(songid) + " and tokenid=" + StaticClass.TokenId;
            cmd.ExecuteNonQuery();
            StaticClass.constr.Close();
        }
        //void insert_temp_data(string songid)
        //{
        //    string filePath = "";
        //    try
        //    {
        //        string mlsSql = "select * from temp_song where tempSongid = " + songid + " and tokenid= " + StaticClass.TokenId + " ";
        //        DataSet ds = new DataSet();
        //        ds = ObjMainClass.fnFillDataSet(mlsSql);
        //        if (ds.Tables[0].Rows.Count != 0) return;
        //        filePath = Application.StartupPath + "\\so\\" + songid + ".mp4";
        //        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
        //        StaticClass.constr.Open();
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = StaticClass.constr;
        //        cmd.CommandText = "INSERT INTO temp_song(tempid, tempSongid,tokenid) VALUES(@param1,@param2,@param3)";
        //        cmd.Parameters.AddWithValue("@param1", songid);
        //        cmd.Parameters.AddWithValue("@param2", songid);
        //        cmd.Parameters.AddWithValue("@param3", StaticClass.TokenId);
        //        cmd.ExecuteNonQuery();
        //        StaticClass.constr.Close();

        //    }
        //    catch (Exception ex)
        //    {

        //        if (System.IO.File.Exists(filePath))
        //        {
        //            delete_temp_data(songid);
        //        }
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {

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

                        return;

                    }
                    else if (Index == -1)
                    {
                        Index = 1;
                        ResetPlaylist(dgPlaylist, Index, file);

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

                        return;

                    }
                    else if (Index == -1)
                    {
                        Index = 1;
                        ResetPlaylist(dgOtherPlaylist, Index, file);

                        return;
                    }
                    else if (Index != -1)
                    {
                        ResetPlaylist(dgOtherPlaylist, Index, file);

                    }
                }

            }

        }
        private void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {


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

        private void musicPlayer1_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            try
            {


                TimerEventProcessorPlayerTwo();

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
                Console.WriteLine(ex.Message);
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            musicPlayer1.Ctlcontrols.play();
            timer1.Enabled = false;
        }
        //private void PlaylistSave()
        //{

        //    Int32 Playlist_Id = 0;
        //    if (StaticClass.constr.State == ConnectionState.Open)
        //    {
        //        StaticClass.constr.Close();
        //    }

        //    StaticClass.constr.Open();
        //    SqlCommand cmd = new SqlCommand("InsertPlayListsNew", StaticClass.constr);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt));
        //    cmd.Parameters["@UserID"].Value = StaticClass.dfClientId;

        //    cmd.Parameters.Add(new SqlParameter("@IsPredefined", SqlDbType.Bit));
        //    cmd.Parameters["@IsPredefined"].Value = 0;

        //    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
        //    cmd.Parameters["@Name"].Value = txtPlaylistName.Text;

        //    cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.VarChar, 50));
        //    cmd.Parameters["@Summary"].Value = " ";

        //    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 50));
        //    cmd.Parameters["@Description"].Value = " ";
        //    cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
        //    cmd.Parameters["@TokenId"].Value = StaticClass.TokenId;

        //    try
        //    {
        //        Playlist_Id = Convert.ToInt32(cmd.ExecuteScalar());
        //        ModifyPlaylistId = Playlist_Id;
        //        string sQr = "";

        //        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
        //        sQr = "insert into PlayLists values(" + Convert.ToInt32(Playlist_Id) + ", ";
        //        sQr = sQr + StaticClass.dfClientId + " , '" + txtPlaylistName.Text + "', " + StaticClass.TokenId + ",'' ,0)";
        //        OleDbCommand cmdSaveLocal = new OleDbCommand();
        //        cmdSaveLocal.Connection = StaticClass.LocalCon;
        //        cmdSaveLocal.CommandText = sQr;
        //        cmdSaveLocal.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        StaticClass.constr.Close();
        //    }
        //}

        //private void PlaylistModify()
        //{
        //    if (StaticClass.constr.State == ConnectionState.Open)
        //    {
        //        StaticClass.constr.Close();
        //    }

        //    StaticClass.constr.Open();
        //    SqlCommand cmd = new SqlCommand("UpdateUserPlayLists", StaticClass.constr);
        //    cmd.CommandType = CommandType.StoredProcedure;

        //    cmd.Parameters.Add(new SqlParameter("@PlayListID", SqlDbType.BigInt));
        //    cmd.Parameters["@PlayListID"].Value = ModifyPlaylistId;

        //    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
        //    cmd.Parameters["@Name"].Value = txtPlaylistName.Text;
        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        string sQr = "";
        //        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
        //        sQr = "update PlayLists set Name= '" + txtPlaylistName.Text + "' ";
        //        sQr = sQr + " where PlayListID= " + Convert.ToInt32(ModifyPlaylistId);
        //        OleDbCommand cmdSaveLocal = new OleDbCommand();
        //        cmdSaveLocal.Connection = StaticClass.LocalCon;
        //        cmdSaveLocal.CommandText = sQr;
        //        cmdSaveLocal.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    finally
        //    {
        //        StaticClass.constr.Close();
        //    }
        //}



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
                    dsTitle = ObjMainClass.fnFillDataSet(sQr);
                    AlbumID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["AlbumID"]);
                    ArtistID = Convert.ToInt32(dsTitle.Tables[0].Rows[0]["ArtistID"]);
                    Special_Name = dsTitle.Tables[0].Rows[0]["Title"].ToString();
                    Special_Change = Special_Name.Replace("'", "??$$$??");
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    sWr = "insert into Titles values (" + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["TitleID"]) + " , " + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["AlbumID"]) + " , ";
                    sWr = sWr + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["ArtistID"]) + ", '" + Special_Change + "' , ";
                    sWr = sWr + "'" + dsTitle.Tables[0].Rows[0]["Gain"] + "' , '" + dsTitle.Tables[0].Rows[0]["Time"] + "' ,";
                    sWr = sWr + Convert.ToInt32(dsTitle.Tables[0].Rows[0]["TitleYear"]) + ")";
                    OleDbCommand cmdTitle = new OleDbCommand();
                    cmdTitle.Connection = StaticClass.LocalCon;
                    cmdTitle.CommandText = sWr;
                    cmdTitle.ExecuteNonQuery();
                }
                else
                {
                    sQr = "select TitleID,AlbumID,ArtistID,Title,Gain,isnull(TitleYear,0) as TitleYear,Time from Titles where TitleID=" + song_id;
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

                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    sWr = "insert into Albums values (" + Convert.ToInt32(dsAlbum.Tables[0].Rows[0]["AlbumID"]) + " , ";
                    sWr = sWr + Convert.ToInt32(dsAlbum.Tables[0].Rows[0]["ArtistID"]) + ", '" + Special_Change + "' ) ";

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
                    dsArtist = ObjMainClass.fnFillDataSet(sQr);
                    Special_Name = dsArtist.Tables[0].Rows[0]["Name"].ToString();
                    Special_Change = Special_Name.Replace("'", "??$$$??");
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    sWr = "insert into Artists values (" + Convert.ToInt32(dsArtist.Tables[0].Rows[0]["ArtistID"]) + ", '" + Special_Change + "' ) ";

                    OleDbCommand cmdAlbum = new OleDbCommand();
                    cmdAlbum.Connection = StaticClass.LocalCon;
                    cmdAlbum.CommandText = sWr;
                    cmdAlbum.ExecuteNonQuery();
                }




                //(Convert.ToInt32(ds.Tables[0].Rows[0]["lastPlaylistId"]));


                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    sWr = "insert into TitlesInPlaylists(PlaylistID,TitleID,SrNo) values (" + Playlist_Id + " , " + Convert.ToInt32(song_id) + ", " + Convert.ToInt32(dgPlaylist.Rows.Count - 1) + ")";
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = StaticClass.LocalCon;
                    cmd.CommandText = sWr;
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    sWr = "insert into TitlesInPlaylists(PlaylistID,TitleID,SrNo) values (" + Playlist_Id + " , " + Convert.ToInt32(song_id) + ", " + Convert.ToInt32(dgOtherPlaylist.Rows.Count - 1) + ")";
                    OleDbCommand cmd = new OleDbCommand();
                    cmd.Connection = StaticClass.LocalCon;
                    cmd.CommandText = sWr;
                    cmd.ExecuteNonQuery();
                }
                // 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                        //PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                        AddSongsInGrid(dgPlaylist, Convert.ToInt32(songid));
                    }
                    else
                    {
                        //PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                        AddSongsInGrid(dgOtherPlaylist, Convert.ToInt32(songid));
                    }

                }
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    rCount = 0;
                    //DropSongLength = "";
                    //IsSongDropAdvt = false;
                    label7.Text = "0";
                    label8.Text = "0";
                    label18.Text = "0";
                    IsAdvtTimeGet = false;
                    GrossTotaltime = 0;
                    // timGetRemainAdvtTime.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                // throw new ApplicationException("Data error.");
            }
            finally
            {
                //                StaticClass.constr.Close();
            }


        }
        private void PlaySongDefault()
        {
            try
            {
                string MusicFileName = "";
                string TempFileName = "";
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    MusicFileName = dgPlaylist.Rows[i].Cells["lUrl"].Value.ToString();
                    TempFileName = MusicFileName;
                    if (System.IO.File.Exists(TempFileName))
                    {

                        musicPlayer1.URL = MusicFileName;
                        MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value);
                        DataTable dtDetail = new DataTable();


                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }



                        CurrentRow = i;

                        CurrentPlaylistRow = dgLocalPlaylist.CurrentCell.RowIndex;
                        var k = Path.GetExtension(MusicFileName);
                        if (k == ".mp3")
                        {
                            lblTitle.Text = dgPlaylist.Rows[CurrentRow].Cells["songname"].Value.ToString();
                            lblArtist.Text = dgPlaylist.Rows[CurrentRow].Cells["artist"].Value.ToString();

                            pnlMp3.Visible = true;
                            pnlMp3.BringToFront();

                            picMp3Logo.Location = new Point(
                      this.pnlLogo.Width / 2 - picMp3Logo.Size.Width / 2,
                      this.pnlLogo.Height / 2 - picMp3Logo.Size.Height / 2);
                        }
                        else
                        {
                            pnlMp3.Visible = false;
                        }



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
            catch (Exception ex)
            {
                // MessageBox.Show("tt" + ex.Message);

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
            lblCurrentTiming.Text = a.ToString();

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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lUrl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer2.URL = MusicFileName;
                    MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer2.URL = MusicFileName;
                    MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer2.URL = MusicFileName;
                    MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
            TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
            MusicFileName = TempMusicFileName;

            if (System.IO.File.Exists(TempMusicFileName))
            {

                musicPlayer2.URL = MusicFileName;
                MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[i].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;

                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer2.URL = MusicFileName;
                    MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value);
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
                        dgLocalPlaylist.Rows[e.RowIndex].Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                        dgLocalPlaylist.Rows[e.RowIndex].Cells[1].Style.SelectionForeColor = Color.Yellow;

                        dgLocalPlaylist.Rows[e.RowIndex].Cells[3].Style.SelectionBackColor = Color.LightBlue;

                        dgPlaylist.Visible = true;

                        dgOtherPlaylist.Visible = false;
                        if (StaticClass.IsStore == true)
                        {
                            PopulateSplPlaylist(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[e.RowIndex].Cells[0].Value));
                        }
                        else
                        {
                            PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[e.RowIndex].Cells[0].Value));
                        }
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
        private void Music_Player_Settings()
        {
            try
            {
                string str = "";
                string Song_Name = "";
                string GetName = "";
                double LastSongDuration = 0;
                Int32 LastPlayId = 0;
                DataTable dtDetail;
                string mlsSql = "";
                if (dgLocalPlaylist.Rows.Count == 0) return;
                if (dgPlaylist.Rows.Count == 0) return;


                if (musicPlayer1.URL != "")
                {
                    Song_Name = MusicPlayer1CurrentSongId.ToString();
                }
                else if (musicPlayer2.URL != "")
                {
                    Song_Name = MusicPlayer2CurrentSongId.ToString();
                }
                for (int i = 0; i < dgLocalPlaylist.Rows.Count; i++)
                {
                    mlsSql = "SELECT  Titles.TitleID, Titles.Title FROM TitlesInPlaylists INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID where TitlesInPlaylists.PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value);
                    dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                    if ((dtDetail.Rows.Count > 0))
                    {
                        for (int iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                        {
                            if (dtDetail.Rows[iCtr]["TitleID"].ToString() == Song_Name)
                            {
                                LastPlayId = Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value);
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

                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                str = "delete from tbMusicLastSettings where tokenNo=" + StaticClass.TokenId;

                OleDbCommand cmdDel = new OleDbCommand();
                cmdDel.Connection = StaticClass.LocalCon;
                cmdDel.CommandText = str;
                cmdDel.ExecuteNonQuery();
                string sQr = "";
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                sQr = "insert into tbMusicLastSettings(DFClientId,localUserId,lastPlaylistId,lastTileId,lastVolume,lastSongDuration,IsFade,IsShuffle,TokenNo) values(" + StaticClass.dfClientId + ", ";
                sQr = sQr + "0 , " + LastPlayId + ", " + Song_Name + ",0,0,0,0, " + StaticClass.TokenId + ")";
                OleDbCommand cmdSaveLocal = new OleDbCommand();
                cmdSaveLocal.Connection = StaticClass.LocalCon;
                cmdSaveLocal.CommandText = sQr;
                cmdSaveLocal.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Get_Last_Settings()
        {
            string tempSongName = "";
            try
            {
                string str = "";
                string SongName = "";
                str = "select * from tbMusicLastSettings where tokenNo=" + StaticClass.TokenId;
                DataSet ds = new DataSet();
                ds = ObjMainClass.fnFillDataSet_Local(str);

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
                        if (dgPlaylist.Rows.Count == 1)
                        {
                            tempRow = 0;
                        }
                        else if (tempRow >= dgPlaylist.Rows.Count)
                        {
                            tempRow = 1;
                        }
                        tempSongName = dgPlaylist.Rows[tempRow].Cells["lurl"].Value.ToString();
                        if (System.IO.File.Exists(tempSongName))
                        {
                            SongName = dgPlaylist.Rows[tempRow].Cells["lurl"].Value.ToString();

                            musicPlayer1.URL = SongName;
                            MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[tempRow].Cells[0].Value);
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
                if (StaticClass.IsStore == false)
                {
                    if (ObjMainClass.CheckForInternetConnection() == true)
                    {
                        DataTable dtDetail = new DataTable();
                        string str007 = "select * from tbLastPosition where tokenid= " + StaticClass.TokenId;
                        dtDetail = ObjMainClass.fnFillDataTable_Local(str007);
                        if ((dtDetail.Rows.Count > 0))
                        {
                            if (dtDetail.Rows[0]["LastPostion"].ToString() == "Stream")
                            {
                                musicPlayer1.settings.volume = 0;
                            }
                            else
                            {
                                if (StaticClass.IsVideoMute == "0")
                                {
                                    musicPlayer1.settings.volume = 100;
                                }
                                else
                                {
                                    musicPlayer1.settings.volume = 0;
                                }
                            }
                        }
                        else
                        {
                            if (StaticClass.IsVideoMute == "0")
                            {
                                musicPlayer1.settings.volume = 100;
                            }
                            else
                            {
                                musicPlayer1.settings.volume = 0;
                            }
                        }
                    }
                    else
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
                }
                else
                {
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 100;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }
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
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer1CurrentSongId.ToString())
                    {
                        CurrentRow = i;

                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            IsVisibleSong = true;

                            UpdateHideSong(MusicPlayer1CurrentSongId.ToString());
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
                        //panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));
                        pbarMusic1.BackColor = Color.FromArgb(9, 130, 154);

                        lblSongName2.ForeColor = Color.Gray;
                        lblArtistName2.ForeColor = Color.Gray;
                        lblMusicTimeTwo.ForeColor = Color.Gray;
                        lblSongDurationTwo.ForeColor = Color.Gray;
                        pbarMusic2.ForeColor = Color.Gray;
                        pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
                        //panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
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
        private void Set_foucs_PayerOne()
        {
            try
            {
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer1CurrentSongId.ToString())
                    {

                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            UpdateHideSong(MusicPlayer1CurrentSongId.ToString());
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
                        //panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));
                        pbarMusic1.BackColor = Color.FromArgb(9, 130, 154);

                        lblSongName2.ForeColor = Color.Gray;
                        lblArtistName2.ForeColor = Color.Gray;
                        lblMusicTimeTwo.ForeColor = Color.Gray;
                        lblSongDurationTwo.ForeColor = Color.Gray;
                        pbarMusic2.ForeColor = Color.Gray;
                        pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
                        //panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));

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
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer2CurrentSongId.ToString())
                    {
                        CurrentRow = i;
                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            IsVisibleSong = true;

                            UpdateHideSong(MusicPlayer2CurrentSongId.ToString());
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
                        //panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));

                        lblSongName.ForeColor = Color.Gray;
                        lblArtistName.ForeColor = Color.Gray;
                        lblMusicTimeOne.ForeColor = Color.Gray;
                        lblSongDurationOne.ForeColor = Color.Gray;
                        pbarMusic1.ForeColor = Color.Gray;
                        pbarMusic1.BackColor = Color.FromArgb(175, 175, 175);
                        //panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
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

        private void Set_foucs_PayerTwo()
        {
            try
            {
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer2CurrentSongId.ToString())
                    {
                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            UpdateHideSong(MusicPlayer2CurrentSongId.ToString());
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
                        //panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));

                        lblSongName.ForeColor = Color.Gray;
                        lblArtistName.ForeColor = Color.Gray;
                        lblMusicTimeOne.ForeColor = Color.Gray;
                        lblSongDurationOne.ForeColor = Color.Gray;
                        pbarMusic1.ForeColor = Color.Gray;
                        pbarMusic1.BackColor = Color.FromArgb(175, 175, 175);
                        //panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                        if (dgHideSongs.Rows.Count > 0)
                        {
                            DeleteParticularHideSong();
                        }
                    }
                }
            }
            catch { }
        }








        private void btnFade_Click(object sender, EventArgs e)
        {
            try
            {
                if (musicPlayer1.URL == "" && musicPlayer2.URL == "" && dgPlaylist.Rows.Count == 0)
                {

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





                    PlaylistFadeSongPlayerOne();

                    timAutoFadePlayerOne.Enabled = false;
                    timAutoFadePlayerTwo.Enabled = false;
                    timer5.Enabled = true;
                    return;
                }
                if (musicPlayer2.URL == "")
                {
                    IsbtnClick = "Y";




                    PlaylistFadeSong();

                    timAutoFadePlayerOne.Enabled = false;
                    timAutoFadePlayerTwo.Enabled = false;
                    timer4.Enabled = true;
                    return;

                }
            }
            catch (Exception ex)
            {





                Console.WriteLine(ex.Message);
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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer1.URL = MusicFileName;
                    MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;

                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer1.URL = MusicFileName;
                    MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer1.URL = MusicFileName;
                    MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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

            TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
            MusicFileName = TempMusicFileName;
            if (System.IO.File.Exists(TempMusicFileName))
            {

                musicPlayer1.URL = MusicFileName;
                MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[i].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;

                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer1.URL = MusicFileName;
                    MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value);
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
                Console.WriteLine(ex.Message);
            }

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            try
            {


                double a = Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);
                lblCurrentTiming.Text = a.ToString();
                double t = Math.Floor(musicPlayer2.currentMedia.duration - musicPlayer2.Ctlcontrols.currentPosition);
                // TimePlayerOne = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);

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
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 75;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
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
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer2.settings.volume = 25;
                    }
                    else
                    {
                        musicPlayer2.settings.volume = 0;
                    }
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
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer2.settings.volume = 50;
                    }
                    else
                    {
                        musicPlayer2.settings.volume = 0;
                    }
                }
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 50;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
                }
                GetSavedRating(MusicPlayer2CurrentSongId.ToString(), dgSongRatingPlayerTwo);
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
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer2.settings.volume = 75;
                    }
                    else
                    {
                        musicPlayer2.settings.volume = 0;
                    }
                }
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 25;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
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
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 85;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
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

                        if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 15;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
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
                    timGetAdvtTime.Enabled = false;
                    lblPlayerName.Text = "One";
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        musicPlayer2.settings.volume = 0;
                        musicPlayer1.Ctlcontrols.pause();
                        musicPlayer2.Ctlcontrols.pause();
                    }
                    if (StaticClass.IsAdvtBetweenTime == true)
                    {
                        musicPlayer2.settings.volume = 0;
                    }

                    if (StaticClass.IsPlayerClose == "No")
                    {
                        // this.Show();
                        //this.WindowState = FormWindowState.Minimized;
                        // musicPlayer2.URL = "";
                        IsbtnClick = "N";
                        timer4.Enabled = false;
                        timer5.Enabled = false;
                        timAutoFadePlayerOne.Enabled = true;
                        timAutoFadePlayerTwo.Enabled = true;

                        FillPanAdvt();

                    }
                }
                else if (btnMute.Text == "")
                {
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer2.settings.volume = 100;
                    }
                    else
                    {
                        musicPlayer2.settings.volume = 0;
                    }
                }



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
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
                }





            }
            else if (Convert.ToInt32(lblCurrentTiming.Text) >= 14)
            {

            }
        }

        private void GetNextSong(string RunningPlayer)
        {
            string currentFileName;
            if (RunningPlayer == "1")
            {
                currentFileName = MusicPlayer2CurrentSongId.ToString();
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
                currentFileName = MusicPlayer1CurrentSongId.ToString();
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
                //TimePlayerTwo =  Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);

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
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 75;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
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
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 25;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
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
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 50;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 50;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
                }
                GetSavedRating(MusicPlayer1CurrentSongId.ToString(), dgSongRatingPlayerOne);
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
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 75;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 25;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
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
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 85;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
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

                        if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 15;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
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
                    timGetAdvtTime.Enabled = false;
                    lblPlayerName.Text = "Two";


                    if (StaticClass.IsPlayerClose == "No")
                    {
                        // this.Show();
                        //this.WindowState = FormWindowState.Minimized;
                        // musicPlayer1.URL = "";
                        IsbtnClick = "N";
                        timer4.Enabled = false;
                        timer5.Enabled = false;
                        timAutoFadePlayerOne.Enabled = true;
                        timAutoFadePlayerTwo.Enabled = true;

                        FillPanAdvt();
                    }

                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        musicPlayer1.settings.volume = 0;
                        musicPlayer2.Ctlcontrols.pause();
                        musicPlayer1.Ctlcontrols.pause();
                    }
                    if (StaticClass.IsAdvtBetweenTime == true)
                    {
                        musicPlayer1.settings.volume = 0;
                    }

                }
                else if (btnMute.Text == "")
                {
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 100;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }
                IsbtnClick = "N";



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
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
                }




            }
            else if (Convert.ToInt32(lblCurrentTimingPlayerOne.Text) >= 14)
            {

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
                Console.WriteLine(ex.Message);
            }
        }




        private void timAutoFadePlayerTwo_Tick(object sender, EventArgs e)
        {
            try
            {
                //drawLine = false;
                //dgPlaylist.Invalidate();
                double t = Math.Floor(musicPlayer1.currentMedia.duration - musicPlayer1.Ctlcontrols.currentPosition);
                TimePlayerOne = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);

                lblMusicTimeremaingPlayerOne.Text = (t.ToString());
                PlayAutoFadeSongPlayerTwo();
                textBox1.Focus();
                textBox1.SendToBack();
                //button3.Visible = true;
                //dgPlaylist.BringToFront();

                if (lblSongCount.Text != "2")
                {
                    //musicPlayer1.fullScreen = true;
                    //Roop
                    if ((musicPlayer1.URL != "") && (musicPlayer2.URL == ""))
                    {
                        musicPlayer1.Dock = DockStyle.Fill;
                        if ((dgPlaylist.Rows.Count > 0) && musicPlayer1.URL != "")
                        {
                            var k = Path.GetExtension(musicPlayer1.URL.ToString());
                            if (k != ".mp3")
                            {
                                musicPlayer1.BringToFront();
                            }
                        }
                        musicPlayer2.SendToBack();
                        AdvtPlayer.SendToBack();
                        musicPlayer2.Dock = DockStyle.None;
                        AdvtPlayer.Dock = DockStyle.None;

                        lblExit.Size = new Size(20, 20);
                        lblExit.Location = new Point(this.Width - lblExit.Width, 0);
                        lblExit.BringToFront();
                        lblExit.Visible = false;
                    }
                }
                //button3.BringToFront();
            }
            catch (Exception ex)
            {
                var h = ex.Message.ToString();
            }
        }
        int imgTwo = 0;
        private void PlayAutoFadeSongPlayerTwo()
        {
            if (musicPlayer2.URL != "")
            {
                var kTwo = Path.GetExtension(musicPlayer2.URL.ToString());
                if (kTwo == ".jpg")
                {
                    if (imgTwo >= 10)
                    {
                        imgTwo = 0;
                        musicPlayer2.Ctlcontrols.play();
                    }
                    imgTwo++;
                }
            }
            if ((lblMusicTimeremaingPlayerOne.Text == "-1") || (lblMusicTimeremaingPlayerOne.Text == "20"))
            {
                SaveLast100();
            }
           else if ((lblMusicTimeremaingPlayerOne.Text == "-2") || (lblMusicTimeremaingPlayerOne.Text == "5"))
            {
                PlayAutoFadingSongPlayerTwo();
            }
            else if ((lblMusicTimeremaingPlayerOne.Text == "-3") || (lblMusicTimeremaingPlayerOne.Text == "4"))
            {
                if ((dgPlaylist.Rows.Count > 0) && musicPlayer2.URL != "")
                {
                    var k = Path.GetExtension(musicPlayer2.URL.ToString());
                    if (k == ".jpg")
                    {
                        musicPlayer2.Ctlcontrols.pause();
                    }

                }

            }
            else if (lblMusicTimeremaingPlayerOne.Text == "-4")
            {

            }
            else if ((lblMusicTimeremaingPlayerOne.Text == "-5") || (lblMusicTimeremaingPlayerOne.Text == "2"))
            {
                //musicPlayer2.Ctlcontrols.play();
                musicPlayer1.settings.volume = 0;
                musicPlayer1.Ctlcontrols.stop();
                musicPlayer1.URL = "";
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;

                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    timGetAdvtTime.Enabled = false;
                    lblPlayerName.Text = "One";


                    if (StaticClass.IsPlayerClose == "No")
                    {
                        // this.Show();
                        //this.WindowState = FormWindowState.Minimized;
                        FillPanAdvt();
                    }
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        musicPlayer2.settings.volume = 0;
                        musicPlayer1.Ctlcontrols.pause();
                        musicPlayer2.Ctlcontrols.pause();
                    }
                    if (StaticClass.IsAdvtBetweenTime == true)
                    {
                        musicPlayer2.settings.volume = 0;
                    }
                }
                else
                {
                    if (btnMute.Text == "")
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }

                }
            }
            if ((dgPlaylist.Rows.Count > 0) && musicPlayer1.URL!="")
            {
               
                var l = Path.GetExtension(musicPlayer1.URL.ToString());
                if (l == ".mp3")
                {
                    lblTitle.Text = dgPlaylist.Rows[CurrentRow].Cells["songname"].Value.ToString();
                    lblArtist.Text = dgPlaylist.Rows[CurrentRow].Cells["artist"].Value.ToString();
                    pnlMp3.Visible = true;
                    pnlMp3.BringToFront();
                    picMp3Logo.Location = new Point(
              this.pnlLogo.Width / 2 - picMp3Logo.Size.Width / 2,
              this.pnlLogo.Height / 2 - picMp3Logo.Size.Height / 2);
                }
                else
                {
                    pnlMp3.Visible = false;
                }
            }

            return;
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
                // TimePlayerOne = TimePlayerOne + Math.Floor(musicPlayer1.currentMedia.duration);
                SaveLast100();
                if (lblSongCount.Text == "2")
                {
                    timGetRemainAdvtTime.Enabled = false;
                    lblAdvtTimeRemain.Text = "00:13";
                }
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




                picFade.Location = new Point(6, 45);
                picFade.Visible = true;
                btnFade.Visible = false;

                PlayAutoFadingSongPlayerTwo();

                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.mute = false;
                    musicPlayer1.settings.mute = false;
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 75;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
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
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 25;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
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
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 50;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:03";
                    timGetRemainAdvtTime.Enabled = false;
                    musicPlayer2.settings.volume = 0;
                }
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 50;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
                }

                GetSavedRating(MusicPlayer2CurrentSongId.ToString(), dgSongRatingPlayerTwo);
                SetDisableRating(dgSongRatingPlayerOne);
                Song_Set_foucs2();
            }

            else if (lblMusicTimeremaingPlayerOne.Text == "8")
            {
                label1.Text = "Player Two----6";
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 25;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:02";
                    timGetRemainAdvtTime.Enabled = false;
                    musicPlayer2.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 75;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
                }
            }
            else if (lblMusicTimeremaingPlayerOne.Text == "6")
            {
                label1.Text = "Player Two----6";
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 25;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
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
                        if (StaticClass.IsAdvtBetweenTime == false)
                        {
                            if (StaticClass.IsVideoMute == "0")
                            {
                                musicPlayer2.settings.volume = 85;
                            }
                            else
                            {
                                musicPlayer2.settings.volume = 0;
                            }
                        }
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
                        if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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
            }

            else if (lblMusicTimeremaingPlayerOne.Text == "5")
            {
                label1.Text = "Player Two----2";
                

                musicPlayer1.settings.volume = 0;
                musicPlayer1.Ctlcontrols.stop();
                musicPlayer1.URL = "";
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;

                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    timGetAdvtTime.Enabled = false;
                    lblPlayerName.Text = "One";


                    if (StaticClass.IsPlayerClose == "No")
                    {
                        // this.Show();
                        //this.WindowState = FormWindowState.Minimized;
                        FillPanAdvt();
                    }
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        musicPlayer2.settings.volume = 0;
                        musicPlayer1.Ctlcontrols.pause();
                        musicPlayer2.Ctlcontrols.pause();
                    }
                    if (StaticClass.IsAdvtBetweenTime == true)
                    {
                        musicPlayer2.settings.volume = 0;
                    }
                }
                else
                {
                    if (btnMute.Text == "")
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }

                }


            }
            else if (lblMusicTimeremaingPlayerOne.Text == "2")
            {
                label1.Text = "Player Two----2";

                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;




                musicPlayer1.settings.volume = 0;
                musicPlayer1.Ctlcontrols.stop();
                musicPlayer1.URL = "";
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
                }
                Song_Set_foucs2();


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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer2.URL = MusicFileName;
                    MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer2.URL = MusicFileName;
                    MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer2.URL = MusicFileName;
                    MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
            TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
            MusicFileName = TempMusicFileName;
            if (System.IO.File.Exists(TempMusicFileName))
            {

                musicPlayer2.URL = MusicFileName;
                MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
                musicPlayer2.settings.volume = 0;
                
                timer2.Enabled = true;
                return;
            }
            for (int i = Convert.ToInt16(CurrentRow); i < dgPlaylist.Rows.Count; i++)
            {
                TempMusicFileName = dgPlaylist.Rows[i].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer2.URL = MusicFileName;
                    MusicPlayer2CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value);
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
                textBox1.Focus();
                textBox1.SendToBack();
                //button3.Visible = true;
                //button3.BringToFront();
                //dgPlaylist.BringToFront();
                PlayAutoFadeSongPlayerOne();
                 
                if (lblSongCount.Text != "2")
                {
                    // musicPlayer2.fullScreen = true;
                    //Roop
                    if ((musicPlayer2.URL != "") && (musicPlayer1.URL == ""))
                    {
                        musicPlayer2.Dock = DockStyle.Fill;
                        if ((dgPlaylist.Rows.Count > 0) && musicPlayer2.URL != "")
                        {
                            var k = Path.GetExtension(musicPlayer2.URL.ToString());
                            if (k != ".mp3")
                            {
                                musicPlayer2.BringToFront();
                            }
                             
                        }
                        musicPlayer1.SendToBack();
                        AdvtPlayer.SendToBack();
                        musicPlayer1.Dock = DockStyle.None;
                        AdvtPlayer.Dock = DockStyle.None;

                        lblExit.Size = new Size(20, 20);
                        lblExit.Location = new Point(this.Width - lblExit.Width, 0);
                        lblExit.BringToFront();
                        lblExit.Visible = false;

                    }
                }
                //button3.BringToFront();
            }

            catch (Exception ex)
            {
                var h = ex.Message.ToString();
            }
        }
        int imgOne = 0;
        private void PlayAutoFadeSongPlayerOne()
        {
            if (musicPlayer1.URL != "")
            {
                var kOne = Path.GetExtension(musicPlayer1.URL.ToString());
                if (kOne == ".jpg")
                {
                    if (imgOne >= 10)
                    {
                       imgOne = 0;
                       musicPlayer1.Ctlcontrols.play();
                    }
                    imgOne++;
                }
            }
            if ((lblMusicTimeremaingPlayerTwo.Text == "-1") || (lblMusicTimeremaingPlayerTwo.Text == "20"))
            {
                SaveLast100();
            }
            else if ((lblMusicTimeremaingPlayerTwo.Text == "-2") || (lblMusicTimeremaingPlayerTwo.Text == "5"))
            {
                PlayAutoFadingSongPlayerOne();
            }
            else if ((lblMusicTimeremaingPlayerTwo.Text == "-3") || (lblMusicTimeremaingPlayerTwo.Text == "4"))
            {
                if ((dgPlaylist.Rows.Count > 0) && musicPlayer1.URL != "")
                {
                    var k = Path.GetExtension(musicPlayer1.URL.ToString());
                    if (k == ".jpg")
                    {
                        musicPlayer1.Ctlcontrols.pause();
                    }

                }

            }
            else if (lblMusicTimeremaingPlayerTwo.Text == "-4")
            {

            }
            else if ((lblMusicTimeremaingPlayerTwo.Text == "-5") || (lblMusicTimeremaingPlayerTwo.Text == "2"))
            {
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;

                //musicPlayer1.Ctlcontrols.play();
                musicPlayer2.settings.volume = 0;
                musicPlayer2.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    timGetAdvtTime.Enabled = false;
                    lblPlayerName.Text = "Two";

                    if (StaticClass.IsPlayerClose == "No")
                    {
                        // this.Show();
                        //this.WindowState = FormWindowState.Minimized;
                        FillPanAdvt();
                    }


                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        musicPlayer1.settings.volume = 0;
                        musicPlayer2.Ctlcontrols.pause();
                        musicPlayer1.Ctlcontrols.pause();
                    }
                    if (StaticClass.IsAdvtBetweenTime == true)
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 100;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }
            }
            if ((dgPlaylist.Rows.Count > 0) && musicPlayer2.URL != "")
            {
                var k = Path.GetExtension(musicPlayer2.URL.ToString());
                if (k == ".mp3")
                {
                    lblTitle.Text = dgPlaylist.Rows[CurrentRow].Cells["songname"].Value.ToString();
                    lblArtist.Text = dgPlaylist.Rows[CurrentRow].Cells["artist"].Value.ToString();

                    pnlMp3.Visible = true;
                    pnlMp3.BringToFront();

                    picMp3Logo.Location = new Point(
              this.pnlLogo.Width / 2 - picMp3Logo.Size.Width / 2,
              this.pnlLogo.Height / 2 - picMp3Logo.Size.Height / 2);
                }
                else
                {
                    pnlMp3.Visible = false;
                }
            }
            return;
            if ((Convert.ToInt32(lblMusicTimeremaingPlayerTwo.Text) <= 60) && (Convert.ToInt32(lblMusicTimeremaingPlayerTwo.Text) >= 21))
            {
                if (lblSongCount.Text == "2")
                {
                    timGetRemainAdvtTime.Enabled = false;
                    lblAdvtTimeRemain.Text = "00:" + lblMusicTimeremaingPlayerTwo.Text;
                }
            }



            else if (lblMusicTimeremaingPlayerTwo.Text == "20")
            {
                // TimePlayerTwo = TimePlayerTwo + Math.Floor(musicPlayer2.currentMedia.duration);
                SaveLast100();
                if (lblSongCount.Text == "2")
                {
                    timGetRemainAdvtTime.Enabled = false;
                    lblAdvtTimeRemain.Text = "00:13";
                }
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




                picFade.Location = new Point(6, 45);
                picFade.Visible = true;
                btnFade.Visible = false;

                PlayAutoFadingSongPlayerOne();


                if (btnMute.Text == "")
                {
                    musicPlayer2.settings.mute = false;
                    musicPlayer1.settings.mute = false;
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 75;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
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
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 25;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
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
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 50;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:03";
                    timGetRemainAdvtTime.Enabled = false;
                    musicPlayer1.settings.volume = 0;
                }
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 50;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }

                GetSavedRating(MusicPlayer1CurrentSongId.ToString(), dgSongRatingPlayerOne);
                SetDisableRating(dgSongRatingPlayerTwo);
                Song_Set_foucs();
            }

            else if (lblMusicTimeremaingPlayerTwo.Text == "8")
            {
                label6.Text = "Player One----6";
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 25;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:02";
                    timGetRemainAdvtTime.Enabled = false;
                    musicPlayer1.settings.volume = 0;
                }
                else if (btnMute.Text == "")
                {
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 75;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }
            }

            else if (lblMusicTimeremaingPlayerTwo.Text == "6")
            {
                label6.Text = "Player One----6";
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {

                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 25;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
                }
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    timGetRemainAdvtTime.Enabled = false;
                    musicPlayer1.settings.volume = 0;
                }
                else
                {
                    if (btnMute.Text == "")
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 85;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
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
                        if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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

            else if (lblMusicTimeremaingPlayerTwo.Text == "5")
            {
                label6.Text = "Player One----5";





                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;


                musicPlayer2.settings.volume = 0;
                musicPlayer2.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                if (lblSongCount.Text == "2")
                {
                    lblAdvtTimeRemain.Text = "00:00";
                    timGetAdvtTime.Enabled = false;
                    lblPlayerName.Text = "Two";

                    if (StaticClass.IsPlayerClose == "No")
                    {
                        // this.Show();
                        //this.WindowState = FormWindowState.Minimized;
                        FillPanAdvt();
                    }


                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        musicPlayer1.settings.volume = 0;
                        musicPlayer2.Ctlcontrols.pause();
                        musicPlayer1.Ctlcontrols.pause();
                    }
                    if (StaticClass.IsAdvtBetweenTime == true)
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 100;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
                }

            }

            else if (lblMusicTimeremaingPlayerTwo.Text == "2")
            {

                label6.Text = "Player One----2";
                btnFade.Location = new Point(6, 45);
                btnFade.Visible = true;
                picFade.Visible = false;





                musicPlayer2.settings.volume = 0;
                musicPlayer2.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                if (btnMute.Text == "")
                {
                    if (StaticClass.IsAdvtBetweenTime == false)
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
                }
                Song_Set_foucs();

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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer1.URL = MusicFileName;
                    MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer1.URL = MusicFileName;
                    MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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
                TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer1.URL = MusicFileName;
                    MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
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

            TempMusicFileName = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["lurl"].Value.ToString();
            MusicFileName = TempMusicFileName;
            if (System.IO.File.Exists(TempMusicFileName))
            {

                musicPlayer1.URL = MusicFileName;
                MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
                musicPlayer1.settings.volume = 0;
                

                timer1.Enabled = true;
                return;
            }
            for (int i = Convert.ToInt16(CurrentRow); i < dgPlaylist.Rows.Count; i++)
            {
                TempMusicFileName = dgPlaylist.Rows[i].Cells["lurl"].Value.ToString();
                MusicFileName = TempMusicFileName;
                if (System.IO.File.Exists(TempMusicFileName))
                {

                    musicPlayer1.URL = MusicFileName;
                    MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[i].Cells[0].Value);
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
                UpcomingSongPlayerOne = "";
                UpcomingSongPlayerTwo = NextSongId;

                string str = ds.Tables[0].Rows[0]["Time"].ToString();
                string[] arr = str.Split(':');
                DropSongLength = arr[1] + ":" + arr[2];

                //lblAlbumName2.Text = Special_Change;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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

        private void SaveDefaultPlaylist(string DefaultSongId)
        {
            //string lStr = "";
            //lStr = "select * from PlayLists where Name='Default' and userid=" + StaticClass.UserId;
            //DataSet ds = new DataSet();
            //ds = ObjMainClass.fnFillDataSet(lStr);

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    DefaultPlaylistSave("Music");
            //    FillLocalPlaylist();
            //   // MessageBox.Show("This playlist name is already used.", "Player");
            //    return;
            //}
            //else if (StaticClass.Is_Admin != "1")
            //{
            //    MessageBox.Show(ObjMainClass.MainMessage, "Player");
            //    return;
            //}
            DefaultPlaylistSave("Music");
            FillLocalPlaylist();

        }

        private void DefaultPlaylistSave(string PlaylistName)
        {
            Int32 Playlist_Id = 0;
            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            StaticClass.constr.Open();
            SqlCommand cmd = new SqlCommand("InsertPlayLists", StaticClass.constr);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt));
            cmd.Parameters["@UserID"].Value = StaticClass.dfClientId;

            cmd.Parameters.Add(new SqlParameter("@IsPredefined", SqlDbType.Bit));
            cmd.Parameters["@IsPredefined"].Value = 0;

            cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
            cmd.Parameters["@Name"].Value = PlaylistName;

            cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.VarChar, 50));
            cmd.Parameters["@Summary"].Value = " ";

            cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 50));
            cmd.Parameters["@Description"].Value = " ";

            cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
            cmd.Parameters["@TokenId"].Value = StaticClass.TokenId;

            try
            {
                Playlist_Id = Convert.ToInt32(cmd.ExecuteScalar());
                StaticClass.Last100PlaylistId = Playlist_Id;
                string sQr = "";
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                sQr = "insert into PlayLists values(" + Playlist_Id + ", ";
                sQr = sQr + StaticClass.dfClientId + " , '" + PlaylistName + "', " + StaticClass.TokenId + ",'',0 )";
                OleDbCommand cmdSaveLocal = new OleDbCommand();
                cmdSaveLocal.Connection = StaticClass.LocalCon;
                cmdSaveLocal.CommandText = sQr;
                cmdSaveLocal.ExecuteNonQuery();

                // MessageBox.Show("Saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                StaticClass.constr.Close();
            }
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
                    //lblAdvtTimeRemain.Text = lblMusicTimeOne.Text;
                }
                double w = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
                pbarMusic1.Maximum = Convert.ToInt16(musicPlayer1.currentMedia.duration);
                pbarMusic1.Value = Convert.ToInt16(w);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                    //lblAdvtTimeRemain.Text = lblMusicTimeTwo.Text;
                }
                double w = Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);
                pbarMusic2.Maximum = Convert.ToInt16(musicPlayer2.currentMedia.duration);
                pbarMusic2.Value = Convert.ToInt16(w);
            }
            catch
            {
            }
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
                if (StaticClass.ScheduleType == "OneToOnePlaylist")
                {
                    mlsSql = " SELECT st.TitleID, ltrim(st.Title) as Title, st.Time, st.alname AS AlbumName ,";
                    mlsSql = mlsSql + " 0 as TitleYear ,  ltrim(st.arname) as ArtistName,  TitlesInPlaylists.id FROM (TitlesInPlaylists ";
                    mlsSql = mlsSql + " INNER JOIN tbSpecialPlaylists_Titles st ON TitlesInPlaylists.TitleID = st.TitleID )  ";
                    mlsSql = mlsSql + " ORDER BY  Rnd(-(date() * TitlesInPlaylists.Id) * Time()),Rnd(-(date() * TitlesInPlaylists.titleid) * Time())";
                }
                else
                {
                    mlsSql = "SELECT  Titles.TitleID, ltrim(Titles.Title) as Title, Titles.Time,Albums.Name AS AlbumName ,";
                    mlsSql = mlsSql + " Titles.TitleYear as TitleYear ,  ltrim(Artists.Name) as ArtistName , Titles.LocalUrl FROM ((( TitlesInPlaylists  ";
                    mlsSql = mlsSql + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID )  ";
                    mlsSql = mlsSql + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ) ";
                    mlsSql = mlsSql + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ) ";
                    mlsSql = mlsSql + " where TitlesInPlaylists.PlaylistID=" + Convert.ToInt32(currentPlayRow) + "  order by Rnd(Titles.TitleID)";
                }

                dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                InitilizeGrid(dgGrid);


                if ((dtDetail.Rows.Count > 0))
                {
                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {
                        GetLocalPath = dtDetail.Rows[iCtr]["TitleID"] + ".mp4";
                        srNo = iCtr;
                        dgGrid.Rows.Add();
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["TitleID"];

                        Special_Name = "";
                        Special_Change = "";
                        Special_Name = dtDetail.Rows[iCtr]["Title"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[1].Value = Special_Change;

                        string str = dtDetail.Rows[iCtr]["Time"].ToString();
                        string[] arr = str.Split(':');
                        TitleTime = arr[1] + ":" + arr[2];

                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[2].Value = TitleTime;

                        Special_Name = "";
                        Special_Change = "";
                        Special_Name = dtDetail.Rows[iCtr]["AlbumName"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[3].Value = Special_Change;

                        TitleYear = dtDetail.Rows[iCtr]["TitleYear"].ToString();
                        if (TitleYear == "0")
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Value = "- - -";
                        }
                        else
                        {
                            dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Value = dtDetail.Rows[iCtr]["TitleYear"];
                        }

                        Special_Name = "";
                        Special_Change = "";
                        Special_Name = dtDetail.Rows[iCtr]["ArtistName"].ToString();
                        Special_Change = Special_Name.Replace("??$$$??", "'");
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[5].Value = Special_Change;
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["lUrl"].Value = dtDetail.Rows[iCtr]["localUrl"];

                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 11);
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Style.Font = new Font("Segoe UI", 11);
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);

                    }
                }
                foreach (DataGridViewRow row in dgGrid.Rows)
                {
                    row.Height = 30;
                }
                RowHide();
                // TitleCategoryRowHide();
            }



            catch
            {

            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (StaticClass.IsLock == true) return;
            if (musicPlayer1.URL != "")
            {
                if (btnPlay.Text == ".")
                {
                    btnPlay.Text = "";
                    musicPlayer1.Ctlcontrols.play();
                    if (btnMute.Text == "")
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
                    btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
                    // timGetRemainAdvtTime.Enabled = true;
                }
                else if (btnPlay.Text == "")
                {
                    btnPlay.Text = ".";
                    musicPlayer1.Ctlcontrols.pause();
                    btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
                    //timGetRemainAdvtTime.Enabled = false;
                }
            }
            else if (musicPlayer2.URL != "")
            {
                if (btnPlay.Text == ".")
                {
                    btnPlay.Text = "";
                    musicPlayer2.Ctlcontrols.play();
                    if (btnMute.Text == "")
                    {
                        musicPlayer2.settings.volume = 100;

                    }
                    btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
                    // timGetRemainAdvtTime.Enabled = true;
                }
                else if (btnPlay.Text == "")
                {
                    btnPlay.Text = ".";
                    musicPlayer2.Ctlcontrols.pause();
                    btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Play_Blue));
                    // timGetRemainAdvtTime.Enabled = false;
                }
            }

        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            if (StaticClass.IsLock == true) return;
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
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer1.settings.volume = 100;
                    }
                    else
                    {
                        musicPlayer1.settings.volume = 0;
                    }
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
                    if (StaticClass.IsVideoMute == "0")
                    {
                        musicPlayer2.settings.volume = 100;
                    }
                    else
                    {
                        musicPlayer2.settings.volume = 0;
                    }
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



        private void txtPlaylistName_KeyDown(object sender, KeyEventArgs e)
        {

            //try
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        if (StaticClass.IsLock == true) return;
            //        if (ObjMainClass.CheckForInternetConnection() == false)
            //        {
            //            MessageBox.Show("Please check your Internet connection.", "Player");
            //            return;
            //        }
            //        string lStr = "";
            //        lStr = "select * from PlayLists where Name='" + txtPlaylistName.Text + "' and userid=" + StaticClass.dfClientId + " and tokenid=" + StaticClass.TokenId;
            //        DataSet ds = new DataSet();
            //        ds = ObjMainClass.fnFillDataSet(lStr);

            //        if (txtPlaylistName.Text == "")
            //        {
            //            MessageBox.Show("The playlist cannot be empty or without a name.", "Player");
            //            return;
            //        }
            //        else if (pAction == "New")
            //        {
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                MessageBox.Show("This playlist name is already used.", "Player");
            //                return;
            //            }
            //        }
            //        else if (StaticClass.Is_Admin != "1")
            //        {
            //            MessageBox.Show(ObjMainClass.MainMessage, "Player");
            //            return;
            //        }
            //        if (pAction == "New")
            //        {
            //            PlaylistSave();
            //            txtPlaylistName.Text = "";
            //            pAction = "New";
            //            //  ModifyPlaylistId = 0;
            //        }
            //        else
            //        {
            //            PlaylistModify();
            //            txtPlaylistName.Text = "";
            //            pAction = "New";
            //        }
            //        FillLocalPlaylist();
            //        Set_Playlist_foucs();
            //        ModifyPlaylistId = 0;

            //    }

            //}
            //catch
            //{
            //    // MessageBox.Show("Please check your Internet connection.","Player");
            //}

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
                dgGrid.Rows.Insert(RowIndex, Title_id, Title, TitleTime, Title_Year, ArtistName, AlbumName);
            }
            for (iCtr = 0; iCtr < dgGrid.Rows.Count; iCtr++)
            {
                dgGrid.Rows[iCtr].Cells["songname"].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                dgGrid.Rows[iCtr].Cells["Length"].Style.Font = new Font("Segoe UI", 9);
                dgGrid.Rows[iCtr].Cells["Album"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                dgGrid.Rows[iCtr].Cells["Artist"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
            }

            foreach (DataGridViewRow row in dgGrid.Rows)
            {
                row.Height = 30;
            }

        }
        private void moveUp(DataGridView dgGrid)
        {
            try
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
            catch (Exception ex)
            {
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
        private void dgLocalPlaylist_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (StaticClass.IsLock == true) return;
                if (e.KeyCode == Keys.Delete)
                {
                    if (ObjMainClass.CheckForInternetConnection() == false)
                    {
                        MessageBox.Show("Please check your Internet connection.", "Player");
                        return;
                    }
                    if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                    {
                        MessageBox.Show("It is not possible to delete the default playlist.", "Player");
                        return;
                    }
                    string sgr = "";
                    if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                    {
                        if (musicPlayer1.URL != "")
                        {
                            sgr = "select * from TitlesInPlaylists where PlaylistID=" + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value + " and TitleId=" + MusicPlayer1CurrentSongId.ToString();
                            DataSet ds = new DataSet();
                            ds = ObjMainClass.fnFillDataSet(sgr);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                                {
                                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer1CurrentSongId.ToString())
                                    {
                                        MessageBox.Show("It is not possible to delete the current playlist while playing.", "Player");
                                        return;
                                    }
                                }
                            }
                        }
                        if (musicPlayer2.URL != "")
                        {
                            sgr = "select * from TitlesInPlaylists where PlaylistID=" + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value + " and TitleId=" + MusicPlayer2CurrentSongId.ToString();
                            DataSet ds = new DataSet();
                            ds = ObjMainClass.fnFillDataSet(sgr);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                                {
                                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer2CurrentSongId.ToString())
                                    {
                                        MessageBox.Show("It is not possible to delete the current playlist while playing.", "Player");
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
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        sQr = "delete from TitlesInPlaylists where PlaylistID =" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);

                        OleDbCommand cmdDelPlaylistLocal = new OleDbCommand();
                        cmdDelPlaylistLocal.Connection = StaticClass.LocalCon;
                        cmdDelPlaylistLocal.CommandText = sQr;
                        cmdDelPlaylistLocal.ExecuteNonQuery();


                        sQr = "";
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        sQr = "delete from Playlists where PlaylistID =" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);

                        OleDbCommand cmdDelLocal = new OleDbCommand();
                        cmdDelLocal.Connection = StaticClass.LocalCon;
                        cmdDelLocal.CommandText = sQr;
                        cmdDelLocal.ExecuteNonQuery();


                        string sdr = "";
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        sdr = "delete from tbMusicLastSettings where tokenno=" + StaticClass.TokenId;
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
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch
            {

                return;
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
            if (StaticClass.IsLock == true) return;
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
                    localfilename = dgPlaylist.Rows[CurrentRow].Cells["lurl"].Value.ToString();
                    TempSongName = localfilename;
                }
                else
                {
                    CurrentRow = dgPlaylist.CurrentCell.RowIndex;
                    localfilename = dgPlaylist.Rows[CurrentRow].Cells["lurl"].Value.ToString();
                    TempSongName = localfilename;
                }

                TempSongPath = Application.StartupPath + "\\so\\" + TempSongName;
                string localfilePath = Application.StartupPath + "\\so\\" + localfilename;
                if (System.IO.File.Exists(TempSongPath))
                {

                    musicPlayer1.URL = localfilePath;
                    MusicPlayer1CurrentSongId = Convert.ToInt32(dgPlaylist.Rows[CurrentRow].Cells[0].Value);
                    if (btnMute.Text == "")
                    {
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
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
                        if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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

                    GetSavedRating(MusicPlayer1CurrentSongId.ToString(), dgSongRatingPlayerOne);
                    SetDisableRating(dgSongRatingPlayerTwo);
                }

                rCount = 0;
                DropSongLength = "";
                IsSongDropAdvt = false;
                label7.Text = "0";
                label8.Text = "0";
                label18.Text = "0";
                IsAdvtTimeGet = false;
                GrossTotaltime = 0;
                // timGetRemainAdvtTime.Enabled = true;

                timResetSong.Start();

            }

            catch { }
        }

        private void picSongRemove_Click(object sender, EventArgs e)
        {
            if (StaticClass.IsLock == true) return;
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Player");
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
                localfilename = dgPlaylist.Rows[rowindex].Cells["lurl"].Value.ToString();
                string localfilePath = Application.StartupPath + "\\so\\" + localfilename;

                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    if ((dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default") && (Convert.ToInt32(dgPlaylist.Rows.Count) <= 1))
                    {
                        MessageBox.Show("It is not possible to delete songs from the default playlist.", "Player");
                        return;
                    }

                    if (StaticClass.isDownload != "1" || StaticClass.isRemove != "1")
                    {
                        MessageBox.Show(ObjMainClass.MainMessage, "Player");
                        return;
                    }
                    if (musicPlayer1.URL != "")
                    {
                        if (dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() == MusicPlayer1CurrentSongId.ToString())
                        {
                            MessageBox.Show("It is not possible to delete the current song.", "Player");
                            return;
                        }
                    }
                    if (musicPlayer2.URL != "")
                    {
                        if (dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString() == MusicPlayer2CurrentSongId.ToString())
                        {
                            MessageBox.Show("It is not possible to delete the current song.", "Player");
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
                        label18.Text = "0";
                        IsAdvtTimeGet = false;
                        GrossTotaltime = 0;
                        // timGetRemainAdvtTime.Enabled = true;

                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgPlaylist.Rows[rowindex].Cells[0].Value;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdLocal = new OleDbCommand();
                        cmdLocal.Connection = StaticClass.LocalCon;
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgPlaylist.Rows[rowindex].Cells[0].Value;
                        cmdLocal.ExecuteNonQuery();

                        delete_temp_data(dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());
                        dgPlaylist.Rows.RemoveAt(dgPlaylist.CurrentCell.RowIndex);
                        //if (chkShuffleSong.Checked == true)
                        //{
                        //    PopulateShuffleSong(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value), ShuffleCount);
                        //}
                        //else
                        //{
                        //    PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                        //}
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
                                    if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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
                                    if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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

                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdLocal = new OleDbCommand();
                        cmdLocal.Connection = StaticClass.LocalCon;
                        cmdLocal.CommandText = "delete from TitlesInPlaylists where PlaylistID=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " and TitleID =" + dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value;
                        cmdLocal.ExecuteNonQuery();

                        //delete_temp_data(dgPlaylist.Rows[dgPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());
                        delete_temp_data(dgOtherPlaylist.Rows[dgOtherPlaylist.CurrentCell.RowIndex].Cells[0].Value.ToString());
                        dgOtherPlaylist.Rows.RemoveAt(dgOtherPlaylist.CurrentCell.RowIndex);
                        //if (chkShuffleSong.Checked == true)
                        //{
                        //    PopulateShuffleSong(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value), ShuffleCount);
                        //}
                        //else
                        //{
                        //    PopulateInputFileTypeDetail(dgOtherPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                        //}
                    }

                    GetSongCounter();



                }

                else
                {
                    MessageBox.Show("Please select a playlist.", "Player");
                }
            }
            catch
            {
                return;
            }
        }



        private void picPlaylistRemove_Click(object sender, EventArgs e)
        {
            if (StaticClass.IsLock == true) return;
            string sgr = "";
            if (ObjMainClass.CheckForInternetConnection() == false)
            {
                MessageBox.Show("Please check your Internet connection.", "Player");
                return;
            }
            try
            {
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    MessageBox.Show("It is not possible to delete the default playlist.", "Player");
                    return;
                }
                if (dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[2].Value.ToString() == "Default")
                {
                    if (musicPlayer1.URL != "")
                    {
                        sgr = "select * from TitlesInPlaylists where PlaylistID=" + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value + " and TitleId=" + MusicPlayer1CurrentSongId.ToString();
                        DataSet ds = new DataSet();
                        ds = ObjMainClass.fnFillDataSet_Local(sgr);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                            {
                                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer1CurrentSongId.ToString())
                                {
                                    MessageBox.Show("It is not possible to delete the current playlist while playing.", "Player");
                                    return;
                                }
                            }
                        }
                    }
                    if (musicPlayer2.URL != "")
                    {
                        sgr = "select * from TitlesInPlaylists where PlaylistID=" + dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value + " and TitleId=" + MusicPlayer2CurrentSongId.ToString();
                        DataSet ds = new DataSet();
                        ds = ObjMainClass.fnFillDataSet_Local(sgr);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                            {
                                if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer2CurrentSongId.ToString())
                                {
                                    MessageBox.Show("It is not possible to delete the current playlist while playing.", "Player");
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
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    sQr = "delete from Playlists where PlaylistID =" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value);

                    OleDbCommand cmdDelLocal = new OleDbCommand();
                    cmdDelLocal.Connection = StaticClass.LocalCon;
                    cmdDelLocal.CommandText = sQr;
                    cmdDelLocal.ExecuteNonQuery();



                    string sdr = "";
                    if (StaticClass.constr.State == ConnectionState.Open)
                    {
                        StaticClass.constr.Close();
                    }
                    sdr = "delete from tbMusicLastSettings where tokenno=" + StaticClass.TokenId;
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
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    StaticClass.constr.Close();
                }
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
                        //dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionBackColor = Color.FromArgb(20, 162, 175);
                        //dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.White;

                        if (dgLocalPlaylist.Rows[i].Cells[2].Value.ToString() == "Default")
                        {
                            dgPlaylist.Visible = true;

                            dgOtherPlaylist.Visible = false;

                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.SelectionForeColor = Color.Yellow;

                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[3].Style.BackColor = Color.LightBlue;
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[3].Style.SelectionBackColor = Color.LightBlue;
                            PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value));
                        }
                        else
                        {
                            dgOtherPlaylist.Visible = true;
                            dgPlaylist.Visible = false;
                            dgOtherPlaylist.Dock = DockStyle.Fill;
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[3].Style.BackColor = Color.White;
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.ForeColor = Color.Black;
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[3].Style.SelectionBackColor = Color.White;
                            dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
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
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
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
                    if (IsCopyFromLocalList == "No")
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

                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
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
                    }
                    DeleteHideSong(dgHideSongs.Rows[i].Cells[0].Value.ToString());
                }
            }

        }

        private void btnGreenDownload_Click(object sender, EventArgs e)
        {

        }

        private void btnPurple_Click(object sender, EventArgs e)
        {

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





        private void mainwindow_FormClosed(object sender, FormClosedEventArgs e)
        {


        }

        private void timResetSong_Tick(object sender, EventArgs e)
        {
            try
            {
                string LocalUpcomingSong = "";
                if (dgPlaylist.Rows.Count == 0) return;
                if ((musicPlayer1.playState == WMPPlayState.wmppsReady) && (musicPlayer2.playState == WMPPlayState.wmppsStopped))
                {
                    ResetFunction();
                }
                if ((musicPlayer1.playState == WMPPlayState.wmppsStopped) && (musicPlayer2.playState == WMPPlayState.wmppsReady))
                {
                    ResetFunction();
                }

                if ((musicPlayer1.playState == WMPPlayState.wmppsReady) && (musicPlayer2.playState == WMPPlayState.wmppsReady))
                {
                    ResetFunction();
                }
                if ((musicPlayer1.playState == WMPPlayState.wmppsReady) && (musicPlayer2.playState == WMPPlayState.wmppsUndefined))
                {
                    ResetFunction();
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void ResetFunction()
        {
            try
            {

                PopulateSplPlaylist(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells["playlistId"].Value));
                PlaySongDefault();
                DisplaySongPlayerOne();
                Set_foucs_PayerOne();

                musicPlayer2.settings.volume = 0;
                musicPlayer2.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                ObjMainClass.CompactLocaldb();
            }
            catch (Exception ex)
            {
            }

        }
        private void mainwindow_Move(object sender, EventArgs e)
        {
            // this.Location = new Point(121, 19);
        }

        private void mainwindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            //IsExitApp = "Yes";
            if (IsExitApp == "No")
            {
                e.Cancel = true;
                return;
            }
            else
            {

                e.Cancel = false;
                Application.Exit();
            }
        }
        private void SaveLastPostion(string LastPosition)
        {
            try
            {
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = "delete from tbLastPosition where tokenid= " + StaticClass.TokenId;
                cmdTitle.ExecuteNonQuery();

                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = "insert into tbLastPosition values(" + StaticClass.TokenId + ",'" + LastPosition + "')";
                cmdTitle.ExecuteNonQuery();
            }
            catch
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






        private void mainwindow_SizeChanged(object sender, EventArgs e)
        {
            picMiddleLogo.Location = new Point(
      this.Width / 2 - picMiddleLogo.Size.Width / 2,
      this.Height / 2 - picMiddleLogo.Size.Height / 2);


            picMp3Logo.Location = new Point(
          this.pnlLogo.Width / 2 - picMp3Logo.Size.Width / 2,
          this.pnlLogo.Height / 2 - picMp3Logo.Size.Height / 2);
        }
        private void dgLocalPlaylist_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (StaticClass.IsLock == true) return;
            //if (e.RowIndex == -1)
            //{
            //    return;
            //}

            //if (e.ColumnIndex == 1)
            //{
            //    if (e.RowIndex >= 0)
            //    {
            //        string str45 = dgLocalPlaylist.Rows[e.RowIndex].Cells[1].Value.ToString();
            //        string[] arr = str45.Split('(');
            //        txtPlaylistName.Text = arr[0].Trim();
            //        ModifyPlaylistId = Convert.ToInt32(dgLocalPlaylist.Rows[e.RowIndex].Cells[0].Value);
            //        pAction = "Modify";
            //        txtPlaylistName.Focus();
            //    }
            //}
        }








        private void picStream_Click(object sender, EventArgs e)
        {


        }






        private void DamPlayer_Validating(object sender, CancelEventArgs e)
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
            if (StaticClass.IsLock == true) return;
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
            catch (Exception ex) { Console.WriteLine(ex.Message); }
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
            catch (Exception ex) { Console.WriteLine(ex.Message); }
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
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
        private void dgSongRatingPlayerOne_MouseLeave(object sender, EventArgs e)
        {
            if (StaticClass.IsLock == true) return;
            if (musicPlayer1.URL != "")
            {
                GetSavedRating(MusicPlayer1CurrentSongId.ToString(), dgSongRatingPlayerOne);
            }

        }

        private void dgSongRatingPlayerOne_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (StaticClass.IsLock == true) return;
            if (musicPlayer1.URL != "")
            {
                SaveRating(e.ColumnIndex, Convert.ToInt32(MusicPlayer1CurrentSongId));
            }

        }
        private void SaveRating(Int32 TitleRating, Int32 RatingTitleId)
        {
            string strInsertrating = "";
            try
            {


                ////////////// Save Local Data ////////////////

                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                strInsertrating = "delete from tbTitleRating where tokenid=" + StaticClass.TokenId + " and titleId= " + RatingTitleId + "";

                OleDbCommand cmdTitleDelete = new OleDbCommand();
                cmdTitleDelete.Connection = StaticClass.LocalCon;
                cmdTitleDelete.CommandText = strInsertrating;
                cmdTitleDelete.ExecuteNonQuery();

                /////////////////////////////////////////////////////////////
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                strInsertrating = "insert into tbTitleRating values (" + StaticClass.TokenId + " , " + RatingTitleId + " , ";
                strInsertrating = strInsertrating + TitleRating + ")";


                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsertrating;
                cmdTitle.ExecuteNonQuery();



                if (musicPlayer1.URL != "")
                {
                    GetSavedRating(MusicPlayer1CurrentSongId.ToString(), dgSongRatingPlayerOne);
                }
                if (musicPlayer2.URL != "")
                {
                    GetSavedRating(MusicPlayer2CurrentSongId.ToString(), dgSongRatingPlayerTwo);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }

        private void dgSongRatingPlayerTwo_MouseLeave(object sender, EventArgs e)
        {
            if (StaticClass.IsLock == true) return;
            if (musicPlayer2.URL != "")
            {
                GetSavedRating(MusicPlayer2CurrentSongId.ToString(), dgSongRatingPlayerTwo);
            }
        }

        private void dgSongRatingPlayerTwo_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (StaticClass.IsLock == true) return;
            if (musicPlayer2.URL != "")
            {
                SaveRating(e.ColumnIndex, Convert.ToInt32(MusicPlayer2CurrentSongId));
            }
        }

        private void dgSongRatingPlayerTwo_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (StaticClass.IsLock == true) return;
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
        #endregion

        private void tbcMain_TabIndexChanged(object sender, EventArgs e)
        {

        }








        private void DisplaySongPlayerOne()
        {
            try
            {
                string mlsSql = "";
                string currentFileName;
                var Special_Name = "";
                string Special_Change = "";

                currentFileName = MusicPlayer1CurrentSongId.ToString();

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
            string mlsSql = "";
            string currentFileName;
            var Special_Name = "";
            string Special_Change = "";
            currentFileName = MusicPlayer2CurrentSongId.ToString();
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
        }
        #region "Advt"
        int AdvtCurrentRowL = 0;
        string IsAdvtPause = "No";
        Int32 CurrAdvtId = 0;
        DataRow dtRow;
        string IsNextAdvtPlaylistFind = "No";
        private void timAdvt_Tick(object sender, EventArgs e)
        {

            //try
            //{
            try
            {
                // AdvtPlayer.fullScreen = true;
                AdvtPlayer.Dock = DockStyle.Fill;
                //Roop
                AdvtPlayer.BringToFront();
                musicPlayer1.SendToBack();
                musicPlayer2.SendToBack();

            }
            catch (Exception ex)
            {
                goto BBx;
            }
        BBx:

            if ((AdvtPlayer.playState == WMPPlayState.wmppsStopped) || (AdvtPlayer.playState == WMPPlayState.wmppsReady))
            {

                AdvtCurrentRow = AdvtCurrentRow + 1;
                AdvtTimeResult = 0;
                TimePlayerOne = 0;
                TimePlayerTwo = 0;
                prvPlayerOneTime = 0;
                prvPlayerTwoTime = 0;
                TimeStreamPlayer = 0;
                AdvtCurrentSongId = 0;
                lblSongCount.Text = "1";

                if (lblPlayerName.Text == "One")
                {
                    musicPlayer1.settings.volume = 0;
                    musicPlayer1.Ctlcontrols.stop();
                    musicPlayer1.URL = "";

                    musicPlayer2.Ctlcontrols.play();
                    musicPlayer2.BringToFront();
                    if (btnMute.Text == "")
                    {
                        musicPlayer2.settings.mute = false;
                        musicPlayer1.settings.mute = false;
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer2.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer2.settings.volume = 0;
                        }
                    }
                    else if (btnMute.Text == ".")
                    {
                        musicPlayer2.settings.mute = true;
                        musicPlayer1.settings.mute = true;
                    }

                    Song_Set_foucs2();

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
                        if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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
                else
                {
                    musicPlayer2.settings.volume = 0;
                    musicPlayer2.Ctlcontrols.stop();
                    musicPlayer2.URL = "";

                    musicPlayer1.Ctlcontrols.play();
                    musicPlayer1.BringToFront();
                    if (btnMute.Text == "")
                    {
                        musicPlayer2.settings.mute = false;
                        musicPlayer1.settings.mute = false;
                        if (StaticClass.IsVideoMute == "0")
                        {
                            musicPlayer1.settings.volume = 100;
                        }
                        else
                        {
                            musicPlayer1.settings.volume = 0;
                        }
                    }
                    else if (btnMute.Text == ".")
                    {
                        musicPlayer2.settings.mute = true;
                        musicPlayer1.settings.mute = true;
                    }

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
                        if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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
                AdvtPlayer.SendToBack();
                //FillMainAdvertisement();
                timGetAdvtTime.Enabled = true;
                timAdvt.Enabled = false;

                rCount = 0;
                DropSongLength = "";
                IsSongDropAdvt = false;
                label7.Text = "0";
                label8.Text = "0";
                label18.Text = "0";
                IsAdvtTimeGet = false;
                GrossTotaltime = 0;
                // timGetRemainAdvtTime.Enabled = true;
            }

            //}

            //catch (Exception ex)
            //{
            //    //MessageBox.Show(ex.Message + " -- timer advt");

            //    timGetAdvtTime.Enabled = true;
            //    rCount = 0;
            //    DropSongLength = "";
            //    IsSongDropAdvt = false;
            //    label7.Text = "0";
            //    label8.Text = "0";
            //    label18.Text = "0";
            //    IsAdvtTimeGet = false;
            //    GrossTotaltime = 0;
            //    timAdvt.Enabled = false;
            //}

        }
        string IsAllDownload = "No";
        private void timGetAdvtTime_Tick(object sender, EventArgs e)
        {
            if (dgPlaylist.Rows.Count == 0)
            {
                return;
            }
            if (StaticClass.IsPlayerClose != "Yes")
            {
                //                GetAdvtPlayingTypeRuntime();
                if (StaticClass.IsAdvtWithSongs == true)
                {
                    if (dgAdvt.Rows.Count > 0)
                    {
                        if (TotalSongPlay >= StaticClass.TotalAdvtSongs)
                        {
                            GetUpcomingAdvt();
                            if (dgAdvt.Rows[AdvtCurrentRow].Cells["Status"].Style.BackColor == Color.LightGreen)
                            {
                                TotalSongPlay = 0;
                                lblSongCount.Text = "2";
                                GetUpcomingAdvtName();
                                timGetAdvtTime.Enabled = false;
                                if (dgAdvt.Rows[AdvtCurrentRow].Cells["playingType"].Value.ToString() == "Hard Stop")
                                {
                                    if ((musicPlayer1.URL != "") && (musicPlayer2.URL != ""))
                                    {
                                        AdvtTimeResult = StaticClass.AdvtTime - 15;
                                        return;
                                    }
                                    if (musicPlayer1.URL == "")
                                    {
                                        IsbtnClick = "Y";
                                        PlaylistFadeSongPlayerOne();
                                        timAutoFadePlayerOne.Enabled = false;
                                        timAutoFadePlayerTwo.Enabled = false;
                                        timer5.Enabled = true;
                                        timGetAdvtTime.Enabled = false;
                                        return;
                                    }
                                    if (musicPlayer2.URL == "")
                                    {
                                        IsbtnClick = "Y";
                                        PlaylistFadeSong();
                                        timAutoFadePlayerOne.Enabled = false;
                                        timAutoFadePlayerTwo.Enabled = false;
                                        timer4.Enabled = true;
                                        timGetAdvtTime.Enabled = false;
                                        return;

                                    }
                                }
                            }
                        }
                    }
                }
                if (StaticClass.IsAdvtManual == true)
                {
                    AdvtTimeResult = AdvtTimeResult + 1;
                    lblAdvtMainTime.Text = AdvtTimeResult.ToString();
                    if (dgAdvt.Rows.Count > 0)
                    {
                        if (AdvtTimeResult >= StaticClass.AdvtTime)
                        {
                            GetUpcomingAdvt();
                            if (dgAdvt.Rows[AdvtCurrentRow].Cells["Status"].Style.BackColor == Color.LightGreen)
                            {
                                AdvtTimeResult = 0;
                                lblSongCount.Text = "2";
                                GetUpcomingAdvtName();

                                if (dgAdvt.Rows[AdvtCurrentRow].Cells["playingType"].Value.ToString() == "Hard Stop")
                                {
                                    if ((musicPlayer1.URL != "") && (musicPlayer2.URL != ""))
                                    {
                                        AdvtTimeResult = StaticClass.AdvtTime - 15;
                                        return;
                                    }

                                    if (dgPlaylist.Rows.Count == 0)
                                    {
                                        AdvtTimeResult = 0;
                                        return;
                                    }

                                    if (musicPlayer1.URL == "")
                                    {
                                        IsbtnClick = "Y";
                                        PlaylistFadeSongPlayerOne();
                                        timAutoFadePlayerOne.Enabled = false;
                                        timAutoFadePlayerTwo.Enabled = false;
                                        timer5.Enabled = true;
                                        timGetAdvtTime.Enabled = false;
                                        return;
                                    }
                                    if (musicPlayer2.URL == "")
                                    {
                                        IsbtnClick = "Y";
                                        PlaylistFadeSong();
                                        timAutoFadePlayerOne.Enabled = false;
                                        timAutoFadePlayerTwo.Enabled = false;
                                        timer4.Enabled = true;
                                        timGetAdvtTime.Enabled = false;
                                        return;

                                    }
                                    //if (musicPlayer1.URL != "")
                                    //{
                                    //    lblPlayerName.Text = "Two";
                                    //    musicPlayer2.Ctlcontrols.stop();
                                    //    musicPlayer2.URL = "";
                                    //    musicPlayer1.Ctlcontrols.pause();
                                    //}
                                    //if (musicPlayer2.URL != "")
                                    //{
                                    //    lblPlayerName.Text = "One";
                                    //    musicPlayer1.Ctlcontrols.stop();
                                    //    musicPlayer1.URL = "";
                                    //    musicPlayer2.Ctlcontrols.pause();
                                    //}
                                    //lblAdvtTimeRemain.Text = "00:00";
                                    //panAdvt.Height = 124;
                                    //panAdvt.Location = new Point(0, 0);
                                    //FillPanAdvt();
                                }



                            }
                        }
                    }
                }
                if (StaticClass.IsAdvtBetweenTime == true)
                {
                    if (dgAdvt.Rows.Count > 0)
                    {
                        IsAllDownload = "Yes";
                        lblCurrentTime.Text = string.Format(fi, "{0:hh:mm tt}", DateTime.Now);
                        if (StaticClass.AdvtClosingTime == lblCurrentTime.Text) return;
                        if (Convert.ToDateTime(StaticClass.AdvtClosingTime) <= Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)))
                        {
                            for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
                            {
                                if ((Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) >= Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgAdvt.Rows[iRow].Cells["bStime"].Value)))) && (Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) < Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgAdvt.Rows[iRow].Cells["bEtime"].Value)))))
                                {
                                    if (dgAdvt.Rows[iRow].Cells["Status"].Style.BackColor != Color.LightGreen)
                                    {
                                        IsAllDownload = "No";
                                        break;
                                    }
                                }
                            }
                            if (IsAllDownload == "No")
                            {
                                return;
                            }
                            for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
                            {
                                if ((Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) >= Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgAdvt.Rows[iRow].Cells["bStime"].Value)))) && (Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) < Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgAdvt.Rows[iRow].Cells["bEtime"].Value)))))
                                {
                                    FillAdvtTempData();
                                    GetUpcomingAdvt_Between();
                                    StaticClass.AdvtClosingTime = string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgAdvt.Rows[iRow].Cells["bEtime"].Value));

                                    if (dgAdvtTemp.Rows.Count > 0)
                                    {
                                        GetUpcomingAdvtName_Between();
                                    }
                                    AdvtTimeResult = 0;
                                    lblSongCount.Text = "2";
                                    timGetAdvtTime.Enabled = false;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (StaticClass.IsAdvtTime == true)
                {
                    lblCurrentTime.Text = DateTime.Now.ToString("hh:mm tt");
                    if (AdvtPlayTime == lblCurrentTime.Text) return;
                    for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
                    {
                        if (dgAdvt.Rows[iRow].Cells["Status"].Style.BackColor == Color.LightGreen)

                        {



                            if (lblCurrentTime.Text == dgAdvt.Rows[iRow].Cells[8].Value.ToString())
                            {
                                GetUpcomingAdvt();
                                AdvtTimeResult = 0;
                                AdvtPlayTime = lblCurrentTime.Text;
                                AdvtCurrentRow = iRow;
                                lblSongCount.Text = "2";
                                GetUpcomingAdvtName();
                                //panUpcomingAdvt.Visible = true;
                                timGetAdvtTime.Enabled = false;


                                if (dgAdvt.Rows[AdvtCurrentRow].Cells["playingType"].Value.ToString() == "Hard Stop")
                                {
                                    if ((musicPlayer1.URL != "") && (musicPlayer2.URL != ""))
                                    {
                                        AdvtTimeResult = StaticClass.AdvtTime - 15;
                                        return;
                                    }
                                    if (musicPlayer1.URL == "")
                                    {
                                        IsbtnClick = "Y";
                                        PlaylistFadeSongPlayerOne();
                                        timAutoFadePlayerOne.Enabled = false;
                                        timAutoFadePlayerTwo.Enabled = false;
                                        timer5.Enabled = true;
                                        timGetAdvtTime.Enabled = false;
                                        return;
                                    }
                                    if (musicPlayer2.URL == "")
                                    {
                                        IsbtnClick = "Y";
                                        PlaylistFadeSong();
                                        timAutoFadePlayerOne.Enabled = false;
                                        timAutoFadePlayerTwo.Enabled = false;
                                        timer4.Enabled = true;
                                        timGetAdvtTime.Enabled = false;
                                        return;

                                    }
                                }


                                break;
                            }
                        }
                    }
                }
            }


        }

        private void GetUpcomingAdvtName_Between()
        {
            if (AdvtCurrentRowL != 0)
            {
                if (AdvtCurrentRowL >= dgAdvtTemp.Rows.Count)
                {
                    AdvtCurrentRowL = 0;
                }
            }

            if (AdvtCurrentSongId != 0)
            {
                for (int iRow = 0; iRow < dgAdvtTemp.Rows.Count; iRow++)
                {
                    if (AdvtCurrentSongId == Convert.ToInt32(dgAdvtTemp.Rows[iRow].Cells[0].Value))
                    {
                        AdvtCurrentRowL = iRow;
                        break;
                    }
                }
            }
            else
            {
                for (int iRow = 0; iRow < dgAdvtTemp.Rows.Count; iRow++)
                {

                    AdvtCurrentRowL = iRow;
                    break;

                }
            }

            timGetRemainAdvtTime.Enabled = true;
            lblUpcomingAdvtName.Text = "Next advertisement:- " + dgAdvtTemp.Rows[AdvtCurrentRowL].Cells[1].Value.ToString().Trim();
            if (Convert.ToInt32(dgAdvtTemp.Rows[AdvtCurrentRowL].Cells["IsVideoMute"].Value) == 1)
            {
                IsVideoMute = 1;
            }
            else
            {
                IsVideoMute = 0;
            }


        }

        private void GetUpcomingAdvt_Between()
        {
            if (AdvtCurrentRowL != 0)
            {
                if (AdvtCurrentRowL >= dgAdvtTemp.Rows.Count)
                {
                    AdvtCurrentRowL = 0;
                }
            }

            if (AdvtCurrentSongId != 0)
            {
                for (int iRow = 0; iRow < dgAdvtTemp.Rows.Count; iRow++)
                {
                    if (AdvtCurrentSongId == Convert.ToInt32(dgAdvtTemp.Rows[iRow].Cells[0].Value))
                    {
                        AdvtCurrentRowL = iRow;
                        break;
                    }
                }
            }
            else
            {
                for (int iRow = 0; iRow < dgAdvtTemp.Rows.Count; iRow++)
                {

                    AdvtCurrentRowL = iRow;
                    break;

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
            if ((StaticClass.IsAdvtManual == true) || (StaticClass.IsAdvtWithSongs == true))
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
            timGetRemainAdvtTime.Enabled = true;
            lblUpcomingAdvtName.Text = "Next advertisement:- " + dgAdvt.Rows[AdvtCurrentRow].Cells[1].Value.ToString().Trim();
            if (Convert.ToInt32(dgAdvt.Rows[AdvtCurrentRow].Cells["IsVideoMute"].Value) == 1)
            {
                IsVideoMute = 1;
            }
            else
            {
                IsVideoMute = 0;
            }


        }

        private void GetUpcomingAdvt()
        {
            if (AdvtCurrentRow != 0)
            {
                if (AdvtCurrentRow >= dgAdvt.Rows.Count)
                {
                    AdvtCurrentRow = 0;
                }
            }
            if ((StaticClass.IsAdvtManual == true) || (StaticClass.IsAdvtWithSongs == true))
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


        }
        private void FillMainAdvertisement()
        {
            string PlayerType = "";
            PlayerType = "Copyright";
            string str = "";
            int iCtr;
            string lPath = "";
            DataTable dtDetail;
            DataTable dtDetailLocal;
            str = "select * from tbAdvt where ScheduleDate=#" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "#";
            dtDetailLocal = ObjMainClass.fnFillDataTable_Local(str);
            str = "";

            str = "select * from tbAdvertisement where #" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "# between AdvtStartDate and AdvtEndDate order by srno";
            dtDetail = ObjMainClass.fnFillDataTable_Local(str);
            InitilizeMainAdvertisementGrid();
            if ((dtDetail.Rows.Count > 0))
            {

                // timGetRemainAdvtTime.Enabled = true;
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgAdvt.Rows.Add();
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[0].Value = dtDetail.Rows[iCtr]["AdvtId"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[1].Value = dtDetail.Rows[iCtr]["AdvtDisplayName"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[2].Value = dtDetail.Rows[iCtr]["AdvtCompanyName"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[3].Value = dtDetail.Rows[iCtr]["AdvtFilePath"];//Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".mp4";

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
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[8].Value = string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["AdvtTime"]);

                    if (Convert.ToInt32(dtDetail.Rows[iCtr]["IsVideo"]) == 1)
                    {
                        lPath = Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".mp4";
                    }
                    else if (Convert.ToInt32(dtDetail.Rows[iCtr]["IsPicture"]) == 1)
                    {
                        lPath = Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".jpg";
                    }
                    else
                    {
                        lPath = Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".mp3";
                    }

                    if (System.IO.File.Exists(lPath))
                    {
                        Int32 AdsSize = 0;
                        if (string.IsNullOrEmpty(dtDetail.Rows[iCtr]["filesize"].ToString()) == false)
                        {
                            AdsSize = Convert.ToInt32(dtDetail.Rows[iCtr]["filesize"]);
                        }
                        else
                        {
                            AdsSize = 0;
                        }
                        long DownloadedSize = new FileInfo(lPath).Length;
                        if (DownloadedSize == AdsSize)
                        {
                            dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["Download"].Value = "Yes";
                            dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["Status"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["Download"].Value = "No";
                            dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["Status"].Style.BackColor = Color.LightSalmon;
                        }
                    }
                    else
                    {
                        dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["Download"].Value = "No";
                        dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["Status"].Style.BackColor = Color.LightSalmon;
                    }
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["AdvthttpUrl"].Value = dtDetail.Rows[iCtr]["AdvthttpUrl"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["IsVideo"].Value = dtDetail.Rows[iCtr]["IsVideo"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["IsVideoMute"].Value = dtDetail.Rows[iCtr]["IsVideoMute"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["IsPicture"].Value = dtDetail.Rows[iCtr]["IsPicture"];

                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["bStime"].Value = string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["bStime"]);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["bEtime"].Value = string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["bEtime"]);



                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["IsTime"].Value = dtDetail.Rows[iCtr]["IsTime"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["IsMinute"].Value = dtDetail.Rows[iCtr]["IsMinute"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["IsSong"].Value = dtDetail.Rows[iCtr]["IsSong"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["IsBetween"].Value = dtDetail.Rows[iCtr]["IsBetween"];

                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["TotalMinutes"].Value = dtDetail.Rows[iCtr]["TotalMinutes"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["TotalSongs"].Value = dtDetail.Rows[iCtr]["TotalSongs"];
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells["playingType"].Value = dtDetail.Rows[iCtr]["playingType"];



                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[6].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[7].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    dgAdvt.Rows[dgAdvt.Rows.Count - 1].Cells[8].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                }

                foreach (DataGridViewRow row in dgAdvt.Rows)
                {
                    row.Height = 30;
                }

                //   GetAdvtPlayingType();

            }
            else
            {

                timGetRemainAdvtTime.Enabled = false;
            }
        }
        private void InitilizeMainAdvertisementGrid()
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


            dgAdvt.Columns.Add("Download", "Download");
            dgAdvt.Columns["Download"].Width = 200;
            dgAdvt.Columns["Download"].Visible = false;
            dgAdvt.Columns["Download"].ReadOnly = true;

            dgAdvt.Columns.Add("AdvthttpUrl", "AdvthttpUrl");
            dgAdvt.Columns["AdvthttpUrl"].Width = 200;
            dgAdvt.Columns["AdvthttpUrl"].Visible = false;
            dgAdvt.Columns["AdvthttpUrl"].ReadOnly = true;

            dgAdvt.Columns.Add("Status", "");
            dgAdvt.Columns["Status"].Width = 30;
            dgAdvt.Columns["Status"].Visible = false;
            dgAdvt.Columns["Status"].ReadOnly = true;


            dgAdvt.Columns.Add("IsVideo", "IsVideo");
            dgAdvt.Columns["IsVideo"].Width = 0;
            dgAdvt.Columns["IsVideo"].Visible = false;
            dgAdvt.Columns["IsVideo"].ReadOnly = true;


            dgAdvt.Columns.Add("IsVideoMute", "IsVideoMute");
            dgAdvt.Columns["IsVideoMute"].Width = 0;
            dgAdvt.Columns["IsVideoMute"].Visible = false;
            dgAdvt.Columns["IsVideoMute"].ReadOnly = true;

            dgAdvt.Columns.Add("IsPicture", "IsPicture");
            dgAdvt.Columns["IsPicture"].Width = 0;
            dgAdvt.Columns["IsPicture"].Visible = false;
            dgAdvt.Columns["IsPicture"].ReadOnly = true;



            dgAdvt.Columns.Add("bStime", "bStime");
            dgAdvt.Columns["bStime"].Width = 0;
            dgAdvt.Columns["bStime"].Visible = false;
            dgAdvt.Columns["bStime"].ReadOnly = true;

            dgAdvt.Columns.Add("bEtime", "bEtime");
            dgAdvt.Columns["bEtime"].Width = 0;
            dgAdvt.Columns["bEtime"].Visible = false;
            dgAdvt.Columns["bEtime"].ReadOnly = true;




            dgAdvt.Columns.Add("IsTime", "IsTime");
            dgAdvt.Columns["IsTime"].Width = 0;
            dgAdvt.Columns["IsTime"].Visible = false;
            dgAdvt.Columns["IsTime"].ReadOnly = true;

            dgAdvt.Columns.Add("IsMinute", "IsMinute");
            dgAdvt.Columns["IsMinute"].Width = 0;
            dgAdvt.Columns["IsMinute"].Visible = false;
            dgAdvt.Columns["IsMinute"].ReadOnly = true;

            dgAdvt.Columns.Add("IsSong", "IsSong");
            dgAdvt.Columns["IsSong"].Width = 0;
            dgAdvt.Columns["IsSong"].Visible = false;
            dgAdvt.Columns["IsSong"].ReadOnly = true;

            dgAdvt.Columns.Add("IsBetween", "IsBetween");
            dgAdvt.Columns["IsBetween"].Width = 0;
            dgAdvt.Columns["IsBetween"].Visible = false;
            dgAdvt.Columns["IsBetween"].ReadOnly = true;


            dgAdvt.Columns.Add("TotalMinutes", "TotalMinutes");
            dgAdvt.Columns["TotalMinutes"].Width = 0;
            dgAdvt.Columns["TotalMinutes"].Visible = false;
            dgAdvt.Columns["TotalMinutes"].ReadOnly = true;

            dgAdvt.Columns.Add("TotalSongs", "TotalSongs");
            dgAdvt.Columns["TotalSongs"].Width = 0;
            dgAdvt.Columns["TotalSongs"].Visible = false;
            dgAdvt.Columns["TotalSongs"].ReadOnly = true;


            dgAdvt.Columns.Add("playingType", "playingType");
            dgAdvt.Columns["playingType"].Width = 0;
            dgAdvt.Columns["playingType"].Visible = false;
            dgAdvt.Columns["playingType"].ReadOnly = true;

            dgAdvt.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        int IsVideoAdvt = 0;
        int IsPictureAdvt = 0;
        int IsVideoMute = 0;
        int PicturePlayTime = 0;
        DataTable dtTable = new DataTable();
        private void panAdvt_VisibleChanged(object sender, EventArgs e)
        {

        }
        private void FillPanAdvt()
        {
            string LocalCheckAdvt = "No";

            if (AdvtCurrentRow != 0)
            {
                if (AdvtCurrentRow >= dgAdvt.Rows.Count)
                {
                    AdvtCurrentRow = 0;
                }
            }
            if ((StaticClass.IsAdvtManual == true) || (StaticClass.IsAdvtWithSongs == true))
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
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    OleDbCommand cmdDelAdvt = new OleDbCommand();
                    cmdDelAdvt.Connection = StaticClass.LocalCon;
                    cmdDelAdvt.CommandText = "delete from tbAdvt  where ScheduleDate=#" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "#";
                    cmdDelAdvt.ExecuteNonQuery();


                    for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
                    {
                        dgAdvt.Rows[iRow].Cells[4].Value = "";
                    }

                }
            }



            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            OleDbCommand cmdInsertVal = new OleDbCommand();
            cmdInsertVal.Connection = StaticClass.LocalCon;
            cmdInsertVal.CommandText = "insert into tbAdvt(AdvtId,ScheduleDate) values(" + dgAdvt.Rows[AdvtCurrentRow].Cells[0].Value.ToString() + ",'" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "')";
            cmdInsertVal.ExecuteNonQuery();

            dgAdvt.Rows[AdvtCurrentRow].Cells["Play"].Value = "Done";


            lblAdvtName.Text = dgAdvt.Rows[AdvtCurrentRow].Cells["Advt"].Value.ToString();
            lblAdvtCompany.Text = dgAdvt.Rows[AdvtCurrentRow].Cells["AdvtComp"].Value.ToString();
            AdvtPlayer.URL = dgAdvt.Rows[AdvtCurrentRow].Cells["AdvtLink"].Value.ToString();
            AdvtPlayer.Ctlcontrols.play();
            AdvtPlayer.settings.volume = 0;
            if (btnMute.Text == ".")
            {
                AdvtPlayer.settings.volume = 0;
            }
            else
            {
                if (StaticClass.IsPlayerClose == "No")
                {

                    if (Convert.ToInt32(dgAdvt.Rows[AdvtCurrentRow].Cells["IsVideoMute"].Value) == 1)
                    {

                        AdvtPlayer.settings.volume = 0;
                    }
                    else
                    {

                        AdvtPlayer.settings.volume = 100;
                    }
                }
            }







            string strAdvt = "";
            strAdvt = "";
            strAdvt = strAdvt + " insert into tbTokenAdvtStatus(TokenId,AdvtId,StatusDate,StatusTime, IsUpload) values( " + StaticClass.TokenId + ", ";
            strAdvt = strAdvt + " '" + dgAdvt.Rows[AdvtCurrentRow].Cells[0].Value.ToString() + "' ,'" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "', ";
            strAdvt = strAdvt + " '" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "',0)";

            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            OleDbCommand cmdAdvtStatus = new OleDbCommand();
            cmdAdvtStatus.Connection = StaticClass.LocalCon;
            cmdAdvtStatus.CommandText = strAdvt;
            cmdAdvtStatus.ExecuteNonQuery();


            timAdvt.Enabled = true;

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

            dgGrid.Columns.Add("Advt", "Advertisement Name");
            dgGrid.Columns["Advt"].Width = 250;
            dgGrid.Columns["Advt"].Visible = true;
            dgGrid.Columns["Advt"].ReadOnly = true;

            dgGrid.Columns.Add("Type", "Type");
            dgGrid.Columns["Type"].Width = 100;
            dgGrid.Columns["Type"].Visible = true;
            dgGrid.Columns["Type"].ReadOnly = true;

            dgGrid.Columns.Add("EndDate", "Start Date");
            dgGrid.Columns["EndDate"].Width = 150;
            dgGrid.Columns["EndDate"].Visible = true;
            dgGrid.Columns["EndDate"].ReadOnly = true;

            dgGrid.Columns.Add("EndDate", "End Date");
            dgGrid.Columns["EndDate"].Width = 150;
            dgGrid.Columns["EndDate"].Visible = true;
            dgGrid.Columns["EndDate"].ReadOnly = true;

            dgGrid.Columns.Add("Time", "Time");
            dgGrid.Columns["Time"].Width = 150;
            dgGrid.Columns["Time"].Visible = true;
            dgGrid.Columns["Time"].ReadOnly = true;
            dgGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

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

                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[2].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[3].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[4].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells[5].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);

                }
                foreach (DataGridViewRow row in dgGrid.Rows)
                {
                    row.Height = 30;
                }
            }
        }

        private void picAdvtPlay_Click(object sender, EventArgs e)
        {


        }












        #endregion
        #region "Last100"



        int TotalSongPlay = 0;
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

                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                OleDbCommand cmdLast = new OleDbCommand();
                                cmdLast.Connection = StaticClass.LocalCon;
                                cmdLast.CommandText = "delete from tbLast100 where TitleId =" + Convert.ToInt32(dtGetOver.Rows[iCtr][1]); ;
                                cmdLast.ExecuteNonQuery();

                            }
                        }
                    }


                }

                if (musicPlayer1.URL != "")
                {
                    LocalTitleId = Convert.ToInt32(MusicPlayer1CurrentSongId);
                }
                else if (musicPlayer2.URL != "")
                {
                    LocalTitleId = Convert.ToInt32(MusicPlayer2CurrentSongId);
                }




                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdDelLast = new OleDbCommand();
                cmdDelLast.Connection = StaticClass.LocalCon;
                cmdDelLast.CommandText = "delete from tbLast100 where TitleId =" + LocalTitleId;
                cmdDelLast.ExecuteNonQuery();


                DataTable dtGetMaxId = new DataTable();
                dtGetMaxId = ObjMainClass.fnFillDataTable_Local("SELECT iif(IsNull(max(srNo)),0,(max(srNo))) + 1 FROM tbLast100");
                MaxTableID = Convert.ToInt32(dtGetMaxId.Rows[0][0]);

                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdSaveLast = new OleDbCommand();
                cmdSaveLast.Connection = StaticClass.LocalCon;
                cmdSaveLast.CommandText = "insert into tbLast100 (Srno, Titleid) values (" + MaxTableID + ", " + LocalTitleId + ")";
                cmdSaveLast.ExecuteNonQuery();

                DataTable dt_ID = new DataTable();
                dt_ID = ObjMainClass.fnFillDataTable_Local("SELECT ArtistID FROM Titles where TitleId=" + LocalTitleId + "");
                Int32 dt_Artist_ID = Convert.ToInt32(dt_ID.Rows[0][0]);
                dt_ID = new DataTable();
                dt_ID = ObjMainClass.fnFillDataTable_Local("SELECT splPlaylistId FROM tbSplPlaylistSchedule where PlaylistId=" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + "");
                Int32 dt_Spl_ID = Convert.ToInt32(dt_ID.Rows[0][0]);
                string strs = "";
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                cmdSaveLast = new OleDbCommand();
                cmdSaveLast.Connection = StaticClass.LocalCon;
                strs = "insert into tbTokenPlayedSongs (TokenId, playDate,playTime,TitleId,Artistid,splplaylistid,IsUpload) values (" + StaticClass.TokenId + ", #" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "#,#" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "# ," + LocalTitleId + "," + dt_Artist_ID + "," + dt_Spl_ID + ",0)";
                cmdSaveLast.CommandText = strs;
                cmdSaveLast.ExecuteNonQuery();

                strs = "";
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                cmdSaveLast = new OleDbCommand();
                cmdSaveLast.Connection = StaticClass.LocalCon;
                strs = "insert into tbTokenOverDueStatus (TokenId, StatusDate,StatusTime,IsUpload) values (" + StaticClass.TokenId + ", #" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "#,#" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "#,0)";
                cmdSaveLast.CommandText = strs;
                cmdSaveLast.ExecuteNonQuery();



                if (lblSongCount.Text != "2")
                {
                    TotalSongPlay = TotalSongPlay + 1;
                }


            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
        }



        private void GetOldSongIdPlayer1()
        {
            Int32 Locali = 0;
            Boolean SongFind = true;
            try
            {

                drawLine = false;
                dgPlaylist.Invalidate();
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() != MusicPlayer1CurrentSongId.ToString())
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
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() != MusicPlayer2CurrentSongId.ToString())
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


        #endregion
        #region "SetDefaultPlaylist"

        DataGridViewCell ActiveCell = null;
        private void dgLocalPlaylist_MouseClick(object sender, MouseEventArgs e)
        {
            if (StaticClass.IsLock == true) return;
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
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    MessageBox.Show("Please check your Internet connection.", "Player");
                    return;
                }
            mlsSql = "SELECT  count(*) from TitlesInPlaylists";
            mlsSql = mlsSql + " where PlaylistID=" + Convert.ToInt32(ActiveCell.Value.ToString());
            dtGetRecords = ObjMainClass.fnFillDataTable_Local(mlsSql);
            if (Convert.ToInt32(dtGetRecords.Rows[0][0]) <= 0)
            {
                MessageBox.Show("To set for default playlist you need to add minimum 1 song in this playlist.", "Player");
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
            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            OleDbCommand cmdUpdateAll = new OleDbCommand();
            cmdUpdateAll.Connection = StaticClass.LocalCon;
            cmdUpdateAll.CommandText = "Update Playlists set PlaylistDefault=''";
            cmdUpdateAll.ExecuteNonQuery();


            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            OleDbCommand cmdUpdate = new OleDbCommand();
            cmdUpdate.Connection = StaticClass.LocalCon;
            cmdUpdate.CommandText = "Update Playlists set PlaylistDefault='Default' where playlistid = " + ActiveCell.Value.ToString();
            cmdUpdate.ExecuteNonQuery();


            PopulateInputFileTypeDetail(dgPlaylist, Convert.ToInt32(ActiveCell.Value.ToString()));

            dgPlaylist.Visible = true;

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
            if (musicPlayer1.URL != "")
            {
                if (MusicPlayer1CurrentSongId.ToString() == dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString())
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
                if (MusicPlayer2CurrentSongId.ToString() == dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString())
                {
                    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(1)].Cells[0].Value.ToString());
                    CurrentRow = 0;
                }
                else
                {
                    NextSongDisplay2(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                }
            }
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[2].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1].Style.SelectionBackColor = Color.Yellow;
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[1].Style.SelectionForeColor = Color.Black;
            //dgLocalPlaylist.Rows[CurrentPlaylistRow].Cells[2].Style.SelectionBackColor = Color.Yellow;
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

                    row.Cells[1].Style.Font = new Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                    row.Cells[1].Style.SelectionForeColor = Color.Yellow;
                    row.Cells[1].Style.ForeColor = Color.FromArgb(20, 162, 175);
                    row.Cells[3].Style.SelectionBackColor = Color.LightBlue;
                    row.Cells[3].Style.BackColor = Color.LightBlue;
                }
                else
                {
                    row.Cells[1].Style.ForeColor = Color.FromArgb(0, 0, 0);
                    row.Cells[1].Style.SelectionForeColor = Color.White;
                    row.Cells[1].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                    row.Cells[3].Style.BackColor = Color.White;
                    row.Cells[3].Style.SelectionBackColor = Color.White;

                }
            }
        }

        string IsCopyFromLocalList = "No";
        private void dgOtherPlaylist_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {

        }



        private void dgOtherPlaylist_DragDrop(object sender, DragEventArgs e)
        {

        }

        private void dgOtherPlaylist_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void dgOtherPlaylist_DragLeave(object sender, EventArgs e)
        {

        }

        private void dgOtherPlaylist_DragOver(object sender, DragEventArgs e)
        {

        }

        private void dgOtherPlaylist_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void dgOtherPlaylist_MouseLeave(object sender, EventArgs e)
        {

        }

        private void dgOtherPlaylist_Paint(object sender, PaintEventArgs e)
        {

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
        int rowIndexFromMouseDown;
        DataGridViewRow rw;
        Boolean StopDup = false;
        private void dgAdvt_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
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

        private void dgAdvt_DragDrop(object sender, DragEventArgs e)
        {

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

        private void dgAdvt_DragEnter(object sender, DragEventArgs e)
        {
            if (dgAdvtComman.SelectedRows.Count > 0)
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void dgAdvt_DragLeave(object sender, EventArgs e)
        {
            //  drawLine = false;
            //  dgAdvt.Invalidate();
        }

        private void dgAdvt_DragOver(object sender, DragEventArgs e)
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
                Console.WriteLine(ex.Message);
            }
        }

        private void dgAdvt_MouseLeave(object sender, EventArgs e)
        {
            //drawLine = false;
            //dgAdvt.Invalidate();
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

        private void dgAdvt_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            StopDup = true;
        }


        private void timGetRemainAdvtTime_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (panAdvt.Visible == false)
                //{
                //    if (IsAdvtTimeGet == false)
                //    {
                //        Int32 TempTime = 0;
                //        TempTime = GetTotalAdvtTime(rCount);
                //        if (TempTime < StaticClass.AdvtTime)
                //        {
                //            rCount = rCount + 1;
                //            TempTime = GetTotalAdvtTime(rCount);
                //        }
                //        if (TempTime >= StaticClass.AdvtTime)
                //        {
                //            rCount = 0;
                //            IsAdvtTimeGet = true;
                //        }
                //        GrossTotaltime = TempTime;
                //        if (musicPlayer1.URL != "")
                //        {
                //            GrossTotaltime = GrossTotaltime - Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
                //        }
                //        else if (musicPlayer2.URL != "")
                //        {
                //            GrossTotaltime = GrossTotaltime - Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);
                //        }
                //    }
                //}


                //GrossTotaltime = GrossTotaltime - 1;
                //double t1 = Math.Floor(GrossTotaltime);
                ////double w1 = Math.Floor(timeLeft);
                //double mint1 = Math.Floor(t1 / 60);
                //double s1;
                //int r1;
                //s1 = Convert.ToInt16(Math.Abs(t1 / 60));
                //r1 = Convert.ToInt16(t1 % 60);
                ////--------------------------------------------//
                ////--------------------------------------------//

                //double fd;
                //fd = Math.Floor(GrossTotaltime);
                //double zh;
                //zh = fd / 60;
                //double left = System.Math.Floor(zh);
                //double sec2 = fd % 60;
                ////--------------------------------------------//
                ////--------------------------------------------//

                //lblAdvtTimeRemain.Text = mint1.ToString("00") + ":" + r1.ToString("00");
                //lblAdvtTimeRemain.Text = GrossTotaltime.ToString() ;

                if (musicPlayer1.URL != "")
                {
                    lblAdvtTimeRemain.Text = lblMusicTimeOne.Text;
                }
                if (musicPlayer2.URL != "")
                {
                    lblAdvtTimeRemain.Text = lblMusicTimeTwo.Text;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public Int32 GetTotalAdvtTime(int numberTotal)
        {
            //label18.Text = "0";
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
                        SongName = MusicPlayer2CurrentSongId.ToString();
                    }
                    else if (musicPlayer1.URL != "")
                    {
                        SongName = MusicPlayer1CurrentSongId.ToString();
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
                                SongName = MusicPlayer2CurrentSongId.ToString();
                            }
                            else if (musicPlayer1.URL != "")
                            {
                                SongName = MusicPlayer1CurrentSongId.ToString();
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
                    label18.Text = (Convert.ToInt32(label18.Text) + ((Convert.ToInt32(label8.Text)))).ToString();
                }
                else
                {
                    label8.Text = dgPlaylist.Rows[Convert.ToInt32(CurrentRow)].Cells["Length"].Value.ToString();
                    string strCurrent = label8.Text.ToString();
                    string[] arr2 = strCurrent.Split(':');
                    label18.Text = (Convert.ToInt32(label18.Text) + ((Convert.ToInt32(arr2[0]) * 60)) + Convert.ToInt32(arr2[1])).ToString();
                }
                if (Convert.ToInt32(label18.Text) >= 300)
                {
                    return Convert.ToInt32(label18.Text);
                }

            }
            string strNext = label7.Text.ToString();
            string[] arr = strNext.Split(':');
            label18.Text = (Convert.ToInt32(label18.Text) + ((Convert.ToInt32(arr[0]) * 60)) + Convert.ToInt32(arr[1])).ToString();
            return Convert.ToInt32(label18.Text);
        }




        private void dgAdvt_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {

        }





        private void FillAllAdvertisement()
        {

            string str = "";
            int iCtr;
            string bTime = "";
            string lPath = "";
            DataTable dtDetail;
            str = "select * from tbAdvertisement where #" + string.Format("{0:dd/MMM/yyyy}", DateAndTime.Now.Date) + "# between AdvtStartDate and AdvtEndDate order by srno";
            dtDetail = ObjMainClass.fnFillDataTable_Local(str);
            InitilizeAdvertisementGrid();
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgAdvtComman.Rows.Add();
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Advtid"].Value = dtDetail.Rows[iCtr]["AdvtId"];
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Advt"].Value = dtDetail.Rows[iCtr]["AdvtDisplayName"];
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["AdvtComp"].Value = dtDetail.Rows[iCtr]["AdvtCompanyName"];
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["AdvtLink"].Value = dtDetail.Rows[iCtr]["AdvtFilePath"];// Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".mp4";
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Play"].Value = "";
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Type"].Value = dtDetail.Rows[iCtr]["AdvtTypeName"];
                    if (Convert.ToInt32(dtDetail.Rows[iCtr]["IsVideo"]) == 1)
                    {
                        dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["pMode"].Value = "Video";
                    }
                    else if (Convert.ToInt32(dtDetail.Rows[iCtr]["IsPicture"]) == 1)
                    {
                        dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["pMode"].Value = "Picture";
                    }
                    else
                    {
                        dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["pMode"].Value = "Audio";
                    }
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["StartDate"].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtStartDate"]);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["EndDate"].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtEndDate"]);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["AdvtTime"].Value = string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["AdvtTime"]);
                    if (dtDetail.Rows[iCtr]["IsMinute"].ToString().Trim() == "1")
                    {
                        dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["AdvtTime"].Value = "After " + dtDetail.Rows[iCtr]["TotalMinutes"].ToString().Trim() + " min";
                    }
                    if (dtDetail.Rows[iCtr]["IsSong"].ToString().Trim() == "1")
                    {
                        dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["AdvtTime"].Value = "After " + dtDetail.Rows[iCtr]["TotalSongs"].ToString().Trim() + " songs";
                    }
                    if (dtDetail.Rows[iCtr]["IsBetween"].ToString().Trim() == "1")
                    {
                        bTime = string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["bStime"]) + "-" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["bEtime"]);
                        dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["AdvtTime"].Value = bTime;
                    }

                    if (Convert.ToInt32(dtDetail.Rows[iCtr]["IsVideo"]) == 1)
                    {
                        lPath = Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".mp4";
                    }
                    else if (Convert.ToInt32(dtDetail.Rows[iCtr]["IsPicture"]) == 1)
                    {
                        lPath = Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".jpg";
                    }
                    else
                    {
                        lPath = Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".mp3";
                    }
                    if (System.IO.File.Exists(lPath))
                    {
                        Int32 AdsSize = 0;
                        if (string.IsNullOrEmpty(dtDetail.Rows[iCtr]["FileSize"].ToString()) == false)
                        {
                            AdsSize = Convert.ToInt32(dtDetail.Rows[iCtr]["FileSize"]);
                        }
                        else
                        {
                            AdsSize = 0;
                        }
                        long DownloadedSize = new FileInfo(lPath).Length;
                        if (DownloadedSize == AdsSize)
                        {
                            dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Download"].Value = "Yes";
                            dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Status"].Style.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Download"].Value = "No";
                            dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Status"].Style.BackColor = Color.LightSalmon;
                        }

                    }
                    else
                    {
                        dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Download"].Value = "No";
                        dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Status"].Style.BackColor = Color.LightSalmon;
                    }
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["AdvthttpUrl"].Value = dtDetail.Rows[iCtr]["AdvthttpUrl"];

                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Advt"].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["Type"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["pMode"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["StartDate"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["EndDate"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                    dgAdvtComman.Rows[dgAdvtComman.Rows.Count - 1].Cells["AdvtTime"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
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

            dgAdvtComman.Columns.Add("Advt", "Advt Name");
            dgAdvtComman.Columns["Advt"].Width = 320;
            dgAdvtComman.Columns["Advt"].Visible = true;
            dgAdvtComman.Columns["Advt"].ReadOnly = true;
            dgAdvtComman.Columns["Advt"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

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
            dgAdvtComman.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgAdvtComman.Columns.Add("pMode", "");
            dgAdvtComman.Columns["pMode"].Width = 100;
            dgAdvtComman.Columns["pMode"].Visible = true;
            dgAdvtComman.Columns["pMode"].ReadOnly = true;
            dgAdvtComman.Columns["pMode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgAdvtComman.Columns.Add("StartDate", "Start Date");
            dgAdvtComman.Columns["StartDate"].Width = 150;
            dgAdvtComman.Columns["StartDate"].Visible = true;
            dgAdvtComman.Columns["StartDate"].ReadOnly = true;
            dgAdvtComman.Columns["StartDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            dgAdvtComman.Columns.Add("EndDate", "End Date");
            dgAdvtComman.Columns["EndDate"].Width = 150;
            dgAdvtComman.Columns["EndDate"].Visible = true;
            dgAdvtComman.Columns["EndDate"].ReadOnly = true;
            dgAdvtComman.Columns["EndDate"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;


            dgAdvtComman.Columns.Add("AdvtTime", "");
            dgAdvtComman.Columns["AdvtTime"].Width = 150;
            dgAdvtComman.Columns["AdvtTime"].Visible = true;
            dgAdvtComman.Columns["AdvtTime"].ReadOnly = true;
            dgAdvtComman.Columns["AdvtTime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;



            dgAdvtComman.Columns.Add("Download", "Download");
            dgAdvtComman.Columns["Download"].Width = 0;
            dgAdvtComman.Columns["Download"].Visible = false;
            dgAdvtComman.Columns["Download"].ReadOnly = true;

            dgAdvtComman.Columns.Add("AdvthttpUrl", "AdvthttpUrl");
            dgAdvtComman.Columns["AdvthttpUrl"].Width = 0;
            dgAdvtComman.Columns["AdvthttpUrl"].Visible = false;
            dgAdvtComman.Columns["AdvthttpUrl"].ReadOnly = true;



            dgAdvtComman.Columns.Add("Status", "");
            dgAdvtComman.Columns["Status"].Width = 30;
            dgAdvtComman.Columns["Status"].Visible = true;
            dgAdvtComman.Columns["Status"].ReadOnly = true;
            dgAdvtComman.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;


            //dgAdvtComman.Columns.Add("Type", "");
            //dgAdvtComman.Columns["Type"].Width = 30;
            //dgAdvtComman.Columns["Type"].Visible = true;
            //dgAdvtComman.Columns["Type"].ReadOnly = true;
            //dgAdvtComman.Columns["Type"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            //dgAdvtComman.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }



        private void SaveSongSequence(DataGridView dgGrid)
        {
            string sWr = "";
            if (dgGrid.Rows.Count == 0) return;
            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            OleDbCommand cmdDelAdvt = new OleDbCommand();
            cmdDelAdvt.Connection = StaticClass.LocalCon;
            cmdDelAdvt.CommandText = "delete from TitlesInPlaylists  where PlaylistID= " + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + "";
            cmdDelAdvt.ExecuteNonQuery();

            int Srno = 0;
            for (int i = 0; i <= dgGrid.Rows.Count - 1; i++)
            {
                Srno = Srno + 1;
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                sWr = "insert into TitlesInPlaylists(PlaylistID,TitleID,SrNo) values (" + Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value) + " , ";
                sWr = sWr + Convert.ToInt32(dgGrid.Rows[i].Cells[0].Value) + " , " + Srno + ")";

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
                label18.Text = "0";
                IsAdvtTimeGet = false;
                GrossTotaltime = 0;
                // timGetRemainAdvtTime.Enabled = true;
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
                            if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(0)].Cells[0].Value.ToString());
                            }
                            else
                            {
                                NextSongDisplay(dgPlaylist.Rows[Convert.ToInt32(LastRowId + 1)].Cells[0].Value.ToString());
                            }
                        }
                        else if (CurrentRow >= dgPlaylist.Rows.Count - 1)
                        {
                            if (LastRowId + 1 >= dgPlaylist.Rows.Count - 1)
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
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer2CurrentSongId.ToString())
                    {
                        CurrentRow = i;
                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            IsVisibleSong = true;
                            UpdateHideSong(MusicPlayer2CurrentSongId.ToString());
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
                        //panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));

                        lblSongName.ForeColor = Color.Gray;
                        lblArtistName.ForeColor = Color.Gray;
                        lblMusicTimeOne.ForeColor = Color.Gray;
                        lblSongDurationOne.ForeColor = Color.Gray;
                        pbarMusic1.ForeColor = Color.Gray;
                        pbarMusic1.BackColor = Color.FromArgb(175, 175, 175);
                        //panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                        if (dgHideSongs.Rows.Count > 0)
                        {
                            DeleteParticularHideSong();
                        }
                        dgPlaylist.ClearSelection();
                        break;
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
                for (int i = 0; i < dgPlaylist.Rows.Count; i++)
                {
                    if (dgPlaylist.Rows[i].Cells[0].Value.ToString() == MusicPlayer1CurrentSongId.ToString())
                    {
                        CurrentRow = i;
                        if (dgPlaylist.Rows[i].Visible == false)
                        {
                            IsVisibleSong = true;
                            UpdateHideSong(MusicPlayer1CurrentSongId.ToString());
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
                        //panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.CurrentPlayer));
                        pbarMusic1.BackColor = Color.FromArgb(9, 130, 154);

                        lblSongName2.ForeColor = Color.Gray;
                        lblArtistName2.ForeColor = Color.Gray;
                        lblMusicTimeTwo.ForeColor = Color.Gray;
                        lblSongDurationTwo.ForeColor = Color.Gray;
                        pbarMusic2.ForeColor = Color.Gray;
                        pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
                        //panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
                        if (dgHideSongs.Rows.Count > 0)
                        {
                            DeleteParticularHideSong();
                        }
                        dgPlaylist.ClearSelection();
                        break;
                    }
                }

            }
            catch
            {
            }
        }

        private void GetCurrentRow()
        {
            string SongName = "";
            if (musicPlayer2.URL != "")
            {
                SongName = MusicPlayer2CurrentSongId.ToString();
            }
            else if (musicPlayer1.URL != "")
            {
                SongName = MusicPlayer1CurrentSongId.ToString();
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



        private void timCurrentTime_Tick(object sender, EventArgs e)
        {

        }







        private void AddSongsInGrid(DataGridView dgGrid, Int32 TitleSongId)
        {
            string mlsSql = "";
            string GetLocalPath = "";
            string TitleYear = "";
            string TitleTime = "";
            var Special_Name = "";
            string Special_Change = "";
            int iCtr = 0;
            DataTable dtDetail;
            mlsSql = "SELECT  Titles.TitleID, ltrim(Titles.Title) as Title, Titles.Time,Albums.Name AS AlbumName ,";
            mlsSql = mlsSql + " Titles.TitleYear as TitleYear ,  ltrim(Artists.Name) as ArtistName  FROM ((( TitlesInPlaylists  ";
            mlsSql = mlsSql + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID )  ";
            mlsSql = mlsSql + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ) ";
            mlsSql = mlsSql + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ) ";
            mlsSql = mlsSql + " where Titles.TitleID=" + Convert.ToInt32(TitleSongId);
            dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
            if ((dtDetail.Rows.Count > 0))
            {
                //GetLocalPath = dtDetail.Rows[iCtr]["TitleID"] + ".mp4";
                iCtr = 0;
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

                dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["songname"].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Length"].Style.Font = new Font("Segoe UI", 9);
                dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Album"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Artist"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);


            }
        }





        WebClient myWebClientAdvt = new WebClient();


        private void myWebClientAdvt_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            //textBox1.Text = e.ProgressPercentage.ToString() + " %";
        }

        private void myWebClientAdvt_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            GC.Collect();
            //progressBar1.Value= 0;
            //lblPercentage.Text = "";
            FillAllAdvertisement();
            FillMainAdvertisement();
            // progressBar1.Value = 0;
            if (ObjMainClass.CheckForInternetConnection() == true)
            {
                for (int iAdvt = 0; iAdvt < dgAdvt.Rows.Count; iAdvt++)
                {
                    if (dgAdvt.Rows[iAdvt].Cells["Download"].Value.ToString() == "No")
                    {
                        DownloadAdvt();
                        break;
                    }
                }
            }
        }




        private void DownloadAdvt()
        {
            if (myWebClientAdvt.IsBusy == false)
            {
                for (int iAdvt = 0; iAdvt < dgAdvt.Rows.Count; iAdvt++)
                {
                    if (dgAdvt.Rows[iAdvt].Cells["Download"].Value.ToString() == "No")
                    {
                        AdvtUrl = dgAdvt.Rows[iAdvt].Cells["AdvthttpUrl"].Value.ToString();
                        AdvtFilePath = dgAdvt.Rows[iAdvt].Cells["AdvtLink"].Value.ToString();
                        //dgAdvt.Rows[iAdvt].Cells["Download"].Value = "Yes";
                        if (myWebClientAdvt.IsBusy == false)
                        {
                            myWebClientAdvt.DownloadFileAsync(new Uri(AdvtUrl), AdvtFilePath);
                            break;
                        }
                    }
                }
            }



        }
        Int32 NetChecker = 10;
        private void timNetChecker_Tick(object sender, EventArgs e)
        {
        CheckNet:
            NetChecker = NetChecker - 1;
            if (NetChecker == 0)
            {
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    NetChecker = 10;
                    goto CheckNet;
                }
                //if (panAdvt.Visible == true)
                //{
                //    NetChecker = 20;
                //    goto CheckNet;
                //}
                if (IsPrayerRunning == "Yes")
                {
                    NetChecker = 30;
                    goto CheckNet;
                }
                Application.Restart();
                //delete_temp_table();
                //DeleteHideSongs();
                //FillDamGenre();
                //FillStreamData("1");
                //FillDamGenreTitles(Convert.ToInt32(dgDam.Rows[0].Cells[0].Value));
                //timNetChecker.Enabled = false;
            }
        }
        public static bool TableExists(string table)
        {
            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            return StaticClass.LocalCon.GetSchema("Tables", new string[4] { null, null, table, "TABLE" }).Rows.Count > 0;
        }
        private void UpdateLocalDatabase()
        {
            string strInsert = "";
            if (TableExists("tbMusicLastSettings") == false)
            {
                strInsert = "CREATE TABLE tbMusicLastSettings([player_setting_id] number NULL,[DFClientId] number NULL, 	[localUserId] number NULL ,";
                strInsert = strInsert + " [lastPlaylistId] number NULL, 	[lastTileId] number NULL, [lastVolume] number NULL, 	[lastSongDuration] number NULL,";
                strInsert = strInsert + "[IsFade] number NULL, 	[IsShuffle] number NULL,[TokenNo] number NULL) ";
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
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
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
            }
            if (TableExists("tbPrayer") == false)
            {
                strInsert = "";
                strInsert = "CREATE TABLE tbPrayer([pId] number NULL,[sDate] Date NULL, 	[eDate] Date NULL , [sTime] Time NULL, [eTime] Time NULL)";
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdTitle = new OleDbCommand();
                cmdTitle.Connection = StaticClass.LocalCon;
                cmdTitle.CommandText = strInsert;
                cmdTitle.ExecuteNonQuery();
            }
        }
        private void GetAdvertisement()
        {

            string strInsert = "";
            string str = "";
            string aId = "";
            string lPath = "";


            str = "spGetAdvtAdmin_NativeOnly '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "','NativeCR'," + StaticClass.dfClientId + "," + ReturnWeekId(DateTime.Now.DayOfWeek.ToString()) + ", " + StaticClass.AdvtCityId + "," + StaticClass.dfClientId + " , " + StaticClass.CountryId + ", " + StaticClass.Stateid + "," + StaticClass.TokenId + "";
            DataTable dtDetail = ObjMainClass.fnFillDataTable(str);
            if ((dtDetail.Rows.Count > 0))
            {
                for (int iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    if (aId == "")
                    {
                        aId = dtDetail.Rows[iCtr]["AdvtId"].ToString();
                    }
                    else
                    {
                        aId = aId + "," + dtDetail.Rows[iCtr]["AdvtId"].ToString();
                    }
                }
            }

            try
            {

                str = "";
                str = "select * from tbAdvertisement where Advtid not in (" + aId + ") ";
                DataTable dtLoc = new DataTable();
                dtLoc = ObjMainClass.fnFillDataTable_Local(str);
                if (dtLoc.Rows.Count > 0)
                {
                    for (int iCtr = 0; (iCtr <= (dtLoc.Rows.Count - 1)); iCtr++)
                    {
                        lPath = Application.StartupPath + "\\Advt\\" + dtLoc.Rows[iCtr]["AdvtId"] + ".mp4";
                        if (System.IO.File.Exists(lPath))
                        {
                            File.Delete(lPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                goto OutQ;
            }
        OutQ:
            strInsert = "";
            lPath = "";
            strInsert = "delete from tbAdvertisement ";
            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            OleDbCommand cmdDel = new OleDbCommand();
            cmdDel.Connection = StaticClass.LocalCon;
            cmdDel.CommandText = strInsert;
            cmdDel.ExecuteNonQuery();
            if ((dtDetail.Rows.Count > 0))
            {
                for (int iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {

                    if (Convert.ToInt32(dtDetail.Rows[iCtr]["IsVideo"]) == 1)
                    {
                        if (StaticClass.IsVedioActive == true)
                        {
                            lPath = Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".mp4";
                            strInsert = "";
                            strInsert = "insert into tbAdvertisement(AdvtId,AdvtDisplayName,AdvtCompanyName,AdvtStartDate,AdvtEndDate,AdvtFilePath,AdvtPlayertype, ";
                            strInsert = strInsert + " DfClientId,CountryCode,TokenId,AdvtTypeId,AdvtTime ,StateId,CityId,Dealercode, AdvtTypeName,AdvthttpUrl, IsTime,IsMinute ,IsSong , TotalMinutes,TotalSongs,SrNo,IsVideo, IsVideoMute,IsPicture,IsBetween, bStime,bEtime ,playingType,filesize) values (";
                            strInsert = strInsert + " " + dtDetail.Rows[iCtr]["AdvtId"] + ", '" + dtDetail.Rows[iCtr]["AdvtDisplayName"] + "', ";
                            strInsert = strInsert + " '" + dtDetail.Rows[iCtr]["AdvtCompanyName"] + "','" + string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtStartDate"]) + "' ,";
                            strInsert = strInsert + " '" + string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtEndDate"]) + "','" + lPath + "',";
                            strInsert = strInsert + " 'NativeCR', " + StaticClass.dfClientId + "," + StaticClass.CountryId + ",";
                            strInsert = strInsert + " " + StaticClass.TokenId + ",1,'" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["AdvtTime"]) + "', ";
                            strInsert = strInsert + " " + StaticClass.Stateid + "," + StaticClass.AdvtCityId + ",'" + StaticClass.DealerCode + "',";
                            strInsert = strInsert + " '" + dtDetail.Rows[iCtr]["AdvtTypeName"] + "', '" + dtDetail.Rows[iCtr]["AdvtFilePath"] + "' ,";
                            strInsert = strInsert + " " + Convert.ToInt32(dtDetail.Rows[iCtr]["IsTime"]) + ", " + Convert.ToInt32(dtDetail.Rows[iCtr]["IsMinute"]) + " , " + Convert.ToInt32(dtDetail.Rows[iCtr]["IsSong"]) + " , ";
                            strInsert = strInsert + " " + dtDetail.Rows[iCtr]["TotalMinutes"] + " , " + dtDetail.Rows[iCtr]["TotalSongs"] + "," + dtDetail.Rows[iCtr]["Srno"] + ", ";
                            strInsert = strInsert + Convert.ToInt32(dtDetail.Rows[iCtr]["IsVideo"]) + ", " + Convert.ToInt32(dtDetail.Rows[iCtr]["IsVideoMute"]) + " ,0 ";
                            strInsert = strInsert + " ," + Convert.ToInt32(dtDetail.Rows[iCtr]["IsBetween"]) + ",'" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["bStime"]) + "'";
                            strInsert = strInsert + " ,'" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["bEtime"]) + "','" + dtDetail.Rows[iCtr]["playingType"] + "','" + dtDetail.Rows[iCtr]["FileSize"] + "')";

                            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                            OleDbCommand cmdSave = new OleDbCommand();
                            cmdSave.Connection = StaticClass.LocalCon;
                            cmdSave.CommandText = strInsert;
                            cmdSave.ExecuteNonQuery();
                        }
                    }
                    else if (Convert.ToInt32(dtDetail.Rows[iCtr]["IsPicture"]) == 1)
                    {
                    }
                    else
                    {
                        lPath = Application.StartupPath + "\\Advt\\" + dtDetail.Rows[iCtr]["AdvtId"] + ".mp3";
                        strInsert = "";
                        strInsert = "insert into tbAdvertisement(AdvtId,AdvtDisplayName,AdvtCompanyName,AdvtStartDate,AdvtEndDate,AdvtFilePath,AdvtPlayertype, ";
                        strInsert = strInsert + " DfClientId,CountryCode,TokenId,AdvtTypeId,AdvtTime ,StateId,CityId,Dealercode, AdvtTypeName,AdvthttpUrl, IsTime,IsMinute ,IsSong , TotalMinutes,TotalSongs,SrNo,IsVideo,IsVideoMute,IsPicture,IsBetween, bStime,bEtime ,playingType,filesize) values (";
                        strInsert = strInsert + " " + dtDetail.Rows[iCtr]["AdvtId"] + ", '" + dtDetail.Rows[iCtr]["AdvtDisplayName"] + "', ";
                        strInsert = strInsert + " '" + dtDetail.Rows[iCtr]["AdvtCompanyName"] + "','" + string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtStartDate"]) + "' ,";
                        strInsert = strInsert + " '" + string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["AdvtEndDate"]) + "','" + lPath + "',";
                        strInsert = strInsert + " 'NativeCR', " + StaticClass.dfClientId + "," + StaticClass.CountryId + ",";
                        strInsert = strInsert + " " + StaticClass.TokenId + ",1,'" + string.Format("{0:hh:mm tt}", dtDetail.Rows[iCtr]["AdvtTime"]) + "', ";
                        strInsert = strInsert + " " + StaticClass.Stateid + "," + StaticClass.AdvtCityId + ",'" + StaticClass.DealerCode + "',";
                        strInsert = strInsert + " '" + dtDetail.Rows[iCtr]["AdvtTypeName"] + "', '" + dtDetail.Rows[iCtr]["AdvtFilePath"] + "' ,";
                        strInsert = strInsert + " " + Convert.ToInt32(dtDetail.Rows[iCtr]["IsTime"]) + ", " + Convert.ToInt32(dtDetail.Rows[iCtr]["IsMinute"]) + " , " + Convert.ToInt32(dtDetail.Rows[iCtr]["IsSong"]) + " , ";
                        strInsert = strInsert + " " + dtDetail.Rows[iCtr]["TotalMinutes"] + " , " + dtDetail.Rows[iCtr]["TotalSongs"] + "," + dtDetail.Rows[iCtr]["Srno"] + ", ";
                        strInsert = strInsert + "0,0,0 ";
                        strInsert = strInsert + " ," + Convert.ToInt32(dtDetail.Rows[iCtr]["IsBetween"]) + ",'" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["bStime"]) + "'";
                        strInsert = strInsert + " ,'" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["bEtime"]) + "','" + dtDetail.Rows[iCtr]["playingType"] + "','" + dtDetail.Rows[iCtr]["FileSize"] + "')";

                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdSave = new OleDbCommand();
                        cmdSave.Connection = StaticClass.LocalCon;
                        cmdSave.CommandText = strInsert;
                        cmdSave.ExecuteNonQuery();

                    }
                }
            }

            GetAdvtPlayingType();

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
        #region Store and Forward
        int sTime = 1;
        Boolean IsDeleteAllOgg = false;
        string SplSongName = "";
        string SplSongUrl = "";
        private void timGetSplPlaylist_Tick(object sender, EventArgs e)
        {
        upAgain:
            FirstTimeSong = true;
            //sTime = 1;
            DataTable dtDetailNew = new DataTable();
            string strNew = "";
            sTime = sTime - 1;
            if (sTime == 0)
            {
                if (ObjMainClass.CheckForInternetConnection() == true)
                {
                    picSplGif.Visible = true;

                    var weekNo = (int)DateTime.Now.DayOfWeek;
                    strNew = "GetSpecialTempPlaylistSchedule " + weekNo + ", " + StaticClass.TokenId + " ," + StaticClass.dfClientId + " ,'" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(DateTime.Now)) + "'";
                    // strNew = "GetSpecialPlaylistSchedule 'NativeCR'," + weekNo + ", " + StaticClass.TokenId + " ,60583";
                    dtDetailNew = ObjMainClass.fnFillDataTable(strNew);
                    if ((dtDetailNew.Rows.Count <= 0))
                    {
                        picSplGif.Visible = false;
                        //  DownloadAdvt();


                    }
                    else
                    {
                        timGetSplPlaylist.Enabled = false;
                        bgGetSplPlaylist.RunWorkerAsync();
                    }
                    timGetSplPlaylist.Enabled = false;

                }
                else
                {
                    sTime = 10;
                    goto upAgain;
                }


            }
        }

        private void bgGetSplPlaylist_DoWork(object sender, DoWorkEventArgs e)
        {
            string strNew = "";
            string strInsert = "";
            string LocalSchId = "";
            string PlayId = "";
            string Special_Name = "";
            string Special_Change = "";
            Int32 LocalpSchId = 0;
            Int32 LocalSplId = 0;
            DataTable dtDetailNew;
            DataTable dtDetail;
            DataTable dtDelete;
            DataTable dtGetRecord;
            DataTable dtGet24Hr;
            DataTable dtCheck24Hr;
            string strGet = "";
            int IsLocalSplSong = 0;
            try
            {
                IsDeleteAllOgg = false;
                #region Get Spl Playlist
                dtDetailNew = new DataTable();
                strNew = "";
                var weekNo = (int)DateTime.Now.DayOfWeek;
                strNew = "GetSpecialTempPlaylistSchedule " + weekNo + ", " + StaticClass.TokenId + " ," + StaticClass.dfClientId + " ,'" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(DateTime.Now)) + "'";

                // strNew = "GetSpecialPlaylistSchedule 'NativeCR'," + weekNo + ", " + StaticClass.TokenId + " ,60583";
                dtDetailNew = ObjMainClass.fnFillDataTable(strNew);
                if ((dtDetailNew.Rows.Count > 0))
                {



                    #region Delete Songs
                    for (int iDel = 0; (iDel <= (dtDetailNew.Rows.Count - 1)); iDel++)
                    {
                        if (LocalSchId == "")
                        {
                            LocalSchId = dtDetailNew.Rows[iDel]["pSchId"].ToString();
                        }
                        else
                        {
                            LocalSchId = LocalSchId + "," + dtDetailNew.Rows[iDel]["pSchId"].ToString();
                        }
                    }
                    //strNew = "";
                    //strNew = "select * from tbSpecialPlaylists_Titles ";
                    //strNew = strNew + " where SchId not in (" + LocalSchId + ")";
                    //dtDetail = new DataTable();
                    //dtDetail = ObjMainClass.fnFillDataTable_Local(strNew);
                    //for (int iDelT = 0; (iDelT <= (dtDetail.Rows.Count - 1)); iDelT++)
                    //{
                    //    if (File.Exists(Application.StartupPath + "\\so\\" + Convert.ToInt32(dtDetail.Rows[iDelT]["titleId"]) + ".mp4"))
                    //    {
                    //        File.Delete(Application.StartupPath + "\\so\\" + Convert.ToInt32(dtDetail.Rows[iDelT]["titleId"]) + ".mp4");
                    //    }
                    //}
                    #endregion
                    #region Delete exixts records
                    strNew = "";
                    strNew = "select * from Playlists ";
                    dtDelete = ObjMainClass.fnFillDataTable_Local(strNew);

                    for (int iDel = 0; (iDel <= (dtDelete.Rows.Count - 1)); iDel++)
                    {
                        if (PlayId == "")
                        {
                            PlayId = dtDelete.Rows[iDel]["PlaylistId"].ToString();
                        }
                        else
                        {
                            PlayId = PlayId + "," + dtDelete.Rows[iDel]["PlaylistId"].ToString();
                        }
                    }
                    if (PlayId != "")
                    {
                        try
                        {
                            if (ObjMainClass.CheckForInternetConnection() == false) { IsDeleteAllOgg = true; return; }
                            if (StaticClass.constr.State == ConnectionState.Open) { StaticClass.constr.Close(); }
                            StaticClass.constr.Open();
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = StaticClass.constr;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "delete from TitlesInPlaylists where Playlistid in(select Playlistid from Playlists where Playlistid not in(" + PlayId + ") and tokenid = " + StaticClass.TokenId + ")";
                            cmd.ExecuteNonQuery();

                            if (ObjMainClass.CheckForInternetConnection() == false) { IsDeleteAllOgg = true; return; }
                            if (StaticClass.constr.State == ConnectionState.Open) { StaticClass.constr.Close(); }
                            StaticClass.constr.Open();
                            cmd = new SqlCommand();
                            cmd.Connection = StaticClass.constr;
                            cmd.CommandType = CommandType.Text;
                            cmd.CommandText = "delete from Playlists where Playlistid not in(" + PlayId + ") and tokenid = " + StaticClass.TokenId;
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception ex) { }


                        strInsert = "";
                        strInsert = "delete from Playlists where Playlistid in( select PlaylistId from tbSplPlaylistSchedule where SchId not in (" + LocalSchId + "))";
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdDel1 = new OleDbCommand();
                        cmdDel1.Connection = StaticClass.LocalCon;
                        cmdDel1.CommandText = strInsert;
                        cmdDel1.ExecuteNonQuery();

                        strInsert = "";
                        strInsert = "delete from TitlesInPlaylists where Playlistid in( select PlaylistId from tbSplPlaylistSchedule where SchId not in (" + LocalSchId + "))";
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        cmdDel1 = new OleDbCommand();
                        cmdDel1.Connection = StaticClass.LocalCon;
                        cmdDel1.CommandText = strInsert;
                        cmdDel1.ExecuteNonQuery();

                    }
                    strInsert = "";
                    strInsert = "delete from tbSplPlaylistSchedule where SchId not in (" + LocalSchId + ")";
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    OleDbCommand cmdDel = new OleDbCommand();
                    cmdDel.Connection = StaticClass.LocalCon;
                    cmdDel.CommandText = strInsert;
                    cmdDel.ExecuteNonQuery();


                    strInsert = "";
                    strInsert = "delete from tbSplPlaylistSchedule_Weekday ";
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    cmdDel = new OleDbCommand();
                    cmdDel.Connection = StaticClass.LocalCon;
                    cmdDel.CommandText = strInsert;
                    cmdDel.ExecuteNonQuery();

                    strInsert = "";
                    strInsert = "delete from tbSpecialPlaylists_Titles ";
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    cmdDel = new OleDbCommand();
                    cmdDel.Connection = StaticClass.LocalCon;
                    cmdDel.CommandText = strInsert;
                    cmdDel.ExecuteNonQuery();
                    #endregion



                    for (int iCtr = 0; (iCtr <= (dtDetailNew.Rows.Count - 1)); iCtr++)
                    {
                        LocalpSchId = Convert.ToInt32(dtDetailNew.Rows[iCtr]["pSchId"]);
                        LocalSplId = Convert.ToInt32(dtDetailNew.Rows[iCtr]["splPlaylistId"]);
                        #region Save in main table
                        strNew = "";
                        strNew = "select tbSpecialPlaylistSchedule.pSchId,tbSpecialPlaylists.splPlaylistName,tbSpecialPlaylistSchedule.StartTime,tbSpecialPlaylistSchedule.EndTime , ";
                        strNew = strNew + " tbSpecialPlaylistSchedule.splPlaylistId, isnull(tbSpecialPlaylists.IsVideoMute,0) as VideoMute from tbSpecialPlaylistSchedule  inner join tbSpecialPlaylists on tbSpecialPlaylists.splPlaylistid= tbSpecialPlaylistSchedule.splPlaylistid  ";
                        strNew = strNew + " where tbSpecialPlaylistSchedule.pSchId=   " + LocalpSchId;
                        dtDetail = new DataTable();

                        dtDetail = ObjMainClass.fnFillDataTable(strNew);
                        if (dtDetail.Rows.Count <= 0) { IsDeleteAllOgg = true; return; }
                        if ((dtDetail.Rows.Count > 0))
                        {
                            strGet = "";
                            strGet = "select * from tbSplPlaylistSchedule where SchId=" + Convert.ToInt32(dtDetail.Rows[0]["pSchId"]);
                            dtGetRecord = new DataTable();
                            dtGetRecord = ObjMainClass.fnFillDataTable_Local(strGet);
                            if (dtGetRecord.Rows.Count <= 0)
                            {
                                strInsert = "";
                                strInsert = "insert into tbSplPlaylistSchedule (SchId,splPlaylistId,StartTime,EndTime,splName,PlaylistId,IsVideoMute) values(" + Convert.ToInt32(dtDetail.Rows[0]["pSchId"]);
                                strInsert = strInsert + " ," + Convert.ToInt32(dtDetail.Rows[0]["splPlaylistId"]) + ", #" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dtDetail.Rows[0]["StartTime"])) + "#,";
                                strInsert = strInsert + " #" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dtDetail.Rows[0]["EndTime"])) + "#,'" + dtDetail.Rows[0]["splPlaylistName"].ToString() + "',0," + dtDetail.Rows[0]["VideoMute"].ToString() + ")";
                            }
                            else
                            {
                                strInsert = "";
                                strInsert = "update tbSplPlaylistSchedule set splPlaylistId=" + Convert.ToInt32(dtDetail.Rows[0]["splPlaylistId"]) + ", StartTime=";
                                strInsert = strInsert + " #" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dtDetail.Rows[0]["StartTime"])) + "# , EndTime= #" + string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dtDetail.Rows[0]["EndTime"])) + "# ,";
                                strInsert = strInsert + " IsVideoMute=" + dtDetail.Rows[0]["VideoMute"].ToString() + " , ";
                                strInsert = strInsert + " splName='" + dtDetail.Rows[0]["splPlaylistName"].ToString() + "' where schid=" + Convert.ToInt32(dtDetail.Rows[0]["pSchId"]);

                            }

                            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                            OleDbCommand cmdTitle = new OleDbCommand();
                            cmdTitle.Connection = StaticClass.LocalCon;
                            cmdTitle.CommandText = strInsert;
                            cmdTitle.ExecuteNonQuery();
                        }
                        #endregion
                        #region Save Weekdays
                        strNew = "";
                        strNew = "select * from tbSpecialPlaylistSchedule_Weekday ";
                        strNew = strNew + " where pSchId=   " + LocalpSchId;
                        dtDetail = new DataTable();
                        dtDetail = ObjMainClass.fnFillDataTable(strNew);
                        if (dtDetail.Rows.Count <= 0) { IsDeleteAllOgg = true; return; }
                        if ((dtDetail.Rows.Count > 0))
                        {
                            for (int iW = 0; (iW <= (dtDetail.Rows.Count - 1)); iW++)
                            {
                                strInsert = "";
                                strInsert = "insert into tbSplPlaylistSchedule_Weekday values(" + Convert.ToInt32(dtDetail.Rows[iW]["pSchId"]);
                                strInsert = strInsert + " ," + Convert.ToInt32(dtDetail.Rows[iW]["wId"]) + ", " + Convert.ToByte(dtDetail.Rows[iW]["IsAllWeek"]) + ")";
                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                OleDbCommand cmdTitle = new OleDbCommand();
                                cmdTitle.Connection = StaticClass.LocalCon;
                                cmdTitle.CommandText = strInsert;
                                cmdTitle.ExecuteNonQuery();
                            }
                        }
                        #endregion
                        #region Save Titles
                        strNew = "";
                        strNew = "GetSpecialPlaylists_Titles " + LocalSplId;
                        dtDetail = new DataTable();
                        dtDetail = ObjMainClass.fnFillDataTable(strNew);
                        if (dtDetail.Rows.Count <= 0) { IsDeleteAllOgg = true; return; }
                        if ((dtDetail.Rows.Count > 0))
                        {
                            for (int iW = 0; (iW <= (dtDetail.Rows.Count - 1)); iW++)
                            {
                                string ext = "";
                                if (dtDetail.Rows[iW]["mType"].ToString()== "Video")
                                {
                                    ext = ".mp4";
                                }
                                if (dtDetail.Rows[iW]["mType"].ToString() == "Audio")
                                {
                                    ext = ".mp3";
                                }
                                if (dtDetail.Rows[iW]["mType"].ToString() == "Image")
                                {
                                    ext = ".jpg";
                                }
                                string filePath = Application.StartupPath + "\\so\\" + Convert.ToInt32(dtDetail.Rows[iW]["titleId"]) + ext;
                                if (File.Exists(filePath))
                                {
                                    long DownloadedSize = new FileInfo(filePath).Length;
                                    long TitleSize = Convert.ToInt32(dtDetail.Rows[iW]["FileSize"]);
                                    if (DownloadedSize == TitleSize)
                                    {
                                        IsLocalSplSong = 1;
                                    }
                                    else
                                    {
                                        IsLocalSplSong = 0;
                                    }
                                }
                                else
                                {
                                    IsLocalSplSong = 0;
                                }
                                Special_Name = "";
                                Special_Change = "";

                                Special_Name = dtDetail.Rows[iW]["title"].ToString().Replace("'", "??$$$??");
                                Special_Change = Special_Name.Replace("'", "??$$$??");
                                string HttpUrl = "http://134.119.178.26/mp3files/" + Convert.ToInt32(dtDetail.Rows[iW]["titleId"]) + ext;
                                strInsert = "";
                                strInsert = "insert into tbSpecialPlaylists_Titles (SchId,titleId,isDownload,Title,AlbumID,ArtistID,[Time],arName,alName,FileSize,HttpUrl,LocalUrl) values(" + LocalpSchId;
                                strInsert = strInsert + " ," + Convert.ToInt32(dtDetail.Rows[iW]["titleId"]) + ", " + IsLocalSplSong + ",'" + dtDetail.Rows[iW]["title"].ToString().Replace("'", "??$$$??") + "', ";
                                strInsert = strInsert + " " + Convert.ToInt32(dtDetail.Rows[iW]["AlbumID"]) + "," + Convert.ToInt32(dtDetail.Rows[iW]["ArtistID"]) + " ,";
                                strInsert = strInsert + " '" + dtDetail.Rows[iW]["Time"] + "','" + dtDetail.Rows[iW]["arName"].ToString().Replace("'", "??$$$??") + "','" + dtDetail.Rows[iW]["aName"].ToString().Replace("'", "??$$$??") + "','" + dtDetail.Rows[iW]["filesize"].ToString() + "' , ";
                                strInsert = strInsert + " '" + HttpUrl + "', '" + filePath + "' )";
                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                OleDbCommand cmdTitle = new OleDbCommand();
                                cmdTitle.Connection = StaticClass.LocalCon;
                                cmdTitle.CommandText = strInsert;
                                cmdTitle.ExecuteNonQuery();
                            }
                        }
                        #endregion
                    }

                }
                else
                {
                    #region Delete Songs
                    //strNew = "";
                    //strNew = "select * from tbSpecialPlaylists_Titles ";
                    //dtDetail = new DataTable();
                    //dtDetail = ObjMainClass.fnFillDataTable_Local(strNew);
                    //for (int iDelT = 0; (iDelT <= (dtDetail.Rows.Count - 1)); iDelT++)
                    //{
                    //    if (File.Exists(Application.StartupPath + "\\" + Convert.ToInt32(dtDetail.Rows[iDelT]["titleId"]) + ".mp4"))
                    //    {
                    //        File.Delete(Application.StartupPath + "\\" + Convert.ToInt32(dtDetail.Rows[iDelT]["titleId"]) + ".mp4");
                    //    }
                    //}
                    #endregion
                    #region Delete exixts records
                    //strNew = "";
                    //strNew = "select * from Playlists ";
                    //dtDelete = ObjMainClass.fnFillDataTable_Local(strNew);

                    //for (int iDel = 0; (iDel <= (dtDelete.Rows.Count - 1)); iDel++)
                    //{
                    //    if (PlayId == "")
                    //    {
                    //        PlayId = dtDelete.Rows[iDel]["PlaylistId"].ToString();
                    //    }
                    //    else
                    //    {
                    //        PlayId = PlayId + "," + dtDelete.Rows[iDel]["PlaylistId"].ToString();
                    //    }
                    //}
                    //if (PlayId != "")
                    //{
                    //    if (ObjMainClass.CheckForInternetConnection() == false) { IsDeleteAllOgg = true; return; }
                    //    if (StaticClass.constr.State == ConnectionState.Open) { StaticClass.constr.Close(); }
                    //    StaticClass.constr.Open();
                    //    SqlCommand cmd = new SqlCommand();
                    //    cmd.Connection = StaticClass.constr;
                    //    cmd.CommandType = CommandType.Text;
                    //    cmd.CommandText = "delete from Playlists where Playlistid not in(" + PlayId + ") and tokenid = " + StaticClass.TokenId;
                    //    cmd.ExecuteNonQuery();

                    //    strInsert = "";
                    //    strInsert = "delete from Playlists where Playlistid in( select PlaylistId from tbSplPlaylistSchedule where SchId not in (" + LocalSchId + "))";
                    //    OleDbCommand cmdDel1 = new OleDbCommand();
                    //    cmdDel1.Connection = StaticClass.LocalCon;
                    //    cmdDel1.CommandText = strInsert;
                    //    cmdDel1.ExecuteNonQuery();


                    //    strInsert = "";
                    //    strInsert = "delete from TitlesInPlaylists where Playlistid in( select PlaylistId from tbSplPlaylistSchedule where SchId not in (" + LocalSchId + "))";
                    //    cmdDel1 = new OleDbCommand();
                    //    cmdDel1.Connection = StaticClass.LocalCon;
                    //    cmdDel1.CommandText = strInsert;
                    //    cmdDel1.ExecuteNonQuery();





                    //}
                    //strInsert = "";
                    //strInsert = "delete from tbSplPlaylistSchedule where SchId not in (" + LocalSchId + ")";


                    //OleDbCommand cmdDel = new OleDbCommand();
                    //cmdDel.Connection = StaticClass.LocalCon;
                    //cmdDel.CommandText = strInsert;
                    //cmdDel.ExecuteNonQuery();


                    //strInsert = "";
                    //strInsert = "delete from tbSplPlaylistSchedule_Weekday ";

                    //cmdDel = new OleDbCommand();
                    //cmdDel.Connection = StaticClass.LocalCon;
                    //cmdDel.CommandText = strInsert;
                    //cmdDel.ExecuteNonQuery();


                    //strInsert = "";
                    //strInsert = "delete from tbSpecialPlaylists_Titles ";

                    //cmdDel = new OleDbCommand();
                    //cmdDel.Connection = StaticClass.LocalCon;
                    //cmdDel.CommandText = strInsert;
                    //cmdDel.ExecuteNonQuery();

                    #endregion
                }
                #endregion
            }
            catch (Exception ex)
            {
                //MessageBox.Show("bgGetSplPlaylist_DoWork " + ex.Message);
                IsDeleteAllOgg = true;
                if (bgGetSplPlaylist.IsBusy == true)
                {
                    bgGetSplPlaylist.CancelAsync();
                    if (bgGetSplPlaylist.CancellationPending == true)
                    {
                        e.Cancel = true;
                    }
                }


                #region Delete exixts records
                //strNew = "";
                //strNew = "select * from Playlists ";
                //dtDelete = ObjMainClass.fnFillDataTable_Local(strNew);

                //for (int iDel = 0; (iDel <= (dtDelete.Rows.Count - 1)); iDel++)
                //{
                //    if (PlayId == "")
                //    {
                //        PlayId = dtDelete.Rows[iDel]["PlaylistId"].ToString();
                //    }
                //    else
                //    {
                //        PlayId = PlayId + "," + dtDelete.Rows[iDel]["PlaylistId"].ToString();
                //    }
                //}
                //if (PlayId != "")
                //{
                //    if (ObjMainClass.CheckForInternetConnection() == false) { IsDeleteAllOgg = true; return; }
                //    if (StaticClass.constr.State == ConnectionState.Open) { StaticClass.constr.Close(); }
                //    StaticClass.constr.Open();
                //    SqlCommand cmd = new SqlCommand();
                //    cmd.Connection = StaticClass.constr;
                //    cmd.CommandType = CommandType.Text;
                //    cmd.CommandText = "delete from Playlists where Playlistid not in(" + PlayId + ") and tokenid = " + StaticClass.TokenId;
                //    cmd.ExecuteNonQuery();

                //    if (LocalSchId != "")
                //    {
                //        strInsert = "";
                //        strInsert = "delete from Playlists where Playlistid in( select PlaylistId from tbSplPlaylistSchedule where SchId not in (" + LocalSchId + "))";

                //        OleDbCommand cmdDel1 = new OleDbCommand();
                //        cmdDel1.Connection = StaticClass.LocalCon;
                //        cmdDel1.CommandText = strInsert;
                //        cmdDel1.ExecuteNonQuery();


                //        strInsert = "";
                //        strInsert = "delete from TitlesInPlaylists where Playlistid in( select PlaylistId from tbSplPlaylistSchedule where SchId not in (" + LocalSchId + "))";

                //        cmdDel1 = new OleDbCommand();
                //        cmdDel1.Connection = StaticClass.LocalCon;
                //        cmdDel1.CommandText = strInsert;
                //        cmdDel1.ExecuteNonQuery();

                //    }



                //}
                //if (LocalSchId != "")
                //{
                //    strInsert = "";
                //    strInsert = "delete from tbSplPlaylistSchedule where SchId not in (" + LocalSchId + ")";

                //    OleDbCommand cmdDel = new OleDbCommand();
                //    cmdDel.Connection = StaticClass.LocalCon;
                //    cmdDel.CommandText = strInsert;
                //    cmdDel.ExecuteNonQuery();


                //    strInsert = "";
                //    strInsert = "delete from tbSplPlaylistSchedule_Weekday ";

                //    cmdDel = new OleDbCommand();
                //    cmdDel.Connection = StaticClass.LocalCon;
                //    cmdDel.CommandText = strInsert;
                //    cmdDel.ExecuteNonQuery();


                //    strInsert = "";
                //    strInsert = "delete from tbSpecialPlaylists_Titles ";

                //    cmdDel = new OleDbCommand();
                //    cmdDel.Connection = StaticClass.LocalCon;
                //    cmdDel.CommandText = strInsert;
                //    cmdDel.ExecuteNonQuery();

                //}
                #endregion

            }

        }

        private void bgGetSplPlaylist_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GC.Collect();
            try
            {
                bgGetSplPlaylist.Dispose();
                FillLocalPlaylist();
                if (IsDeleteAllOgg == true)
                {
                    picSplGif.Visible = false;
                    UpdateScheduleRecord();

                    sTime = 0;
                    timGetSplPlaylist.Enabled = true;
                    return;
                }

                Spl_song_download();
            }
            catch (Exception ex) { }
        }

        private void Spl_song_download()
        {
            string mlsSql = "";
            string strGet = "";
            string PlaylistName = "";
            string sQr = "";
            int i;
            Int32 Playlist_Id = 0;
            timResetSplDownloading.Enabled = false;
            //mlsSql = "SELECT * FROM tbSpecialPlaylists_Titles WHERE TitleId>=(Select Max(TitleId) from tbSpecialPlaylists_Titles where  isdownload=0)-30 and   isdownload=0";
            //mlsSql = "SELECT  * from tbSplPlaylistSchedule where #03:30 AM# >=Starttime And  #03:30 AM# <=EndTime";


            mlsSql = "SELECT * FROM tbSpecialPlaylists_Titles WHERE isdownload=0 and SchId=(SELECT max(Schid) as Schid from tbSplPlaylistSchedule where #" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "# >=Starttime And  #" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "# <=EndTime)";
            DataSet ds = new DataSet();
            ds = ObjMainClass.fnFillDataSet_Local(mlsSql);
            if (ds.Tables[0].Rows.Count <= 0)
            {
                mlsSql = "SELECT * FROM tbSpecialPlaylists_Titles WHERE SchId=(SELECT max(tbSplPlaylistSchedule.Schid) as Schid from (tbSplPlaylistSchedule inner join  tbSpecialPlaylists_Titles on  tbSpecialPlaylists_Titles.schid = tbSplPlaylistSchedule.SchId ) where    tbSpecialPlaylists_Titles.isdownload=0)";
                ds = ObjMainClass.fnFillDataSet_Local(mlsSql);
            }

            InitilizeSplGrid();
            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string filePath = ds.Tables[0].Rows[i]["localUrl"].ToString();
                string filePathOgg = ds.Tables[0].Rows[i]["localUrl"].ToString();
                StaticClass.SchId = Convert.ToInt32(ds.Tables[0].Rows[i]["SchId"]);
                if (File.Exists(filePath))
                {
                    Int32 TitleSize = 0;
                    sQr = "";
                    sQr = "select distinct filesize from tbSpecialPlaylists_Titles where titleid=" + ds.Tables[0].Rows[i]["titleId"];
                    DataTable dt = new DataTable();
                    dt = ObjMainClass.fnFillDataTable_Local(sQr);
                    if (dt.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dt.Rows[0]["Filesize"].ToString()) == false)
                        {
                            TitleSize = Convert.ToInt32(dt.Rows[0]["Filesize"]);
                        }
                        else
                        {
                            TitleSize = 0;
                        }
                    }
                    long DownloadedSize = new FileInfo(filePath).Length;
                    if (TitleSize != DownloadedSize)
                    {
                        dgSpl.Rows.Add();
                        dgSpl.Rows[dgSpl.Rows.Count - 1].Cells["songid"].Value = ds.Tables[0].Rows[i]["titleId"].ToString();
                        dgSpl.Rows[dgSpl.Rows.Count - 1].Cells["httpUrl"].Value = ds.Tables[0].Rows[i]["HttpUrl"].ToString();
                    }
                }
                if (!File.Exists(filePath) && !File.Exists(filePathOgg))
                {

                    dgSpl.Rows.Add();
                    dgSpl.Rows[dgSpl.Rows.Count - 1].Cells["songid"].Value = ds.Tables[0].Rows[i]["titleId"].ToString();
                    dgSpl.Rows[dgSpl.Rows.Count - 1].Cells["httpUrl"].Value = ds.Tables[0].Rows[i]["HttpUrl"].ToString();
                }
            }
            //lblPercentage.Visible = true;
            //lblPercentage.Text = "";
            if (ds.Tables[0].Rows.Count == 0)
            {
                SaveDownloadPlaylist("Direct");
                //                mlsSql = "SELECT * FROM tbSplPlaylistSchedule WHERE PlaylistId=0 ";
                //DataSet dsSch = new DataSet();
                //dsSch = ObjMainClass.fnFillDataSet_Local(mlsSql);
                //for (i = 0; i < dsSch.Tables[0].Rows.Count; i++)
                //{

                //    }
                UpdateScheduleRecord();
                picSplGif.Visible = false;
                //DownloadAdvt();

                timGetSplPlaylistScheduleTime.Enabled = true;
                return;
            }

            DownloadSplSongs();
        }

        private void UpdateScheduleRecord()
        {

            string sQr = "";
            DataSet dtGetRecordNew;
            DataSet dtGetDefault;
            string strGet = "";
            string PlaylistName = "";
            Int32 Playlist_Id = 0;

            #region Save Records
            strGet = "";
            string Special_Name = "";
            string Special_Change = "";
            strGet = "SELECT * FROM tbSplPlaylistSchedule ";
            dtGetRecordNew = new DataSet();
            dtGetRecordNew = ObjMainClass.fnFillDataSet_Local(strGet);
            for (int iPlaylist = 0; (iPlaylist <= (dtGetRecordNew.Tables[0].Rows.Count - 1)); iPlaylist++)
            {

                StaticClass.SchId = Convert.ToInt32(dtGetRecordNew.Tables[0].Rows[iPlaylist]["SchId"]);
                if (Convert.ToInt32(dtGetRecordNew.Tables[0].Rows[iPlaylist]["playlistid"]) == 0)
                {

                    #region Insert Record
                    PlaylistName = dtGetRecordNew.Tables[0].Rows[iPlaylist]["splName"].ToString();
                    #region SplPlaylistSave

                    if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                    StaticClass.constr.Open();
                    SqlCommand cmd = new SqlCommand("InsertPlayListsNew", StaticClass.constr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt));
                    cmd.Parameters["@UserID"].Value = StaticClass.dfClientId;
                    cmd.Parameters.Add(new SqlParameter("@IsPredefined", SqlDbType.Bit));
                    cmd.Parameters["@IsPredefined"].Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Name"].Value = PlaylistName;
                    cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Summary"].Value = " ";
                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Description"].Value = " ";
                    cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
                    cmd.Parameters["@TokenId"].Value = StaticClass.TokenId;
                    try
                    {
                        Playlist_Id = Convert.ToInt32(cmd.ExecuteScalar());
                        sQr = "";
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        sQr = "insert into PlayLists values(" + Convert.ToInt32(Playlist_Id) + ", ";
                        sQr = sQr + StaticClass.dfClientId + " , '" + PlaylistName + "', " + StaticClass.TokenId + ",'',1 )";

                        OleDbCommand cmdSaveLocal = new OleDbCommand();
                        cmdSaveLocal.Connection = StaticClass.LocalCon;
                        cmdSaveLocal.CommandText = sQr;
                        cmdSaveLocal.ExecuteNonQuery();


                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show("bgSaveSplPlaylist_DoWork Playlist " + ex.Message);

                    }
                    finally
                    {
                        StaticClass.constr.Close();
                    }
                    #endregion
                    sQr = "";
                    sQr = "update tbSplPlaylistSchedule set PlaylistId=" + Playlist_Id + " where SchId=" + dtGetRecordNew.Tables[0].Rows[iPlaylist]["SchId"].ToString();
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    OleDbCommand cmdDel = new OleDbCommand();
                    cmdDel.Connection = StaticClass.LocalCon;
                    cmdDel.CommandText = sQr;
                    cmdDel.ExecuteNonQuery();

                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    string sWr1 = "delete from  TitlesInPlaylists where PlaylistID = " + Playlist_Id;
                    OleDbCommand cmdP1 = new OleDbCommand();
                    cmdP1.Connection = StaticClass.LocalCon;
                    cmdP1.CommandText = sWr1;
                    cmdP1.ExecuteNonQuery();

                    #region insert_SplPlaylist_song_LocalDatabase
                    string sWr = "";

                    sQr = "";

                    DataSet ds = new DataSet();
                    try
                    {

                        sQr = "";

                        sQr = "select * from tbSpecialPlaylists_Titles where SchId=" + StaticClass.SchId + " and isDownload=1";
                        DataSet dsMain = new DataSet();
                        dsMain = ObjMainClass.fnFillDataSet_Local(sQr);
                        for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
                        {
                            sQr = "select * from Titles where TitleID=" + dsMain.Tables[0].Rows[i]["titleId"];
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            #region Save Title
                            if (ds.Tables[0].Rows.Count <= 0)
                            {

                                Special_Name = dsMain.Tables[0].Rows[i]["title"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");

                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into Titles (TitleID,AlbumID,ArtistID,Title,Gain,[Time],TitleYear,HttpUrl,LocalUrl) values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["TitleID"]) + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' , ";
                                sWr = sWr + " '0' , '" + dsMain.Tables[0].Rows[i]["Time"] + "' ,0 ,";
                                sWr = sWr + " '" + dsMain.Tables[0].Rows[i]["HttpUrl"] + "', ";
                                sWr = sWr + " '" + dsMain.Tables[0].Rows[i]["LocalUrl"] + "' )";
                                OleDbCommand cmdTitle = new OleDbCommand();
                                cmdTitle.Connection = StaticClass.LocalCon;
                                cmdTitle.CommandText = sWr;
                                cmdTitle.ExecuteNonQuery();

                            }
                            #endregion

                            #region SaveAlbum

                            Special_Name = dsMain.Tables[0].Rows[i]["alName"].ToString();
                            Special_Change = Special_Name.Replace("'", "??$$$??");

                            sQr = "select * from Albums where albumid=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]);
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into Albums values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                                OleDbCommand cmdAlbum = new OleDbCommand();
                                cmdAlbum.Connection = StaticClass.LocalCon;
                                cmdAlbum.CommandText = sWr;
                                cmdAlbum.ExecuteNonQuery();
                            }
                            #endregion

                            #region Save Artist
                            sQr = "select * from Artists where ArtistID=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]);
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                Special_Name = dsMain.Tables[0].Rows[i]["arName"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");

                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into Artists values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                                OleDbCommand cmdAlbum = new OleDbCommand();
                                cmdAlbum.Connection = StaticClass.LocalCon;
                                cmdAlbum.CommandText = sWr;
                                cmdAlbum.ExecuteNonQuery();
                            }
                            #endregion

                            #region Save TitlesInPlaylists



                            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                            sWr = "insert into TitlesInPlaylists(PlaylistID,TitleID,SrNo) values (" + Playlist_Id + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]) + ", " + Convert.ToInt32(i + 1) + ")";
                            OleDbCommand cmdP = new OleDbCommand();
                            cmdP.Connection = StaticClass.LocalCon;
                            cmdP.CommandText = sWr;
                            cmdP.ExecuteNonQuery();
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        //                        MessageBox.Show("bgSaveSplPlaylist_DoWork Detail" + ex.Message);
                    }
                    #endregion

                    #endregion

                }
                else
                {

                    Playlist_Id = Convert.ToInt32(dtGetRecordNew.Tables[0].Rows[iPlaylist]["playlistid"]);

                    #region insert_SplPlaylist_song_LocalDatabase
                    string sWr = "";

                    sQr = "";

                    DataSet ds = new DataSet();
                    try
                    {
                        sQr = "";
                        //sQr = "select * from tbSpecialPlaylists_Titles where SchId=" + StaticClass.SchId + " and IsDownload=1 and titleid not in (select titleid from TitlesInPlaylists where PlaylistID= " + Playlist_Id + ")";
                        sQr = "select * from tbSpecialPlaylists_Titles where SchId=" + StaticClass.SchId + " and IsDownload=1 ";
                        DataSet dsMain = new DataSet();
                        dsMain = ObjMainClass.fnFillDataSet_Local(sQr);
                        for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
                        {

                            #region Save Title
                            sQr = "select * from Titles where TitleID=" + dsMain.Tables[0].Rows[i]["titleId"];
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                Special_Name = dsMain.Tables[0].Rows[i]["title"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");

                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                //sWr = "insert into Titles values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["TitleID"]) + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                //sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' , ";
                                //sWr = sWr + " '0' , '" + dsMain.Tables[0].Rows[i]["Time"] + "' ,0)";

                                sWr = "insert into Titles (TitleID,AlbumID,ArtistID,Title,Gain,[Time],TitleYear,HttpUrl,LocalUrl) values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["TitleID"]) + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' , ";
                                sWr = sWr + " '0' , '" + dsMain.Tables[0].Rows[i]["Time"] + "' ,0 ,";
                                sWr = sWr + " '" + dsMain.Tables[0].Rows[i]["HttpUrl"] + "', ";
                                sWr = sWr + " '" + dsMain.Tables[0].Rows[i]["LocalUrl"] + "' )";

                                OleDbCommand cmdTitle = new OleDbCommand();
                                cmdTitle.Connection = StaticClass.LocalCon;
                                cmdTitle.CommandText = sWr;
                                cmdTitle.ExecuteNonQuery();

                            }
                            else
                            {
                                sWr = "update  Titles set ";
                                sWr = sWr + " HttpUrl= '" + dsMain.Tables[0].Rows[i]["HttpUrl"] + "', ";
                                sWr = sWr + " LocalUrl= '" + dsMain.Tables[0].Rows[i]["LocalUrl"] + "' ";
                                sWr = sWr + " where titleid = " + dsMain.Tables[0].Rows[i]["TitleID"] + " ";
                                OleDbCommand cmdTitle = new OleDbCommand();
                                cmdTitle.Connection = StaticClass.LocalCon;
                                cmdTitle.CommandText = sWr;
                                cmdTitle.ExecuteNonQuery();
                            }
                            #endregion

                            #region SaveAlbum
                            sQr = "select * from Albums where albumid=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]);
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                Special_Name = dsMain.Tables[0].Rows[i]["alName"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");

                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into Albums values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                                OleDbCommand cmdAlbum = new OleDbCommand();
                                cmdAlbum.Connection = StaticClass.LocalCon;
                                cmdAlbum.CommandText = sWr;
                                cmdAlbum.ExecuteNonQuery();

                            }
                            #endregion

                            #region Save Artist
                            sQr = "select * from Artists where ArtistID=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]);
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                Special_Name = dsMain.Tables[0].Rows[i]["arName"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");
                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into Artists values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                                OleDbCommand cmdAlbum = new OleDbCommand();
                                cmdAlbum.Connection = StaticClass.LocalCon;
                                cmdAlbum.CommandText = sWr;
                                cmdAlbum.ExecuteNonQuery();

                            }
                            #endregion

                            #region Save TitlesInPlaylists



                            strGet = "";
                            strGet = "select * from TitlesInPlaylists where PlaylistID= " + Playlist_Id + " and TitleID=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]);
                            dtGetDefault = ObjMainClass.fnFillDataSet_Local(strGet);
                            if (dtGetDefault.Tables[0].Rows.Count <= 0)
                            {
                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into TitlesInPlaylists(PlaylistID,TitleID,SrNo) values (" + Playlist_Id + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]) + ", " + Convert.ToInt32(i + 1) + ")";
                                OleDbCommand cmdP = new OleDbCommand();
                                cmdP.Connection = StaticClass.LocalCon;
                                cmdP.CommandText = sWr;
                                cmdP.ExecuteNonQuery();
                            }
                            #endregion

                            #region Add Titles in Grid
                            strGet = "";
                            strGet = "SELECT PlaylistDefault FROM Playlists where playlistid =" + Playlist_Id;
                            dtGetDefault = new DataSet();
                            dtGetDefault = ObjMainClass.fnFillDataSet_Local(strGet);
                            if (dtGetDefault.Tables[0].Rows[0]["PlaylistDefault"].ToString() == "Default")
                            {
                                Boolean isTitleFind = false;
                                for (int iSpl = 0; iSpl < dgPlaylist.Rows.Count; iSpl++)
                                {
                                    if (Convert.ToInt32(dgPlaylist.Rows[iSpl].Cells["songid"].Value) == Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]))
                                    {
                                        isTitleFind = true;
                                        break;
                                    }
                                }
                                if (isTitleFind == false)
                                {
                                    AddSongsInGrid(dgPlaylist, Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]));
                                }
                            }
                            #endregion

                            if (FirstTimeSong == true)
                            {
                                if (dgPlaylist.Rows.Count > 1)
                                {
                                    NextSongDisplay(dgPlaylist.Rows[1].Cells[0].Value.ToString());
                                    FirstTimeSong = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show("bgSaveSplPlaylist_DoWork Detail" + ex.Message);
                    }
                    #endregion
                }
            }
            FillLocalPlaylist();
            if (dgLocalPlaylist.Rows.Count >= 0)
            {
                PopulateSplPlaylist(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[0].Cells["playlistId"].Value));
            }
            if (timGetSplPlaylistScheduleTime.Enabled == false)
            {
                timGetSplPlaylistScheduleTime.Enabled = true;
            }
            GetSplSongCounter(Playlist_Id);
            #endregion
        }


        private void InitilizeSplGrid()
        {
            if (dgSpl.Rows.Count > 0)
            {
                dgSpl.Rows.Clear();
            }
            if (dgSpl.Columns.Count > 0)
            {
                dgSpl.Columns.Clear();
            }

            dgSpl.Columns.Add("songid", "song Id");
            dgSpl.Columns["songid"].Width = 100;
            dgSpl.Columns["songid"].Visible = true;
            dgSpl.Columns["songid"].ReadOnly = true;

            dgSpl.Columns.Add("httpUrl", "httpUrl");
            dgSpl.Columns["httpUrl"].Width = 100;
            dgSpl.Columns["httpUrl"].Visible = true;
            dgSpl.Columns["httpUrl"].ReadOnly = true;
        }

        private void DownloadSplSongs()
        {
            for (int iSpl = 0; iSpl < dgSpl.Rows.Count; iSpl++)
            {
                if (wcDownloadSplSongs.IsBusy == false)
                {

                    SplSongName = dgSpl.Rows[iSpl].Cells["songid"].Value.ToString();
                    SplSongUrl = dgSpl.Rows[iSpl].Cells["httpUrl"].Value.ToString();
                    dgSpl.Rows.RemoveAt(iSpl);
                    bgDownloadSplSongs();
                    break;
                }
            }
        }



        private void SaveDownloadPlaylist(string KeyComeFrom)
        {

            string mstr = "";
            mstr = "delete from TitlesInPlaylists where titleid not in (select distinct titleid from tbSpecialPlaylists_Titles)";
            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            OleDbCommand cmdP1 = new OleDbCommand();
            cmdP1.Connection = StaticClass.LocalCon;
            cmdP1.CommandText = mstr;
            cmdP1.ExecuteNonQuery();
            if (KeyComeFrom == "Download")
            {
                PopulateSplPlaylist(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[dgLocalPlaylist.CurrentCell.RowIndex].Cells[0].Value));
                FirstTimeConditation = "Yes";
                PlaylistTime = "";
                if (timGetSplPlaylistScheduleTime.Enabled == false)
                {
                    timGetSplPlaylistScheduleTime.Enabled = true;
                }
            }


            //string mlsSql = "";
            //string strGet = "";
            //string PlaylistName = "";
            //string sQr = "";
            //int i;
            //Int32 Playlist_Id = 0;
            //DataSet ds = new DataSet();
            //mlsSql = "SELECT * FROM tbSplPlaylistSchedule WHERE PlaylistId=0 ";
            //DataSet dsSch = new DataSet();
            //dsSch = ObjMainClass.fnFillDataSet_Local(mlsSql);
            //if (dsSch.Tables[0].Rows.Count > 0)
            //{
            //    for (i = 0; i < dsSch.Tables[0].Rows.Count; i++)
            //    {
            #region Save Records
            //strGet = "";
            //string Special_Name = "";
            //string Special_Change = "";

            //PlaylistName = dsSch.Tables[0].Rows[i]["splName"].ToString();
            //#region SplPlaylistSave

            //if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
            //StaticClass.constr.Open();
            //SqlCommand cmd = new SqlCommand("InsertPlayListsNew", StaticClass.constr);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt));
            //cmd.Parameters["@UserID"].Value = StaticClass.dfClientId;
            //cmd.Parameters.Add(new SqlParameter("@IsPredefined", SqlDbType.Bit));
            //cmd.Parameters["@IsPredefined"].Value = 0;
            //cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
            //cmd.Parameters["@Name"].Value = PlaylistName;
            //cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.VarChar, 50));
            //cmd.Parameters["@Summary"].Value = " ";
            //cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 50));
            //cmd.Parameters["@Description"].Value = " ";
            //cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
            //cmd.Parameters["@TokenId"].Value = StaticClass.TokenId;
            //try
            //{
            //    Playlist_Id = Convert.ToInt32(cmd.ExecuteScalar());
            //    sQr = "";
            //    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            //    sQr = "insert into PlayLists values(" + Convert.ToInt32(Playlist_Id) + ", ";
            //    sQr = sQr + StaticClass.dfClientId + " , '" + PlaylistName + "', " + StaticClass.TokenId + ",'',1 )";
            //    OleDbCommand cmdSaveLocal = new OleDbCommand();
            //    cmdSaveLocal.Connection = StaticClass.LocalCon;
            //    cmdSaveLocal.CommandText = sQr;
            //    cmdSaveLocal.ExecuteNonQuery();
            //}
            //catch (Exception ex)
            //{
            //    // MessageBox.Show("bgSaveSplPlaylist_DoWork Playlist " + ex.Message);

            //}
            //finally
            //{
            //    StaticClass.constr.Close();
            //}
            //#endregion
            //sQr = "";
            //sQr = "update tbSplPlaylistSchedule set PlaylistId=" + Playlist_Id + " where SchId=" + dsSch.Tables[0].Rows[i]["SchId"].ToString();
            //if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            //OleDbCommand cmdDel = new OleDbCommand();
            //cmdDel.Connection = StaticClass.LocalCon;
            //cmdDel.CommandText = sQr;
            //cmdDel.ExecuteNonQuery();


            //#region insert_SplPlaylist_song_LocalDatabase
            //string sWr = "";

            //sQr = "";

            //ds = new DataSet();
            //try
            //{
            //    sQr = "";
            //    sQr = "select * from tbSpecialPlaylists_Titles where SchId=" + dsSch.Tables[0].Rows[i]["SchId"].ToString() + " and isDownload=1";
            //    DataSet dsMain = new DataSet();
            //    dsMain = ObjMainClass.fnFillDataSet_Local(sQr);
            //    for (int iT = 0; iT < dsMain.Tables[0].Rows.Count; iT++)
            //    {
            //        sQr = "select * from Titles where TitleID=" + dsMain.Tables[0].Rows[iT]["titleId"];
            //        ds = ObjMainClass.fnFillDataSet_Local(sQr);
            //        #region Save Title
            //        if (ds.Tables[0].Rows.Count <= 0)
            //        {

            //            Special_Name = dsMain.Tables[0].Rows[iT]["title"].ToString();
            //            Special_Change = Special_Name.Replace("'", "??$$$??");
            //            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            //            sWr = "insert into Titles values (" + Convert.ToInt32(dsMain.Tables[0].Rows[iT]["TitleID"]) + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[iT]["AlbumID"]) + " , ";
            //            sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[iT]["ArtistID"]) + ", '" + Special_Change + "' , ";
            //            sWr = sWr + " '0' , '" + dsMain.Tables[0].Rows[iT]["Time"] + "' ,0)";
            //            OleDbCommand cmdTitle = new OleDbCommand();
            //            cmdTitle.Connection = StaticClass.LocalCon;
            //            cmdTitle.CommandText = sWr;
            //            cmdTitle.ExecuteNonQuery();
            //        }
            //        #endregion

            //        #region SaveAlbum

            //        Special_Name = dsMain.Tables[0].Rows[iT]["alName"].ToString();
            //        Special_Change = Special_Name.Replace("'", "??$$$??");

            //        sQr = "select * from Albums where albumid=" + Convert.ToInt32(dsMain.Tables[0].Rows[iT]["AlbumID"]);
            //        ds = ObjMainClass.fnFillDataSet_Local(sQr);
            //        if (ds.Tables[0].Rows.Count <= 0)
            //        {
            //            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            //            sWr = "insert into Albums values (" + Convert.ToInt32(dsMain.Tables[0].Rows[iT]["AlbumID"]) + " , ";
            //            sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[iT]["ArtistID"]) + ", '" + Special_Change + "' ) ";
            //            OleDbCommand cmdAlbum = new OleDbCommand();
            //            cmdAlbum.Connection = StaticClass.LocalCon;
            //            cmdAlbum.CommandText = sWr;
            //            cmdAlbum.ExecuteNonQuery();
            //        }
            //        #endregion

            //        #region Save Artist
            //        sQr = "select * from Artists where ArtistID=" + Convert.ToInt32(dsMain.Tables[0].Rows[iT]["ArtistID"]);
            //        ds = ObjMainClass.fnFillDataSet_Local(sQr);
            //        if (ds.Tables[0].Rows.Count <= 0)
            //        {
            //            Special_Name = dsMain.Tables[0].Rows[iT]["arName"].ToString();
            //            Special_Change = Special_Name.Replace("'", "??$$$??");

            //            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            //            sWr = "insert into Artists values (" + Convert.ToInt32(dsMain.Tables[0].Rows[iT]["ArtistID"]) + ", '" + Special_Change + "' ) ";
            //            OleDbCommand cmdAlbum = new OleDbCommand();
            //            cmdAlbum.Connection = StaticClass.LocalCon;
            //            cmdAlbum.CommandText = sWr;
            //            cmdAlbum.ExecuteNonQuery();

            //        }
            //        #endregion

            //        #region Save TitlesInPlaylists
            //        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            //        sWr = "insert into TitlesInPlaylists(PlaylistID,TitleID,SrNo) values (" + Playlist_Id + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[iT]["titleId"]) + ", " + Convert.ToInt32(iT + 1) + ")";
            //        OleDbCommand cmdP = new OleDbCommand();
            //        cmdP.Connection = StaticClass.LocalCon;
            //        cmdP.CommandText = sWr;
            //        cmdP.ExecuteNonQuery();
            //        #endregion
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //                        MessageBox.Show("bgSaveSplPlaylist_DoWork Detail" + ex.Message);
            //}
            //#endregion

            #endregion
            //    }
            //    FillLocalPlaylist();
            //}
        }


        private void timResetSplDownloading_Tick(object sender, EventArgs e)
        {
            sTime = sTime + 1;
            if (sTime == 10)
            {
                if (ObjMainClass.CheckForInternetConnection() == true)
                {
                    picSplGif.Visible = true;

                    Spl_song_download();
                }
                else
                {
                    picSplGif.Visible = false;


                    sTime = 0;
                }

            }
        }

        private void GetSplSongCounter(Int32 playlistId)
        {
            string strNew = "";
            DataTable dtDetailNew = new DataTable();
            strNew = "select TitlesInPlaylists.playlistId, Count(*) as Total  from TitlesInPlaylists ";
            strNew = strNew + " where TitlesInPlaylists.playlistId = " + playlistId + " ";
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

        #region Implement Spl schedule on players

        DateTime EndTime;
        int PlaylistRow = 0;
        int StartPlaylist = 0;
        int rNetState = 0;
        string PlaylistTime = "";
        string FindTime = "No";
        private void timGetSplPlaylistScheduleTime_Tick(object sender, EventArgs e)
        {
            try
            {
                if (PlaylistTime != string.Format(fi, "{0:hh:mm tt}", DateTime.Now))
                {
                    PlaylistTime = string.Format(fi, "{0:hh:mm tt}", DateTime.Now);
                    for (int iRow = 0; iRow < dgLocalPlaylist.Rows.Count; iRow++)
                    {
                        if (dgLocalPlaylist.Rows[iRow].Cells["sTime"].Value.ToString() != "Nill")
                        {

                            if (FirstTimeConditation == "Yes")
                            {
                                #region Simple
                                if ((Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) >= Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgLocalPlaylist.Rows[iRow].Cells["sTime"].Value)))) && (Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) < Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgLocalPlaylist.Rows[iRow].Cells["eTime"].Value)))))
                                {

                                    picMiddleLogo.BringToFront();
                                    picMiddleLogo.Visible = false;


                                    StaticClass.IsPlayerClose = "No";
                                    IsFormatFirstTimeLoad = "No";
                                    for (int i = 0; i < dgLocalPlaylist.Rows.Count; i++)
                                    {
                                        if (Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells["playlistId"].Value) == Convert.ToInt32(dgLocalPlaylist.Rows[iRow].Cells["playlistId"].Value))
                                        {
                                            dgLocalPlaylist.Rows[i].Cells[2].Value = "Default";

                                            dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[i].Cells[1];

                                            dgLocalPlaylist.Rows[i].Cells[1].Style.ForeColor = Color.FromArgb(20, 162, 175);
                                            dgLocalPlaylist.Rows[i].Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                                            dgLocalPlaylist.Rows[i].Cells[1].Style.SelectionForeColor = Color.Yellow;

                                            dgLocalPlaylist.Rows[i].Cells[3].Style.SelectionBackColor = Color.LightBlue;
                                            dgLocalPlaylist.Rows[i].Cells[3].Style.BackColor = Color.LightBlue;
                                            StaticClass.DefaultPlaylistId = Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value);
                                            StaticClass.DefaultPlaylistCurrentRow = dgLocalPlaylist.CurrentCell.RowIndex;
                                            StaticClass.IsVideoMute = dgLocalPlaylist.Rows[i].Cells["IsvMute"].Value.ToString();
                                        }
                                        else
                                        {
                                            dgLocalPlaylist.Rows[i].Cells[2].Value = "";
                                            dgLocalPlaylist.Rows[i].Cells[1].Style.ForeColor = Color.FromArgb(0, 0, 0);
                                            dgLocalPlaylist.Rows[i].Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                                            dgLocalPlaylist.Rows[i].Cells[3].Style.BackColor = Color.White;
                                            dgLocalPlaylist.Rows[i].Cells[3].Style.SelectionBackColor = Color.White;

                                        }
                                    }

                                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                    OleDbCommand cmdUpdateAll = new OleDbCommand();
                                    cmdUpdateAll.Connection = StaticClass.LocalCon;
                                    cmdUpdateAll.CommandText = "Update Playlists set PlaylistDefault=''";
                                    cmdUpdateAll.ExecuteNonQuery();


                                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                    OleDbCommand cmdUpdate = new OleDbCommand();
                                    cmdUpdate.Connection = StaticClass.LocalCon;
                                    cmdUpdate.CommandText = "Update Playlists set PlaylistDefault='Default' where playlistid = " + dgLocalPlaylist.Rows[iRow].Cells["playlistId"].Value;
                                    cmdUpdate.ExecuteNonQuery();

                                    FirstTimeConditation = "No";
                                    EndTime = Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgLocalPlaylist.Rows[iRow].Cells["eTime"].Value)));
                                    StaticClass.DefaultPlaylistCurrentRow = iRow;
                                    PlaylistRow = iRow;
                                    StartPlaylist = 0;
                                    timStartPlaylistSchedule.Enabled = true;
                                    timPrayerClosing.Enabled = true;
                                    FindTime = "Yes";
                                    StaticClass.PlayerClosingTime = string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgLocalPlaylist.Rows[iRow].Cells["eTime"].Value));
                                    //  timGetSplPlaylistScheduleTime.Enabled = false;
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Step 2
                                if (string.Format(fi, "{0:hh:mm tt}", DateTime.Now) == string.Format(fi, "{0:hh:mm tt}", dgLocalPlaylist.Rows[iRow].Cells["sTime"].Value))
                                {
                                    picMiddleLogo.BringToFront();
                                    picMiddleLogo.Visible = false;
                                    StaticClass.IsPlayerClose = "No";
                                    IsFormatFirstTimeLoad = "No";
                                    for (int i = 0; i < dgLocalPlaylist.Rows.Count; i++)
                                    {
                                        if (Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells["playlistId"].Value) == Convert.ToInt32(dgLocalPlaylist.Rows[iRow].Cells["playlistId"].Value))
                                        {
                                            dgLocalPlaylist.Rows[i].Cells[2].Value = "Default";

                                            dgLocalPlaylist.CurrentCell = dgLocalPlaylist.Rows[i].Cells[1];

                                            dgLocalPlaylist.Rows[i].Cells[1].Style.ForeColor = Color.FromArgb(20, 162, 175);
                                            dgLocalPlaylist.Rows[i].Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                                            dgLocalPlaylist.Rows[i].Cells[1].Style.SelectionForeColor = Color.Yellow;

                                            dgLocalPlaylist.Rows[i].Cells[3].Style.SelectionBackColor = Color.LightBlue;
                                            dgLocalPlaylist.Rows[i].Cells[3].Style.BackColor = Color.LightBlue;
                                            StaticClass.DefaultPlaylistId = Convert.ToInt32(dgLocalPlaylist.Rows[i].Cells[0].Value);
                                            StaticClass.DefaultPlaylistCurrentRow = dgLocalPlaylist.CurrentCell.RowIndex;
                                            StaticClass.IsVideoMute = dgLocalPlaylist.Rows[i].Cells["IsvMute"].Value.ToString();
                                        }
                                        else
                                        {
                                            dgLocalPlaylist.Rows[i].Cells[2].Value = "";
                                            dgLocalPlaylist.Rows[i].Cells[1].Style.ForeColor = Color.FromArgb(0, 0, 0);
                                            dgLocalPlaylist.Rows[i].Cells[1].Style.Font = new Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);
                                            dgLocalPlaylist.Rows[i].Cells[3].Style.BackColor = Color.White;
                                            dgLocalPlaylist.Rows[i].Cells[3].Style.SelectionBackColor = Color.White;

                                        }
                                    }

                                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                    OleDbCommand cmdUpdateAll = new OleDbCommand();
                                    cmdUpdateAll.Connection = StaticClass.LocalCon;
                                    cmdUpdateAll.CommandText = "Update Playlists set PlaylistDefault=''";
                                    cmdUpdateAll.ExecuteNonQuery();


                                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                    OleDbCommand cmdUpdate = new OleDbCommand();
                                    cmdUpdate.Connection = StaticClass.LocalCon;
                                    cmdUpdate.CommandText = "Update Playlists set PlaylistDefault='Default' where playlistid = " + dgLocalPlaylist.Rows[iRow].Cells["playlistId"].Value;
                                    cmdUpdate.ExecuteNonQuery();

                                    FirstTimeConditation = "No";
                                    EndTime = Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgLocalPlaylist.Rows[iRow].Cells["eTime"].Value)));
                                    StaticClass.DefaultPlaylistCurrentRow = iRow;
                                    PlaylistRow = iRow;
                                    StartPlaylist = 0;
                                    timStartPlaylistSchedule.Enabled = true;
                                    timPrayerClosing.Enabled = true;
                                    FindTime = "Yes";
                                    StaticClass.PlayerClosingTime = string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgLocalPlaylist.Rows[iRow].Cells["eTime"].Value));
                                    break;
                                }
                                #endregion
                            }
                        }
                    }
                    if (FindTime == "No")
                    {
                        StaticClass.IsPlayerClose = "Yes";
                        musicPlayer1.URL = "";
                        musicPlayer1.Ctlcontrols.stop();
                        musicPlayer2.URL = "";
                        musicPlayer2.Ctlcontrols.stop();

                        DisablePlayers();
                        timResetSong.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("ex" + ex.Message);
            }
        }

        private void timStartPlaylistSchedule_Tick(object sender, EventArgs e)
        {
        RunAg:
            if (StaticClass.IsPlayerClose == "Yes") return;
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

                if (lblSongCount.Text == "2")
                {
                    StartPlaylist = 20;
                    goto RunAg;
                }

                if (musicPlayer1.URL != "")
                {
                    musicPlayer1.Ctlcontrols.pause();
                }
                else if (musicPlayer2.URL != "")
                {
                    musicPlayer2.Ctlcontrols.pause();
                }


                #region Running Playlist

                dgLocalPlaylist.Rows[PlaylistRow].Selected = true;
                PopulateSplPlaylist(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[PlaylistRow].Cells["playlistId"].Value));



                PlaySongDefault();
                DisplaySongPlayerOne();
                Set_foucs_PayerOne();
                GetSavedRating(MusicPlayer1CurrentSongId.ToString(), dgSongRatingPlayerOne);
                SetDisableRating(dgSongRatingPlayerTwo);
                musicPlayer2.settings.volume = 0;
                musicPlayer2.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                //  PlaylistTime = "";
                rNetState = 0;

                timStartPlaylistSchedule.Enabled = false;

                #endregion
                if (timResetSong.Enabled == false)
                {
                    timResetSong.Enabled = true;
                }
            }
        }
        private void PopulateSplPlaylist(DataGridView dgGrid, Int32 currentPlayRow)
        {
            try
            {
                string mlsSql = "";
                string TitleYear = "";
                string TitleTime = "";
                var Special_Name = "";
                string Special_Change = "";
                Int32 iCtr = 0;
                Int32 srNo = 0;
                DataTable dtDetail = new DataTable();
                DataSet dtse = new DataSet();
                if (StaticClass.ScheduleType == "OneToOnePlaylist")
                {
                    mlsSql = " SELECT st.TitleID, ltrim(st.Title) as Title, st.Time, st.alname AS AlbumName ,";
                    mlsSql = mlsSql + " 0 as TitleYear ,  ltrim(st.arname) as ArtistName,  TitlesInPlaylists.id FROM (TitlesInPlaylists ";
                    mlsSql = mlsSql + " INNER JOIN tbSpecialPlaylists_Titles st ON TitlesInPlaylists.TitleID = st.TitleID )  ";
                    mlsSql = mlsSql + " ORDER BY  Rnd(-(date() * TitlesInPlaylists.Id) * Time()),Rnd(-(date() * TitlesInPlaylists.titleid) * Time())";
                }
                else
                {
                    mlsSql = "SELECT  Titles.TitleID, ltrim(Titles.Title) as Title, Titles.Time,Albums.Name AS AlbumName ,";
                    mlsSql = mlsSql + " Titles.TitleYear as TitleYear ,  ltrim(Artists.Name) as ArtistName , Titles.localUrl FROM ((( TitlesInPlaylists  ";
                    mlsSql = mlsSql + " INNER JOIN Titles ON TitlesInPlaylists.TitleID = Titles.TitleID )  ";
                    mlsSql = mlsSql + " INNER JOIN Albums ON Titles.AlbumID = Albums.AlbumID ) ";
                    mlsSql = mlsSql + " INNER JOIN Artists ON Titles.ArtistID = Artists.ArtistID ) ";
                    mlsSql = mlsSql + " where TitlesInPlaylists.PlaylistID=" + Convert.ToInt32(currentPlayRow) + " ORDER BY Rnd(-(100000*Titles.TitleID)*Time())";
                }

                dtDetail = ObjMainClass.fnFillDataTable_Local(mlsSql);
                InitilizeGrid(dgGrid);
                if ((dtDetail.Rows.Count > 0))
                {
                    for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                    {
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
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["lUrl"].Value = dtDetail.Rows[iCtr]["localUrl"];


                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["songname"].Style.Font = new Font("Segoe UI", 11, System.Drawing.FontStyle.Regular);
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Length"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Album"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);
                        dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["Artist"].Style.Font = new Font("Segoe UI", 9, System.Drawing.FontStyle.Regular);

                    }
                }
                foreach (DataGridViewRow row in dgGrid.Rows)
                {
                    row.Height = 30;
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        #endregion
        #endregion



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
            InitilizePrayerGrid(dgGrid);
            if ((dtDetail.Rows.Count > 0))
            {
                for (iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    dgGrid.Rows.Add();
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["pId"].Value = dtDetail.Rows[iCtr]["pId"];
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["sDate"].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["sDate"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["eDate"].Value = string.Format("{0:dd-MMM-yyyy}", dtDetail.Rows[iCtr]["eDate"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["sTime"].Value = string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["sTime"]);
                    dgGrid.Rows[dgGrid.Rows.Count - 1].Cells["eTime"].Value = string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["eTime"]);
                }
                foreach (DataGridViewRow row in dgGrid.Rows)
                {
                    row.Height = 30;
                }
                timPrayer.Enabled = true;
            }
        }




        string PrayerTime = "";
        string IsPrayerRunning = "No";
        private void timPrayer_Tick(object sender, EventArgs e)
        {
            if (StaticClass.IsPlayerClose == "Yes")
            {
                return;
            }
            if (PrayerTime != string.Format(fi, "{0:hh:mm tt}", DateTime.Now))
            {
                PrayerTime = string.Format(fi, "{0:hh:mm tt}", DateTime.Now);
                for (int iRow = 0; iRow < dgPrayer.Rows.Count; iRow++)
                {
                    if (Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) >= Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", dgPrayer.Rows[iRow].Cells["sTime"].Value)) && (Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) < Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", dgPrayer.Rows[iRow].Cells["eTime"].Value))))
                    {
                        if (IsPrayerRunning == "No")
                        {
                            Mute();
                            IsPrayerRunning = "Yes";

                        }
                        else
                        {
                            break;
                        }
                    }
                    if (Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) == Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", dgPrayer.Rows[iRow].Cells["eTime"].Value)))
                    {
                        UnMute();
                        IsPrayerRunning = "No";

                    }
                }
            }
        }
        string IsAlreadyMute = "No";
        string IsAlreadyMuteStream = "No";
        private void Mute()
        {
            timResetSong.Enabled = false;
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
            AdvtPlayer.settings.mute = true;
            Song_Mute = true;

            btnMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_red));


            string strPrayer = "";
            strPrayer = "";
            strPrayer = strPrayer + " insert into tbTokenPrayerStatus(TokenId,StatusDate,StatusTime,IsUpload) values( " + StaticClass.TokenId + ", ";
            strPrayer = strPrayer + "  '" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "', ";
            strPrayer = strPrayer + " '" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "',0)";

            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
            OleDbCommand cmdPrayerStatus = new OleDbCommand();
            cmdPrayerStatus.Connection = StaticClass.LocalCon;
            cmdPrayerStatus.CommandText = strPrayer;
            cmdPrayerStatus.ExecuteNonQuery();


        }
        private void UnMute()
        {
            if (IsAlreadyMute == "No")
            {
                btnMute.Text = "";
                musicPlayer1.settings.mute = false;
                musicPlayer2.settings.mute = false;
                if (StaticClass.IsVideoMute == "0")
                {
                    musicPlayer1.settings.volume = 100;
                    musicPlayer2.settings.volume = 100;
                }
                else
                {
                    musicPlayer1.settings.volume = 0;
                    musicPlayer2.settings.volume = 0;
                }
                Song_Mute = false;
                btnMute.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Mute_blue));
            }

            AdvtPlayer.settings.mute = false;
            timResetSong.Enabled = true;
        }
        #endregion

        private void timPrayerClosing_Tick(object sender, EventArgs e)
        {
            if (StaticClass.PlayerClosingTime == "") { timPrayerClosing.Enabled = false; return; }
            if (Convert.ToDateTime(StaticClass.PlayerClosingTime) == Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)))
            {
                StaticClass.IsPlayerClose = "Yes";
                //if (StaticClass.LocalCon.State == ConnectionState.Open) { StaticClass.LocalCon.Close(); }

                musicPlayer1.URL = "";
                musicPlayer1.Ctlcontrols.stop();
                musicPlayer2.URL = "";
                musicPlayer2.Ctlcontrols.stop();

                DisablePlayers();
                timResetSong.Enabled = false;


                picMiddleLogo.BringToFront();
                picMiddleLogo.Visible = true;




            }
        }

        private void GetAdvtPlayingType()
        {
            DataTable dtPlayType = new DataTable();
            string str = "select top 1 * from tbAdvertisement where #" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "# between AdvtStartDate and AdvtEndDate order by AdvtId desc";
            dtPlayType = ObjMainClass.fnFillDataTable_Local(str);
            if (dtPlayType.Rows.Count > 0)
            {
                if (Convert.ToInt32(dtPlayType.Rows[0]["IsTime"]) == 1)
                {
                    StaticClass.IsAdvtTime = true;
                    StaticClass.IsAdvtManual = false;
                    StaticClass.IsAdvtWithSongs = false;
                    StaticClass.TotalAdvtSongs = 1;
                    StaticClass.AdvtTime = 0;
                }
                else if (Convert.ToInt32(dtPlayType.Rows[0]["IsMinute"]) == 1)
                {
                    StaticClass.IsAdvtManual = true;
                    StaticClass.AdvtTime = (Convert.ToInt32(dtPlayType.Rows[0]["TotalMinutes"]) * 60);
                    StaticClass.IsAdvtWithSongs = false;
                    StaticClass.IsAdvtTime = false;
                    StaticClass.TotalAdvtSongs = 1;
                }
                else if (Convert.ToInt32(dtPlayType.Rows[0]["IsSong"]) == 1)
                {
                    StaticClass.IsAdvtTime = false;
                    StaticClass.IsAdvtManual = false;
                    StaticClass.IsAdvtWithSongs = true;
                    StaticClass.TotalAdvtSongs = Convert.ToInt32(dtPlayType.Rows[0]["TotalSongs"]);
                    StaticClass.AdvtTime = 0;
                }
                if (Convert.ToInt32(dtPlayType.Rows[0]["IsBetween"]) == 1)
                {
                    StaticClass.IsAdvtTime = false;
                    StaticClass.IsAdvtManual = false;
                    StaticClass.IsAdvtWithSongs = false;
                    StaticClass.TotalAdvtSongs = 1;
                    StaticClass.AdvtTime = 0;
                    StaticClass.IsAdvtBetweenTime = true;
                    StaticClass.AdvtClosingTime = string.Format(fi, "{0:hh:mm tt}", DateTime.Now);
                }

            }
        }
        private void UploadPlayerStatus()
        {
            string strZ = "";
            try
            {
                string strTotal = "select * from tbTokenPlayedSongs where playDate=#" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "# and isUpload=0";
                DataTable dtGet = new DataTable();
                dtGet = ObjMainClass.fnFillDataTable_Local(strTotal);

                if ((dtGet.Rows.Count > 0))
                {
                    for (int iCtr = 0; (iCtr <= (dtGet.Rows.Count - 1)); iCtr++)
                    {
                        strZ = "insert into tbTokenPlayedSongs(Tokenid,PlayDTP,TitleId,ArtistId,splPlaylistId) values(" + dtGet.Rows[iCtr]["Tokenid"] + " , ";
                        strZ = strZ + " '" + string.Format("{0:dd-MMM-yyyy}", dtGet.Rows[iCtr]["PlayDate"]) + " " + string.Format(fi, "{0:hh:mm tt}", dtGet.Rows[iCtr]["PlayTime"]) + "', " + dtGet.Rows[iCtr]["TitleID"] + " ,";
                        strZ = strZ + " " + dtGet.Rows[iCtr]["ArtistId"] + " , " + dtGet.Rows[iCtr]["splPlaylistId"] + " )";
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = strZ;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdUpdateAll = new OleDbCommand();
                        cmdUpdateAll.Connection = StaticClass.LocalCon;
                        cmdUpdateAll.CommandText = "Update tbTokenPlayedSongs set isUpload=1 where id=" + dtGet.Rows[iCtr]["id"];
                        cmdUpdateAll.ExecuteNonQuery();
                    }
                }


                strTotal = "";
                strTotal = "select * from tbTokenOverDueStatus where StatusDate=#" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "# and isUpload=0";
                dtGet = new DataTable();
                dtGet = ObjMainClass.fnFillDataTable_Local(strTotal);
                strZ = "";
                if ((dtGet.Rows.Count > 0))
                {
                    for (int iCtr = 0; (iCtr <= (dtGet.Rows.Count - 1)); iCtr++)
                    {
                        strZ = "insert into tbTokenOverDueStatus(Tokenid,StatusDateTime) values(" + dtGet.Rows[iCtr]["Tokenid"] + " , ";
                        strZ = strZ + " '" + string.Format("{0:dd-MMM-yyyy}", dtGet.Rows[iCtr]["StatusDate"]) + " " + string.Format(fi, "{0:hh:mm tt}", dtGet.Rows[iCtr]["StatusTime"]) + "') ";
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = strZ;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdUpdateAll = new OleDbCommand();
                        cmdUpdateAll.Connection = StaticClass.LocalCon;
                        cmdUpdateAll.CommandText = "Update tbTokenOverDueStatus set isUpload=1 where id=" + dtGet.Rows[iCtr]["id"];
                        cmdUpdateAll.ExecuteNonQuery();
                    }
                }

                #region Upload Advt Status
                strTotal = "";
                strTotal = "select * from tbTokenAdvtStatus where StatusDate=#" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "# and isUpload=0";
                dtGet = new DataTable();
                dtGet = ObjMainClass.fnFillDataTable_Local(strTotal);
                strZ = "";
                if ((dtGet.Rows.Count > 0))
                {
                    for (int iCtr = 0; (iCtr <= (dtGet.Rows.Count - 1)); iCtr++)
                    {
                        strZ = "insert into tbTokenAdvtStatus(TokenId,AdvtId,StatusDate,StatusTime) values(" + dtGet.Rows[iCtr]["Tokenid"] + " , " + dtGet.Rows[iCtr]["AdvtId"] + ", ";
                        strZ = strZ + " '" + string.Format("{0:dd-MMM-yyyy}", dtGet.Rows[iCtr]["StatusDate"]) + "','" + string.Format(fi, "{0:hh:mm tt}", dtGet.Rows[iCtr]["StatusTime"]) + "') ";
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = strZ;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdUpdateAll = new OleDbCommand();
                        cmdUpdateAll.Connection = StaticClass.LocalCon;
                        cmdUpdateAll.CommandText = "Update tbTokenAdvtStatus set isUpload=1 where id=" + dtGet.Rows[iCtr]["id"];
                        cmdUpdateAll.ExecuteNonQuery();
                    }
                }
                #endregion

                #region Upload Login Status
                strTotal = "";
                strTotal = "select * from tbTokenLoginStatus where StatusDate=#" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "# and isUpload=0";
                dtGet = new DataTable();
                dtGet = ObjMainClass.fnFillDataTable_Local(strTotal);
                strZ = "";
                if ((dtGet.Rows.Count > 0))
                {
                    for (int iCtr = 0; (iCtr <= (dtGet.Rows.Count - 1)); iCtr++)
                    {
                        strZ = "insert into tbTokenLoginStatus(TokenId,StatusDate,StatusTime) values(" + dtGet.Rows[iCtr]["Tokenid"] + " , ";
                        strZ = strZ + " '" + string.Format("{0:dd-MMM-yyyy}", dtGet.Rows[iCtr]["StatusDate"]) + "','" + string.Format(fi, "{0:hh:mm tt}", dtGet.Rows[iCtr]["StatusTime"]) + "') ";
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = strZ;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdUpdateAll = new OleDbCommand();
                        cmdUpdateAll.Connection = StaticClass.LocalCon;
                        cmdUpdateAll.CommandText = "Update tbTokenLoginStatus set isUpload=1 where id=" + dtGet.Rows[iCtr]["id"];
                        cmdUpdateAll.ExecuteNonQuery();
                    }
                }
                #endregion

                #region Upload Prayer Status
                strTotal = "";
                strTotal = "select * from tbTokenPrayerStatus where StatusDate=#" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "# and isUpload=0";
                dtGet = new DataTable();
                dtGet = ObjMainClass.fnFillDataTable_Local(strTotal);
                strZ = "";
                if ((dtGet.Rows.Count > 0))
                {
                    for (int iCtr = 0; (iCtr <= (dtGet.Rows.Count - 1)); iCtr++)
                    {
                        strZ = "insert into tbTokenPrayerStatus(TokenId,StatusDate,StatusTime) values(" + dtGet.Rows[iCtr]["Tokenid"] + " , ";
                        strZ = strZ + " '" + string.Format("{0:dd-MMM-yyyy}", dtGet.Rows[iCtr]["StatusDate"]) + "','" + string.Format(fi, "{0:hh:mm tt}", dtGet.Rows[iCtr]["StatusTime"]) + "') ";
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = StaticClass.constr;
                        cmd.CommandText = strZ;
                        cmd.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdUpdateAll = new OleDbCommand();
                        cmdUpdateAll.Connection = StaticClass.LocalCon;
                        cmdUpdateAll.CommandText = "Update tbTokenPrayerStatus set isUpload=1 where id=" + dtGet.Rows[iCtr]["id"];
                        cmdUpdateAll.ExecuteNonQuery();
                    }
                }
                #endregion


            }
            catch (Exception ex)
            {

            }
        }
        Int32 AdvtTime = 0;
        Int32 PlayNowWeb = 0;
        string CrDate = string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date);
        string IsExitApp = "No";
        private void timRefershData_Tick(object sender, EventArgs e)
        {
            AdvtTime = AdvtTime + 1;
            PlayNowWeb = PlayNowWeb + 1;

            if (PlayNowWeb == 10)
            {
                dtPlayNow = new DataTable();
                if (HttpRuntime.Cache["PlayNow_Web"] != null)
                {
                    dtPlayNow = (DataTable)HttpRuntime.Cache["PlayNow_Web"];
                }
                if (dtPlayNow.Rows.Count > 0)
                {
                    PlayNowOption();
                    if (bgPlayNow.IsBusy == false)
                    {
                        bgPlayNow.RunWorkerAsync();
                    }
                }
                PlayNowWeb = 0;
            }
            if (CrDate != string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date))
            {
                AdvtTime = 600;
            }
            if (AdvtTime >= 600)
            {
                ObjMainClass.CompactLocaldb();
                string strZ = "";
                double w1;
                double t1;
                double mint1 = 0;
                if (musicPlayer1.URL != "")
                {

                    t1 = Math.Floor(musicPlayer1.currentMedia.duration - musicPlayer1.Ctlcontrols.currentPosition);
                    w1 = Math.Floor(musicPlayer1.Ctlcontrols.currentPosition);
                    mint1 = Math.Floor(t1 / 60);
                    if (mint1 < 1)
                    {
                        AdvtTime = 510;
                        return;
                    }
                }
                if (musicPlayer2.URL != "")
                {

                    t1 = Math.Floor(musicPlayer2.currentMedia.duration - musicPlayer2.Ctlcontrols.currentPosition);
                    w1 = Math.Floor(musicPlayer2.Ctlcontrols.currentPosition);
                    mint1 = Math.Floor(t1 / 60);
                    if (mint1 < 1)
                    {
                        AdvtTime = 510;
                        return;
                    }
                }
                if ((musicPlayer1.URL != "") && (musicPlayer2.URL != ""))
                {
                    AdvtTime = 510;
                    return;
                }
                AdvtTime = 0;



                if (wcDownloadSplSongs.IsBusy == true)
                {
                    return;
                }


                if (ObjMainClass.CheckForInternetConnection() == true)
                {

                    UploadPlayerStatus();

                    if (lblSongCount.Text == "2")
                    {
                        AdvtTime = 510;
                        return;
                    }
                    if (myWebClientAdvt.IsBusy == false)
                    {
                        GetAdvertisement();
                        FillMainAdvertisement();
                        FillAllAdvertisement();
                        DownloadAdvt();
                    }
                    if (btnMute.Text == ".")
                    {
                        AdvtTime = 510;
                        return;
                    }
                    GetPrayer();
                    if (StaticClass.IsStore == true)
                    {
                        if (CrDate != string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date))
                        {
                            CrDate = string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date);

                            m_notifyicon.Dispose();
                            Process.Start(Application.StartupPath + "\\SFVideoPlayer.exe");
                            AdvtTime = 0;
                            timRefershData.Enabled = false;
                            IsExitApp = "Yes";
                            Application.Exit();
                            return;

                            //if (IsFormatFirstTimeLoad == "No")
                            //{
                            //    sTime = 1;
                            //    timResetSong.Enabled = false;
                            //    timGetSplPlaylist.Enabled = true;
                            //}
                        }
                    }
                }
            }
        }
        private void GetPrayer()
        {
            DataTable dtDetail = new DataTable();
            string str = "spGetPrayerData " + DateTime.Now.Date.Month + " ," + StaticClass.AdvtCityId + "," + StaticClass.CountryId + ", " + StaticClass.Stateid + ", " + StaticClass.TokenId;
            dtDetail = ObjMainClass.fnFillDataTable(str);
            if ((dtDetail.Rows.Count > 0))
            {
                str = "";
                str = "delete from tbPrayer";
                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                OleDbCommand cmdDel = new OleDbCommand();
                cmdDel.Connection = StaticClass.LocalCon;
                cmdDel.CommandText = str;
                cmdDel.ExecuteNonQuery();
                for (int iCtr = 0; (iCtr <= (dtDetail.Rows.Count - 1)); iCtr++)
                {
                    str = "";
                    str = "insert into tbPrayer(pId,sDate,eDate,sTime,eTime) values(" + dtDetail.Rows[iCtr]["pId"] + ", #" + string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["sDate"]) + "# ,";
                    str = str + " #" + string.Format("{0:dd/MMM/yyyy}", dtDetail.Rows[iCtr]["eDate"]) + "# ,#" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["sTime"]) + "#, #" + string.Format(fi, "{0:hh:mm tt}", dtDetail.Rows[iCtr]["eTime"]) + "# )";
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    OleDbCommand cmdIns = new OleDbCommand();
                    cmdIns.Connection = StaticClass.LocalCon;
                    cmdIns.CommandText = str;
                    cmdIns.ExecuteNonQuery();
                }

            }
            FillPrayer(dgPrayer);
        }
        private void DisablePlayers()
        {
            lblSongName.ForeColor = Color.Gray;
            lblArtistName.ForeColor = Color.Gray;
            lblMusicTimeOne.ForeColor = Color.Gray;
            lblSongDurationOne.ForeColor = Color.Gray;
            pbarMusic1.ForeColor = Color.Gray;
            pbarMusic1.BackColor = Color.FromArgb(175, 175, 175);
            //panMusicOne.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));

            lblSongName2.ForeColor = Color.Gray;
            lblArtistName2.ForeColor = Color.Gray;
            lblMusicTimeTwo.ForeColor = Color.Gray;
            lblSongDurationTwo.ForeColor = Color.Gray;
            pbarMusic2.ForeColor = Color.Gray;
            pbarMusic2.BackColor = Color.FromArgb(175, 175, 175);
            //panMusicTwo.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.DisablePlayer));
            SetDisableRating(dgSongRatingPlayerOne);
            SetDisableRating(dgSongRatingPlayerTwo);
        }



        private void InitilizeAdvtTempGrid()
        {
            if (dgAdvtTemp.Rows.Count > 0)
            {
                dgAdvtTemp.Rows.Clear();
            }
            if (dgAdvtTemp.Columns.Count > 0)
            {
                dgAdvtTemp.Columns.Clear();
            }
            dgAdvtTemp.Columns.Add("Advtid", "Advt Id");
            dgAdvtTemp.Columns["Advtid"].Width = 0;
            dgAdvtTemp.Columns["Advtid"].Visible = false;
            dgAdvtTemp.Columns["Advtid"].ReadOnly = true;

            dgAdvtTemp.Columns.Add("Advt", "Advertisement Name");
            dgAdvtTemp.Columns["Advt"].Width = 245;
            dgAdvtTemp.Columns["Advt"].Visible = true;
            dgAdvtTemp.Columns["Advt"].ReadOnly = true;

            dgAdvtTemp.Columns.Add("AdvtComp", "Advt Comp");
            dgAdvtTemp.Columns["AdvtComp"].Width = 0;
            dgAdvtTemp.Columns["AdvtComp"].Visible = false;
            dgAdvtTemp.Columns["AdvtComp"].ReadOnly = true;

            dgAdvtTemp.Columns.Add("AdvtLink", "AdvtLink");
            dgAdvtTemp.Columns["AdvtLink"].Width = 0;
            dgAdvtTemp.Columns["AdvtLink"].Visible = false;
            dgAdvtTemp.Columns["AdvtLink"].ReadOnly = true;


            dgAdvtTemp.Columns.Add("Play", "Play");
            dgAdvtTemp.Columns["Play"].Width = 0;
            dgAdvtTemp.Columns["Play"].Visible = false;
            dgAdvtTemp.Columns["Play"].ReadOnly = true;

            dgAdvtTemp.Columns.Add("IsVideo", "IsVideo");
            dgAdvtTemp.Columns["IsVideo"].Width = 0;
            dgAdvtTemp.Columns["IsVideo"].Visible = false;
            dgAdvtTemp.Columns["IsVideo"].ReadOnly = true;


            dgAdvtTemp.Columns.Add("IsVideoMute", "IsVideoMute");
            dgAdvtTemp.Columns["IsVideoMute"].Width = 0;
            dgAdvtTemp.Columns["IsVideoMute"].Visible = false;
            dgAdvtTemp.Columns["IsVideoMute"].ReadOnly = true;

            dgAdvtTemp.Columns.Add("IsPicture", "IsPicture");
            dgAdvtTemp.Columns["IsPicture"].Width = 0;
            dgAdvtTemp.Columns["IsPicture"].Visible = false;
            dgAdvtTemp.Columns["IsPicture"].ReadOnly = true;
            dgAdvtTemp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }
        private void FillAdvtTempData()
        {
            DataTable dtDetailLocal;
            string str = "select * from tbAdvt where ScheduleDate=#" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "#";
            dtDetailLocal = ObjMainClass.fnFillDataTable_Local(str);
            InitilizeAdvtTempGrid();
            for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
            {

                if ((Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) >= Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgAdvt.Rows[iRow].Cells["bStime"].Value)))) && (Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", DateTime.Now)) < Convert.ToDateTime(string.Format(fi, "{0:hh:mm tt}", Convert.ToDateTime(dgAdvt.Rows[iRow].Cells["bEtime"].Value)))))
                {
                    if (dgAdvt.Rows[iRow].Cells["Status"].Style.BackColor == Color.LightGreen)
                    {
                        dgAdvtTemp.Rows.Add();
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["Advtid"].Value = dgAdvt.Rows[iRow].Cells["Advtid"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["Advt"].Value = dgAdvt.Rows[iRow].Cells["Advt"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["AdvtComp"].Value = dgAdvt.Rows[iRow].Cells["AdvtComp"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["AdvtLink"].Value = dgAdvt.Rows[iRow].Cells["AdvtLink"].Value;

                        bool exists = dtDetailLocal.Select().ToList().Exists(row => row["AdvtId"].ToString() == dgAdvt.Rows[iRow].Cells["Advtid"].Value.ToString());
                        if (exists == true)
                        {
                            dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["Play"].Value = "Done";
                        }
                        else
                        {
                            dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["Play"].Value = "a";
                        }
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["IsVideo"].Value = dgAdvt.Rows[iRow].Cells["IsVideo"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["IsVideoMute"].Value = dgAdvt.Rows[iRow].Cells["IsVideoMute"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["IsPicture"].Value = dgAdvt.Rows[iRow].Cells["IsPicture"].Value;
                    }
                }
            }
            dgAdvtTemp.Visible = false;
            //dgAdvtTemp.BringToFront();

            //dgAdvtTemp.Location = new Point(3, 130);
        }

        private void FillAdvtTempDataSingle()
        {
            DataTable dtDetailLocal;
            string str = "select * from tbAdvt where ScheduleDate=#" + string.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date) + "#";
            dtDetailLocal = ObjMainClass.fnFillDataTable_Local(str);
            InitilizeAdvtTempGrid();
            for (int iRow = 0; iRow < dgAdvt.Rows.Count; iRow++)
            {

                if (string.Format(fi, "{0:hh:mm tt}", DateTime.Now) == string.Format(fi, "{0:hh:mm tt}", dgAdvt.Rows[iRow].Cells["bStime"].Value))
                {
                    if (dgAdvt.Rows[iRow].Cells["Status"].Style.BackColor == Color.LightGreen)
                    {
                        dgAdvtTemp.Rows.Add();
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["Advtid"].Value = dgAdvt.Rows[iRow].Cells["Advtid"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["Advt"].Value = dgAdvt.Rows[iRow].Cells["Advt"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["AdvtComp"].Value = dgAdvt.Rows[iRow].Cells["AdvtComp"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["AdvtLink"].Value = dgAdvt.Rows[iRow].Cells["AdvtLink"].Value;

                        bool exists = dtDetailLocal.Select().ToList().Exists(row => row["AdvtId"].ToString() == dgAdvt.Rows[iRow].Cells["Advtid"].Value.ToString());
                        if (exists == true)
                        {
                            dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["Play"].Value = "Done";
                        }
                        else
                        {
                            dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["Play"].Value = "a";
                        }
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["IsVideo"].Value = dgAdvt.Rows[iRow].Cells["IsVideo"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["IsVideoMute"].Value = dgAdvt.Rows[iRow].Cells["IsVideoMute"].Value;
                        dgAdvtTemp.Rows[dgAdvtTemp.Rows.Count - 1].Cells["IsPicture"].Value = dgAdvt.Rows[iRow].Cells["IsPicture"].Value;
                    }
                }
            }
            dgAdvtTemp.Visible = false;
            //dgAdvtTemp.BringToFront();

            //dgAdvtTemp.Location = new Point(3, 130);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Int32 CurrrentPos;
            if (musicPlayer1.URL != "")
            {
                double t = Math.Floor(musicPlayer1.currentMedia.duration);
                CurrrentPos = (Convert.ToInt32(t) - 30);
                musicPlayer1.Ctlcontrols.currentPosition = CurrrentPos;
            }
            if (musicPlayer2.URL != "")
            {
                double t = Math.Floor(musicPlayer2.currentMedia.duration);
                CurrrentPos = (Convert.ToInt32(t) - 30);
                musicPlayer2.Ctlcontrols.currentPosition = CurrrentPos;
            }

        }

        void wcDownloadSplSongs_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Maximum = (int)e.TotalBytesToReceive / 100;
            progressBar1.Value = (int)e.BytesReceived / 100;
        }
        private void bgDownloadSplSongs()
        {
            IsDeleteAllOgg = false;
            String RemoteFtpPath = "";



            RemoteFtpPath = SplSongUrl;
            String LocalDestinationPath = Application.StartupPath + "\\so\\" + SplSongName + Path.GetExtension(SplSongUrl);
            try
            {
                wcDownloadSplSongs.DownloadFileAsync(new Uri(RemoteFtpPath), LocalDestinationPath);
            }
            catch (Exception ex)
            {
                IsDeleteAllOgg = true;
                return;
            }

        }
        private void wcDownloadSplSongs_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string lFilName = "";
            string sQr = "";
            DataSet dtGetRecord;
            DataSet dtGetDefault;
            string strGet = "";
            string PlaylistName = "";
            Int32 Playlist_Id = 0;
            GC.Collect();
            try
            {

                wcDownloadSplSongs.Dispose();


                progressBar1.Value = 0;
                if (IsDeleteAllOgg == true)
                {
                    picSplGif.Visible = false;
                    //DownloadAdvt();
                }


                lFilName = Application.StartupPath + "\\so\\" + SplSongName + Path.GetExtension(SplSongUrl);
                if (File.Exists(lFilName))
                {
                    Int32 TitleSize = 0;
                    sQr = "";
                    sQr = "select distinct filesize from tbSpecialPlaylists_Titles where titleid=" + SplSongName;
                    DataTable dt = new DataTable();
                    dt = ObjMainClass.fnFillDataTable_Local(sQr);
                    if (dt.Rows.Count > 0)
                    {
                        if (string.IsNullOrEmpty(dt.Rows[0]["Filesize"].ToString()) == false)
                        {
                            TitleSize = Convert.ToInt32(dt.Rows[0]["Filesize"]);
                        }
                        else
                        {
                            TitleSize = 0;
                        }
                    }

                    long DownloadedSize = new FileInfo(lFilName).Length;
                    //56563877
                    if (TitleSize == DownloadedSize)
                    {
                        sQr = "update tbSpecialPlaylists_Titles set isDownload=1 where titleId=" + SplSongName;
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        OleDbCommand cmdDel = new OleDbCommand();
                        cmdDel.Connection = StaticClass.LocalCon;
                        cmdDel.CommandText = sQr;
                        cmdDel.ExecuteNonQuery();
                    }

                }




                #region Save Records
                strGet = "";
                string Special_Name = "";
                string Special_Change = "";
                strGet = "SELECT * FROM tbSplPlaylistSchedule where schid =" + StaticClass.SchId;
                dtGetRecord = new DataSet();
                dtGetRecord = ObjMainClass.fnFillDataSet_Local(strGet);
                if (Convert.ToInt32(dtGetRecord.Tables[0].Rows[0]["playlistid"]) == 0)
                {

                    #region Insert Record
                    PlaylistName = dtGetRecord.Tables[0].Rows[0]["splName"].ToString();
                    #region SplPlaylistSave

                    if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                    StaticClass.constr.Open();
                    SqlCommand cmd = new SqlCommand("InsertPlayListsNew", StaticClass.constr);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.BigInt));
                    cmd.Parameters["@UserID"].Value = StaticClass.dfClientId;
                    cmd.Parameters.Add(new SqlParameter("@IsPredefined", SqlDbType.Bit));
                    cmd.Parameters["@IsPredefined"].Value = 0;
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Name"].Value = PlaylistName;
                    cmd.Parameters.Add(new SqlParameter("@Summary", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Summary"].Value = " ";
                    cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar, 50));
                    cmd.Parameters["@Description"].Value = " ";
                    cmd.Parameters.Add(new SqlParameter("@TokenId", SqlDbType.BigInt));
                    cmd.Parameters["@TokenId"].Value = StaticClass.TokenId;
                    try
                    {
                        Playlist_Id = Convert.ToInt32(cmd.ExecuteScalar());
                        sQr = "";
                        if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                        sQr = "insert into PlayLists values(" + Convert.ToInt32(Playlist_Id) + ", ";
                        sQr = sQr + StaticClass.dfClientId + " , '" + PlaylistName + "', " + StaticClass.TokenId + ",'',1 )";

                        OleDbCommand cmdSaveLocal = new OleDbCommand();
                        cmdSaveLocal.Connection = StaticClass.LocalCon;
                        cmdSaveLocal.CommandText = sQr;
                        cmdSaveLocal.ExecuteNonQuery();


                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show("bgSaveSplPlaylist_DoWork Playlist " + ex.Message);

                    }
                    finally
                    {
                        StaticClass.constr.Close();
                    }
                    #endregion
                    sQr = "";
                    sQr = "update tbSplPlaylistSchedule set PlaylistId=" + Playlist_Id + " where SchId=" + dtGetRecord.Tables[0].Rows[0]["SchId"].ToString();
                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    OleDbCommand cmdDel = new OleDbCommand();
                    cmdDel.Connection = StaticClass.LocalCon;
                    cmdDel.CommandText = sQr;
                    cmdDel.ExecuteNonQuery();

                    if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                    string sWr1 = "delete from  TitlesInPlaylists where PlaylistID = " + Playlist_Id;
                    OleDbCommand cmdP1 = new OleDbCommand();
                    cmdP1.Connection = StaticClass.LocalCon;
                    cmdP1.CommandText = sWr1;
                    cmdP1.ExecuteNonQuery();

                    #region insert_SplPlaylist_song_LocalDatabase
                    string sWr = "";

                    sQr = "";

                    DataSet ds = new DataSet();
                    try
                    {

                        sQr = "";
                        sQr = "select * from tbSpecialPlaylists_Titles where SchId=" + StaticClass.SchId + " and isDownload=1";
                        DataSet dsMain = new DataSet();
                        dsMain = ObjMainClass.fnFillDataSet_Local(sQr);
                        for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
                        {
                            sQr = "select * from Titles where TitleID=" + dsMain.Tables[0].Rows[i]["titleId"];
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            #region Save Title
                            if (ds.Tables[0].Rows.Count <= 0)
                            {

                                Special_Name = dsMain.Tables[0].Rows[i]["title"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");

                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                //sWr = "insert into Titles values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["TitleID"]) + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                //sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' , ";
                                //sWr = sWr + " '0' , '" + dsMain.Tables[0].Rows[i]["Time"] + "' ,0)";

                                sWr = "insert into Titles (TitleID,AlbumID,ArtistID,Title,Gain,[Time],TitleYear,HttpUrl,LocalUrl) values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["TitleID"]) + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' , ";
                                sWr = sWr + " '0' , '" + dsMain.Tables[0].Rows[i]["Time"] + "' ,0 ,";
                                sWr = sWr + " '" + dsMain.Tables[0].Rows[i]["HttpUrl"] + "', ";
                                sWr = sWr + " '" + dsMain.Tables[0].Rows[i]["LocalUrl"] + "' )";

                                OleDbCommand cmdTitle = new OleDbCommand();
                                cmdTitle.Connection = StaticClass.LocalCon;
                                cmdTitle.CommandText = sWr;
                                cmdTitle.ExecuteNonQuery();

                            }
                            #endregion

                            #region SaveAlbum

                            Special_Name = dsMain.Tables[0].Rows[i]["alName"].ToString();
                            Special_Change = Special_Name.Replace("'", "??$$$??");

                            sQr = "select * from Albums where albumid=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]);
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into Albums values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                                OleDbCommand cmdAlbum = new OleDbCommand();
                                cmdAlbum.Connection = StaticClass.LocalCon;
                                cmdAlbum.CommandText = sWr;
                                cmdAlbum.ExecuteNonQuery();
                            }
                            #endregion

                            #region Save Artist
                            sQr = "select * from Artists where ArtistID=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]);
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                Special_Name = dsMain.Tables[0].Rows[i]["arName"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");

                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into Artists values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                                OleDbCommand cmdAlbum = new OleDbCommand();
                                cmdAlbum.Connection = StaticClass.LocalCon;
                                cmdAlbum.CommandText = sWr;
                                cmdAlbum.ExecuteNonQuery();
                            }
                            #endregion

                            #region Save TitlesInPlaylists



                            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                            sWr = "insert into TitlesInPlaylists(PlaylistID,TitleID,SrNo) values (" + Playlist_Id + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]) + ", " + Convert.ToInt32(i + 1) + ")";
                            OleDbCommand cmdP = new OleDbCommand();
                            cmdP.Connection = StaticClass.LocalCon;
                            cmdP.CommandText = sWr;
                            cmdP.ExecuteNonQuery();


                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        //                        MessageBox.Show("bgSaveSplPlaylist_DoWork Detail" + ex.Message);
                    }
                    #endregion

                    #endregion
                    FillLocalPlaylist();
                    if (dgLocalPlaylist.Rows.Count >= 0)
                    {
                        PopulateSplPlaylist(dgPlaylist, Convert.ToInt32(dgLocalPlaylist.Rows[0].Cells["playlistId"].Value));
                    }
                    if (timGetSplPlaylistScheduleTime.Enabled == false)
                    {
                        timGetSplPlaylistScheduleTime.Enabled = true;
                    }
                }
                else
                {

                    Playlist_Id = Convert.ToInt32(dtGetRecord.Tables[0].Rows[0]["playlistid"]);

                    #region insert_SplPlaylist_song_LocalDatabase
                    string sWr = "";

                    sQr = "";

                    DataSet ds = new DataSet();
                    try
                    {
                        sQr = "";
                        sQr = "select * from tbSpecialPlaylists_Titles where SchId=" + StaticClass.SchId + " and IsDownload=1 and titleid not in (select titleid from TitlesInPlaylists where PlaylistID= " + Playlist_Id + ")";
                        DataSet dsMain = new DataSet();
                        dsMain = ObjMainClass.fnFillDataSet_Local(sQr);
                        for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
                        {

                            #region Save Title
                            sQr = "select * from Titles where TitleID=" + dsMain.Tables[0].Rows[i]["titleId"];
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                Special_Name = dsMain.Tables[0].Rows[i]["title"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");

                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                //sWr = "insert into Titles values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["TitleID"]) + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                //sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' , ";
                                //sWr = sWr + " '0' , '" + dsMain.Tables[0].Rows[i]["Time"] + "' ,0)";

                                sWr = "insert into Titles (TitleID,AlbumID,ArtistID,Title,Gain,[Time],TitleYear,HttpUrl,LocalUrl) values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["TitleID"]) + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' , ";
                                sWr = sWr + " '0' , '" + dsMain.Tables[0].Rows[i]["Time"] + "' ,0 ,";
                                sWr = sWr + " '" + dsMain.Tables[0].Rows[i]["HttpUrl"] + "', ";
                                sWr = sWr + " '" + dsMain.Tables[0].Rows[i]["LocalUrl"] + "' )";
                                OleDbCommand cmdTitle = new OleDbCommand();
                                cmdTitle.Connection = StaticClass.LocalCon;
                                cmdTitle.CommandText = sWr;
                                cmdTitle.ExecuteNonQuery();

                            }
                            #endregion

                            #region SaveAlbum
                            sQr = "select * from Albums where albumid=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]);
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                Special_Name = dsMain.Tables[0].Rows[i]["alName"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");

                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into Albums values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["AlbumID"]) + " , ";
                                sWr = sWr + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                                OleDbCommand cmdAlbum = new OleDbCommand();
                                cmdAlbum.Connection = StaticClass.LocalCon;
                                cmdAlbum.CommandText = sWr;
                                cmdAlbum.ExecuteNonQuery();

                            }
                            #endregion

                            #region Save Artist
                            sQr = "select * from Artists where ArtistID=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]);
                            ds = ObjMainClass.fnFillDataSet_Local(sQr);
                            if (ds.Tables[0].Rows.Count <= 0)
                            {
                                Special_Name = dsMain.Tables[0].Rows[i]["arName"].ToString();
                                Special_Change = Special_Name.Replace("'", "??$$$??");
                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into Artists values (" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["ArtistID"]) + ", '" + Special_Change + "' ) ";
                                OleDbCommand cmdAlbum = new OleDbCommand();
                                cmdAlbum.Connection = StaticClass.LocalCon;
                                cmdAlbum.CommandText = sWr;
                                cmdAlbum.ExecuteNonQuery();

                            }
                            #endregion

                            #region Save TitlesInPlaylists



                            strGet = "";
                            strGet = "select * from TitlesInPlaylists where PlaylistID= " + Playlist_Id + " and TitleID=" + Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]);
                            dtGetDefault = ObjMainClass.fnFillDataSet_Local(strGet);
                            if (dtGetDefault.Tables[0].Rows.Count <= 0)
                            {
                                if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                                sWr = "insert into TitlesInPlaylists(PlaylistID,TitleID,SrNo) values (" + Playlist_Id + " , " + Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]) + ", " + Convert.ToInt32(i + 1) + ")";
                                OleDbCommand cmdP = new OleDbCommand();
                                cmdP.Connection = StaticClass.LocalCon;
                                cmdP.CommandText = sWr;
                                cmdP.ExecuteNonQuery();
                            }
                            #endregion

                            #region Add Titles in Grid
                            strGet = "";
                            strGet = "SELECT PlaylistDefault FROM Playlists where playlistid =" + Playlist_Id;
                            dtGetDefault = new DataSet();
                            dtGetDefault = ObjMainClass.fnFillDataSet_Local(strGet);
                            if (dtGetDefault.Tables[0].Rows[0]["PlaylistDefault"].ToString() == "Default")
                            {
                                Boolean isTitleFind = false;
                                for (int iSpl = 0; iSpl < dgPlaylist.Rows.Count; iSpl++)
                                {
                                    if (Convert.ToInt32(dgPlaylist.Rows[iSpl].Cells["songid"].Value) == Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]))
                                    {
                                        isTitleFind = true;
                                        break;
                                    }
                                }
                                if (isTitleFind == false)
                                {
                                    AddSongsInGrid(dgPlaylist, Convert.ToInt32(dsMain.Tables[0].Rows[i]["titleId"]));
                                }
                            }
                            #endregion

                            if (FirstTimeSong == true)
                            {
                                if (dgPlaylist.Rows.Count > 1)
                                {
                                    NextSongDisplay(dgPlaylist.Rows[1].Cells[0].Value.ToString());
                                    FirstTimeSong = false;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // MessageBox.Show("bgSaveSplPlaylist_DoWork Detail" + ex.Message);
                    }
                    #endregion
                }

                GetSplSongCounter(Playlist_Id);
                #endregion

                if (dgSpl.Rows.Count > 0)
                {
                    DownloadSplSongs();
                }
                else
                {
                    string Sql = "SELECT * FROM tbSpecialPlaylists_Titles WHERE isdownload=0";
                    DataSet ds = new DataSet();
                    ds = ObjMainClass.fnFillDataSet_Local(Sql);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        sTime = 0;
                        timResetSplDownloading.Enabled = true;
                    }
                    else
                    {
                        sTime = 0;
                        //progressBar1.Value = 0;
                        //lblPercentage.Text = "";
                        SaveDownloadPlaylist("Download");
                        picSplGif.Visible = false;
                        //DownloadAdvt();

                        timResetSplDownloading.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //  MessageBox.Show("bgDownloadSplSongs_RunWorkerCompleted " + ex.Message);
            }
        }

        private void lblExit_Click(object sender, EventArgs e)
        {

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show("Are you sure to exit ?", "Music Player", buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                
                try
                {
                    if (ObjMainClass.CheckForInternetConnection() == true)
                    {
                        UploadPlayerStatus();
                        #region Upload LogOut Status

                        string strZ = "insert into tbTokenLogOutStatus(TokenId,StatusDate,StatusTime) values(" + StaticClass.TokenId + " , ";
                        strZ = strZ + " '" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "','" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "') ";
                        if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                        StaticClass.constr.Open();
                        SqlCommand cmdLog = new SqlCommand();
                        cmdLog.Connection = StaticClass.constr;
                        cmdLog.CommandText = strZ;
                        cmdLog.ExecuteNonQuery();
                        StaticClass.constr.Close();

                        #endregion
                    }
                }
                catch (Exception ex)
                {

                }
                m_notifyicon.Dispose();


                try
                {
                    Process[] prs = Process.GetProcesses();
                    foreach (Process pr in prs)
                    {
                        if (pr.ProcessName == "SFVideoPlayer")
                            pr.Kill();
                    }
                    IsExitApp = "Yes";
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    IsExitApp = "Yes";
                    Application.Exit();
                }

            }

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            if (e.KeyCode == Keys.Escape)
            {

                result = MessageBox.Show("Are you sure to exit ?", "Music Player", buttons);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                     
                    try
                    {
                        //Int32 CurrrentPos;
                        //if (musicPlayer1.URL != "")
                        //{
                        //    double t = Math.Floor(musicPlayer1.currentMedia.duration);
                        //    CurrrentPos = (Convert.ToInt32(t) - 30);
                        //    musicPlayer1.Ctlcontrols.currentPosition = CurrrentPos;
                        //}
                        //if (musicPlayer2.URL != "")
                        //{
                        //    double t = Math.Floor(musicPlayer2.currentMedia.duration);
                        //    CurrrentPos = (Convert.ToInt32(t) - 30);
                        //    musicPlayer2.Ctlcontrols.currentPosition = CurrrentPos;
                        //}
                        //return;
                        if (ObjMainClass.CheckForInternetConnection() == true)
                        {
                            UploadPlayerStatus();
                            #region Upload LogOut Status

                            string strZ = "insert into tbTokenLogOutStatus(TokenId,StatusDate,StatusTime) values(" + StaticClass.TokenId + " , ";
                            strZ = strZ + " '" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now.Date) + "','" + string.Format(fi, "{0:hh:mm tt}", DateTime.Now) + "') ";
                            if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                            StaticClass.constr.Open();
                            SqlCommand cmdLog = new SqlCommand();
                            cmdLog.Connection = StaticClass.constr;
                            cmdLog.CommandText = strZ;
                            cmdLog.ExecuteNonQuery();
                            StaticClass.constr.Close();

                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    m_notifyicon.Dispose();


                    try
                    {
                        Process[] prs = Process.GetProcesses();
                        foreach (Process pr in prs)
                        {
                            if (pr.ProcessName == "SFVideoPlayer")
                                pr.Kill();
                        }
                        IsExitApp = "Yes";
                        Application.Exit();
                    }
                    catch (Exception ex)
                    {
                        IsExitApp = "Yes";
                        Application.Exit();
                    }

                }
            }
        }




        DataTable dtPlayNow = new DataTable();
        DataTable dtDeleteTitle = new DataTable();
        private void PlayNowSQLDependency()
        {
            try
            {
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    return;
                }
                DataTable ds = new DataTable();
                string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                SqlCacheDependencyAdmin.EnableNotifications(CS);
                SqlCacheDependencyAdmin.EnableTableForNotifications(CS, "tbTokenPlayNow_Web");
                string sQr = "select * from tbTokenPlayNow_Web where tokenid= " + StaticClass.TokenId;
                ds = ObjMainClass.fnFillDataTable(sQr);
                CacheItemRemovedCallback onCacheItemRemoved = new CacheItemRemovedCallback(CacheItemRemovedCallbackMethod);
                SqlCacheDependency sqlDependency = new SqlCacheDependency("OnlineDB", "tbTokenPlayNow_Web");
                HttpRuntime.Cache.Insert("PlayNow_Web", ds, sqlDependency, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheItemRemoved);
            }
            catch (Exception ex)
            {

            }

        }
        public void CacheItemRemovedCallbackMethod(string key, object value, CacheItemRemovedReason reason)
        {
            try
            {
                string sQr = "";
                DataTable ds = new DataTable();
                if (ObjMainClass.CheckForInternetConnection() == false)
                {
                    return;
                }
                string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                SqlCacheDependencyAdmin.EnableNotifications(CS);
                SqlCacheDependencyAdmin.EnableTableForNotifications(CS, "tbTokenPlayNow_Web");
                sQr = "select * from tbTokenPlayNow_Web where tokenid= " + StaticClass.TokenId;
                ds = ObjMainClass.fnFillDataTable(sQr);
                CacheItemRemovedCallback onCacheItemRemoved = new CacheItemRemovedCallback(CacheItemRemovedCallbackMethod);
                SqlCacheDependency sqlDependency = new SqlCacheDependency("OnlineDB", "tbTokenPlayNow_Web");
                HttpRuntime.Cache.Insert("PlayNow_Web", ds, sqlDependency, DateTime.Now.AddDays(1), Cache.NoSlidingExpiration, CacheItemPriority.Default, onCacheItemRemoved);
            }
            catch (Exception ex)
            {
            }

        }
        private void PlayNowOption()
        {
            try
            {


                int RowIndex = 0;
                int schidWeb = 0;
                if (dtDeleteTitle.Rows.Count > 0)
                {
                    string iPr = "select * from tbBlockTitles";
                    DataTable dtLocalDelete = new DataTable();
                    dtLocalDelete = ObjMainClass.fnFillDataTable_Local(iPr);

                    for (int iDel = 0; iDel < dtDeleteTitle.Rows.Count; iDel++)
                    {
                        bool exists = dtLocalDelete.Select().ToList().Exists(row => row["titleid"].ToString() == dtDeleteTitle.Rows[iDel]["Titleid"].ToString());
                        if (exists == false)
                        {
                            if (StaticClass.LocalCon.State == ConnectionState.Closed) { StaticClass.LocalCon.Open(); }
                            OleDbCommand cmdLast = new OleDbCommand();
                            cmdLast.Connection = StaticClass.LocalCon;
                            cmdLast.CommandText = "insert into tbBlockTitles (titleid) values (" + dtDeleteTitle.Rows[iDel]["titleid"] + ")";
                            cmdLast.ExecuteNonQuery();
                        }
                    }
                }
                if (dtPlayNow.Rows.Count > 0)
                {
                    timResetSong.Enabled = false;


                    if (dtPlayNow.Rows[0]["PlayMode"].ToString() == "Song")
                    {
                        if (dtPlayNow.Rows[0]["PlayType"].ToString() == "Now")
                        {
                            PlaySongNow(dgPlaylist, dtPlayNow.Rows[0]["cID"].ToString());
                        }
                    }


                    timResetSong.Enabled = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        string RequestSongId = "";
        string RequestSongUrl = "";
        private void PlaySongNow(DataGridView dgGrid, string songIdWeb)
        {
            try
            {

                string TempSongName = "";
                timResetSong.Stop();
                btnPlay.BackgroundImage = ((System.Drawing.Image)(Properties.Resources.Pause_Blue));
                btnPlay.Text = "";
                string localfilename = "";
                string localSongId = "";

                if (songIdWeb != "")
                {
                    localSongId = songIdWeb;
                }
                else
                {
                    localSongId = dgGrid.Rows[dgGrid.CurrentCell.RowIndex].Cells["songid"].Value.ToString();
                }
                RequestSongId = localSongId;
                //localfilename = Application.StartupPath + "\\so\\" + localSongId + ".mp4";
                //TempSongName = Application.StartupPath + "\\so\\" + localSongId + ".mp4";
                localfilename = dgGrid.Rows[dgGrid.CurrentCell.RowIndex].Cells["localurl"].Value.ToString();
                TempSongName = localfilename;

                int LSongId = 0;
                if (musicPlayer2.URL != "")
                {
                    LSongId = MusicPlayer2CurrentSongId;
                }
                else if (musicPlayer1.URL != "")
                {
                    LSongId = MusicPlayer1CurrentSongId;
                }
                if (File.Exists(TempSongName))
                {
                    string str = "select filesize from tbSpecialPlaylists_Titles where titleId= " + RequestSongId;
                    DataTable dtRequest = new DataTable();
                    dtRequest = ObjMainClass.fnFillDataTable_Local(str);
                    long RequestFileSize = 0;
                    if (dtRequest.Rows.Count == 0)
                    {
                        str = "";
                        str = "select filesize from Titles where titleid= " + RequestSongId;
                        dtRequest = new DataTable();
                        dtRequest = ObjMainClass.fnFillDataTable(str);
                        if (dtRequest.Rows.Count > 0)
                        {
                            RequestFileSize = Convert.ToInt32(dtRequest.Rows[0]["filesize"]);
                        }
                        long DownloadedSize = new FileInfo(TempSongName).Length;
                        if (RequestFileSize != DownloadedSize)
                        {
                            File.Delete(TempSongName);
                        }
                    }
                }


                if (File.Exists(TempSongName))
                {
                    if (LSongId.ToString() != localSongId)
                    {
                        musicPlayer1.Dock = DockStyle.Fill;
                        musicPlayer1.BringToFront();

                        musicPlayer1.URL = localfilename;
                        MusicPlayer1CurrentSongId = Convert.ToInt32(localSongId);
                        if (btnMute.Text == "")
                        {if (StaticClass.IsVideoMute == "0")
                            {
                                musicPlayer1.settings.volume = 100;
                            }
                        else
                            {
                                musicPlayer1.settings.volume = 0;
                            }
                        }
                        musicPlayer2.URL = "";
                        musicPlayer2.Ctlcontrols.stop();
                    }
                    if (songIdWeb != "")
                    {
                        CurrentRow = CurrentRow + 1;
                    }
                    else
                    {
                        if (dgGrid.Name == "dgOtherPlaylist")
                        {
                            CurrentRow = CurrentRow + 1;
                        }
                    }

                    DisplaySongPlayerOne();
                    Song_Set_foucs();


                    if (songIdWeb == "")
                    {
                        if (dgGrid.Name == "dgPlaylist")
                        {
                            CurrentRow = dgGrid.CurrentCell.RowIndex;
                        }
                    }
                }
                else
                {
                    if (ObjMainClass.CheckForInternetConnection() == true)
                    {
                        if (wcDownloadRequestSong.IsBusy == false)
                        {
                            RequestSongUrl = "http://134.119.178.26/mp3files/" + localSongId + Path.GetExtension(localfilename);
                            wcDownloadRequestSong.DownloadFileAsync(new Uri(RequestSongUrl), localfilename);
                        }
                    }
                }
                timResetSong.Start();
            }
            catch (Exception ex)
            {
            }

        }


        private void bgPlayNow_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (ObjMainClass.CheckForInternetConnection() == true)
                {

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.Connection = StaticClass.constr;
                    cmd1.CommandText = "delete from tbTokenPlayNow_Web where tokenid= " + StaticClass.TokenId;
                    if (StaticClass.constr.State == ConnectionState.Open) StaticClass.constr.Close();
                    StaticClass.constr.Open(); cmd1.ExecuteNonQuery();
                    StaticClass.constr.Close();

                }

            }
            catch (Exception ex)
            {

                bgPlayNow.CancelAsync();
                bgPlayNow.Dispose();
            }

        }

        private void bgPlayNow_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GC.Collect();
            bgPlayNow.Dispose();

        }
        void wcDownloadRequestSong_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Maximum = (int)e.TotalBytesToReceive / 100;
            progressBar1.Value = (int)e.BytesReceived / 100;
        }
        private void wcDownloadRequestSong_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                string str = "select filesize from Titles where titleid= " + RequestSongId;
                DataTable dtRequest = new DataTable();
                dtRequest = ObjMainClass.fnFillDataTable(str);
                long RequestFileSize = 0;
                if (dtRequest.Rows.Count > 0)
                {
                    RequestFileSize = Convert.ToInt32(dtRequest.Rows[0]["filesize"]);
                }

                string TempSongName = Application.StartupPath + "\\so\\" + RequestSongId + Path.GetExtension(RequestSongUrl);
                long DownloadedSize = new FileInfo(TempSongName).Length;
                if (RequestFileSize == DownloadedSize)
                {
                    if (File.Exists(TempSongName))
                    {
                       musicPlayer1.Dock = DockStyle.Fill;
                        musicPlayer1.BringToFront();
                        musicPlayer1.URL = TempSongName;
                        MusicPlayer1CurrentSongId = Convert.ToInt32(RequestSongId);
                        if (btnMute.Text == "")
                        {
                            if (StaticClass.IsVideoMute == "0")
                            {
                                musicPlayer1.settings.volume = 100;
                            }
                            else
                            {
                                musicPlayer1.settings.volume = 0;
                            }
                        }
                        musicPlayer2.URL = "";
                        musicPlayer2.Ctlcontrols.stop();

                        if (RequestSongId != "")
                        {
                            CurrentRow = CurrentRow + 1;
                        }

                        DisplaySongPlayerOne();
                        Song_Set_foucs();
                    }
                }
                else
                {
                    File.Delete(TempSongName);
                    if (ObjMainClass.CheckForInternetConnection() == true)
                    {
                        if (wcDownloadRequestSong.IsBusy == false)
                        {
                            string RemoteFtpPath = "http://134.119.178.26/mp3files/" + RequestSongId + Path.GetExtension(RequestSongUrl);
                            wcDownloadRequestSong.DownloadFileAsync(new Uri(RemoteFtpPath), TempSongName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

        }


    }
}
