using CIM.BusinessLogic.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class AccidentService : BaseService, IAccidentService
    {
        public Task Create(AccidentModel model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AccidentModel> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AccidentModel>> List(string keyword = "", int page = 1, int howmany = 10, bool isActive = true)
        {
            throw new NotImplementedException();
        }

        public Task Update(AccidentModel model)
        {
            throw new NotImplementedException();
        }
    }
}
