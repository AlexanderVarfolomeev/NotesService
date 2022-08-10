namespace Notes.WPF.Services.UserDialog;

public interface IUserDialogService
{
    void OpenMainWindow();
    bool OpenRegisterWindow();
    bool Edit(object item);
    bool Add(object item);
    void ShowInformation(string Information, string Caption);

    void ShowWarning(string Message, string Caption);

    void ShowError(string Message, string Caption);

    bool Confirm(string Message, string Caption, bool Exclamation = false);
}