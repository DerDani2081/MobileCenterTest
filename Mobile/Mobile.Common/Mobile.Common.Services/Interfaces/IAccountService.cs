using System;
using Mobile.Common.Services.DataTransfairObject;

namespace Mobile.Common.Services.Interfaces
{
	public interface IAccountService
	{
		string LoginUser(DtoUserLogin user);
		void LogoutUser(string token);

		bool IsLoggedIn(string token);

		DtoUserInfo GetUserInfo(string token);
	}
}
