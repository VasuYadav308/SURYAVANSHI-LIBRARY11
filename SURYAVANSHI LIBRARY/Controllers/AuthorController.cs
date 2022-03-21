using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SURYAVANSHI_LIBRARY.Data;
using System.Threading.Tasks;

namespace SURYAVANSHI_LIBRARY.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthorController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var students = await _dbContext.Authors.ToListAsync();

            return View(students);
        }
    }
}
