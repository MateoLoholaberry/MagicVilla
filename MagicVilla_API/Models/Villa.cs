using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models
{
    public class Villa
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Detalle { get; set; }
        public double Tarifa { get; set; }
        public int Ocupantes { get; set; }
        public int MetrosCuadrados { get; set; }
        public string? ImageUrl { get; set; }
        public string? Amenidad { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
