using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace testPostgre
{
    public class DbPostgreManager
    {
        string connectionString = @"Host=localhost;Username=postgres;Password=postgres;Database=postgres";

        public string TestConnection()
        {
            string version = null;
            using (var con = new NpgsqlConnection(connectionString))
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
        public bool CreateNewTable(List<string> headers)
        {
            try
            {
                using (var con = new NpgsqlConnection(connectionString))
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
            catch
            {
                return false;
            }
        }
        public bool InsertData( List<string> headers, List<Row> data, int argsCount)
        {
            try
            {
                foreach (var item in data)
                {
                    if (item.args.Count == argsCount)
                    {
                        using (var con = new NpgsqlConnection(connectionString))
                        {
                            con.Open();

                            using (var cmd = new NpgsqlCommand())
                            {
                                cmd.Connection = con;
                                cmd.CommandText =
                                    @"insert into data(" +
                                    headers[0] + "," + headers[1] + "," + headers[2] + "," + headers[3] + "," + headers[4] + "," +
                                    headers[5] + "," + headers[6] + "," + headers[7] + "," + headers[8] + "," + headers[9] + "," +
                                    headers[10] + "," + headers[11] + "," + headers[12] + "," + headers[13] + "," + headers[14] + "," +
                                    headers[15] + "," + headers[16] + ")" +
                                    "values('" +
                                    item.args[0] + "','" + item.args[1] + "','" + item.args[2] + "','" + item.args[3] + "','" + item.args[4] + "','" +
                                    item.args[5] + "','" + item.args[6] + "','" + item.args[7] + "','" + item.args[8] + "','" + item.args[9] + "','" +
                                    item.args[10] + "','" + item.args[11] + "','" + item.args[12] + "','" + item.args[13] + "','" + item.args[14] + "','" +
                                    item.args[15] + "','" + item.args[16] + "')";

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Row> SelectData()
        {
            List<Row> rows = new List<Row>();
            using (var con = new NpgsqlConnection(connectionString))
            {
                con.Open();

                var sql = @"SELECT * FROM data";
                using (var npgsqlCommand = new NpgsqlCommand(sql, con))
                {
                    using (var npgsqlDataReader = npgsqlCommand.ExecuteReader())
                    {
                        while (npgsqlDataReader.Read())
                        {
                            string result = string.Format("{0} {1} {2}", 
                                npgsqlDataReader.GetInt32(0), 
                                npgsqlDataReader.GetString(1),
                                npgsqlDataReader.GetString(2));
                            List<string> args = new List<string>() {
                                npgsqlDataReader.GetInt32(0).ToString(),
                                npgsqlDataReader.GetString(1),
                                npgsqlDataReader.GetString(2),
                                npgsqlDataReader.GetString(3),
                                npgsqlDataReader.GetString(4),
                                npgsqlDataReader.GetString(5),
                                npgsqlDataReader.GetString(6),
                                npgsqlDataReader.GetString(7),
                                npgsqlDataReader.GetString(8),
                                npgsqlDataReader.GetString(9),
                                npgsqlDataReader.GetString(10),
                                npgsqlDataReader.GetString(11),
                                npgsqlDataReader.GetString(12),
                                npgsqlDataReader.GetString(13),
                                npgsqlDataReader.GetString(14),
                                npgsqlDataReader.GetString(15),
                                npgsqlDataReader.GetString(16),
                                npgsqlDataReader.GetString(17)};
                            rows.Add(new Row(args));
                        }
                    }
                }
            }

            return rows;
        }
    }
}