using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents;

public class _DefaultEducationComponentPartial : ViewComponent
{
    private readonly PortfolioContext _context;

    public _DefaultEducationComponentPartial(PortfolioContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var values = _context.Educations.OrderBy(x => x.DisplayOrder).ToList();

        return View(values);
    }
}
