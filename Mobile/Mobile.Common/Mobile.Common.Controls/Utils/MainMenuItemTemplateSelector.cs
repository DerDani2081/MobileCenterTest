using System;
using Mobile.Common.Controls.ViewModel;
using Xamarin.Forms;

namespace Mobile.Common.Controls.Utils
{
	class MainMenuItemTemplateSelector : DataTemplateSelector
	{
		public DataTemplate SeparatorTemplate { get; set; }
		public DataTemplate ItemTemplate { get; set; }

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item as MainMenuSeparatorItemViewModel != null)
			{
				return SeparatorTemplate;
			}

			return ItemTemplate;
		}
	}
}
