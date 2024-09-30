using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CedrosNahuizalquenos.Models;

namespace CedrosNahuizalquenos.Controllers
{
    public class PersonalizacionesController : Controller
    {
        private readonly CedrosNahuiContext _context;

        public PersonalizacionesController(CedrosNahuiContext context)
        {
            _context = context;
        }

        // GET: Personalizaciones
        public async Task<IActionResult> Index()
        {
            var cedrosNahuiContext = _context.Personalizaciones.Include(p => p.Producto);
            return View(await cedrosNahuiContext.ToListAsync());
        }

        // GET: Personalizaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalizacione = await _context.Personalizaciones
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.PersonalizacionId == id);
            if (personalizacione == null)
            {
                return NotFound();
            }

            return View(personalizacione);
        }

        // GET: Personalizaciones/Create
        public IActionResult Create()
        {
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId");
            return View();
        }

        // POST: Personalizaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PersonalizacionId,ProductoId,OpcionPersonalizacion,CostosExtras")] Personalizacione personalizacione)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personalizacione);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", personalizacione.ProductoId);
            return View(personalizacione);
        }

        // GET: Personalizaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalizacione = await _context.Personalizaciones.FindAsync(id);
            if (personalizacione == null)
            {
                return NotFound();
            }
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", personalizacione.ProductoId);
            return View(personalizacione);
        }

        // POST: Personalizaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PersonalizacionId,ProductoId,OpcionPersonalizacion,CostosExtras")] Personalizacione personalizacione)
        {
            if (id != personalizacione.PersonalizacionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personalizacione);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonalizacioneExists(personalizacione.PersonalizacionId))
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
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", personalizacione.ProductoId);
            return View(personalizacione);
        }

        // GET: Personalizaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personalizacione = await _context.Personalizaciones
                .Include(p => p.Producto)
                .FirstOrDefaultAsync(m => m.PersonalizacionId == id);
            if (personalizacione == null)
            {
                return NotFound();
            }

            return View(personalizacione);
        }

        // POST: Personalizaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personalizacione = await _context.Personalizaciones.FindAsync(id);
            if (personalizacione != null)
            {
                _context.Personalizaciones.Remove(personalizacione);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonalizacioneExists(int id)
        {
            return _context.Personalizaciones.Any(e => e.PersonalizacionId == id);
        }
    }
}
