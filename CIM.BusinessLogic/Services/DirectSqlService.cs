using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using System.Collections.Generic;

namespace CIM.BusinessLogic.Services
{
    public class DirectSqlService : IDirectSqlService
    {
        private IDirectSqlRepository _directSqlRepository;

        public DirectSqlService(IDirectSqlRepository directSqlRepository)
        {
            _directSqlRepository = directSqlRepository;
        }

        public void ExecuteNonQuery(string sql, Dictionary<string, object> parameters = null)
        {
            _directSqlRepository.ExecuteNonQuery(sql);
        }

        public string ExecuteReader(string sql, object[] parameters = null)
        {
            return _directSqlRepository.ExecuteReader(sql, parameters);
        }
    }
}
