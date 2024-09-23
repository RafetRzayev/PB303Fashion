using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PB303Fashion.DataAccessLayer;

namespace PB303Fashion.Areas.AdminPanel.Controllers
{
    public class CategoryController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public CategoryController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.Categories.ToListAsync();

            return View(categories);
        }
    }
}
