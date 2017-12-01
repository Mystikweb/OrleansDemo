using OrleansDemo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using OrleansDemo.Models.ViewModels.Runtime;
using System.Threading.Tasks;
using OrleansDemo.Models.Runtime;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OrleansDemo.Services.Instances
{
    public class RuntimeReading : IRuntimeReading
    {
        private readonly RuntimeContext runtime;

        public RuntimeReading(RuntimeContext runtimeContext)
        {
            runtime = runtimeContext;
        }

        public Task<List<ReadingViewModel>> GetReadingList()
        {
            throw new NotImplementedException();
        }

        public async Task<ReadingViewModel> GetReading(int id)
        {
            return await runtime.Readings.Select(r => new ReadingViewModel
            {
                ReadingId = r.Id,
                Device = r.Device,
                Type = r.Type,
                UOM = r.Uom,
                DataType = r.DataType,
                Value = r.Value
            }).FirstOrDefaultAsync(s => s.ReadingId == id);
        }

        public async Task SaveReading(ReadingViewModel reading)
        {
            Reading deviceReading = new Reading()
            {
                Device = reading.Device,
                Type = reading.Type,
                DataType = reading.DataType,
                Uom = reading.UOM,
                Value = reading.Value,
                InsertedAt = DateTime.Now
            };

            runtime.Readings.Add(deviceReading);

            await runtime.SaveChangesAsync();
        }

        public void Dispose()
        {
            runtime.Dispose();
        }
    }
}
