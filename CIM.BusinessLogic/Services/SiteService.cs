using System;
using System.Collections.Generic;
using CIM.DAL.Interfaces;
using CIM.Model;
using System.Linq;
using CIM.BusinessLogic.Interfaces;

namespace CIM.BusinessLogic.Services
{
    public class SiteService : ISiteService
    {

        private readonly ISiteRepository _siteRepository;
        private readonly IUnitOfWorkCIM _unitOfWork;

        public SiteService(
            ISiteRepository siteRepository,
            IUnitOfWorkCIM unitOfWork
            )
        {
            _siteRepository = siteRepository;
            _unitOfWork = unitOfWork;
        }

        public List<SiteModel> List()
        {
            return _siteRepository.All().Select(x => new SiteModel { 
                Id = x.Id
            }).ToList();
        }
    }
}
