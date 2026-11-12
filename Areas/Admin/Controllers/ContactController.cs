using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class ContactController : BaseController
{
    public ContactController(PortfolioContext context, IWebHostEnvironment env) : base(context, env)
    {
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

        var existing = _context.Contacts.Find(contact.ContactId);

        if (existing is null)
        {
            TempData["Error"] = "Veri Bulunamadı.";
            return RedirectToAction("Index");
        }

        existing.Email = contact.Email;
        existing.Phone = contact.Phone;
        existing.Location = contact.Location;
        existing.Linkedin = contact.Linkedin;
        existing.Github = contact.Github;

        _context.SaveChanges();
        TempData["Success"] = "İletişim Başarıyla Güncellendi.";
        return RedirectToAction("Index");
    }
}
