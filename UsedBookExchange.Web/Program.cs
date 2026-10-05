using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsedBookExchange.Infrastructure.Data;
using UsedBookExchange.Infrastructure.Identity;
using UsedBookExchange.Infrastructure.Repositories;
using UsedBookExchange.Infrastructure.Repositories.Interfaces;
using UsedBookExchange.Web.Services;
using UsedBookExchange.Web.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
builder.Configuration.GetConnectionString("DefaultConnection")
?? throw new InvalidOperationException(
"Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;

options.User.RequireUniqueEmail = true;

})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookRequestRepository, BookRequestRepository>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Handle HTTP status codes such as 404 and 403.
app.UseStatusCodePagesWithReExecute(
"/Home/StatusCode",
"?code={0}");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
name: "default",
pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
