using UsedBookExchange.Domain.Enums;

namespace UsedBookExchange.Domain.Entities;

public class Book
{
	public int Id { get; set; }

	public string Title { get; set; } = string.Empty;

	public string Author { get; set; } = string.Empty;

	public string Description { get; set; } = string.Empty;

	public string Category { get; set; } = string.Empty;

	public BookCondition Condition { get; set; }

	public string? ImageUrl { get; set; }

	public BookStatus Status { get; set; } = BookStatus.Available;

	public string OwnerId { get; set; } = string.Empty;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}