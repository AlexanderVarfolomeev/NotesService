using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.NotesService;
using Notes.TaskTypeService;

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
        public async Task GetTypes()
        {
            var a = await typesService.GetTaskTypes();
        }
    }
}
