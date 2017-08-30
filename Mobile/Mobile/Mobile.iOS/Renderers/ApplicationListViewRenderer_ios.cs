using Mobile.Controls;
using Mobile.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ApplicationListView), typeof(ApplicationListViewRenderer_ios))]
namespace Mobile.iOS.Renderers
{
	public class ApplicationListViewRenderer_ios : ListViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
			{
				return;
			}

			var tableView = Control as UITableView;
			tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			tableView.AllowsSelection = false;

			Control.TableFooterView = new UIView();
		}
	}
}
