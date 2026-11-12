using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Areas.Admin.Controllers;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

[Area("Admin")]
public class AboutController : BaseController
{
    public AboutController(PortfolioContext context, IWebHostEnvironment env) : base(context, env)
    {
    }

    [HttpGet]
    public IActionResult Index()
    {
        var about = _context.Abouts.OrderBy(x => x.AboutId).FirstOrDefault() ?? new About();
        return View(about);
    }

    [HttpPost]
    public async Task<IActionResult> Index(About model, IFormFile? ImageFile, IFormFile? CvFile)
    {
        if (!ModelState.IsValid)
            return View(model);

        var about = await _context.Abouts.OrderBy(x => x.AboutId).FirstOrDefaultAsync();

        if (ImageFile is { Length: > 0 })
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
            const long maxSize = 2 * 1024 * 1024;

            if (!allowedExtensions.Contains(extension) || !allowedContentTypes.Contains(ImageFile.ContentType))
            {
                TempData["Error"] = "Yalnızca JPG, JPEG, PNG ve WEBP formatında görseller yüklenebilir.";
                return RedirectToAction("Index");
            }

            if (ImageFile.Length > maxSize)
            {
                TempData["Error"] = "Görsel boyutu 2 MB'ı geçemez.";
                return RedirectToAction("Index");
            }

            DeleteFileExists(about?.ImageUrl);
            model.ImageUrl = await SaveFileAsync(ImageFile, "about");
        }
        else if (about is not null)
        {
            model.ImageUrl = about.ImageUrl;
        }

        if (CvFile is { Length: > 0 })
        {
            var allowedExtensions = new[] { ".pdf" };
            const long maxSize = 5 * 1024 * 1024;
            var extension = Path.GetExtension(CvFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension) || CvFile.ContentType != "application/pdf")
            {
                TempData["Error"] = "CV için yalnızca PDF formatında dosyalar yüklenebilir.";
                return RedirectToAction("Index");
            }

            if (CvFile.Length > maxSize)
            {
                TempData["Error"] = "CV dosya boyutu 5 MB'ı geçemez.";
                return RedirectToAction("Index");
            }

            DeleteFileExists(about?.CvUrl);
            model.CvUrl = await SaveFileAsync(CvFile, "about");
        }
        else if (about is not null && string.IsNullOrWhiteSpace(model.CvUrl))
        {
            model.CvUrl = about.CvUrl;
        }

        if (about is null)
            _context.Abouts.Add(model);
        else
        {
            about.FirstLastName = model.FirstLastName;
            about.JobTitle = model.JobTitle;
            about.Education = model.Education;
            about.Description = model.Description;
            about.Email = model.Email;
            about.Phone = model.Phone;
            about.Location = model.Location;
            about.ImageUrl = model.ImageUrl;
            about.CvUrl = model.CvUrl;
        }
        await _context.SaveChangesAsync();
        TempData["Success"] = "Hakkımda Başarıyla Güncellendi.";
        return RedirectToAction("Index");
    }

}