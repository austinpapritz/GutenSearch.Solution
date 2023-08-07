using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Putting SelectList in ViewBag
using Microsoft.EntityFrameworkCore;
using GutenSearch.Models;
using System.Collections.Generic;
using System.Diagnostics;

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

    public IActionResult Details(int id)
    {
        Book model = _db.Books.FirstOrDefault(e => e.BookId == id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    public IActionResult Create()
    {
        // Both Create and Edit routes use `Form.cshtml`
        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add Book";
        return View("Form");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title")] Book exampleBook)
    {
        if (ModelState.IsValid)
        {
            _db.Books.Add(exampleBook);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add Book";
        return View("Form");
    }

    public IActionResult Edit(int id)
    {
        Book exampleBookToBeEdited = _db.Books.FirstOrDefault(e => e.BookId == id);

        if (exampleBookToBeEdited == null)
        {
            return NotFound();
        }

        // Both Create and Edit routes use `Form.cshtml`.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update Book";

        return View("Form", exampleBookToBeEdited);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("BookId,Title")] Book exampleBook)
    {
        // Ensure id from form and url match.
        if (id != exampleBook.BookId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Try to update changes, catch any ConcurrencyExceptions.
            try
            {
                _db.Update(exampleBook);
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(exampleBook.BookId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("details", "books", new { id = exampleBook.BookId });
        }

        // Otherwise reload form.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update Book";
        return RedirectToAction("edit", new { id = exampleBook.BookId });
    }

    // Method to validate model in db.
    private bool BookExists(int id)
    {
        return _db.Books.Any(e => e.BookId == id);
    }
}
