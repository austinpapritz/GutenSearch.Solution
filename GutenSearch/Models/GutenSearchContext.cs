using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GutenSearch.Models;

public class GutenSearchContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Book> Books { get; set; }

    public GutenSearchContext(DbContextOptions options) : base(options) { }

}
