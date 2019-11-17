using System;
using System.Collections.Generic;
using System.Text;
using CIM.DAL.Implements;
using CIM.DAL.Interfaces;
using CIM.Model;
using System.Linq;

namespace CIM.BusinessLogic
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
