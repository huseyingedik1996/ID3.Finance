using Microsoft.AspNetCore.Identity;

namespace ID3.Finance.Auth.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime CreatedTime { get; set; }
    }
}
