using CIM.Domain.Models;
using CIM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IUserGroupRepository : IRepository<UserGroups, UserGroupModel>
    {
    }
}
