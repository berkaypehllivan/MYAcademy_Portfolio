using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

[Area("Admin")]
public class AboutController : Controller
{
    private readonly PortfolioContext _context;
    private readonly IWebHostEnvironment _env;

    public AboutController(PortfolioContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
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
        var about = await _context.Abouts.OrderBy(x => x.AboutId).FirstOrDefaultAsync();

        var uploadFolder = Path.Combine(_env.WebRootPath, "uploads", "about");
        if (ImageFile is { Length: > 0 } || CvFile is { Length: > 0 })
        {
            Directory.CreateDirectory(uploadFolder);
        }

        // 1. Profil Görseli İşlemleri
        if (ImageFile is { Length: > 0 })
        {
            var allowedImageExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var imageExtension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

            if (!allowedImageExtensions.Contains(imageExtension))
            {
                TempData["Error"] = "Yalnızca JPG, JPEG, PNG ve WEBP formatında görseller yüklenebilir.";
                return RedirectToAction("Index");
            }

            var imageFileName = $"{Guid.NewGuid()}{imageExtension}";
            var imageFilePath = Path.Combine(uploadFolder, imageFileName);

            using (var stream = new FileStream(imageFilePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            model.ImageUrl = $"/uploads/about/{imageFileName}";
        }
        else if (about is not null)
        {
            model.ImageUrl = about.ImageUrl;
        }

        // 2. CV Dosyası İşlemleri
        if (CvFile is { Length: > 0 })
        {
            var allowedCvExtensions = new[] { ".pdf" };
            var cvExtension = Path.GetExtension(CvFile.FileName).ToLowerInvariant();

            if (!allowedCvExtensions.Contains(cvExtension))
            {
                TempData["Error"] = "CV için yalnızca PDF formatında dosyalar yüklenebilir.";
                return RedirectToAction("Index");
            }

            var cvFileName = $"{Guid.NewGuid()}{cvExtension}";
            var cvFilePath = Path.Combine(uploadFolder, cvFileName);

            using (var stream = new FileStream(cvFilePath, FileMode.Create))
            {
                await CvFile.CopyToAsync(stream);
            }

            model.CvUrl = $"/uploads/about/{cvFileName}";
        }
        else if (about is not null && string.IsNullOrEmpty(model.CvUrl))
        {
            model.CvUrl = about.CvUrl;
        }

        // 3. Veritabanı Kayıt / Güncelleme
        if (about is null)
        {
            _context.Abouts.Add(model);
        }
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
        TempData["Success"] = "Hakkımda bilgileri başarıyla kaydedildi.";
        return RedirectToAction("Index");
    }
}