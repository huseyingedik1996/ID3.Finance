using ID3.Finance.Binance.BinanceService;
using Quartz;

namespace ID3.Finance.Binance.Extensions
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

            });
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
            });

            services.ConfigureOptions<BackgroundJobSetup>();
        }
    }
}



