using System;
using System.Collections.Generic;
using Mobile.Common.Utilities.Interfaces;
using Mobile.Features.Messenger.ViewModel;
using Xamarin.Forms;

namespace Mobile.Features.Messenger.View
{
	public partial class MessengerMainView : TabbedPage
	{
		private MessengerMainViewModel _messengerMainViewModel;

		private INavigationHandler _navigationHandler;

		public MessengerMainView()
		{
			InitializeComponent();

			_messengerMainViewModel = new MessengerMainViewModel();

			BindingContext = _messengerMainViewModel;

			_navigationHandler = DependencyService.Get<INavigationHandler>();

			if (_navigationHandler == null)
			{
				throw new Exception("Missing implementation of " + nameof(INavigationHandler));
			}

			_navigationHandler.NavigationPage.PushAsync(this);
		}

		public MessengerMainView(string url) : this()
		{
			CalledByLink(url);
		}

		private void CalledByLink(string url)
		{
			url = url.ToLower();

			foreach(TabViewModel tab in _messengerMainViewModel.Tabs)
			{
				if(tab.HostName != null && tab.HostName.ToLower().Equals(url))
				{
					_messengerMainViewModel.SelectedItem = tab;
					return;
				}
			}
		}
	}
}
