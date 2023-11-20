using Microsoft.AspNetCore.Identity;

namespace ID3.Finance.Auth.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Region { get; set; }
    }
}
