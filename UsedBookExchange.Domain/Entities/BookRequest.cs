using UsedBookExchange.Domain.Enums;

namespace UsedBookExchange.Domain.Entities;

public class BookRequest
{
	public int Id { get; set; }

	public int BookId { get; set; }

	public string RequesterId { get; set; } = string.Empty;

	public string? Message { get; set; }

	public RequestStatus Status { get; set; } = RequestStatus.Pending;

	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	public Book Book { get; set; } = null!;
}