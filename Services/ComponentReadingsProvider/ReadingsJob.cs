using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;
using System;
using ServerTemperatureSystem.Models;

namespace ServerTemperatureSystem.Services.ComponentReadingsProvider
{
    [DisallowConcurrentExecution]
    public class ReadingsJob: IJob
    {
        private readonly IReadingsService _readings;
        private readonly Temperatures _temperatures;
        private readonly Usage _usage;
        private readonly ILogger<ReadingsJob> _logger;
        public ReadingsJob(ILogger<ReadingsJob> logger, 
            IReadingsService readings) 
        {
            _logger = logger;
            _readings = readings;
            _temperatures = new Temperatures();
            _usage = new Usage();
        }
        
        public async Task Execute(IJobExecutionContext context)
        {
          Components components = _temperatures.GetCurrentTemps();
            _usage.SetUsage(ref components); 
            await _readings.InsertCurrentReadings(components);
            await Task.CompletedTask;
        }
    }
}