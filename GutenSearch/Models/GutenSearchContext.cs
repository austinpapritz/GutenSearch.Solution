using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GutenSearch.Models;

public class GutenSearchContext : IdentityDbContext<ApplicationUser>
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }
    public DbSet<Copy> Copies { get; set; }

    public GutenSearchContext(DbContextOptions options) : base(options) { }

}
