using System.Collections.Generic;
using System.Collections.ObjectModel;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.ViewModels.Base;

namespace Notes.WPF.ViewModels;

public class TaskTypeDetailViewModel : ViewModel
{
    public TaskTypeDetailViewModel()
    {
        _typeColors = new ObservableCollection<TypeColor>()
        {
            new TypeColor()
            {
                Code = "#FF0000",
                Name = "Red"
            },
            new TypeColor()
            {
                Code = "#00FF00",
                Name = "Green"
            }
        };
    }


    private TaskType _taskType;

    public TaskType TaskType
    {
        get => _taskType;
        set => Set(ref _taskType, value);
    }


    private ObservableCollection<TypeColor> _typeColors;

    public ObservableCollection<TypeColor> TypeColors
    {
        get => _typeColors;
        set => Set(ref _typeColors, value);
    }
}