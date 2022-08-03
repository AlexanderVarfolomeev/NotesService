using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Notes.WPF.Infrastructure.Commands;
using Notes.WPF.Models.Notes;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Notes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly ITaskTypeService _taskTypeService;
    private readonly INotesService _notesService;
    private readonly IUserDialogService _userDialogService;
    public MainWindowViewModel()
    {
        _notesService = new NotesService();
        _taskTypeService = new TaskTypeService();
        _userDialogService = new UserDialogService();

        DeleteTaskTypeCommand = new LambdaCommand(OnDeleteTaskTypeExecuted, CanDeleteTaskTypeExecute);
        EditTaskTypeCommand = new LambdaCommand(OnEditTaskTypeExecuted, CanEditTaskTypeExecute);
    }

    [ObservableProperty]
    private ObservableCollection<TaskType> _taskTypes;

    [ObservableProperty]
    private TaskType? _selectedType;

    [ObservableProperty]
    private ObservableCollection<Note> _notes;
    [ObservableProperty]
    private Note? _selectedNote;

    [ICommand]
    private async void RefreshData(object p)
    {
        TaskTypes = new ObservableCollection<TaskType>(await _taskTypeService.GetTaskTypes());
        Notes = new ObservableCollection<Note>(await _notesService.GetNotes());
        await RefreshLastFourWeeksNotes();
    }

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

    [ICommand]
    private async void AddTaskType(object p)
    {
        EditTaskType? type = new EditTaskType();
        type = await _userDialogService.Add(type) as EditTaskType;
        if (type is null)
            return;
        await _taskTypeService.AddTask(type);
        RefreshData(new object());
    }

    public ICommand EditTaskTypeCommand { get; }
    private bool CanEditTaskTypeExecute(object p) => SelectedType != null;
    private async void OnEditTaskTypeExecuted(object p)
    {
        if (p is TaskType taskType)
        {
            EditTaskType? type = new EditTaskType()
            {
                Name = taskType.Name,
                TypeColorId = taskType.TypeColorId
            };
            type = await _userDialogService.Edit(type) as EditTaskType;
            if (type is null)
                return;
            await _taskTypeService.UpdateTask(type, taskType.Id);
            RefreshData(new object());
        }
    }


    [ObservableProperty] private ISeries[] _series;

    [ObservableProperty] private Axis[] _xAxes;

    [ObservableProperty]
    private Dictionary<string, IEnumerable<Note>> _notesLastWeeks;

    [ICommand]
    private async Task RefreshLastFourWeeksNotes()
    {
        NotesLastWeeks = new Dictionary<string, IEnumerable<Note>>(await _notesService.GetCompletedTaskForLastFourWeeks());
        XAxes = new Axis[]
        {
            new Axis(){
                Labels = NotesLastWeeks.Select(x => x.Key).ToArray(),
            }
        };

        Dictionary<string, double[]> dictionary = new Dictionary<string, double[]>();
        int i = 0;
        foreach (var week in NotesLastWeeks)
        {
            foreach (var note in week.Value)
            {
                if (!dictionary.ContainsKey(note.Type))
                    dictionary.Add(note.Type, new double[4] { 0, 0, 0, 0 });
                var hours = (note.EndDateTime - note.StartDateTime).TotalHours;
                dictionary[note.Type][i] += hours;
            }

            i++;
        }
        Series = new ColumnSeries<double>[dictionary.Count];
        i = 0;
        foreach (var pair in dictionary)
        {
            Series[i] = new ColumnSeries<double>()
            {
                Name = pair.Key,
                Values = pair.Value,
            };
            i++;
        }
    }
}
