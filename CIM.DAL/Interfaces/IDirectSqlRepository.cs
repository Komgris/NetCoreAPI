using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Interfaces
{
    public interface IDirectSqlRepository
    {

        void ExecuteNonQuery(string sql, object[] parameters);

        string ExecuteReader(string sql, object[] parameters);
    }
}
