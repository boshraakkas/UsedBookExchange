using UsedBookExchange.Domain.Entities;

namespace UsedBookExchange.Infrastructure.Repositories.Interfaces;

public interface IBookRepository
{
	Task<IEnumerable<Book>> GetAllAsync();

	Task<Book?> GetByIdAsync(int id);

	Task AddAsync(Book book);

	Task UpdateAsync(Book book);

	Task DeleteAsync(Book book);
}