using System;
using System.Linq;
using System.Threading.Tasks;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Colors;
using Notes.WPF.Services.Colors.Models;
using Notes.WPF.Views;

namespace Notes.WPF.Services.UserDialog;
//TODO доработать сервис до конца
public class UserDialogService : IUserDialogService
{
    public async Task<object> Edit(object item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        switch (item)
        {
            default: throw new NotSupportedException($"Редактирование объекта типа {item.GetType().Name} не поддерживается");
            case EditTaskType type:
                return await EditTaskType(type);
        }
    }

    public async Task<object> Add(object item)
    {
        switch (item)
        {
            default: throw new NotSupportedException($"Добавление объекта типа {item.GetType().Name} не поддерживается");
            case EditTaskType type:
                return await AddTaskType(type);
        }
    }

    public void ShowInformation(string Information, string Caption)
    {
        throw new NotImplementedException();
    }

    public void ShowWarning(string Message, string Caption)
    {
        throw new NotImplementedException();
    }

    public void ShowError(string Message, string Caption)
    {
        throw new NotImplementedException();
    }

    public bool Confirm(string Message, string Caption, bool Exclamation = false)
    {
        throw new NotImplementedException();
    }

    private static async Task<EditTaskType?> EditTaskType(EditTaskType type)
    {
        await ColorRepository.GetColors();
        var dialog = new TaskTypeDetailWindow();
        dialog.TypeName = type.Name;
        dialog.Color = ColorRepository.Colors.FirstOrDefault(x => x.Id == type.TypeColorId) ?? new ColorResponse();
        if (dialog.ShowDialog() == true)
        {
            type = new EditTaskType
            {
                Name = dialog.TypeName,
                TypeColorId = dialog.Color.Id
            };
            return type;
        }
        return null;
    }

    private static async Task<EditTaskType?> AddTaskType(EditTaskType type)
    {
        await ColorRepository.GetColors();
        var dialog = new TaskTypeDetailWindow();
        if (dialog.ShowDialog() == true)
        {
            type = new EditTaskType
            {
                Name = dialog.TypeName,
                TypeColorId = dialog.Color.Id
            };
            return type;
        }
        return null;
    }
}