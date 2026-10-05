using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsedBookExchange.Domain.Entities;
using UsedBookExchange.Domain.Enums;
using UsedBookExchange.Infrastructure.Repositories.Interfaces;
using UsedBookExchange.Web.ViewModels;

namespace UsedBookExchange.Web.Controllers;

[Authorize]
public class RequestsController : Controller
{
    private readonly IBookRequestRepository _requestRepository;
    private readonly IBookRepository _bookRepository;

public RequestsController(
    IBookRequestRepository requestRepository,
    IBookRepository bookRepository)
    {
        _requestRepository = requestRepository;
        _bookRepository = bookRepository;
    }

    // GET: /Requests/Create?bookId=5
    [HttpGet]
    public async Task<IActionResult> Create(int bookId)
    {
        var book = await _bookRepository.GetByIdAsync(bookId);

        if (book == null)
        {
            return NotFound();
        }

        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        // A user cannot request their own book.
        if (book.OwnerId == currentUserId)
        {
            return Forbid();
        }

        // Only available books can be requested.
        if (book.Status != BookStatus.Available)
        {
            return BadRequest(
                "This book is not available for exchange.");
        }

        return View(new BookRequestViewModel
        {
            BookId = book.Id
        });
    }

    // POST: /Requests/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        BookRequestViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var book = await _bookRepository.GetByIdAsync(
            model.BookId);

        if (book == null)
        {
            return NotFound();
        }

        // A user cannot request their own book.
        if (book.OwnerId == currentUserId)
        {
            return Forbid();
        }

        // Only available books can be requested.
        if (book.Status != BookStatus.Available)
        {
            ModelState.AddModelError(
                string.Empty,
                "This book is not available for exchange.");

            return View(model);
        }

        // Prevent duplicate pending requests.
        var existingRequests =
            await _requestRepository.GetByRequesterIdAsync(
                currentUserId);

        var alreadyRequested = existingRequests.Any(
            request =>
                request.BookId == model.BookId &&
                request.Status == RequestStatus.Pending);

        if (alreadyRequested)
        {
            ModelState.AddModelError(
                string.Empty,
                "You already have a pending request for this book.");

            return View(model);
        }

        var request = new BookRequest
        {
            BookId = book.Id,
            RequesterId = currentUserId,
            Message = model.Message,
            Status = RequestStatus.Pending
        };

        await _requestRepository.AddAsync(request);

        return RedirectToAction(nameof(MyRequests));
    }

    // GET: /Requests/MyRequests
    [HttpGet]
    public async Task<IActionResult> MyRequests()
    {
        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var requests =
            await _requestRepository.GetByRequesterIdAsync(
                currentUserId);

        return View(requests);
    }

    // GET: /Requests/ReceivedRequests
    [HttpGet]
    public async Task<IActionResult> ReceivedRequests()
    {
        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var requests =
            await _requestRepository.GetByOwnerIdAsync(
                currentUserId);

        return View(requests);
    }

    // POST: /Requests/Accept/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Accept(int id)
    {
        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var request = await _requestRepository.GetByIdAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        // Only the book owner can accept the request.
        if (request.Book.OwnerId != currentUserId)
        {
            return Forbid();
        }

        // Only pending requests can be accepted.
        if (request.Status != RequestStatus.Pending)
        {
            return BadRequest(
                "Only pending requests can be accepted.");
        }

        // The book must still be available.
        if (request.Book.Status != BookStatus.Available)
        {
            return BadRequest(
                "This book is no longer available.");
        }

        // Accept the selected request.
        request.Status = RequestStatus.Accepted;

        // Reserve the book.
        request.Book.Status = BookStatus.Reserved;

        // Reject all other pending requests.
        var otherRequests =
            await _requestRepository.GetByBookIdAsync(
                request.BookId);

        var pendingOtherRequests = otherRequests
            .Where(otherRequest =>
                otherRequest.Id != request.Id &&
                otherRequest.Status == RequestStatus.Pending)
            .ToList();

        foreach (var otherRequest in pendingOtherRequests)
        {
            otherRequest.Status = RequestStatus.Rejected;
        }

        // Save the accepted request and reserved book.
        await _requestRepository.UpdateAsync(request);

        // Save the other rejected requests.
        if (pendingOtherRequests.Count > 0)
        {
            await _requestRepository.UpdateRangeAsync(
                pendingOtherRequests);
        }

        return RedirectToAction(
            nameof(ReceivedRequests));
    }

    // POST: /Requests/Reject/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var request = await _requestRepository.GetByIdAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        // Only the book owner can reject the request.
        if (request.Book.OwnerId != currentUserId)
        {
            return Forbid();
        }

        // Only pending requests can be rejected.
        if (request.Status != RequestStatus.Pending)
        {
            return BadRequest(
                "Only pending requests can be rejected.");
        }

        request.Status = RequestStatus.Rejected;

        await _requestRepository.UpdateAsync(request);

        return RedirectToAction(
            nameof(ReceivedRequests));
    }

    // POST: /Requests/Complete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
        var currentUserId = User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(currentUserId))
        {
            return Unauthorized();
        }

        var request = await _requestRepository.GetByIdAsync(id);

        if (request == null)
        {
            return NotFound();
        }

        // Only the book owner can complete the exchange.
        if (request.Book.OwnerId != currentUserId)
        {
            return Forbid();
        }

        // Only accepted requests can be completed.
        if (request.Status != RequestStatus.Accepted)
        {
            return BadRequest(
                "Only accepted requests can be completed.");
        }

        // The book must be reserved.
        if (request.Book.Status != BookStatus.Reserved)
        {
            return BadRequest(
                "This book is not reserved.");
        }

        // Complete the request.
        request.Status = RequestStatus.Completed;

        // Mark the book as exchanged.
        request.Book.Status = BookStatus.Exchanged;

        await _requestRepository.UpdateAsync(request);

        return RedirectToAction(
            nameof(ReceivedRequests));
    }

}
