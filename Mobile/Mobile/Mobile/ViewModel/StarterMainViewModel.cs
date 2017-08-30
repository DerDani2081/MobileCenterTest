using System;
using System.Collections.ObjectModel;
using Mobile.Common.Controls.ViewModel;
using Mobile.Common.Utilities;
using Xamarin.Forms;

namespace Mobile.ViewModel
{
	public class StarterMainViewModel : NotificationBase
	{
		public StarterMainViewModel()
		{
			GtueMainViewModel.Instance.AppTitle = "App Home";
		}

		public ObservableCollection<DoubleApplicationItem> DoubleApplicationItems
		{
			get
			{
				ObservableCollection<DoubleApplicationItem> doubleApplicationItems = new ObservableCollection<DoubleApplicationItem>();

				DoubleApplicationItem currentItem = null;

				foreach (MainMenuItemViewModel menuItem in GtueMainViewModel.Instance.MenuItems)
				{
					Type t = menuItem.GetType(); //.GetGenericTypeDefinition();

					if (t == typeof(MainMenuApplicationItemViewModel) || (t == typeof(MainMenuPluginItemViewModel)))
					{
						if (currentItem == null)
						{
							currentItem = new DoubleApplicationItem { Left = menuItem };
						}
						else
						{
							currentItem.Right = menuItem;
							doubleApplicationItems.Add(currentItem);
							currentItem = null;
						}
					}
				}

				if (currentItem != null)
				{
					doubleApplicationItems.Add(currentItem);
				}

				return doubleApplicationItems;
			}
		}
	}
}
