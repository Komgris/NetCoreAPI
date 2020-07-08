﻿using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IRecordMaintenancePlanService : IBaseService
    {
        Task<List<RecordMaintenancePlanModel>> ListByMonth(int month, int year, bool isActive);
        Task<List<RecordMaintenancePlanModel>> ListByDate(DateTime date);
        Task<RecordMaintenancePlanModel> Get(int id);
        Task Create(RecordMaintenancePlanModel data);
        Task Update(RecordMaintenancePlanModel data);
    }
}
