using OrleansDemo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrleansDemo.Services.Interfaces
{
    public interface IReadingTypeConfiguration : IDisposable
    {
        Task<bool> ReadingTypeExists(int id);
        Task<IEnumerable<ReadingTypeViewModel>> GetList();
        Task<ReadingTypeViewModel> Get(int id);
        Task<ReadingTypeViewModel> Save(ReadingTypeViewModel readingType);
        Task Remove(int id);
    }
}
