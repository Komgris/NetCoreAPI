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
    public class UserRepository : Repository<Users, object>, IUserRepository
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
                    //|| x.UserProfiles.Any(profile =>
                    //    profile.FirstName.Contains(keyword) ||
                    //    profile.LastName.Contains(keyword))
                );
            }
            var data = await query
                .Select(x=> new UserModel { 
                    Email = x.Email,
                    FirstName = "",
                    LastName = "",
                    Id = x.Id,
                    LanguageId = x.DefaultLanguageId,
                    UserGroupId = x.UserGroupId,
                    UserName = x.UserName,
                    Image = null
                })
                .ToListAsync();
            return new PagingModel<UserModel>
            {
                Data = data
            };

        }

    }
}
