using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Riots.Standard.Data
{
    interface iDataMethod
    {
        DataTable ToQueryDataTable(string sql);

        string SetConnectionString(string text);
    }
}
