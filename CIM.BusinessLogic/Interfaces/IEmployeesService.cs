using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IEmployeesService : IBaseService
    {
        Task<PagingModel<EmployeesModel>> List(string keyword, int page, int howmany, bool isActive);
        Task<EmployeesModel> Create(EmployeesModel model);
        Task<EmployeesModel> Update(EmployeesModel model);
        Task<EmployeesModel> Get(int id);
    }
}
