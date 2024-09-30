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
    public class DetallesPedidoesController : Controller
    {
        private readonly CedrosNahuiContext _context;

        public DetallesPedidoesController(CedrosNahuiContext context)
        {
            _context = context;
        }

        // GET: DetallesPedidoes
        public async Task<IActionResult> Index()
        {
            var cedrosNahuiContext = _context.DetallesPedidos.Include(d => d.Pedido).Include(d => d.Personalizacion).Include(d => d.Producto);
            return View(await cedrosNahuiContext.ToListAsync());
        }

        // GET: DetallesPedidoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallesPedido = await _context.DetallesPedidos
                .Include(d => d.Pedido)
                .Include(d => d.Personalizacion)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.DetallePedidoId == id);
            if (detallesPedido == null)
            {
                return NotFound();
            }

            return View(detallesPedido);
        }

        // GET: DetallesPedidoes/Create
        public IActionResult Create()
        {
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId");
            ViewData["PersonalizacionId"] = new SelectList(_context.Personalizaciones, "PersonalizacionId", "PersonalizacionId");
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId");
            return View();
        }

        // POST: DetallesPedidoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DetallePedidoId,PedidoId,ProductoId,Cantidad,PrecioUnitario,Subtotal,PersonalizacionId,Anticipo")] DetallesPedido detallesPedido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detallesPedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId", detallesPedido.PedidoId);
            ViewData["PersonalizacionId"] = new SelectList(_context.Personalizaciones, "PersonalizacionId", "PersonalizacionId", detallesPedido.PersonalizacionId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", detallesPedido.ProductoId);
            return View(detallesPedido);
        }

        // GET: DetallesPedidoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallesPedido = await _context.DetallesPedidos.FindAsync(id);
            if (detallesPedido == null)
            {
                return NotFound();
            }
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId", detallesPedido.PedidoId);
            ViewData["PersonalizacionId"] = new SelectList(_context.Personalizaciones, "PersonalizacionId", "PersonalizacionId", detallesPedido.PersonalizacionId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", detallesPedido.ProductoId);
            return View(detallesPedido);
        }

        // POST: DetallesPedidoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DetallePedidoId,PedidoId,ProductoId,Cantidad,PrecioUnitario,Subtotal,PersonalizacionId,Anticipo")] DetallesPedido detallesPedido)
        {
            if (id != detallesPedido.DetallePedidoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detallesPedido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetallesPedidoExists(detallesPedido.DetallePedidoId))
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
            ViewData["PedidoId"] = new SelectList(_context.Pedidos, "PedidoId", "PedidoId", detallesPedido.PedidoId);
            ViewData["PersonalizacionId"] = new SelectList(_context.Personalizaciones, "PersonalizacionId", "PersonalizacionId", detallesPedido.PersonalizacionId);
            ViewData["ProductoId"] = new SelectList(_context.Productos, "ProductoId", "ProductoId", detallesPedido.ProductoId);
            return View(detallesPedido);
        }

        // GET: DetallesPedidoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detallesPedido = await _context.DetallesPedidos
                .Include(d => d.Pedido)
                .Include(d => d.Personalizacion)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(m => m.DetallePedidoId == id);
            if (detallesPedido == null)
            {
                return NotFound();
            }

            return View(detallesPedido);
        }

        // POST: DetallesPedidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var detallesPedido = await _context.DetallesPedidos.FindAsync(id);
            if (detallesPedido != null)
            {
                _context.DetallesPedidos.Remove(detallesPedido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetallesPedidoExists(int id)
        {
            return _context.DetallesPedidos.Any(e => e.DetallePedidoId == id);
        }
    }
}
