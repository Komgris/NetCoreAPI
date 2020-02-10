using CIM.BusinessLogic.Interfaces;
using CIM.DAL.Interfaces;
using CIM.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CIM.BusinessLogic.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        public MaterialService(
            IUnitOfWorkCIM unitOfWork,
            IMaterialRepository materialRepository
            )
        {
            _materialRepository = materialRepository;
        }
        public List<MaterialModel> List()
        {
            var result = _materialRepository.List();
            return result;
        }

        public void Insert(MaterialModel model)
        {
            _materialRepository.Insert(model);
        }

        public Task<PagingModel<MaterialModel>> Paging(int page, int howmany)
        {
            var result = _materialRepository.Paging(page, howmany);
            return result;
        }

        public void Update(MaterialModel model)
        {
            _materialRepository.Update(model);
        }

        public MaterialModel Get(int id)
        {
            var result = _materialRepository.Get(id);
            return result;
        }
    }
}
