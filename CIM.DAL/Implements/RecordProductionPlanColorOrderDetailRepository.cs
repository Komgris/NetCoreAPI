﻿using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class RecordProductionPlanColorOrderDetailRepository : Repository<RecordProductionPlanColorOrderDetail, RecordProductionPlanColorOrderDetailModel>, IRecordProductionPlanColorOrderDetailRepository
    {
        public RecordProductionPlanColorOrderDetailRepository(cim_3m_1Context context, IConfiguration configuration) : base(context, configuration)
        {

        }
    }
}
