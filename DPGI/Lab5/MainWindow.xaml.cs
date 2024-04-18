using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Lab5.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Lab5DbContext _context;
        private List<Book> _books;
        private List<Publisher> _publishers;

        public MainWindow()
        {
            InitializeComponent();
            _context = new Lab5DbContext();
            _books = _context.Books.ToList();
            _publishers = _context.Publishers.ToList();
            dataGrid1.ItemsSource = _books;
            dataGrid2.ItemsSource = _publishers;
            JoinTables();
            var column = dataGrid1.Columns.SingleOrDefault(c => c.Header.ToString() == "PublisherCodeNavigation");
            if (column != null)
            {
                column.Visibility = Visibility.Collapsed;
            }
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            Close();
        }

        private void Select1Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var books = _context.Books.Where(b => b.PublicationYear == Int32.Parse(publicationYearTextBox.Text))
                    .ToList();
                dataGridSelect1.ItemsSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка під час виконання запиту: " + ex.Message);
            }
        }

        private void Select2Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var books = _context.Books.FromSqlRaw("SELECT * FROM Books WHERE ISBN LIKE '%' + {0} + '%'",
                    publicationISBNTextBox.Text).ToList();
                dataGridSelect2.ItemsSource = books;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка під час виконання запиту: " + ex.Message);
            }
        }

        private void JoinTables()
        {
            try
            {
                var query = from book in _context.Books
                    join publisher in _context.Publishers
                        on book.PublisherCode equals publisher.PublisherCode
                    select new
                    {
                        book.Isbn,
                        book.Authors,
                        book.PublicationYear,
                        book.PublisherCode,
                        publisher.PublisherName
                    };

                dataGridJoin.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка під час виконання запиту: " + ex.Message);
            }
        }
    }
}