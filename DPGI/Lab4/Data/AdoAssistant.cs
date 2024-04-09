using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

public class AdoAssistant
{
    private string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

    public DataTable TableLoad()
    {
        DataTable dt = new DataTable();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            string query = "SELECT * FROM books";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dt);
            connection.Close();
        }

        return dt;
    }

    public void InsertBook(string ISBN, string name, string author, string publisher, string year)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                string query = $"INSERT INTO books (ISBN, name, author, publisher, year) VALUES ('{ISBN}', '{name}', '{author}', '{publisher}', {year})";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Обробка помилки, наприклад, виведення її на консоль або логування
                MessageBox.Show($"Помилка при вставці книги: {ex.Message}");
            }
            finally
            {
                // Закриваємо підключення у блоку finally, щоб гарантувати його виконання
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }





    public void UpdateBook(string ISBN, string name, string author, string publisher, string year)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                string query = $"UPDATE books SET name = '{name}', author = '{author}', publisher = '{publisher}', year = {year} WHERE ISBN = '{ISBN}'";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при оновленні книги: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }

    public void DeleteBook(string ISBN)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                string query = "DELETE FROM books WHERE ISBN = @ISBN";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ISBN", ISBN);

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при видаленні книги: {ex.Message}");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }

}
