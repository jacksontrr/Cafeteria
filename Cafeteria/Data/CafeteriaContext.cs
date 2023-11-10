using Cafeteria.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafeteria.Data
{
    public class CafeteriaContext : DbContext
    {

        public CafeteriaContext(DbContextOptions<CafeteriaContext> options)
            : base(options)
        {
        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Favorito> Favoritos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<PedidoProduto> PedidoProdutos { get; set; }
        
    }
}
