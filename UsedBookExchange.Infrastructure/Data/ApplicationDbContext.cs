using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsedBookExchange.Domain.Entities;
using UsedBookExchange.Infrastructure.Identity;

namespace UsedBookExchange.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
	public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public DbSet<Book> Books => Set<Book>();

	public DbSet<BookRequest> BookRequests => Set<BookRequest>();

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.ApplyConfigurationsFromAssembly(
			typeof(ApplicationDbContext).Assembly);
	}
}