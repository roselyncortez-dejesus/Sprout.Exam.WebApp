using Sprout.Exam.Business.DataTransferObjects;
using System.Threading.Tasks;
using static Sprout.Exam.Business.DataTransferObjects.GenericResponse;

namespace Sprout.Exam.Business.Interface
{
    public interface IEmployeeSalary
    {
        Task<ResultResponse<decimal>> CalculateSalary(int id, CalculateSalaryDto request);
    }
}
