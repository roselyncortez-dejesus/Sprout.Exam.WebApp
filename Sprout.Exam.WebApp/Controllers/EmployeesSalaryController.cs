using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Business.Interface;
using Sprout.Exam.Business.Services;
using Sprout.Exam.Common.Constants;
using System.Threading.Tasks;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesSalaryController : ControllerBase
    {
        private readonly EmployeeSalaryService _employeeSalaryService;

        public EmployeesSalaryController(IEmployeeSalary salaryService)
        {
            _employeeSalaryService = (EmployeeSalaryService)salaryService;
        }

        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> CalculateSalary([FromRoute] int id, [FromBody] CalculateSalaryDto request)
        {
            var result = await _employeeSalaryService.CalculateSalary(id, request);

            if (result.status.code == ResponseCodes.SUCCESS)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
