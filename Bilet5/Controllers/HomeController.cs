using Bilet5.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Bilet5.Controllers
{
    public class HomeController : Controller
    {
       private readonly AppDbContext  _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
           
            return View(await _context.TeamMembers.Where(p => !p.IsDelected).OrderByDescending(p => p.Id).ToListAsync());
        }

   
    }
}