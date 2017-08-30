using System;
using System.Collections.Generic;
using Mobile.Features.Qr.Converter;
using Mobile.Features.Qr.ViewModel;
using Xamarin.Forms;

namespace Mobile.Features.Qr.View
{
	public partial class EditDateView : ContentPage
	{
		public EditDateView()
		{
			InitializeComponent();

			Disappearing += EditDateView_Disappearing;
		}

		public string PropertyName
		{
			set
			{
				monthPicker.ItemsSource = Months;
				monthPicker.SetBinding(Picker.SelectedIndexProperty, value + "Month", BindingMode.TwoWay, new MonthToIndexConverter());

				yearPicker.ItemsSource = Years;
				yearPicker.SetBinding(Picker.SelectedItemProperty, value + "Year", BindingMode.TwoWay);
			}
		}

		List<string> Months
		{
			get
			{
				string[] dtList = new string[12];

				for (int i = 0; i < 12; i++)
				{
					DateTime dt = new DateTime(2000, i + 1, 23);
					dtList[i] = dt.ToString("MMMM");
				}

				return new List<string>(dtList);
			}
		}

		List<int> Years
		{
			get
			{
				List<int> list = new List<int>();
				int currentYear = DateTime.Now.Year;

				for (int i = currentYear - 50; i < currentYear + 50; i++)
				{
					list.Add(i);
				}

				return list;
			}
		}

		void EditDateView_Disappearing(object sender, EventArgs e)
		{
			PruefmittelItemViewModel pruefmittel = (BindingContext as PruefmittelItemViewModel);

			if (pruefmittel != null && pruefmittel.IsModified)
			{
				pruefmittel.CancelChanges();
			}
		}

		void Handle_MinusMonat_Clicked(object sender, System.EventArgs e)
		{
			if (monthPicker.SelectedIndex == 0)
			{
				monthPicker.SelectedIndex = monthPicker.Items.Count - 1;
				return;
			}

			monthPicker.SelectedIndex--;
		}

		void Handle_PlusMonat_Clicked(object sender, System.EventArgs e)
		{
			if (monthPicker.SelectedIndex > monthPicker.Items.Count - 2)
			{
				monthPicker.SelectedIndex = 0;
				return;
			}

			monthPicker.SelectedIndex++;
		}

		void Handle_MinusYear_Clicked(object sender, System.EventArgs e)
		{
			if (yearPicker.SelectedIndex == 0)
			{
				yearPicker.SelectedIndex = yearPicker.Items.Count - 1;
				return;
			}

			yearPicker.SelectedIndex--;
		}

		void Handle_PlusYear_Clicked(object sender, System.EventArgs e)
		{
			if (yearPicker.SelectedIndex > yearPicker.Items.Count - 2)
			{
				yearPicker.SelectedIndex = 0;
				return;
			}

			yearPicker.SelectedIndex++;
		}
	}
}
