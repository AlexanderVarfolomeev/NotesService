using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Common.Exceptions;
using Notes.Common.Validator;
using Notes.Context.Context;
using Notes.Entities;
using Notes.NotesService.Models;
using DateTime = System.DateTime;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.NotesService;

public class NotesService : INotesService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;
    private readonly IModelValidator<NoteRequestModel> modelValidator;
  

    public NotesService(IDbContextFactory<MainDbContext> contextFactory,
        IMapper mapper,
        IModelValidator<NoteRequestModel> modelValidator)
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
        this.modelValidator = modelValidator;
       
    }
    public async Task<IEnumerable<NoteModel>> GetNotes()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
            .Include(x => x.Type)
            .AsQueryable();
        var data = (await notes.ToListAsync()).Select(x => mapper.Map<NoteModel>(x));
        return data;
    }

    public async Task<NoteModel> GetNoteById(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
                .Include(x => x.Type)
                .AsQueryable();
        var note = notes
            .FirstOrDefault(x => x.Id == id);
        ProcessException.ThrowIf(() => note is null, "The task type with this ID was not found in the database");
        var data = mapper.Map<NoteModel>(note);
        return data;
    }

    public async Task AddNote(NoteRequestModel requestModel)
    {
        modelValidator.Check(requestModel);
        using var context = await contextFactory.CreateDbContextAsync();
        var note = mapper.Map<Note>(requestModel);
        await context.Notes.AddAsync(note);
        await context.SaveChangesAsync();
    }

    public async Task DeleteNote(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var note = context.Notes
            .FirstOrDefault(x => x.Id == id);
        ProcessException.ThrowIf(() => note is null, "The note with this ID was not found in the database");
        context.Notes.Remove(note);
        await context.SaveChangesAsync();
    }

    public async Task UpdateNote(int id, NoteRequestModel requestModel)
    {
        modelValidator.Check(requestModel);
        using var context = await contextFactory.CreateDbContextAsync();
        var note = context.Notes
            .FirstOrDefault(x => x.Id == id);
        ProcessException.ThrowIf(() => note is null, "The note with this ID was not found in the database");
        var data = mapper.Map(requestModel, note);
        context.Notes.Update(data);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<NoteModel>> GetCompletedTaskForLastFourWeeks()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
            .Include(x => x.Type)
            .AsQueryable();
        var data = (await notes.ToListAsync()).Select(x => mapper.Map<NoteModel>(x));
        var result = data.Select(x => x).Where(x =>IncludeInLastFourWeek(x.StartDateTime.LocalDateTime) && x.Status == TaskStatus.Done).ToList();
        return result;
    }

    /// <summary>
    /// Changing the status of failed tasks and create the next notes
    /// </summary>
    /// <returns></returns>
    public async Task UpdateNoteStatus()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
            .Include(x => x.Type)
            .AsQueryable()
            .ToList();

        foreach (var note in notes)
        {
            if (note.EndDateTime < DateTime.Now && note.Status != TaskStatus.Failed)
            {
                if (note.Status != TaskStatus.Done)
                {
                    note.Status = TaskStatus.Failed;
                    context.Entry(note).State = EntityState.Modified;
                    await context.SaveChangesAsync();
                }

                if (note.RepeatFrequency != RepeatFrequency.None)
                {
                    var newNote = CreateNextNoteFromRepeat(note);
                    newNote.Status = TaskStatus.Waiting;
                    newNote.Id = 0;
                    await context.Notes.AddAsync(newNote);
                }
                await context.SaveChangesAsync();
            }
        }
    }

    private Note CreateNextNoteFromRepeat(Note note)
    {
        switch (note.RepeatFrequency)
        {
            case RepeatFrequency.Daily:
                note.StartDateTime = note.StartDateTime.AddDays(1);
                note.EndDateTime = note.EndDateTime.AddDays(1);
                break;
            case RepeatFrequency.Weekly:
                note.StartDateTime = note.StartDateTime.AddDays(7);
                note.EndDateTime = note.EndDateTime.AddDays(7);
                break;
            case RepeatFrequency.Monthly:
                note.StartDateTime = note.StartDateTime.AddMonths(1);
                note.EndDateTime = note.EndDateTime.AddMonths(1);
                break;
            case RepeatFrequency.Annually:
                note.StartDateTime = note.StartDateTime.AddYears(1);
                note.EndDateTime = note.EndDateTime.AddYears(1);
                break;
        }

        return note;
    }

    /// <summary>
    /// Checking that the date is for the last 4 weeks
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    private bool IncludeInLastFourWeek(DateTime date)
    {
        var today = DateTime.Now.DayOfWeek;
        var startDate = DateTime.Now.AddDays(-21 - (int)today);
        return date >= startDate && date <= DateTime.Now;
    }
}