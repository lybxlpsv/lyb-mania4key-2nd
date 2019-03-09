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
    public class BMDifficulty
    {
            public BMDifficulty(string path, string difficulty, string highscore, string acc, int bmid)
            {
                this.path = path;
                this.difficulty = difficulty;
                this.highscore = highscore;
                this.acc = acc;
                this.bmid = bmid;
            }
            public string path { private set; get; }
            public string difficulty { private set; get; }
            public string highscore { private set; get; }
            public int bmid { private set; get; }
            public string acc { private set; get; }
            public string liststring { set; get; }
            public string liststring2 { set; get; }
    }
}