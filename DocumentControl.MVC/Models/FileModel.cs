using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentControl.MVC.Models
{
    public abstract class FileModel
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public string FileType { get; set; }
        public string Process { get; set; }
        public string Category { get; set; }
        public string Extension { get; set; }
      
    }
}
