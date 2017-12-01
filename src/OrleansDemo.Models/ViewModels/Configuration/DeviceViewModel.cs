using System;
using System.Collections.Generic;

namespace OrleansDemo.Models.ViewModels.Configuration
{
    public class DeviceViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int DeviceTypeId { get; set; }
        public string DeviceType { get; set; }
        public bool Enabled { get; set; }
        public bool RunOnStartup { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }

        public IEnumerable<ReadingViewModel> Readings { get; set; } = new List<ReadingViewModel>();
    }

    public class ReadingViewModel
    {
        public Guid Id { get; set; }
        public int ReadingTypeId { get; set; }
        public string ReadingType { get; set; }
        public string ReadingUom { get; set; }
        public string ReadingDataType { get; set; }
        public bool Enabled { get; set; }
    }
}
