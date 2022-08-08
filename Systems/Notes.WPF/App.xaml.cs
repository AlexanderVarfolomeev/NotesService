using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Notes.WPF.Services.Colors;
using Notes.WPF.Services.Notes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.Services.UserDialog;

namespace Notes.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddScoped<ITaskTypeService, TaskTypeService>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<INotesService, NotesService>();
            services.AddScoped<IUserDialogService, UserDialogService>();
            services.AddSingleton<MainWindow>();

        }
     
    }
}
