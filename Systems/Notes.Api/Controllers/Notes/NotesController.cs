using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Notes.Api.Controllers.Notes.Models;
using Notes.NotesService;
using Notes.NotesService.Models;

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
        public async Task<Dictionary<string, double[]>> GetCompletedTaskForLastFourWeeks()
        {
            await notesService.UpdateNoteStatus();
            var data = await notesService.GetCompletedTaskForLastFourWeeksDictionary();
            return data;
        }

        [HttpGet("get-in-interval-{start}-{end}")]
        public async Task<IEnumerable<NoteResponse>> GetNotesInInterval([FromRoute]DateTimeOffset start, [FromRoute]DateTimeOffset end)
        {
            await notesService.UpdateNoteStatus();
            var data = await notesService.GetNotesInInterval(start.ToUniversalTime(), end.ToUniversalTime());
            var result = mapper.Map<IEnumerable<NoteResponse>>(data);
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

        [HttpPut("do-task-{id}")]
        public async Task<IActionResult> DoTask([FromRoute] int id)
        {
            await notesService.DoTask(id);
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
