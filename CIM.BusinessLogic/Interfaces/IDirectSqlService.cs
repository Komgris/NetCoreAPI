﻿using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IDirectSqlService
    {
        void ExecuteNonQuery(string sql, object[] parameters);
        string ExecuteReader(string sql, object[] parameters);
        List<LossLevel1Model> All();
        void LossLevel1Insert(LossLevel1Model parameters);
    }
}
