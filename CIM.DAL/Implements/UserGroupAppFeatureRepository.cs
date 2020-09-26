using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace CIM.DAL.Implements
{
    public class UserGroupAppFeatureRepository : Repository<UserGroupsAppFeatures, UserGroupAppFeatureModel>, IUserGroupAppFeatureRepository
    {
        public UserGroupAppFeatureRepository(cim_3m_1Context context, IConfiguration configuration ) : base(context, configuration)
        {
        }
    }
}
