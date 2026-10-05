using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UsedBookExchange.Domain.Enums;
using UsedBookExchange.Infrastructure.Data;
using UsedBookExchange.Infrastructure.Identity;
using UsedBookExchange.Infrastructure.Repositories.Interfaces;
using UsedBookExchange.Domain.DTOs;

namespace UsedBookExchange.Infrastructure.Repositories;

public class AdminDashboardRepository
:IAdminDashboardRepository
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
public AdminDashboardRepository(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<int> GetTotalUsersAsync()
    {
        return await _userManager.Users.CountAsync();
    }

    public async Task<int> GetTotalBooksAsync()
    {
        return await _context.Books.CountAsync();
    }

    public async Task<int> GetAvailableBooksAsync()
    {
        return await _context.Books
            .CountAsync(book =>
                book.Status == BookStatus.Available);
    }

    public async Task<int> GetReservedBooksAsync()
    {
        return await _context.Books
            .CountAsync(book =>
                book.Status == BookStatus.Reserved);
    }

    public async Task<int> GetExchangedBooksAsync()
    {
        return await _context.Books
            .CountAsync(book =>
                book.Status == BookStatus.Exchanged);
    }

    public async Task<int> GetPendingRequestsAsync()
    {
        return await _context.BookRequests
            .CountAsync(request =>
                request.Status == RequestStatus.Pending);
    }

    public async Task<IEnumerable<AdminUserDto>> GetUsersAsync()
    {
        var users = await _userManager.Users
        .AsNoTracking()
        .OrderBy(user => user.FullName)
        .ToListAsync();

        var result = new List<AdminUserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var booksCount = await _context.Books
                .CountAsync(book => book.OwnerId == user.Id);

            result.Add(new AdminUserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email ?? string.Empty,
                Role = roles.FirstOrDefault() ?? "User",
                BooksCount = booksCount
            });
        }

        return result;

    }
    public async Task<IEnumerable<AdminBookDto>> GetBooksAsync()
    {
        var books = await _context.Books
        .AsNoTracking()
        .OrderByDescending(book => book.CreatedAt)
        .ToListAsync();

var result = new List<AdminBookDto>();

        foreach (var book in books)
        {
            var owner = await _userManager.FindByIdAsync(book.OwnerId);

            result.Add(new AdminBookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                OwnerName = owner?.FullName ?? "Unknown",
                Status = book.Status,
                Condition = book.Condition,
                ImageUrl = book.ImageUrl,
                CreatedAt = book.CreatedAt
            });
        }

        return result;

}



    public async Task<IEnumerable<AdminRequestDto>> GetRequestsAsync()
    {
        var requests = await _context.BookRequests
        .AsNoTracking()
        .Include(request => request.Book)
        .OrderByDescending(request => request.CreatedAt)
        .ToListAsync();

var result = new List<AdminRequestDto>();

        foreach (var request in requests)
        {
            var requester =
                await _userManager.FindByIdAsync(request.RequesterId);

            result.Add(new AdminRequestDto
            {
                Id = request.Id,
                BookTitle = request.Book.Title,
                RequesterName = requester?.FullName ?? "Unknown",
                Status = request.Status,
                CreatedAt = request.CreatedAt
            });
        }

        return result;

}


}
