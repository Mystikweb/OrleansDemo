using OrleansDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using OrleansDemo.Models.ViewModels;
using System.Threading.Tasks;
using OrleansDemo.Models.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrleansDemo.Services.Instances
{
    public class DeviceTypeConfiguration : IDeviceTypeConfiguration
    {
        private readonly ConfigurationContext context;

        public DeviceTypeConfiguration(ConfigurationContext configurationContext)
        {
            context = configurationContext;
        }

        public async Task<bool> DeviceTypeExistsAsync(int id)
        {
            return await context.DeviceTypes.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<DeviceTypeViewModel>> GetListAsync()
        {
            return await (context.DeviceTypes.Select(t => new DeviceTypeViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Active = t.Active ?? false,
                ReadingTypes = t.DeviceTypeReadingTypes.Select(r => new DeviceTypeReadingTypeViewModel
                {
                    Id = r.Id,
                    ReadingTypeId = r.ReadingTypeId,
                    ReadingType = r.ReadingType.Name,
                    ReadingTypeUom = r.ReadingType.Uom,
                    ReadingTypeDataType = r.ReadingType.DataType,
                    Active = r.Active ?? false
                }).ToList()
            })).ToListAsync();
        }

        public async Task<DeviceTypeViewModel> GetAsync(int id)
        {
            return await (context.DeviceTypes.Select(t => new DeviceTypeViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Active = t.Active ?? false,
                ReadingTypes = t.DeviceTypeReadingTypes.Select(r => new DeviceTypeReadingTypeViewModel
                {
                    Id = r.Id,
                    ReadingTypeId = r.ReadingTypeId,
                    ReadingType = r.ReadingType.Name,
                    ReadingTypeUom = r.ReadingType.Uom,
                    ReadingTypeDataType = r.ReadingType.DataType,
                    Active = r.Active ?? false
                }).ToList()
            })).FirstOrDefaultAsync(dt => dt.Id == id);
        }

        public async Task<DeviceTypeViewModel> SaveAsync(DeviceTypeViewModel deviceType)
        {
            DeviceType model = null;
            if (deviceType.Id.HasValue)
            {
                model = await context.DeviceTypes.FirstOrDefaultAsync(t => t.Id == deviceType.Id.Value);
                model.Name = deviceType.Name;
                model.Active = deviceType.Active;

                context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                model = new DeviceType
                {
                    Name = deviceType.Name,
                    Active = deviceType.Active
                };

                context.DeviceTypes.Add(model);
            }

            await context.SaveChangesAsync();

            foreach (var item in deviceType.ReadingTypes)
            {
                DeviceTypeReadingType map = null;
                if (item.Id.HasValue)
                {
                    map = await context.DeviceTypeReadingTypes.FirstOrDefaultAsync(a => a.Id == item.Id.Value);
                    map.Active = item.Active;

                    context.Entry(map).State = EntityState.Modified;
                }
                else
                {
                    map = new DeviceTypeReadingType
                    {
                        DeviceTypeId = model.Id,
                        ReadingTypeId = item.ReadingTypeId,
                        Active = item.Active
                    };

                    context.DeviceTypeReadingTypes.Add(map);
                }
            }

            await context.SaveChangesAsync();

            return await GetAsync(model.Id);
        }

        public async Task RemoveAsync(int id)
        {
            DeviceType deviceType = await context.DeviceTypes.FirstOrDefaultAsync(t => t.Id == id);

            foreach (var item in deviceType.DeviceTypeReadingTypes)
            {
                DeviceTypeReadingType readingType = await context.DeviceTypeReadingTypes.FirstOrDefaultAsync(m => m.Id == item.Id);
                context.DeviceTypeReadingTypes.Remove(readingType);
            }

            await context.SaveChangesAsync();

            context.DeviceTypes.Remove(deviceType);
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
