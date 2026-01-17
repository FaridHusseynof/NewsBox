using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsBox.Data;
using NewsBox.Models;

namespace NewsBox.Controllers
{
    public class HomeController : Controller
    {
        private NewsDbContext _context { get; }
        public HomeController(NewsDbContext context)
        {
         _context=context;   
        }
        public IActionResult Index()
        {
            return View(_context.infos.Where(c=>!c.IsDeleted));
        }

       
    }
}
