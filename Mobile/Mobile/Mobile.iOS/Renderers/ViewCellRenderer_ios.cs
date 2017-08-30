using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Mobile.iOS.Renderers;

[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellRenderer_ios))]
namespace Mobile.iOS.Renderers
{
	public class ViewCellRenderer_ios : ViewCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);
			var view = item as ViewCell;

			cell.SelectedBackgroundView = new UIView
			{
				BackgroundColor = UIColor.FromRGB(204, 0, 0)
			};

			return cell;
		}
	}
}
