﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notes.Context.Context;
using Notes.Context.Factories;
using Notes.Entities;
using Notes.TaskTypeService.Models;

namespace Notes.TaskTypeService;

public class TaskTypeService : ITaskTypeService
{
    private readonly IMapper mapper;
    private readonly IDbContextFactory<MainDbContext> contextFactory;

    public TaskTypeService(IMapper mapper, IDbContextFactory<MainDbContext> contextFactory)
    {
        this.mapper = mapper;
        this.contextFactory = contextFactory;
    }

    public async Task<IEnumerable<TaskTypeModel>> GetTaskTypes()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var types = context.Tasks
            .AsQueryable();
        var data = (await types.ToListAsync()).Select(x => mapper.Map<TaskTypeModel>(x));
        return data;
    }

    public async Task<TaskTypeModel> GetTaskById(int taskId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var type = context.Tasks
            .Where(x => x.Id == taskId);

        var data = mapper.Map<TaskTypeModel>(type);
        return data;
    }

    public async Task AddTask(TaskTypeAddModel task)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var type = mapper.Map<TaskType>(task);
        await context.Tasks.AddAsync(type);
        await context.SaveChangesAsync();
    }

    public async Task UpdateTask(TaskTypeUpdateModel task, int taskId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var type = context.Tasks
            .FirstOrDefault(x => x.Id == taskId);
        if (type == null)
            throw new NotImplementedException();
        var data = mapper.Map(task, type);
        context.Tasks.Update(data);
        await context.SaveChangesAsync();
    }

    public async Task DeleteTask(int taskId)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var type = context.Tasks
            .FirstOrDefault(x => x.Id == taskId);
        if(type == null)
            throw new NotImplementedException();
        context.Tasks.Remove(type);
        await context.SaveChangesAsync();
    }
}