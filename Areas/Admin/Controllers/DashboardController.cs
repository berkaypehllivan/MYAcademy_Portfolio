using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;
using System.Globalization;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]

public class DashboardController : BaseController
{
    public DashboardController(PortfolioContext context, IWebHostEnvironment env) : base(context, env)
    {
    }

    public async Task<IActionResult> Index()
    {
        var tr = new CultureInfo("tr-TR");

        ViewBag.projectCount = await _context.Projects.CountAsync();
        ViewBag.categoryCount = await _context.Categories.CountAsync();
        ViewBag.skillCount = await _context.Skills.CountAsync();
        ViewBag.experienceCount = await _context.Experiences.CountAsync();
        ViewBag.educationCount = await _context.Educations.CountAsync();

        ViewBag.messageCount = await _context.Messages.CountAsync();
        ViewBag.messageReadByTrueCount = await _context.Messages.CountAsync(m => m.IsRead);
        ViewBag.messageReadByFalseCount = await _context.Messages.CountAsync(m => !m.IsRead);

        ViewBag.projectsByCategory = await _context.Categories
            .Select(c => new { c.CategoryName, Count = c.Projects.Count })
            .ToDictionaryAsync(x => x.CategoryName, x => x.Count);

        ViewBag.projectDemoCount = await _context.Projects.CountAsync(p => p.LiveDemoUrl != null && p.LiveDemoUrl != "");
        ViewBag.projectKeywordCount = await _context.Projects.CountAsync(p => p.Keywords != null && p.Keywords != "");

        // Son 6 ay mesaj
        var since = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-5);
        var dates = await _context.Messages.Where(m => m.SendDate >= since).Select(m => m.SendDate).ToListAsync();
        var monthly = new Dictionary<string, int>();
        for (int i = 0; i < 6; i++)
        {
            var d = since.AddMonths(i);
            monthly[d.ToString("MMM", tr)] = dates.Count(x => x.Year == d.Year && x.Month == d.Month);
        }
        ViewBag.messagesByMonth = monthly;

        ViewBag.activeExperienceCount = await _context.Experiences.CountAsync(e => e.IsActive);
        ViewBag.workTypes = await _context.Experiences
            .GroupBy(e => e.WorkType)
            .Select(g => new { g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Key, x => x.Count);

        var skills = await _context.Skills.ToListAsync();
        int KeywordCount(Skill s) => (s.Keywords ?? "")
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length;

        ViewBag.keywordTotal = skills.Sum(KeywordCount);
        ViewBag.topSkills = skills
            .OrderByDescending(KeywordCount)
            .Take(6)
            .GroupBy(s => s.Title)
            .ToDictionary(g => g.Key, g => g.Max(KeywordCount));

        ViewBag.experiences = (await _context.Experiences.OrderByDescending(e => e.ExperienceId).Take(4).ToListAsync())
            .Select(e => (e.Title, $"{e.CompanyName} · {e.WorkType}", e.StartDate))
            .ToList();
        ViewBag.educations = (await _context.Educations.OrderByDescending(e => e.EducationId).Take(4).ToListAsync())
            .Select(e => (e.Title, e.SubTitle ?? e.Location ?? "", e.Duration))
            .ToList();

        ViewBag.recentMessages = (await _context.Messages.OrderByDescending(m => m.SendDate).Take(5).ToListAsync())
            .Select(m => (m.NameSurname, m.Email, m.Subject, m.SendDate.ToString("dd.MM.yyyy"), m.IsRead))
            .ToList();

        var about = await _context.Abouts.FirstOrDefaultAsync();
        var contact = await _context.Contacts.FirstOrDefaultAsync();
        var aboutFields = new[] { about?.FirstLastName, about?.Description, about?.ImageUrl };
        var contactFields = new[] { contact?.Email, contact?.Phone, contact?.Location };
        ViewBag.aboutFilled = aboutFields.Count(f => !string.IsNullOrWhiteSpace(f));
        ViewBag.aboutTotal = aboutFields.Length;
        ViewBag.contactFilled = contactFields.Count(f => !string.IsNullOrWhiteSpace(f));
        ViewBag.contactTotal = contactFields.Length;

        return View();
    }
}
