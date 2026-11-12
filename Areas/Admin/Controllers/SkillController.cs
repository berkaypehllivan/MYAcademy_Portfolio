using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Areas.Admin.Controllers;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

[Area("Admin")]
public class SkillController : BaseController
{
    public SkillController(PortfolioContext context, IWebHostEnvironment env) : base(context, env)
    {
    }

    public IActionResult Index()
    {
        var values = _context.Skills.OrderBy(x => x.DisplayOrder).ToList();
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
        if (!ModelState.IsValid)
            return View(skill);

        skill.SkillId = 0;

        skill.DisplayOrder = (_context.Skills.Max(x => (int?)x.DisplayOrder) ?? 0) + 1;


        _context.Skills.Add(skill);
        _context.SaveChanges();
        TempData["Success"] = "Yetenekler Başarıyla Eklendi.";

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(Skill skill)
    {
        if (!ModelState.IsValid)
            return View(skill);

        var existing = _context.Skills.Find(skill.SkillId);

        if (existing is null)
        {
            TempData["Error"] = "Veri Bulunamadı.";
            return RedirectToAction("Index");
        }

        existing.Title = skill.Title;
        existing.Keywords = skill.Keywords;
        existing.Description = skill.Description;
        existing.IsActive = skill.IsActive;

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
    [HttpPost]
    public IActionResult MoveUp(int id)
    {
        var skills = _context.Skills.OrderBy(x => x.DisplayOrder).ToList();
        var index = skills.FindIndex(x => x.SkillId == id);

        if (index > 0)
        {
            var temp = skills[index];
            skills[index] = skills[index - 1];
            skills[index - 1] = temp;
        }

        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].DisplayOrder = i + 1;
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult MoveDown(int id)
    {
        var skills = _context.Skills.OrderBy(x => x.DisplayOrder).ToList();
        var index = skills.FindIndex(x => x.SkillId == id);

        if (index >= 0 && index < skills.Count - 1)
        {
            var temp = skills[index];
            skills[index] = skills[index + 1];
            skills[index + 1] = temp;
        }

        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].DisplayOrder = i + 1;
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}