using UsedBookExchange.Domain.Enums;

namespace UsedBookExchange.Domain.DTOs;

public class AdminBookDto
{
    public int Id { get; set; }

public string Title { get; set; } = string.Empty;

    public string Author { get; set; } = string.Empty;

    public string OwnerName { get; set; } = string.Empty;

    public BookStatus Status { get; set; }

    public BookCondition Condition { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

}
