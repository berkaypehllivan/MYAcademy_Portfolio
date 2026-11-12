using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Areas.Admin.Controllers;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

[Area("Admin")]
public class ExperienceController : BaseController
{
    public ExperienceController(PortfolioContext context, IWebHostEnvironment env) : base(context, env)
    {
    }

    public IActionResult Index()
    {
        var values = _context.Experiences.OrderBy(x => x.DisplayOrder).ToList();
        return View(values);
    }

    [HttpGet]
    public IActionResult Create(int? id)
    {
        if (id.HasValue && id > 0)
        {
            var value = _context.Experiences.Find(id.Value);
            if (value != null) return View(value);
        }
        return View(new Experience());
    }

    [HttpPost]
    public IActionResult Create(Experience experience)
    {
        if (!ModelState.IsValid)
            return View(experience);

        experience.DisplayOrder = (_context.Experiences.Max(x => (int?)x.DisplayOrder) ?? 0) + 1;

        _context.Experiences.Add(experience);
        _context.SaveChanges();
        TempData["Success"] = "Deneyim Başarıyla Eklendi.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(Experience experience)
    {
        if (!ModelState.IsValid)
            return View(experience);

        var existing = _context.Experiences.Find(experience.ExperienceId);

        if (existing is null)
        {
            TempData["Error"] = "Veri Bulunamadı.";
            return RedirectToAction("Index");
        }

        existing.Title = experience.Title;
        existing.CompanyName = experience.CompanyName;
        existing.StartDate = experience.StartDate;
        existing.EndDate = experience.EndDate;
        existing.SideTitle = experience.SideTitle;
        existing.Description = experience.Description;
        existing.IsActive = experience.IsActive;

        _context.SaveChanges();
        TempData["Success"] = "Deneyim Başarıyla Güncellendi.";
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var value = _context.Experiences.Find(id);
        if (value != null)
        {
            _context.Experiences.Remove(value);
            _context.SaveChanges();
            TempData["Success"] = "Deneyim Başarıyla Silindi.";

        }
        return RedirectToAction("Index");
    }
    [HttpPost]
    public IActionResult MoveUp(int id)
    {
        var experiences = _context.Experiences.OrderBy(x => x.DisplayOrder).ToList();
        var index = experiences.FindIndex(x => x.ExperienceId == id);

        if (index > 0)
        {
            var temp = experiences[index];
            experiences[index] = experiences[index - 1];
            experiences[index - 1] = temp;
        }

        for (int i = 0; i < experiences.Count; i++)
        {
            experiences[i].DisplayOrder = i + 1;
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult MoveDown(int id)
    {
        var experiences = _context.Experiences.OrderBy(x => x.DisplayOrder).ToList();
        var index = experiences.FindIndex(x => x.ExperienceId == id);

        if (index >= 0 && index < experiences.Count - 1)
        {
            var temp = experiences[index];
            experiences[index] = experiences[index + 1];
            experiences[index + 1] = temp;
        }

        for (int i = 0; i < experiences.Count; i++)
        {
            experiences[i].DisplayOrder = i + 1;
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}