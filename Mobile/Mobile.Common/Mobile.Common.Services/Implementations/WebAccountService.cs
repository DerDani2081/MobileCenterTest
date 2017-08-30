using System;
using Mobile.Common.Services.DataTransfairObject;
using Mobile.Common.Services.Interfaces;

#if PRODUCTION
[assembly: Dependency(typeof(WebAccountService))]
#endif
namespace Mobile.Common.Services.Implamentations
{
	public class WebAccountService : IAccountService
	{
		public DtoUserInfo GetUserInfo()
		{
			throw new NotImplementedException();
		}

		public DtoUserInfo GetUserInfo(string token)
		{
			throw new NotImplementedException();
		}

		public bool IsLoggedIn(string token)
		{
			throw new NotImplementedException();
		}

		public string LoginUser(DtoUserLogin user)
		{
			throw new NotImplementedException();
		}

		public void LogoutUser(string token)
		{
			throw new NotImplementedException();
		}
	}
}
