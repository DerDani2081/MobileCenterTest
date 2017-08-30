using System;
using Mobile.Common.Services.DataTransfairObject;
using Mobile.Common.Services.Interfaces;

namespace Mobile.Common.Services.Implementations
{
	public class WebPruefmittelService : IPruefmittelService
	{
		public DtoFile GetDtoFile(string UB_anhangKey)
		{
			throw new NotImplementedException();
		}

		public DtoPruefmittel GetDtoPruefmittel(string id)
		{
			throw new NotImplementedException();
		}

		public bool SetDtoFile(Guid pruefmittelId, DtoFile file)
		{
			throw new NotImplementedException();
		}

		public bool SetDtoPruefmittel(DtoPruefmittel pruefmittel)
		{
			throw new NotImplementedException();
		}
	}
}
