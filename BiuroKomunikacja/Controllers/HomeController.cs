using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BiuroKomunikacja.Models;
using Microsoft.AspNetCore.Identity;
using BiuroKomunikacja.Areas.Identity;
using Microsoft.AspNetCore.Authorization;
using BiuroKomunikacja.Data;
using Microsoft.EntityFrameworkCore;

namespace BiuroKomunikacja.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user.IsAdmin)
            {
                return RedirectToAction(nameof(IndexAdmin));
            }

            if (user.IsEmployee)
            {
                return RedirectToAction(nameof(IndexEmployee));
            }

            user.RelatedUser = await _userManager.FindByIdAsync(user.RelatedUserId);

            return View(user);
        }

        public async Task<IActionResult> IndexAdmin()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.IsAdmin)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(user);
        }

        public async Task<IActionResult> IndexEmployee()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (!user.IsEmployee)
            {
                return RedirectToAction(nameof(Index));
            }

            var relatedUsers = _userManager.Users
                .Include(a => a.RelatedUser)
                .Where(x => x.RelatedUserId == user.Id);


            return View(relatedUsers);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
