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
    public class DeviceConfiguration : IDeviceConfiguration
    {
        private readonly ConfigurationContext context;

        public DeviceConfiguration(ConfigurationContext configurationContext)
        {
            context = configurationContext;
        }

        public async Task<bool> DeviceExistsAsync(Guid id)
        {
            return await context.Devices.AnyAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<DeviceViewModel>> GetListAsync()
        {
            return await context.Devices.Select(d => new DeviceViewModel
            {
                Id = d.Id,
                Name = d.Name,
                DeviceTypeId = d.DeviceTypeId,
                DeviceType = d.DeviceType.Name,
                Enabled = d.Enabled ?? false,
                RunOnStartup = d.RunOnStartup,
                CreatedAt = d.CreatedAt,
                CreatedBy = d.CreatedBy,
                UpdatedAt = d.UpdatedAt,
                UpdatedBy = d.UpdatedBy,
                Readings = d.Readings.Select(r => new ReadingViewModel
                {
                    Id = r.Id,
                    ReadingTypeId = r.ReadingTypeId,
                    ReadingType = r.ReadingType.Name,
                    ReadingUom = r.ReadingType.Uom,
                    ReadingDataType = r.ReadingType.DataType,
                    Enabled = r.Enabled ?? false
                }).ToList()
            }).ToListAsync();
        }

        public async Task<DeviceViewModel> GetAsync(Guid id)
        {
            return await context.Devices.Select(d => new DeviceViewModel
            {
                Id = d.Id,
                Name = d.Name,
                DeviceTypeId = d.DeviceTypeId,
                DeviceType = d.DeviceType.Name,
                Enabled = d.Enabled ?? false,
                RunOnStartup = d.RunOnStartup,
                CreatedAt = d.CreatedAt,
                CreatedBy = d.CreatedBy,
                UpdatedAt = d.UpdatedAt,
                UpdatedBy = d.UpdatedBy,
                Readings = d.Readings.Select(r => new ReadingViewModel
                {
                    Id = r.Id,
                    ReadingTypeId = r.ReadingTypeId,
                    ReadingType = r.ReadingType.Name,
                    ReadingUom = r.ReadingType.Uom,
                    ReadingDataType = r.ReadingType.DataType,
                    Enabled = r.Enabled ?? false
                }).ToList()
            }).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<DeviceViewModel> SaveAsync(DeviceViewModel device)
        {
            Device model = null;
            if (device.Id != Guid.Empty)
            {
                model = await context.Devices.FirstOrDefaultAsync(d => d.Id == device.Id);
                model.DeviceTypeId = device.DeviceTypeId;
                model.Name = device.Name;
                model.Enabled = device.Enabled;
                model.RunOnStartup = device.RunOnStartup;
                model.UpdatedAt = DateTime.Now;
                model.UpdatedBy = "Admin";

                context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                model = new Device
                {
                    DeviceTypeId = device.DeviceTypeId,
                    Name = device.Name,
                    Enabled = device.Enabled,
                    RunOnStartup = device.RunOnStartup,
                    CreatedAt = DateTime.Now,
                    CreatedBy = "Admin"
                };

                context.Devices.Add(model);
            }

            await context.SaveChangesAsync();

            foreach (var item in device.Readings)
            {
                Reading reading = null;
                if (item.Id != Guid.Empty)
                {
                    reading = await context.Readings.FirstOrDefaultAsync(r => r.Id == item.Id);
                    reading.Enabled = item.Enabled;
                    reading.UpdatedAt = DateTime.Now;
                    reading.UpdatedBy = "Admin";

                    context.Entry(reading).State = EntityState.Modified;
                }
                else
                {
                    reading = new Reading
                    {
                        DeviceId = model.Id,
                        ReadingTypeId = item.ReadingTypeId,
                        Enabled = item.Enabled,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "Admin"
                    };

                    context.Readings.Add(reading);
                }
            }

            await context.SaveChangesAsync();

            return await GetAsync(model.Id);
        }

        public async Task RemoveAsync(Guid id)
        {
            Device device = await context.Devices.FirstOrDefaultAsync(d => d.Id == id);

            context.Readings.RemoveRange(device.Readings);
            context.Devices.Remove(device);

            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
