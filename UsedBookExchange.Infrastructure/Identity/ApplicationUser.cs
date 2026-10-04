using Microsoft.AspNetCore.Identity;

namespace UsedBookExchange.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
	public string FullName { get; set; } = string.Empty;
}