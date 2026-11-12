using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.Areas.Admin.ViewComponents.AdminLayoutViewComponents;

public class _AdminLayoutSidebarComponentPartial : ViewComponent
{
    private readonly PortfolioContext _context;

    public _AdminLayoutSidebarComponentPartial(PortfolioContext context)
    {
        _context = context;
    }
    public IViewComponentResult Invoke()
    {
        var about = _context.Abouts.FirstOrDefault();

        return View(about);
    }
}
