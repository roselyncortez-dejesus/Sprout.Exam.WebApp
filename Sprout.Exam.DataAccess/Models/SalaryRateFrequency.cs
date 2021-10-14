using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.DataAccess.Models
{
    [Table("SalaryRateFrequency")]
    public class SalaryRateFrequency
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(32)")]
        public string FrequencyName { get; set; }
    }
}
