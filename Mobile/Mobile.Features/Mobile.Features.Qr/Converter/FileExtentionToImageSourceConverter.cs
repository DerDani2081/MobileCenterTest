using System;
using System.Globalization;
using Xamarin.Forms;

namespace Mobile.Features.Qr.Converter
{
	class FileExtentionToImageSourceConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string fileExtention = value as string;

			if (string.IsNullOrEmpty(fileExtention))
			{
				return null;
			}

			string imagePath;

			switch (fileExtention)
			{
				case "pdf":
					imagePath = "document";
					break;
				case "jpg":
				case "jpeg":
				case "png":
					imagePath = "image";
					break;
				default:
					imagePath = "unknownFile";
					break;
			}

			ImageSource source = ImageSource.FromFile(imagePath + ".png");

			return source;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
