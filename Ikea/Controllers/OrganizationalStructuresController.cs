using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ikea.Models;

namespace Ikea.Controllers
{
    public class OrganizationalStructuresController : Controller
    {
        private readonly ApplicationContext _context;

        public OrganizationalStructuresController(ApplicationContext context)
        {
            _context = context;
        }

        // GET: OrganizationalStructures
        public async Task<IActionResult> Index()
        {
            return View(await _context.OrganizationalStructures.ToListAsync());
        }

        // GET: OrganizationalStructures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationalStructure = await _context.OrganizationalStructures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organizationalStructure == null)
            {
                return NotFound();
            }

            return View(organizationalStructure);
        }

        // GET: OrganizationalStructures/Create
        public IActionResult Create()
        {
            ViewData["Department"] = new SelectList(_context.OrganizationalStructures.Where(p => p.BusinessUnit != null), "Id", "Name");
            ViewData["BusinessUnit"] = new SelectList(_context.OrganizationalStructures.Where(p => p.Department == null && p.BusinessUnit == null), "Id", "Name");
            return View();
        }

        // POST: OrganizationalStructures/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Department,BusinessUnit")] OrganizationalStructure organizationalStructure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(organizationalStructure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(organizationalStructure);
        }

        // GET: OrganizationalStructures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Department"] = new SelectList(_context.OrganizationalStructures.Where(p => p.BusinessUnit != null), "Id", "Name");
            ViewData["BusinessUnit"] = new SelectList(_context.OrganizationalStructures.Where(p => p.Department == null && p.BusinessUnit == null), "Id", "Name");
            var organizationalStructure = await _context.OrganizationalStructures.FindAsync(id);
            if (organizationalStructure == null)
            {
                return NotFound();
            }
            return View(organizationalStructure);
        }

        // POST: OrganizationalStructures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Department,BusinessUnit")] OrganizationalStructure organizationalStructure)
        {
            if (id != organizationalStructure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(organizationalStructure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrganizationalStructureExists(organizationalStructure.Id))
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
            return View(organizationalStructure);
        }

        // GET: OrganizationalStructures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var organizationalStructure = await _context.OrganizationalStructures
                .FirstOrDefaultAsync(m => m.Id == id);
            if (organizationalStructure == null)
            {
                return NotFound();
            }

            return View(organizationalStructure);
        }

        // POST: OrganizationalStructures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var organizationalStructure = await _context.OrganizationalStructures.FindAsync(id);
            _context.OrganizationalStructures.Remove(organizationalStructure);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrganizationalStructureExists(int id)
        {
            return _context.OrganizationalStructures.Any(e => e.Id == id);
        }
    }
}
