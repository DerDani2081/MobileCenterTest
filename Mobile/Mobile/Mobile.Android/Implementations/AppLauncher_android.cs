using Android.Content;
using Android.Content.PM;
using Android.Net;
using Mobile.Common.Utilities.Interfaces;
using Mobile.Droid.Implementations;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppLauncher_android))]
namespace Mobile.Droid.Implementations
{
	public class AppLauncher_android : IAppLauncher
	{
		public bool IsInstalled(string target)
		{
			target = target.ToLower() + "://";
			Uri uri = Uri.Parse(target);

			try
			{
				Forms.Context.PackageManager.GetPackageInfo(target, PackageInfoFlags.Activities);
			}
			catch (PackageManager.NameNotFoundException e)
			{
				return false;
			}

			return true;
		}

		public void CallApplication(string target, string callParameter)
		{
			try
			{
				target = target.ToLower() + "://" + callParameter;
				Uri uri = Uri.Parse(target);

				var TargetIntent = new Intent(Intent.ActionView, uri);

				Forms.Context.StartActivity(TargetIntent);
			}
			catch (ActivityNotFoundException e)
			{
				e.PrintStackTrace();
			}
		}

	}
}
