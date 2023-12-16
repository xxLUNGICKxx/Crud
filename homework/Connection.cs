using System;
using Npgsql;
using NpgsqlTypes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.ObjectModel;

namespace homework
{
    public class Connection
    {

        public static ObservableCollection<table> tables { get; set; } = new ObservableCollection<table>();

        private static NpgsqlConnection connection;
        public static void Connect(string host, string port, string user, string pass, string dbname)
        {
            
            string cs = string.Format("Server={0};Port={1};User ID={2};Password={3};DataBase={4}", host, port, user, pass, dbname);

            connection = new NpgsqlConnection(cs);
            connection.Open();
        }
        public static NpgsqlCommand GetCommand(string sql)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = connection;
            command.CommandText = sql;
            return command;
        }

    }
}
