using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class EducationController : Controller
{
    private readonly PortfolioContext _context;

    public EducationController(PortfolioContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var values = _context.Educations.ToList();
        return View(values);
    }

    [HttpGet]
    public IActionResult Create(int? id)
    {
        if (id.HasValue && id > 0)
        {
            var value = _context.Educations.Find(id);
            if (value is not null)
                return View(value);
        }
        return View(new Education());
    }


    [HttpPost]
    public IActionResult Create(Education education)
    {
        _context.Educations.Add(education);
        _context.SaveChanges();
        TempData["Success"] = "Eğitim Başarıyla Eklendi.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(Education education)
    {
        _context.Educations.Update(education);
        _context.SaveChanges();
        TempData["Success"] = "Eğitim Başarıyla Güncellendi.";
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var value = _context.Educations.Find(id);
        if (value is not null)
        {
            _context.Educations.Remove(value);
            _context.SaveChanges();
            TempData["Success"] = "Eğitim Başarıyla Silindi.";
        }
        return RedirectToAction("Index");
    }
}
