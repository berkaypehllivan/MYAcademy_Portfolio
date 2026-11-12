using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.Areas.Admin.ViewComponents.AdminLayoutViewComponents;

public class _AdminLayoutUnreadMessageBadgeViewComponentPartial : ViewComponent
{
    private readonly PortfolioContext _context;

    public _AdminLayoutUnreadMessageBadgeViewComponentPartial(PortfolioContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var count = await _context.Messages.CountAsync(x => !x.IsRead);
        return View(count);
    }

}
