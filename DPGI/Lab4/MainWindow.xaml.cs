using System;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using Lab4.Enum;

namespace Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AdoAssistant myTable = new AdoAssistant();
            list.SelectedIndex = 0;
            list.Focus();
            list.DataContext = myTable.TableLoad();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeDataWindow changeDataWindow = new ChangeDataWindow(TypeOfAction.Create, null);
            changeDataWindow.Show();
            Hide();
        }

        private DataTable? takeData()
        {
            string ISBN = Microsoft.VisualBasic.Interaction.InputBox("Введіть ISBN:");
            if (ISBN == "")
            {
                return null;
            }

            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    string query = $"SELECT * FROM books WHERE ISBN = '{ISBN}'";
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                    else
                    {
                        MessageBox.Show("Не існує такого ISBN");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при виконанні запиту до бази даних: {ex.Message}");
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
            }


            return dt;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var dt = takeData();
            if (dt == null) return;

            var changeDataWindow = new ChangeDataWindow(TypeOfAction.Update, dt);
            changeDataWindow.Show();
            Hide();

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var dt = takeData();
            if (dt == null) return;

            var changeDataWindow = new ChangeDataWindow(TypeOfAction.Delete, dt);
            changeDataWindow.Show();
            Hide();
        }

        private void MainWindow_OnClosing(object? sender, EventArgs e)
        {
            Close();
        }
    }
}