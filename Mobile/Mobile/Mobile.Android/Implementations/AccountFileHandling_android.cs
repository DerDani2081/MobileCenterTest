using System;
using Mobile.Common.Services.Interfaces;
using Mobile.Common.Utilities.Interfaces;
using Mobile.Droid.Implementations;
using Xamarin.Forms;
using Android.Content;

[assembly: Dependency(typeof(AccountFileHandling_android))]
namespace Mobile.Droid.Implementations
{
	public class AccountFileHandling_android : IAccountFileHandling
	{
		#region Properties
    ISharedPreferences prefs = Forms.Context.GetSharedPreferences("Gtue_App_Common_Auth_Store", FileCreationMode.Private);
    #endregion

    #region Methods
    public string LoadLoginToken()
    {
      return prefs.GetString("auth_token", null);
    }


    public void SaveLoginToken(string token)
    {
      ISharedPreferencesEditor editor = prefs.Edit();
      if (string.IsNullOrEmpty(token))
      {
				editor.Remove("auth_token").Apply();
      }
      else
      {
        editor.PutString("auth_token", token).Apply();
      }
    }


    public string LoadPreviousUserLogin()
    {
      return prefs.GetString("email", null);
    }


    public void SavePreviousUserLogin(string userLogin)
    {
      ISharedPreferencesEditor editor = prefs.Edit();
      if (string.IsNullOrEmpty(userLogin) == false)
      {
        editor.PutString("email", userLogin).Apply();
      }
      else
      {
        editor.Remove("email");
      }
    }

    #endregion
	}
}
