using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Model;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.Iservices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService, IMapper mapper) 
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> list = new List<VillaDTO>();
            var response = await _villaService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionToken));
            if (response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
