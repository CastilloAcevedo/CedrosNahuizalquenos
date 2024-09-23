using System;
using System.Collections.Generic;

namespace CedrosNahuizalquenos.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int PedidoId { get; set; }

    public DateTime? FechaPago { get; set; }

    public decimal MontoPagado { get; set; }

    public string MetodoPago { get; set; } = null!;

    public string? Ncuenta { get; set; }

    public string Referencia { get; set; } = null!;

    public virtual Pedido Pedido { get; set; } = null!;
}
