using Bytescout.PDFExtractor;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace sample1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class formController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        //public List<IFormFile> Files { get; set; }
        private string fileName;

        public class Document
        {
            public int DocId { get; set; }
            public string DocName { get; set; }
            public byte[] DocContent { get; set; }
        }

        /* public Document FileToByteArray(List<IFormFile> fileName)
         {
             byte[] fileContent = null;
             System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
             System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
             long byteLength = new System.IO.FileInfo(fileName).Length;
             fileContent = binaryReader.ReadBytes((Int32)byteLength);
             fs.Close();
             fs.Dispose();
             binaryReader.Close();
             Document Document = new Document();
             Document.DocName = fileName;
             Document.DocContent = fileContent;
             return Document;
         }
        */
       /*  private void ShowDocument(List<Document> fileName, byte[] fileContent)
         {
             //Split the string by character . to get file extension type  
             string[] stringParts = fileName.Split(new char[] { '.' });
             string strType = stringParts[1];
             Response.Clear();
             Response.ClearContent();
             Response.ClearHeaders();
             Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
             //Set the content type as file extension type  
             Response.ContentType = strType;
             //Write the file content  
             this.Response.BinaryWrite(fileContent);
             this.Response.End();
         }
        */

        public formController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("[action]")]
        public  IActionResult uploadFiles(List<IFormFile> files, int num)
        {

            if (files.Count == 0)
                return BadRequest();
            string directoryPath = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadFiles");
            //string directoryPath_id = Path.Combine(_webHostEnvironment.ContentRootPath, "uploadFiles_id");
            foreach (var file in files)
             {

                 string filePath = Path.Combine(directoryPath, file.FileName);
                 using (var stream = new FileStream(filePath, FileMode.Create))
                 {
                     file.CopyTo(stream);
                    //FileToByteArray(files);


                 } 
            }
           

                //var json = FileToByteArray(directoryPath);
                //ShowDocument(json);
               var webClient = new WebClient();
               var json = webClient.DownloadString("Wrapper_API_Response.json");
            return Ok (json);


        }
       
       
        

        private List<Document> FileToByteArray(string folderPath)
        {
            byte[] fileContent = null;
            //fileName = files[0].FileName;
            Document Document = new Document();
            List<Document> _modelList = new List<Document>();
            foreach (string file in Directory.EnumerateFiles(folderPath))
            {                
                string contents = System.IO.File.ReadAllText(file);
                string[] arrFileName = new string[] { };
                arrFileName = file.Split("\\");
                Document = new Document();
                Document.DocName = arrFileName[arrFileName.Length - 1];
                System.IO.FileStream fs = new System.IO.FileStream(file, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
               
                //long byteLength = new System.IO.FileInfo(fileName).Length;
                //fileContent = binaryReader.ReadBytes((Int32)byteLength);
                Byte[] bytes = binaryReader.ReadBytes((Int32)fs.Length);
               // string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                Document.DocContent = bytes;
                _modelList.Add(Document);
            }
            return _modelList;
        }
    }
}
