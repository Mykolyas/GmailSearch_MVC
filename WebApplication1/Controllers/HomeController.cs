using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login()
    {
        var redirectUrl = Url.Action("Index", "Gmail");
        return Redirect(redirectUrl);
    }

    public IActionResult Error()
    {
        return View();
    }
}
