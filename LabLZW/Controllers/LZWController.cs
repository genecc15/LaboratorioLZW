using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LabLZW.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LZWController : ControllerBase
    {
        public static IWebHostEnvironment _environment;
        private readonly LZWMetodos LZWCompresion = new LZWMetodos();
        private readonly LZWMetodos LZWDesc = new LZWMetodos();

        public LZWController(IWebHostEnvironment env)
        {
            _environment = env;
        }
        public class FileUploadAPI
        {
            public IFormFile Files { get; set; }
        }

        [Route("/Compress/{id}/LZW")]
        [HttpPost]
        public async Task<string> UploadFileText([FromForm] FileUploadAPI objFile, string id)
        {
            try
            {
                if (objFile.Files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\UploadLZW\\")) Directory.CreateDirectory(_environment.WebRootPath + "\\UploadLZW\\");
                    using var _fileStream = System.IO.File.Create(_environment.WebRootPath + "\\UploadLZW\\" + objFile.Files.FileName);
                    objFile.Files.CopyTo(_fileStream);
                    _fileStream.Flush();
                    _fileStream.Close();

                    LZWCompress(objFile, id);
                    return "\\UploadLZW\\" + objFile.Files.FileName;
                }
                else return "Archivo Vacio";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }
        public void LZWCompress(FileUploadAPI objFile, string id)
        {
            string[] FileName1 = objFile.Files.FileName.Split(".");
            LZWMetodos.LZWAlgoritmo(_environment.WebRootPath + "\\UploadLZW\\" + objFile.Files.FileName, _environment.WebRootPath + "\\UploadLZW\\" + id + ".lzw", _environment.WebRootPath + "\\UploadLZW\\" +"Compresiones.txt");
        }
        [Route("/Decompress/LZW")]
        [HttpPost]
        public async Task<string> UploadFileLZW([FromForm] FileUploadAPI objFile)
        {
            try
            {
                if (objFile.Files.Length > 0)
                {
                    if (!Directory.Exists(_environment.WebRootPath + "\\UploadLZW\\")) Directory.CreateDirectory(_environment.WebRootPath + "\\UploadLZW\\");
                    using var _fileStream = System.IO.File.Create(_environment.WebRootPath + "\\UploadLZW\\" + objFile.Files.FileName);
                    objFile.Files.CopyTo(_fileStream);
                    _fileStream.Flush();
                    _fileStream.Close();
                    LZWDecompress(objFile);

                    return "\\UploadLZW\\" + objFile.Files.FileName;

                }
                else
                {
                    return "Archivo vacío.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public void LZWDecompress(FileUploadAPI LZWFile)
        {

            string[] FileName1 = LZWFile.Files.FileName.Split(".");
            LZWMetodos.LZWAlgoritmo2(_environment.WebRootPath + "\\UploadLZW\\" + LZWFile.Files.FileName, _environment.WebRootPath + "\\UploadLZW\\" + FileName1[0] + ".txt");

        }
    }
}