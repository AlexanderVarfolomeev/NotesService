using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Notes.WPF.Infrastructure.Commands;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.ViewModels.Base;

namespace Notes.WPF.ViewModels;

public class TaskTypeDetailViewModel : ViewModel
{
    private TaskTypeService _taskTypeService;

    public TaskTypeDetailViewModel()
    {
        _taskTypeService = new TaskTypeService();
        DeleteTaskTypeCommand = new LambdaCommand(OnDeleteTaskTypeExecuted, CanDeleteTaskTypeExecute);
    }

    public ICommand DeleteTaskTypeCommand { get; }
    private bool CanDeleteTaskTypeExecute(object p)
    {
        return true;
    }
    private async void OnDeleteTaskTypeExecuted(object p)
    {
        if (p is EditTaskType taskType)
        {
            var type = (await _taskTypeService.GetTaskTypes())
                .FirstOrDefault(x => x.Name == taskType.Name && x.TypeColorId == taskType.TypeColorId)
                ?? throw new ArgumentNullException();
            await _taskTypeService.DeleteTask(type.Id);
        }
    }
}