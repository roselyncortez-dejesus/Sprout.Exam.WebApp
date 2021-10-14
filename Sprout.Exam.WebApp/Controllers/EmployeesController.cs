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
    public class EmployeesController : ControllerBase
    {

        private readonly EmployeeService _employeeService;

        public EmployeesController(IEmployee service)
        {
            _employeeService = (EmployeeService)service;
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _employeeService.GetEmployees();
            if (result.status.code == ResponseCodes.SUCCESS)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await _employeeService.GetEmployeeById(id);
            if (result.status.code == ResponseCodes.SUCCESS)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto request)
        {
            var result = await _employeeService.UpdateEmployee(request);
            if (result.status.code == ResponseCodes.SUCCESS)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEmployeeDto request)
        {
            var result = await _employeeService.InsertEmployee(request);

            if (result.status.code == ResponseCodes.SUCCESS)
            {
                //return Created($"/api/employees/{result.data}", result.data);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _employeeService.DeleteEmployee(id);
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
