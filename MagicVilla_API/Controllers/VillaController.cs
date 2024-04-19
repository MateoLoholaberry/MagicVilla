using AutoMapper;
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
        private readonly IMapper _mapper;

        public VillaController(ILogger<VillaController> logger, AppDbContext appDbContext, IMapper mapper)
        {
            _logger = logger;
            _appDbContext = appDbContext;
            _mapper = mapper;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVillas()
        {
            _logger.LogInformation("Obtener todas las villas");

            var villaList = await _appDbContext.Villas.ToListAsync();

            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villaList));
        }


        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {

            if (id == 0)
            {
                _logger.LogError($"Error al traer Villa con Id {id}");
                return BadRequest();
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _appDbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<VillaDto>(villa));
        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> CrearVilla(VillaCreateDto villaCreateDto)
        {

            if (await _appDbContext.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == villaCreateDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExistente", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (villaCreateDto == null)
            {
                return BadRequest(villaCreateDto);
            }

            //villaDto.Id = VillaDatos.villaList.OrderByDescending(v => v.Id).First().Id + 1;
            //VillaDatos.villaList.Add(villaDto);

            Villa modelo = _mapper.Map<Villa>(villaCreateDto);
            modelo.FechaActualizacion = DateTime.Now;
            modelo.FechaCreacion = DateTime.Now;

            _appDbContext.Villas.Add(modelo);
            await _appDbContext.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _mapper.Map<VillaDto>(modelo));
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVilla(int id, VillaUpdateDto villaUpdateDto)
        {
            if (villaUpdateDto == null || id != villaUpdateDto.Id)
            {
                return BadRequest();
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            Villa modelo = _mapper.Map<Villa>(villaUpdateDto);
            modelo.FechaActualizacion = DateTime.Now;

            _appDbContext.Villas.Update(modelo);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<bool>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchVillaDto)
        {
            if (patchVillaDto == null || id == 0)
            {
                return BadRequest();
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _appDbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null) return BadRequest();

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            patchVillaDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _appDbContext.Entry(villa).CurrentValues.SetValues(villaDto);
            villa.FechaActualizacion = DateTime.Now;

            await _appDbContext.SaveChangesAsync();

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
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _appDbContext.Villas.FirstOrDefaultAsync(v => v.Id == id);

            if (villa == null)
            {
                return NotFound();
            }

            //VillaDatos.villaList.Remove(villa);
            _appDbContext.Villas.Remove(villa);
            await _appDbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
