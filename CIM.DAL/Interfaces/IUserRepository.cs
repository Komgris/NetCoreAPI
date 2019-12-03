using CIM.Domain.Models;
using CIM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IUserRepository : IRepository<Users>
    {
        Task<PagingModel<UserModel>> List(string keyword, int page, int howmany);
    }
}
