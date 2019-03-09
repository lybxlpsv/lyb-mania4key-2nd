using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace mania4key_v2
{
    public class BMSong
    {
        public BMSong(string SongName, string artist, List<BMDifficulty> difficulty, int bmid, string audiopath, int audiopreview, string dir)
        {
            this.SongName = SongName;
            this.artist = artist;
            this.difficulty = difficulty;
            this.highscore = highscore;
            this.audiopreview = audiopreview;
            this.audiopath = audiopath;
            this.dir = dir;
        }

        public string SongName { set; get; }
        public string artist { set; get; }
        public List<BMDifficulty> difficulty { set; get; }
        public string highscore { set; get; }
        public int bmid { get; set; }
        public string audiopath { get; set; }
        public string dir { get; set; }
        public int audiopreview { get; set; }

    }
}