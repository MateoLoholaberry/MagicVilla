using MagicVilla_API.Datos;
using MagicVilla_API.Dtos;
using MagicVilla_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly AppDbContext _appDbContext;

        public VillaController(ILogger<VillaController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            _logger.LogInformation("Obtener todas las villas");

            var villas = _appDbContext.Villas.ToList();
            return Ok(villas);
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {

            if (id == 0)
            {
                _logger.LogError($"Error al traer Villa con Id {id}");
                return BadRequest();
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _appDbContext.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(villa);
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> CrearVilla(VillaDto villaDto)
        {

            if (_appDbContext.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExistente", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (villaDto == null)
            {
                return BadRequest();
            }

            if (villaDto.Id != 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            //villaDto.Id = VillaDatos.villaList.OrderByDescending(v => v.Id).First().Id + 1;
            //VillaDatos.villaList.Add(villaDto);

            Villa modelo = new()
            {
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImageUrl = villaDto.ImageUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad,
                FechaActualizacion = DateTime.Now,
                FechaCreacion = DateTime.Now
            };

            _appDbContext.Villas.Add(modelo);
            _appDbContext.SaveChanges();

            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> UpdateVilla(int id, VillaDto villaDto)
        {
            if (villaDto == null || id != villaDto.Id)
            {
                return BadRequest();
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            Villa modelo = new()
            {
                Id = id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImageUrl = villaDto.ImageUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad,
                FechaActualizacion = DateTime.Now
            };

            _appDbContext.Villas.Update(modelo);
            _appDbContext.SaveChanges();

            return NoContent();
        }


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<bool> UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchVillaDto)
        {
            if (patchVillaDto == null || id == 0)
            {
                return BadRequest();
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _appDbContext.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null) return BadRequest();

            VillaDto villaDto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImageUrl = villa.ImageUrl,
                Ocupantes = villa.Ocupantes,
                Amenidad = villa.Amenidad,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
            };

            patchVillaDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _appDbContext.Entry(villa).CurrentValues.SetValues(villaDto);
            _appDbContext.SaveChanges();

            // Este tipo de actualizacion da error. Consultar https://learn.microsoft.com/es-es/ef/core/change-tracking/identity-resolution
            //Villa modelo = new()
            //{
            //    Id = villaDto.Id,
            //    Nombre = villaDto.Nombre,
            //    Detalle = villaDto.Detalle,
            //    ImageUrl = villaDto.ImageUrl,
            //    Ocupantes = villaDto.Ocupantes,
            //    Amenidad = villaDto.Amenidad,
            //    Tarifa = villaDto.Tarifa,
            //    MetrosCuadrados = villaDto.MetrosCuadrados,
            //};

            //_appDbContext.Villas.Update(modelo);
            //_appDbContext.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            var villa = _appDbContext.Villas.FirstOrDefault(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            //VillaDatos.villaList.Remove(villa);
            _appDbContext.Villas.Remove(villa);
            _appDbContext.SaveChanges();

            return NoContent();
        }
    }
}
