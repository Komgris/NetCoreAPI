
using System.Collections.Generic;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IDirectSqlService
    {
        void ExecuteNonQuery(string sql, Dictionary<string, object> parameters);

        string ExecuteReader(string sql, object[] parameters);

    }
}
