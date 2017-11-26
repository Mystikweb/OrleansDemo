using System.Collections.Generic;

namespace OrleansDemo.Models.ViewModels
{
    public class ReadingTypeViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public string DataType { get; set; }
        public string Assembly { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
    }
}
