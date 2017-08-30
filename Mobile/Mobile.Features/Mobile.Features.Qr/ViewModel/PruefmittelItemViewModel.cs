using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Mobile.Common.Utilities;
using Mobile.Common.Utilities.Interfaces;
using Mobile.Features.Qr.Model;
using Mobile.Features.Qr.Utils;
using Mobile.Features.Qr.View;
using Xamarin.Forms;

namespace Mobile.Features.Qr.ViewModel
{
	public class PruefmittelItemViewModel : NotificationBase<PruefmittelItem>
	{
		private INavigationHandler _navigationHandler; 

		public PruefmittelItemViewModel(PruefmittelItem pruefmittelItem = null) : base(pruefmittelItem)
		{
			_navigationHandler = DependencyService.Get<INavigationHandler>();

			_anlagenList = new ObservableCollection<FileItemViewModel>();

			foreach (FileItem fileItem in This.AnlagenList)
			{
				FileItemViewModel fileItemViewModel = new FileItemViewModel(fileItem);
				_anlagenList.Add(fileItemViewModel);
			}

			IsModified = false;
		}

		public string Name => This.Name;
		public string Id => This.Id.ToString();
		public string Inventarkennzeichen => This.Inventarkennzeichen;
		public string Seriennummer => This.Seriennummer;
		public string Hersteller => This.Hersteller;
		public string Typ => This.Typ;
		public string Pruefmittelklasse => This.Pruefmittelklasse;
		public string Pruefungsart => This.Pruefungsart;

		public string Pruefungsintervall
		{
			get
			{
				if (This.Pruefungsintervall == 1)
				{
					return This.Pruefungsintervall + " Monat";
				}

				return This.Pruefungsintervall + " Monate";
			}
		}

		private DateTime _letztePruefungOld;

		public DateTime LetztePruefung
		{
			get { return This.LetztePruefung; }
			set
			{
				if (SetProperty(This.LetztePruefung, value, () => This.LetztePruefung = value))
				{
					IsModified = true;
					RaisePropertyChanged(nameof(LetztePruefungGui));

					This.NaechstePruefung = value.AddMonths(This.Pruefungsintervall);
					RaisePropertyChanged(nameof(NaechstePruefung));
					RaisePropertyChanged(nameof(NaechstePruefungGui));
				}
			}
		}

		public int LetztePruefungMonth
		{
			get
			{
				return This.LetztePruefung.Month;
			}
			set
			{
				DateTime letztePruefung = new DateTime(This.LetztePruefung.Year, value, This.LetztePruefung.Day);
				LetztePruefung = letztePruefung;
			}
		}

		public int LetztePruefungYear
		{
			get
			{
				return This.LetztePruefung.Year;
			}
			set
			{
				DateTime letztePruefung = new DateTime(value, This.LetztePruefung.Month, This.LetztePruefung.Day);
				LetztePruefung = letztePruefung;
			}
		}

		public string LetztePruefungGui
		{
			get
			{
				return LetztePruefung.ToString("dd.MM.yyyy");
			}
		}

		private DateTime _naechstePruefungOld;

		public DateTime NaechstePruefung
		{
			get { return This.NaechstePruefung; }
			set
			{
				if (SetProperty(This.NaechstePruefung, value, () => This.NaechstePruefung = value))
				{
					IsModified = true;
					RaisePropertyChanged(nameof(NaechstePruefungGui));

					This.LetztePruefung = value.AddMonths(-1 * This.Pruefungsintervall);
					RaisePropertyChanged(nameof(LetztePruefung));
					RaisePropertyChanged(nameof(LetztePruefungGui));
				}
			}
		}

		public int NaechstePruefungMonth
		{
			get
			{
				return This.NaechstePruefung.Month;
			}
			set
			{
				DateTime naechstePruefung = new DateTime(This.NaechstePruefung.Year, value, This.NaechstePruefung.Day);
				NaechstePruefung = naechstePruefung;
			}
		}

		public int NaechstePruefungYear
		{
			get
			{
				return This.NaechstePruefung.Year;
			}
			set
			{
				DateTime naechstePruefung = new DateTime(value, This.NaechstePruefung.Month, This.NaechstePruefung.Day);
				NaechstePruefung = naechstePruefung;
			}
		}

		public string NaechstePruefungGui
		{
			get
			{
				return NaechstePruefung.ToString("dd.MM.yyyy");
			}
		}

		private string _bemerkungOld;

		public string Bemerkung
		{
			get { return This.Bemerkung; }
			set
			{
				if (SetProperty(This.Bemerkung, value, () => This.Bemerkung = value))
				{
					IsModified = true;
				}
			}
		}

		public void ShowPreviewPage(byte[] data)
		{
			NewFileItemViewModel = new FileItemViewModel(new FileItem
			{
				FileContent = data,
				FileExtention = "jpg"
			});

			CameraResultPage page = new CameraResultPage
			{
				BindingContext = this
			};

			_navigationHandler.NavigationPage.PushAsync(page);
		}

		private FileItemViewModel _newFileItemViewModel;

		public FileItemViewModel NewFileItemViewModel
		{
			get
			{
				return _newFileItemViewModel;
			}
			set
			{
				SetProperty(ref _newFileItemViewModel, value);
			}
		}

		private ObservableCollection<FileItemViewModel> _anlagenList;

		public ObservableCollection<FileItemViewModel> AnlagenList
		{
			get { return _anlagenList; }
			set { SetProperty(ref _anlagenList, value); }
		}

		private bool _isModified;

		public bool IsModified
		{
			get { return _isModified; }
			set
			{
				if (SetProperty(ref _isModified, value))
				{
					RaisePropertyChanged(nameof(Save));
				}

				if (_isModified == false)
				{
					_letztePruefungOld = LetztePruefung;
					_naechstePruefungOld = NaechstePruefung;
					_bemerkungOld = Bemerkung;
				}
			}
		}

		public ICommand Save
		{
			get
			{
				return new Command(() => SaveChanges(), () => IsModified);
			}
		}

		public void SaveChanges()
		{
			RestServiceManager.SavePruefmittel(This);
			IsModified = false;

			_navigationHandler.NavigationPage.PopAsync();
		}

		public void CancelChanges()
		{
			LetztePruefung = _letztePruefungOld;
			NaechstePruefung = _naechstePruefungOld;
			Bemerkung = _bemerkungOld;
			IsModified = false;
		}

		public ICommand SaveNewAnhang
		{
			get
			{
				return new Command(() => SaveNewAnhangChanges());
			}
		}

		private void SaveNewAnhangChanges()
		{
			if (RestServiceManager.SaveFileItem(This, NewFileItemViewModel))
			{
				OnIsDeprecated();

				_navigationHandler.NavigationPage.PopAsync(false);
				_navigationHandler.NavigationPage.PopAsync(false);
			}
		}

		public event EventHandler IsDeprecated;

		protected virtual void OnIsDeprecated()
		{
			EventHandler handler = IsDeprecated;
			if (handler != null)
			{
				handler(this, null);
			}
		}
	}
}
