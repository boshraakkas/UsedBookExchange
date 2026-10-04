using System.ComponentModel.DataAnnotations;

namespace UsedBookExchange.Web.ViewModels;

public class BookRequestViewModel
{
	public int BookId { get; set; }

	[StringLength(
		500,
		ErrorMessage = "Message cannot exceed 500 characters.")]
	public string? Message { get; set; }
}