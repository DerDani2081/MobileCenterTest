using System;
using Mobile.Controls;
using Mobile.Droid.Implementations;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ApplicationListView), typeof(ApplicationListViewRenderer_android))]
namespace Mobile.Droid.Implementations
{
	public class ApplicationListViewRenderer_android : ListViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
				return;

			Android.Widget.ListView listView = Control as Android.Widget.ListView;

			if (listView == null)
			{
				return;
			}

			//avoiding selecting items
			listView.Enabled = false;
		}
	}
}
