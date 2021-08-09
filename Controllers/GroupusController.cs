﻿using System;
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
    public class GroupusController : Controller
    {
        private readonly ILogger<GroupusController> _logger;
        private readonly VoterCollectorContext _context;
        private ServiceUser _serviceUser;

        public GroupusController(VoterCollectorContext context, ILogger<GroupusController> logger)
        {
            _context = context;
            _logger = logger;
            _serviceUser = new ServiceUser(context);
        }

        // GET: Groupus
        public async Task<IActionResult> Index()
        {
            List<Groupu> groupsUser = _serviceUser.GetGroupsUser(User.Identity.Name);
            Groupu mainGroup = _context.Groupu.Where(g => g.Name.Equals("Main")).FirstOrDefault();

            if (groupsUser.Contains(mainGroup))
            {
                return View(await _context.Groupu.Include(g => g.FieldActivity).Include(g => g.Organization).ToListAsync());
            }
            else
            {
                return View(await _context.Groupu.Include(g => g.FieldActivity).Include(g => g.Organization).Where(g => groupsUser.Contains(g)).ToListAsync());                
            }            
        }

        // GET: Groupus/Create
        public IActionResult Create()
        {

            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name");
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name");
            return View();
        }

        // POST: Groupus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGroup,Name,FieldActivityId,OrganizationId")] Groupu groupu)
        {

            if (ModelState.IsValid)
            {
                Groupu groupuDB = _context.Groupu.FirstOrDefault(g => g.Name.Equals(groupu.Name));

                if (groupuDB == null)
                {
                    User currentUser = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                    groupu.CreatorGroup = currentUser.FamilyName + " " + currentUser.Name + " " + currentUser.PatronymicName;
                    _context.Add(groupu);
                    _context.SaveChanges();

                    Groupsusers groupsusers = new Groupsusers();
                    groupsusers.GroupUId = groupu.IdGroup;
                    groupsusers.UserId = currentUser.IdUser;
                    _context.Add(groupsusers);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                else ModelState.AddModelError("", "Группа с данным именем уже существует");
            }
            //
            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name");
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name");
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

            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name", groupu.Organization);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", groupu.FieldActivityId);
            return View(groupu);
        }

        // POST: Groupus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGroup,Name,FieldActivityId,OrganizationId")] Groupu groupu)
        {
            if (id != groupu.IdGroup)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Groupu groupuDB = _context.Groupu.FirstOrDefault(g => g.Name.Equals(groupu.Name) && g.IdGroup!=id);
                if (groupuDB == null)
                {
                    try
                    {
                        User currentUser = _context.User.Where(u => u.UserName.Equals(User.Identity.Name)).FirstOrDefault();
                        groupu.CreatorGroup = currentUser.FamilyName + " " + currentUser.Name + " " + currentUser.PatronymicName;

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
                else ModelState.AddModelError("", "Группа с данным именем уже существует");
            }

            ViewData["OrganizationId"] = new SelectList(_context.Organization, "IdOrganization", "Name", groupu.Organization);
            ViewData["FieldActivityId"] = new SelectList(_context.Fieldactivity, "IdFieldActivity", "Name", groupu.FieldActivityId);
            return View(groupu);
        }

        // GET: Groupus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupu = await _context.Groupu.Include(g => g.FieldActivity)
                .Include(g => g.Organization)
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

        [HttpGet]
        public async Task<IActionResult> RedirectTo()
        {
            return RedirectToAction("Index", "Admin");
        }
    }
}
