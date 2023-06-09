﻿using CIM.Domain.Models;
using CIM.Model;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IRouteRepository : IRepository<Route, object> 
    {
        Task<PagingModel<RouteListModel>> List(int page, int howmany, string keyword, bool isActive, int? processTypeId);
    }
}
