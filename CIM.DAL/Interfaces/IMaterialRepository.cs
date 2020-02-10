using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IMaterialRepository
    {
        Task<PagingModel<MaterialModel>> Paging(int page, int howmany);
        List<MaterialModel> List();
        void Insert(MaterialModel model);
        void Update(MaterialModel model);
    }
}
