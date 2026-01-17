using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsBox.Areas.AdminPanel.ViewModels;
using NewsBox.Data;
using NewsBox.Models;
using System.Threading.Tasks;

namespace NewsBox.Areas.AdminPanel.Controllers
{
    [Authorize(Roles = "SuperAdmin, Admin")]
    [Area("AdminPanel")]
    public class InfoController : Controller
    {
        private NewsDbContext _context { get; }
        public InfoController(NewsDbContext context)
        {
            _context=context;
        }
        public IActionResult Index()
        {
            return View(_context.infos.Where(c => !c.IsDeleted));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Info info = new Info
            {
                IsDeleted=false,
                Title=vm.title,
                Author=vm.author
            };
            if (info == null) return NotFound();
            if (vm.imageFile !=null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.imageFile.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.imageFile.CopyToAsync(stream);
                }
                info.ImageURL = fileName;
            }
            _context.Add(info);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            Info? info = _context.infos.Where(c => !c.IsDeleted).FirstOrDefault(i => i.Id==id);
            if (info == null) return NotFound();
            info.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int? id)
        {
            if (id == null) return BadRequest();
            Info? info = _context.infos.Where(c => !c.IsDeleted).FirstOrDefault(i => i.Id==id);
            if (info == null) return NotFound();
            UpdateVM vm = new UpdateVM
            {
                title=info.Title,
                author=info.Author,
                id_=info.Id
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateVM vm)
        {
            if (!ModelState.IsValid) return View(vm);
            Info? info = _context.infos.Where(c => !c.IsDeleted).FirstOrDefault(i => i.Id==vm.id_);
            if (info == null) return NotFound();

            info.Title=vm.title;
            info.Author=vm.author;
            if (vm.imageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.imageFile.FileName);
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await vm.imageFile.CopyToAsync(stream);
                }
                info.ImageURL = fileName;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
