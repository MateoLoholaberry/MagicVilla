using AutoMapper;
using MagicVilla_API.Dtos;
using MagicVilla_API.Models;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepositorio;
        private readonly INumeroVillaRepositorio _numeroVillaRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepositorio, IMapper mapper, INumeroVillaRepositorio numeroVillaRepositorio)
        {
            _logger = logger;
            _villaRepositorio = villaRepositorio;
            _mapper = mapper;
            _response = new APIResponse();
            _numeroVillaRepositorio = numeroVillaRepositorio;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("Obtener todos los numeros de villas");

                var numeroVillaList = await _numeroVillaRepositorio.ObtenerTodos();

                _response.Result = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillaList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }


        [HttpGet("{id:int}", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {

            try
            {
                if (id == 0)
                {
                    _logger.LogError($"Error al traer Numero Villa con Id {id}");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
                var numeroVilla = await _numeroVillaRepositorio.Obtener(v => v.VillaNo == id);

                if (numeroVilla == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<NumeroVillaDto>(numeroVilla);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;

        }



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CrearNumeroVilla(NumeroVillaCreateDto createDto)
        {

            try
            {
                if (await _numeroVillaRepositorio.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExistente", "El número de villa ya existe");
                    return BadRequest(ModelState);
                }

                if (await _villaRepositorio.Obtener(v => v.Id == createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForánea", "El Id de villa no existe");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                //villaDto.Id = VillaDatos.villaList.OrderByDescending(v => v.Id).First().Id + 1;
                //VillaDatos.villaList.Add(villaDto);

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);
                modelo.FechaActualizacion = DateTime.Now;
                modelo.FechaCreacion = DateTime.Now;

                await _numeroVillaRepositorio.Crear(modelo);

                _response.Result = _mapper.Map<NumeroVilla>(modelo);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, NumeroVillaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.VillaNo)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _villaRepositorio.Obtener(v => v.Id == updateDto.VillaNo) == null)
            {
                ModelState.AddModelError("ClaveForánea", "El Id de villa no existe");
                return BadRequest(ModelState);
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

            await _numeroVillaRepositorio.Actualizar(modelo);

            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
                var numeroVilla = await _numeroVillaRepositorio.Obtener(v => v.VillaNo == id);

                if (numeroVilla == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                //VillaDatos.villaList.Remove(villa);
                await _numeroVillaRepositorio.Remover(numeroVilla);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
        }
    }
}
