using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Security.Principal;
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
using System.Xml.Linq;

namespace homework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NpgsqlConnection connection;
        ObservableCollection<table> tables { get; set; } = new ObservableCollection<table>();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            string connectionString = "host=localhost;database=homework;port=5432;username=postgres;password=123";
            connection = new NpgsqlConnection(connectionString);
            connection.Open();
            LvList.SetBinding(ItemsControl.ItemsSourceProperty, new Binding()
            {
                Source = tables
            });

            SelectAll();

            //ListWithInf();
        }



        private NpgsqlCommand GetCommand(string command)
        {
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(command);
            npgsqlCommand.Connection = connection;
            return npgsqlCommand;
        }

        private void SelectAll()
        {
            NpgsqlCommand command = GetCommand("SELECT * FROM \"table\" order by id");
            var result = command.ExecuteReader();
            if (result.HasRows)
            {
                while (result.Read())
                {
                    tables.Add(new table(result.GetInt32(0), result.GetString(1)));
                }
            }
            result.Close();
        }


        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

            NpgsqlCommand cmd = GetCommand("INSERT INTO \"table\" (\"name\") VALUES (@name)");
            cmd.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, BoxWithName.Text.Trim());
            
            var result = cmd.ExecuteScalar();
            SelectAll();

        }

        private void ButtomDel_Click(object sender, RoutedEventArgs e)
        {
            var selected = tables.Single(c => c.Equals(LvList.SelectedItem));
            NpgsqlCommand command = GetCommand($"DELETE FROM \"table\" WHERE id = \'{selected.Id}\'");
            command.ExecuteNonQuery();
            tables.Clear();
            SelectAll();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var selected = tables.Single(c => c.Equals(LvList.SelectedItem));
           
            NpgsqlCommand command = GetCommand("UPDATE \"table\"" +
                $"SET \"name\" = '{BoxWithName.Text}'" +
                $"WHERE \"id\" = '{selected.Id}'");
            command.ExecuteNonQuery();
            tables.Clear();
            SelectAll();
        }

        private void LvList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = tables.Single(c => c.Equals(LvList.SelectedItem));
            BoxWithName.Text = selected.Name.ToString();
        }
    }
}
