using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.NotesService;

namespace Notes.Api.Controllers.Notes
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly INotesService notesService;

        public NotesController(IMapper mapper, INotesService notesService)
        {
            this.mapper = mapper;
            this.notesService = notesService;
        }

        [HttpGet("")]
        public async Task GetNotes()
        {
            notesService.GetNotes();
        }

    }
}
