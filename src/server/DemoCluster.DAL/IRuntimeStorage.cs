using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL.ViewModels;

namespace DemoCluster.DAL
{
    public interface IRuntimeStorage
    {
        Task<List<DeviceSummary>> GetDashboardSummary();
    }
}