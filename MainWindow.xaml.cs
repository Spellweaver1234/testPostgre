using Microsoft.Win32;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace testPostgre
{
    public partial class MainWindow : Window
    {
        readonly string connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";
        readonly List<string> columnHeaders = new List<string> 
        { "oneA", "oneB", "oneC", "oneD", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen" };
        public List<Row> rows = new List<Row>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    rows = ReadFile(openFileDialog.FileName);
                    MessageBox.Show("Файл прочитан успешно");
                }
                catch
                {
                    MessageBox.Show("Не удалось прочитать файл");
                }
            }
        }
        private void btnLoadFileToDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var version = TestConnection(connectionString);
                if (version != null)
                {
                    MessageBox.Show(version, "Подключение успешное");

                    var result = LoadDataToDB(connectionString, rows, columnHeaders);

                    if (result)
                    {
                        MessageBox.Show("Данные добавлены");
                    }
                    else
                    {
                        MessageBox.Show("Данные добавлены");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к базе");
            }
        }

        private List<Row> ReadFile(string fileName)
        {
            var result=new List<Row>();
            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var args = line.Split(new char[] { '|' }).Where(x => x != "").ToList();
                    result.Add(new Row(args));
                }
            }

            return result;
        }
        private string TestConnection(string connString)
        {
            string version = null;
            using (var con = new NpgsqlConnection(connString))
            {
                con.Open();
                var sql = "SELECT version()";

                using (var cmd = new NpgsqlCommand(sql, con))
                {
                    version = cmd.ExecuteScalar().ToString();
                }
            }

            return version;
        }
        private bool LoadDataToDB(string connString, List<Row> data, List<string> headers)
        {
            if (data.Count > 0)
            {
                using (var con = new NpgsqlConnection(connString))
                {
                    con.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "DROP TABLE IF EXISTS data";
                        cmd.ExecuteNonQuery();

                        cmd.CommandText = @"CREATE TABLE data(id SERIAL PRIMARY KEY";
                        foreach (var item in headers)
                        {
                            cmd.CommandText += ", " + item + " VARCHAR(255)";
                        }
                        cmd.CommandText += ")";
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}