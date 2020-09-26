using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface ILossLevel2Repository : IRepository<LossLevel2, LossLevel2Model>
    {
        Task<PagingModel<LossLevel2ListModel>> List(int page, int howmany, string keyword, bool isActive);
    }
}