using OrleansDemo.Models.ViewModels.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrleansDemo.Services.Interfaces
{
    public interface IReadingTypeConfiguration : IDisposable
    {
        Task<bool> ReadingTypeExistsAsync(int id);
        Task<IEnumerable<ReadingTypeViewModel>> GetListAsync();
        Task<ReadingTypeViewModel> GetAsync(int id);
        Task<ReadingTypeViewModel> SaveAsync(ReadingTypeViewModel readingType);
        Task RemoveAsync(int id);
    }
}
