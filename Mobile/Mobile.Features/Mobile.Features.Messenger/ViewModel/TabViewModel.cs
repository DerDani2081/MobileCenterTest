using System;
using Xamarin.Forms;

namespace Mobile.Features.Messenger.ViewModel
{
	public class TabViewModel
	{
		public string HostName { get; set; }

		public string Title { get; set; }
		public string Icon { get; set; }
		public string Url { get; set; }

		public WebViewSource Source
		{
			get
			{
				//var source = new HtmlWebViewSource();
				//source.Html = "<html><body><h1>test</h1></body></html>";
				//return source;

				return new UrlWebViewSource { Url = Url };
			}
		}
	}
}
