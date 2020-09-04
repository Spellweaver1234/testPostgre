using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testPostgre
{
    class MainTableData
    {
        public List<Row> rows = new List<Row>();
        public MainTableData(List<Row> rows)
        {
            this.rows = rows;
        }
    }
}
