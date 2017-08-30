using System;
using Mobile.Common.Controls.Implementations;
using Mobile.Common.Controls.View;
using Mobile.Common.Controls.ViewModel;
using Mobile.Common.Utilities.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(NavigationHandler))]
namespace Mobile.Common.Controls.Implementations
{
	public class NavigationHandler : INavigationHandler
	{
		public string Title { set => GtueMainViewModel.Instance.AppTitle = value; }

		public NavigationPage NavigationPage => GtueMainView.Instance.NavigationPage;
	}
}
