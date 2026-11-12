using Microsoft.AspNetCore.Mvc;
using MYAcademy_Portfolio.Context;

namespace MYAcademy_Portfolio.Areas.Admin.Controllers;

[Area("Admin")]
public class MessageController : BaseController
{
    public MessageController(PortfolioContext context, IWebHostEnvironment env) : base(context, env)
    {
    }

    public IActionResult Index()
    {
        var values = _context.Messages
            .OrderBy(x => x.IsRead)
            .ThenByDescending(x => x.SendDate)
            .ToList();
        return View(values);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var message = await _context.Messages.FindAsync(id);
        if (message is null) return NotFound();

        if (!message.IsRead)
        {
            message.IsRead = true;
            await _context.SaveChangesAsync();
        }
        return Ok();
    }

    public IActionResult Delete(int id)
    {
        var value = _context.Messages.Find(id);

        if (value is not null)
        {
            _context.Messages.Remove(value);
            _context.SaveChanges();
            TempData["Success"] = "Mesaj Başarıyla Silindi.";
        }
        return RedirectToAction("Index");
    }

}
