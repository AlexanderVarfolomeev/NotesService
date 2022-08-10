using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notes.Context.Context;
using Notes.Entities;
using TaskStatus = Notes.Entities.TaskStatus;

namespace Notes.Context.Setup;
//TODO добавить юзера
public class DbSeed
{
    public static void Execute(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.GetService<IServiceScopeFactory>()?.CreateScope();
        ArgumentNullException.ThrowIfNull(scope);

        var factory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<MainDbContext>>();
        using var context = factory.CreateDbContext();

        //AddColors(context);
        //AddTaskTypes(context);
        //AddNotes(context);
    }

    private static void AddColors(MainDbContext context)
    {
        if (context.Colors.Any())
            return;

        var c1 = new TypeColor()
        {
            Code = "#FF0000",
            Name = "Red",
        };

        context.Colors.Add(c1);

        var c2 = new TypeColor()
        {
            Code = "#00FFFF",
            Name = "Aqua",
        };

        context.Colors.Add(c2);

        var c3 = new TypeColor()
        {
            Code = "#00FF00",
            Name = "Lime",
        };

        context.Colors.Add(c3);

        var c4 = new TypeColor()
        {
            Code = "#FFC0CB",
            Name = "Pink",
        };

        context.Colors.Add(c4);

        var c5 = new TypeColor()
        {
            Code = "#FFA500",
            Name = "Orange",
        };

        context.Colors.Add(c5);

        var c6 = new TypeColor()
        {
            Code = "#FFFF00",
            Name = "Yellow",
        };

        context.Colors.Add(c6);

        var c7 = new TypeColor()
        {
            Code = "#C0C0C0",
            Name = "Gray",
        };

        context.Colors.Add(c7);

        var c8 = new TypeColor()
        {
            Code = "#C71585",
            Name = "Violet",
        };

        context.Colors.Add(c8);

        var c9 = new TypeColor()
        {
            Code = "#D3D3D3",
            Name = "LightGray",
        };

        context.Colors.Add(c9);

        context.SaveChanges();

    }

    private static void AddTaskTypes(MainDbContext context)
    {
        if (context.Tasks.Any())
            return;

        var t1 = new TaskType()
        {
            Name = "Дом",
            TypeColorId = 1
        };

        context.Tasks.Add(t1);

        var t2 = new TaskType()
        {
            Name = "Работа",
            TypeColorId = 2
        };

        context.Tasks.Add(t2);

        var t3 = new TaskType()
        {
            Name = "Учеба",
            TypeColorId = 3
        };

        context.Tasks.Add(t3);

        var t4 = new TaskType()
        {
            Name = "Срочно",
            TypeColorId = 4
        };

        context.Tasks.Add(t4);

        var t5 = new TaskType()
        {
            Name = "Неважно",
            TypeColorId = 5
        };

        context.Tasks.Add(t5);

        context.SaveChanges();
    }

    private static void AddNotes(MainDbContext context)
    {
        if (context.Notes.Any())
            return;

        var n1 = new Note()
        {
            Name = "Помыть пол",
            StartDateTime = new DateTimeOffset(2022, 8, 12, 13, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 8, 12, 13, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Weekly,
            Status = TaskStatus.Failed,
            TaskTypeId = 1
        };

        context.Notes.Add(n1);

        var n2 = new Note()
        {
            Name = "Добавить кнопку на форму",
            StartDateTime = new DateTimeOffset(2022, 9, 18, 15, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 9, 18, 16, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.None,
            Status = TaskStatus.Waiting,
            TaskTypeId = 1
        };

        context.Notes.Add(n2);

        var n3 = new Note()
        {
            Name = "Позвонить родителям",
            StartDateTime = new DateTimeOffset(2022, 8, 13, 20, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 8, 13, 20, 15, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Daily,
            Status = TaskStatus.Waiting,
            TaskTypeId = 4
        };

        context.Notes.Add(n3);
        context.SaveChanges();
    }
}