using System;
using System.Collections.Generic;
using System.Text;

namespace OrleansDemo.Models.ViewModels.Runtime
{
    public class ReadingViewModel
    {
        public int? ReadingId { get; set; }
        public string Device { get; set; }
        public string Type { get; set; }
        public string UOM { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }
        public DateTime? InsertedAt { get; set; }
    }
}
