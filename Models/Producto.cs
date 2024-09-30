using System;
using System.Collections.Generic;

namespace CedrosNahuizalquenos.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string NombreProducto { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public decimal PrecioBase { get; set; }

    public byte[]? Imagen { get; set; } // Cambia a tipo nullable

    public string EstadoProducto { get; set; } = null!;

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();

    public virtual ICollection<Inventario> Inventarios { get; set; } = new List<Inventario>();

    public virtual ICollection<Personalizacione> Personalizaciones { get; set; } = new List<Personalizacione>();
}
