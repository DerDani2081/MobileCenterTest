using System;
namespace Mobile.Common.Utilities.Interfaces
{
	public interface IAccountFileHandling
	{
		string LoadLoginToken();
		void SaveLoginToken(string token);

		string LoadPreviousUserLogin();
		void SavePreviousUserLogin(string userLogin);
	}
}
