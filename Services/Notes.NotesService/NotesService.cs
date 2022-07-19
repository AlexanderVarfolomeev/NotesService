using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Context.Context;
using Notes.Entities;
using Notes.NotesService.Models;

namespace Notes.NotesService;

public class NotesService : INotesService
{
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IMapper mapper;

    public NotesService(IDbContextFactory<MainDbContext> contextFactory, IMapper mapper)
    {
        this.contextFactory = contextFactory;
        this.mapper = mapper;
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

        var data = mapper.Map<NoteModel>(note);
        return data;
    }

    public async Task AddNote(AddNoteModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var note = mapper.Map<Note>(model);
        await context.Notes.AddAsync(note);
        await context.SaveChangesAsync();
    }

    public async Task DeleteNote(int id)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var note = context.Notes
            .FirstOrDefault(x => x.Id == id);
        if (note == null)
            throw new NotImplementedException();
        context.Notes.Remove(note);
        await context.SaveChangesAsync();
    }

    public async Task UpdateNote(int id, UpdateNoteModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var note = context.Notes
            .FirstOrDefault(x => x.Id == id);
        if (note == null)
            throw new NotImplementedException();
        var data = mapper.Map(model, note);
        context.Notes.Update(data);
        await context.SaveChangesAsync();
    }
}