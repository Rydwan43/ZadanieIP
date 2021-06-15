using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiuroKomunikacja.Data;
using BiuroKomunikacja.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using BiuroKomunikacja.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;

namespace BiuroKomunikacja.Controllers
{
    [Authorize]
    public class DocsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;
        
        public DocsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Docs
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var docs = _context.DocsModel
                .Where(x => user.Id == x.ClientId);

            var relatedUsers = _userManager.Users.Where(x => x.RelatedUserId == user.Id);
            foreach (var item in relatedUsers)
            {
                docs = docs.Union(_context.DocsModel.Where(x => x.ClientId == item.Id));
            }

            if (user.IsAdmin)
            {
                docs = _context.DocsModel;
            }

            return View(docs);
        }

        // GET: Docs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docsModel = await _context.DocsModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docsModel == null)
            {
                return NotFound();
            }

            var wc = new System.Net.WebClient();
            wc.DownloadFile(docsModel.DocumentUrl, docsModel.DocumentUrl);

            return View(docsModel);
        }

        // GET: Docs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Docs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> files)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            DocsModel docsModel = new DocsModel { ClientId = user.Id};

            docsModel.Id = Guid.NewGuid();

            var size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Docs", formFile.FileName);
                    filePaths.Add(filePath);
                    docsModel.Name = formFile.FileName + docsModel.Id.ToString();
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            docsModel.DocumentUrl = filePaths.FirstOrDefault();

            _context.Add(docsModel);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));

        }

        // GET: Docs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docsModel = await _context.DocsModel.FindAsync(id);
            if (docsModel == null)
            {
                return NotFound();
            }
            return View(docsModel);
        }

        // POST: Docs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DocumentUrl,ClientId,EmployeeId")] DocsModel docsModel)
        {
            if (id != docsModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(docsModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocsModelExists(docsModel.Id))
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
            return View(docsModel);
        }

        // GET: Docs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var docsModel = await _context.DocsModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (docsModel == null)
            {
                return NotFound();
            }

            return View(docsModel);
        }

        // POST: Docs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var docsModel = await _context.DocsModel.FindAsync(id);
            _context.DocsModel.Remove(docsModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocsModelExists(Guid id)
        {
            return _context.DocsModel.Any(e => e.Id == id);
        }
    }
}
