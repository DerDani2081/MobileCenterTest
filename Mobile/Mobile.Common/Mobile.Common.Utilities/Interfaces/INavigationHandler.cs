using System;
using Xamarin.Forms;

namespace Mobile.Common.Utilities.Interfaces
{
	public interface INavigationHandler
	{
		string Title{ set; }
		NavigationPage NavigationPage { get; }
	}
}
