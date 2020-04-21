using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IDirectSqlService
    {
        void ExecuteNonQuery(string sql, object[] parameters);

        string ExecuteReader(string sql, object[] parameters);

    }
}
