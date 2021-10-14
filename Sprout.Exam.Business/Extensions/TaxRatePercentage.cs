using Sprout.Exam.Common.Enums;

namespace Sprout.Exam.Business.Extensions
{
    public static class TaxRatePercentage
    {
        public static int GetTaxRate(this int employeeType)
        {
            int taxRate;
            switch (employeeType)
            {
                case (int)EmployeeType.Regular:
                    taxRate = (int)TaxRate.Reguar;
                    break;
                case (int)EmployeeType.Contractual:
                    taxRate = (int)TaxRate.Contractrual;
                    break;
                default:
                    return 0;
            }

            return taxRate;
        }
    }
}
