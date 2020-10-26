﻿using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface IProductionPlanCheckListService : IBaseService
    {
        Task<List<ProductionPlanCheckListModel>> List(int machineTypeId, int CheckListTypeId);
    }
}