using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notes.WPF.Services.Auth;
using Notes.WPF.Services.Colors;
using Notes.WPF.Services.Notes;
using Notes.WPF.Services.TaskTypes;
using Notes.WPF.Services.UserDialog;
using Notes.WPF.ViewModels;

namespace Notes.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static bool IsDesignMode { get; private set; } = true;

        private static IHost? __Host;

        public static IHost? Host => __Host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        protected override async void OnStartup(StartupEventArgs e)
        {
            IsDesignMode = false;
            var host = Host;
            base.OnStartup(e);

            await host.StartAsync().ConfigureAwait(false);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            var host = Host;
            await host.StopAsync().ConfigureAwait(false);
            host.Dispose();
            __Host = null;
        }

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {

            services.AddSingleton<MainWindowViewModel>();
            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<INotesService, NotesService>();
            services.AddScoped<ITaskTypeService, TaskTypeService>();
            services.AddScoped<IUserDialogService, UserDialogService>();
            services.AddSingleton<HttpClient>();
            services.AddScoped<IAuthService, AuthService>();
        }

        public static string CurrentDirectory => IsDesignMode
            ? Path.GetDirectoryName(GetSourceCodePath())
            : Environment.CurrentDirectory;

        private static string GetSourceCodePath([CallerFilePath] string Path = null) => Path;

    }
}
