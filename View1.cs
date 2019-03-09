using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Java.Util.Zip;
using Android.Media;
using ExtensionsCarousel;
using System.Threading.Tasks;
using System.Globalization;
using mania4key_v2.Legacy;

namespace mania4key_v2
{
    //TODO : REMOVE LEGACY OSU PARSER, ADD REPLAY LIST
    public partial class App : Xamarin.Forms.Application
    {

        public App()
        {
            //InitializeComponent();
            MainPage = new ManPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
    public class MasterPageItem
    {
        public string Title { get; set; }


        public Type TargetType { get; set; }
    }
    public class ManPage : MasterDetailPage
    {
        MasterPageCS masterPage;

        public ManPage()
        {
            masterPage = new MasterPageCS();
            Master = masterPage;
            Detail = new NavigationPage(new MainPageCS());

            masterPage.ListView.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.ListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
    public class MasterPageCS : ContentPage
    {
        public ListView ListView { get { return listView; } }
        private bool _canClose = true;

        ListView listView;

        public MasterPageCS()
        {
            Title = "lyb!mania";
            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Play",
                TargetType = typeof(MainPageCS)
            });


            masterPageItems.Add(new MasterPageItem
            {
                Title = "Options",
                TargetType = typeof(OptionsPage)
            });

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Help",
                TargetType = typeof(HelpPage)
            });

            listView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.SetBinding(Label.TextProperty, "Title");

                    grid.Children.Add(label, 1, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };

            Content = new StackLayout
            {
                Children = { listView }
            };
        }
    }
    public class MainPageCS : CarouselPage
    {
        
        public MainPageCS()
        {
            
            Title = "lyb!mania 4k";
            var padding = new Thickness(0, Device.OnPlatform(40, 40, 0), 0, 0);
            
            ButtonDemoPage playpage = new ButtonDemoPage();
            BeatmapInfoPage bminfopage = new BeatmapInfoPage();

            Children.Add(playpage);
            Children.Add(bminfopage);
        }

        public void test()
        {

        }
       
    }

    class HelpPage : ContentPage
    {
        public List<string> names = new List<string>();
        public ListView list = new ListView();

        public static Label info = new Label
        {
            Text = "To play this game, you have to get osu!mania beatmaps from osu.ppy.sh or other beatmap download website, make sure that the key amount is four(4). There are 2 way to install beatmap.",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };
        public static Label info2 = new Label
        {
            Text = "You can download OSZ file and/or put it on /sdcard/download, then press the 'Refresh' button.",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };
        public static Label info3 = new Label
        {
            Text = "Or you can copy the beatmap folder directly from your pc to the Osu Path Directory which is set on the Options Page.",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };
        public static Label info4 = new Label
        {
            Text = "To put default soundset, copy osu hitsound files into /sdcard/lybmania4key/sounds",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };
        public static Label aff = new Label
        {
            Text = "lyb!mania is not affiliated with Osu Ppy Ltd and is a separate entity.",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };

        public HelpPage()
        {

            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);


            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    info,info2,info3,info4,aff
                }
            };


        }


    }

    class BeatmapInfoPage : ContentPage
    {
        public List<string> names = new List<string>();
        public static ListView lstview = new ListView
        {
            RowHeight = 60,
            ItemTemplate = new DataTemplate(() =>
            {

                Label a1 = new Label();
                a1.SetBinding(Label.TextProperty, "difficulty");

                Label a2 = new Label();
                a2.SetBinding(Label.TextProperty, "liststring");

                Label a3 = new Label();
                a3.SetBinding(Label.TextProperty, "liststring2");

                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Padding = new Thickness(0, 5),
                        Orientation = StackOrientation.Horizontal,
                        Children =
                                    { new StackLayout {
                                        VerticalOptions = LayoutOptions.Center,
                                        Spacing = 0,
                                        Children =
                                        {
                                            a1,a2,a3
                                        }
                                }
                                    }
                    }
                };
            })
        };
        public static string osu_path;
        public static int bmid;

        Button button = new Button
        {
            Text = "Start Game!",
            Font = Font.SystemFontOfSize(NamedSize.Large),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End
        };
        //button.Clicked += OnButtonClicked;
        

            

    public static Label header = new Label
        {
            Text = "Beatmap Info",
            Font = Font.BoldSystemFontOfSize(50),
            HorizontalOptions = LayoutOptions.Center
        };

        public static Label name = new Label
        {
            Text = "Beatmap Name",
            Font = Font.BoldSystemFontOfSize(25),
            HorizontalOptions = LayoutOptions.Center,
            VerticalTextAlignment = TextAlignment.Center
        };

        public static Label artist = new Label
        {
            Text = "Beatmap Artist",
            Font = Font.BoldSystemFontOfSize(20),
            HorizontalOptions = LayoutOptions.Center
        };

        public static Label creator = new Label
        {
            Text = "Beatmap Creator",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };

        public static Label ver = new Label
        {
            Text = "Beatmap Difficulty/Version",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };

        public static Label bpm = new Label
        {
            Text = "Beatmap BPM",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };

        public static Label id = new Label
        {
            Text = "Beatmap ID",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };

        public static Label score = new Label
        {
            Text = "",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };

        public static Label maxmm = new Label
        {
            Text = "",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };

        public static Label acc = new Label
        {
            Text = "",
            Font = Font.BoldSystemFontOfSize(15),
            HorizontalOptions = LayoutOptions.Start
        };



        public BeatmapInfoPage()
        {
            button.Clicked += delegate
            {
   
                    BMDifficulty diff = lstview.SelectedItem as BMDifficulty;

                if (diff != null)
                {
                    mania4key_v2.Game1.osu_path = diff.path;
                    if (lyb_rhythmdroid.Game1.mediaPlayer != null) lyb_rhythmdroid.Game1.mediaPlayer.Reset();

                    Forms.Context.StartActivity(typeof(testandroid1.Activity1));
                }
                 else
                {
                    DisplayAlert("Error", "Make sure you select the difficulty before starting the game!", "Ok");
                }
            };

            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    name, artist, creator, lstview,button
                }
            };


        }

        public static BMSong cursong;

        public static  void set_data2(BMSong song)
        {
            name.Text = song.SongName;
            artist.Text = song.artist;
            creator.Text = "Difficulties :";
            List<BMDifficulty> bmdiff = new List<BMDifficulty>();
            foreach (BMDifficulty diff in song.difficulty)
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string filename = "scores";
                string dbPath = Path.Combine(path, filename);
                Ini ini = new Ini(dbPath);

                int last_score = int.Parse(ini.GetValue(diff.bmid.ToString(), "score", "0"));
                int last_combo = int.Parse(ini.GetValue(diff.bmid.ToString(), "maxmm", "0"));
                int last_acc = (int)float.Parse(ini.GetValue(diff.bmid.ToString(), "acc", "0"), new CultureInfo("en-US"));

                BMDifficulty newdiff = new BMDifficulty(diff.path, diff.difficulty, diff.highscore, diff.acc, diff.bmid);
                newdiff.liststring = "Score : " + last_score;
                newdiff.liststring2 = "Combo : " + last_combo;
                bmdiff.Add(newdiff);

            }

            lstview.BeginRefresh();
            lstview.ItemsSource = bmdiff;
            lstview.EndRefresh();
            lstview.SelectedItem = bmdiff[0];
            cursong = song;
        }

        public static void set_data(string bmname, string bmartist, string bmver, string creators, int bmid, int bpms)
        {
            name.Text = bmname.Replace("TitleUnicode:", "");
            artist.Text = bmartist;
            ver.Text = "Difficulty : " + bmver;
            creator.Text = "Beatmap Creator : " + creators;
            id.Text = "Beatmap ID " + bmid.ToString();
            bpm.Text = bpms.ToString() + " BPM";

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string filename = "scores";
            string dbPath = Path.Combine(path, filename);
            Ini ini = new Ini(dbPath);

            int last_score = int.Parse(ini.GetValue(bmid.ToString(), "score", "-1"));
            int last_combo = int.Parse(ini.GetValue(bmid.ToString(), "maxmm", "-1"));
            int last_acc = (int)float.Parse(ini.GetValue(bmid.ToString(), "acc", "0"), new CultureInfo("en-US"));
            if (last_score != -1)
            {
                score.Text = "HighScore : " + last_score;
                maxmm.Text = "Max Combo : " + last_combo;
                acc.Text = "Accuracy : " + last_acc;
            }
            else
            {
                score.Text = "";
                maxmm.Text = "";
                acc.Text = "";
            }
        }
    }

    class OptionsPage : ContentPage
    {

        public List<string> names = new List<string>();
        public ListView list = new ListView();
        public static string osu_path;
        public static int bmid;

        public static void set_uri(string poyo)
        {
            osupath_uri.Text = poyo;
        }

        Label lb1 = new Label
        {
            Text = "On-Screen notes in miliseconds :",
            HorizontalOptions = LayoutOptions.Start
        };
        public static Entry speed = new Entry
        {
            Text = "600"
        };
        Label lb2 = new Label
        {
            Text = "osu! Beatmaps path :",
            HorizontalOptions = LayoutOptions.Start
        };
        public static Entry osupath_uri = new Entry
        {
            Text = "/sdcard/osu!droid/Songs/"
        };
        Button button = new Button
        {
            Text = "Extract and Remove OSZ in /sdcard/Download",
            Font = Font.SystemFontOfSize(NamedSize.Small),
            HorizontalOptions = LayoutOptions.Center
        };

        Button savebtn = new Button
        {
            Text = "Save Settings",
            Font = Font.SystemFontOfSize(NamedSize.Small),
            HorizontalOptions = LayoutOptions.Center
        };

        Button bmsound = new Button
        {
            Text = "Hitsound Enabled/(Disabled)",
            Font = Font.SystemFontOfSize(NamedSize.Small),
            HorizontalOptions = LayoutOptions.Center
        };

        public OptionsPage()
        {
            Title = "Options";

            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            string fname = "scores";
            string dbPath = Path.Combine(path, fname);
            Ini ini = new Ini(dbPath);
            osupath_uri.Text = ini.GetValue("settings", "osupath", "/sdcard/osu!droid/Songs/");
            speed.Text = ini.GetValue("settings", "speed", "600");

            bmsound.Clicked += delegate
            {

            };
            savebtn.Clicked += delegate
            {
                Ini inii = new Ini(dbPath);
                inii.WriteValue("settings", "osupath", osupath_uri.Text);
                inii.WriteValue("settings", "speed", speed.Text);
                inii.Save();
            };
            button.Clicked += delegate
            {
                try
                {
                    var fileList = new DirectoryInfo("/sdcard/Download/").GetFiles("*.osz", SearchOption.AllDirectories);
                    int counter = 0;
                    foreach (var files in fileList)
                    {
                        counter++;
                        string strSourcePath = files.FullName;
                        string filename = Path.GetFileNameWithoutExtension(files.FullName);
                        string strDestFolderPath = osupath_uri.Text + filename;
                        System.IO.Directory.CreateDirectory(strDestFolderPath);
                        using (ZipInputStream s = new ZipInputStream(File.OpenRead(strSourcePath)))
                        {
                            ZipEntry theEntry;
                            while ((theEntry = s.NextEntry) != null)
                            {
                                string directoryName = Path.GetDirectoryName(theEntry.Name);
                                string fileName = Path.GetFileName(theEntry.Name);
                                directoryName = Path.Combine(strDestFolderPath, directoryName);
                                if (directoryName.Length > 0)
                                {
                                    Directory.CreateDirectory(directoryName);
                                }
                                if (fileName != String.Empty)
                                {
                                    using (FileStream streamWriter = File.Create(Path.Combine(strDestFolderPath, theEntry.Name)))
                                    {
                                        int size = 2048;
                                        byte[] data = new byte[size];
                                        while (true)
                                        {
                                            size = s.Read(data, 0, data.Length);
                                            if (size > 0)
                                            {
                                                streamWriter.Write(data, 0, size);
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        System.IO.File.Delete(strSourcePath);
                    }
                    DisplayAlert("OSZ Extract", "In total theres " + counter + " Beatmaps that have been Imported. You will need to recheck for new beatmaps to see your newly extracted beatmaps.", "Ok");
                }
                catch (Exception e)
                {
                    DisplayAlert("OSZ Extract", "An error has occured : " + e, "Ok");
                }
            };
            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    lb2,osupath_uri,lb1,speed,savebtn,button
                }
            };
        }
    }

    class ButtonDemoPage : ContentPage
    {
        public string[] fullpath = new string[99999];
        public string[] dir = new string[99999];
        public string[] AudioFilename = new string[99999];
        public string[] tofile = new string[99999];
        public int[] AudioPreview = new int[99999];
        public List<string> name = new List<string>();
        public ListView list = new ListView
        {
            RowHeight = 60,
            ItemTemplate = new DataTemplate(() =>
            {

                Label a1 = new Label();
                a1.SetBinding(Label.TextProperty, "SongName");

                Label a2 = new Label();
                a2.SetBinding(Label.TextProperty, "artist");

                return new ViewCell
                {
                    View = new StackLayout
                    {
                        Padding = new Thickness(0, 5),
                        Orientation = StackOrientation.Horizontal,
                        Children =
                                    { new StackLayout {
                                        VerticalOptions = LayoutOptions.Center,
                                        Spacing = 0,
                                        Children =
                                        {
                                            a1,a2
                                        }
                                }
                                    }
                    }
                };
            })
        };
        public string lastpreview = "";
        public int index = 0;
        public int previewchan = 0;
        public float vol = 0;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(async () =>
            {
                string path = System.Environment.GetFolderPath(
    System.Environment.SpecialFolder.Personal);
                string filename = "beatmaps";
                string ininame = "scores";
                string dbPath = Path.Combine(path, filename);
                string dbPath2 = Path.Combine(path, ininame);
                Ini ini = new Ini(dbPath2);
                string iii = ini.GetValue("Settings", "FirstRun", "0");
                if (iii == "0")
                {

                    bool opsu = false;
                    bool osud = false;
                    if (Directory.Exists("/sdcard/opsu")) opsu = true;
                    if (Directory.Exists("/sdcard/osu!droid")) osud = true;
                    bool yes = false;
                    if (opsu)
                    {
                        var answer = await DisplayAlert("Beatmap Path", "Opsu detected, Would you like to use it as Beatmap Path?", "Yes", "No");
                        if (answer)
                        {
                            ini.WriteValue("Settings", "osupath", "/sdcard/opsu/Songs/");
                            OptionsPage.set_uri("/sdcard/opsu/Songs/");
                            yes = true;
                        }
                    }
                    if (osud && !yes)
                    {
                        var answer = await DisplayAlert("Beatmap Path", "Osu!droid detected, Would you like to use it as Beatmap Path?", "Yes", "No");
                        if (answer)
                        {
                            ini.WriteValue("Settings", "osupath", "/sdcard/osu!droid/Songs/");
                            OptionsPage.set_uri("/sdcard/osu!droid/Songs/");
                        }
                    }
                    if (!osud && !opsu)
                    {
                        await DisplayAlert("Beatmap Path", "We did not detect any beatmap folder on your device, beatmap path will default to /sdcard/osu!droid/Songs. For more information on how to import beatmaps, please read the help page.", "Ok");

                    }
                    await DisplayAlert("Beatmap Path", "For more information on how to import beatmaps, please read the help page.", "Ok");
                    ini.WriteValue("Settings", "FirstRun", "1");
                    ini.Save();
                }
            });
        }
        class bmlist
        {
            public bmlist(string name, string artist, string difficulty, string highscore)
            {
                this.name = name;
                this.artist = artist;
                this.difficulty = difficulty;
                this.highscore = highscore;
            }

            public string name { private set; get; }

            public string artist { private set; get; }
            public string difficulty { private set; get; }
            public string highscore { private set; get; }
        };

        Label label;
        public ButtonDemoPage()
        {

            Label header = new Label
            {
                Text = "lyb!mania 4k",
                Font = Font.BoldSystemFontOfSize(50),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            string path = System.Environment.GetFolderPath(
                   System.Environment.SpecialFolder.Personal);
            string filename = "beatmaps";
            string ininame = "scores";
            string dbPath = Path.Combine(path, filename);
            string dbPath2 = Path.Combine(path, ininame);
            List<BMSong> bmlists = new List<BMSong>();

            bool taskdone = true;
            int u = 0;
            try
            {

                using (StreamReader sr = new StreamReader(dbPath))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] lines = line.Split(new[] { ":::" }, StringSplitOptions.None);
                        name.Add(lines[0]);
                        fullpath[u] = lines[1];
                        dir[u] = lines[2];
                        AudioFilename[u] = lines[3];
                        AudioPreview[u] = int.Parse(lines[4]);

                        string title = lines[5]; string artist = lines[6]; int bmid = int.Parse(lines[9]);
                        string creator = lines[7]; string ver = lines[8];
                        Ini ini = new Ini(dbPath2);

                        //bmlists.Add(new bmlist(title, artist + " (" + creator + ")", ver, "High Score : " + ini.GetValue(bmid.ToString(), "score", "Not Yet Played")));


                        bool found = false;
                        for (int i = 0; i < bmlists.Count; i++)
                        {
                            if (bmlists[i].SongName == title)
                            {
                                found = true;
                                var difficulty_to_add = new BMDifficulty(lines[1],lines[8], ini.GetValue(bmid.ToString(), "score", "0"), ini.GetValue(bmid.ToString(), "acc", "0"),bmid);
                                bmlists[i].difficulty.Add(difficulty_to_add);
                                break;
                            }
                        }

                        if (!found)
                        {
                            var difficulty_to_add = new BMDifficulty(lines[1], lines[8], ini.GetValue(bmid.ToString(), "score", "0"), ini.GetValue(bmid.ToString(), "acc", "0"),bmid);
                            var list_to_add = new List<BMDifficulty>();
                            list_to_add.Add(difficulty_to_add);
                            bmlists.Add(new BMSong(title, artist, list_to_add, bmid, lines[3], int.Parse(lines[4]),lines[2]));
                        }

                        u++;
                    }
                }

            }
            catch
            {

            }
            list.ItemsSource = bmlists;
            Button button2 = new Button
            {
                Text = "Refresh",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };


            Button button = new Button
            {
                Text = "Start Game!",
                Font = Font.SystemFontOfSize(NamedSize.Large),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End
            };
            //button.Clicked += OnButtonClicked;

            list.ItemSelected += Lst_ItemSelected;

            button.Clicked += delegate
            {
                //ManagedBass.Bass.Free();
                mania4key_v2.Game1.osu_path = fullpath[index];
                if (lyb_rhythmdroid.Game1.mediaPlayer != null) lyb_rhythmdroid.Game1.mediaPlayer.Reset();

                Forms.Context.StartActivity(typeof(testandroid1.Activity1));
            };
            button2.Clicked += delegate
            {

                list.ItemsSource = null;
                list.SelectedItem = null;
                list.BeginRefresh();
                name.Clear();
                bmlists.Clear();

                if (!Directory.Exists((OptionsPage.osupath_uri.Text)))
                {
                    Directory.CreateDirectory(OptionsPage.osupath_uri.Text);
                }

                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            var fileList1 = new DirectoryInfo("/sdcard/Download/").GetFiles("*.osz", SearchOption.AllDirectories);
                            int counters = 0;
                            foreach (var files in fileList1)
                            {
                                counters++;
                                string strSourcePath = files.FullName;
                                string thefilename = Path.GetFileNameWithoutExtension(files.FullName);
                                string strDestFolderPath = OptionsPage.osupath_uri.Text + thefilename;
                                System.IO.Directory.CreateDirectory(strDestFolderPath);
                                using (ZipInputStream s = new ZipInputStream(File.OpenRead(strSourcePath)))
                                {
                                    ZipEntry theEntry;
                                    while ((theEntry = s.NextEntry) != null)
                                    {
                                        string directoryName = Path.GetDirectoryName(theEntry.Name);
                                        string fileName = Path.GetFileName(theEntry.Name);
                                        directoryName = Path.Combine(strDestFolderPath, directoryName);
                                        if (directoryName.Length > 0)
                                        {
                                            Directory.CreateDirectory(directoryName);
                                        }
                                        if (fileName != String.Empty)
                                        {
                                            using (FileStream streamWriter = File.Create(Path.Combine(strDestFolderPath, theEntry.Name)))
                                            {
                                                int size = 2048;
                                                byte[] data = new byte[size];
                                                while (true)
                                                {
                                                    size = s.Read(data, 0, data.Length);
                                                    if (size > 0)
                                                    {
                                                        streamWriter.Write(data, 0, size);
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                System.IO.File.Delete(strSourcePath);
                            }
                        }
                        catch
                        {

                        }

                        var fileList = new DirectoryInfo(OptionsPage.osupath_uri.Text).GetFiles("*.osu", SearchOption.AllDirectories);
                        int counter = 0; int _audiostuff = 0;
                        foreach (var files in fileList)
                        {
                            using (StreamReader sr = new StreamReader(files.FullName))
                            {

                                try
                                {
                                    int a = 0;
                                    string line; string title = ""; string artist = ""; int bmid = 0;
                                    string creator = ""; string ver = "";

                                    while ((line = sr.ReadLine()) != "[HitObjects]")
                                    {
                                        if (line.Contains("AudioFilename") == true)
                                            AudioFilename[_audiostuff] = line.Replace("AudioFilename: ", "");
                                        if (line.Contains("PreviewTime") == true)
                                            AudioPreview[_audiostuff] = int.Parse(line.Replace("PreviewTime: ", ""));
                                        if (line.Contains("Title:") == true)
                                            title = line.Replace("Title:", "");

                                        if (line.Contains("Artist:") == true)
                                            artist = line.Replace("Artist:", "");

                                        if (line.Contains("Creator:") == true)
                                            creator = line.Replace("Creator:", "");

                                        if (line.Contains("Version:") == true)
                                            ver = line.Replace("Version:", "");
                                        if (line.Contains("BeatmapID:") == true)
                                        {
                                            string temp = line.Replace("BeatmapID:", "");
                                            bmid = int.Parse(temp);
                                        }
                                    }
                                    bool check = false;
                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        string[] lines = line.Split(',');
                                        int i = int.Parse(lines[0]);
                                        if (i != 0)
                                        {
                                            if ((i == 64) || (i == 192) || (i == 320) || (i == 448)) check = true;
                                            else check = false;
                                        }
                                    }
                                    if (check == true)
                                    {

                                        fullpath[counter] = files.FullName;
                                        dir[counter] = files.DirectoryName + "/";
                                        string ii = files.ToString();
                                        ii = ii.Replace(dir[counter], "");
                                        ii = ii.Replace(".osu", "");
                                        name.Add(ii);
                                        Ini ini = new Ini(dbPath2);

                                        //bmlists.Add(new bmlist(title, artist + "(" + creator + ")", ver, "High Score : " + ini.GetValue(bmid.ToString(), "score", "Not Yet Played")));

                                        bool found = false;
                                        for (int i = 0; i < bmlists.Count; i++)
                                        {
                                            if (bmlists[i].SongName == title)
                                            {
                                                found = true;
                                                var difficulty_to_add = new BMDifficulty(files.FullName, ver, ini.GetValue(bmid.ToString(), "score", "0"), ini.GetValue(bmid.ToString(), "acc", "0"), bmid);
                                                bmlists[i].difficulty.Add(difficulty_to_add);
                                                break;
                                            }
                                        }

                                        if (!found)
                                        {
                                            var difficulty_to_add = new BMDifficulty(files.FullName, ver, ini.GetValue(bmid.ToString(), "score", "0"), ini.GetValue(bmid.ToString(), "acc", "0"), bmid);
                                            var list_to_add = new List<BMDifficulty>();
                                            list_to_add.Add(difficulty_to_add);
                                            bmlists.Add(new BMSong(title, artist, list_to_add, bmid, AudioFilename[_audiostuff], (AudioPreview[_audiostuff]), dir[counter]));
                                        }

                                        tofile[counter] = ii + ":::" + fullpath[counter] + ":::" + dir[counter] + ":::" + AudioFilename[_audiostuff] + ":::" + AudioPreview[_audiostuff] + ":::" + title + ":::" + artist + ":::" + creator + ":::" + ver + ":::" + bmid.ToString();

                                        counter++; _audiostuff++;
                                    }


                                }
                                catch
                                {

                                }
                            }
                        }

                        path = System.Environment.GetFolderPath(
                       System.Environment.SpecialFolder.Personal);
                        filename = "beatmaps";
                        dbPath = Path.Combine(path, filename);
                        u = 0;
                        using (StreamWriter sr = new StreamWriter(dbPath))
                        {
                            foreach (string s in name)
                            {

                                sr.WriteLine(tofile[u]);
                                u++;
                            }
                        }
                    }).ContinueWith(task =>
                    {
                        //list.ItemsSource = name;
                        list.ItemsSource = bmlists;
                        list.EndRefresh();

                        if (bmlists.Count == 0)
                        {
                            DisplayAlert("Warning", "No Beatmaps Detected, Please read the help page on how to import beatmap.", "Ok");
                        }

                        path = System.Environment.GetFolderPath(
                       System.Environment.SpecialFolder.Personal);
                        filename = "scores";
                        dbPath = Path.Combine(path, filename);
                        

                    }, TaskScheduler.FromCurrentSynchronizationContext());
                
                
            };
            // Accomodate iPhone status bar.
            this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            // Build the page.
            this.Content = new StackLayout
            {
                Children =
                {
                    list,button2
                }
            };

        }


        private void Lst_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            

            try
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                string filename = "scores";
                string dbPath = Path.Combine(path, filename);
                
                string line = ""; string title = ""; string artist = ""; int bmid = 0;
                string creator = ""; string ver = ""; int bpm = 0;

                index = (list.ItemsSource as List<BMSong>).IndexOf(e.SelectedItem as BMSong); // to get a index value or postion value 
                using (StreamReader sr = new StreamReader(fullpath[index]))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains("Title") == true)
                            title = line.Replace("Title:", "");

                        if (line.Contains("Artist:") == true)
                            artist = line.Replace("Artist:", "");

                        if (line.Contains("Creator:") == true)
                            creator = line.Replace("Creator:", "");

                        if (line.Contains("Version:") == true)
                            ver = line.Replace("Version:", "");

                        if (line.Contains("BeatmapID:") == true)
                        {
                            string temp = line.Replace("BeatmapID:", "");
                            bmid = int.Parse(temp);
                        }

                        if (line.Contains("[TimingPoints]") == true)
                        {
                            string asdf = sr.ReadLine();
                            string[] asdfg = asdf.Split(',');



                            bpm = System.Convert.ToInt32(Math.Round((double)(60000 / Decimal.Parse(asdfg[1], new CultureInfo("en-US")))));
                        }

                    };

                }
                //BeatmapInfoPage.set_data(title, artist, ver, creator, bmid, bpm);
                var parent = Parent as MainPageCS;
                    parent.PageRight();
                {
                    //ManagedBass.Bass.Free();
                    //ManagedBass.Bass.Init(-1, 44100, ManagedBass.DeviceInitFlags.Default, System.IntPtr.Zero);
                    //ManagedBass.Bass.StreamFree(previewchan);
                    ManagedBass.Bass.StreamFree(mania4key_v2.Game1.i);
                    ManagedBass.Bass.Free();
                    var song = e.SelectedItem as BMSong;
                    BeatmapInfoPage.set_data2(song);
                    if (lyb_rhythmdroid.Game1.mediaPlayer != null) lyb_rhythmdroid.Game1.mediaPlayer.Reset();
                    lyb_rhythmdroid.Game1.mediaPlayer = MediaPlayer.Create(Android.App.Application.Context, Android.Net.Uri.Parse(song.dir + song.audiopath));
                    lyb_rhythmdroid.Game1.mediaPlayer.SetVolume(0, 0);
                    lyb_rhythmdroid.Game1.mediaPlayer.SeekTo(song.audiopreview);
                    lyb_rhythmdroid.Game1.mediaPlayer.Start();
                    //previewchan = ManagedBass.Bass.CreateStream(dir[index] + AudioFilename[index]);
                    //ManagedBass.Bass.ChannelSetAttribute(previewchan, ManagedBass.ChannelAttribute.Volume, 0);
                    vol = 0;
                    //ManagedBass.Bass.ChannelPlay(previewchan);
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        while (vol < 1)
                        {
                            vol = vol + 0.01f;
                            lyb_rhythmdroid.Game1.mediaPlayer.SetVolume(vol, vol);
                        //ManagedBass.Bass.ChannelSetAttribute(previewchan, ManagedBass.ChannelAttribute.Volume, vol);
                        System.Threading.Thread.Sleep(5);
                        }
                    });
                    //ManagedBass.Bass.ChannelSetPosition(previewchan, ManagedBass.Bass.ChannelSeconds2Bytes(previewchan, AudioPreview[index] / 1000), ManagedBass.PositionFlags.Bytes);
                    //lastpreview = AudioFilename[index];
                }
            }
            catch (Exception ex)
            {
                Ini ini = new Ini("/sdcard/lybmania4key/lyb_errlog.txt");
                ini.WriteValue("loadbm", ex.Message);
                ini.Save();
            }
            //((ListView)sender).SelectedItem = null;
            

        }

    }
}