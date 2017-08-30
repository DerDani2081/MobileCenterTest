using System;
using System.IO;
using Mobile.Common.Controls.Model;
using Mobile.Common.Controls.Utils;
using Mobile.Common.Utilities;
using Xamarin.Forms;

namespace Mobile.Common.Controls.ViewModel
{
	public class UserInfoViewModel : NotificationBase<UserInfo>
	{
		public UserInfoViewModel(UserInfo userInfo) : base(userInfo)
		{

		}

		public string FullName => This.FirstName + " " + This.LastName;

		public ImageSource ImageSource
		{
			get
			{
				if (This.Image == null)
				{
					return ImageSource.FromResource(string.Format("App.Common.UiElements.Icons.DefaultPerson.png"));
				}

				return ImageSource.FromStream(() => new MemoryStream(This.Image));
			}
		}

		public int PruefingenieurId => This.PruefingenieurId;
	}
}
