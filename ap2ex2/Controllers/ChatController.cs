using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ap2ex2.Controllers
{
    public class ChatController : Controller
    {
        // GET: ChatController
        public ActionResult Index()
        {
            return View();
        }
    }
}
