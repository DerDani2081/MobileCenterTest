using System;
using Mobile.Common.Utilities.Interfaces;
using Xamarin.Forms;

namespace Mobile.Common.Controls.ViewModel
{
	public class MainMenuApplicationItemViewModel : MainMenuItemViewModel
	{
		IAppLauncher _appLauncher;

		public MainMenuApplicationItemViewModel()
		{
			_appLauncher = DependencyService.Get<IAppLauncher>();

			if(_appLauncher == null)
			{
				throw new Exception("Missing implementation of " + nameof(IAppLauncher));
			}

			CallCommandExecute = CallApplication;
			CallCommandCanExecute = () => { return IsInstalled; };
		}

		public String BundleId { get; set; }

		public String CallParameter { get; set; }

		public bool IsInstalled
		{
			get
			{
				if (string.IsNullOrEmpty(BundleId))
				{
					return false;
				}

				return _appLauncher.IsInstalled(BundleId);
			}
		}

		private void CallApplication()
		{
			_appLauncher.CallApplication(BundleId, CallParameter);
		}
	}
}
