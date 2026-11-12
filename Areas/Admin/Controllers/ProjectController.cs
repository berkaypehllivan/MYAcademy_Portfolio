using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class ProjectController : BaseController
{
    public ProjectController(PortfolioContext context, IWebHostEnvironment env) : base(context, env)
    {
    }

    public IActionResult Index()
    {
        var values = _context.Projects
        .Include(x => x.Category)
        .OrderBy(x => x.DisplayOrder)
        .ToList();

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
    public async Task<IActionResult> Create(Project project, IFormFile? CoverImageFile)
    {
        if (!ModelState.IsValid)
        {
            var invalidFields = ModelState
                .Where(x => x.Value.ValidationState == Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
                .Select(x => x.Key)
                .ToList();

            TempData["Error"] = "Lütfen şu alanları kontrol edin: " + string.Join(", ", invalidFields);

            LoadCategories();
            return View(project);
        }

        if (CoverImageFile is { Length: > 0 })
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            var extension = Path.GetExtension(CoverImageFile.FileName).ToLowerInvariant();
            const long maxSize = 2 * 1024 * 1024;

            if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(CoverImageFile.ContentType))
            {
                TempData["Error"] = "Yalnızca JPG, JPEG, PNG ve WEBP formatında görseller yüklenebilir.";
                LoadCategories();
                return View(project);
            }

            if (CoverImageFile.Length > maxSize)
            {
                TempData["Error"] = "Görsel boyutu 2 MB'ı geçemez.";
                LoadCategories();
                return View(project);
            }

            DeleteFileExists(project.CoverImageUrl);
            project.CoverImageUrl = await SaveFileAsync(CoverImageFile, "projects");

        }
        if (string.IsNullOrWhiteSpace(project.CoverImageUrl))
            project.CoverImageUrl = "/img/default-image.png";

        if (project.ProjectId == 0)
        {
            project.DisplayOrder = (_context.Projects.Max(x => (int?)x.DisplayOrder) ?? 0) + 1;

            _context.Projects.Add(project);
            TempData["Success"] = "Proje Başarıyla Eklendi.";
        }
        else
        {
            var existing = _context.Projects.Find(project.ProjectId);

            if (existing is null)
            {
                TempData["Error"] = "Veri Bulunamadı.";
                return RedirectToAction("Index");
            }

            existing.Title = project.Title;
            existing.CategoryId = project.CategoryId;
            existing.Keywords = project.Keywords;
            existing.GithubUrl = project.GithubUrl;
            existing.LiveDemoUrl = project.LiveDemoUrl;
            existing.CoverImageUrl = project.CoverImageUrl;
            existing.Description = project.Description;
            existing.IsActive = project.IsActive;

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
            TempData["Success"] = "Kategori Başarıyla Eklendi.";
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
            TempData["Success"] = "Kategori Başarıyla Silindi.";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult MoveUp(int id)
    {
        var projects = _context.Projects.OrderBy(x => x.DisplayOrder).ToList();
        var index = projects.FindIndex(x => x.ProjectId == id);

        if (index > 0)
        {
            var temp = projects[index];
            projects[index] = projects[index - 1];
            projects[index - 1] = temp;
        }

        for (int i = 0; i < projects.Count; i++)
        {
            projects[i].DisplayOrder = i + 1;
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult MoveDown(int id)
    {
        var projects = _context.Projects.OrderBy(x => x.DisplayOrder).ToList();
        var index = projects.FindIndex(x => x.ProjectId == id);

        if (index >= 0 && index < projects.Count - 1)
        {
            var temp = projects[index];
            projects[index] = projects[index + 1];
            projects[index + 1] = temp;
        }

        for (int i = 0; i < projects.Count; i++)
        {
            projects[i].DisplayOrder = i + 1;
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}
