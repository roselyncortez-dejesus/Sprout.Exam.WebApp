namespace Sprout.Exam.Business.DataTransferObjects
{
    public class CalculateSalaryDto
    {
        public decimal AbsentDays { get; set; }
        public decimal WorkedDays { get; set; }
        public int TypeId { get; set; }
    }
}
