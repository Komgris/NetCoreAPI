using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
     public interface IComponentTypeLossLevel3Repository : IRepository<ComponentTypeLossLevel3>
    {
        Task<PagingModel<ComponentTypeLossLevel3ListModel>> List(int? componentTypeId, int? lossLevel3Id, int page, int howmany);
    }
}
