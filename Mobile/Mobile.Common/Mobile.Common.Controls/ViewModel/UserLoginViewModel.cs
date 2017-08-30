using System;
using System.Diagnostics;
using System.Windows.Input;
using Mobile.Common.Controls.Model;
using Mobile.Common.Controls.Utils;
using Mobile.Common.Utilities;
using Xamarin.Forms;

namespace Mobile.Common.Controls.ViewModel
{
	public class UserLoginViewModel : NotificationBase<UserLogin>
	{
		public UserLoginViewModel(UserLogin userLogin) : base(userLogin)
		{
		}

		public ImageSource GtueImageSource
		{
			get
			{
				return ImageSource.FromResource(string.Format("Mobile.Common.Controls.Icons.GtueLogo.png"));
			}
		}

		public string Email
		{
			get
			{
				return This.Email;
			}
			set
			{
				if (SetProperty(This.Email, value, () => This.Email = value))
				{
					RaisePropertyChanged(nameof(LoginUser));
				}
			}
		}

		public string Password
		{
			get
			{
				return This.Password;
			}
			set
			{
				if (SetProperty(This.Password, value, () => This.Password = value))
				{
					RaisePropertyChanged(nameof(LoginUser));
				}
			}
		}

		public bool SaveLoginData { get; set; }

		public ICommand LoginUser
		{
			get
			{
				return new Command(ExecuteLoginUser, CanLoginUser);
			}
		}

		private void ExecuteLoginUser()
		{
			string token = RestServiceManager.LoginUser(This);
			Debug.WriteLine("test");

			IsLoggedIn(this, token);

			//TODO show main page
			//DependencyService.Get<INavigationHelper>().SetMainPage();
		}

		public event EventHandler<string> IsLoggedIn;

		private bool CanLoginUser()
		{
			if (string.IsNullOrEmpty(This.Email) || string.IsNullOrEmpty(This.Password))
			{
				return false;
			}

			return true;
		}
	}
}
