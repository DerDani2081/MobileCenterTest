using System;
using System.Collections.Generic;
using Mobile.Features.Qr.ViewModel;
using Xamarin.Forms;

namespace Mobile.Features.Qr.View
{
	public partial class EditCommentView : ContentPage
	{
		public EditCommentView()
		{
			InitializeComponent();

			Appearing += EditCommentView_Appearing;
			Disappearing += EditComment_Disappearing;
		}

		void EditCommentView_Appearing(object sender, EventArgs e)
		{
			editor.Focus();
		}

		private void EditComment_Disappearing(object sender, EventArgs e)
		{
			PruefmittelItemViewModel pruefmittel = (BindingContext as PruefmittelItemViewModel);

			if (pruefmittel != null && pruefmittel.IsModified)
			{
				pruefmittel.CancelChanges();
			}
		}
	}
}
