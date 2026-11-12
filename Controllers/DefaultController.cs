using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MYAcademy_Portfolio.Context;
using MYAcademy_Portfolio.Entities;
using MYAcademy_Portfolio.Models;

namespace MYAcademy_Portfolio.Controllers;

public class DefaultController : Controller
{
    private readonly PortfolioContext _context;

    public DefaultController(PortfolioContext context)
    {
        _context = context;
    }

    [Route("")]
    [Route("Default")]
    [Route("Default/Index")]
    [Route("Default/{section}")]
    [Route("{section}")]
    public IActionResult Index(string section)
    {
        var projects = _context.Projects
            .Include(x => x.Category)
            .ToList();

        ViewBag.ActiveSection = section?.ToLower();
        return View(projects);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendMessage(ContactFormDto dto)
    {
        if (!ModelState.IsValid) return BadRequest();

        var message = new Message
        {
            NameSurname = dto.NameSurname,
            Email = dto.Email,
            Subject = dto.Subject,
            MessageDetail = dto.Message,
            SendDate = DateTime.Now,
            IsRead = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
