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
                typeof(ObservableCollection<TypeColor>),
                typeof(TaskTypeDetailWindow),
                new PropertyMetadata(null));

        public ObservableCollection<TypeColor> Colors { get => (ObservableCollection<TypeColor>)GetValue(ColorsProperty); set => SetValue(ColorsProperty, value); }

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
                typeof(TypeColor),
                typeof(TaskTypeDetailWindow),
                new PropertyMetadata(null));

        public TypeColor Color { get => (TypeColor)GetValue(ColorProperty); set => SetValue(ColorProperty, value); }
        public TaskTypeDetailWindow()
        {
            InitializeComponent();
            Colors = new ObservableCollection<TypeColor>()
            {
                new TypeColor()
                {
                    Code = "#FF0000",
                    Name = "Red"
                },
                new TypeColor()
                {
                    Code = "#00FF00",
                    Name = "Green"
                }
            };
        }
    }
}
