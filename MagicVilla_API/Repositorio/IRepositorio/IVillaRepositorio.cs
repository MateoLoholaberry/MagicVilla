using MagicVilla_API.Dtos;
using MagicVilla_API.Models;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IVillaRepositorio : IRepositorio<Villa>
    {
        Task<Villa> Actualizar(Villa villa);
        Task<Villa> PartialActualizar(Villa entidad, VillaUpdateDto villaDto);
    }
}
