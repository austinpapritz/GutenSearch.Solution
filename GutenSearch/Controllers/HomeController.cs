using Microsoft.AspNetCore.Mvc;
using GutenSearch.Models;
using System.Diagnostics;

namespace GutenSearch.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        return View();
    }

}
