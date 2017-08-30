using System;
namespace Mobile.Features.Qr.Model
{
	public class FileItem
	{
		public string Id { get; set; }

		public Guid PruefmittelId { get; set; }
		public string FileName { get; set; }
		public string FileExtention { get; set; }
		public byte[] FileContent { get; set; }
	}
}
