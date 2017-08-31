using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace MobileTests
{
	[TestFixture(Platform.Android)]
	[TestFixture(Platform.iOS)]
	public class Tests
	{
		IApp app;
		Platform platform;
		public string Path { get; }

		public Tests(Platform platform)
		{
			this.platform = platform;
			Path = $"C:\\wc\\MobileCenterTest\\Mobile.Mobile.apk";
		}

		[SetUp]
		public void BeforeEachTest()
		{
			app = ConfigureApp.Android.ApkFile(Path).StartApp();
			//app = AppInitializer.StartApp(platform);
		}

		[Test]
		public void AppLaunches()
		{
			app.Repl();
			//app.Screenshot("First screen.");
		}
	}
}

