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
    public class UserRepository : Repository<Users, UserModel>, IUserRepository
    {
        public UserRepository(cim_dbContext context, IConfiguration configuration ) : base(context, configuration)
        {
        }
        public async Task<PagingModel<UserModel>> List(string keyword, int page, int howmany)
        {
            var query = _entities.Users.Where(x => x.IsDelete == false);

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    x.UserName.Contains(keyword)
                );
            }
            var data = await query
                .Select(x => new UserModel
                {
                    Email = x.Email,
                    FirstName = "",
                    LastName = "",
                    Id = x.Id,
                    DefaultLanguageId = x.DefaultLanguageId,
                    UserGroupId = x.UserGroupId,
                    UserName = x.UserName
                })
                .ToListAsync();
            return new PagingModel<UserModel>
            {
                Data = data
            };

        }

    }
}
