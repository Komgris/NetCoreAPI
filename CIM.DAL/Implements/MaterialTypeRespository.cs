using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class MaterialTypeRespository : Repository<MaterialType, object>, IMaterialTypeRepository
    {
        public MaterialTypeRespository(cim_3m_1Context context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            
        }
    }
}
