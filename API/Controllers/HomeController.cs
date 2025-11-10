using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Controller para a página inicial da aplicação MVC
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Página inicial
        /// </summary>
        [HttpGet("/")]
        [HttpGet("/Home")]
        [HttpGet("/Home/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
