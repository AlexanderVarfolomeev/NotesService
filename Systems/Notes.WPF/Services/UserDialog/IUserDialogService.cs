using System.Threading.Tasks;

namespace Notes.WPF.Services.UserDialog;

public interface IUserDialogService
{
    Task<bool> Edit(object item);
    Task<bool> Add(object item);
    void ShowInformation(string Information, string Caption);

    void ShowWarning(string Message, string Caption);

    void ShowError(string Message, string Caption);

    bool Confirm(string Message, string Caption, bool Exclamation = false);
}