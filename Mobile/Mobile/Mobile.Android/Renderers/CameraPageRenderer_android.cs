using System;
using Android.Views;
using Android.App;
using Android.Hardware;
using Android.Graphics;
using Android.Widget;
using Mobile.Droid.Renderers;
using Mobile.Features.Qr.View;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System.IO;

[assembly: ExportRenderer(typeof(CameraPage), typeof(CameraPageRenderer_android))]
namespace Mobile.Droid.Renderers
{
	public class CameraPageRenderer_android : PageRenderer, TextureView.ISurfaceTextureListener
	{
		CameraPage cameraPage;

		global::Android.Hardware.Camera camera;
		global::Android.Widget.Button takePhotoButton;
		global::Android.Widget.Button toggleFlashButton;
		global::Android.Widget.Button switchCameraButton;
		global::Android.Views.View view;

		Activity activity;
		CameraFacing cameraType;
		TextureView textureView;
		SurfaceTexture surfaceTexture;

		bool flashOn;
		byte[] imageBytes;

		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null || Element == null)
			{
				return;
			}

			try
			{
				SetupUserInterface();
				SetupEventHandlers();
				AddView(view);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
			}

			cameraPage = e.NewElement as CameraPage;
		}

		void SetupUserInterface()
		{
			activity = this.Context as Activity;
			view = activity.LayoutInflater.Inflate(Resource.Layout.CameraLayout, this, false);
			cameraType = CameraFacing.Back;

			textureView = view.FindViewById<TextureView>(Resource.Id.textureView);
			textureView.SurfaceTextureListener = this;
		}

		void SetupEventHandlers()
		{
			takePhotoButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.takePhotoButton);
			takePhotoButton.Click += TakePhotoButtonTapped;

			toggleFlashButton = view.FindViewById<global::Android.Widget.Button>(Resource.Id.toggleFlashButton);
			toggleFlashButton.Click += ToggleFlashButtonTapped;
		}

		protected override void OnLayout(bool changed, int l, int t, int r, int b)
		{
			base.OnLayout(changed, l, t, r, b);

			var msw = MeasureSpec.MakeMeasureSpec(r - l, MeasureSpecMode.Exactly);
			var msh = MeasureSpec.MakeMeasureSpec(b - t, MeasureSpecMode.Exactly);

			view.Measure(msw, msh);
			view.Layout(0, 0, r - l, b - t);
		}

		public void OnSurfaceTextureUpdated(SurfaceTexture surface)
		{

		}

		public void OnSurfaceTextureAvailable(SurfaceTexture surface, int width, int height)
		{
			camera = global::Android.Hardware.Camera.Open((int)cameraType);
			textureView.LayoutParameters = new FrameLayout.LayoutParams(width, height);
			surfaceTexture = surface;

			camera.SetPreviewTexture(surface);
			PrepareAndStartCamera();
		}

		public bool OnSurfaceTextureDestroyed(SurfaceTexture surface)
		{
			camera.StopPreview();
			camera.Release();
			return true;
		}

		public void OnSurfaceTextureSizeChanged(SurfaceTexture surface, int width, int height)
		{
			PrepareAndStartCamera();
		}

		void PrepareAndStartCamera()
		{
			camera.StopPreview();

			var display = activity.WindowManager.DefaultDisplay;
			if (display.Rotation == SurfaceOrientation.Rotation0)
			{
				camera.SetDisplayOrientation(90);
			}

			if (display.Rotation == SurfaceOrientation.Rotation270)
			{
				camera.SetDisplayOrientation(180);
			}

			camera.StartPreview();
		}

		void ToggleFlashButtonTapped(object sender, EventArgs e)
		{
			var flashMode = camera.GetParameters().FlashMode;

			switch (flashMode)
			{
				case global::Android.Hardware.Camera.Parameters.FlashModeAuto: //Auto Flash -> On
					toggleFlashButton.SetBackgroundResource(Resource.Drawable.flashOn);
					cameraType = CameraFacing.Back;

					camera.StopPreview();
					camera.Release();
					camera = global::Android.Hardware.Camera.Open((int)cameraType);
					var parametersOn = camera.GetParameters();
					parametersOn.FlashMode = global::Android.Hardware.Camera.Parameters.FlashModeTorch;
					camera.SetParameters(parametersOn);
					camera.SetPreviewTexture(surfaceTexture);
					PrepareAndStartCamera();
					break;
				case global::Android.Hardware.Camera.Parameters.FlashModeTorch: //Auto On -> Off:
					toggleFlashButton.SetBackgroundResource(Resource.Drawable.flashOff);

					camera.StopPreview();
					camera.Release();
					camera = global::Android.Hardware.Camera.Open((int)cameraType);
					var parametersOff = camera.GetParameters();
					parametersOff.FlashMode = global::Android.Hardware.Camera.Parameters.FlashModeOff;
					camera.SetParameters(parametersOff);
					camera.SetPreviewTexture(surfaceTexture);
					PrepareAndStartCamera();
					break;
				case global::Android.Hardware.Camera.Parameters.FlashModeOff: //Auto Off -> Auto:
					toggleFlashButton.SetBackgroundResource(Resource.Drawable.flashAuto);

					camera.StopPreview();
					camera.Release();
					camera = global::Android.Hardware.Camera.Open((int)cameraType);
					var parametersAuto = camera.GetParameters();
					parametersAuto.FlashMode = global::Android.Hardware.Camera.Parameters.FlashModeAuto;
					camera.SetParameters(parametersAuto);
					camera.SetPreviewTexture(surfaceTexture);
					PrepareAndStartCamera();
					break;
			}

			//if (flashOn)
			//{



			//	if (cameraType == CameraFacing.Back)
			//	{
			//		toggleFlashButton.SetBackgroundResource(Resource.Drawable.FlashButton);
			//		cameraType = CameraFacing.Back;

			//		camera.StopPreview();
			//		camera.Release();
			//		camera = global::Android.Hardware.Camera.Open((int)cameraType);
			//		var parameters = camera.GetParameters();
			//		parameters.FlashMode = global::Android.Hardware.Camera.Parameters.FlashModeTorch;
			//		camera.SetParameters(parameters);
			//		camera.SetPreviewTexture(surfaceTexture);
			//		PrepareAndStartCamera();
			//	}
			//}
			//else
			//{
			//	toggleFlashButton.SetBackgroundResource(Resource.Drawable.NoFlashButton);
			//	camera.StopPreview();
			//	camera.Release();

			//	camera = global::Android.Hardware.Camera.Open((int)cameraType);
			//	var parameters = camera.GetParameters();
			//	parameters.FlashMode = global::Android.Hardware.Camera.Parameters.FlashModeOff;
			//	camera.SetParameters(parameters);
			//	camera.SetPreviewTexture(surfaceTexture);
			//	PrepareAndStartCamera();
			//}
		}

		async void TakePhotoButtonTapped(object sender, EventArgs e)
		{
			camera.StopPreview();

			var image = textureView.Bitmap;

			try
			{
				MemoryStream stream = new MemoryStream();
				await image.CompressAsync(Bitmap.CompressFormat.Jpeg, 50, stream);
				Byte[] data = stream.ToArray();
				stream.Close();
				image.Recycle();

				cameraPage.ViewModel.ShowPreviewPage(data);

				//var absolutePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim).AbsolutePath;
				//var folderPath = absolutePath + "/Camera";
				//var filePath = System.IO.Path.Combine(folderPath, string.Format("photo_{0}.jpg", Guid.NewGuid()));

				//var fileStream = new FileStream(filePath, FileMode.Create);
				//await image.CompressAsync(Bitmap.CompressFormat.Jpeg, 50, fileStream);
				//fileStream.Close();
				//image.Recycle();

				//var intent = new Android.Content.Intent(Android.Content.Intent.ActionMediaScannerScanFile);
				//var file = new Java.IO.File(filePath);
				//var uri = Android.Net.Uri.FromFile(file);
				//intent.SetData(uri);
				//Forms.Context.SendBroadcast(intent);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"				", ex.Message);
			}

			camera.StartPreview();
		}
	}
}
