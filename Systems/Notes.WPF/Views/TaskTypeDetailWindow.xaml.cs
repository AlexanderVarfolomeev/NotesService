using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Notes.WPF.Models.TaskTypes;
using Notes.WPF.Services.Colors;
using Notes.WPF.Services.Colors.Models;
using ColorRepository = Notes.WPF.Services.Colors.ColorRepository;

namespace Notes.WPF.Views
{
    /// <summary>
    /// Interaction logic for TaskTypeDetailWindow.xaml
    /// </summary>
    public partial class TaskTypeDetailWindow : Window
    {
        public static readonly DependencyProperty ColorsProperty =
            DependencyProperty.Register(
                nameof(Colors),
                typeof(ObservableCollection<ColorResponse>),
                typeof(TaskTypeDetailWindow),
                new PropertyMetadata(null));

        public ObservableCollection<ColorResponse> Colors { get => (ObservableCollection<ColorResponse>)GetValue(ColorsProperty); set => SetValue(ColorsProperty, value); }

        public static readonly DependencyProperty TypeNameProperty =
            DependencyProperty.Register(
                nameof(TypeName),
                typeof(string),
                typeof(TaskTypeDetailWindow),
                new PropertyMetadata(null));

        public string TypeName { get => (string)GetValue(TypeNameProperty); set => SetValue(TypeNameProperty, value); }

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(
                nameof(Color),
                typeof(ColorResponse),
                typeof(TaskTypeDetailWindow),
                new PropertyMetadata(null));

        public ColorResponse Color { get => (ColorResponse)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }


        public TaskTypeDetailWindow()
        {
            InitializeComponent();
            Colors = new ObservableCollection<ColorResponse>(ColorRepository.Colors);
        }
    }
}
