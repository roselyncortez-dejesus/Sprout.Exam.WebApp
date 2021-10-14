using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprout.Exam.DataAccess.Models
{
    [Table("EmployeeSalary")]
    public class EmployeeSalary
    {
        [Required]
        public int EmployeeSalaryId { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,4)")]
        public decimal BasicRate { get; set; }

        [DefaultValue(1)]
        [ForeignKey("SalaryRateFrequency")]
        public int FrequencyId { get; set; }
        public SalaryRateFrequency SalaryRateFrequency { get; set; }

        [DefaultValue(0)]
        [Column(TypeName = "decimal(10,4)")]
        public int TaxRatePercentage { get; set; }

        public DateTime CreatedDateTime { get; set; }
        public DateTime LastModifiedDateTime { get; set; }


    }
}
