using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CedrosNahuizalquenos.Models;

public partial class Usuario
{

    public int UsuarioId { get; set; }
    [Required(ErrorMessage = "El nombre completo es obligatorio.")]
    public string NombreCompleto { get; set; } = null!;
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "Ingrese un correo electrónico válido.")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(100, ErrorMessage = "La contraseña debe tener al menos {2} caracteres.", MinimumLength = 6)]
    public string Contrasena { get; set; } = null!;
    [Required(ErrorMessage = "Seleccione un rol.")]
    public string Rol { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public virtual ICollection<Notificacione> Notificaciones { get; set; } = new List<Notificacione>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
