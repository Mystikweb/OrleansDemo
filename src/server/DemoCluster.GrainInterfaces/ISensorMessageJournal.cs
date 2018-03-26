using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoCluster.GrainInterfaces
{
    public interface ISensorMessageJournal : IGrainWithIntegerKey
    {
        Task Initialiaze();
        Task<bool> StartReceiver();
        Task<bool> StopReceiver();
    }
}
