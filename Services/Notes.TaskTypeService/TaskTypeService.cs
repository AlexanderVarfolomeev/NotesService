using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Common.Exceptions;
using Notes.Common.Validator;
using Notes.Context.Context;
using Notes.Context.Factories;
using Notes.Entities;
using Notes.TaskTypeService.Models;

namespace Notes.TaskTypeService;

public class TaskTypeService : ITaskTypeService
{
    private readonly IMapper mapper;
    private readonly IDbContextFactory<MainDbContext> contextFactory;
    private readonly IModelValidator<TaskTypeRequestModel> typeModelValidator;

    public TaskTypeService(IMapper mapper,
        IDbContextFactory<MainDbContext> contextFactory,
        IModelValidator<TaskTypeRequestModel> typeModelValidator)
    {
        this.mapper = mapper;
        this.contextFactory = contextFactory;
        this.typeModelValidator = typeModelValidator;
    }

    public async Task<IEnumerable<TaskTypeModel>> GetTaskTypes()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var types = context.Tasks
            .Include(x => x.Color)
            .AsQueryable();
        var data = (await types.ToListAsync()).Select(x => mapper.Map<TaskTypeModel>(x));
        return data;
    }

    public async Task<TaskTypeModel> GetTaskById(int taskId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
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
        using var context = await contextFactory.CreateDbContextAsync();
        var type = mapper.Map<TaskType>(task);
        await context.Tasks.AddAsync(type);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTask(TaskTypeRequestModel task, int taskId)
    {
        typeModelValidator.Check(task);
        using var context = await contextFactory.CreateDbContextAsync();
        var type = context.Tasks
            .FirstOrDefault(x => x.Id == taskId);
        ProcessException.ThrowIf(() => type is null, "The task type with this ID was not found in the database");
        var data = mapper.Map(task, type);
        context.Tasks.Update(data);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTask(int taskId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var type = context.Tasks
            .FirstOrDefault(x => x.Id == taskId);
        ProcessException.ThrowIf(() => type is null, "The task type with this ID was not found in the database");
        context.Tasks.Remove(type);
        await context.SaveChangesAsync();
    }
}