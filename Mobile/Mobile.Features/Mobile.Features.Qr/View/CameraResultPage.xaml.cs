using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Mobile.Features.Qr.View
{
	public partial class CameraResultPage : ContentPage
	{
		public CameraResultPage()
		{
			InitializeComponent();
		}

		public bool CancelsTouchesInView
		{
			get
			{
				return true;
			}
		}

		void Handle_Tapped(object sender, System.EventArgs e)
		{
			this.Navigation.PopAsync();
		}
	}
}
