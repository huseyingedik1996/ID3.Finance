using Microsoft.EntityFrameworkCore;

namespace ID3.Finance.Portfolio.Models
{
    public class PortDbContext : DbContext
    {
        public PortDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) 
        {
            
        }

        public DbSet<PortfolioModel> PorfolioModel { get; set; }
        public DbSet<UserModel> UserModel { get; set; }
    }
}
