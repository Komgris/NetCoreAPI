using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CIM.Model
{
    public class OverallDashboard
    {
        Dictionary<string, DataTable> DashboardData = new Dictionary<string, DataTable>()
        {
            { "active-process",new DataTable() }
        };
    }
}
