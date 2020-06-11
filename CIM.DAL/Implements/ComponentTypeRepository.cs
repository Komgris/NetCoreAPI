using CIM.DAL.Interfaces;
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
    public class ComponentTypeRepository : Repository<ComponentType>, IComponentTypeRepository
    {
        private IDirectSqlRepository _directSqlRepository;
        public ComponentTypeRepository(cim_dbContext context, IDirectSqlRepository directSqlRepository, IConfiguration configuration) : base(context, configuration)
        {
            _directSqlRepository = directSqlRepository;
        }

        public async Task<List<ComponentTypeModel>> ListComponentTypeByMachineType(int machineTypeId)
        {
            return await Task.Run(() =>
            {
                var parameterList = new Dictionary<string, object>()
                                        {
                                            {"@machinetype_id", machineTypeId},
                                        };

                var dt = _directSqlRepository.ExecuteSPWithQuery("sp_ListMachineTypeComponentType", parameterList);

                return (dt.ToModel<ComponentTypeModel>());
            });
        }
    }
}
