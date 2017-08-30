using System;
using System.Collections.Generic;
using Mobile.Features.Qr.ViewModel;
using Xamarin.Forms;

namespace Mobile.Features.Qr.View
{
	public partial class CameraPage : ContentPage
	{
		public PruefmittelItemViewModel ViewModel
		{
			get
			{
				return BindingContext as PruefmittelItemViewModel;
			}
		}

		public CameraPage()
		{
			InitializeComponent();
		}
	}
}
