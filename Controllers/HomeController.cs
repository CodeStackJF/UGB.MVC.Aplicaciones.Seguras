using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UGB.MVC.Helper;
using UGB.MVC.Interfaces;

namespace UGB.MVC.Controllers;

public class HomeController() : Controller
{
    //Este controlador es publico, no requiere autenticación para acceder a sus acciones
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
