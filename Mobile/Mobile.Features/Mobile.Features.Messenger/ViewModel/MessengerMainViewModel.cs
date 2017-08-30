using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Mobile.Common.Utilities;
using Mobile.Features.Messenger.Interfaces;
using Xamarin.Forms;

namespace Mobile.Features.Messenger.ViewModel
{
	public class MessengerMainViewModel : NotificationBase
	{
		public string Language => "de";

		private ObservableCollection<TabViewModel> _tabs;

		public ObservableCollection<TabViewModel> Tabs
		{
			get
			{
				if (_tabs == null)
				{
					_tabs = new ObservableCollection<TabViewModel>
					{
						new TabViewModel { Title="News", Icon="news.png", Url=GetSourceString("news"), HostName="News" },
						new TabViewModel { Title="Nachrichten", Icon="messenger.png", Url=GetSourceString("chat"), HostName="Messenger"  },
						new TabViewModel { Title="Einstellungen", Icon="settings.png", Url=GetSourceString("settings") }
					};
				}

				return _tabs;
			}
		}

		private TabViewModel _selectedItem;

		public TabViewModel SelectedItem
		{
			get
			{
				if (_selectedItem == null)
				{
					return Tabs[0];
				}

				return _selectedItem;
			}
			set
			{
				SetProperty(ref _selectedItem, value);
			}
		}

		private IBaseUrl _baseUrl;

		public MessengerMainViewModel()
		{
			_baseUrl = DependencyService.Get<IBaseUrl>();
		}

		public string GetSourceString(string subpage, Dictionary<string, string> parameters = null)
		{
			string url = _baseUrl.Get();

			url += "/MessengerWebApi";
			url += "/index.html#/" + Language + "/" + subpage;

			if (parameters != null && parameters.Count > 0)
			{
				url += "?";

				foreach (KeyValuePair<string, string> pair in parameters)
				{
					url += pair.Key + "=" + pair.Value + "&";
				}

				url.Remove(url.LastIndexOf('&'));
			}

			return url;
		}
	}
}
