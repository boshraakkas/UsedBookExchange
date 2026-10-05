using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedBookExchange.Domain.Entities;
using UsedBookExchange.Domain.Enums;
using UsedBookExchange.Infrastructure.Repositories.Interfaces;
using UsedBookExchange.Web.Services.Interfaces;
using UsedBookExchange.Web.ViewModels;

namespace UsedBookExchange.Web.Controllers;

public class BooksController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IImageService _imageService;

public BooksController(
    IBookRepository bookRepository,
    IImageService imageService)
    {
        _bookRepository = bookRepository;
        _imageService = imageService;
    }

    public async Task<IActionResult> Index(
        string? searchTerm,
        string? category,
        BookCondition? condition)
    {
        var books = await _bookRepository.SearchAsync(
            searchTerm,
            category,
            condition);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.Category = category;
        ViewBag.Condition = condition;

        return View(books);
    }

    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
            return NotFound();

        return View(book);
    }

    [Authorize]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(
        BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();

        try
        {
            var imagePath = await _imageService.SaveImageAsync(
                model.Image!);

            var book = new Book
            {
                Title = model.Title,
                Author = model.Author,
                Description = model.Description,
                Category = model.Category,
                Condition = model.Condition!.Value,
                ImageUrl = imagePath,
                OwnerId = currentUserId
            };

            await _bookRepository.AddAsync(book);

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                nameof(model.Image),
                ex.Message);

            return View(model);
        }
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
            return NotFound();

        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();

        if (book.OwnerId != currentUserId)
            return Forbid();

        if (book.Status != BookStatus.Available)
            return BadRequest(
                "Only available books can be edited.");

        var model = new BookEditViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Category = book.Category,
            Condition = book.Condition,
            CurrentImageUrl = book.ImageUrl
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(
        int id,
        BookEditViewModel model)
    {
        if (id != model.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(model);

        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
            return NotFound();

        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();

        if (book.OwnerId != currentUserId)
            return Forbid();

        if (book.Status != BookStatus.Available)
            return BadRequest(
                "Only available books can be edited.");

        try
        {
            if (model.Image != null)
            {
                var oldImagePath = book.ImageUrl;

                var newImagePath =
                    await _imageService.SaveImageAsync(
                        model.Image);

                book.ImageUrl = newImagePath;

                _imageService.DeleteImage(oldImagePath);
            }

            book.Title = model.Title;
            book.Author = model.Author;
            book.Description = model.Description;
            book.Category = model.Category;
            book.Condition = model.Condition!.Value;

            await _bookRepository.UpdateAsync(book);

            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(
                nameof(model.Image),
                ex.Message);

            return View(model);
        }
    }

    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
            return NotFound();

        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();

        if (book.OwnerId != currentUserId)
            return Forbid();

        if (book.Status != BookStatus.Available)
            return BadRequest(
                "Only available books can be deleted.");

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
            return NotFound();

        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
            return Unauthorized();

        if (book.OwnerId != currentUserId)
            return Forbid();

        if (book.Status != BookStatus.Available)
            return BadRequest(
                "Only available books can be deleted.");

        _imageService.DeleteImage(book.ImageUrl);

        await _bookRepository.DeleteAsync(book);

        return RedirectToAction(nameof(Index));
    }

}
