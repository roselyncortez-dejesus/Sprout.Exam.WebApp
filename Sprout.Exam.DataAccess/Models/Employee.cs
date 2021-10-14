using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprout.Exam.DataAccess.Models
{
    [Table("Employee")]
    public class Employee
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(128)")]
        public string FullName { get; set; }

        [Required]
        public string Birthdate { get; set; }

        [Required]
        public string Tin { get; set; }

        [Required]
        [ForeignKey("EmployeeType")]
        public int TypeId { get; set; }
        public EmployeeType EmployeeType { get; set; }


        [DefaultValue(false)]
        [Column(TypeName = "bit")]
        public bool IsDeleted { get; set; }
    }
}
