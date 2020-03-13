//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace CIM.DAL.Interfaces
//{
//    interface IDirectSqlRepository2
//    {
//    }
//}
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CIM.DAL.Interfaces
{
    public interface IDirectSqlRepository2
    {

        void ExecuteNonQuery(string sql, object[] parameters);
        string ExecuteReader(string sql, object[] parameters);
        public List<LossLevel1Model> LossLevel1ListAll();
        public void LossLevel1Insert(LossLevel1Model lossLevel1);
        public void LossLevel2Insert(LossLevel1Model lossLevel2);

        public DataTable Query(String sql);
        public bool NonQuery(String sql);
    }
}
