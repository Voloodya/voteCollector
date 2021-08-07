using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using voteCollector.Data;
using voteCollector.Models;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly VoterCollectorContext _context;
        private ServiceUser _serviceUser;

        public UsersController(VoterCollectorContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
            _serviceUser = new ServiceUser(context);
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                var voterCollectorContext = _context.User.Include(u => u.Role);
                return View(await voterCollectorContext.ToListAsync());

            }
            else
            {
                List<User> voterCollectorContext = await _context.User.Include(u => u.Role).ToListAsync();
                List<User> voterCollector = voterCollectorContext.Where(u => groupsUser.Intersect(_serviceUser.GetGroupsUser(u.UserName)).Count() != 0).ToList();
                return View(voterCollector);
            }
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Role, "IdRole", "Name");
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUser,UserName,Password,RoleId,FamilyName,Name,PatronymicName,DateBirth,Telephone")] User user)
        {

            if (ModelState.IsValid)
            {

                User userDB = _context.User.FirstOrDefault(u => u.UserName.Equals(user.UserName));

                if (userDB == null)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь с данным логином уже существует!");
                }
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "IdRole", "Name", user.RoleId);
            return View(user);
        }



        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "IdRole", "Name", user.RoleId);
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdUser,UserName,Password,RoleId,FamilyName,Name,PatronymicName,DateBirth,Telephone")] User user)
        {
            if (id != user.IdUser)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                User userDB = _context.User.FirstOrDefault(u => u.UserName.Equals(user.UserName) && u.IdUser != id);
                if (userDB == null)
                {
                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.IdUser))
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
                else ModelState.AddModelError("", "Пользователь с данным логином уже существует!");
            }
            ViewData["RoleId"] = new SelectList(_context.Role, "IdRole", "Name", user.RoleId);
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.IdUser == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(long id)
        {
            return _context.User.Any(e => e.IdUser == id);
        }

        private List<Groupu> GetGroupsUser(string userName)
        {
            User userSave = _context.User.Where(u => u.UserName.Equals(userName)).FirstOrDefault();
            List<Groupsusers> groupsUsers = userSave.Groupsusers.ToList();
            return groupsUsers.Select(g => g.GroupU).ToList();
        }

        [HttpGet]
        public async Task<IActionResult> RedirectTo()
        {
            return RedirectToAction("Index", "Admin");
        }

    }
}
