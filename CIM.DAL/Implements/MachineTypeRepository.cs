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
    public class MachineTypeRepository : Repository<MachineType, object>, IMachineTypeRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public MachineTypeRepository(cim_3m_1Context context, IDirectSqlRepository directSqlRepository,IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<PagingModel<MachineTypeModel>> List(string keyword, int page, int howMany, bool isActive)
        {
            return await Task.Run(() =>
            {
                Dictionary<string, object> parameterList = new Dictionary<string, object>()
                                        {
                                            {"@keyword", keyword},
                                            {"@howmany", howMany},
                                            { "@page", page},
                                            { "@is_active", isActive}
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListMachineType", parameterList);
                var totalCount = 0;
                if (dt.Rows.Count > 0)
                    totalCount = Convert.ToInt32(dt.Rows[0]["TotalCount"] ?? 0);

                return ToPagingModel(dt.ToModel<MachineTypeModel>(), totalCount, page, howMany);
            });

        }
    }
}
