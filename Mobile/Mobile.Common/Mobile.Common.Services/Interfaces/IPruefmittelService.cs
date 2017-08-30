using System;
using Mobile.Common.Services.DataTransfairObject;

namespace Mobile.Common.Services.Interfaces
{
	public interface IPruefmittelService
	{
		DtoPruefmittel GetDtoPruefmittel(string id);
		bool SetDtoPruefmittel(DtoPruefmittel pruefmittel);

		DtoFile GetDtoFile(string UB_anhangKey);
		bool SetDtoFile(Guid pruefmittelId, DtoFile file);
	}
}
