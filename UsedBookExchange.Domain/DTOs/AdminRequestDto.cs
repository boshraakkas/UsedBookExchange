using UsedBookExchange.Domain.Enums;

namespace UsedBookExchange.Domain.DTOs;

public class AdminRequestDto
{
    public int Id { get; set; }

public string BookTitle { get; set; } = string.Empty;

    public string RequesterName { get; set; } = string.Empty;

    public RequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }

}
