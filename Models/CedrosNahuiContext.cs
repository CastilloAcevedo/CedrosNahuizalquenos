using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CedrosNahuizalquenos.Models;

public partial class CedrosNahuiContext : DbContext
{
    public CedrosNahuiContext()
    {
    }

    public CedrosNahuiContext(DbContextOptions<CedrosNahuiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DetallesPedido> DetallesPedidos { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Notificacione> Notificaciones { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Personalizacione> Personalizaciones { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { 
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetallesPedido>(entity =>
        {
            entity.HasKey(e => e.DetallePedidoId).HasName("PK__Detalles__6ED21C011773CE1E");

            entity.ToTable("DetallesPedido");

            entity.Property(e => e.DetallePedidoId).HasColumnName("DetallePedidoID");
            entity.Property(e => e.Anticipo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PedidoId).HasColumnName("PedidoID");
            entity.Property(e => e.PersonalizacionId).HasColumnName("PersonalizacionID");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Pedido).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallesP__Pedid__4316F928");

            entity.HasOne(d => d.Personalizacion).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.PersonalizacionId)
                .HasConstraintName("FK__DetallesP__Perso__44FF419A");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallesPedidos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallesP__Produ__440B1D61");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.InventarioId).HasName("PK__Inventar__FB8A24B78B036958");

            entity.ToTable("Inventario");

            entity.HasIndex(e => e.ProductoId, "IDX_Inventario_Producto");

            entity.Property(e => e.InventarioId).HasColumnName("InventarioID");
            entity.Property(e => e.FechaActualizacion)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");

            entity.HasOne(d => d.Producto).WithMany(p => p.Inventarios)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventari__Produ__4CA06362");
        });

        modelBuilder.Entity<Notificacione>(entity =>
        {
            entity.HasKey(e => e.NotificacionId).HasName("PK__Notifica__BCC120C4BE8CB643");

            entity.Property(e => e.NotificacionId).HasColumnName("NotificacionID");
            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaEnvio).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PedidoId).HasColumnName("PedidoID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.PedidoId)
                .HasConstraintName("FK__Notificac__Pedid__5165187F");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificac__Usuar__5070F446");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pagos__F00B6158E7A601F8");

            entity.Property(e => e.PagoId).HasColumnName("PagoID");
            entity.Property(e => e.FechaPago).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.MetodoPago).HasMaxLength(50);
            entity.Property(e => e.MontoPagado).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Ncuenta)
                .HasMaxLength(100)
                .HasColumnName("NCuenta");
            entity.Property(e => e.PedidoId).HasColumnName("PedidoID");
            entity.Property(e => e.Referencia).HasMaxLength(100);

            entity.HasOne(d => d.Pedido).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pagos__PedidoID__48CFD27E");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.PedidoId).HasName("PK__Pedidos__09BA141029F3EEC0");

            entity.HasIndex(e => e.FechaPedido, "IDX_Pedido_Fecha");

            entity.HasIndex(e => e.UsuarioId, "IDX_Pedido_Usuario");

            entity.Property(e => e.PedidoId).HasColumnName("PedidoID");
            entity.Property(e => e.EstadoPedido).HasMaxLength(50);
            entity.Property(e => e.FechaPedido)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnType("datetime");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pedidos__Usuario__403A8C7D");
        });

        modelBuilder.Entity<Personalizacione>(entity =>
        {
            entity.HasKey(e => e.PersonalizacionId).HasName("PK__Personal__EBAB3CF7C34B8D65");

            entity.Property(e => e.PersonalizacionId).HasColumnName("PersonalizacionID");
            entity.Property(e => e.CostosExtras).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");

            entity.HasOne(d => d.Producto).WithMany(p => p.Personalizaciones)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Personali__Produ__3C69FB99");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AE834815DED0");

            entity.Property(e => e.ProductoId).HasColumnName("ProductoID");
            entity.Property(e => e.EstadoProducto).HasMaxLength(50);
            entity.Property(e => e.NombreProducto).HasMaxLength(100);
            entity.Property(e => e.PrecioBase).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798504C2300");

            entity.HasIndex(e => e.Email, "IDX_Usuario_Email");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D10534BD8171D9").IsUnique();

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Contrasena).HasMaxLength(256);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(sysdatetime())")
                .HasColumnType("datetime");
            entity.Property(e => e.NombreCompleto).HasMaxLength(100);
            entity.Property(e => e.Rol).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
