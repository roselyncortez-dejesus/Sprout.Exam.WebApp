using Sprout.Exam.Common.Enums;

namespace Sprout.Exam.Business.Extensions
{
    public static class CheckFrequency 
    {
        public static int GetFrequency(this int employeeType)
        {
            var frequency = 0;
            switch (employeeType)
            {
                case (int)EmployeeType.Regular:
                    frequency = (int)SalaryRateFrequency.Monthly;
                    break;
                case (int)EmployeeType.Contractual:
                    frequency = (int)SalaryRateFrequency.Daily;
                    break;
                default:
                    return 0;
            }

            return frequency;
        }
    }
}
