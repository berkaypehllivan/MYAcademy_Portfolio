using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents;

public class _DefaultFooterComponentPartial : ViewComponent
{
    private readonly PortfolioContext _context;

    public _DefaultFooterComponentPartial(PortfolioContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var contact = _context.Contacts.FirstOrDefault();
        var about = _context.Abouts.FirstOrDefault();

        ViewBag.Github = contact.Github;
        ViewBag.Linkedin = contact.Linkedin;

        ViewBag.FirstLastName = about.FirstLastName;
        ViewBag.JobTitle = about.JobTitle;
        return View();
    }
}
