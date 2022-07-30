using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.TaskTypes.Models;
using Notes.NotesService;
using Notes.TaskTypeService;
using Notes.TaskTypeService.Models;

namespace Notes.Api.Controllers.TaskTypes
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskTypesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly ITaskTypeService typesService;

        public TaskTypesController(IMapper mapper, ITaskTypeService typesService)
        {
            this.mapper = mapper;
            this.typesService = typesService;
        }

        [HttpGet("")]
        public async Task<IEnumerable<TaskTypeResponse>> GetTypes()
        {
            var data = await typesService.GetTaskTypes();
            var result = mapper.Map<IEnumerable<TaskTypeResponse>>(data);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<TaskTypeResponse> GetTypeById([FromRoute] int id)
        {
            var data = await typesService.GetTaskById(id);
            var result = mapper.Map<TaskTypeResponse>(data);
            return result;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddType([FromBody] TaskTypeRequest type)
        {
            var data = mapper.Map<TaskTypeRequestModel>(type);
            await typesService.AddTask(data);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateType([FromRoute] int id, [FromBody] TaskTypeRequest type)
        {
            var data = mapper.Map<TaskTypeRequestModel>(type);
            await typesService.UpdateTask(data, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteType([FromRoute] int id)
        {
            await typesService.DeleteTask(id);
            return Ok();
        }
    }
}
