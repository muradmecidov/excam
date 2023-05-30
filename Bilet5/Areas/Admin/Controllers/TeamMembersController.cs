using Bilet5.Areas.Admin.ViewModels;
using Bilet5.DAL;
using Bilet5.Models;
using Bilet5.Utlities.Contants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Bilet5.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class TeamMembersController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public TeamMembersController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			return View(await _context.TeamMembers.OrderByDescending(p => p.Id).ToListAsync());
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(CreateTeamMembersVM create)
		{
			if (!ModelState.IsValid) { return View(create); }
			if (!create.Photo.ContentType.Contains("image/"))
			{
				ModelState.AddModelError("Photo", ErrorMessage.FMBTI);
			}
			if (create.Photo.Length / 1024 > 200)
			{
                ModelState.AddModelError("Photo", ErrorMessage.FMBSI);

            }
			string rootpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team");
			string filename = Guid.NewGuid().ToString() + create.Photo.FileName;
			using (FileStream fileStream = new FileStream(Path.Combine(rootpath, filename), FileMode.Create))
			{
				await create.Photo.CopyToAsync(fileStream);
			};
			TeamMember team = new TeamMember()
			{
				Fullname = create.Fullname,
				Information = create.Information,
				JobDescription = create.JobDescription,
				ImagePath = filename
			};

			await _context.TeamMembers.AddAsync(team);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index)); 



        }


		public async Task<IActionResult> Update(int id)
		{
            TeamMember team = await _context.TeamMembers.FindAsync(id);
            if (team == null) { return NotFound(); };
			UpdateTeamMembersVM vm = new UpdateTeamMembersVM()
			{
				Fullname= team.Fullname,
				Information = team.Information,
				JobDescription = team.JobDescription,
				Id = id
			};
			return View(vm);
           
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(UpdateTeamMembersVM update)
		{
            if (!ModelState.IsValid) { return View(update); }
            if (!update.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", ErrorMessage.FMBTI);
            }
            if (update.Photo.Length / 1024 > 200)
            {
                ModelState.AddModelError("Photo", ErrorMessage.FMBSI);
            }
            string rootpath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team");
			string oldfilename = (await _context.TeamMembers.FindAsync(update.Id))?.ImagePath;
			string filepath = Path.Combine(rootpath, oldfilename);

            if (System.IO.File.Exists(rootpath))
            {
                System.IO.File.Delete(rootpath);
            }

            string filename = Guid.NewGuid().ToString() + update.Photo.FileName;

            using (FileStream fileStream = new FileStream(Path.Combine(rootpath, filename), FileMode.Create))
            {
                await update.Photo.CopyToAsync(fileStream);
            }

            TeamMember teamMember = await _context.TeamMembers.FindAsync(update.Id);

            if (teamMember == null)
            {
                return NotFound();
            }

            teamMember.Fullname = update.Fullname;
            teamMember.Information = update.Information;
            teamMember.JobDescription = update.JobDescription;
            teamMember.ImagePath = filename;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Delete(int id)
		{ 
			TeamMember team = await _context.TeamMembers.FindAsync(id);
			if (team == null) { return NotFound(); }
			string filepath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", team.ImagePath);
			if (System.IO.File.Exists(filepath))
			{
				System.IO.File.Delete(filepath);
			}

			_context.TeamMembers.Remove(team);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));	

        }










    }
}
