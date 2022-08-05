using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml.Serialization;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notes.WPF.Infrastructure.Commands;
using Notes.WPF.Models.Notes;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Colors;
using Notes.WPF.Services.Colors.Models;
using Notes.WPF.Services.Notes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.Services.UserDialog;
using SkiaSharp;
using TaskStatus = Notes.WPF.Models.Notes.TaskStatus;

namespace Notes.WPF.ViewModels;
//TODO добавить поддержку Dependency injection
//TODO вряди ли тут должно находится так много логики, мб перенести
public partial class MainWindowViewModel : ObservableObject
{
    private readonly ITaskTypeService _taskTypeService;
    private readonly IColorService _colorService;
    private readonly INotesService _notesService;
    private readonly IUserDialogService _userDialogService;
    public MainWindowViewModel()
    {
        _colorService = new ColorService();
        _notesService = new NotesService();
        _taskTypeService = new TaskTypeService();
        _userDialogService = new UserDialogService();

        currentMonday = GetDateOfMondayOnThisWeek();

        DeleteTaskTypeCommand = new LambdaCommand(OnDeleteTaskTypeExecuted, CanDeleteTaskTypeExecute);
    }

    [ObservableProperty]
    private ObservableCollection<TaskType> _taskTypes;

    [ObservableProperty]
    private ObservableCollection<ColorResponse> _colors;

    [ObservableProperty] private ColorResponse? _selectedColor;

    [ObservableProperty]
    private TaskType? _selectedType;

    [ObservableProperty] private EditTaskType? _editType;

    [ObservableProperty]
    private ObservableCollection<Note> _currentWeekNotes;

    [ObservableProperty]
    private Note? _selectedNote;

    [ObservableProperty] private bool _isEditType;

    [RelayCommand]
    private async void RefreshData(object p)
    {
        await RepetitionNotesInit();
        Colors = new ObservableCollection<ColorResponse>(await _colorService.GetColors());
        TaskTypes = new ObservableCollection<TaskType>(await _taskTypeService.GetTaskTypes());
        //TODO убрать отсюда
        CurrentWeekNotes = new ObservableCollection<Note>((await _notesService
                .GetNotesInInterval(DateTimeOffset.Now.Date, currentMonday.AddDays(6)))
            .OrderBy(x => x.StartDateTime.Hour));
        await RefreshLastFourWeeksNotes();
        RefreshNotes();
    }

    #region Delete Task Type 

    public ICommand DeleteTaskTypeCommand { get; }
    private bool CanDeleteTaskTypeExecute(object p) => SelectedType != null;
    private async void OnDeleteTaskTypeExecuted(object p)
    {
        if (p is TaskType taskType)
        {
            await _taskTypeService.DeleteTask(taskType.Id);
            _taskTypes.Remove(taskType);
        }
    }

    #endregion

    [RelayCommand]
    private async void AddTaskType(object p)
    {
        IsEditType = false;
        EditType = new EditTaskType();
        SelectedColor = null;
        if (await _userDialogService.Edit(EditType))
        {
            EditType.TypeColorId = SelectedColor.Id;
            await _taskTypeService.AddTask(EditType);
            RefreshData(new object());
        }
    }

    #region Edit task type

     private bool CanEditTaskTypeExecute() => SelectedType != null;
    [RelayCommand(CanExecute = nameof(CanEditTaskTypeExecute))]
    private async void EditTaskType(object p)
    {
        if (p is TaskType taskType)
        {
            IsEditType = true;
            EditType = new EditTaskType()
            {
                Name = SelectedType.Name,
                TypeColorId = SelectedType.TypeColorId
            };
            SelectedColor = Colors.FirstOrDefault(x => x.Id == SelectedType.TypeColorId) ?? throw new ArgumentNullException();
            if (await _userDialogService.Edit(EditType))
            {
                EditType.TypeColorId = SelectedColor.Id;
                await _taskTypeService.UpdateTask(EditType, taskType.Id);
                RefreshData(new object());
            }

        }
    }


    #endregion

    #region Activity chart

    [ObservableProperty]
    private Dictionary<string, double[]> _completedNotesLastFourWeeks;

    [ObservableProperty] private ISeries[] _series;

    [ObservableProperty] private Axis[] _xAxes;
    //TODO не добавлять задачи на которые потрачено 0 минут
    private async Task RefreshLastFourWeeksNotes()
    {
        var dates = new string[4];
        var dateTimeNow = DateTimeOffset.Now;
        var startDate = dateTimeNow.AddDays(-21 - (int)dateTimeNow.DayOfWeek);
        for (int j = 0; j < 4; j++)
        {
            var start = startDate.AddDays(7 * j);
            var end = j == 3 ? dateTimeNow : startDate.AddDays(7 * (j + 1));
            var startStr = start.Day + "." + start.Month;
            var endStr = end.Day + "." + end.Month;
            dates[j] = startStr + " - " + endStr;
        }

        CompletedNotesLastFourWeeks = new Dictionary<string, double[]>(await _notesService.GetCompletedTaskForLastFourWeeks());
        XAxes = new Axis[]
        {
            new Axis(){
                Labels = dates.Select(x => x).ToArray(),
            }
        };

        Series = new ISeries[CompletedNotesLastFourWeeks.Count];
        int i = 0;
        foreach (var pair in CompletedNotesLastFourWeeks)
        {
            var rgb = GetRGBCodeFromTaskTypeName(pair.Key);
            Series[i] = new ColumnSeries<double>
            {
                Name = pair.Key,
                Values = pair.Value,
                Fill = new SolidColorPaint(new SKColor(rgb.Item1, rgb.Item2, rgb.Item3))
            };
            i++;
        }
    }


    #endregion
    private (byte, byte, byte) GetRGBCodeFromTaskTypeName(string name)
    {
        var type = TaskTypes.FirstOrDefault(x => x.Name == name);
        var color = type.Color.Code.Substring(1);
        return (Convert.ToByte(color.Substring(0, 2), 16),
            Convert.ToByte(color.Substring(2, 2), 16),
            Convert.ToByte(color.Substring(4, 2), 16));
    }
    private DateTimeOffset GetDateOfMondayOnThisWeek()
    {
        var today = DateTime.Today;
        switch (today.DayOfWeek)
        {
            default:
                return today;
            case DayOfWeek.Tuesday:
                return today.AddDays(-1);
            case DayOfWeek.Wednesday:
                return today.AddDays(-2);
            case DayOfWeek.Thursday:
                return today.AddDays(-3);
            case DayOfWeek.Friday:
                return today.AddDays(-4);
            case DayOfWeek.Saturday:
                return today.AddDays(-5);
            case DayOfWeek.Sunday:
                return today.AddDays(-6);

        }
    }

    [RelayCommand]
    private async Task GetNextWeek()
    {
        currentMonday = currentMonday.AddDays(7);
        CurrentWeekNotes =
            new ObservableCollection<Note>(
                await _notesService.GetNotesInInterval(currentMonday, currentMonday.AddDays(6)));
        RefreshNotes();
    }

    [RelayCommand]
    private async Task GetPreviousWeek()
    {
        currentMonday = currentMonday.AddDays(-7);
        CurrentWeekNotes =
            new ObservableCollection<Note>(
                await _notesService.GetNotesInInterval(currentMonday, currentMonday.AddDays(6)));
        RefreshNotes();
    }

    #region This week
    private DateTimeOffset currentMonday;

    [ObservableProperty] private Note _selectedNoteWeek;

    [ObservableProperty] private ObservableCollection<Note> _mondayNotes;
    [ObservableProperty] private ObservableCollection<Note> _tuesdayNotes;
    [ObservableProperty] private ObservableCollection<Note> _wednesdayNotes;
    [ObservableProperty] private ObservableCollection<Note> _thursdayNotes;
    [ObservableProperty] private ObservableCollection<Note> _fridayNotes;
    [ObservableProperty] private ObservableCollection<Note> _saturdayNotes;
    [ObservableProperty] private ObservableCollection<Note> _sundayNotes;


    private List<Note> _everyDayNotes;
    private List<Note> _everyWeekNotes;
    private List<Note> _everyMonthNotes;
    private List<Note> _everyYearNotes;

    private async Task RepetitionNotesInit()
    {
        _everyDayNotes = new List<Note>();
        _everyMonthNotes = new List<Note>();
        _everyWeekNotes = new List<Note>();
        _everyYearNotes = new List<Note>();

        var notes = (await _notesService.GetNotes()).Where(x => x.Status == TaskStatus.Waiting);
        foreach (var note in notes)
        {
            switch (note.RepeatFrequency)
            {
                case RepeatFrequency.Daily:
                    _everyDayNotes.Add(note);
                    break;
                case RepeatFrequency.Weekly:
                    _everyWeekNotes.Add(note);
                    break;
                case RepeatFrequency.Monthly:
                    _everyMonthNotes.Add(note);
                    break;
                case RepeatFrequency.Annually:
                    _everyYearNotes.Add(note);
                    break;
            }
        }
    }

    //TODO ужасный код, оптимизировать, мб занести каждый день в массив и работать с ним, разнести по методам
    //TODO исправить логику опредления заметки на следующий месяц
    private void RefreshNotes()
    {
        var today = DateTimeOffset.Now;
        MondayNotes = new ObservableCollection<Note>(CurrentWeekNotes
            .Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Monday &&
                        (x.RepeatFrequency == RepeatFrequency.None || x.Status == TaskStatus.Failed || x.Status == TaskStatus.Done)));
        TuesdayNotes = new ObservableCollection<Note>(CurrentWeekNotes
            .Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Tuesday &&
                        (x.RepeatFrequency == RepeatFrequency.None || x.Status == TaskStatus.Failed || x.Status == TaskStatus.Done)));
        WednesdayNotes = new ObservableCollection<Note>(CurrentWeekNotes
            .Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Wednesday &&
                        (x.RepeatFrequency == RepeatFrequency.None || x.Status == TaskStatus.Failed || x.Status == TaskStatus.Done)));
        ThursdayNotes = new ObservableCollection<Note>(CurrentWeekNotes
            .Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Thursday &&
                        (x.RepeatFrequency == RepeatFrequency.None || x.Status == TaskStatus.Failed || x.Status == TaskStatus.Done)));
        FridayNotes = new ObservableCollection<Note>(CurrentWeekNotes
            .Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Friday &&
                        (x.RepeatFrequency == RepeatFrequency.None || x.Status == TaskStatus.Failed || x.Status == TaskStatus.Done)));
        SaturdayNotes = new ObservableCollection<Note>(CurrentWeekNotes
            .Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Saturday &&
                        (x.RepeatFrequency == RepeatFrequency.None || x.Status == TaskStatus.Failed || x.Status == TaskStatus.Done)));
        SundayNotes = new ObservableCollection<Note>(CurrentWeekNotes
            .Where(x => x.StartDateTime.DayOfWeek == DayOfWeek.Sunday &&
                        (x.RepeatFrequency == RepeatFrequency.None || x.Status == TaskStatus.Failed || x.Status == TaskStatus.Done)));

        //TODO придумать как оптимизировать!!!!!
        foreach (var everyDayNote in _everyDayNotes)
        {
            if (currentMonday.Date > today.Date)
                MondayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(1).Date > today.Date)
                TuesdayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(2).Date > today.Date)
                WednesdayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(3).Date > today.Date)
                ThursdayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(4).Date > today.Date)
                FridayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(5).Date > today.Date)
                SaturdayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(6).Date > today.Date)
                SundayNotes.Add(everyDayNote);
        }
        foreach (var everyWeekNote in _everyWeekNotes)
        {
            switch (everyWeekNote.StartDateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (currentMonday.Date >= today.Date)
                        MondayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Tuesday:
                    if (currentMonday.AddDays(1).Date >= today.Date)
                        TuesdayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Wednesday:
                    if (currentMonday.AddDays(2).Date >= today.Date)
                        WednesdayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Thursday:
                    if (currentMonday.AddDays(3).Date >= today.Date)
                        ThursdayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Friday:
                    if (currentMonday.AddDays(4).Date >= today.Date)
                        FridayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Saturday:
                    if (currentMonday.AddDays(5).Date >= today.Date)
                        SaturdayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Sunday:
                    if (currentMonday.AddDays(6).Date >= today.Date)
                        SundayNotes.Add(everyWeekNote);
                    break;
            }
        }
        foreach (var everyMonthNote in _everyMonthNotes)
        {
            var dateDay = everyMonthNote.StartDateTime.Day;
            if (currentMonday.Day == dateDay && currentMonday.Date >= today.Date)
                MondayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 1 == dateDay && currentMonday.AddDays(1).Date >= today.Date)
                TuesdayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 2 == dateDay && currentMonday.AddDays(2).Date >= today.Date)
                WednesdayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 3 == dateDay && currentMonday.AddDays(3).Date >= today.Date)
                ThursdayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 4 == dateDay && currentMonday.AddDays(4).Date >= today.Date)
                FridayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 5 == dateDay && currentMonday.AddDays(5).Date >= today.Date)
                SaturdayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 6 == dateDay && currentMonday.AddDays(6).Date >= today.Date)
                SundayNotes.Add(everyMonthNote);
        }
        foreach (var everyYearNote in _everyYearNotes)
        {
            var date = everyYearNote.StartDateTime;
            if (currentMonday.Day == date.Day && currentMonday.Month == date.Month && currentMonday.Date >= today.Date)
                MondayNotes.Add(everyYearNote);

            var nextDay = currentMonday.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date)
                TuesdayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date)
                WednesdayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date)
                ThursdayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date)
                FridayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date)
                SaturdayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date)
                SundayNotes.Add(everyYearNote);
        }

        MondayNotes = new ObservableCollection<Note>(MondayNotes.OrderBy(x => x.StartDateTime));
        TuesdayNotes = new ObservableCollection<Note>(TuesdayNotes.OrderBy(x => x.StartDateTime));
        WednesdayNotes = new ObservableCollection<Note>(WednesdayNotes.OrderBy(x => x.StartDateTime));
        ThursdayNotes = new ObservableCollection<Note>(ThursdayNotes.OrderBy(x => x.StartDateTime));
        FridayNotes = new ObservableCollection<Note>(FridayNotes.OrderBy(x => x.StartDateTime));
        SaturdayNotes = new ObservableCollection<Note>(SaturdayNotes.OrderBy(x => x.StartDateTime));
        SundayNotes = new ObservableCollection<Note>(SundayNotes.OrderBy(x => x.StartDateTime));

    }

    private bool IsDayInCurrentWeek(DateTimeOffset date)
    {
        return date.Date >= currentMonday && date.Date <= currentMonday;
    }
    #endregion
}