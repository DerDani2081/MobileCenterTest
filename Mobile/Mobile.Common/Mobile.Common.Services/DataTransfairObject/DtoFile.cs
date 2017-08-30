using System;
using System.Collections.Generic;
using System.Text;

namespace Mobile.Common.Services.DataTransfairObject
{
  public class DtoFile
  {
    public string Id { get; set; }

    public Guid PruefmittelId { get; set; }
    public string FileName { get; set; }
    public string FileExtention { get; set; }
    public byte[] FileContent { get; set; }
  }
}
