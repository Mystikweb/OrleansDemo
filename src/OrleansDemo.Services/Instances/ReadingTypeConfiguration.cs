using OrleansDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using OrleansDemo.Models.ViewModels;
using System.Threading.Tasks;
using OrleansDemo.Models.Configuration;
using Microsoft.EntityFrameworkCore;

namespace OrleansDemo.Services.Instances
{
    public class ReadingTypeConfiguration : IReadingTypeConfiguration
    {
        private readonly ConfigurationContext context;

        public ReadingTypeConfiguration(ConfigurationContext configurationContext)
        {
            context = configurationContext;
        }

        public async Task<bool> ReadingTypeExists(int id)
        {
            return await context.ReadingTypes.AnyAsync(r => r.Id == id);
        }

        public Task<IEnumerable<ReadingTypeViewModel>> GetList()
        {
            throw new NotImplementedException();
        }

        public Task<ReadingTypeViewModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ReadingTypeViewModel> Save(ReadingTypeViewModel readingType)
        {
            throw new NotImplementedException();
        }

        public Task Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
