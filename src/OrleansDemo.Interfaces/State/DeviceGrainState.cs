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
    }
}
