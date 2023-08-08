using GutenSearch.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GutenSearch.Controllers;

public class AuthorsController : Controller
{
    private readonly GutenSearchContext _db;
    public AuthorsController(GutenSearchContext db)
    {
        _db = db;
    }

    public ActionResult Index()
    {
        List<Author> model = _db.Authors.ToList();
        return View(model);
    }

    public IActionResult Details(int id)
    {
        Author model = _db.Authors.FirstOrDefault(e => e.AuthorId == id);

        if (model == null)
        {
            return NotFound();
        }

        return View(model);
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    public IActionResult Create()
    {
        // Both Create and Edit routes use `Form.cshtml`
        ViewData["FormAction"] = "Create";
        ViewData["SubmitButton"] = "Add Author";
        return View("Form");
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("FirstName,LastName")] Author author)
    {
        if (!ModelState.IsValid)
        {
            // Reload form if model is invalid.
            ViewData["FormAction"] = "Create";
            ViewData["SubmitButton"] = "Add Author";
            return View("Form");
        }

        // If model looks good, add to server and go to author list page.
        _db.Authors.Add(author);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    public IActionResult Edit(int id)
    {
        Author authorToBeEdited = _db.Authors.FirstOrDefault(e => e.AuthorId == id);

        if (authorToBeEdited != null)
        {
            ViewData["FormAction"] = "Edit";
            ViewData["SubmitButton"] = "Update Author";

            return View("Form", authorToBeEdited);
        }

        return View("Index");
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("AuthorId,FirstName,LastName")] Author author)
    {
        // Ensure id from form and url match.
        if (id != author.AuthorId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            // Try to update changes, catch any ConcurrencyExceptions.
            try
            {
                _db.Update(author);
                _db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(author.AuthorId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("details", "Authors", new { id = author.AuthorId });
        }

        // Otherwise reload form.
        ViewData["FormAction"] = "Edit";
        ViewData["SubmitButton"] = "Update Author";
        return RedirectToAction("edit", new { id = author.AuthorId });
    }

    // Method to validate model in db.
    private bool AuthorExists(int id)
    {
        return _db.Authors.Any(e => e.AuthorId == id);
    }
}
