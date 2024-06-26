﻿using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Dtos;
using MagicVilla_API.Models;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepositorio;
        private readonly IMapper _mapper;
        protected APIResponse _response;

        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepositorio, IMapper mapper)
        {
            _logger = logger;
            _villaRepositorio = villaRepositorio;
            _mapper = mapper;
            _response = new APIResponse();
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                _logger.LogInformation("Obtener todas las villas");

                var villaList = await _villaRepositorio.ObtenerTodos();

                _response.Result = _mapper.Map<IEnumerable<VillaDto>>(villaList);
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


        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {

            try
            {
                if (id == 0)
                {
                    _logger.LogError($"Error al traer Villa con Id {id}");
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villaRepositorio.Obtener(v => v.Id == id);

                if (villa == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<VillaDto>(villa);
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
        public async Task<ActionResult<APIResponse>> CrearVilla(VillaCreateDto villaCreateDto)
        {

            try
            {
                if (await _villaRepositorio.Obtener(v => v.Nombre.ToLower() == villaCreateDto.Nombre.ToLower()) != null)
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

                await _villaRepositorio.Crear(modelo);

                _response.Result = _mapper.Map<VillaDto>(modelo);
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _response);
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
        public async Task<IActionResult> UpdateVilla(int id, VillaUpdateDto villaUpdateDto)
        {
            if (villaUpdateDto == null || id != villaUpdateDto.Id)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            Villa modelo = _mapper.Map<Villa>(villaUpdateDto);

            await _villaRepositorio.Actualizar(modelo);

            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }


        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchVillaDto)
        {
            if (patchVillaDto == null || id == 0)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            //var villa = VillaDatos.villaList.FirstOrDefault(v => v.Id == id);
            var villa = await _villaRepositorio.Obtener(v => v.Id == id);

            if (villa == null) return BadRequest();

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

            patchVillaDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _villaRepositorio.PartialActualizar(villa, villaDto);


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

            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
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
                var villa = await _villaRepositorio.Obtener(v => v.Id == id);

                if (villa == null)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                //VillaDatos.villaList.Remove(villa);
                await _villaRepositorio.Remover(villa);

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
