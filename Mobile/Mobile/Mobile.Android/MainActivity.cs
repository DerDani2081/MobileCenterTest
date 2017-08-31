using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace Mobile.Droid
{
	[Activity (Label = "GTÜ World", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		public TaskCompletionSource<byte[]> PickImageTaskCompletionSource { set; get; }

		protected override void OnCreate (Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar; 

			base.OnCreate (bundle);

			global::Xamarin.Forms.Forms.Init (this, bundle);
			LoadApplication (new Mobile.App ());

			MobileCenter.Start("eaf0dde8-6120-4139-b7eb-0b6ed79892ac",
				   typeof(Analytics), typeof(Crashes));

			ZXing.Net.Mobile.Forms.Android.Platform.Init();
		}
	}
}

