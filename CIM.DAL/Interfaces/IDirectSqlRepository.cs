using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Interfaces
{
    public interface IDirectSqlRepository
    {

        void ExecuteNonQuery(string sql, object[] parameters);
        string ExecuteReader(string sql, object[] parameters);
        public List<LossLevel1Model> LossLevel1ListAll();
        void LossLevel1Insert(LossLevel1Model lossLevel1);
        void LossLevel2Insert(LossLevel1Model lossLevel2);
    }
}
