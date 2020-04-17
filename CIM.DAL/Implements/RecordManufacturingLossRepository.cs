﻿using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class RecordManufacturingLossRepository : Repository<RecordManufacturingLoss>, IRecordManufacturingLossRepository
    {
        public RecordManufacturingLossRepository(cim_dbContext context) : base(context)
        {

        }
    }
}