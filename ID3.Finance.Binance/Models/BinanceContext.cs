using ID3.Finance.Binance.Models.BinanceModels;
using Microsoft.EntityFrameworkCore;

namespace ID3.Finance.Binance.Models
{
    public class BinanceContext : DbContext
    {
        public BinanceContext(DbContextOptions<BinanceContext> options) : base(options) 
        {
            
        }

        public DbSet<BinanceKlineResultModel> KlineResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        
            base.OnModelCreating(modelBuilder);
        }
    }
}
