using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ID3.Finance.Auth.Entities
{
    public class UserDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options) 
        {
            
        }
    }
}
