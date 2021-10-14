using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Business.Extensions;
using Sprout.Exam.Business.Interface;
using Sprout.Exam.Business.Services.Validators;
using Sprout.Exam.Common.Constants;
using Sprout.Exam.DataAccess.Data;
using Sprout.Exam.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Sprout.Exam.Business.DataTransferObjects.GenericResponse;

namespace Sprout.Exam.Business.Services
{
    public class EmployeeService : IEmployee
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public EmployeeService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ResultResponse<EmployeeDto>> GetEmployeeById(int Id)
        {
            ResultResponse<EmployeeDto> mainResponse = new ResultResponse<EmployeeDto>();
            mainResponse.data = null;
            try
            {
                var employee = await _dbContext.EmployeeSalaries
               .Include(e => e.Employee)
               .Where(e => e.Employee.Id.Equals(Id))
               .FirstOrDefaultAsync();

                if (employee != null)
                {
                    mainResponse.status.code = ResponseCodes.SUCCESS;
                    mainResponse.status.message = ResponseMessages.SUCCESS;
                    mainResponse.data = _mapper.Map<EmployeeDto>(employee);
                    return mainResponse;
                }
                else
                {
                    mainResponse.status.code = ResponseCodes.NO_RECORD_FOUND;
                    mainResponse.status.message = ResponseMessages.NO_RECORD_FOUND;
                    mainResponse.data = null;
                    return mainResponse;
                }

            }
            catch (Exception)
            {
                mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;
                return mainResponse;
            }

        }

        public async Task<ResultResponse<List<EmployeeDto>>> GetEmployees()
        {
            ResultResponse<List<EmployeeDto>> mainResponse = new ResultResponse<List<EmployeeDto>>();
            mainResponse.data = null;
            try
            {
                var employee = await _dbContext.EmployeeSalaries
                    .Include(e => e.Employee)
                    .Where(e => e.Employee.IsDeleted.Equals(false))
                    .ToListAsync();

                mainResponse.status.code = ResponseCodes.SUCCESS;
                mainResponse.status.message = ResponseMessages.SUCCESS;
                mainResponse.data = _mapper.Map<List<EmployeeDto>>(employee);
                return mainResponse;
            }
            catch (Exception)
            {
                

                mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;
                return mainResponse;
            }

        }

        public async Task<ResultResponse<int>> InsertEmployee(CreateEmployeeDto request)
        {
            ResultResponse<int> mainResponse = new ResultResponse<int>();
            mainResponse.data = 0;

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    #region Validation
                    EmployeeInsertValidator validator = new EmployeeInsertValidator(_dbContext);
                    var resultValidator = validator.Validate(request);
                    if (!resultValidator.IsValid)
                    {
                        if (resultValidator.Errors.Where(x => x.ErrorCode == ResponseCodes.EMPTY_FIELDS).Count() > 1)
                        {
                            mainResponse.status.code = resultValidator.Errors.Select(x => x.ErrorCode).FirstOrDefault();
                            mainResponse.status.message = ResponseMessages.EMPTY_FIELDS.Replace("{fieldnames}", String.Join(",", resultValidator.Errors.Where(y => y.ErrorCode == ResponseCodes.EMPTY_FIELDS).Select(x => x.PropertyName)));
                            mainResponse.data = 0;
                        }
                        else
                        {
                            mainResponse.status.code = resultValidator.Errors.Select(x => x.ErrorCode).FirstOrDefault();
                            mainResponse.status.message = resultValidator.Errors.Select(x => x.ErrorMessage).FirstOrDefault();
                        }

                        return mainResponse;
                    }
                    #endregion

                    Employee employee = _mapper.Map<Employee>(request);

                    var result = _dbContext.Employees.Add(employee);
                    await _dbContext.SaveChangesAsync();

                    var employeeId = (result != null ? result.Entity.Id : 0);

                    #region Insert Salary Details
                    if (employeeId > 0)
                    {
                        var resultSalary = await InsertEmployeeSalaryDetails(employeeId, request.Salary, request.TypeId);

                        if (resultSalary.status.code == "0")
                        {
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            return resultSalary;
                        }

                    }
                    #endregion

                    mainResponse.status.code = ResponseCodes.SUCCESS;
                    mainResponse.status.message = ResponseMessages.SUCCESS;
                    mainResponse.data = employeeId;

                    return mainResponse;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();


                    mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                    mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;
                    return mainResponse;
                }
            }
        }

        public async Task<ResultResponse<EmployeeDto>> UpdateEmployee(EditEmployeeDto request)
        {
            ResultResponse<EmployeeDto> mainResponse = new ResultResponse<EmployeeDto>();
            mainResponse.data = null;
            try
            {
                #region Validation
                EmployeeEditValidator validator = new EmployeeEditValidator(_dbContext);
                var resultValidator = validator.Validate(request);
                if (!resultValidator.IsValid)
                {
                    mainResponse.status.code = resultValidator.Errors.Select(x => x.ErrorCode).FirstOrDefault();
                    mainResponse.status.message = resultValidator.Errors.Select(x => x.ErrorMessage).FirstOrDefault();
                    return mainResponse;
                }
                #endregion

                var employee = await _dbContext.Employees.Where(x => x.Id == request.Id).FirstOrDefaultAsync();

                employee.FullName = request.FullName;
                employee.Birthdate = request.Birthdate;
                employee.Tin = request.Tin;
                employee.TypeId = request.TypeId;

                _dbContext.Employees.Update(employee);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    var resultUpdateSalary = await UpdateEmployeeSalaryDetails(request.Id, request.Salary, request.TypeId);

                    mainResponse.status.code = resultUpdateSalary.status.code;
                    mainResponse.status.message = resultUpdateSalary.status.message;
                    mainResponse.data = (resultUpdateSalary.status.code == ResponseCodes.SUCCESS ? _mapper.Map<EmployeeDto>(employee) : null);
                    return mainResponse;
                }
                else
                {
                    mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                    mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;
                    return mainResponse;
                }
            }
            catch (Exception)
            {
                mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;

                return mainResponse;
            }
        }

        public async Task<ResultResponse<EmployeeDto>> DeleteEmployee(int id)
        {
            ResultResponse<EmployeeDto> mainResponse = new ResultResponse<EmployeeDto>();
            mainResponse.data = null;

            try
            {
                var employee = await _dbContext.Employees.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (employee == null)
                {
                    mainResponse.status.code = ResponseCodes.NO_RECORD_FOUND;
                    mainResponse.status.message = ResponseCodes.NO_RECORD_FOUND;

                    return mainResponse;
                }

                employee.IsDeleted = true;
                _dbContext.Employees.Update(employee);
                _dbContext.SaveChanges();

                mainResponse.status.code = ResponseCodes.SUCCESS;
                mainResponse.status.message = ResponseMessages.SUCCESS;
                mainResponse.data = _mapper.Map<EmployeeDto>(employee);
                return mainResponse;
            }
            catch (Exception)
            {
                mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;

                return mainResponse;
            }

        }

        #region PRIVATE METHODS
        private async Task<ResultResponse<int>> InsertEmployeeSalaryDetails(int id, decimal salary, int type)
        {
            ResultResponse<int> mainResponse = new ResultResponse<int>();
            mainResponse.data = 0;
            try
            {
                EmployeeSalary employeeSalary = new EmployeeSalary()
                {
                    BasicRate = salary,
                    EmployeeId = id,
                    FrequencyId = type.GetFrequency(),
                    TaxRatePercentage = type.GetTaxRate(),
                    CreatedDateTime = DateTime.Now
                };

                var result = _dbContext.EmployeeSalaries.Add(employeeSalary);
                await _dbContext.SaveChangesAsync();

                if (result.Entity.EmployeeId > 1)
                {
                    mainResponse.status.code = ResponseCodes.SUCCESS;
                    mainResponse.status.message = ResponseMessages.SUCCESS;
                    mainResponse.data = result.Entity.EmployeeId;
                    return mainResponse;

                }
                else
                {
                    mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                    mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;
                    return mainResponse;
                }


            }
            catch (Exception)
            {
                mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;
                return mainResponse;
            }
        }
        private async Task<ResultResponse<bool>> UpdateEmployeeSalaryDetails(int id, decimal salary, int type)
        {
            ResultResponse<bool> mainResponse = new ResultResponse<bool>();
            try
            {
                var employeeSalary = await _dbContext.EmployeeSalaries.Where(x => x.EmployeeId == id).FirstOrDefaultAsync();
                if (employeeSalary == null)
                {
                    mainResponse.status.code = ResponseCodes.NO_RECORD_FOUND;
                    mainResponse.status.message = ResponseMessages.NO_RECORD_FOUND;

                    return mainResponse;
                }

                employeeSalary.BasicRate = salary;
                employeeSalary.LastModifiedDateTime = DateTime.Now;
                employeeSalary.FrequencyId = type.GetFrequency();
                employeeSalary.TaxRatePercentage = type.GetTaxRate();

                _dbContext.EmployeeSalaries.Update(employeeSalary);
                var result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    mainResponse.status.code = ResponseCodes.SUCCESS;
                    mainResponse.status.message = ResponseMessages.SUCCESS;
                    mainResponse.data = true;

                    return mainResponse;
                }
                else
                {
                    mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                    mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;

                    return mainResponse;
                }

            }
            catch (Exception)
            {
                mainResponse.status.code = ResponseCodes.SOMETHING_WENT_WRONG;
                mainResponse.status.message = ResponseMessages.SOMETHING_WENT_WRONG;

                return mainResponse;
            }
        }

        #endregion
    }
}
