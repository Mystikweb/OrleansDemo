using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrleansDemo.Models.ViewModels
{
    public class ReadingTypeViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string UOM { get; set; }
        public string DataType { get; set; }
    }
}
