using MagicVilla_API.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Villa> Villas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                    new Villa()
                    {
                        Id = 1,
                        Nombre = "Villa Real",
                        Detalle = "Detalle de la villa...",
                        ImageUrl = "",
                        Ocupantes = 5,
                        MetrosCuadrados = 50,
                        Tarifa = 250,
                        Amenidad = "",
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                    },
                    new Villa()
                    {
                        Id = 2,
                        Nombre = "Premiun vista a la piscina",
                        Detalle = "Detalle de la villa...",
                        ImageUrl = "",
                        Ocupantes = 4,
                        MetrosCuadrados = 40,
                        Tarifa = 150,
                        Amenidad = "",
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,
                    }
                );
        }
    }
}
