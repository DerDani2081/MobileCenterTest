using System;
using Xamarin.Forms;
using System.Windows.Input;
using Mobile.Common.Utilities;

namespace Mobile.Common.Controls.ViewModel
{
	public class MainMenuItemViewModel : NotificationBase
	{
		public int Id { get; set; }
		public string Title { get; set; }

		private string _source;
		public string Source
		{
			set
			{
				if (SetProperty(ref _source, value))
				{
					RaisePropertyChanged(nameof(ImgSource));
				}
			}
		}

		public ImageSource ImgSource
		{
			get
			{
				if (string.IsNullOrEmpty(_source))
				{
					string normalizedTitle = Title.Replace(" ", "");
					return ImageSource.FromResource(string.Format("Mobile.Common.Controls.Icons.{0}.png", normalizedTitle));
				}

				return ImageSource.FromResource(string.Format("Mobile.Common.Controls.Icons.{0}.png", _source));
			}
		}

		private bool _isEnabled;

		public bool IsEnabled
		{
			get
			{
				if (CallCommandExecute != null)
				{
					_isEnabled = CallCommandCanExecute.Invoke();
				}

				return _isEnabled;
			}
			set
			{
				SetProperty(ref _isEnabled, value);
			}
		}

		public ICommand CallCommand
		{
			get
			{
				if (CallCommandExecute == null)
				{
					return null;
				}

				if (CallCommandCanExecute == null)
				{
					return new Command(CallCommandExecute);
				}

				return new Command(CallCommandExecute, CallCommandCanExecute);
			}
		}

		public Action CallCommandExecute { get; set; }
		public Func<bool> CallCommandCanExecute { get; set; }

		public MainMenuItemViewModel()
		{
			CallCommandCanExecute = () => { return IsEnabled; };
		}
	}
}
