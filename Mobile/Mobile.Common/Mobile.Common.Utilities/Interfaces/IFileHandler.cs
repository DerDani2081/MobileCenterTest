using System;
using System.Threading.Tasks;

namespace Mobile.Common.Utilities.Interfaces
{
	public interface IFileHandler
	{
		string LocalStoragePath { get; }
		string PruefmittelPath { get; }
		string FilesPath { get; }
		string CachePath { get; }

		bool IsInitialized { get; }

		string PathCombine(string p1, string p2);

		bool FileExists(string filepath);
		byte[] Load(string filepath);
		bool Save(string filepath, byte[] data);
		bool Delete(string filepath);

		void ShowDocumentFile(string filepath, string mimeType);

		Task<byte[]> GetImageStreamAsync();
	}
}
