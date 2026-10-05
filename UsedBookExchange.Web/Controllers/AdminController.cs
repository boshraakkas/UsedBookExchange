using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedBookExchange.Infrastructure.Repositories.Interfaces;
using UsedBookExchange.Web.ViewModels;

namespace UsedBookExchange.Web.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IAdminDashboardRepository _dashboardRepository;

public AdminController(
    IAdminDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<IActionResult> Index()
    {
        var model = new AdminDashboardViewModel
        {
            TotalUsers =
                await _dashboardRepository.GetTotalUsersAsync(),

            TotalBooks =
                await _dashboardRepository.GetTotalBooksAsync(),

            AvailableBooks =
                await _dashboardRepository.GetAvailableBooksAsync(),

            ReservedBooks =
                await _dashboardRepository.GetReservedBooksAsync(),

            ExchangedBooks =
                await _dashboardRepository.GetExchangedBooksAsync(),

            PendingRequests =
                await _dashboardRepository.GetPendingRequestsAsync(),

            Users =
                (await _dashboardRepository.GetUsersAsync())
                .Select(user => new AdminUserItemViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Role = user.Role,
                    BooksCount = user.BooksCount
                })
                .ToList(),

            Books =
                (await _dashboardRepository.GetBooksAsync())
                .Select(book => new AdminBookItemViewModel
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                    OwnerName = book.OwnerName,
                    Status = book.Status,
                    Condition = book.Condition,
                    ImageUrl = book.ImageUrl,
                    CreatedAt = book.CreatedAt
                })

                .ToList(),
                Requests =
(await _dashboardRepository.GetRequestsAsync())
.Select(request => new AdminRequestItemViewModel
{
    Id = request.Id,
    BookTitle = request.BookTitle,
    RequesterName = request.RequesterName,
    Status = request.Status,
    CreatedAt = request.CreatedAt
})
.ToList()

        };

        return View(model);
    }

}
