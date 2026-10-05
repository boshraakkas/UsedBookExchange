using UsedBookExchange.Domain.Enums;

namespace UsedBookExchange.Web.ViewModels;

public class AdminDashboardViewModel
{
    // Statistics
    public int TotalUsers { get; set; }

    public int TotalBooks { get; set; }

    public int AvailableBooks { get; set; }

    public int ReservedBooks { get; set; }

    public int ExchangedBooks { get; set; }

    public int PendingRequests { get; set; }

    // Dashboard data
    public List<AdminUserItemViewModel> Users { get; set; } = [];

    public List<AdminBookItemViewModel> Books { get; set; } = [];

    public List<AdminRequestItemViewModel> Requests { get; set; } = [];
}


public class AdminUserItemViewModel
{
    public string Id { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public int BooksCount { get; set; }
}


public class AdminBookItemViewModel
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


public class AdminRequestItemViewModel
{
    public int Id { get; set; }

    public string BookTitle { get; set; } = string.Empty;

    public string RequesterName { get; set; } = string.Empty;

    public RequestStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}