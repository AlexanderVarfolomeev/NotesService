using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Notes.WPF.Infrastructure.Commands;
using Notes.WPF.Models.Notes;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Colors;
using Notes.WPF.Services.Notes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.Services.UserDialog;
using Notes.WPF.ViewModels.Base;
using Notes.WPF.Views;

namespace Notes.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private readonly ITaskTypeService _taskTypeService;
    private readonly INotesService _notesService;
    private readonly IUserDialogService _userDialogService;
    public MainWindowViewModel()
    {
        _notesService = new NotesService();
        _taskTypeService = new TaskTypeService();

        _userDialogService = new UserDialogService();
        RefreshTaskTypesDataCommand = new LambdaCommand(OnRefreshTaskTypesDataExecuted, CanRefreshTaskTypesDataExecute);
        DeleteTaskTypeCommand = new LambdaCommand(OnDeleteTaskTypeExecuted, CanDeleteTaskTypeExecute);
        AddTaskTypeCommand = new LambdaCommand(OnAddTaskTypeExecuted, CanAddTaskTypeExecute);
        EditTaskTypeCommand = new LambdaCommand(OnEditTaskTypeExecuted, CanEditTaskTypeExecute);

        RefreshNotesDataCommand =
            new LambdaCommand(OnRefreshNotesDataCommandExecuted, CanRefreshNotesDataCommandExecute);
    }


    private ObservableCollection<TaskType> _taskTypes;

    public ObservableCollection<TaskType> TaskTypes
    {
        get => _taskTypes;
        set => Set(ref _taskTypes, value);
    }


    private ObservableCollection<Note> _notes;

    public ObservableCollection<Note> Notes
    {
        get => _notes;
        set => Set(ref _notes, value);
    }


    private TaskType? _selectedType;

    public TaskType? SelectedType
    {
        get => _selectedType;
        set
        {
            Set(ref _selectedType, value);
            if(value != null)
                OnEditTaskTypeExecuted(value);
        }
    }

    private Note? _selectedNote;

    public Note? SelectedNote
    {
        get => _selectedNote;
        set => Set(ref _selectedNote, value);
    }

    #region Commands

    public ICommand RefreshTaskTypesDataCommand { get; }
    private bool CanRefreshTaskTypesDataExecute(object p)
    {
        return true;
    }
    private async void OnRefreshTaskTypesDataExecuted(object p)
    {
        TaskTypes = new ObservableCollection<TaskType>(await _taskTypeService.GetTaskTypes());
        Notes = new ObservableCollection<Note>(await _notesService.GetNotes());
    }


    public ICommand DeleteTaskTypeCommand { get; }
    private bool CanDeleteTaskTypeExecute(object p)
    {
        return _selectedType != null;
    }
    private async void OnDeleteTaskTypeExecuted(object p)
    {
        if (p is TaskType taskType)
        {
            await _taskTypeService.DeleteTask(taskType.Id);
            _taskTypes.Remove(taskType);
        }
    }


    public ICommand AddTaskTypeCommand { get; }
    private bool CanAddTaskTypeExecute(object p) => true;
    private async void OnAddTaskTypeExecuted(object p)
    {
        EditTaskType? type = new EditTaskType();
        type = await _userDialogService.Add(type) as EditTaskType;
        if(type is null)
            return;
        await _taskTypeService.AddTask(type);
        OnRefreshTaskTypesDataExecuted(new object());
    }


    public ICommand EditTaskTypeCommand { get; }
    private bool CanEditTaskTypeExecute(object p) => SelectedType != null;
    private async void OnEditTaskTypeExecuted(object p)
    {
        EditTaskType? type = new EditTaskType()
        {
            Name = SelectedType.Name,
            TypeColorId = SelectedType.TypeColorId
        };
        type = await _userDialogService.Edit(type) as EditTaskType;
        if (type is null)
            return;
        await _taskTypeService.UpdateTask(type, SelectedType.Id);
        OnRefreshTaskTypesDataExecuted(new object());
    }


    public ICommand RefreshNotesDataCommand { get; }
    private bool CanRefreshNotesDataCommandExecute(object p)
    {
        return true;
    }
    private async void OnRefreshNotesDataCommandExecuted(object p)
    {
        Notes = new ObservableCollection<Note>(await _notesService.GetNotes());
    }
    #endregion
}
