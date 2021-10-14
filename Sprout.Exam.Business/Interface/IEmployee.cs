using Sprout.Exam.Business.DataTransferObjects;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Sprout.Exam.Business.DataTransferObjects.GenericResponse;

namespace Sprout.Exam.Business.Interface
{
    public interface IEmployee
    {
        Task<ResultResponse<int>> InsertEmployee(CreateEmployeeDto request);
        Task<ResultResponse<EmployeeDto>> UpdateEmployee(EditEmployeeDto request);
        Task<ResultResponse<EmployeeDto>> DeleteEmployee(int id);
        Task<ResultResponse<EmployeeDto>> GetEmployeeById(int id);
        Task<ResultResponse<List<EmployeeDto>>> GetEmployees();
    }
}
