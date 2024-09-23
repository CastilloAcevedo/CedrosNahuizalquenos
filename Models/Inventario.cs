using System;
using System.Collections.Generic;

namespace CedrosNahuizalquenos.Models;

public partial class Inventario
{
    public int InventarioId { get; set; }

    public int ProductoId { get; set; }

    public int CantidadStock { get; set; }

    public DateTime? FechaActualizacion { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
