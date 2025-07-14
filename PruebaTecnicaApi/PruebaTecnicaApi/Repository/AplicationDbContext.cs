using Microsoft.EntityFrameworkCore;
using PruebaTecnicaModelo.Modelos;

namespace PruebaTecnicaApi.Repository
{
    public class AplicationDbContext :DbContext 
    {
        public AplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Empleado> Empleado { get; set; }
    }
}
  