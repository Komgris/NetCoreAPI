using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IAccidentService : IBaseService
    {
        Task<AccidentModel> Get(int id);
        Task<PagingModel<AccidentModel>> List(string keyword = "", int page = 1, int howmany = 10);
        Task Create(AccidentModel model);
        Task Update(AccidentModel model);
        Task Delete(int id);
        Task End(int id);
    }
}
