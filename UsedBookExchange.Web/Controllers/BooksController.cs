using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedBookExchange.Domain.Entities;
using UsedBookExchange.Domain.Enums;
using UsedBookExchange.Infrastructure.Repositories.Interfaces;
using UsedBookExchange.Web.ViewModels;

namespace UsedBookExchange.Web.Controllers;

public class BooksController : Controller
{
    private readonly IBookRepository _bookRepository;

    public BooksController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    // GET: /Books
    public async Task<IActionResult> Index()
    {
        var books = await _bookRepository.GetAllAsync();

        return View(books);
    }

    // GET: /Books/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // GET: /Books/Create
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create(BookCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var currentUserId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var book = new Book
        {
            Title = model.Title,
            Author = model.Author,
            Description = model.Description,
            Category = model.Category,
            Condition = model.Condition,
            ImageUrl = model.ImageUrl,
            OwnerId = currentUserId
        };

        await _bookRepository.AddAsync(book);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Books/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        var currentUserId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (book.OwnerId != currentUserId)
        {
            return Forbid();
        }

        if (book.Status != BookStatus.Available)
        {
            return BadRequest(
                "Only available books can be edited.");
        }

        var model = new BookEditViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Category = book.Category,
            Condition = book.Condition,
            ImageUrl = book.ImageUrl
        };

        return View(model);
    }

    // POST: /Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Edit(
        int id,
        BookEditViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        var currentUserId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (book.OwnerId != currentUserId)
        {
            return Forbid();
        }

        if (book.Status != BookStatus.Available)
        {
            return BadRequest(
                "Only available books can be edited.");
        }

        book.Title = model.Title;
        book.Author = model.Author;
        book.Description = model.Description;
        book.Category = model.Category;
        book.Condition = model.Condition;
        book.ImageUrl = model.ImageUrl;

        await _bookRepository.UpdateAsync(book);

        return RedirectToAction(nameof(Index));
    }

    // GET: /Books/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        var currentUserId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (book.OwnerId != currentUserId)
        {
            return Forbid();
        }

        if (book.Status != BookStatus.Available)
        {
            return BadRequest(
                "Only available books can be deleted.");
        }

        return View(book);
    }

    // POST: /Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        var currentUserId = User.FindFirst(
            System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (book.OwnerId != currentUserId)
        {
            return Forbid();
        }

        if (book.Status != BookStatus.Available)
        {
            return BadRequest(
                "Only available books can be deleted.");
        }

        await _bookRepository.DeleteAsync(book);

        return RedirectToAction(nameof(Index));
    }
}