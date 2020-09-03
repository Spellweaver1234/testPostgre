using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testPostgre
{
    public class Row
    {
        public List<string> args;

        public Row(List<string> args)
        {
            this.args = args;
        }
    }
}
