﻿using CIM.DAL.Interfaces;
using CIM.DAL.Utility;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.DAL.Implements
{
    public class EmployeesRepository : Repository<Employees>, IEmployeesRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public EmployeesRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<EmployeesModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                    {
                        {"@keyword", keyword},
                        {"@howmany", howMany},
                        {"@page", page},
                        {"@is_active", isActive}
                    };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListEmployees", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<EmployeesModel>(), totalCount, page, howMany);
            });
        }

    }
}
