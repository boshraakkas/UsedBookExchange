using Microsoft.EntityFrameworkCore;
using UsedBookExchange.Domain.Entities;
using UsedBookExchange.Domain.Enums;
using UsedBookExchange.Infrastructure.Data;
using UsedBookExchange.Infrastructure.Repositories.Interfaces;

namespace UsedBookExchange.Infrastructure.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .AsNoTracking()
            .OrderByDescending(book => book.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Book>> SearchAsync(
        string? searchTerm,
        string? category,
        BookCondition? condition)
    {
        var query = _context.Books
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            searchTerm = searchTerm.Trim();

            query = query.Where(book =>
                book.Title.Contains(searchTerm) ||
                book.Author.Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(book =>
                book.Category == category);
        }

        if (condition.HasValue)
        {
            query = query.Where(book =>
                book.Condition == condition.Value);
        }

        return await query
            .OrderByDescending(book => book.CreatedAt)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .FirstOrDefaultAsync(book => book.Id == id);
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Book book)
    {
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}