using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents;

public class _DefaultAboutComponentPartial : ViewComponent
{
    private readonly PortfolioContext _context;

    public _DefaultAboutComponentPartial(PortfolioContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var values = _context.Abouts.FirstOrDefault();
        return View(values);
    }
}
