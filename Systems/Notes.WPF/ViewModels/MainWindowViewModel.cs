using System.Collections.Generic;
using System.Windows.Input;
using Notes.WPF.Infrastructure.Commands;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.ViewModels.Base;

namespace Notes.WPF.ViewModels;

public class MainWindowViewModel : ViewModel
{
    private readonly ITaskTypeService _taskTypeService;

    public MainWindowViewModel(ITaskTypeService taskTypeService)
    {
        _taskTypeService = taskTypeService;

        RefreshTaskTypesDataCommand = new LambdaCommand(OnRefreshTaskTypesDataExecuted, CanRefreshTaskTypesDataExecute);
    }

    public MainWindowViewModel()
    {
        _taskTypeService = new TaskTypeService();

        RefreshTaskTypesDataCommand = new LambdaCommand(OnRefreshTaskTypesDataExecuted, CanRefreshTaskTypesDataExecute);
    }

    private IEnumerable<TaskType> _taskTypes;


    public IEnumerable<TaskType> TaskTypes
    {
        get => _taskTypes;
        set => Set(ref _taskTypes, value);
    }


    #region Commands

    public ICommand RefreshTaskTypesDataCommand { get; }
    private bool CanRefreshTaskTypesDataExecute(object p)
    {
        return true;
    }
    private async void OnRefreshTaskTypesDataExecuted(object p)
    {
        TaskTypes = await _taskTypeService.GetTaskTypes();
    }

    #endregion
}