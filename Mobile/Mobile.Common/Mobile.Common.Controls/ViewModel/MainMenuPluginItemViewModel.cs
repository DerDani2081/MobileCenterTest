using System;
using Mobile.Common.Controls.View;
using Mobile.Common.Utilities.Interfaces;
using Xamarin.Forms;

namespace Mobile.Common.Controls.ViewModel
{
	public class MainMenuPluginItemViewModel : MainMenuItemViewModel
	{
		public MainMenuPluginItemViewModel()
		{
			CallCommandExecute = CallApplication;
			CallCommandCanExecute = () => { return true; };
		}

		public String CallParameter { get; set; }

		public Type PageType { get; set; }

		private void CallApplication()
		{
			if(GtueMainView.Instance.NavigationPage.Navigation.NavigationStack.Count > 1)
			{
				GtueMainView.Instance.NavigationPage.PopToRootAsync();
			}

			Page p;

			if(string.IsNullOrEmpty(CallParameter))
			{
				p = Activator.CreateInstance(PageType) as Page;
			}
			else
			{
				p = Activator.CreateInstance(PageType, CallParameter) as Page;
			}
		}
	}
}
