using Microsoft.EntityFrameworkCore;
using UsedBookExchange.Domain.Entities;
using UsedBookExchange.Infrastructure.Data;
using UsedBookExchange.Infrastructure.Repositories.Interfaces;

namespace UsedBookExchange.Infrastructure.Repositories;

public class BookRequestRepository : IBookRequestRepository
{
	private readonly ApplicationDbContext _context;

	public BookRequestRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<BookRequest>> GetAllAsync()
	{
		return await _context.BookRequests
			.AsNoTracking()
			.Include(request => request.Book)
			.OrderByDescending(request => request.CreatedAt)
			.ToListAsync();
	}

	public async Task<BookRequest?> GetByIdAsync(int id)
	{
		return await _context.BookRequests
			.Include(request => request.Book)
			.FirstOrDefaultAsync(request => request.Id == id);
	}

	public async Task<IEnumerable<BookRequest>> GetByBookIdAsync(int bookId)
	{
		return await _context.BookRequests
			.AsNoTracking()
			.Where(request => request.BookId == bookId)
			.OrderByDescending(request => request.CreatedAt)
			.ToListAsync();
	}

	public async Task<IEnumerable<BookRequest>> GetByRequesterIdAsync(
		string requesterId)
	{
		return await _context.BookRequests
			.AsNoTracking()
			.Include(request => request.Book)
			.Where(request => request.RequesterId == requesterId)
			.OrderByDescending(request => request.CreatedAt)
			.ToListAsync();
	}

	public async Task AddAsync(BookRequest request)
	{
		await _context.BookRequests.AddAsync(request);
		await _context.SaveChangesAsync();
	}

	public async Task UpdateAsync(BookRequest request)
	{
		_context.BookRequests.Update(request);
		await _context.SaveChangesAsync();
	}


    public async Task<IEnumerable<BookRequest>> GetByOwnerIdAsync(
    string ownerId)
    {
        return await _context.BookRequests
            .AsNoTracking()
            .Include(request => request.Book)
            .Where(request => request.Book.OwnerId == ownerId)
            .OrderByDescending(request => request.CreatedAt)
            .ToListAsync();
    }




    public async Task UpdateRangeAsync(
        IEnumerable<BookRequest> requests)
    {
        _context.BookRequests.UpdateRange(requests);

        await _context.SaveChangesAsync();
    }
}