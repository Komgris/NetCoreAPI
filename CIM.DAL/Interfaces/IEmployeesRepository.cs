using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IEmployeesRepository : IRepository<Employees>
    {
        Task<PagingModel<EmployeesModel>> List(string keyword, int page, int howMany, bool isActive);
    }
}
