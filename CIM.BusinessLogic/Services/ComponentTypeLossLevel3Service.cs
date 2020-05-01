using System;
using System.Collections.Generic;
using System.Text;

using CIM.BusinessLogic.Interfaces;
using CIM.BusinessLogic.Utility;
using CIM.DAL.Interfaces;
using CIM.Domain.Models;
using CIM.Model;
using System.Linq;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class ComponentTypeLossLevel3Service : BaseService, IComponentTypeLossLevel3Service
    {
        private readonly IComponentTypeLossLevel3Repository _componentTypeLossLevel3Repository;
        private IUnitOfWorkCIM _unitOfWork;

        public ComponentTypeLossLevel3Service(
            IUnitOfWorkCIM unitOfWork,
            IComponentTypeLossLevel3Repository componentTypeLossLevel3Repository
            )
        {
            _componentTypeLossLevel3Repository = componentTypeLossLevel3Repository;
            _unitOfWork = unitOfWork;
        }
    }
}
