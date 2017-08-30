using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Mobile.Common.Services.DataTransfairObject;
using Mobile.Common.Services.Implementations;
using Mobile.Common.Services.Interfaces;
using Mobile.Common.Services.Utils;
using Mobile.Common.Utilities.Interfaces;
using Xamarin.Forms;

#if !PRODUCTION
[assembly: Dependency(typeof(StoragePruefmittelService))]
#endif
namespace Mobile.Common.Services.Implementations
{
	public class StoragePruefmittelService : IPruefmittelService
	{
		private IFileHandler _fileHandler;

		public Dictionary<Guid, DtoPruefmittel> PruefmittelDictionary { get; set; }
		public Dictionary<int, DtoFile> AnhaengeDictionary { get; set; }

		public StoragePruefmittelService()
		{
			_fileHandler = DependencyService.Get<IFileHandler>();

			if (_fileHandler == null)
			{
				throw new Exception("Missing implementation of " + nameof(IFileHandler));
			}

			if (!_fileHandler.IsInitialized)
			{
				InitDatabase();
			}
		}

		public DtoPruefmittel GetDtoPruefmittel(string id)
		{
			string dst = _fileHandler.PathCombine(_fileHandler.PruefmittelPath, id);
			byte[] data = _fileHandler.Load(dst);
			return Parser.ParseObjectFromBytes<DtoPruefmittel>(data);
		}

		public bool SetDtoPruefmittel(DtoPruefmittel pruefmittel)
		{
			byte[] data = Parser.ParseObjectToBytes(pruefmittel);
			string dst = _fileHandler.PathCombine(_fileHandler.PruefmittelPath, pruefmittel.Id.ToString());
			return _fileHandler.Save(dst, data);
		}

		public DtoFile GetDtoFile(string UB_anhangKey)
		{
			string dst = _fileHandler.PathCombine(_fileHandler.FilesPath, UB_anhangKey);
			byte[] data = _fileHandler.Load(dst);
			return Parser.ParseObjectFromBytes<DtoFile>(data);
		}

		public bool SetDtoFile(Guid pruefmittelId, DtoFile file)
		{
			//get a id for file
			file.Id = DateTime.Now.Ticks.ToString();

			//updagte Pruefmittel
			DtoPruefmittel pruefmittel = GetDtoPruefmittel(pruefmittelId.ToString());
			pruefmittel.AnlagenList.Add(new DtoFile
			{
				Id = file.Id,
				FileName = file.FileName,
				FileExtention = file.FileExtention
			});

			if (!SetDtoPruefmittel(pruefmittel))
			{
				return false;
			}

			return SetDtoFile(file);
		}

		public bool SetDtoFile(DtoFile file)
		{
			byte[] data = Parser.ParseObjectToBytes(file);
			string dst = _fileHandler.PathCombine(_fileHandler.FilesPath, file.Id.ToString());
			return _fileHandler.Save(dst, data);
		}

		private void InitDatabase()
		{
			List<DtoPruefmittel> list = new List<DtoPruefmittel>
			{
				new DtoPruefmittel
				{
					Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
					Name = "Bremsenprüfstand",
					Inventarkennzeichen = "Z169-00012",
					Seriennummer = "SGH1234-87965 H",
					Hersteller = "PCE Group",
					Typ = "4 in 1",
					Pruefmittelklasse = "Klasse 2",
					Pruefungsart = "Kalibrierung",
					Pruefungsintervall = 24,
					LetztePruefung = new DateTime(2017, 01, 01),
					NaechstePruefung = new DateTime(2019, 01, 01),
					Bemerkung = "Test",
					AnlagenList = new List<DtoFile>
					{
						new DtoFile{FileExtention = "jpg", FileName="test1", Id="000001"},
						new DtoFile{FileExtention = "jpg", FileName="test2", Id="000002"},
						new DtoFile{FileExtention = "png", FileName="test3", Id="000003"},
						new DtoFile{FileExtention = "pdf", FileName="test4", Id="000004"},
						new DtoFile{FileExtention = "pdf", FileName="test5", Id="000005"},
						new DtoFile{FileExtention = "pdf", FileName="test6", Id="000006"}
					}
				},
				new DtoPruefmittel
				{
					Id = Guid.Parse("0f8fad5b-d9cb-469f-a165-70867728950e"),
					Name = "Hebebühne",
					Inventarkennzeichen = "Z169-00023",
					Seriennummer = "SGHXXXX-87965 H",
					Hersteller = "PCE Group2",
					Typ = "2 in 1",
					Pruefmittelklasse = "Klasse 23",
					Pruefungsart = "Sichtprüfung",
					Pruefungsintervall = 12,
					LetztePruefung = new DateTime(2017, 01, 01),
					NaechstePruefung = new DateTime(2018, 01, 01),
					AnlagenList = new List<DtoFile>
					{
						new DtoFile{FileExtention = "pdf", FileName="test1", Id="000012"},
						new DtoFile{FileExtention = "jpg", FileName="test2", Id="000013"},
						new DtoFile{FileExtention = "png", FileName="test3", Id="000014"}
					}
				},
				new DtoPruefmittel
				{
					Id = Guid.Parse("96094f56-9da2-4ba2-97ff-3213e4d6f649"),
					Name = "Aufstellfläche und Einsatzbereich",
					Inventarkennzeichen="76-0455",
					Seriennummer = "842556/45/4522158/5",
					Hersteller="ATH Heinl",
					Typ = "SH-5000 (Hebebühne)",
					Pruefmittelklasse = "2",
					Pruefungsart = "Lasermessung",
					Pruefungsintervall = 24,
					Bemerkung=@"(A) Es dürfen nur Scheinwerfer an Fahrzeugen überprüft werden, 
														wenn alle Räder des Fahrzeugs innerhalb der nachgewiesenen 
														Aufstellflächen stehen.",
					LetztePruefung = new DateTime(2017, 08, 05),
					NaechstePruefung = new DateTime(2019, 08, 05),
					AnlagenList = new List<DtoFile>
					{
						new DtoFile
						{
							FileExtention = "pdf",
							FileName = "SEP-Platz-KalibrierscheinX",
							Id = "fe5ab1c8-12f4-40b4-aad3-5af141224c3c",
							FileContent = ReadFromEmbedded("SEP-Platz-KalibrierscheinX.pdf")
						},
						new DtoFile
						{
							FileExtention = "jpg",
							FileName = "SH-5000",
							Id = "2f3fb542-db31-4aeb-9094-8c23a5c5c2b6",
							FileContent=ReadFromEmbedded("SH-5000.jpg")
						}
					}
				},
				new DtoPruefmittel
				{
					Id = Guid.Parse("8d3b47b4-abd2-4289-bc5a-b4892ad2f6ad"),
					Name = "Rollenbremsprüfstand",
					Inventarkennzeichen="149711",
					Seriennummer = "3054C0246",
					Hersteller="Nussbaum",
					Typ = "BT 110/41X NTS 81X",
					Pruefmittelklasse = "1",
					Pruefungsart = "Referenzkraft- und Durchmessermessung",
					Pruefungsintervall = 24,
					LetztePruefung = new DateTime(2017, 07, 12),
					NaechstePruefung = new DateTime(2019, 07, 12),
					AnlagenList = new List<DtoFile>
					{
						new DtoFile
						{
							FileExtention = "pdf",
							FileName = "Kalibrierschein BremsenprüfstandX",
							Id = "ff14abbd-f448-45ab-b7e5-7adcc0ae9d13",
							FileContent = ReadFromEmbedded("Kalibrierschein BremsenprüfstandX.pdf")
						},
						new DtoFile
						{
							FileExtention = "jpg",
							FileName = "BT110",
							Id = "e4e38ca6-55e2-4679-a613-0ca6698d350e",
							FileContent=ReadFromEmbedded("BT110.jpg")
						}
					}
				},
				new DtoPruefmittel
				{
					Id = Guid.Parse("bad2d30e-7a93-4a2d-8c06-3a598de0817b"),
					Name = "Scheinwerfer-Einstell-Prüfgerät",
					Inventarkennzeichen="2364/5",
					Seriennummer = "102314",
					Hersteller="Hella",
					Typ = "SEG 4",
					Pruefmittelklasse = "4",
					Pruefungsart = "Sicht-, Funktionsprüfung und Justierung",
					Pruefungsintervall = 24,
					LetztePruefung = new DateTime(2017, 07, 12),
					NaechstePruefung = new DateTime(2019, 07, 12),
					AnlagenList = new List<DtoFile>
					{
						new DtoFile
						{
							FileExtention = "pdf",
							FileName = "SEP-KalibrierscheinX",
							Id = "3f1b3a6d-05af-44be-b2d0-a4743d856ab6",
							FileContent = ReadFromEmbedded("SEP-KalibrierscheinX.pdf")
						},
						new DtoFile
						{
							FileExtention = "jpg",
							FileName = "SEG4",
							Id = "8e364415-3102-477f-a684-8bb8fac39e81",
							FileContent=ReadFromEmbedded("SEG4.jpg")
						}
					}
				}
			};

			byte[] pdfBytes = ReadFromEmbedded("test.pdf");
			byte[] jpgBytes = ReadFromEmbedded("test.jpg");
			byte[] pngBytes = ReadFromEmbedded("test.png");

			foreach (DtoPruefmittel pm in list)
			{
				SetDtoPruefmittel(pm);

				foreach (DtoFile f in pm.AnlagenList)
				{
					if (f.FileContent == null)
					{
						switch (f.FileExtention.ToLower())
						{
							case "pdf":
								f.FileContent = pdfBytes;
								break;
							case "jpg":
								f.FileContent = jpgBytes;
								break;
							case "png":
								f.FileContent = pngBytes;
								break;
						}
					}

					SetDtoFile(f);
				}
			}
		}

		private byte[] ReadFromEmbedded(string filename)
		{
			filename = "Mobile.Common.Services.TestFiles." + filename;

			var assembly = this.GetType().GetTypeInfo().Assembly;

			using (Stream resFileStream = assembly.GetManifestResourceStream(filename))
			{
				if (resFileStream == null)
				{
					return null;
				}

				byte[] ba = new byte[resFileStream.Length];
				resFileStream.Read(ba, 0, ba.Length);
				return ba;
			}

		}
	}
}
