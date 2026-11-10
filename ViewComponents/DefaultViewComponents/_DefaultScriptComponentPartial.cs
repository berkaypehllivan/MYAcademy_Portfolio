using Microsoft.AspNetCore.Mvc;

namespace MYAcademy_Portfolio.ViewComponents.DefaultViewComponents;

public class _DefaultScriptComponentPartial : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        return View();
    }
}
