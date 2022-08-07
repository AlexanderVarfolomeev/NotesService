using AutoMapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        await using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
            .Include(x => x.Type)
            .Include(x => x.Type.Color)
            .AsQueryable();
        var data = (await notes.ToListAsync()).Select(x => mapper.Map<NoteModel>(x));
        return data;
    }

    public async Task<NoteModel> GetNoteById(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
                .Include(x => x.Type)
                .Include(x => x.Type.Color)
                .AsQueryable();
        var note = notes
            .FirstOrDefault(x => x.Id == id);
        ProcessException.ThrowIf(() => note is null, "The note with this ID was not found in the database");
        var data = mapper.Map<NoteModel>(note);
        return data;
    }

    public async Task AddNote(NoteRequestModel requestModel)
    {
        modelValidator.Check(requestModel);
        await using var context = await contextFactory.CreateDbContextAsync();
        var note = mapper.Map<Note>(requestModel);
        await context.Notes.AddAsync(note);
        await context.SaveChangesAsync();
    }

    public async Task DeleteNote(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var note = context.Notes
            .FirstOrDefault(x => x.Id == id);
        ProcessException.ThrowIf(() => note is null, "The note with this ID was not found in the database");
        context.Notes.Remove(note);
        await context.SaveChangesAsync();
    }

    public async Task UpdateNote(int id, NoteRequestModel requestModel)
    {
        modelValidator.Check(requestModel);
        await using var context = await contextFactory.CreateDbContextAsync();
        var note = context.Notes
            .FirstOrDefault(x => x.Id == id);
        ProcessException.ThrowIf(() => note is null, "The note with this ID was not found in the database");
        var data = mapper.Map(requestModel, note);
        context.Notes.Update(data);
        await context.SaveChangesAsync();
    }

    //TODO разбить на методы?
    public async Task<Dictionary<string, double[]>> GetCompletedTaskForLastFourWeeksDictionary()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
            .Include(x => x.Type)
            .Include(x => x.Type.Color)
            .AsQueryable();
        var data = (await notes.ToListAsync()).Select(x => mapper.Map<NoteModel>(x));

        var result = data.Select(x => x)
            .Where(x => IncludeInLastFourWeek(x.StartDateTime.LocalDateTime) && x.Status == TaskStatus.Done).ToList();

        Dictionary<string, IEnumerable<NoteModel>> resultDictionary = new Dictionary<string, IEnumerable<NoteModel>>();

        var dateTimeNow = DateTimeOffset.Now;
        var startDate = dateTimeNow.AddDays(-21 - (int)dateTimeNow.DayOfWeek);

        for (int i = 0; i < 4; i++)
        {
            var start = startDate.AddDays(7 * i);
            var end = i == 3 ? dateTimeNow : startDate.AddDays(7 * (i + 1));

            var startStr = start.Day + "." + start.Month;
            var endStr = end.Day + "." + end.Month;

            resultDictionary.Add(startStr + " - " + endStr,
                result.Where(x => IncludeInInterval(start, end, x.StartDateTime)));
        }

        Dictionary<string, double[]> dictionary = new Dictionary<string, double[]>();
        int j = 0;
        foreach (var week in resultDictionary)
        {
            foreach (var note in week.Value)
            {
                if (!dictionary.ContainsKey(note.Type))
                    dictionary.Add(note.Type, new double[4] { 0, 0, 0, 0 });
                var hours = (note.EndDateTime - note.StartDateTime).TotalHours;
                dictionary[note.Type][j] += hours;
            }

            j++;
        }

        foreach (var p in dictionary)
        {
            for (int i = 0; i < 4; i++)
            {
                p.Value[i] = Math.Round(p.Value[i], 1);
            }
        }
        return dictionary;
    }


    public async Task<IEnumerable<NoteModel>> GetNotesInInterval(DateTimeOffset start, DateTimeOffset end)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
            .Include(x => x.Type)
            .Include(x => x.Type.Color)
            .AsQueryable();
        var result = (await notes.ToListAsync()).Select(x => mapper.Map<NoteModel>(x));
        var data = result.Where(x => IncludeInInterval(start, end, x.StartDateTime.LocalDateTime));

        return data;
    }

    public async Task DoTask(int id)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var note = context.Notes
            .FirstOrDefault(x => x.Id == id);
        ProcessException.ThrowIf(() => note is null, "The note with this ID was not found in the database");
        var data = note;
        data.Status = TaskStatus.Done;
        context.Notes.Update(data);
        await context.SaveChangesAsync();

        await AddNextNote(note);
    }

    private async Task AddNextNote(Note note)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        if (note.RepeatFrequency != RepeatFrequency.None)
        {
            var newNote = CreateNextNoteFromRepeat(note);
            newNote.Status = TaskStatus.Waiting;
            newNote.Id = 0;
            newNote.Type = null;
            await context.Notes.AddAsync(newNote);
            await context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Changing the status of failed tasks and create the next notes
    /// </summary>
    /// <returns></returns>
    public async Task UpdateNoteStatus()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
            .Include(x => x.Type)
            .AsQueryable()
            .ToList();

        foreach (var note in notes)
        {
            if (note.EndDateTime < DateTimeOffset.Now && note.Status == TaskStatus.Waiting)
            {
                note.Status = TaskStatus.Failed;
                context.Entry(note).State = EntityState.Modified;
                await context.SaveChangesAsync();

                await AddNextNote(note);
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
    private bool IncludeInLastFourWeek(DateTimeOffset date)
    {
        var dateTimeNow = DateTimeOffset.Now;
        var today = dateTimeNow.DayOfWeek;
        var startDate = dateTimeNow.AddDays(-21 - (int)today);
        return date >= startDate && date <= dateTimeNow;
    }

    private bool IncludeInInterval(DateTimeOffset startDate, DateTimeOffset endDate, DateTimeOffset current)
    {
        return current.Date >= startDate.Date && current.Date <= endDate.Date;
    }
}