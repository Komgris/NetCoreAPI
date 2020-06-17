using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IAccidentService
    {
        Task<AccidentModel> Get(int id);
        Task<List<AccidentModel>> List(string keyword = "", int page = 1, int howmany = 10, bool isActive = true);
        Task Create(AccidentModel model);
        Task Update(AccidentModel model);
        Task Delete(int id);
    }
}
