using Microsoft.EntityFrameworkCore;
using OrleansDemo.Models.Configuration;
using OrleansDemo.Models.ViewModels.Configuration;
using OrleansDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                FileId = t.FileId,
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
                FileId = t.FileId,
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

        public async Task<DeviceTypeFileViewModel> GetFileAsync(int id)
        {
            return await context.Files.Select(f => new DeviceTypeFileViewModel
            {
                Id = f.Id,
                Name = f.Name,
                Extension = f.Extension,
                MimeType = f.MimeType,
                FileType = f.FileType,
                Data = f.Data
            }).FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<DeviceTypeViewModel> SaveAsync(DeviceTypeViewModel deviceType)
        {
            DeviceType model = null;
            if (deviceType.Id.HasValue)
            {
                model = await context.DeviceTypes.FirstOrDefaultAsync(t => t.Id == deviceType.Id.Value);
                model.Name = deviceType.Name;
                model.Active = deviceType.Active;

                context.DeviceTypes.Update(model);
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

                    context.DeviceTypeReadingTypes.Update(map);
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

        public async Task<DeviceTypeViewModel> SaveImageFileAsync(DeviceTypeFileViewModel fileViewModel)
        {
            DeviceType deviceType = await context.DeviceTypes.FirstOrDefaultAsync(t => t.Id == fileViewModel.DecviceTypeId);

            File file = null;
            if (fileViewModel.Id.HasValue)
            {
                file = await context.Files.FirstOrDefaultAsync(f => f.Id == fileViewModel.Id.Value);
                file.Name = fileViewModel.Name;
                file.Extension = fileViewModel.Extension;
                file.MimeType = fileViewModel.MimeType;
                file.FileType = fileViewModel.FileType;
                file.Data = fileViewModel.Data;
                file.UpdatedAt = DateTime.Now;
                file.UpdatedBy = "Admin";

                context.Files.Update(file);
            }
            else
            {
                file = new File()
                {
                    Name = fileViewModel.Name,
                    Extension = fileViewModel.Extension,
                    MimeType = fileViewModel.MimeType,
                    FileType = fileViewModel.FileType,
                    Data = fileViewModel.Data,
                    UpdatedAt = DateTime.Now,
                    UpdatedBy = "Admin"
                };

                context.Files.Add(file);
            }

            await context.SaveChangesAsync();

            deviceType.FileId = file.Id;

            context.DeviceTypes.Update(deviceType);
            await context.SaveChangesAsync();

            return await GetAsync(deviceType.Id);
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
