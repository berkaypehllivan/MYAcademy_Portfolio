using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

[Area("Admin")]
public class SkillController : Controller
{
    private readonly PortfolioContext _context;

    public SkillController(PortfolioContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var values = _context.Skills.ToList();
        return View(values);
    }

    [HttpGet]
    public IActionResult Create(int? id)
    {
        if (id.HasValue && id > 0)
        {
            var value = _context.Skills.Find(id);
            if (value != null) return View(value);
        }
        return View(new Skill());
    }

    [HttpPost]
    public IActionResult Create(Skill skill)
    {
        skill.SkillId = 0;

        _context.Skills.Add(skill);
        _context.SaveChanges();
        TempData["Success"] = "Yetenekler Başarıyla Eklendi.";

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(Skill skill)
    {
        _context.Skills.Update(skill);
        _context.SaveChanges();
        TempData["Success"] = "Yetenekler Başarıyla Güncellendi.";

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var value = _context.Skills.Find(id);
        if (value != null)
        {
            _context.Skills.Remove(value);
            _context.SaveChanges();
            TempData["Success"] = "Yetenekler Başarıyla Silindi.";

        }
        return RedirectToAction("Index");
    }
}