using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using mania4key_v2;
using Plugin.CurrentActivity;

namespace testandroid1
{
    [Activity(Label = "lyb!mania"
        , MainLauncher = false
        , Icon = "@drawable/icon"
        , Theme = "@android:style/Theme.Material"
        , AlwaysRetainTaskState = true
        , ScreenOrientation = ScreenOrientation.FullUser
        , NoHistory = true
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout)]
    public class Activity1 : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            ManagedBass.Bass.StreamFree(mania4key_v2.Game1.i);
            ManagedBass.Bass.Free();
            var g = new Game1();
            mania4key_v2.Game1.exit = 0;
            SetContentView((View)g.Services.GetService(typeof(View)));
            g.Run();
            System.Threading.Tasks.Task.Run(() =>
            {
                while (mania4key_v2.Game1.exit <= 2)
                {
                    System.Threading.Thread.Sleep(100);
                }
                {   
                    this.Finish(); }
            });
        }
    }
}

