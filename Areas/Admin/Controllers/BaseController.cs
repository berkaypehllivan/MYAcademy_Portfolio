using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public abstract class BaseController : Controller
{
    protected readonly IWebHostEnvironment _env;
    protected readonly PortfolioContext _context;

    protected BaseController(PortfolioContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }


    protected void DeleteFileExists(string? relativeUrl)
    {
        if (string.IsNullOrWhiteSpace(relativeUrl)) return;

        var fullPath = Path.Combine(_env.WebRootPath, relativeUrl.TrimStart('/'));
        if (System.IO.File.Exists(fullPath))
            System.IO.File.Delete(fullPath);
    }

    protected async Task<string> SaveFileAsync(IFormFile file, string subFolder)
    {
        var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", subFolder);
        Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var savePath = Path.Combine(uploadFolder, fileName);

        using (var stream = new FileStream(savePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        return $"/uploads/{subFolder}/{fileName}";
    }

    protected IActionResult MoveOrderable<T>(DbSet<T> dbSet, DbContext context, int id, bool moveUp) where T : class, IOrderable
    {
        var current = dbSet.Find(id);
        if (current is null)
        {
            TempData["Error"] = "Veri Bulunamadı.";
            return RedirectToAction("Index");
        }

        var neighbor = moveUp
        ? dbSet.Where(x => x.DisplayOrder < current.DisplayOrder)
        .OrderByDescending(x => x.DisplayOrder)
        .FirstOrDefault()
        : dbSet.Where(x => x.DisplayOrder > current.DisplayOrder)
        .OrderBy(x => x.DisplayOrder)
        .FirstOrDefault();

        if (neighbor is not null)
        {
            (current.DisplayOrder, neighbor.DisplayOrder) = (neighbor.DisplayOrder, current.DisplayOrder);
            context.SaveChanges();
        }

        return RedirectToAction("Index");
    }
}
