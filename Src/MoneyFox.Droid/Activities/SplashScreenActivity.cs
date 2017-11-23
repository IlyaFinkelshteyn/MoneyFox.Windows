using System;
using System.IO;
using Android.App;
using Android.Content.PM;
using MoneyFox.Foundation.Constants;
using MvvmCross.Droid.Views;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "Money Fox",
        MainLauncher = true,
        Icon = "@drawable/ic_launcher",
        Theme = "@style/AppTheme.Splash",
        NoHistory = true,
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashScreenActivity : MvxSplashScreenActivity
    {
        public SplashScreenActivity() : base(Resource.Layout.activity_splash_screen)
        {
            try
            {
                DataAccess.ApplicationContext.DbPath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),
                        DatabaseConstants.DB_NAME);
            } catch (Exception)
            {
            }
        }
    }
}