using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CedrosNahuizalquenos.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CedrosNahuizalquenos.Controllers
{
    public class ProductoesController : Controller
    {
        private readonly CedrosNahuiContext _context;
        private readonly IMemoryCache _memoryCache;

        public ProductoesController(CedrosNahuiContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }
        // GET: Productoes
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 6)
        {
            var cacheKey = "productosCache";
            List<Producto> productos;

            // Intenta obtener productos de la caché
            if (!_memoryCache.TryGetValue(cacheKey, out productos))
            {
                // Cargar solo los productos que están disponibles
                productos = await _context.Productos
                                           .Where(p => p.EstadoProducto != "No Disponible") // Filtrar productos no disponibles
                                           .Skip((pageNumber - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

                // Guardar en caché
                _memoryCache.Set(cacheKey, productos, TimeSpan.FromMinutes(5));
            }

            // Siempre cargar nuevos productos que no están en caché y que están disponibles
            var nuevosProductos = await _context.Productos
                                                 .Where(p => p.EstadoProducto != "No Disponible" && // Filtrar productos no disponibles
                                                             !productos.Select(prod => prod.ProductoId).Contains(p.ProductoId))
                                                 .ToListAsync();

            // Combina los productos del caché con los nuevos
            productos.AddRange(nuevosProductos);

            // Filtra nuevamente los productos combinados para eliminar los "No disponible"
            productos = productos.Where(p => p.EstadoProducto != "No Disponible").ToList();

            // Paginación
            ViewBag.TotalCount = await _context.Productos.CountAsync(p => p.EstadoProducto != "No Disponible"); // Para paginación
            ViewBag.PageNumber = pageNumber;
            ViewBag.PageSize = pageSize;

            // Aplica la paginación
            var paginatedProductos = productos.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return View(paginatedProductos);
        }



        // GET: Productoes/Details/5
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _context.Productos
                                .Include(p => p.Personalizaciones) // Incluimos personalizaciones
                                .FirstOrDefaultAsync(p => p.ProductoId == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Productoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Productoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Producto producto, IFormFile imagen)
        {
            string message = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (imagen != null && imagen.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await imagen.CopyToAsync(memoryStream);
                            producto.Imagen = memoryStream.ToArray(); // Convertir a VARBINARY
                        }
                    }

                    // Guardar el producto en la base de datos
                    _context.Productos.Add(producto);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Producto agregado exitosamente." });
                }
            }
            catch (Exception ex)
            {
               message = ex.Message;
            }
            return Json(new { success = false, message = $"Error: {message}" });
        }

        // GET: Productoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.ProductoId == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Productoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.ProductoId == id);
        }
        [HttpPost]
        public async Task<IActionResult> AgregarAlCarrito([FromBody] PedidoDTO pedidoDto)
        {
            // Validar el producto
            var producto = await _context.Productos.FindAsync(pedidoDto.ProductoID);
            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }
            var opcionesPersonalizacion = $"Tamaño: {pedidoDto.Tamano}, Tipo: {pedidoDto.Tipo}, Tono de Madera: {pedidoDto.TonoMadera}";
            // Calcular los costos adicionales basados en las opciones seleccionadas

            decimal costoExtraTamano = pedidoDto.Tamano.Contains("6 personas") ? 100 : pedidoDto.Tamano.Contains("8 personas") ? 200 : 0;
            decimal costoExtraTono = pedidoDto.TonoMadera.Contains("Blanco") ? 100 : 0;

            // Calcular el total del pedido
            decimal totalCosto = (producto.PrecioBase + costoExtraTamano + costoExtraTono) * pedidoDto.Cantidad;

            // Crear una nueva entrada en la tabla de personalizaciones
            var nuevaPersonalizacion = new Personalizacione
            {
                ProductoId = pedidoDto.ProductoID,
                OpcionPersonalizacion  = opcionesPersonalizacion,
                CostosExtras = costoExtraTamano + costoExtraTono// Sumatoria de los costos extra de las opciones
            };
            _context.Personalizaciones.Add(nuevaPersonalizacion);
            await _context.SaveChangesAsync(); // Guardar la personalización para generar el PersonalizacionID

            // Crear un nuevo pedido
            var nuevoPedido = new Pedido
            {
                UsuarioId = pedidoDto.UsuarioID,
                EstadoPedido = "En Carrito",
                FechaPedido = DateTime.Now
            };
            _context.Pedidos.Add(nuevoPedido);
            await _context.SaveChangesAsync(); // Guardar el pedido para generar el PedidoID

            // Crear un detalle de pedido asociado al pedido y a la personalización
            var detallePedido = new DetallesPedido
            {
                PedidoId = nuevoPedido.PedidoId,
                ProductoId = producto.ProductoId,
                Cantidad = pedidoDto.Cantidad,
                PrecioUnitario = producto.PrecioBase,
                Subtotal = totalCosto,
                PersonalizacionId = nuevaPersonalizacion.PersonalizacionId, // Asociar la personalización recién creada
                Anticipo = totalCosto * 0.5m // Ejemplo de anticipo, 50% del total
            };
            _context.DetallesPedidos.Add(detallePedido);
            await _context.SaveChangesAsync(); // Guardar el detalle del pedido

            return Json(new { success = true, message = "Producto y personalización agregados al carrito correctamente." });
        }



    }
}
