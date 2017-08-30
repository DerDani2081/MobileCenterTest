using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mobile.Common.Controls.View;
using Mobile.View;
using Xamarin.Forms;

namespace Mobile
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			GtueMainView.Instance.MainContent = new StarterMainView();

			MainPage = GtueMainView.Instance;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
