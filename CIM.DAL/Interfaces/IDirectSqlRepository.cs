using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CIM.DAL.Interfaces
{
    public interface IDirectSqlRepository
    {

        void ExecuteNonQuery(string sql, object[] parameters);

        string ExecuteReader(string sql, object[] parameters);

        DataTable ExecuteSPWithQuery(string sql, List<SqlParameter> parameters);
        DataTable ExecuteWithQuery(string sql);
    }
}
