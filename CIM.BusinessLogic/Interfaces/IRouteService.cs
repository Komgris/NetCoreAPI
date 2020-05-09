using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRouteService : IBaseService
    {
        Task Create(RouteListModel data);
        Task<PagingModel<RouteListModel>> List(string keyword, int page, int howmany);
        Task<RouteListModel> Get(int id);
        Task Update(RouteListModel data);
    }
}
