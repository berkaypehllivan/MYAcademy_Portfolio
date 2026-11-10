using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;

[Area("Admin")]
public class AboutController : Controller
{
    private readonly PortfolioContext _context;

    public AboutController(PortfolioContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var about = _context.Abouts.OrderBy(x => x.AboutId).FirstOrDefault() ?? new About();
        return View(about);
    }

    [HttpPost]
    public async Task<IActionResult> Index(About model)
    {
        var about = _context.Abouts.OrderBy(x => x.AboutId).FirstOrDefault();

        if (about is null)
            _context.Abouts.Add(model);
        else
        {
            about.FirstLastName = model.FirstLastName;
            about.JobTitle = model.JobTitle;
            about.Expertise = model.Expertise;
            about.Description = model.Description;
            about.Email = model.Email;
            about.Phone = model.Phone;
            about.Location = model.Location;
            about.ImageUrl = model.ImageUrl;
            about.CvUrl = model.CvUrl;
        }

        await _context.SaveChangesAsync();
        TempData["Success"] = "Hakkımda Bilgileri Kaydedildi.";
        return RedirectToAction("Index");
    }
}