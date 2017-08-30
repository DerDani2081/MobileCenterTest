using System;
using AVFoundation;
using CoreGraphics;
using Foundation;
using Mobile.Features.Qr.View;
using Mobile.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CameraPage), typeof(CameraPageRenderer_ios))]
namespace Mobile.iOS.Renderers
{
	public class CameraPageRenderer_ios : PageRenderer
	{
		CameraPage cameraPage;

		AVCaptureSession captureSession;
		AVCaptureDeviceInput captureDeviceInput;
		AVCaptureStillImageOutput stillImageOutput;
		UIView liveCameraStream;
		UIButton takePhotoButton;
		UIButton toggleFlashButton;

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
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
				SetupLiveCameraStream();
				AuthorizeCameraUse();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(@"			ERROR: ", ex.Message);
			}

			if (e.NewElement == null)
			{
				return;
			}

			cameraPage = e.NewElement as CameraPage;
		}

		void SetupUserInterface()
		{
			var centerButtonX = View.Bounds.GetMidX() - 35f;
			var topLeftX = View.Bounds.X + 25;
			var topRightX = View.Bounds.Right - 65;
			var bottomButtonY = View.Bounds.Bottom - 150;
			var topButtonY = View.Bounds.Top + 15;
			var buttonWidth = 70;
			var buttonHeight = 70;

			liveCameraStream = new UIView()
			{
				Frame = new CGRect(0f, 0f, View.Bounds.Width, View.Bounds.Height)
			};

			takePhotoButton = new UIButton()
			{
				Frame = new CGRect(centerButtonX, bottomButtonY, buttonWidth, buttonHeight)
			};
			takePhotoButton.SetBackgroundImage(UIImage.FromFile("takePhotoButton.png"), UIControlState.Normal);

			//toggleCameraButton = new UIButton()
			//{
			//	Frame = new CGRect(topRightX, topButtonY + 5, 35, 26)
			//};
			//toggleCameraButton.SetBackgroundImage(UIImage.FromFile("ToggleCameraButton.png"), UIControlState.Normal);

			toggleFlashButton = new UIButton()
			{
				Frame = new CGRect(topLeftX, topButtonY, 37, 37)
			};
			toggleFlashButton.SetBackgroundImage(UIImage.FromFile("flashOff.png"), UIControlState.Normal);

			View.Add(liveCameraStream);
			View.Add(takePhotoButton);
			//View.Add(toggleCameraButton);
			View.Add(toggleFlashButton);
		}

		void SetupEventHandlers()
		{
			takePhotoButton.TouchUpInside += (object sender, EventArgs e) => {
				CapturePhoto();
			};

			//toggleCameraButton.TouchUpInside += (object sender, EventArgs e) => {
			//	ToggleFrontBackCamera();
			//};

			toggleFlashButton.TouchUpInside += (object sender, EventArgs e) => {
				ToggleFlash();
			};
		}

		async void CapturePhoto()
		{
			var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);
			var jpegImage = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);

			UIImage photo = new UIImage(jpegImage);

			Byte[] data = jpegImage.ToArray();

			cameraPage.ViewModel.ShowPreviewPage(data);
		}

		void ToggleFrontBackCamera()
		{
			var devicePosition = captureDeviceInput.Device.Position;
			if (devicePosition == AVCaptureDevicePosition.Front)
			{
				devicePosition = AVCaptureDevicePosition.Back;
			}
			else
			{
				devicePosition = AVCaptureDevicePosition.Front;
			}

			var device = GetCameraForOrientation(devicePosition);
			ConfigureCameraForDevice(device);

			captureSession.BeginConfiguration();
			captureSession.RemoveInput(captureDeviceInput);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
			captureSession.AddInput(captureDeviceInput);
			captureSession.CommitConfiguration();
		}

		void ToggleFlash()
		{
			var device = captureDeviceInput.Device;

			var error = new NSError();
			if (device.HasFlash)
			{
				device.LockForConfiguration(out error);

				switch (device.FlashMode)
				{
					case AVCaptureFlashMode.Auto:
						device.FlashMode = AVCaptureFlashMode.On;
						toggleFlashButton.SetBackgroundImage(UIImage.FromFile("flashOn.png"), UIControlState.Normal);
						break;
					case AVCaptureFlashMode.On:
						device.FlashMode = AVCaptureFlashMode.Off;
						toggleFlashButton.SetBackgroundImage(UIImage.FromFile("flashOff.png"), UIControlState.Normal);
						break;
					case AVCaptureFlashMode.Off:
						device.FlashMode = AVCaptureFlashMode.Auto;
						toggleFlashButton.SetBackgroundImage(UIImage.FromFile("flashAuto.png"), UIControlState.Normal);
						break;
				}

				device.UnlockForConfiguration();
			}
		}

		AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
		{
			var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);

			foreach (var device in devices)
			{
				if (device.Position == orientation)
				{
					return device;
				}
			}
			return null;
		}

		void SetupLiveCameraStream()
		{
			captureSession = new AVCaptureSession();

			var viewLayer = liveCameraStream.Layer;
			var videoPreviewLayer = new AVCaptureVideoPreviewLayer(captureSession)
			{
				Frame = liveCameraStream.Bounds
			};
			liveCameraStream.Layer.AddSublayer(videoPreviewLayer);

			var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType(AVMediaType.Video);
			ConfigureCameraForDevice(captureDevice);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);

			var dictionary = new NSMutableDictionary();
			dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
			stillImageOutput = new AVCaptureStillImageOutput()
			{
				OutputSettings = new NSDictionary()
			};

			captureSession.AddOutput(stillImageOutput);
			captureSession.AddInput(captureDeviceInput);
			captureSession.StartRunning();
		}

		void ConfigureCameraForDevice(AVCaptureDevice device)
		{
			var error = new NSError();
			if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
			{
				device.LockForConfiguration(out error);
				device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				device.UnlockForConfiguration();
			}
			else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
			{
				device.LockForConfiguration(out error);
				device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration();
			}
			else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
			{
				device.LockForConfiguration(out error);
				device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration();
			}
		}

		async void AuthorizeCameraUse()
		{
			var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);
			if (authorizationStatus != AVAuthorizationStatus.Authorized)
			{
				await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video);
			}
		}
	}
}
