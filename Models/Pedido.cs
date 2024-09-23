using System;
using System.Collections.Generic;

namespace CedrosNahuizalquenos.Models;

public partial class Pedido
{
    public int PedidoId { get; set; }

    public int UsuarioId { get; set; }

    public string EstadoPedido { get; set; } = null!;

    public DateTime? FechaPedido { get; set; }

    public DateTime? FechaEntregaEstimada { get; set; }

    public virtual ICollection<DetallesPedido> DetallesPedidos { get; set; } = new List<DetallesPedido>();

    public virtual ICollection<Notificacione> Notificaciones { get; set; } = new List<Notificacione>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Usuario Usuario { get; set; } = null!;
}
