﻿using System;
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

namespace Notes.WPF.Views
{
    /// <summary>
    /// Interaction logic for TaskTypeDetailWindow.xaml
    /// </summary>
    public partial class TaskTypeDetailWindow : Window
    {
        public TaskTypeDetailWindow()
        {
            InitializeComponent();
        }
    }
}
