using System.Diagnostics;
using Mobile.Common.Controls.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Common.Controls.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GtueMainView : MasterDetailPage
	{
		private static GtueMainView _instance;

		public static GtueMainView Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GtueMainView();
				}

				return _instance;
			}
		}

		private GtueMainView()
		{
			InitializeComponent();

			BindingContext = GtueMainViewModel.Instance;

			GtueMainViewModel.Instance.IsLoggedInChanged += Instance_IsLoggedInChanged;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			GtueMainViewModel.Instance.Initialize();
		}

		public NavigationPage NavigationPage
		{
			get
			{
				return DetailContent;
			}
		}

		private Page _mainContent;

		public Page MainContent
		{
			get
			{
				return _mainContent;
			}
			set
			{
				if (value != null)
				{
					_mainContent = value;
				}

				//setting to current page
				DetailContent.PushAsync(_mainContent);
			}
		}

		public void Instance_IsLoggedInChanged(object sender, bool e)
		{
			if (e)
			{
				Application.Current.MainPage = this;
				MainContent = null;
			}
			else
			{
				Debug.WriteLine("false");

				Application.Current.MainPage = new LoginPage { BindingContext = sender };
			}
		}

		public void CollapseMenu()
		{
			IsPresented = false;
		}
	}
}
