using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.Widget;
using Mobile.Common.Utilities.Interfaces;
using Mobile.Droid.Implementations;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHandler_android))]
namespace Mobile.Droid.Implementations
{
	public class FileHandler_android : IFileHandler
	{
		public string LocalStoragePath => System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

		public string PruefmittelPath => PathCombine(LocalStoragePath, "Pruefmittel");

		public string FilesPath => PathCombine(LocalStoragePath, "Files");

		public string CachePath
		{
			get
			{
				string doc;

				if (Android.OS.Environment.IsExternalStorageEmulated)
				{
					doc = Android.OS.Environment.ExternalStorageDirectory.ToString();
				}
				else
				{
					doc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
				}

				return Path.Combine(doc, "GtueCache");
			}
		}



		private bool _isInitialized;

		public bool IsInitialized => _isInitialized;

		public FileHandler_android()
		{
			_isInitialized = true;

			if (!Directory.Exists(PruefmittelPath))
			{
				Directory.CreateDirectory(PruefmittelPath);
				_isInitialized = false;
			}

			if (!Directory.Exists(FilesPath))
			{
				Directory.CreateDirectory(FilesPath);
				_isInitialized = false;
			}

			if (Directory.Exists(CachePath))
			{
				Directory.Delete(CachePath, true);
			}

			Directory.CreateDirectory(CachePath);
		}

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
			try
			{
				return File.ReadAllBytes(filepath);
			}
			catch (Exception e)
			{
				return null;
			}
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

		public void ShowDocumentFile(string filepath, string mimeType)
		{
			Android.Net.Uri uri = Android.Net.Uri.Parse("file://" + filepath);
			Intent intent = new Intent(Intent.ActionView);
			intent.SetDataAndType(uri, mimeType);
			intent.SetFlags(ActivityFlags.ClearWhenTaskReset | ActivityFlags.NewTask);

			try
			{
				Forms.Context.StartActivity(Intent.CreateChooser(intent, "Select App"));
			}
			catch (Exception ex)
			{
				Toast.MakeText(Forms.Context, "No Application Available to View PDF", ToastLength.Short).Show();
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

		public string LoadLoginToken()
		{
			//TODO 
			return "test";
		}

		public Task<byte[]> GetImageStreamAsync()
		{
			// Define the Intent for getting images
			Intent intent = new Intent();
			intent.SetType("image/*");
			intent.SetAction(Intent.ActionGetContent);

			// Get the MainActivity instance
			MainActivity activity = Forms.Context as MainActivity;

			// Start the picture-picker activity (resumes in MainActivity.cs)
			activity.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), 1000);

			// Save the TaskCompletionSource object as a MainActivity property
			activity.PickImageTaskCompletionSource = new TaskCompletionSource<byte[]>();

			// Return Task object
			return activity.PickImageTaskCompletionSource.Task;
		}
	}
}
