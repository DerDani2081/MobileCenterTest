using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mobile.Common.Utilities.Interfaces;
using Mobile.Features.Qr.ViewModel;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Mobile.Features.Qr.View
{
	public partial class QrMainView : ContentPage
	{
		private QrMainViewModel _qrMainViewModel;
		private INavigationHandler _navigationHandler;

		public QrMainView()
		{
			InitializeComponent();

			_qrMainViewModel = new QrMainViewModel();
			BindingContext = _qrMainViewModel;

			_navigationHandler = DependencyService.Get<INavigationHandler>();

			if (_navigationHandler == null)
			{
				throw new Exception("Missing implementation of " + nameof(INavigationHandler));
			}

			StartQrScanner();

			//_qrMainViewModel.PruefmittelGuid = "7c9e6679-7425-40de-944b-e07fc1f90ae7";
			//_navigationHandler.NavigationPage.PushAsync(this);
		}

		//private void OpenQrScanner_Clicked(object sender, EventArgs e)
		//{
		//	StartQrScanner();
		//}

		private void StartQrScanner()
		{
			ZXingScannerPage scanPage = new ZXingScannerPage()
			{
				Title = "QR-Code",
			};

			scanPage.ToolbarItems.Add(new ToolbarItem("Recent", null, HandleAction_Previous));

			scanPage.OnScanResult += (result) => 
			{
				// Stop scanning
				scanPage.IsScanning = false;

				// Pop the page and show the result
				Device.BeginInvokeOnMainThread(() =>
				{
					_qrMainViewModel.PruefmittelGuid = result.Text;
					_navigationHandler.NavigationPage.PushAsync(this);
				});
			};

			_navigationHandler.NavigationPage.PushAsync(scanPage);
		}

		async void Handle_Tapped(object sender, System.EventArgs e)
		{
            VisualElement view = sender as VisualElement; ;

			if (view == null)
				return;


            Color color = view.BackgroundColor;

            view.BackgroundColor = Color.DarkGray; // FromRgb(190f, 0, 41);

			await Task.Delay(200);

            view.BackgroundColor = color; // Color.FromRgb(248, 248, 248);
		}

		void HandleAction_Previous()
		{
			Dictionary<string, string> elements = new Dictionary<string, string>
			{
				{"a1", "test"},
				{"a2","id-jnfg321r532"},
				{"a3","id-jnfg321r532"},
				{"a4","id-jnfg321r532"},
			};

			//TODO implementation
		}
	}
}
