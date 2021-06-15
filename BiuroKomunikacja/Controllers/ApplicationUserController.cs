using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiuroKomunikacja.Areas.Identity;
using BiuroKomunikacja.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BiuroKomunikacja.Controllers
{
    [Authorize]
    public class ApplicationUserController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: ApplicationUser
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.IsAdmin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var applicationDbContext = _context.Users.Include(a => a.RelatedUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ApplicationUser/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.IsAdmin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
                .Include(a => a.RelatedUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // GET: ApplicationUser/Create
        public IActionResult Create()
        {
            
            ViewData["RelatedUserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: ApplicationUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,IsAdmin,IsEmployee,RelatedUserId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.IsAdmin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (ModelState.IsValid)
            {
                _context.Add(applicationUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RelatedUserId"] = new SelectList(_context.Users, "Id", "UserName", applicationUser.RelatedUserId);
            return View(applicationUser);
        }

        // GET: ApplicationUser/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.IsAdmin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }
            ViewData["RelatedUserId"] = new SelectList(_context.Users, "Id", "UserName", applicationUser.RelatedUserId);
            return View(applicationUser);
        }

        // POST: ApplicationUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("FullName,IsAdmin,IsEmployee,RelatedUserId,Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount")] ApplicationUser applicationUser)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.IsAdmin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (id != applicationUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicationUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicationUserExists(applicationUser.Id))
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
            ViewData["RelatedUserId"] = new SelectList(_context.Users, "Id", "UserName", applicationUser.RelatedUserId);
            return View(applicationUser);
        }

        // GET: ApplicationUser/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.IsAdmin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            if (id == null)
            {
                return NotFound();
            }

            var applicationUser = await _context.Users
                .Include(a => a.RelatedUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            return View(applicationUser);
        }

        // POST: ApplicationUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.IsAdmin)
            {
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }

            var applicationUser = await _context.Users.FindAsync(id);
            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
