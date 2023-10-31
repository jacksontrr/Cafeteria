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
        public DbSet<Cliente> Usuarios { get; set; }
        
    }
}
