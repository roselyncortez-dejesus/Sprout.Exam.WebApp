using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Business.Interface;
using Sprout.Exam.Common.Constants;
using Sprout.Exam.Common.Enums;
using Sprout.Exam.DataAccess.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using static Sprout.Exam.Business.DataTransferObjects.GenericResponse;

namespace Sprout.Exam.Business.Services
{
    public class EmployeeSalaryService : IEmployeeSalary
    {
        private readonly ApplicationDbContext _dbContext;
        public EmployeeSalaryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ResultResponse<decimal>> CalculateSalary(int id, CalculateSalaryDto request)
        {
            ResultResponse<decimal> mainResponse = new ResultResponse<decimal>();
            ResultResponse<decimal> salaryCalculateResponse = new ResultResponse<decimal>();
            mainResponse.data = 0;

            switch (request.TypeId)
            {
                case (int)EmployeeType.Regular:
                    salaryCalculateResponse = await CalculateRegularEmployeeSalary(id, request.AbsentDays);
                    break;
                case (int)EmployeeType.Contractual:
                    salaryCalculateResponse = await CalculateContractualEmployeeSalary(id, request.WorkedDays);
                    break;
                default:
                    break;
            }

            if(salaryCalculateResponse.status.code == "")
            {
                mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;
            }
            else
            {
                mainResponse = salaryCalculateResponse;
            }
           
            return mainResponse;
        }

        #region PRIVATE METHODS
        private async Task<ResultResponse<decimal>> CalculateRegularEmployeeSalary(int id, decimal absentDays)
        {
            ResultResponse<decimal> mainResponse = new ResultResponse<decimal>();
            mainResponse.data = 0;
            try
            {
                var employee = await _dbContext.EmployeeSalaries.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
                if (employee == null)
                {
                    mainResponse.status.code = ResponseCodes.NO_RECORD_FOUND;
                    mainResponse.status.message = ResponseMessages.NO_RECORD_FOUND;
                    mainResponse.data = 0;
                    return mainResponse;
                }

                decimal rate = employee.BasicRate;
                decimal taxPercentage = employee.TaxRatePercentage;
                decimal absentDeduction = 0;

                decimal tax = rate * (taxPercentage / 100);
                if (absentDays > 0)
                {
                    decimal ratePerDay = rate / 22;
                    absentDeduction = ratePerDay * absentDays;
                }

                decimal salary = rate - absentDeduction - tax;
                salary = Math.Round(salary, 2);

                mainResponse.status.code = ResponseCodes.SUCCESS;
                mainResponse.status.message = ResponseMessages.SUCCESS;
                mainResponse.data = salary;
                return mainResponse;
            }
            catch (Exception)
            {
                mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;
                mainResponse.data = 0;
                return mainResponse;
            }

        }

        private async Task<ResultResponse<decimal>> CalculateContractualEmployeeSalary(int id, decimal workedDays)
        {
            ResultResponse<decimal> mainResponse = new ResultResponse<decimal>();
            mainResponse.data = 0;
            try
            {
                var employee = await _dbContext.EmployeeSalaries.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();

                if(employee == null)
                {
                    mainResponse.status.code = ResponseCodes.NO_RECORD_FOUND;
                    mainResponse.status.message = ResponseMessages.NO_RECORD_FOUND;
                    mainResponse.data = 0;
                    return mainResponse;
                }

                decimal rate = employee.BasicRate;
                decimal salary = (rate * workedDays);

                salary = Math.Round(salary, 2);

                mainResponse.status.code = ResponseCodes.SUCCESS;
                mainResponse.status.message = ResponseMessages.SUCCESS;
                mainResponse.data = salary;
                return mainResponse;

            }
            catch (Exception)
            {
                mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;
                mainResponse.data = 0;
                return mainResponse;
            }
        }
        #endregion
    }
}
