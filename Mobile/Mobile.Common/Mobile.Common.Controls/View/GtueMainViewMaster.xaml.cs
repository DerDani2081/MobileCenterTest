using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Mobile.Common.Controls.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mobile.Common.Controls.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GtueMainViewMaster : ContentPage
	{
		public ListView ListView;

		public GtueMainViewMaster()
		{
			InitializeComponent();
		}

		async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return;
			}

			MainMenuItemViewModel item = e.SelectedItem as MainMenuItemViewModel;

			if (item == null)
			{
				return;
			}

			ICommand command = item.CallCommand;

			if (command != null && command.CanExecute(null))
			{
				command.Execute(e.SelectedItem);
			}

			await Task.Delay(200);

			(sender as ListView).SelectedItem = null;
		}
	}
}
