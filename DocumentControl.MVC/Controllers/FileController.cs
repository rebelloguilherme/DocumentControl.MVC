using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentControl.MVC.Data;
using DocumentControl.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocumentControl.MVC.Controllers
{
    public class FileController : Controller
    {
        private readonly ApplicationDbContext context;

        public FileController(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<IActionResult> Index()
        {
            var fileuploadViewModel = await LoadAllFiles();
            ViewBag.Message = TempData["Message"];
            return View(fileuploadViewModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, int code, string title, string process, string category)
        {
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);

                if (extension != ".pdf" && extension != ".doc" && extension != ".xls" &&
                    extension != ".docx" && extension != ".xlsx")
                {
                    TempData["Message"] = "File Extension is not Supported";
                    return RedirectToAction("Index");
                }
                else
                {
                    var fileModel = new FileOnDatabaseModel
                    {
                        Code = code,
                        Title = title,
                        FileType = file.ContentType,
                        Process = process,
                        Category = category,
                        Extension = extension

                    };
                    using (var dataStream = new MemoryStream())
                    {
                        await file.CopyToAsync(dataStream);
                        fileModel.Data = dataStream.ToArray();
                    }
                    context.FilesOnDatabase.Add(fileModel);
                    context.SaveChanges();
                    
                }
                
            }
            TempData["Message"] = "File Saved Sucessfully";
            return RedirectToAction("Index");
        }

        private async Task<FileUploadViewModel> LoadAllFiles()
        {
            var viewModel = new FileUploadViewModel();
            viewModel.FilesOnDatabase = await context.FilesOnDatabase.ToListAsync();    
            return viewModel;
        }

        public async Task<IActionResult> DownloadFileFromDatabase(int id)
        {

            var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (file == null) return null;
            return File(file.Data, file.FileType, file.Title+file.Extension);
        }
        public async Task<IActionResult> DeleteFileFromDatabase(int id)
        {

            var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
            context.FilesOnDatabase.Remove(file);
            context.SaveChanges();
            TempData["Message"] = $"Removed {file.Title + file.Extension} successfully from Database.";
            return RedirectToAction("Index");
        }
    }
}