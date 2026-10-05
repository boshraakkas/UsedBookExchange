using UsedBookExchange.Domain.Entities;

namespace UsedBookExchange.Infrastructure.Repositories.Interfaces;

public interface IBookRequestRepository
{
    Task<IEnumerable<BookRequest>> GetAllAsync();

Task<BookRequest?> GetByIdAsync(int id);

    Task<IEnumerable<BookRequest>> GetByBookIdAsync(int bookId);

    Task<IEnumerable<BookRequest>> GetByRequesterIdAsync(
        string requesterId);

    Task<IEnumerable<BookRequest>> GetByOwnerIdAsync(
        string ownerId);

    Task AddAsync(BookRequest request);

    Task UpdateAsync(BookRequest request);

    Task UpdateRangeAsync(
        IEnumerable<BookRequest> requests);

}
