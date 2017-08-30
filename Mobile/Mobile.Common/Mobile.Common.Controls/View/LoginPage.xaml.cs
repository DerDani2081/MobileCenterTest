using System;
using System.Collections.Generic;
using Mobile.Common.Controls.ViewModel;
using Xamarin.Forms;

namespace Mobile.Common.Controls.View
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			UserLoginViewModel userLogin = BindingContext as UserLoginViewModel;

			if (userLogin == null)
			{
				return;
			}

			if (string.IsNullOrEmpty(userLogin.Email))
			{
				this.FindByName<Entry>("emailEntry").Focus();
				return;
			}

			this.FindByName<Entry>("passwordEntry").Focus();
		}
	}
}
