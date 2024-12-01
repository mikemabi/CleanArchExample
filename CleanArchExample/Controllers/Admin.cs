using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchExample.Controllers
{
    public class Admin : Controller
    {
        [Authorize(Roles = "Admin, SuperAdmin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
