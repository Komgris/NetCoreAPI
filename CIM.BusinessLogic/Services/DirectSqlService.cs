﻿using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.BusinessLogic.Services
{
    public class DirectSqlService : IDirectSqlService
    {
        private IDirectSqlRepository _directSqlRepository;

        public DirectSqlService(IDirectSqlRepository directSqlRepository)
        {
            _directSqlRepository = directSqlRepository;
        }
        public void ExecuteNonQuery(string sql, object[] parameters = null)
        {
            _directSqlRepository.ExecuteNonQuery(sql, parameters);
        }

        public string ExecuteReader(string sql, object[] parameters = null)
        {
            return _directSqlRepository.ExecuteReader(sql, parameters);
        }
        public List<LossLevel1Model> All()
        {
            return _directSqlRepository.LossLevel1ListAll();
            //throw new NotImplementedException();
        }
        public void LossLevel1Insert(LossLevel1Model lossLevel1)
        {

            //object[] parameters;
            //parameters = new string[] { "value1", "value2" };

            _directSqlRepository.LossLevel1Insert(lossLevel1);
        }
    }
}
