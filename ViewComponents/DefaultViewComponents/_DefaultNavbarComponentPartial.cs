using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents;

public class _DefaultNavbarComponentPartial : ViewComponent
{
    private readonly PortfolioContext _context;

    public _DefaultNavbarComponentPartial(PortfolioContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var about = _context.Abouts.FirstOrDefault();
        ViewBag.CvUrl = about?.CvUrl;
        return View();
    }
}
