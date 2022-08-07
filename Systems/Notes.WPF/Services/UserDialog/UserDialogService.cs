using System;
using System.Linq;
using System.Threading.Tasks;
using Notes.WPF.Models.Notes;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Colors;
using Notes.WPF.Services.Colors.Models;
using Notes.WPF.ViewModels;
using Notes.WPF.Views;

namespace Notes.WPF.Services.UserDialog;
//TODO доработать сервис до конца
public class UserDialogService : IUserDialogService
{
    public void OpenMainWindow()
    {
        var dialog = new MainWindow();
        dialog.ShowDialog();
    }

    public bool Edit(object item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        switch (item)
        {
            default: throw new NotSupportedException($"Редактирование объекта типа {item.GetType().Name} не поддерживается");
            case EditTaskType type:
                 return EditTaskType();
            case EditNote note:
                return  EditNote();
        }
    }

    public bool Add(object item)
    {
        switch (item)
        {
            default: throw new NotSupportedException($"Добавление объекта типа {item.GetType().Name} не поддерживается");
            case EditTaskType type:
                return  AddTaskType();
            case EditNote note:
                return AddNote();
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

    private static bool EditNote()
    {
        var dialog = new NoteDetailWindow();
        return dialog.ShowDialog() ?? throw new NullReferenceException();
    }

    private static bool AddNote()
    {
        var dialog = new NoteDetailWindow();
        return dialog.ShowDialog() ?? throw new NullReferenceException();
    }

    private static bool EditTaskType()
    {
        var dialog = new TaskTypeDetailWindow();
        return dialog.ShowDialog() ?? throw new NullReferenceException();
    }

    private static bool AddTaskType()
    {
        var dialog = new TaskTypeDetailWindow();
        return dialog.ShowDialog() ?? throw new NullReferenceException();
    }
}