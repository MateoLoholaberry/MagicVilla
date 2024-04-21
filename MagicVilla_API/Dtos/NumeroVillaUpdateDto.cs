using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Dtos
{
    public class NumeroVillaUpdateDto
    {
        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }

        public string DetalleEspecial { get; set; }
    }
}
