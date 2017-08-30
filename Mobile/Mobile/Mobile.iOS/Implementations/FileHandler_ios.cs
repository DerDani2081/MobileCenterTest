using System;
using System.IO;
using System.Threading.Tasks;
using Foundation;
using Mobile.Common.Utilities.Interfaces;
using Mobile.iOS.Implementations;
using QuickLook;
using Security;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHandler_ios))]
namespace Mobile.iOS.Implementations
{
	public class FileHandler_ios : IFileHandler
	{
		#region Properties
		public string LocalStoragePath => Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		public string PruefmittelPath => PathCombine(LocalStoragePath, "Pruefmittel");
		public string FilesPath => PathCombine(LocalStoragePath, "Files");
		public string CachePath => Environment.GetFolderPath(Environment.SpecialFolder.InternetCache);
		public bool IsInitialized { get; private set; }

		TaskCompletionSource<byte[]> taskCompletionSource;
		UIImagePickerController imagePicker;
		#endregion

		#region Constructor
		public FileHandler_ios()
		{
			IsInitialized = true;

			if (!Directory.Exists(PruefmittelPath))
			{
				Directory.CreateDirectory(PruefmittelPath);
				IsInitialized = false;
			}

			if (!Directory.Exists(FilesPath))
			{
				Directory.CreateDirectory(FilesPath);
				IsInitialized = false;
			}
		}
		#endregion

		#region Methods
		public string PathCombine(string p1, string p2)
		{
			return Path.Combine(p1, p2);
		}

		public bool FileExists(string filepath)
		{
			return File.Exists(filepath);
		}

		public byte[] Load(string filepath)
		{
			return File.ReadAllBytes(filepath);
		}

		public bool Save(string filepath, byte[] data)
		{
			try
			{
				if (!File.Exists(filepath))
				{
					File.Create(filepath).Close();
				}

				File.WriteAllBytes(filepath, data);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public bool Delete(string filepath)
		{
			try
			{
				File.Delete(filepath);
				return true;
			}
			catch (Exception e)
			{
				return false;
			}
		}

		public void ShowDocumentFile(string filepath, string mimeType)
		{
			var fileinfo = new FileInfo(filepath);
			var previewController = new QLPreviewController()
			{
				DataSource = new PreviewControllerDataSource(fileinfo.FullName, fileinfo.Name)
			};

			UINavigationController controller = FindNavigationController();

			if (controller != null)
			{
				controller.PresentViewController((UIViewController)previewController, true, (Action)null);
			}
		}

		public Task<byte[]> GetImageStreamAsync()
		{
			// Create and define UIImagePickerController
			imagePicker = new UIImagePickerController();
			imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
			imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

			// Set event handlers
			imagePicker.FinishedPickingMedia += OnImagePickerFinishedPickingMedia;
			imagePicker.Canceled += OnImagePickerCancelled;

			// Present UIImagePickerController;
			UIWindow window = UIApplication.SharedApplication.KeyWindow;
			var viewController = window.RootViewController;
			viewController.PresentModalViewController(imagePicker, true);

			// Return Task object
			taskCompletionSource = new TaskCompletionSource<byte[]>();

			return taskCompletionSource.Task;
		}
		#endregion

		#region HelperMethods
		private UINavigationController FindNavigationController()
		{
			foreach (var window in UIApplication.SharedApplication.Windows)
			{
				if (window.RootViewController.NavigationController != null)
				{
					return window.RootViewController.NavigationController;
				}
				else
				{
					UINavigationController value = CheckSubs(window.RootViewController.ChildViewControllers);
					if (value != null)
					{
						return value;
					}
				}
			}

			return null;
		}

		private UINavigationController CheckSubs(UIViewController[] controllers)
		{
			foreach (var controller in controllers)
			{
				if (controller.NavigationController != null)
				{
					return controller.NavigationController;
				}
				else
				{
					UINavigationController value = CheckSubs(controller.ChildViewControllers);

					if (value != null)
					{
						return value;
					}
				}
			}

			return null;
		}

		public string LoadLoginToken()
		{
			var rec = new SecRecord(SecKind.GenericPassword)
			{
				Generic = NSData.FromString("test")
			};

			SecStatusCode res;
			var match = SecKeyChain.QueryAsRecord(rec, out res);
			if (res == SecStatusCode.Success)
			{
				return match.ValueData.ToString();
				//DisplayMessage(this, "Key found, password is: {0}", match.ValueData);
			}

			//DisplayMessage(this, "Key not found: {0}", res);
			return null;// plist.StringForKey("token");

			NSUserDefaults plist = new NSUserDefaults("de.App", NSUserDefaultsType.SuiteName);
			return plist.StringForKey("token");
		}

		void OnImagePickerFinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs args)
		{
			UIImage image = args.EditedImage ?? args.OriginalImage;

			if (image != null)
			{
				// Convert UIImage to .NET Stream object
				NSData data = image.AsJPEG(1);
				byte[] array = data.ToArray();

				// Set the Stream as the completion of the Task
				taskCompletionSource.SetResult(array);
			}
			else
			{
				taskCompletionSource.SetResult(null);
			}
			imagePicker.DismissModalViewController(true);
		}

		void OnImagePickerCancelled(object sender, EventArgs args)
		{
			taskCompletionSource.SetResult(null);
			imagePicker.DismissModalViewController(true);
		}
		#endregion

		#region HelperClasses
		public class DocumentItem : QLPreviewItem
		{
			private string _title;
			private string _uri;

			public DocumentItem(string title, string uri)
			{
				_title = title;
				_uri = uri;
			}

			public override string ItemTitle
			{
				get
				{
					return _title;
				}
			}

			public override NSUrl ItemUrl
			{
				get
				{
					return NSUrl.FromFilename(_uri);
				}
			}
		}

		public class PreviewControllerDataSource : QLPreviewControllerDataSource
		{
			private string _url;
			private string _filename;

			public PreviewControllerDataSource(string url, string filename)
			{
				_url = url;
				_filename = filename;
			}

			public override IQLPreviewItem GetPreviewItem(QLPreviewController controller, nint index)
			{
				return (IQLPreviewItem)new DocumentItem(_filename, _url);
			}

			public override nint PreviewItemCount(QLPreviewController controller)
			{
				return (nint)1;
			}
		}
		#endregion
	}
}
