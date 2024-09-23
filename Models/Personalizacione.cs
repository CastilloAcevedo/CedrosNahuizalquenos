using System;
using System.Collections.Generic;

namespace CedrosNahuizalquenos.Models;

public partial class Personalizacione
{
    public int PersonalizacionId { get; set; }

    public int ProductoId { get; set; }

    public string OpcionPersonalizacion { get; set; } = null!;

    public decimal CostosExtras { get; set; }

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();

    public virtual Producto Producto { get; set; } = null!;
}
