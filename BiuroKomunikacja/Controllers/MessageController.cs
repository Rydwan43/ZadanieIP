using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BiuroKomunikacja.Data;
using BiuroKomunikacja.Models;
using BiuroKomunikacja.Areas.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace BiuroKomunikacja.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessageController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Message
        public IActionResult Index()
        {

            return RedirectToAction(nameof(Recived));
        }

        public async Task<IActionResult> Recived()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var messages = _context.MessageModel
                .Where(x => x.ReceiverId == user.Id)
                .ToList();

            foreach (var item in messages)
            {
                item.sender = await _userManager.FindByIdAsync(item.SenderId);
            }

            return View(messages);
        }

        public async Task<IActionResult> Sent()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var messages = _context.MessageModel
                .Where(x => x.SenderId == user.Id)
                .ToList();

            foreach (var item in messages)
            {
                item.reciver = await _userManager.FindByIdAsync(item.ReceiverId);
            }

            return View(messages);
        }

        // GET: Message/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageModel = await _context.MessageModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageModel == null)
            {
                return NotFound();
            }

            messageModel.sender = await _userManager.FindByIdAsync(messageModel.SenderId);
            messageModel.reciver = await _userManager.FindByIdAsync(messageModel.ReceiverId);

            return View(messageModel);
        }

        // GET: Message/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Message/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SenderId,ReceiverId,Message,MessageDate")] MessageModel messageModel)
        {
            var reciver = _userManager.Users.FirstOrDefault(x => x.UserName.Contains(messageModel.ReceiverId));

            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (ModelState.IsValid && reciver is not null)
            {
                messageModel.Id = Guid.NewGuid();
                messageModel.SenderId = user.Id;
                messageModel.ReceiverId = reciver.Id.ToString();
                messageModel.MessageDate = DateTime.UtcNow;
                _context.Add(messageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(messageModel);
        }

        // GET: Message/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messageModel = await _context.MessageModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messageModel == null)
            {
                return NotFound();
            }

            return View(messageModel);
        }

        // POST: Message/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var messageModel = await _context.MessageModel.FindAsync(id);
            _context.MessageModel.Remove(messageModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageModelExists(Guid id)
        {
            return _context.MessageModel.Any(e => e.Id == id);
        }
    }
}
