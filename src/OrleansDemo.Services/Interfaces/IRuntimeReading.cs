using OrleansDemo.Models.ViewModels.Runtime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansDemo.Services.Interfaces
{
    public interface IRuntimeReading : IDisposable
    {
        Task<List<ReadingViewModel>> GetReadingList();
        Task<ReadingViewModel> GetReading(int id);
        Task SaveReading(ReadingViewModel reading);
    }
}
