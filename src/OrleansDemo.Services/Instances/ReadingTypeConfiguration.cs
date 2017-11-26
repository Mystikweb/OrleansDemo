using OrleansDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using OrleansDemo.Models.ViewModels;
using System.Threading.Tasks;
using OrleansDemo.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OrleansDemo.Services.Instances
{
    public class ReadingTypeConfiguration : IReadingTypeConfiguration
    {
        private readonly ConfigurationContext context;

        public ReadingTypeConfiguration(ConfigurationContext configurationContext)
        {
            context = configurationContext;
        }

        public async Task<bool> ReadingTypeExistsAsync(int id)
        {
            return await context.ReadingTypes.AnyAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<ReadingTypeViewModel>> GetListAsync()
        {
            return await context.ReadingTypes.Select(r => new ReadingTypeViewModel
            {
                Id = r.Id,
                Name = r.Name,
                UOM = r.Uom,
                DataType = r.DataType,
                Assembly = r.Assembly,
                Class = r.Class,
                Method = r.Method
            }).ToListAsync();
        }

        public async Task<ReadingTypeViewModel> GetAsync(int id)
        {
            return await context.ReadingTypes.Select(r => new ReadingTypeViewModel
            {
                Id = r.Id,
                Name = r.Name,
                UOM = r.Uom,
                DataType = r.DataType,
                Assembly = r.Assembly,
                Class = r.Class,
                Method = r.Method
            }).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<ReadingTypeViewModel> SaveAsync(ReadingTypeViewModel readingType)
        {
            ReadingType model = null;
            if (readingType.Id.HasValue)
            {
                model = await context.ReadingTypes.FirstOrDefaultAsync(t => t.Id == readingType.Id.Value);
                model.Name = readingType.Name;
                model.Uom = readingType.UOM;
                model.DataType = readingType.DataType;
                model.Assembly = readingType.Assembly;
                model.Class = readingType.Class;
                model.Method = readingType.Method;

                context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                model = new ReadingType
                {
                    Name = readingType.Name,
                    Uom = readingType.UOM,
                    DataType = readingType.DataType,
                    Assembly = readingType.Assembly,
                    Class = readingType.Class,
                    Method = readingType.Method
                };

                context.ReadingTypes.Add(model);
            }

            await context.SaveChangesAsync();

            return await GetAsync(model.Id);
        }

        public async Task RemoveAsync(int id)
        {
            ReadingType readingType = await context.ReadingTypes.FirstOrDefaultAsync(t => t.Id == id);
            context.ReadingTypes.Remove(readingType);
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
