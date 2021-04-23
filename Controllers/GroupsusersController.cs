using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using voteCollector.Data;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    [Authorize(Roles = "admin")]
    public class GroupsusersController : Controller
    {
        private readonly VoterCollectorContext _context;

        public GroupsusersController(VoterCollectorContext context)
        {
            _context = context;
        }

        // GET: Groupsusers
        public async Task<IActionResult> Index()
        {
            var voterCollectorContext = _context.Groupsusers.Include(g => g.GroupU).Include(g => g.User);
            return View(await voterCollectorContext.ToListAsync());
        }

        // GET: Groupsusers/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupsusers = await _context.Groupsusers
                .Include(g => g.GroupU)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.IdGroupsUsers == id);
            if (groupsusers == null)
            {
                return NotFound();
            }

            return View(groupsusers);
        }

        // GET: Groupsusers/Create
        public IActionResult Create()
        {
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name");
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "UserName");
            return View();
        }

        // POST: Groupsusers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGroupsUsers,GroupUId,Name,UserId")] Groupsusers groupsusers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupsusers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name", groupsusers.GroupUId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "UserName", groupsusers.UserId);
            return View(groupsusers);
        }

        // GET: Groupsusers/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupsusers = await _context.Groupsusers.FindAsync(id);
            if (groupsusers == null)
            {
                return NotFound();
            }
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name", groupsusers.GroupUId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "UserName", groupsusers.UserId);
            return View(groupsusers);
        }

        // POST: Groupsusers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("IdGroupsUsers,GroupUId,Name,UserId")] Groupsusers groupsusers)
        {
            if (id != groupsusers.IdGroupsUsers)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupsusers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupsusersExists(groupsusers.IdGroupsUsers))
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
            ViewData["GroupUId"] = new SelectList(_context.Groupu, "IdGroup", "Name", groupsusers.GroupUId);
            ViewData["UserId"] = new SelectList(_context.User, "IdUser", "UserName", groupsusers.UserId);
            return View(groupsusers);
        }

        // GET: Groupsusers/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupsusers = await _context.Groupsusers
                .Include(g => g.GroupU)
                .Include(g => g.User)
                .FirstOrDefaultAsync(m => m.IdGroupsUsers == id);
            if (groupsusers == null)
            {
                return NotFound();
            }

            return View(groupsusers);
        }

        // POST: Groupsusers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var groupsusers = await _context.Groupsusers.FindAsync(id);
            _context.Groupsusers.Remove(groupsusers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupsusersExists(long id)
        {
            return _context.Groupsusers.Any(e => e.IdGroupsUsers == id);
        }
    }
}
