using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SkillfullAPI.Models.AuthorizationModels;

namespace SkillfullAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<RefreshTokenModel> RefreshTokens { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
