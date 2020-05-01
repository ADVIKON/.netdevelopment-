using System; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace SFVideoPlayer
{
    public static class StaticClass
    {
        public static string Is_Admin = "";
        public static string dfClientId = "";
        public static string TokenId = "";
        
        public static string LocalUserId = "";
        public static string isRemove = "";
        public static string isDownload = "";
        public static SqlConnection constr;
        public static OleDbConnection LocalCon= new OleDbConnection();
        public static string PlayerExpiryMessage = "";
        public static Boolean IsCopyright = false;
        public static string MainwindowMessage = "";
        public static Boolean IsStream = false;
        public static string StreamExpiryMessage = "";
        public static Int32 LeftStreamtDays = 0;


        public static string DealerCode = "";
        public static string CountryCode = "";
        public static Int32 AdvtTime = 0;
        public static Int32 DefaultPlaylistId = 0;
        public static Int32 DefaultPlaylistCurrentRow = 0;

        public static Int32 Last100PlaylistId = 0;

        public static Boolean IsAdvtManual=false;
        public static Boolean IsAdvtTime = false;
        public static Boolean IsAdvt = false;
        public static Int32 LeftAdvtDays = 0;
        public static Boolean IsBlockAdvt = false;

        public static Int32 TokenServiceId = 0;
        public static Int32 TokenUserId = 0;
        public static Int32 AdvtCityId = 0;

        public static Int32 Stateid = 0;
        public static Int32 CountryId = 0;


        public static Boolean IsStore = false;
        public static Int32 SchId = 0;
        public static string PlayerClosingTime="";
        public static string IsPlayerClose = "No";
        public static int TotalAdvtSongs = 0;
        public static Boolean IsAdvtWithSongs = false;

        public static Boolean IsAdvtBetweenTime = false;

        public static Boolean IsLock = false;
        public static Boolean IsVedioActive = false;
        public static string AdvtClosingTime = "";
        public static string ScheduleType = "";
        public static string IsVideoMute = "0";
    }
    //user name=IN- Paras Technologies
    //token no=FOHM-FRML-EFLD-EEGS-AYXD
}
