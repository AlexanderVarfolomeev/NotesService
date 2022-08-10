using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Notes.WPF.Models.Auth;
using Notes.WPF.Models.Notes;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Auth;
using Notes.WPF.Services.Colors;
using Notes.WPF.Services.Colors.Models;
using Notes.WPF.Services.Notes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.Services.UserDialog;
using Notes.WPF.Validators.Notes;
using Notes.WPF.Validators.TaskTypes;
using SkiaSharp;
using TaskStatus = Notes.WPF.Models.Notes.TaskStatus;

namespace Notes.WPF.ViewModels;
//TODO вряди ли тут должно находится так много логики, мб перенести
//TODO баг, если запускать в докере, время постоянно смещается
public partial class MainWindowViewModel : ObservableObject
{
    private readonly ITaskTypeService _taskTypeService;
    private readonly IColorService _colorServiceService;
    private readonly INotesService _notesService;
    private readonly IUserDialogService _userDialogService;
    private readonly IAuthService _authService;

    //TODO добавить поддержку Dependency injection
    public MainWindowViewModel(IColorService colorService, INotesService notesService, IUserDialogService userDialogService, ITaskTypeService taskTypeService, IAuthService authService)
    {
        _colorServiceService = colorService;
        _notesService = notesService;
        _userDialogService = userDialogService;
        _taskTypeService = taskTypeService;
        _authService = authService;

        currentMonday = GetDateOfMondayOnThisWeek();
    }

    [ObservableProperty]
    private Note? _selectedNote;

    [ObservableProperty] private string _login;
    [ObservableProperty] private string _password;

    [RelayCommand]
    private async Task InitData()
    {
        LoginModel lg = new LoginModel()
        {
            Email = Login,
            Password = Password
        };
        bool a =(await _authService.Login(lg)).Successful;
        Colors = new ObservableCollection<ColorResponse>(await _colorServiceService.GetColors());
        await RefreshData();
        //TODO авторизация
        if (true)
        {
            _userDialogService.OpenMainWindow();
        }
        else
        {
            //_userDialogService.ShowError
        }
    }

    [RelayCommand]
    private async Task RefreshData()
    {
        TaskTypes = new ObservableCollection<TaskType>(await _taskTypeService.GetTaskTypes());
        await RepetitionNotesRefresh();
        CurrentWeekNotes = new ObservableCollection<Note>((await _notesService
                .GetNotesInInterval(currentMonday, currentMonday.AddDays(6)))
            .OrderBy(x => x.StartDateTime.Hour));
        await RefreshLastFourWeeksNotes();
        RefreshDaysLabels();
        RefreshNotes();
    }

    #region TaskType

    [ObservableProperty]
    private ObservableCollection<TaskType> _taskTypes;

    [ObservableProperty]
    private ObservableCollection<ColorResponse> _colors;

    [ObservableProperty]
    private ColorResponse? _selectedColor;

    [ObservableProperty]
    private bool _isEditType;

    [ObservableProperty]
    private TaskType? _selectedType;

    [ObservableProperty] 
    private EditTaskType? _editType;

    #region Commands

    private bool CanDeleteTaskTypeExecute(object p) => SelectedType != null && SelectedType.Name != "Empty";

    [RelayCommand(CanExecute = nameof(CanDeleteTaskTypeExecute))]
    private async void DeleteTaskType(object p)
    {
        if (p is TaskType taskType)
        {
            if (_userDialogService.Confirm("Вы действительно хотите удалить тип задачи?", "Предупреждение"))
            {
                await _taskTypeService.DeleteTask(taskType.Id);
                await RefreshData();
            }
        }
    }

    [RelayCommand]
    private async void AddTaskType(object p)
    {
        IsEditType = false;
        EditType = new EditTaskType();
        SelectedColor = null;
        if (_userDialogService.Add(EditType))
        {
            EditType.TypeColorId = SelectedColor?.Id ?? 0;
            if (EditTaskTypeValidator.Check(EditType))
            {
                await _taskTypeService.AddTask(EditType);
                await RefreshData();
            }
        }
    }

    private bool CanEditTaskTypeExecute() => SelectedType != null && SelectedType.Name != "Empty";
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
            SelectedColor = Colors.FirstOrDefault(x => x.Id == SelectedType.TypeColorId);
            if (_userDialogService.Edit(EditType))
            {
                EditType.TypeColorId = SelectedColor?.Id ?? 0;
                if (EditTaskTypeValidator.Check(EditType))
                {
                    await _taskTypeService.UpdateTask(EditType, taskType.Id);
                    await RefreshData();
                }
            }
        }
    }
    #endregion


    #endregion

    #region Activity chart

    [ObservableProperty]
    private Dictionary<string, double[]> _completedNotesLastFourWeeks;

    [ObservableProperty] 
    private ISeries[] _series;

    [ObservableProperty]
    private Axis[] _xAxes;

    //TODO подправить код
    private async Task RefreshLastFourWeeksNotes()
    {
        var dates = new string[4];
        var dateTimeNow = DateTimeOffset.Now;
        var startDate = dateTimeNow.AddDays(-21 - ((int)dateTimeNow.DayOfWeek == 0 ? 7 : (int)dateTimeNow.DayOfWeek) + 1);
        for (int j = 0; j < 4; j++)
        {
            var start = startDate.AddDays(7 * j);
            var end = j == 3 ? dateTimeNow : start.AddDays(6);
            var startStr = start.Day + "." + start.Month;
            var endStr = end.Day + "." + end.Month;
            dates[j] = startStr + " - " + endStr;
        }

        CompletedNotesLastFourWeeks = new Dictionary<string, double[]>(await _notesService.GetCompletedTaskForLastFourWeeks());
        XAxes = new[]
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
                Fill = new SolidColorPaint(new SKColor(rgb.Item1, rgb.Item2, rgb.Item3)),
            };
            i++;
        }
    }
    private (byte, byte, byte) GetRGBCodeFromTaskTypeName(string name)
    {
        var type = TaskTypes.FirstOrDefault(x => x.Name == name);
        var color = type.Color.Code.Substring(1);
        return (Convert.ToByte(color.Substring(0, 2), 16),
            Convert.ToByte(color.Substring(2, 2), 16),
            Convert.ToByte(color.Substring(4, 2), 16));
    }
    #endregion

    #region This week
    [ObservableProperty]
    private ObservableCollection<Note> _currentWeekNotes;
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
        RefreshDaysLabels();
        RefreshNotes();
    }

    [RelayCommand]
    private async Task GetPreviousWeek()
    {
        currentMonday = currentMonday.AddDays(-7);
        CurrentWeekNotes =
            new ObservableCollection<Note>(
                await _notesService.GetNotesInInterval(currentMonday, currentMonday.AddDays(6)));
        RefreshDaysLabels();
        RefreshNotes();
    }


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

    private async Task RepetitionNotesRefresh()
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
            if (currentMonday.Date >= today.Date && everyDayNote.StartDateTime.Date <= currentMonday.Date)
                MondayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(1).Date >= today.Date && everyDayNote.StartDateTime.Date <= currentMonday.AddDays(1).Date)
                TuesdayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(2).Date >= today.Date && everyDayNote.StartDateTime.Date <= currentMonday.AddDays(2).Date)
                WednesdayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(3).Date >= today.Date && everyDayNote.StartDateTime.Date <= currentMonday.AddDays(3).Date)
                ThursdayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(4).Date >= today.Date && everyDayNote.StartDateTime.Date <= currentMonday.AddDays(4).Date)
                FridayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(5).Date >= today.Date && everyDayNote.StartDateTime.Date <= currentMonday.AddDays(5).Date)
                SaturdayNotes.Add(everyDayNote);

            if (currentMonday.AddDays(6).Date >= today.Date && everyDayNote.StartDateTime.Date <= currentMonday.AddDays(6).Date)
                SundayNotes.Add(everyDayNote);
        }
        foreach (var everyWeekNote in _everyWeekNotes)
        {
            switch (everyWeekNote.StartDateTime.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    if (currentMonday.Date >= today.Date && everyWeekNote.StartDateTime.Date <= currentMonday.Date)
                        MondayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Tuesday:
                    if (currentMonday.AddDays(1).Date >= today.Date && everyWeekNote.StartDateTime.Date <= currentMonday.AddDays(1).Date)
                        TuesdayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Wednesday:
                    if (currentMonday.AddDays(2).Date >= today.Date && everyWeekNote.StartDateTime.Date <= currentMonday.AddDays(2).Date)
                        WednesdayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Thursday:
                    if (currentMonday.AddDays(3).Date >= today.Date && everyWeekNote.StartDateTime.Date <= currentMonday.AddDays(3).Date)
                        ThursdayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Friday:
                    if (currentMonday.AddDays(4).Date >= today.Date && everyWeekNote.StartDateTime.Date <= currentMonday.AddDays(4).Date)
                        FridayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Saturday:
                    if (currentMonday.AddDays(5).Date >= today.Date && everyWeekNote.StartDateTime.Date <= currentMonday.AddDays(5).Date)
                        SaturdayNotes.Add(everyWeekNote);
                    break;
                case DayOfWeek.Sunday:
                    if (currentMonday.AddDays(6).Date >= today.Date && everyWeekNote.StartDateTime.Date <= currentMonday.AddDays(6).Date)
                        SundayNotes.Add(everyWeekNote);
                    break;
            }
        }
        foreach (var everyMonthNote in _everyMonthNotes)
        {
            var dateDay = everyMonthNote.StartDateTime.Day;
            if (currentMonday.Day == dateDay && currentMonday.Date >= today.Date && everyMonthNote.StartDateTime.Date <= currentMonday.Date)
                MondayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 1 == dateDay && currentMonday.AddDays(1).Date >= today.Date
                                                 && everyMonthNote.StartDateTime.Date <= currentMonday.AddDays(1).Date)
                TuesdayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 2 == dateDay && currentMonday.AddDays(2).Date >= today.Date
                                                 && everyMonthNote.StartDateTime.Date <= currentMonday.AddDays(2).Date)
                WednesdayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 3 == dateDay && currentMonday.AddDays(3).Date >= today.Date
                                                 && everyMonthNote.StartDateTime.Date <= currentMonday.AddDays(3).Date)
                ThursdayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 4 == dateDay && currentMonday.AddDays(4).Date >= today.Date
                                                 && everyMonthNote.StartDateTime.Date <= currentMonday.AddDays(4).Date)
                FridayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 5 == dateDay && currentMonday.AddDays(5).Date >= today.Date
                                                 && everyMonthNote.StartDateTime.Date <= currentMonday.AddDays(5).Date)
                SaturdayNotes.Add(everyMonthNote);

            if (currentMonday.Day + 6 == dateDay && currentMonday.AddDays(6).Date >= today.Date
                                                 && everyMonthNote.StartDateTime.Date <= currentMonday.AddDays(6).Date)
                SundayNotes.Add(everyMonthNote);
        }
        foreach (var everyYearNote in _everyYearNotes)
        {
            var date = everyYearNote.StartDateTime;
            if (currentMonday.Day == date.Day && currentMonday.Month == date.Month && currentMonday.Date >= today.Date
                && everyYearNote.StartDateTime.Date <= currentMonday.Date)
                MondayNotes.Add(everyYearNote);

            var nextDay = currentMonday.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date
                && everyYearNote.StartDateTime.Date <= currentMonday.AddDays(1).Date)
                TuesdayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date
                && everyYearNote.StartDateTime.Date <= currentMonday.AddDays(2).Date)
                WednesdayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date
                && everyYearNote.StartDateTime.Date <= currentMonday.AddDays(3).Date)
                ThursdayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date
                && everyYearNote.StartDateTime.Date <= currentMonday.AddDays(4).Date)
                FridayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date
                && everyYearNote.StartDateTime.Date <= currentMonday.AddDays(5).Date)
                SaturdayNotes.Add(everyYearNote);

            nextDay = nextDay.AddDays(1);
            if (nextDay.Day == date.Day && nextDay.Month == date.Month && nextDay.Date >= today.Date
                && everyYearNote.StartDateTime.Date <= currentMonday.AddDays(6).Date)
                SundayNotes.Add(everyYearNote);
        }

        MondayNotes = new ObservableCollection<Note>(MondayNotes.OrderBy(x => x.StartDateTime.TimeOfDay));
        TuesdayNotes = new ObservableCollection<Note>(TuesdayNotes.OrderBy(x => x.StartDateTime.TimeOfDay));
        WednesdayNotes = new ObservableCollection<Note>(WednesdayNotes.OrderBy(x => x.StartDateTime.TimeOfDay));
        ThursdayNotes = new ObservableCollection<Note>(ThursdayNotes.OrderBy(x => x.StartDateTime.TimeOfDay));
        FridayNotes = new ObservableCollection<Note>(FridayNotes.OrderBy(x => x.StartDateTime.TimeOfDay));
        SaturdayNotes = new ObservableCollection<Note>(SaturdayNotes.OrderBy(x => x.StartDateTime.TimeOfDay));
        SundayNotes = new ObservableCollection<Note>(SundayNotes.OrderBy(x => x.StartDateTime.TimeOfDay));
    }
    #endregion

    #region Notes

    private void RefreshDaysLabels()
    {
        MondayLabel = currentMonday.ToString("dd/MM");
        TuesdayLabel = currentMonday.AddDays(1).ToString("dd/MM");
        WednesdayLabel = currentMonday.AddDays(2).ToString("dd/MM");
        ThursdayLabel = currentMonday.AddDays(3).ToString("dd/MM");
        FridayLabel = currentMonday.AddDays(4).ToString("dd/MM");
        SaturdayLabel = currentMonday.AddDays(5).ToString("dd/MM");
        SundayLabel = currentMonday.AddDays(6).ToString("dd/MM");
    }
    [ObservableProperty] private string _mondayLabel;
    [ObservableProperty] private string _tuesdayLabel;
    [ObservableProperty] private string _wednesdayLabel;
    [ObservableProperty] private string _thursdayLabel;
    [ObservableProperty] private string _fridayLabel;
    [ObservableProperty] private string _saturdayLabel;
    [ObservableProperty] private string _sundayLabel;

    [ObservableProperty] private int _taskTypeIdNote;
    [ObservableProperty] private DateTime _dayDateNote;
    [ObservableProperty] private DateTime _startTimeNote;
    [ObservableProperty] private DateTime _endTimeNote;
    [ObservableProperty] private RepeatFrequency _repeatFrequencyNote;

    [ObservableProperty] private TaskType _selectedTaskType;
    [ObservableProperty] private EditNote? _editNote;
    [ObservableProperty] private bool _isEditNote;

    [RelayCommand]
    private async Task DoEditNote(object p)
    {
        if (p is Note note)
        {
            IsEditNote = true;
            EditNote = new EditNote()
            {
                Name = SelectedNote.Name,
                Description = SelectedNote.Description,
                TaskTypeId = SelectedNote.TaskTypeId,
                EndDateTime = SelectedNote.EndDateTime,
                StartDateTime = SelectedNote.StartDateTime,
                RepeatFrequency = SelectedNote.RepeatFrequency,
                Status = SelectedNote.Status
            };

            SelectedTaskType = TaskTypes.FirstOrDefault(x => x.Id == SelectedNote.TaskTypeId) ?? throw new ArgumentNullException();
            DayDateNote = SelectedNote.StartDateTime.DateTime;
            StartTimeNote = SelectedNote.StartDateTime.LocalDateTime;
            EndTimeNote = SelectedNote.EndDateTime.LocalDateTime;


            if (_userDialogService.Edit(EditNote))
            {
                EditNote.TaskTypeId = SelectedTaskType.Id;
                EditNote.EndDateTime = new DateTime(DayDateNote.Year, DayDateNote.Month, DayDateNote.Day, EndTimeNote.Hour, EndTimeNote.Minute, 0);
                EditNote.StartDateTime = new DateTime(DayDateNote.Year, DayDateNote.Month, DayDateNote.Day, StartTimeNote.Hour, StartTimeNote.Minute, 0);
                if (EditNoteValidator.Check(EditNote))
                {
                    await _notesService.UpdateNote(SelectedNote.Id, EditNote);
                    await RefreshData();
                }
            }
        }
    }

    [RelayCommand]
    private async Task DeleteNote(object p)
    {
        if (p is Note note)
        {
            if (_userDialogService.Confirm("Вы действительно хотите удалить задачу?", "Предупреждение"))
            {
                await _notesService.DeleteNote(note.Id);
                await RefreshData();
            }
        }
    }

    [RelayCommand]
    private async Task DoTask(object p)
    {
        if (p is Note note)
        {
            await _notesService.DoTask(SelectedNote.Id);
            await RefreshData();
        }
    }

    [RelayCommand]
    private async Task AddNote()
    {
        DayDateNote = DateTime.Now;
        IsEditNote = false;
        EditNote = new EditNote();
        SelectedTaskType = null;
        if (_userDialogService.Add(EditNote))
        {
            EditNote.TaskTypeId = SelectedTaskType?.Id ?? 0;
            EditNote.EndDateTime =
                new DateTime(DayDateNote.Year, DayDateNote.Month, DayDateNote.Day, EndTimeNote.Hour, EndTimeNote.Minute, 0);
            EditNote.StartDateTime =
                new DateTime(DayDateNote.Year, DayDateNote.Month, DayDateNote.Day, StartTimeNote.Hour, StartTimeNote.Minute, 0);
            EditNote.Status = TaskStatus.Waiting;

            if (EditNoteValidator.Check(EditNote))
            {
                await _notesService.AddNote(EditNote);
                await RefreshData();
            }
        }
    }

    #endregion
}