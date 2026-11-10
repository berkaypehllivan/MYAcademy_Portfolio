using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

[Area("Admin")]
public class ExperienceController : Controller
{
    private readonly PortfolioContext _context;

    public ExperienceController(PortfolioContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var values = _context.Experiences.ToList();
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
        _context.Experiences.Add(experience);
        _context.SaveChanges();
        TempData["Success"] = "Deneyim Başarıyla Eklendi.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(Experience experience)
    {
        _context.Experiences.Update(experience);
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
}