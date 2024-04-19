using MagicVilla_API.Dtos;

namespace MagicVilla_API.Datos
{
    public static class VillaDatos
    {
        public static List<VillaDto> villaList = new List<VillaDto> 
        {
                new() { Id = 1, Nombre="Vista a la Piscina", Ocupantes= 3, MetrosCuadrados= 50},
                new() { Id = 2, Nombre="Vista a la Playa", Ocupantes= 5, MetrosCuadrados= 80}
        };
    }
}
