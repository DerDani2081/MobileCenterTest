using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mobile.Common.Controls.Model;
using Mobile.Common.Controls.Utils;
using Mobile.Common.Controls.View;
using Mobile.Common.Services.Interfaces;
using Mobile.Common.Utilities;
using Mobile.Common.Utilities.Interfaces;
using Mobile.Features.Messenger.View;
using Mobile.Features.Qr.View;
using Xamarin.Forms;

namespace Mobile.Common.Controls.ViewModel
{
	public class GtueMainViewModel : NotificationBase
	{
		private static GtueMainViewModel _instance;
		public static GtueMainViewModel Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GtueMainViewModel();
				}

				return _instance;
			}
		}

		private IAccountFileHandling _accountFileHandling;

		private GtueMainViewModel()
		{
			_accountFileHandling = DependencyService.Get<IAccountFileHandling>();

			if (_accountFileHandling == null)
			{
				throw new Exception("Missing implementation of " + nameof(IAccountFileHandling));
			}
		}

		public string LoginToken
		{
			get
			{
				return _accountFileHandling.LoadLoginToken();
			}
			set
			{
				_accountFileHandling.SaveLoginToken(value);
			}
		}

		public bool IsLoggedIn
		{
			get
			{
				return RestServiceManager.IsUserLoggedIn(LoginToken);
			}
		}

		private UserInfoViewModel _userInfo;

		public UserInfoViewModel UserInfo
		{
			get
			{
				return _userInfo;
			}
			set
			{
				SetProperty(ref _userInfo, value);
			}
		}

		private ObservableCollection<MainMenuItemViewModel> _menuItems;

		public ObservableCollection<MainMenuItemViewModel> MenuItems
		{
			get
			{
				if (_menuItems == null)
				{
					_menuItems = new ObservableCollection<MainMenuItemViewModel>
					{
                        new MainMenuSeparatorItemViewModel{  Id= 99, Title=string.Empty},
                        new MainMenuItemViewModel { Id = 0, Title = "App Home", CallCommandExecute=ResetViewToStart, CallCommandCanExecute = () => { return true; }},
						new MainMenuPluginItemViewModel { Id = 1, Title = "News", PageType=typeof(MessengerMainView), CallParameter="News" },
						new MainMenuPluginItemViewModel { Id = 2, Title = "Messenger",  PageType=typeof(MessengerMainView), CallParameter="Messenger" },
						new MainMenuPluginItemViewModel { Id = 3, Title = "QR-Code", PageType=typeof(QrMainView), Source="qrcode_small" },
						new MainMenuApplicationItemViewModel { Id = 4, Title = "GRIPS", Source="grips_small" },
						new MainMenuApplicationItemViewModel { Id = 5, Title = "Selbstservice", Source="selbst_small" },
						new MainMenuApplicationItemViewModel { Id = 6, Title = "Statistik", Source="Statistik" },

						//Seperator
						new MainMenuSeparatorItemViewModel(),

						new MainMenuItemViewModel { Id = 7, Title = "Informationen zur App" },
						new MainMenuItemViewModel { Id = 8, Title = "Hilfe" },
						new MainMenuItemViewModel { Id = 9, Title = "Abmelden", CallCommandExecute=LogoutCurrentUser, CallCommandCanExecute = () => { return true; }}
					};
				}

				return _menuItems;
			}
			set
			{
				SetProperty(ref _menuItems, value);
			}
		}

		public MainMenuItemViewModel SelectedMenuItem
		{
			get
			{
				return null;
			}
			set
			{
				if(value != null)
				{
					GtueMainView.Instance.CollapseMenu();
				}

				RaisePropertyChanged(nameof(SelectedMenuItem));
			}
		}

		private string _appTitle;

		public string AppTitle
		{
			get
			{
				return _appTitle;
			}
			set
			{
				SetProperty(ref _appTitle, value);
			}
		}

		public async void Initialize()
		{
			if (string.IsNullOrEmpty(LoginToken))
			{
				LoginToken = _accountFileHandling.LoadLoginToken();
				Debug.WriteLine("Token successful retrieved from storage.");
			}

			if (!IsLoggedIn)
			{
				Debug.WriteLine("Login required.");

				string userEmail = _accountFileHandling.LoadPreviousUserLogin();

				UserLoginViewModel userLogin = new UserLoginViewModel(new UserLogin { Email = userEmail });
				userLogin.SaveLoginData = !string.IsNullOrEmpty(userEmail);
				userLogin.IsLoggedIn += UserLogin_IsLogginFinished;

				//for Android only -> avoiding crash
				await Task.Delay(20);

				IsLoggedInChanged(userLogin, false);
				return;
			}

			UserLoggedIn(LoginToken);
		}

		void UserLogin_IsLogginFinished(object sender, string e)
		{
			UserLoginViewModel login = (sender as UserLoginViewModel);

			Debug.WriteLine("Finished login user: " + login.Email);

			if (login == null)
			{
				return;
			}

			string userEmail = login.Email;
			LoginToken = e;

			if (login.SaveLoginData == true)
			{
				_accountFileHandling.SavePreviousUserLogin(userEmail);
			}
			else
			{
				_accountFileHandling.SavePreviousUserLogin(null);
			}

			_accountFileHandling.SaveLoginToken(LoginToken);

			UserLoggedIn(LoginToken);
		}


		public void UserLoggedIn(string token)
		{
			LoginToken = token;
			UserInfo userInfo = RestServiceManager.GetUserInfo(token);

			if (userInfo == null)
			{
				Debug.WriteLine("Error: loading user information");
			}

			UserInfo = new UserInfoViewModel(userInfo);
			IsLoggedInChanged(this, true);
		}

		public void LogoutCurrentUser()
		{
			Debug.WriteLine("logging out current user");
			RestServiceManager.LogoutCurrentUser(LoginToken);
			_accountFileHandling.SaveLoginToken(null);
			LoginToken = null;

			UserInfo = null;

			Initialize();
		}

		public void ResetViewToStart()
		{
			GtueMainView.Instance.NavigationPage.PopToRootAsync();
		}

		public event EventHandler<bool> IsLoggedInChanged;
	}
}
