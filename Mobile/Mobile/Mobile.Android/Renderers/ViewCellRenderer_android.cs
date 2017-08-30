using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Mobile.Droid.Renderers
{
	public class ViewCellRenderer_android : ViewCellRenderer
	{
		private Android.Views.View _cellCore;
		private Drawable _unselectedBackground;
		private bool _selected;

		protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
		{
			_cellCore = base.GetCellCore(item, convertView, parent, context);
			_unselectedBackground = _cellCore.Background;

			return _cellCore;
		}

		protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnCellPropertyChanged(sender, e);

			if (e.PropertyName == "IsSelected")
			{
				_selected = !_selected;

				if (_selected)
				{
					ViewCell viewCell = sender as ViewCell;
					_cellCore.SetBackgroundColor(Android.Graphics.Color.Argb(100, 204, 0, 0));
				}
				else
				{
					_cellCore.SetBackgroundColor(Android.Graphics.Color.Argb(100, 242, 242, 242));
				}
			}
		}
	}
}
