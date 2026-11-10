using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.Controllers;

public class DefaultController : Controller
{
    private readonly PortfolioContext _context;

    public DefaultController(PortfolioContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var projects = _context.Projects
            .Include(x => x.Category)
            .ToList();
        return View(projects);
    }
}
