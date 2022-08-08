using Notes.WPF.Models.Notes;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF.Validators.Notes;

public static class EditNoteValidator
{
    private static readonly IUserDialogService userDialogService;

    static EditNoteValidator()
    {
        userDialogService = new UserDialogService();
    }

    public static bool Check(EditNote note)
    {
        if (note.Name == "")
        {
            userDialogService.ShowError("Необходимо указать имя задачи.", "Ошибка!");
            return false;
        }

        if (note.TaskTypeId == 0)
        {
            userDialogService.ShowError("Необходимо указать тип задачи.", "Ошибка!");
            return false;
        }

        if (note.RepeatFrequency == 0)
        {
            userDialogService.ShowError("Необходимо указать частоту повторения задачи.", "Ошибка!");
            return false;
        }

        if (note.StartDateTime >= note.EndDateTime)
        {
            userDialogService.ShowError("Время начала выполнения задачи, не может быть позже времени конца выполнения задачи.", "Ошибка!");
            return false;
        }

        if (note.StartDateTime.Date != note.EndDateTime.Date)
        {
            userDialogService.ShowError("Задача должна начинаться и оканчиваться в один день.", "Ошибка!");
            return false;
        }

        return true;
    }
}   