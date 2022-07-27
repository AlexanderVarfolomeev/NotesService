using System.Windows;
using Notes.WPF.Infrastructure.Commands.Base;

namespace Notes.WPF.Infrastructure.Commands;

class CloseWindowCommand : Command
{
    public override bool CanExecute(object parameter) => parameter is Window;

    public override void Execute(object parameter)
    {
        if (!CanExecute(parameter)) return;

        var window = (Window)parameter;
        window.Close();
    }
}

class CloseDialogCommand : Command
{
    public bool? DialogResult { get; set; }

    public override bool CanExecute(object parameter) => parameter is Window;

    public override void Execute(object parameter)
    {
        if (!CanExecute(parameter)) return;

        var window = (Window)parameter;
        window.DialogResult = DialogResult;
        window.Close();
    }
}