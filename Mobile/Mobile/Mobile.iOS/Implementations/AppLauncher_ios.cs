using System;
using System.Diagnostics;
using Foundation;
using Mobile.Common.Utilities.Interfaces;
using Mobile.iOS.Implementations;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(AppLauncher_ios))]
namespace Mobile.iOS.Implementations
{
	public class AppLauncher_ios : IAppLauncher
	{
		public void CallApplication(string target, string callParameter)
		{
			target = target.Replace(".", "").ToLower() + "://" + callParameter;

			Debug.WriteLine("link: " + target);
			NSUrl url = new NSUrl(target);

			bool canOpen = UIApplication.SharedApplication.CanOpenUrl(url);
			Debug.WriteLine("link can open: " + canOpen);

			UIApplication.SharedApplication.OpenUrl(url);
		}

		public bool IsInstalled(string target)
		{
			target = target.Replace(".", "").ToLower() + "://";
			NSUrl url = new NSUrl(target);

			try
			{
				return UIApplication.SharedApplication.CanOpenUrl(url);
			}
			catch (Exception e)
			{
				return false;
			}
		}
	}
}
