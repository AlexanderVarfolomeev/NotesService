using Microsoft.AspNetCore.Identity;
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
        if (context.Colors.Any() || context.Tasks.Any() || context.Notes.Any())
            return;
        AddColors(context);
        AddTaskTypes(context);
        AddNotes(context);
    }

    private static void AddColors(MainDbContext context)
    {
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

        var t6 = new TaskType()
        {
            Name = "Спорт",
            TypeColorId = 6
        };

        context.Tasks.Add(t6);

        context.SaveChanges();
    }

    private static void AddNotes(MainDbContext context)
    {

        var n = new Note()
        {
            Name = "Убраться",
            StartDateTime = new DateTimeOffset(2022, 12, 28, 13, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, 28, 13, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Daily,
            Status = TaskStatus.Failed,
            TaskTypeId = 1
        };

        context.Notes.Add(n);

        int day = 6;
        for (int i = 0; i < 16; i++)
        {
            n = new Note()
            {
                Name = "Убраться",
                StartDateTime = new DateTimeOffset(2022, 12, day++, 13, 0, 0, TimeSpan.Zero),
                EndDateTime = new DateTimeOffset(2022, 12, day, 13, 30, 0, TimeSpan.Zero),
                RepeatFrequency = RepeatFrequency.Daily,
                Status = TaskStatus.Done,
                TaskTypeId = 1
            };

            context.Notes.Add(n);
        }

        day = 1;
        for (int i = 0; i < 25; i++)
        {
            n = new Note()
            {
                Name = "Сделать дз",
                StartDateTime = new DateTimeOffset(2022, 12, day++, 15, 0, 0, TimeSpan.Zero),
                EndDateTime = new DateTimeOffset(2022, 12, day, 15, 30, 0, TimeSpan.Zero),
                RepeatFrequency = RepeatFrequency.Daily,
                Status = TaskStatus.Done,
                TaskTypeId = 3
            };

            context.Notes.Add(n);
        }

        day = 1;
        for (int i = 0; i < 25; i++)
        {
            n = new Note()
            {
                Name = "Созвон",
                StartDateTime = new DateTimeOffset(2022, 12, day++, 19, 0, 0, TimeSpan.Zero),
                EndDateTime = new DateTimeOffset(2022, 12, day, 20, 30, 0, TimeSpan.Zero),
                RepeatFrequency = RepeatFrequency.Daily,
                Status = TaskStatus.Done,
                TaskTypeId = 2
            };

            context.Notes.Add(n);
        }

        n = new Note()
        {
            Name = "Поздравить леру с др",
            StartDateTime = new DateTimeOffset(2022, 12, 27, 11, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, 27, 11, 10, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Annually,
            Status = TaskStatus.Done,
            TaskTypeId = 4
        };

        context.Notes.Add(n);

        n = new Note()
        {
            Name = "Поздравить сашу с др",
            StartDateTime = new DateTimeOffset(2022, 12, 28, 11, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, 28, 11, 10, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Annually,
            Status = TaskStatus.Done,
            TaskTypeId = 4
        };

        context.Notes.Add(n);


        n = new Note()
        {
            Name = "Встреча с максом",
            StartDateTime = new DateTimeOffset(2022, 12, 29, 11, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, 29, 16, 0, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Annually,
            Status = TaskStatus.Waiting,
            TaskTypeId = 2
        };

        context.Notes.Add(n);


        day = 1;
        for (int i = 0; i < 14; i++)
        {
            n = new Note()
            {
                Name = "Убраться",
                StartDateTime = new DateTimeOffset(2022, 12, day++, 13, 0, 0, TimeSpan.Zero),
                EndDateTime = new DateTimeOffset(2022, 12, day, 13, 30, 0, TimeSpan.Zero),
                RepeatFrequency = RepeatFrequency.Daily,
                Status = TaskStatus.Done,
                TaskTypeId = 1
            };

            context.Notes.Add(n);
        }

        day = 1;
        for (int i = 0; i < 14; i++)
        {
            n = new Note()
            {
                Name = "Сделать дз",
                StartDateTime = new DateTimeOffset(2022, 12, day++, 15, 0, 0, TimeSpan.Zero),
                EndDateTime = new DateTimeOffset(2022, 12, day, 15, 30, 0, TimeSpan.Zero),
                RepeatFrequency = RepeatFrequency.Daily,
                Status = TaskStatus.Done,
                TaskTypeId = 3
            };

            context.Notes.Add(n);
        }

        day = 1;
        for (int i = 0; i < 14; i++)
        {
            n = new Note()
            {
                Name = "Созвон",
                StartDateTime = new DateTimeOffset(2022, 12, day++, 19, 0, 0, TimeSpan.Zero),
                EndDateTime = new DateTimeOffset(2022, 12, day, 20, 30, 0, TimeSpan.Zero),
                RepeatFrequency = RepeatFrequency.Daily,
                Status = TaskStatus.Done,
                TaskTypeId = 2
            };

            context.Notes.Add(n);
        }

        day = 15;
        for (int i = 0; i < 14; i++)
        {
            n = new Note()
            {
                Name = "Поход в зал",
                StartDateTime = new DateTimeOffset(2022, 12, day++, 9, 0, 0, TimeSpan.Zero),
                EndDateTime = new DateTimeOffset(2022, 12, day, 12, 30, 0, TimeSpan.Zero),
                RepeatFrequency = RepeatFrequency.Daily,
                Status = TaskStatus.Done,
                TaskTypeId = 5
            };

            context.Notes.Add(n);
        }


        context.Notes.Add(new Note()
        {
            Name = "Поход в зал",
            StartDateTime = new DateTimeOffset(2022, 12, DateTime.Today.Day + 1, 9, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, DateTime.Today.Day + 1, 12, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Daily,
            Status = TaskStatus.Waiting,
            TaskTypeId = 5
        });

        n = new Note()
        {
            Name = "Созвон",
            StartDateTime = new DateTimeOffset(2022, 12, DateTime.Today.Day + 1, 19, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, DateTime.Today.Day + 1, 20, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Daily,
            Status = TaskStatus.Waiting,
            TaskTypeId = 2
        };

        context.Notes.Add(n);

        n = new Note()
        {
            Name = "Поход в бассейн",
            StartDateTime = new DateTimeOffset(2022, 12, 14, 13, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, 14, 13, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Weekly,
            Status = TaskStatus.Done,
            TaskTypeId = 5
        };
        context.Notes.Add(n);

        n = new Note()
        {
            Name = "Поход в бассейн",
            StartDateTime = new DateTimeOffset(2022, 12, 21, 13, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, 21, 13, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Weekly,
            Status = TaskStatus.Done,
            TaskTypeId = 5
        };
        context.Notes.Add(n);

        n = new Note()
        {
            Name = "Поход в бассейн",
            StartDateTime = new DateTimeOffset(2022, 12, 28, 13, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, 28, 13, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Weekly,
            Status = TaskStatus.Failed,
            TaskTypeId = 5
        };
        context.Notes.Add(n);


        n = new Note()
        {
            Name = "Поход в бассейн",
            StartDateTime = new DateTimeOffset(2022, 12, 4, 13, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, 4, 13, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Weekly,
            Status = TaskStatus.Done,
            TaskTypeId = 5
        };
        context.Notes.Add(n);

        n = new Note()
        {
            Name = "Поход в бассейн",
            StartDateTime = new DateTimeOffset(2022, 12,11, 13, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12,11, 13, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Weekly,
            Status = TaskStatus.Done,
            TaskTypeId = 5
        };

        context.Notes.Add(n);
        n = new Note()
        {
            Name = "Поход в бассейн",
            StartDateTime = new DateTimeOffset(2022, 12, 18, 13, 0, 0, TimeSpan.Zero),
            EndDateTime = new DateTimeOffset(2022, 12, 18, 13, 30, 0, TimeSpan.Zero),
            RepeatFrequency = RepeatFrequency.Weekly,
            Status = TaskStatus.Waiting,
            TaskTypeId = 5
        };
        context.Notes.Add(n);


        context.SaveChanges();
    }
}