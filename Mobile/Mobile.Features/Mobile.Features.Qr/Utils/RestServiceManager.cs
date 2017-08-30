using System;
using AutoMapper;
using Mobile.Common.Services.DataTransfairObject;
using Mobile.Common.Services.Interfaces;
using Mobile.Common.Utilities.Interfaces;
using Mobile.Features.Qr.Model;
using Xamarin.Forms;

namespace Mobile.Features.Qr.Utils
{
	public class RestServiceManager
	{
		private static IPruefmittelService _pruefmittelService;

		private static IPruefmittelService PruefmittelService
		{
			get
			{
				if (_pruefmittelService == null)
				{
					_pruefmittelService = DependencyService.Get<IPruefmittelService>();

					if (_pruefmittelService == null)
					{
						throw new Exception("Missing implementation of " + nameof(IPruefmittelService));
					}
				}

				return _pruefmittelService;
			}
		}

		private static IFileHandler _fileHandler;

		private static IFileHandler FileHandler
		{
			get
			{
				if (_fileHandler == null)
				{
					_fileHandler = DependencyService.Get<IFileHandler>();

					if (_fileHandler == null)
					{
						throw new Exception("Missing implementation of " + nameof(IFileHandler));
					}
				}

				return _fileHandler;
			}
		}

		private static IMapper Mappings
		{
			get
			{
				MapperConfiguration config = new MapperConfiguration(cfg =>
				{
					cfg.CreateMap<DtoPruefmittel, PruefmittelItem>();
					cfg.CreateMap<DtoFile, FileItem>();
				});

				IMapper mapper = config.CreateMapper();

				return mapper;
			}
		}

		private static IMapper InvertedMappings
		{
			get
			{
				MapperConfiguration config = new MapperConfiguration(cfg =>
				{
					cfg.CreateMap<PruefmittelItem, DtoPruefmittel>();
					cfg.CreateMap<FileItem, DtoFile>();
				});

				IMapper mapper = config.CreateMapper();

				return mapper;
			}
		}

		public static PruefmittelItem LoadPruefmittel(string uid)
		{
			DtoPruefmittel pm = PruefmittelService.GetDtoPruefmittel(uid);
			PruefmittelItem item = Mappings.Map<PruefmittelItem>(pm);

			return item;
		}

		public static void SavePruefmittel(PruefmittelItem pruefmittelItem)
		{
			DtoPruefmittel pm = InvertedMappings.Map<DtoPruefmittel>(pruefmittelItem);
			PruefmittelService.SetDtoPruefmittel(pm);
		}

		public static FileItem LoadFileItem(string id)
		{
			DtoFile dtoFile = PruefmittelService.GetDtoFile(id);
			FileItem fi = Mappings.Map<FileItem>(dtoFile);

			return fi;
		}

		internal static void LoadAndOpenFileItem(string id)
		{
			FileItem fi = LoadFileItem(id);

			string fileName = fi.FileName + "." + fi.FileExtention;
			string cacheFile = FileHandler.PathCombine(FileHandler.CachePath, fileName);

			if (FileHandler.Save(cacheFile, fi.FileContent))
			{
				string contentType = MimeTypes.GetMimeType(fileName);
				FileHandler.ShowDocumentFile(cacheFile, contentType);
			}
		}

		internal static bool SaveFileItem(PruefmittelItem pruefmittelItem, FileItem fileItem)
		{
			DtoFile dtoFile = InvertedMappings.Map<DtoFile>(fileItem);
			return PruefmittelService.SetDtoFile(pruefmittelItem.Id, dtoFile);
		}
	}
}
