using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sprout.Exam.DataAccess.Models
{
    [Table("EmployeeType")]
    public class EmployeeType
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(32)")]
        public string TypeName { get; set; }
    }
}
