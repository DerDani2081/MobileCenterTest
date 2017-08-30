using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mobile.ViewModel;
using Xamarin.Forms;

namespace Mobile.View
{
	public partial class StarterMainView : ContentPage
	{
		private StarterMainViewModel _starterMainViewModel;

		public StarterMainView()
		{
			InitializeComponent();

			_starterMainViewModel = new StarterMainViewModel();
			BindingContext = _starterMainViewModel;
		}

		async void Handle_Tapped(object sender, System.EventArgs e)
		{
			Frame view = sender as Frame;

			if (view == null)
			{ 
				return;
			}

			if (view.OutlineColor.Equals(Color.FromRgb(204, 0, 0)) == false)
			{
				return;
			}

			view.BackgroundColor = Color.FromRgb(204, 0, 0);

			await Task.Delay(200);

			view.BackgroundColor = Color.FromRgb(255,255,255);
		}
	}
}
