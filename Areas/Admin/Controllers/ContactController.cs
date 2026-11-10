using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class ContactController : Controller
{
    private readonly PortfolioContext _context;

    public ContactController(PortfolioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var values = _context.Contacts.FirstOrDefault();
        return View(values);
    }

    [HttpPost]
    public IActionResult Index(Contact contact)
    {
        if (!ModelState.IsValid)
            return View(contact);

        _context.Contacts.Update(contact);
        _context.SaveChanges();
        TempData["Success"] = "İletişim Bilgileri Başarıyla Güncellendi.";

        return RedirectToAction("Index");
    }
}
