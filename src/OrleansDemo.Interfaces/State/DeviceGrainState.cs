using System;
using System.Collections.Generic;
using System.Text;

namespace OrleansDemo.Interfaces.State
{
    public class DeviceGrainState
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsRunning { get; set; }
        public int Attempts { get; set; } = 0;
        public int Successes { get; set; } = 0;
        public int Failures { get; set; } = 0;
    }
}
