using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Notes.Models;
using Notes.Api.Controllers.TaskTypes.Models;
using Notes.NotesService;
using Notes.NotesService.Models;
using Notes.TaskTypeService.Models;

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
        public async Task<IEnumerable<NoteResponse>> GetNotes()
        {
            await notesService.UpdateNoteStatus();
            var data = await notesService.GetNotes();
            var result = mapper.Map<IEnumerable<NoteResponse>>(data);
            return result;
        }

        [HttpGet("get-last-four-weeks-completed")]
        public async Task<Dictionary<string, IEnumerable<NoteResponse>>> GetCompletedTaskForLastFourWeeks()
        {
            await notesService.UpdateNoteStatus();
            var data = await notesService.GetCompletedTaskForLastFourWeeksDictionary();
            var result = new Dictionary<string, IEnumerable<NoteResponse>>();
            foreach (var p in data)
            {
                result.Add(p.Key, mapper.Map<IEnumerable<NoteResponse>>(p.Value));
            }
            return result;
        }


        [HttpGet("{id}")]
        public async Task<NoteResponse> GetNoteById([FromRoute] int id)
        {
            var data = await notesService.GetNoteById(id);
            var result = mapper.Map<NoteResponse>(data);
            return result;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddNote([FromBody] NoteRequest note)
        {
            var data = mapper.Map<NoteRequestModel>(note);
            await notesService.AddNote(data);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNote([FromRoute] int id, [FromBody] NoteRequest type)
        {
            var data = mapper.Map<NoteRequestModel>(type);
            await notesService.UpdateNote(id, data);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNote([FromRoute] int id)
        {
            await notesService.DeleteNote(id);
            return Ok();
        }

    }
}
