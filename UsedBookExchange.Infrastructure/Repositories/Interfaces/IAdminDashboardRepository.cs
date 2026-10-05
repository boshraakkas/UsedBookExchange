using UsedBookExchange.Domain.DTOs;

namespace UsedBookExchange.Infrastructure.Repositories.Interfaces;

public interface IAdminDashboardRepository
{
    Task<int> GetTotalUsersAsync();

Task<int> GetTotalBooksAsync();

    Task<int> GetAvailableBooksAsync();

    Task<int> GetReservedBooksAsync();

    Task<int> GetExchangedBooksAsync();

    Task<int> GetPendingRequestsAsync();

    Task<IEnumerable<AdminUserDto>> GetUsersAsync();
    Task<IEnumerable<AdminBookDto>> GetBooksAsync();
    Task<IEnumerable<AdminRequestDto>> GetRequestsAsync();

}
