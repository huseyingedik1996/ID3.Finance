using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Quartz;

namespace ID3.Finance.Binance.BinanceService
{
    public class BackgroundJobSetup : IConfigureOptions<QuartzOptions>
    {
        public void Configure(QuartzOptions options)
        {
            var jobKey = JobKey.Create(nameof(BinanceJobService));
            options.AddJob<BinanceJobService>(jobBuilder => jobBuilder.WithIdentity(jobKey))
            .AddTrigger(trigger => trigger.ForJob(jobKey).
            WithSimpleSchedule(schedule =>
            schedule.WithIntervalInMinutes(5).RepeatForever()));
        }
        
    }
}
