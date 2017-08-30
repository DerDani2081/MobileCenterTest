using System;
using System.Windows.Input;
using Mobile.Common.Services.Interfaces;
using Mobile.Common.Utilities;
using Mobile.Common.Utilities.Interfaces;
using Mobile.Features.Qr.Model;
using Mobile.Features.Qr.Utils;
using Mobile.Features.Qr.View;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Mobile.Features.Qr.ViewModel
{
	public class QrMainViewModel : NotificationBase
	{
		#region Properties
		private IFileHandler _fileHandler;
		private INavigationHandler _navigationHandler;

		private string _pruefmittelGuid;

		public string PruefmittelGuid
		{
			set
			{
				string applink = "degtueappqr://";

				if (value.StartsWith(applink, System.StringComparison.Ordinal) == true)
				{
					value = value.Remove(0, applink.Length);
				}

				if (SetProperty(ref _pruefmittelGuid, value))
				{
					LoadPruefmittel(value);
				}
			}
			get
			{
				return _pruefmittelGuid;
			}
		}

		private PruefmittelItemViewModel _pruefmittelItemViewModel;

		public PruefmittelItemViewModel PruefmittelItemViewModel
		{
			get
			{
				return _pruefmittelItemViewModel;
			}
			set
			{
				if (SetProperty(ref _pruefmittelItemViewModel, value))
				{
					_pruefmittelItemViewModel.IsDeprecated += (sender, e) =>
					{
						PruefmittelItemViewModel pruefmittelItemViewModel = sender as PruefmittelItemViewModel;

						if (pruefmittelItemViewModel == null || string.IsNullOrEmpty(pruefmittelItemViewModel.Id))
						{
							return;
						}

						LoadPruefmittel(pruefmittelItemViewModel.Id);
					};
				}
			}
		}

		private FileItemViewModel _selectedAnlage;

		public FileItemViewModel SelectedAnlage
		{
			get
			{
				return _selectedAnlage;
			}
			set
			{
				if (SetProperty(ref _selectedAnlage, value))
				{
					RestServiceManager.LoadAndOpenFileItem(value.Id);

					_selectedAnlage = null;
					RaisePropertyChanged(nameof(SelectedAnlage));
				}
			}
		}
		#endregion

		#region Constructor
		public QrMainViewModel()
		{
			_fileHandler = DependencyService.Get<IFileHandler>();

			if (_fileHandler == null)
			{
				throw new Exception("Missing implementation of " + nameof(IFileHandler));
			}

			_navigationHandler = DependencyService.Get<INavigationHandler>();

			if (_navigationHandler == null)
			{
				throw new Exception("Missing implementation of " + nameof(INavigationHandler));
			}

			_navigationHandler.Title = "QR-Code";
		}
		#endregion

		#region HelperMethods
		public void PushScanPage()
		{
			ZXingScannerPage scanPage = new ZXingScannerPage()
			{
				Title = "QR-Code-Scanner"
			};

			scanPage.OnScanResult += (result) =>
			{
				// Stop scanning
				scanPage.IsScanning = false;

				// Pop the page and show the result
				Device.BeginInvokeOnMainThread(() =>
				{
					_navigationHandler.NavigationPage.PopAsync();
					PruefmittelGuid = result.Text;
				});
			};

			_navigationHandler.NavigationPage.PushAsync(scanPage);
		}

		private void LoadPruefmittel(string guid)
		{
			PruefmittelItem pruefmittelitem = RestServiceManager.LoadPruefmittel(guid);

			if (pruefmittelitem == null)
			{
				return;
			}

			PruefmittelItemViewModel = new PruefmittelItemViewModel(pruefmittelitem);
		}
		#endregion

		#region Commands
		public ICommand StartQrScanner
		{
			get
			{
				return new Command(PushScanPage);
			}
		}

		public ICommand OpenBemerkungEditAction
		{
			get
			{
				return new Command(() =>
				{
					EditCommentView edit = new EditCommentView
					{
						BindingContext = PruefmittelItemViewModel
					};

						_navigationHandler.NavigationPage.PushAsync(edit);
				});
			}
		}

		public ICommand OpenEditLetztePruefung
		{
			get
			{
				return new Command(() =>
				{
					EditDateView edit = new EditDateView
					{
						BindingContext = PruefmittelItemViewModel,
						PropertyName = nameof(PruefmittelItemViewModel.LetztePruefung)
					};

					_navigationHandler.NavigationPage.PushAsync(edit);
				});
			}
		}

		public ICommand OpenEditNaechstePruefung
		{
			get
			{
				return new Command(() =>
				{
					EditDateView edit = new EditDateView
					{
						BindingContext = PruefmittelItemViewModel,
						PropertyName = nameof(PruefmittelItemViewModel.NaechstePruefung)
					};

					_navigationHandler.NavigationPage.PushAsync(edit);
				});
			}
		}

		public ICommand AddAnhangFile
		{
			get
			{
				return new Command(async () => 
				{
					string action;

					if (Device.RuntimePlatform == Device.iOS)
					{
						//TODO evtl icloud upload
						action = await _navigationHandler.NavigationPage.CurrentPage.DisplayActionSheet("Quelle für Anhänge wählen", "Cancel", null, "Kamera", "Foto & Video Mediathek");
					}
					else
					{
						action = await _navigationHandler.NavigationPage.CurrentPage.DisplayActionSheet("Quelle für Anhänge wählen", "Cancel", null, "Kamera", "Foto & Video Mediathek", "Dokument");
					}

					switch (action)
					{
						case "Kamera":
							CameraPage page = new CameraPage
							{
								BindingContext = PruefmittelItemViewModel
							};

							await _navigationHandler.NavigationPage.PushAsync(page);
							break;

						case "Foto & Video Mediathek":
							byte[] data = await _fileHandler.GetImageStreamAsync();
							PruefmittelItemViewModel.ShowPreviewPage(data);
							break;

						case "Dokument":
							//TODO document selection
							break;
					}
				});
			}
		}
		#endregion
	}
}
