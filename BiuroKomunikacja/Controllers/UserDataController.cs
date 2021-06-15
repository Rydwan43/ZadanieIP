using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiuroKomunikacja.Data;
using BiuroKomunikacja.Models;
using Microsoft.AspNetCore.Authorization;
using BiuroKomunikacja.Areas.Identity;
using Microsoft.AspNetCore.Identity;

namespace BiuroKomunikacja.Controllers
{
    [Authorize]
    public class UserDataController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserDataController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserData
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var applicationDbContext = _context.UserDataModel
                .Include(u => u.User)
                .Where(x => x.UserId == user.Id);

            if (user.IsEmployee)
            {
                applicationDbContext = _context.UserDataModel
                .Include(u => u.User)
                .Where(x => x.User.RelatedUserId == user.Id);
            }

            if (user.IsAdmin)
            {
                applicationDbContext = _context.UserDataModel
                    .Include(u => u.User);
            }
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserData/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDataModel = await _context.UserDataModel
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDataModel == null)
            {
                return NotFound();
            }

            return View(userDataModel);
        }

        // GET: UserData/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");

            return View();
        }

        // POST: UserData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dane,UserId,CzasTransakcji,Kwota")] UserDataModel userDataModel)
        {
            if (ModelState.IsValid)
            {
                userDataModel.Id = Guid.NewGuid();
                _context.Add(userDataModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userDataModel.UserId);
            return View(userDataModel);
        }

        // GET: UserData/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDataModel = await _context.UserDataModel.FindAsync(id);
            if (userDataModel == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userDataModel.UserId);
            return View(userDataModel);
        }

        // POST: UserData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Dane,UserId,CzasTransakcji,Kwota")] UserDataModel userDataModel)
        {
            if (id != userDataModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDataModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDataModelExists(userDataModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName", userDataModel.UserId);
            return View(userDataModel);
        }

        // GET: UserData/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDataModel = await _context.UserDataModel
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userDataModel == null)
            {
                return NotFound();
            }

            return View(userDataModel);
        }

        // POST: UserData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var userDataModel = await _context.UserDataModel.FindAsync(id);
            _context.UserDataModel.Remove(userDataModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDataModelExists(Guid id)
        {
            return _context.UserDataModel.Any(e => e.Id == id);
        }
    }
}
