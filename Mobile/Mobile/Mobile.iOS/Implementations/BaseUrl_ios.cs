using System;
using Foundation;
using Mobile.Features.Messenger.Interfaces;
using Mobile.iOS.Implementations;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_ios))]
namespace Mobile.iOS.Implementations
{
	public class BaseUrl_ios : IBaseUrl
	{
		public string Get()
		{
			return NSBundle.MainBundle.BundlePath;
		}
	}
}
