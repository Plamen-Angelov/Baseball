using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Baseball.Infrastructure.Data
{
    public class BaseballDbContext : IdentityDbContext
    {
        public BaseballDbContext(DbContextOptions<BaseballDbContext> options)
            : base(options)
        {
        }
    }
}