using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testPostgre
{
    public static class TextFileManager
    {
        public static List<Row> Read(string path)
        {
            var result = new List<Row>();
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (line[0] != '*' && line[0] != '#' && !line.Contains("ТБ=01"))
                    {
                        var args = line.Split(new char[] { '|' }).Where(x => x != "").ToList();
                        result.Add(new Row(args));
                    }
                }
            }

            return result;
        }
    }
}
