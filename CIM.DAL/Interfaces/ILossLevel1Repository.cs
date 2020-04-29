using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface ILossLevel1Repository : IRepository<LossLevel1>
    {
        Task<PagingModel<LossLevel1Model>> List(int page, int howmany, string keyword, bool isActive);
    }
}
