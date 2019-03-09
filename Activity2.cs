using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V4.App;
using System;
using System.IO;

namespace mania4key_v2
{
    [Activity(Label = "lyb!mania2nd"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance)]


    public class Activity1 : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity

    {


        readonly string[] perms =
    {
      Manifest.Permission.ReadExternalStorage,
      Manifest.Permission.WriteExternalStorage
    };
        const int RequestLocationId = 0;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            if (Build.VERSION.SdkInt >= Build.VERSION_CODES.M)
            {
                ActivityCompat.RequestPermissions(this, new String[] { Manifest.Permission.WriteExternalStorage }, 0);
            }

        }


    }

}

