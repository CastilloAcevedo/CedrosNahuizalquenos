using System;
using System.Collections.Generic;

namespace CedrosNahuizalquenos.Models;

public partial class DetallesPedido
{
    public int DetallePedidoId { get; set; }

    public int PedidoId { get; set; }

    public int ProductoId { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public int? PersonalizacionId { get; set; }

    public decimal Anticipo { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;

    public virtual Personalizacione? Personalizacion { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
