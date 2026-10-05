using UsedBookExchange.Domain.Entities;
using UsedBookExchange.Domain.Enums;

namespace UsedBookExchange.Infrastructure.Repositories.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();

    Task<IEnumerable<Book>> SearchAsync(
        string? searchTerm,
        string? category,
        BookCondition? condition);

    Task<Book?> GetByIdAsync(int id);

    Task AddAsync(Book book);

    Task UpdateAsync(Book book);

    Task DeleteAsync(Book book);
}