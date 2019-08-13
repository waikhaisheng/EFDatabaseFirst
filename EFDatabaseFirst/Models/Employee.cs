using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EFDatabaseFirst.Models
{
    [MetadataType(typeof(EmployeeMetaData))]
    public partial class tblEmployee
    {
    }

    public class EmployeeMetaData
    {
        //[Required]
        public string Name { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
    }
}