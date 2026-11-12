using System.Collections.Frozen;
using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class EducationController : BaseController
{
    public EducationController(PortfolioContext context, IWebHostEnvironment env) : base(context, env)
    {
    }

    public IActionResult Index()
    {
        var values = _context.Educations.OrderBy(x => x.DisplayOrder).ToList();
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
        if (!ModelState.IsValid)
            return View(education);

        if (education.EducationId == 0)
        {

            education.DisplayOrder = (_context.Educations.Max(x => (int?)x.DisplayOrder) ?? 0) + 1;
            _context.Educations.Add(education);
        }
        _context.SaveChanges();
        TempData["Success"] = "Eğitim Başarıyla Eklendi.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(Education education)
    {
        if (!ModelState.IsValid)
            return View(education);

        var existing = _context.Educations.Find(education.EducationId);
        if (existing is null)
        {
            TempData["Error"] = "Veri Bulunamadı.";
            return RedirectToAction("Index");
        }

        existing.Title = education.Title;
        existing.SubTitle = education.SubTitle;
        existing.Duration = education.Duration;
        existing.Location = education.Location;
        existing.Description = education.Description;

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


    [HttpPost]
    public IActionResult MoveUp(int id) => MoveOrderable(_context.Educations, _context, id, moveUp: true);

    [HttpPost]
    public IActionResult MoveDown(int id) => MoveOrderable(_context.Educations, _context, id, moveUp: false);
}
