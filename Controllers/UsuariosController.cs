using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CedrosNahuizalquenos.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Scripting;


namespace CedrosNahuizalquenos.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly CedrosNahuiContext _context;
        public UsuariosController(CedrosNahuiContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,NombreCompleto,Email,Contrasena,Rol,FechaRegistro")] Usuario usuario, string? CodigoSeguridad)
        {
            // Validar el código de seguridad si el rol es Administrador
            if (usuario.Rol == "Administrador" && !IsValidSecurityCode(CodigoSeguridad))
            {
                ModelState.AddModelError("CodigoSeguridad", "El código de seguridad no tiene un formato válido.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Encriptar la contraseña
                    var passwordHasher = new PasswordHasher<Usuario>();
                    usuario.Contrasena = passwordHasher.HashPassword(usuario, usuario.Contrasena);

                    _context.Add(usuario);
                    await _context.SaveChangesAsync();
                    // Retorna una respuesta JSON indicando éxito
                    return Json(new { success = true });
                }
                catch (Exception ex) {
                    return Json(new { success = false, errors = "No se permiten registros duplicados." });
                }
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        // Método para validar el formato del código de seguridad
        private bool IsValidSecurityCode(string codigoSeguridad)
        {
            try
            {
                return Regex.IsMatch(codigoSeguridad, @"^[A-F0-9]{8}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{4}-[A-F0-9]{12}$");
            }
            catch (Exception ex) {
                return false;
            }
            // Regex para validar el formato del código de seguridad
           
        }

        // POST: Usuarios/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {


            // Instancia de PasswordHasher para comparar contraseñas
            var passwordHasher = new PasswordHasher<Usuario>();

            // Buscar el usuario por el email
            var userInDb = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);

            if (userInDb == null)
            {
                return Json(new { success = false, errors = "El correo electrónico es incorrecto." });
            }

            // Verificar la contraseña
            var verificationResult = passwordHasher.VerifyHashedPassword(userInDb, userInDb.Contrasena, password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return Json(new { success = false, errors = "La contraseña es incorrecta." });
            }

            // Si la autenticación es correcta, redirigir según el rol
            if (userInDb.Rol == "Administrador")
            {
                // Guardar el rol en la sesión
                HttpContext.Session.SetString("UsuarioName", userInDb.NombreCompleto);
                HttpContext.Session.SetString("UserRole", userInDb.Rol);
                HttpContext.Session.SetString("UserId", userInDb.UsuarioId.ToString());
                return Json(new { success = true, rol="admin", name = userInDb.NombreCompleto });
            }
            else if (userInDb.Rol == "Cliente")
            {
                HttpContext.Session.SetString("UsuarioName", userInDb.NombreCompleto);
                HttpContext.Session.SetString("UserRole", userInDb.Rol);
                HttpContext.Session.SetString("UserId", userInDb.UsuarioId.ToString());
                return Json(new { success = true, rol = "client", name = userInDb.NombreCompleto });
            }

            // Si el rol es diferente o no está configurado, redirigir a una página predeterminada
            return RedirectToAction("Index", "Home");
        }
        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,NombreCompleto,Email,Contrasena,Rol,FechaRegistro")] Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
        // Esta acción redirige al login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RedirectToLogin()
        {
            return RedirectToAction("Index", "Home"); // Redirige a la acción de login
        }
    }
}
