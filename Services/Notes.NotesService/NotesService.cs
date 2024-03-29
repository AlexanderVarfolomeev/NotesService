﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Common.Exceptions;
using Notes.Common.Validator;
using Notes.Context.Context;
using Notes.Entities;
using Notes.NotesService.Models;
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

    public async Task<Dictionary<string, double[]>> GetCompletedTaskForLastFourWeeksDictionary()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes
            .Include(x => x.Type)
            .Include(x => x.Type.Color)
            .AsQueryable();

        var result = (await notes.ToListAsync())
            .Where(x => IncludeInLastFourWeek(x.StartDateTime.LocalDateTime) && x.Status == TaskStatus.Done &&
                        x.Type.Name != "Empty").Select(x => mapper.Map<NoteModel>(x)).ToList();

        var dateTimeNow = DateTimeOffset.Now;
        var startDate = dateTimeNow.AddDays(-21 - ((int)dateTimeNow.DayOfWeek == 0 ? 7 : (int)dateTimeNow.DayOfWeek) + 1);

        List<IEnumerable<NoteModel>> list = new List<IEnumerable<NoteModel>>();

        for (int i = 0; i < 4; i++)
        {
            var start = startDate.AddDays(7 * i);
            var end = i == 3 ? dateTimeNow : start.AddDays(6);

            list.Add(result.Where(x => IncludeInInterval(start, end, x.StartDateTime)));
        }

        Dictionary<string, double[]> dictionary = new Dictionary<string, double[]>();
        int j = 0;
        foreach (var week in list)
        {
            foreach (var note in week)
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
        Note newNote = new Note()
        {
            Description = note.Description,
            EndDateTime = note.EndDateTime,
            Status = note.Status,
            StartDateTime = note.StartDateTime,
            Name = note.Name,
            RepeatFrequency = note.RepeatFrequency,
            TaskTypeId = note.TaskTypeId,
            Type = note.Type
        };
        switch (newNote.RepeatFrequency)
        {
            case RepeatFrequency.Daily:
                newNote.StartDateTime = newNote.StartDateTime.AddDays(1);
                newNote.EndDateTime = newNote.EndDateTime.AddDays(1);
                break;
            case RepeatFrequency.Weekly:
                newNote.StartDateTime = newNote.StartDateTime.AddDays(7);
                newNote.EndDateTime = newNote.EndDateTime.AddDays(7);
                break;
            case RepeatFrequency.Monthly:
                newNote.StartDateTime = newNote.StartDateTime.AddMonths(1);
                newNote.EndDateTime = newNote.EndDateTime.AddMonths(1);
                break;
            case RepeatFrequency.Annually:
                newNote.StartDateTime = newNote.StartDateTime.AddYears(1);
                newNote.EndDateTime = newNote.EndDateTime.AddYears(1);
                break;
        }
        return newNote;
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
        var startDate = dateTimeNow.AddDays(-21 - ((int)today == 0 ? 7 : (int)today) + 1);
        return date.Date >= startDate.Date && date.Date <= dateTimeNow.Date;
    }

    private bool IncludeInInterval(DateTimeOffset startDate, DateTimeOffset endDate, DateTimeOffset current)
    {
        return current.Date >= startDate.Date && current.Date <= endDate.Date;
    }
}