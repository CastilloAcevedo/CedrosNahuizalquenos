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
        public IActionResult Carrito(int usuarioId)
        {
            var pedidos = _context.Pedidos
                .Where(p => p.UsuarioId == usuarioId)
                .Select(p => new
                {
                    PedidoId = p.PedidoId,
                    Detalles = p.DetallesPedidos.Select(d => new
                    {
                        DetallePedidoId = d.DetallePedidoId,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario,
                        Subtotal = d.Subtotal,
                        Producto = new
                        {
                            ProductoId = d.ProductoId,
                            NombreProducto = d.Producto.NombreProducto,
                            Imagen = d.Producto.Imagen
                        },
                        Personalizacion = d.Personalizacion != null ? new
                        {
                            OpcionPersonalizacion = d.Personalizacion.OpcionPersonalizacion,  // Opción de personalización
                            CostosExtras = d.Personalizacion.CostosExtras  // Costo extra por la personalización
                        } : null
                    }).ToList()
                })
                .ToList();

            // Calcular el total considerando el costo adicional de las personalizaciones
            var total = pedidos.Sum(p => p.Detalles.Sum(d => d.Subtotal));

            var totalAnticipo = total * 0.5m;  // Anticipo del 50%

            ViewBag.Pedidos = pedidos;
            ViewBag.Total = total;
            ViewBag.TotalAnticipo = totalAnticipo;

            return View();
        }
        [HttpPost]
        public IActionResult EliminarDelCarrito([FromBody] elimDTO detallePedidoId)
        {
            try
            {
                // Buscar el detalle del pedido por el ID
                var detallePedido = _context.DetallesPedidos
                    .Include(d => d.Pedido)  // Incluir el pedido relacionado
                    .Include(d => d.Personalizacion)  // Incluir la personalización relacionada
                    .FirstOrDefault(d => d.DetallePedidoId == detallePedidoId.detalle);

                if (detallePedido == null)
                {
                    return Json(new { success = false, message = "Producto no encontrado en el carrito." });
                }

                // Eliminar la personalización asociada si existe
                if (detallePedido.PersonalizacionId.HasValue)
                {
                    var personalizacion = _context.Personalizaciones.FirstOrDefault(p => p.PersonalizacionId == detallePedido.PersonalizacionId);
                    if (personalizacion != null)
                    {
                        _context.Personalizaciones.Remove(personalizacion);
                    }
                }

                // Eliminar el detalle del pedido
                _context.DetallesPedidos.Remove(detallePedido);

                // Verificar si el pedido tiene otros detalles. Si no tiene, eliminar también el pedido.
                var pedido = detallePedido.Pedido;
                _context.Pedidos.Remove(pedido);  // Eliminar el pedido si no hay más detalles

                // Guardar los cambios en la base de datos
                _context.SaveChanges();

                return Json(new { success = true, message = "Producto eliminado del carrito correctamente." });
            }
            catch (Exception ex)
            {
                // Manejar cualquier error y retornar un mensaje adecuado
                return Json(new { success = false, message = "Ocurrió un error al intentar eliminar el producto del carrito: " + ex.Message });
            }
        }




        public IActionResult ObtenerImagenProducto(int productoId)
        {
            var producto = _context.Productos.Find(productoId);
            if (producto?.Imagen != null)
            {
                // Retornar la imagen como un archivo de tipo MIME para imágenes
                return File(producto.Imagen, "image/jpg"); // Puedes ajustar el tipo MIME según el tipo de la imagen
            }
            return NotFound(); // En caso de que no exista la imagen
        }

    }
}

