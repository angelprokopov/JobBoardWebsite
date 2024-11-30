using JobBoard.Data;
using JobBoard.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Controllers
{
    public class DocumentController : Controller
    {
        private readonly JobBoardContext _context;
        private readonly IWebHostEnvironment _environment;

        public DocumentController(JobBoardContext context, IWebHostEnvironment env)
        {
            _context = context;
            _environment = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = Guid.Parse(User.FindFirst("UserId")?.Value);
            var documents = await _context.Documents.Where(d => d.UserId == userId).ToListAsync();
            
            return View(documents);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Не е избран файл.");
            }
            var path = Path.Combine(_environment.WebRootPath, "", file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var document = new Document
            {
                FileName = file.FileName,
                FilePath = "/uploads/" + file.FileName,
                UserId = Guid.Parse(User.FindFirst("UserId")?.Value),   
                DateUploaded = DateTime.Now,
            };

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var document = await _context.Documents.FindAsync(id);
            if (document == null || document.UserId != Guid.Parse(User.FindFirst("UserId")?.Value))
            {
                return View("Error404");
            }

            var filePath = Path.Combine(_environment.WebRootPath,document.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(filePath)) 
                System.IO.File.Delete(filePath);

            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();
           
            return RedirectToAction(nameof(Index));
        }
    }
}
