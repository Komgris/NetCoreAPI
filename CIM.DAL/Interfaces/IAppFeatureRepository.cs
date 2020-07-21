﻿using CIM.Domain.Models;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Interfaces
{
    public interface IAppFeatureRepository : IRepository<AppFeatures, AppFeatureModel>
    {
    }
}
