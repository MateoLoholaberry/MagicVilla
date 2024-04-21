using MagicVilla_API.Datos;
using MagicVilla_API.Dtos;
using MagicVilla_API.Models;
using MagicVilla_API.Repositorio.IRepositorio;

namespace MagicVilla_API.Repositorio
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
    {
        private readonly AppDbContext _appDbContext;

        public NumeroVillaRepositorio(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<NumeroVilla> Actualizar(NumeroVilla entidadNueva)
        {
            var entidadVieja = await Obtener(v => v.VillaNo == entidadNueva.VillaNo);
            entidadNueva.FechaCreacion = entidadVieja.FechaCreacion;
            entidadNueva.FechaActualizacion = DateTime.Now;

            _appDbContext.Entry(entidadVieja).CurrentValues.SetValues(entidadNueva);
            await Grabar();

            return entidadVieja;
        }

    }
}
