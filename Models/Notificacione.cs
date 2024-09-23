using System;
using System.Collections.Generic;

namespace CedrosNahuizalquenos.Models;

public partial class Notificacione
{
    public int NotificacionId { get; set; }

    public int UsuarioId { get; set; }

    public int? PedidoId { get; set; }

    public string Mensaje { get; set; } = null!;

    public DateTime? FechaEnvio { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Pedido? Pedido { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
