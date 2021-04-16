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
    public class GroupusController : Controller
    {
        private readonly voterCollectorContext _context;

        public GroupusController(voterCollectorContext context)
        {
            _context = context;
        }

        // GET: Groupus
        public async Task<IActionResult> Index()
        {
            return View(await _context.Groupu.ToListAsync());
        }

        // GET: Groupus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupu = await _context.Groupu
                .FirstOrDefaultAsync(m => m.IdGroup == id);
            if (groupu == null)
            {
                return NotFound();
            }

            return View(groupu);
        }

        // GET: Groupus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groupus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGroup,Name")] Groupu groupu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(groupu);
        }

        // GET: Groupus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupu = await _context.Groupu.FindAsync(id);
            if (groupu == null)
            {
                return NotFound();
            }
            return View(groupu);
        }

        // POST: Groupus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGroup,Name")] Groupu groupu)
        {
            if (id != groupu.IdGroup)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupuExists(groupu.IdGroup))
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
            return View(groupu);
        }

        // GET: Groupus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupu = await _context.Groupu
                .FirstOrDefaultAsync(m => m.IdGroup == id);
            if (groupu == null)
            {
                return NotFound();
            }

            return View(groupu);
        }

        // POST: Groupus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupu = await _context.Groupu.FindAsync(id);
            _context.Groupu.Remove(groupu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupuExists(int id)
        {
            return _context.Groupu.Any(e => e.IdGroup == id);
        }
    }
}
