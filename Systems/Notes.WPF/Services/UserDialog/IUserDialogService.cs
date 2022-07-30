﻿using System.Threading.Tasks;

namespace Notes.WPF.Services.UserDialog;

public interface IUserDialogService
{
    Task<object> Edit(object item);
    Task<object> Add(object item);
    void ShowInformation(string Information, string Caption);

    void ShowWarning(string Message, string Caption);

    void ShowError(string Message, string Caption);

    bool Confirm(string Message, string Caption, bool Exclamation = false);
}