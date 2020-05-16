using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CIM.DAL.Interfaces
{
    public interface IDirectSqlRepository
    {

        int ExecuteNonQuery(string sql);

        string ExecuteReader(string sql, object[] parameters);

        DataTable ExecuteSPWithQuery(string sql, Dictionary<string, object> parameters);

        int ExecuteSPNonQuery(string sql, Dictionary<string, object> parameters);

        T ExecuteFunction<T>(string sql, Dictionary<string, object> parameters);

        DataTable ExecuteWithQuery(string sql);
    }
}
