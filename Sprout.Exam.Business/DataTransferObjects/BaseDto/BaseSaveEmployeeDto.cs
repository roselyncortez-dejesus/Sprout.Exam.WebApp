using System;

namespace Sprout.Exam.Business.DataTransferObjects.BaseDto
{
    public abstract class BaseSaveEmployeeDto
    {
        public string FullName { get; set; }
        public string Tin { get; set; }
        public string Birthdate { get; set; }
        public int TypeId { get; set; }
        public decimal Salary { get; set; }
    }
}
