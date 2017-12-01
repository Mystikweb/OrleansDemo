using System.Collections.Generic;

namespace OrleansDemo.Models.ViewModels.Configuration
{
    public class DeviceTypeViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? FileId { get; set; }
        public List<DeviceTypeReadingTypeViewModel> ReadingTypes { get; set; } = new List<DeviceTypeReadingTypeViewModel>();
    }

    public class DeviceTypeFileViewModel
    {
        public int? Id { get; set; }
        public int? DecviceTypeId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string MimeType { get; set; }
        public int FileType { get; set; }
        public byte[] Data { get; set; }
    }

    public class DeviceTypeReadingTypeViewModel
    {
        public int? Id { get; set; }
        public int ReadingTypeId { get; set; }
        public string ReadingType { get; set; }
        public string ReadingTypeUom { get; set; }
        public string ReadingTypeDataType { get; set; }
        public bool Active { get; set; } = false;
    }
}
