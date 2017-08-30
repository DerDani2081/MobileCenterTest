using System;
using Foundation;
using Mobile.Common.Services.Interfaces;
using Mobile.Common.Utilities.Interfaces;
using Mobile.iOS.Implementations;
using Security;
using Xamarin.Forms;

[assembly: Dependency(typeof(AccountFileHandling_ios))]
namespace Mobile.iOS.Implementations
{
	public class AccountFileHandling_ios : IAccountFileHandling
	{
		#region Properties
		NSUserDefaults plist = new NSUserDefaults("de.App", NSUserDefaultsType.SuiteName);
		#endregion

		#region Methods
		public string LoadLoginToken()
		{
			var rec = new SecRecord(SecKind.GenericPassword)
			{
				Generic = NSData.FromString("test")
			};

			SecStatusCode res;
			var match = SecKeyChain.QueryAsRecord(rec, out res);
			if (res == SecStatusCode.Success)
			{
				return match.ValueData.ToString();
			}

			return null;
		}

		public void SaveLoginToken(string token)
		{
			SecStatusCode error;

			if (string.IsNullOrEmpty(token))
			{
				SecRecord secureRecord = new SecRecord(SecKind.GenericPassword)
				{
					Generic = NSData.FromString("test")
				};
				error = SecKeyChain.Remove(secureRecord);
			}
			else
			{
				SecRecord secureRecord = new SecRecord(SecKind.GenericPassword)
				{
					ValueData = NSData.FromString(token),
					Generic = NSData.FromString("test")
				};
				error = SecKeyChain.Add(secureRecord);
			}

			if (error != SecStatusCode.Success && error != SecStatusCode.DuplicateItem)
			{
				Console.WriteLine(error);
			}
		}

		public string LoadPreviousUserLogin()
		{
			return plist.StringForKey("email");
		}

		public void SavePreviousUserLogin(string userLogin)
		{
			if (string.IsNullOrEmpty(userLogin) == false)
			{
				plist.SetString(userLogin, "email");
			}
			else
			{
				plist.RemoveObject("email");
			}
		}
		#endregion
	}
}
