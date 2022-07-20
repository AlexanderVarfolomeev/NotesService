using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notes.Context.Context;
using Notes.Entities;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.Context.Setup;

public class DbSeed
{
    public static void Execute(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>();
        using var context = factory.CreateDbContext();

        AddData(context);
    }

    private static void AddData(MainDbContext context)
    {
        if (context.Tasks.Any() || context.Notes.Any())
            return;

        var type1 = new TaskType()
        {
            Name = "Дом",
            Color = ColorTaskType.Green
        };

        context.Tasks.Add(type1);

        var type2 = new TaskType()
        {
            Name = "Учеба",
            Color = ColorTaskType.Red
        };

        context.Tasks.Add(type2);

        var note1 = new Note()
        {
            Name = "Уборка",
            Description = "Вымыть пол",
            Status = TaskStatus.Waiting,
            StartDateTime = new DateTime(2022, 7, 10, 15, 30, 0),
            EndDateTime = new DateTime(2022, 7, 10, 16, 30, 0),
            RepetitionRate = RepetitionRate.Week,
            TaskTypeId = 1
        };

        context.Notes.Add(note1);

        var note2 = new Note()
        {
            Name = "Работа",
            Description = "",
            Status = TaskStatus.Waiting,
            StartDateTime = new DateTime(2022, 7, 10, 15, 30, 0),
            EndDateTime = new DateTime(2022, 7, 10, 16, 30, 0),
            RepetitionRate = RepetitionRate.Day,
            TaskTypeId = 2
        };

        context.Notes.Add(note2);

        context.SaveChanges();

    }
}