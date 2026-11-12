using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents;

public class _DefaultContactComponentPartial : ViewComponent
{
    private readonly PortfolioContext _context;

    public _DefaultContactComponentPartial(PortfolioContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var contact = _context.Contacts.FirstOrDefault();

        if (contact is null) return View();

        ViewBag.Email = contact.Email;
        ViewBag.Location = contact.Location;
        ViewBag.Linkedin = contact.Linkedin;
        ViewBag.Github = contact.Github;

        return View();
    }

}
