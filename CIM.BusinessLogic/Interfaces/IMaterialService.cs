using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IMaterialService : IBaseService
    {
        Task<PagingModel<MaterialModel>> List(int page, int howmany);
        void Create(MaterialModel model);
        void Update(MaterialModel model);
        Task<MaterialModel> Get(int id);
    }
}
