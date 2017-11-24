using System.Collections.Generic;

namespace OrleansDemo.Models.ViewModels
{
    public class DeviceTypeViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public List<DeviceTypeReadingTypeViewModel> ReadingTypes { get; set; } = new List<DeviceTypeReadingTypeViewModel>();
    }

    public class DeviceTypeReadingTypeViewModel
    {
        public int? Id { get; set; }
        public int ReadingTypeId { get; set; }
        public string ReadingType { get; set; }
        public bool Active { get; set; } = false;
    }
}
