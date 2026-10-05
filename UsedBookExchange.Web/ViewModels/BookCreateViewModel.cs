using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using UsedBookExchange.Domain.Enums;

namespace UsedBookExchange.Web.ViewModels;

public class BookCreateViewModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(
    200,
    ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

[Required(ErrorMessage = "Author is required.")]
    [StringLength(
    150,
    ErrorMessage = "Author cannot exceed 150 characters.")]
    public string Author { get; set; } = string.Empty;

    [StringLength(
        1000,
        ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category is required.")]
    [StringLength(
        100,
        ErrorMessage = "Category cannot exceed 100 characters.")]
    public string Category { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please select the book condition.")]
    public BookCondition? Condition { get; set; }

    public IFormFile? Image { get; set; }

}
