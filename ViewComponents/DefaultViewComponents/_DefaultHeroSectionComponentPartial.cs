using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents;

public class _DefaultHeroSectionComponentPartial : ViewComponent
{
    private readonly PortfolioContext _context;

    public _DefaultHeroSectionComponentPartial(PortfolioContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var about = _context.Abouts.FirstOrDefault();
        var contact = _context.Contacts.FirstOrDefault();

        ViewBag.FirstLastName = about!.FirstLastName;
        ViewBag.JobTitle = about.JobTitle;
        ViewBag.ImageUrl = about.ImageUrl;
        ViewBag.Github = contact?.Github;
        ViewBag.Linkedin = contact?.Linkedin;

        return View();
    }
}
