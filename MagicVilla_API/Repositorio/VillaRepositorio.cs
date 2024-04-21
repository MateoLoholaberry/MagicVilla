using MagicVilla_API.Datos;
using MagicVilla_API.Dtos;
using MagicVilla_API.Models;
using MagicVilla_API.Repositorio.IRepositorio;

namespace MagicVilla_API.Repositorio
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly AppDbContext _appDbContext;

        public VillaRepositorio(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<Villa> Actualizar(Villa entidadNueva)
        {
            var entidadVieja = await Obtener(v => v.Id == entidadNueva.Id);
            entidadNueva.FechaCreacion = entidadVieja.FechaCreacion;
            entidadNueva.FechaActualizacion = DateTime.Now;

            _appDbContext.Entry(entidadVieja).CurrentValues.SetValues(entidadNueva);
            await Grabar();

            return entidadVieja;
        }

        public async Task<Villa> PartialActualizar(Villa entidad, VillaUpdateDto villaDto)
        {
            _appDbContext.Entry(entidad).CurrentValues.SetValues(villaDto);
            entidad.FechaActualizacion = DateTime.Now;
            await Grabar();
            return entidad;
        }
    }
}
