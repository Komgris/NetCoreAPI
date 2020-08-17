using CIM.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;
using CIM.DAL.Interfaces;
using CIM.Model;

namespace CIM.BusinessLogic.Services
{
    public class UtilitiesService : BaseService, IUtilitiesService
    {
        private readonly IUnitOfWorkCIM _unitOfWork;
        public UtilitiesService(
            IUnitOfWorkCIM unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> UploadImage(IFormFile image, string pathName,bool isPlanDoc)
        {
            string toDbPath = "";
            string pathToSave = "";

            if (image != null)
            {
                if (image.Length > 0)
                {
                    var savePath = isPlanDoc ? DocPath : ImagePath;
                    var folderName = Path.Combine(savePath, pathName);
                    if (!Directory.Exists(folderName))
                    {
                        Directory.CreateDirectory(folderName);
                    }
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"');
                    pathToSave = Path.Combine(folderName, fileName);
                    toDbPath = (Path.Combine(pathName, fileName)).Replace(@"\", "/" );
                    using (var stream = new FileStream(pathToSave, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }
            }
            return isPlanDoc? pathToSave: toDbPath;
        }
    }
}
