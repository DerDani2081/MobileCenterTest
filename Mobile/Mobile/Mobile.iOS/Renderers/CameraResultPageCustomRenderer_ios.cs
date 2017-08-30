using System;
using Foundation;
using Mobile.iOS.Renderers;
using Mobile.Features.Qr.View;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CameraResultPage), typeof(CameraResultPageCustomRenderer_ios))]
namespace Mobile.iOS.Renderers
{
	public class CameraResultPageCustomRenderer_ios : PageRenderer
	{
		private NSObject _observerHideKeyboard;
		private NSObject _observerShowKeyboard;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			CameraResultPage cp = Element as CameraResultPage;

			if (cp != null && !cp.CancelsTouchesInView)
			{
				foreach (var g in View.GestureRecognizers)
				{
					g.CancelsTouchesInView = false;
				}
			}
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			_observerHideKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			_observerShowKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			NSNotificationCenter.DefaultCenter.RemoveObserver(_observerHideKeyboard);
			NSNotificationCenter.DefaultCenter.RemoveObserver(_observerShowKeyboard);
		}

		void OnKeyboardNotification(NSNotification notification)
		{
			if (!IsViewLoaded) return;

			var frameBegin = UIKeyboard.FrameBeginFromNotification(notification);
			var frameEnd = UIKeyboard.FrameEndFromNotification(notification);

			var page = Element as ContentPage;
			if (page != null && !(page.Content is ScrollView))
			{
				var padding = page.Padding;
				page.Padding = new Thickness(padding.Left, padding.Top, padding.Right, padding.Bottom + frameBegin.Top - frameEnd.Top);
			}
		}
	}
}
