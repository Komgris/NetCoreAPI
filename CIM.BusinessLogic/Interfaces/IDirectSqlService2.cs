//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace CIM.BusinessLogic.Interfaces
//{
//    interface IDirectSqlService2
//    {
//    }
//}

using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IDirectSqlService2
    {
        void ExecuteNonQuery(string sql, object[] parameters);
        string ExecuteReader(string sql, object[] parameters);
        List<LossLevel1Model> All();
        void LossLevel2Insert(LossLevel1Model parameters);

    }
}
