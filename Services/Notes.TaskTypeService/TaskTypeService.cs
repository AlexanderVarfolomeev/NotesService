using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.ColorService;
using Notes.Common.Exceptions;
using Notes.Common.Validator;
using Notes.Context.Context;
using Notes.Entities;
using Notes.NotesService;
using Notes.TaskTypeService.Models;

namespace Notes.TaskTypeService;

public class TaskTypeService : ITaskTypeService
{
    private readonly IMapper mapper;
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IModelValidator<TaskTypeRequestModel> typeModelValidator;
    private readonly INotesService notesService;
    private readonly IColorService colorService;
    public TaskTypeService(IMapper mapper,
        IDbContextFactory<MainDbContext> contextFactory,
        IModelValidator<TaskTypeRequestModel> typeModelValidator, INotesService notesService, IColorService colorService)
    {
        this.mapper = mapper;
        this.contextFactory = contextFactory;
        this.typeModelValidator = typeModelValidator;
        this.notesService = notesService;
        this.colorService = colorService;
    }

    public async Task<IEnumerable<TaskTypeModel>> GetTaskTypes()
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var types = context.Tasks
            .Include(x => x.Color)
            .AsQueryable();
        var data = (await types.ToListAsync()).Select(x => mapper.Map<TaskTypeModel>(x));
        return data;
    }

    public async Task<TaskTypeModel> GetTaskById(int taskId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var types = context.Tasks
            .Include(x => x.Color)
            .AsQueryable();

        var type = types
            .FirstOrDefault(x => x.Id == taskId);
        ProcessException.ThrowIf(() => type is null, "The task type with this ID was not found in the database");
        var data = mapper.Map<TaskTypeModel>(type);
        return data;
    }

    public async Task AddTask(TaskTypeRequestModel task)
    {
        typeModelValidator.Check(task);
        await using var context = await contextFactory.CreateDbContextAsync();
        var type = mapper.Map<TaskType>(task);
        await context.Tasks.AddAsync(type);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTask(TaskTypeRequestModel task, int taskId)
    {
        typeModelValidator.Check(task);
        await using var context = await contextFactory.CreateDbContextAsync();
        var type = context.Tasks
            .FirstOrDefault(x => x.Id == taskId);
        ProcessException.ThrowIf(() => type is null, "The task type with this ID was not found in the database");
        var data = mapper.Map(task, type);
        context.Tasks.Update(data);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTask(int taskId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var type = context.Tasks
            .FirstOrDefault(x => x.Id == taskId);
        ProcessException.ThrowIf(() => type is null, "The task type with this ID was not found in the database");
        ProcessException.ThrowIf(() => type.Name == "Empty", "An empty task type cannot be deleted.");

        await UpdateNotesAfterDeleteTaskType(taskId);

        context.Tasks.Remove(type);
        await context.SaveChangesAsync();

    }

    private async Task UpdateNotesAfterDeleteTaskType(int taskId)
    {
        await using var context = await contextFactory.CreateDbContextAsync();
        var notes = context.Notes.ToList();

        var emptyTask = context.Tasks.FirstOrDefault(x => x.Name == "Empty");

        if (emptyTask is null)
        {
            var lightGrayColor = context.Colors.FirstOrDefault(x => x.Name == "LightGray");
            if (lightGrayColor is null)
            {
                var lightColor = new Notes.Entities.TypeColor()
                {
                    Name = "LightGray",
                    Code = "#D3D3D3"
                };
                await context.Colors.AddAsync(lightColor);
                await context.SaveChangesAsync();
                lightGrayColor = context.Colors.FirstOrDefault(x => x.Name == "LightGray");
            }

            TaskTypeRequestModel empty = new TaskTypeRequestModel()
            {
                TypeColorId = lightGrayColor.Id,
                Name = "Empty"
            };
            var type = mapper.Map<TaskType>(empty);
            await context.Tasks.AddAsync(type);
            await context.SaveChangesAsync();

            emptyTask = context.Tasks.FirstOrDefault(x => x.Name == "Empty");
        }

        var emptyTypeNotes = notes.Where(x => x.TaskTypeId == taskId).ToList();
        foreach (var emptyTypeNote in emptyTypeNotes)
        {
            emptyTypeNote.TaskTypeId = emptyTask.Id;
            context.Notes.Update(emptyTypeNote);
            await context.SaveChangesAsync();
        }
    }
}