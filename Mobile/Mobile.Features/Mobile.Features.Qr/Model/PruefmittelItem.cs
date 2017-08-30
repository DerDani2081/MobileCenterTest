using System;
using System.Collections.Generic;

namespace Mobile.Features.Qr.Model
{
	public class PruefmittelItem
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public string Inventarkennzeichen { get; set; }
		public string Seriennummer { get; set; }

		public string Hersteller { get; set; }
		public string Typ { get; set; }
		public string Pruefmittelklasse { get; set; }
		public string Pruefungsart { get; set; }
		public int Pruefungsintervall { get; set; }

		public DateTime LetztePruefung { get; set; }
		public DateTime NaechstePruefung { get; set; }

		public string Bemerkung { get; set; }

		public List<FileItem> AnlagenList { get; set; }
	}
}
