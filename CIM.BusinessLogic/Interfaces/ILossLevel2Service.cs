//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace CIM.BusinessLogic.Interfaces
//{
//    interface ILossLevel2Service
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
    public interface ILossLevel2Service
    {
        void ExecuteNonQuery(string sql, object[] parameters);
        string ExecuteReader(string sql, object[] parameters);
        List<LossLevel2Model> ListAllLossLevel2();
        void InsertLossLevel2(LossLevel2Model parameters);

    }
}