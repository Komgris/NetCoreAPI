using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CIM.DAL.Implements
{
    public class ColorRepository : Repository<Color,ColorModel>, IColorRepository
    {
        public ColorRepository(cim_3m_1Context context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {

        }
    }
}
