using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace Lab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CommandBinding saveCommand = new CommandBinding(ApplicationCommands.Save, execute_Save, canExecute_Save);
            CommandBindings.Add(saveCommand);

            CommandBinding openCommand = new CommandBinding(ApplicationCommands.Open, execute_Open, canExecute_Open);
            CommandBindings.Add(openCommand);

            CommandBinding cutCommand = new CommandBinding(ApplicationCommands.Cut, execute_Cut, canExecute_Cut);
            CommandBindings.Add(cutCommand);
        }

        void canExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            if (InputTextBox.Text.Trim().Length > 0) e.CanExecute = true;
            else e.CanExecute = false;
        }

        void execute_Save(object sender, ExecutedRoutedEventArgs e)
        {
            System.IO.File.WriteAllText("F:\\GIT\\DPGI\\DPGI\\Lab2\\file.txt", InputTextBox.Text);
            MessageBox.Show("Файл збережено!");
        }

        void canExecute_Open(object sender, CanExecuteRoutedEventArgs e)
        {
            if (InputTextBox.Text.Trim().Length == 0) e.CanExecute = true;
            else e.CanExecute = false;
        }

        void execute_Open(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
            openFileDialog.Title = "Виберіть файл";
            if (openFileDialog.ShowDialog() == true)
            {
                InputTextBox.Text = System.IO.File.ReadAllText(openFileDialog.FileName);
            }
        }

        void canExecute_Cut(object sender, CanExecuteRoutedEventArgs e)
        {
            if (InputTextBox.Text.Trim().Length > 0) e.CanExecute = true;
            else e.CanExecute = false;
        }

        void execute_Cut(object sender, ExecutedRoutedEventArgs e)
        {
            InputTextBox.Text = "";
        }
    }
}