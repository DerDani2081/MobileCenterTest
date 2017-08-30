using System;
using Mobile.Droid.Implementations;
using Mobile.Features.Messenger.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_android))]
namespace Mobile.Droid.Implementations
{
	public class BaseUrl_android : IBaseUrl
	{
		public string Get()
		{
			return "file:///android_asset";
		}
	}
}
