using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lab4.Enum;
using Microsoft.IdentityModel.Tokens;

namespace Lab4;

public partial class ChangeDataWindow : Window
{
    private TypeOfAction TypeOfAction;
    public ChangeDataWindow(TypeOfAction typeOfAction, DataTable dataTable)
    {
        InitializeComponent();
        TypeOfAction = typeOfAction;
        if (dataTable != null)
        {
            DataRow row = dataTable.Rows[0];
            
            TextBox1.Text = row["ISBN"].ToString();
            TextBox2.Text = row["name"].ToString();
            TextBox3.Text = row["author"].ToString();
            TextBox4.Text = row["publisher"].ToString();
            TextBox5.Text = row["year"].ToString();

        }

        switch (TypeOfAction)
        {
            case TypeOfAction.Create:
                PreLabel.Content = "Create Data";
                ButtonChange.Content = "Create";
                break;
            case TypeOfAction.Update:
                PreLabel.Content = "Update Data";
                ButtonChange.Content = "Update";
                TextBox1.IsReadOnly = true;
                break;
            case TypeOfAction.Delete:
                PreLabel.Content = "Delete Data";
                ButtonChange.Content = "Delete";
                TextBox1.IsReadOnly = true;
                TextBox2.IsReadOnly = true;
                TextBox3.IsReadOnly = true;
                TextBox4.IsReadOnly = true;
                TextBox5.IsReadOnly = true;
                break;
        }
        
    }


    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        AdoAssistant myAssistant = new AdoAssistant();

        if (TypeOfAction == TypeOfAction.Create)
        {
            if (!checkData())
            {
                return;
            }
            myAssistant.InsertBook(TextBox1.Text, TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text);

            
        }else if (TypeOfAction == TypeOfAction.Update)
        {
            myAssistant.UpdateBook(TextBox1.Text, TextBox2.Text, TextBox3.Text, TextBox4.Text, TextBox5.Text);
        }
        else
        {
            myAssistant.DeleteBook(TextBox1.Text);
        }

        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Hide();
    }
    private void TextBox1_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text) || ((TextBox)sender).Text.Length >= 13;
    }

    private void TextBox5_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        e.Handled = new Regex("[^0-9.]+").IsMatch(e.Text) || ((TextBox)sender).Text.Length >= 4;
    }

    private bool checkData()
    {
        if (TextBox1.Text.IsNullOrEmpty() || TextBox2.Text.IsNullOrEmpty() || TextBox3.Text.IsNullOrEmpty() || TextBox4.Text.IsNullOrEmpty() || TextBox5.Text.IsNullOrEmpty() )
        {
            MessageBox.Show("Заповніть всі дані");
            return false;
        }


        if (TextBox1.Text.Length != 13)
        {
            MessageBox.Show("ISBN повинен містити 13 символів ");
            return false;
        }

        return true;
    }

    private void ChangeDataWindow_OnClosing(object? sender, EventArgs e)
    {
        Close();
    }

    private void BackButton_Click(object sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow();
        mainWindow.Show();
        Hide();
    }
}