using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Notes.WPF.Infrastructure.Commands;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Colors;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.Services.UserDialog;
using Notes.WPF.ViewModels.Base;
using Notes.WPF.Views;

namespace Notes.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private readonly ITaskTypeService _taskTypeService;
    private readonly IUserDialogService _userDialogService;
    public MainWindowViewModel()
    {
        _taskTypeService = new TaskTypeService();
        _userDialogService = new UserDialogService();
        RefreshTaskTypesDataCommand = new LambdaCommand(OnRefreshTaskTypesDataExecuted, CanRefreshTaskTypesDataExecute);
        DeleteTaskTypeCommand = new LambdaCommand(OnDeleteTaskTypeExecuted, CanDeleteTaskTypeExecute);
        AddTaskTypeCommand = new LambdaCommand(OnAddTaskTypeExecuted, CanAddTaskTypeExecute);
        EditTaskTypeCommand = new LambdaCommand(OnEditTaskTypeExecuted, CanEditTaskTypeExecute);
    }


    private ObservableCollection<TaskType> _taskTypes;

    public ObservableCollection<TaskType> TaskTypes
    {
        get => _taskTypes;
        set => Set(ref _taskTypes, value);
    }


    private TaskType? _selectedType;

    public TaskType? SelectedType
    {
        get => _selectedType;
        set => Set(ref _selectedType, value);
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
    #endregion
}