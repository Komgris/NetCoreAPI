﻿using System;
using System.Collections.Generic;
using System.Text;

using CIM.Model;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Interfaces
{
    public interface ILossLevel3Service : IBaseService
    {
        Task<PagingModel<LossLevel3ListModel>> List(string keyword, int page, int howmany, bool isActive);
        Task<LossLevel3Model> Create(LossLevel3EditableModel model);
        Task<LossLevel3Model> Update(LossLevel3EditableModel model);
        Task<LossLevel3EditableModel> Get(int id);
    }
}
