using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace mania4key_v2.IO
{
    class Path
    {
        public static string datapath = "/sdcard/lybmania4key/";
        public static string audiopath = datapath + "audio/";
        public static string sp = "/";
        public static void CreateFolderIfMissing()
        {
            if (!Directory.Exists("/sdcard/lybmania4key"))
            {
                Directory.CreateDirectory("/sdcard/lybmania4key");
                Directory.CreateDirectory("/sdcard/lybmania4key/sounds");
            }
        }
    }
}