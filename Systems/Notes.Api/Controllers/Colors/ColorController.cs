using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Colors.Models;
using Notes.ColorService;

namespace Notes.Api.Controllers.Colors
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IColorService colorService;

        public ColorController(IMapper mapper, IColorService colorService)
        {
            this.mapper = mapper;
            this.colorService = colorService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<ColorResponse>> GetTypes()
        {
            var data = await colorService.GetColors();
            var result = mapper.Map<IEnumerable<ColorResponse>>(data);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ColorResponse> GetTypeById([FromRoute] int id)
        {
            var data = await colorService.GetColorById(id);
            var result = mapper.Map<ColorResponse>(data);
            return result;
        }
    }
}
