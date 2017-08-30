using System;
namespace Mobile.Common.Utilities.Interfaces
{
	public interface IAppLauncher
	{
		bool IsInstalled(string target);
		void CallApplication(string target, string callParameter);
	}
}
