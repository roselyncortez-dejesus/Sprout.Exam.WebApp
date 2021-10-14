using AutoMapper;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using Sprout.Exam.Business.CustomExceptions;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Business.EntityMapper;
using Sprout.Exam.Business.Extensions;
using Sprout.Exam.Business.Services;
using Sprout.Exam.Common.Constants;
using Sprout.Exam.DataAccess.Data;
using Sprout.Exam.DataAccess.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sprout.Exam.Business.Tests.Services
{
    [TestFixture]
    public class EmployeeServiceTests
    {
        private ApplicationDbContext _dbContext;
        private EmployeeService _testSubject;
        private EmployeeSalaryService _dependecyService;
        private readonly IMapper _mapper;

        public EmployeeServiceTests()
        {
            _mapper = new Mapper(new MapperConfiguration(
                cfg => cfg.AddProfile(new EmployeeMappingProfile())));
        }

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            options.ConfigureWarnings(warnings => warnings.Log(InMemoryEventId.TransactionIgnoredWarning));

            var services = new ServiceCollection();
            services.AddTransient<IOptions<OperationalStoreOptions>>(
                provider => Options.Create<OperationalStoreOptions>(new OperationalStoreOptions
                {
                    DeviceFlowCodes = new TableConfiguration("test") { Name = "DeviceCodes", Schema = null },
                    EnableTokenCleanup = false,
                    PersistedGrants = new TableConfiguration("test") { Name = "PersistedGrants", Schema = null },
                    ResolveDbContextOptions = null,
                    TokenCleanupBatchSize = 100,
                    TokenCleanupInterval = 3600
                }));
            var _provider = services.BuildServiceProvider();

            IOptions<OperationalStoreOptions> someOptions = _provider.GetService<IOptions<OperationalStoreOptions>>();

            _dbContext = new ApplicationDbContext(options.Options, someOptions);

            _dbContext.Employees.AddRange(new Employee
            {
                Id = 1,
                FullName = "Juan Dela Cruz",
                Birthdate = DateTime.Now.ToShortDateString(),
                Tin = "1111111111",
                IsDeleted = false,
                TypeId = 1
            },new Employee
            {
                Id = 2,
                FullName = "Juan Dela Cruz 2",
                Birthdate = DateTime.Now.ToShortDateString(),
                Tin = "1111111111",
                IsDeleted = false,
                TypeId = 2
            });

            _dbContext.SaveChanges();

            _testSubject = new EmployeeService(_dbContext, _mapper);

            _dbContext.EmployeeSalaries.AddRange(
                new EmployeeSalary
                {
                    EmployeeSalaryId = 1,
                    EmployeeId = 1,
                    BasicRate = 30000.00m,
                    FrequencyId = 1,
                    TaxRatePercentage = 12,
                    CreatedDateTime = DateTime.Now
                },
                new EmployeeSalary
                {
                    EmployeeSalaryId = 2,
                    EmployeeId = 2,
                    BasicRate = 1000.00m,
                    FrequencyId = 2,
                    TaxRatePercentage = 0,
                    CreatedDateTime = DateTime.Now
                }
            );

            _dbContext.SaveChanges();

            _dependecyService = new EmployeeSalaryService(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
            _dbContext = null;
        }

        #region Insert Employee
        [Test]
        public async Task InsertEmployee_Should_Succesfully_Insert_Employee_WithNoErrors()
        {
            CreateEmployeeDto request =
                new CreateEmployeeDto
                {
                    FullName = "FullName-UnitTest",
                    Birthdate = DateTime.Now.ToShortDateString(),
                    Tin = "1234567890",
                    Salary = 1000.00m,
                    TypeId = 1,
                };
            var result = await _testSubject.InsertEmployee(request);
            Assert.AreEqual(ResponseCodes.SUCCESS, result.status.code);

            int employeeId = result.data;

            var insertedEmployee = await _dbContext.Employees.Where(x => x.Id.Equals(employeeId)).FirstOrDefaultAsync();
            var insertedEmployeeSalary = await _dbContext.EmployeeSalaries.Where(x => x.EmployeeId.Equals(employeeId)).FirstOrDefaultAsync();

            Assert.AreEqual(3, employeeId);
            Assert.AreEqual(request.FullName, insertedEmployee.FullName);
            Assert.AreEqual(request.Birthdate, insertedEmployee.Birthdate);
            Assert.AreEqual(request.Tin, insertedEmployee.Tin);
            Assert.AreEqual(request.TypeId, insertedEmployee.TypeId);

            Assert.AreEqual(employeeId, insertedEmployeeSalary.EmployeeId);
            Assert.AreEqual(request.Salary, insertedEmployeeSalary.BasicRate);
            Assert.AreEqual(request.TypeId.GetFrequency(), insertedEmployeeSalary.FrequencyId);
            Assert.AreEqual(request.TypeId.GetTaxRate(), insertedEmployeeSalary.TaxRatePercentage);
        }
        [Test]
        public async Task InsertEmployee_Should_Not_Allow_Empty_Employee_Details()
        {
            CreateEmployeeDto request = new CreateEmployeeDto
            {
                FullName = "",
                Birthdate = "",
                Tin = "",
                Salary = 0,
                TypeId = 0
            };

            var result = await _testSubject.InsertEmployee(request);
            Assert.AreNotEqual(ResponseCodes.SUCCESS, result.status.code);
        }
        [Test]
        public async Task InsertEmployee_Should_Not_Allow_Duplicate_Employee()
        {
            CreateEmployeeDto request = new CreateEmployeeDto
            {
                FullName = "Juan Dela Cruz",
                Birthdate = DateTime.Now.ToShortDateString(),
                Tin = "1111111111",
                Salary = 10000.00m,
                TypeId = 1
            };

            var result = await _testSubject.InsertEmployee(request);
            Assert.AreEqual(ResponseCodes.EXISTING_RECORD, result.status.code);
        }

        #endregion

        #region GetEmployee
        [Test]
        public async Task GetEmployees_Should_Return_All_Active_Employee_WithNoErrors()
        {
            var result = await _testSubject.GetEmployees();
            Assert.AreEqual(ResponseCodes.SUCCESS, result.status.code);
            Assert.IsNotNull(result.data);
        }
        [Test]
        public async Task GetEmployeesById_Should_Return_Employee_With_The_Same_Id_WithNoErrors()
        {
            var result = await _testSubject.GetEmployeeById(1);
            Assert.AreEqual(ResponseCodes.SUCCESS, result.status.code);
            Assert.IsNotNull(result.data);
        }
        [Test]
        public async Task GetEmployeesById_Should_Return_Correct_Error_Message_If_Employee_Not_Exists_WithNoErrors()
        {
            var result = await _testSubject.GetEmployeeById(3);
            Assert.AreEqual(ResponseCodes.NO_RECORD_FOUND, result.status.code);
            Assert.IsNull(result.data);
        }
        #endregion

        #region UpdateEmployee
        [Test]
        public async Task UpdateEmployee_Should_Update_Employee_Record_WithNoErrors()
        {
            EditEmployeeDto request =
                new EditEmployeeDto
                {
                    Id = 1,
                    FullName = "Updated Name",
                    Birthdate = DateTime.Now.ToShortDateString(),
                    Tin = "555555",
                    Salary = 30000.00m,
                    TypeId = 2,
                };
            var result = await _testSubject.UpdateEmployee(request);
            Assert.AreEqual(ResponseCodes.SUCCESS, result.status.code);
            Assert.IsNotNull(result.data);

            int employeeId = result.data.Id;
            var updatedEmployee = await _dbContext.Employees.Where(x => x.Id.Equals(employeeId)).FirstOrDefaultAsync();
            var updatedEmployeeSalary = await _dbContext.EmployeeSalaries.Where(x => x.EmployeeId.Equals(employeeId)).FirstOrDefaultAsync();

            Assert.AreEqual(1, employeeId);
            Assert.AreEqual(request.FullName, updatedEmployee.FullName);
            Assert.AreEqual(request.Birthdate, updatedEmployee.Birthdate);
            Assert.AreEqual(request.Tin, updatedEmployee.Tin);
            Assert.AreEqual(request.TypeId, updatedEmployee.TypeId);

            Assert.AreEqual(employeeId, updatedEmployeeSalary.EmployeeId);
            Assert.AreEqual(request.Salary, updatedEmployeeSalary.BasicRate);
            Assert.AreEqual(request.TypeId.GetFrequency(), updatedEmployeeSalary.FrequencyId);
            Assert.AreEqual(request.TypeId.GetTaxRate(), updatedEmployeeSalary.TaxRatePercentage);
        }

        [Test]
        public async Task UpdateEmployee_Should_Return_Correct_Error_Message_If_No_Record_Found()
        {
            EditEmployeeDto request =
                 new EditEmployeeDto
                 {
                     Id = 3,
                     FullName = "Updated Name",
                     Birthdate = DateTime.Now.ToShortDateString(),
                     Tin = "555555",
                     Salary = 30000.00m,
                     TypeId = 2,
                 };
            var result = await _testSubject.UpdateEmployee(request);
            Assert.AreEqual(ResponseCodes.NO_RECORD_FOUND, result.status.code);
            Assert.IsNull(result.data);
        }

        
        [Test]
        public async Task UpdateEmployee_Service_Should_Not_Allow_Empty_Employee_Details()
        {
            EditEmployeeDto request =
                new EditEmployeeDto
                {
                    Id = 1,
                    FullName = null,
                    Birthdate = "",
                    Tin = "",
                    Salary = 0.00m,
                    TypeId = 0,
                };

            var result = await _testSubject.UpdateEmployee(request);
            Assert.AreEqual(ResponseCodes.EMPTY_FIELDS, result.status.code);
            Assert.IsNull(result.data);
        }

        [Test]
        public async Task UpdateEmployee_Service_Should_Return_Exception()
        {
            EditEmployeeDto request = null;

            var result = await _testSubject.UpdateEmployee(request);
            Assert.AreEqual(ResponseCodes.SOMETHING_WENT_WRONG, result.status.code);
            Assert.IsNull(result.data);
        }
        #endregion

        #region DeleteEmployee
        [Test]
        public async Task Delete_Should_Update_Employee_Record_IsDeleted_WithNoErrors()
        {
            EditEmployeeDto request =
                new EditEmployeeDto
                {
                    Id = 1,
                    FullName = "Updated Name",
                    Birthdate = DateTime.Now.ToShortDateString(),
                    Tin = "555555",
                    Salary = 30000.00m,
                    TypeId = 2,
                };
            var result = await _testSubject.DeleteEmployee(request.Id);
            Assert.AreEqual(ResponseCodes.SUCCESS, result.status.code);

            var deletedEmployee = await _dbContext.Employees.Where(x => x.Id.Equals(request.Id)).FirstOrDefaultAsync();
            Assert.AreEqual(true, deletedEmployee.IsDeleted);
        }
        [Test]
        public async Task DeleteEmployee_Should_Return_Correct_Error_Message_If_No_Record_Found()
        {
            EditEmployeeDto request =
                new EditEmployeeDto
                {
                    Id = 3,
                    FullName = "Updated Name",
                    Birthdate = DateTime.Now.ToShortDateString(),
                    Tin = "555555",
                    Salary = 30000.00m,
                    TypeId = 2,
                };
            var result = await _testSubject.DeleteEmployee(request.Id);
            Assert.AreEqual(ResponseCodes.NO_RECORD_FOUND, result.status.code);

        }

        #endregion
    }
}
