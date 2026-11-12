using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents;

public class _DefaultSkillComponentPartial : ViewComponent
{
    private readonly PortfolioContext _context;

    public _DefaultSkillComponentPartial(PortfolioContext context)
    {
        _context = context;
    }
    public IViewComponentResult Invoke()
    {
        var values = _context.Skills.Where(x => x.IsActive).OrderBy(x => x.DisplayOrder).ToList();
        return View(values);
    }
}
