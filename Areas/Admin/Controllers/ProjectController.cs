using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class ProjectController : Controller
{
    private readonly PortfolioContext _context;
    private readonly IWebHostEnvironment _env;

    public ProjectController(PortfolioContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }
    public IActionResult Index()
    {
        var values = _context.Projects.Include(x => x.Category).ToList();

        ViewBag.Categories = _context.Categories.ToList();
        return View(values);
    }

    [HttpGet]
    public IActionResult Create(int? id)
    {
        LoadCategories();

        if (id.HasValue && id > 0)
        {
            var value = _context.Projects.Find(id);
            if (value is not null) return View(value);
        }

        return View(new Project());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Project project)
    {
        if (string.IsNullOrWhiteSpace(project.CoverImageUrl))
            project.CoverImageUrl = "/img/default-project.png";

        if (project.ProjectId == 0)
        {
            _context.Projects.Add(project);
            TempData["Success"] = "Proje Başarıyla Eklendi.";
        }
        else
        {
            _context.Projects.Update(project);
            TempData["Success"] = "Proje Başarıyla Güncellendi.";
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    private void LoadCategories()
    {
        ViewBag.CategoryList = new SelectList(_context.Categories.ToList(), "CategoryId", "CategoryName");
    }
    public IActionResult Delete(int id)
    {
        var value = _context.Projects.Find(id);

        if (value is not null)
        {
            _context.Projects.Remove(value);
            _context.SaveChanges();
            TempData["Success"] = "Proje Başarıyla Silindi.";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult AddCategory(string categoryName)
    {
        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            _context.Categories.Add(new Category { CategoryName = categoryName });
            _context.SaveChanges();
            TempData["Success"] = "Kategori Başarıyla Silindi.";
        }
        return RedirectToAction("Index");
    }

    public IActionResult DeleteCategory(int id)
    {
        var category = _context.Categories.Include(x => x.Projects).FirstOrDefault(x => x.CategoryId == id);

        if (category.Projects.Any())
        {
            TempData["Error"] = "Kategori Kullanıldığı İçin Silinemedi.";
            return RedirectToAction("Index");
        }

        if (category is not null && !category.Projects.Any())
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            TempData["Success"] = "Kategori Başarıyla Eklendi.";
        }
        return RedirectToAction("Index");
    }
}
