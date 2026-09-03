using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UGB.MVC.Helper;
using UGB.MVC.Interfaces;

namespace UGB.MVC.Controllers;

public class HomeController() : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
