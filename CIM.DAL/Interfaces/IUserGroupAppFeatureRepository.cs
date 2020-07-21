using CIM.Domain.Models;
using CIM.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CIM.DAL.Interfaces
{
    public interface IUserGroupAppFeatureRepository : IRepository<UserGroupsAppFeatures, UserGroupAppFeatureModel>
    {
    }
}
