using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Notes.WPF.Infrastructure.Commands;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.ViewModels.Base;
using Notes.WPF.Views;

namespace Notes.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private readonly ITaskTypeService _taskTypeService;

    public MainWindowViewModel()
    {
        _taskTypeService = new TaskTypeService();

        RefreshTaskTypesDataCommand = new LambdaCommand(OnRefreshTaskTypesDataExecuted, CanRefreshTaskTypesDataExecute);
        DeleteTaskTypeCommand = new LambdaCommand(OnDeleteTaskTypeExecuted, CanDeleteTaskTypeExecute);
        AddTaskTypeCommand = new LambdaCommand(OnAddTaskTypeExecuted, CanAddTaskTypeExecute);
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
        TaskTypeDetailWindow window = new TaskTypeDetailWindow();
        window.ShowDialog();
        var a = window.TypeName;
        var asa = window.Color;
        Console.WriteLine();
    }
    #endregion
}