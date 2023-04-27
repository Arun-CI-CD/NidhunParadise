using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NidhunParadise_API.Data;
using NidhunParadise_API.Model;
using NidhunParadise_API.Model.Dto;
using NidhunParadise_API.Repository.IRepository;
using System.Collections.Generic;
using System.Net;

namespace NidhunParadise_API.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/ParadiseAPI")]
    [ApiController]
    public class ParadiseAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaRepository _dbVilla;
        //private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public ParadiseAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;
            _mapper = mapper;
            this._response = new();
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<APIResponse>> GetVillas() 
        {
            try
            {
                IEnumerable<Villa> villalist = await _dbVilla.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villalist);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() {ex.ToString() };
            }
            return _response;
        }

        [HttpGet("{id:int}", Name ="GetVilla")]
        [ProducesResponseType(200, Type=typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                else
                {
                    _response.Result = _mapper.Map<VillaDTO>(villa);
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaCreateDTO createDto)
        {
            try
            {
                //if (await _db.Villas.FirstOrDefaultAsync(u => u.Name.ToLower() == villaDto.Name.ToLower()))
                //{
                //    ModelState.AddModelError("Error", "Villa Already Exists");
                //    return BadRequest(ModelState);
                //}
                if (createDto == null)
                {
                    return BadRequest();
                }
                //if(villaDto.Id>0)
                //{
                //    return StatusCode(StatusCodes.Status500InternalServerError);
                //}

                Villa villa = _mapper.Map<Villa>(createDto);

                //Villa model = new()
                //{
                //    Amenity = villcreateDtoaDto.Amenity,
                //    Detials = createDto.Detials,
                //    Sqft = createDto.Sqft,
                //    Occupancy = createDto.Occupancy,
                //    Rate = createDto.Rate,
                //    Name = createDto.Name,
                //    ImageUrl = createDto.ImageUrl
                //};
                await _dbVilla.CreateVillaAsync(villa);
                //return Ok(villaDto);
                _response.Result = _mapper.Map<VillaDTO>(villa);
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        [Authorize(Roles = "Custom")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    return NotFound();
                }
                else
                {
                    await _dbVilla.RemoveVillaAsync(villa);
                    _response.IsSuccess = true;
                    _response.StatusCode = HttpStatusCode.NoContent;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPut("{id:int})", Name ="UpdateVilla")]
        public async Task<ActionResult<APIResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDTO updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    return BadRequest();
                }
                Villa model = _mapper.Map<Villa>(updateDto);
                await _dbVilla.UpdateAsync(model);
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPatch("{id:int}",Name ="UpdatePartialVilla")]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)
        {
                if (patchDTO == null || id == 0)
                {
                    return BadRequest();
                }
                var villa = await _dbVilla.GetAsync(u => u.Id == id, tracked: false);
                VillaUpdateDTO villaDto = _mapper.Map<VillaUpdateDTO>(villa);

                if (villa == null)
                {
                    return BadRequest();
                }
                patchDTO.ApplyTo(villaDto, ModelState);
                Villa model = _mapper.Map<Villa>(villaDto);
                await _dbVilla.UpdateAsync(model);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return NoContent();
        }
    }
}
