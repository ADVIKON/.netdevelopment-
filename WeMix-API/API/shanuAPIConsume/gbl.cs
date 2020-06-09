using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeMixApi
{
   public class gbl
    {
        public class Playlist
        {
            public List<PlaylistInfo> data { get; set; } // Filename of the file 
            public string[] notice { get; set; } // Path of the file on the server 
        }
        public class PlaylistInfo
        {
            public string id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public string published { get; set; }

        }
        public class PlaylistContent
        {
            public PlaylistIdInfo data { get; set; } // Filename of the file 
            public string[] notice { get; set; } // Path of the file on the server 
        }
        public class PlaylistIdInfo
        {
            public string playlistid { get; set; }
            public List<PlaylistContentInfo> content { get; set; }
            

        }
        public class PlaylistContentInfo
        {
            public string Artist { get; set; }
            public string Title { get; set; }
            public string Duration { get; set; }
            public string BPM { get; set; }

            public string Genre { get; set; }
            public string Year { get; set; }
            public string Country { get; set; }
            public string Explicit { get; set; }

            public string MediaType { get; set; }
            public string AssetID { get; set; }
            public string FileType { get; set; }
            public string downloadLink { get; set; }
        }
    }
}
