using System;
using System.IO;
using System.Reflection;
using Mobile.Common.Services.DataTransfairObject;
using Mobile.Common.Services.Implamentations;
using Mobile.Common.Services.Interfaces;
using Xamarin.Forms;

#if !PRODUCTION
[assembly: Dependency(typeof(StorageAccountService))]
#endif
namespace Mobile.Common.Services.Implamentations
{
	public class StorageAccountService : IAccountService
	{
		public DtoUserInfo GetUserInfo(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return null;
			}

			return new DtoUserInfo
			{
				FirstName = "Manuel",
				LastName = "Köbler",
				PruefingenieurId = 000132,
				Image = ReadFromResource("Mobile.Common.Services.Images.testPI.png")
			};
		}

		public bool IsLoggedIn(string token)
		{
			if (string.IsNullOrEmpty(token))
			{
				return false;
			}

			return true;
		}

		public string LoginUser(DtoUserLogin user)
		{
			string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
			return token;
		}

		public void LogoutUser(string token)
		{

		}

		public byte[] ReadFromResource(string resourceFile)
		{
			Assembly assembly = typeof(StorageAccountService).GetTypeInfo().Assembly;
			Stream resourceStream = assembly.GetManifestResourceStream(resourceFile);

			if(resourceStream == null)
			{
				return null;
			}

			using (var memoryStream = new MemoryStream())
			{
				resourceStream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}
	}
}
