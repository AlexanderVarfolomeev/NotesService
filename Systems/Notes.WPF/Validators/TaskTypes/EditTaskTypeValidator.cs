using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF.Validators.TaskTypes;

public static class EditTaskTypeValidator
{
    private static readonly IUserDialogService userDialogService;

    static EditTaskTypeValidator()
    {
        userDialogService = new UserDialogService();
    }

    public static bool Check(EditTaskType task)
    {
        if (task.Name == "")
        {
            userDialogService.ShowError("Необходимо указать имя задачи!", "Ошибка.");
            return false;
        }

        if (task.TypeColorId == 0)
        {
            userDialogService.ShowError("Необходимо выбрать цвет задачи!", "Ошибка.");
            return false;
        }
        return true;
    }
}