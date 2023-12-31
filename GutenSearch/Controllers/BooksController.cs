using System;
using System.Security.AccessControl;
using System.Reflection.PortableExecutable;
using GutenSearch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace GutenSearch.Controllers;

public class BooksController : Controller
{
    private readonly GutenSearchContext _db;
    public BooksController(GutenSearchContext db)
    {
        _db = db;
    }

    public ActionResult Index()
    {
        List<Book> model = _db.Books.ToList();
        return View(model);
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [Authorize(Policy = "RequirePatronRole")]
    public IActionResult Details(int id)
    {
        Book model = _db.Books
            .Include(b => b.AuthorBooks)
            .ThenInclude(ab => ab.Author)
            .FirstOrDefault(e => e.BookId == id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    public IActionResult Create()
    {
        // Grab authors to populate dropdown.
        List<Author> authors = _db.Authors.ToList();
        MultiSelectList authorList = new MultiSelectList(authors, "AuthorId", "FullName");
        ViewBag.AuthorIds = authorList;

        // Both Create and Edit routes use `Form.cshtml`.
        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add Book";
        return View("Form");
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPost]
    public IActionResult Create(List<int> authorIds, Book book)
    {
        if (ModelState.IsValid)
        {
            // Add book to db so it gets assigned a `BookId`.
            _db.Books.Add(book);
            _db.SaveChanges();

            // Manually create the associated `AuthorBook`.
            foreach (int authorId in authorIds)
            {
                AuthorBook authorBook = new AuthorBook
                {
                    AuthorId = authorId,
                    BookId = book.BookId
                };

                // Add authorBook to db.
                _db.AuthorBooks.Add(authorBook);
            }

            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add Book";
        return View("Form");
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    public IActionResult Edit(int id)
    {
        // Fetch the book and its authors.
        Book exampleBookToBeEdited = _db.Books
            .Include(b => b.AuthorBooks)
            .ThenInclude(ab => ab.Author)
            .FirstOrDefault(e => e.BookId == id);

        if (exampleBookToBeEdited == null)
        {
            return NotFound();
        }

        // Grab authors to populate dropdown.
        List<Author> authors = _db.Authors.ToList();

        // Get the selected author IDs for the book.
        List<int> selectedAuthorIds = exampleBookToBeEdited.AuthorBooks
            .Select(ab => ab.AuthorId)
            .ToList();

        // Create `MultiSelectList`, send in `selectedAuthorIds` to preselect authors.
        MultiSelectList authorList = new MultiSelectList(authors, "AuthorId", "FullName", selectedAuthorIds);
        ViewBag.AuthorIds = authorList;

        // Both Create and Edit routes use `Form.cshtml`.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update Book";

        return View("Form", exampleBookToBeEdited);
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPost]
    public IActionResult Edit(int id, List<int> authorIds, Book book)
    {
        if (ModelState.IsValid)
        {
            _db.Update(book);
            _db.SaveChanges();

            // Fetch the book from db.
            var bookInDatabase = _db.Books
                .Include(b => b.AuthorBooks)
                .FirstOrDefault(b => b.BookId == id);

            // Clear book's old AuthorBooks list.
            bookInDatabase.AuthorBooks.Clear();

            // Manually Re-add each AuthorBook with the `List<int> authorIds` from form.
            foreach (int authorId in authorIds)
            {
                AuthorBook authorBook = new AuthorBook
                {
                    AuthorId = authorId,
                    BookId = book.BookId
                };
                // Add authorBook to db.
                _db.AuthorBooks.Add(authorBook);
            }
            _db.SaveChanges();

            return RedirectToAction("details", "books", new { id = book.BookId });
        }

        // Otherwise reload form.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update Book";
        return RedirectToAction("edit", new { id = book.BookId });
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    // Handled by wwwroot/js/site.js.
    [HttpPost]
    public IActionResult Delete(int id)
    {
        Book bookToBeDeleted = _db.Books.FirstOrDefault(s => s.BookId == id);

        if (bookToBeDeleted == null)
        {
            return NotFound();
        }

        _db.Books.Remove(bookToBeDeleted);
        _db.SaveChanges();

        // Return HTTP 200 OK to AJAX request, signalling successful deletion.
        return Ok();
    }

    // Method to validate model in db.
    private bool BookExists(int id)
    {
        return _db.Books.Any(e => e.BookId == id);
    }
}
