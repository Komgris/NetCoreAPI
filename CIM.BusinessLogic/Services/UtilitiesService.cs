﻿using CIM.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net.Http.Headers;
using CIM.DAL.Interfaces;

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

        public async Task<string> UploadImage(IFormFile image, string pathName)
        {
            string toDbPath = "";
            if (image != null)
            {
                if (image.Length > 0)
                {
                    var savePath = @"C:\Image";
                    var folderName = Path.Combine(savePath, pathName);
                    var serverPath = Path.Combine("http://localhost/Image/Image/", pathName);
                    var fileName = ContentDispositionHeaderValue.Parse(image.ContentDisposition).FileName.Trim('"');
                    var pathToSave = Path.Combine(folderName, fileName);
                    toDbPath = Path.Combine(serverPath, fileName);
                    using (var stream = new FileStream(pathToSave, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                }
            }
            return toDbPath;
        }
    }
}
