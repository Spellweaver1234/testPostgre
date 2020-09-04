using System.Collections.Generic;

namespace testPostgre
{
    public class TextData
    {
        public List<Row> rows = new List<Row>();
        public TextData(List<Row> rows)
        {
            this.rows = rows;
        }
    }
}
