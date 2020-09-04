using Microsoft.Win32;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace testPostgre
{
    public partial class MainWindow : Window
    {
        readonly List<string> columnHeaders = new List<string>
        { "oneA", "oneB", "oneC", "oneD", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen" };
        TextData textData;
        MainTableData tableData;
        DbPostgreManager postgreManager;

        public MainWindow()
        {
            InitializeComponent();
            postgreManager = new DbPostgreManager();
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
                    var rows = TextFileManager.Read(openFileDialog.FileName);
                    textData = new TextData(rows);
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
                var version = postgreManager.TestConnection();
                MessageBox.Show(version,"версия базы данных");

                var tableIsCreate = postgreManager.CreateNewTable(columnHeaders);
                if (version != null &&
                    textData.rows.Count > 0 &&
                    tableIsCreate)
                {
                    postgreManager.InsertData(columnHeaders, textData.rows, 17);
                    textData = null;
                    MessageBox.Show("Таблица создана и данные загружены");
                }
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к базе");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rows = postgreManager.SelectData();
            tableData = new MainTableData(rows);
            dgMainTable.ItemsSource = tableData.rows[0].args;
        }
    }
}