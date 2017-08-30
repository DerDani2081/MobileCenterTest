using System;
using System.IO;
using Mobile.Common.Utilities;
using Mobile.Features.Qr.Model;
using Xamarin.Forms;

namespace Mobile.Features.Qr.ViewModel
{
	public class FileItemViewModel : NotificationBase<FileItem>
	{
		public FileItemViewModel(FileItem fileItem) : base(fileItem)
		{

		}

		public string Id
		{
			get { return This.Id; }
		}

		public string FileName
		{
			set
			{
				SetProperty(This.FileName, value, () => This.FileName = value);
			}
			get
			{
				if (string.IsNullOrEmpty(This.FileName))
				{
					return This.Id;
				}

				return This.FileName;
			}
		}

        public string FileNameWithExtension
        {
            get
            {
                if (string.IsNullOrEmpty(This.FileName))
                {
                    return This.Id;
                }

                return $"{This.FileName}.{This.FileExtention}";
            }
        }

        public string FileExtention
		{
			get { return This.FileExtention; }
		}

		public byte[] FileContent
		{
			get { return This.FileContent; }
		}

        public string FileSize
        {
            get
            {
                return $"{FileContent.Length/1024} KB";
            }
        }

		public ImageSource ImageSource
		{
			get
			{
				if (FileContent == null)
				{
					return null;
				}

				return ImageSource.FromStream(() => new MemoryStream(FileContent));
			}
		}

		public FileItem Model
		{
			get
			{
				return This;
			}
		}
	}
}
