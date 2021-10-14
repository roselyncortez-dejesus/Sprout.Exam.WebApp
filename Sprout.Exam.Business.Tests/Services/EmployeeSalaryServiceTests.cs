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
using Sprout.Exam.DataAccess.Data;
using Sprout.Exam.DataAccess.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
namespace Sprout.Exam.Business.Tests.Services
{
    [TestFixture]
    public class EmployeeSalaryServiceTests
    {
        private ApplicationDbContext _dbContext;
        private EmployeeSalaryService _testSubject;
        private EmployeeService _dependencyService;
        private readonly IMapper _mapper;

        public EmployeeSalaryServiceTests()
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
                TypeId = 1,
            }, new Employee
            {
                Id = 2,
                FullName = "Noli De Jesus",
                Birthdate = DateTime.Now.ToShortDateString(),
                Tin = "22222222",
                IsDeleted = false,
                TypeId = 2,
            });

            _dbContext.SaveChanges();

            _dependencyService = new EmployeeService(_dbContext, _mapper);

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

            _testSubject = new EmployeeSalaryService(_dbContext);


        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Dispose();
            _dbContext = null;
        }

        #region Calculate Regular Salary
        [Test]
        public async Task Calculate_Salary_Regular_Employee_No_Absent_WithNoErrors()
        {
            int employeeId = 1;
            decimal expectedSalary = 26400.00m;
            CalculateSalaryDto request =
                new CalculateSalaryDto
                {
                    AbsentDays = 0,
                    WorkedDays = 0,
                    TypeId = 1,
                };
            var result = await _testSubject.CalculateSalary(employeeId, request);
            Assert.AreEqual(expectedSalary, result.data);

        }
        [Test]
        public async Task Calculate_Salary_Regular_Employee_With_Absent_WithNoErrors()
        {
            int employeeId = 1;
            decimal expectedSalary = 23672.73m;
            CalculateSalaryDto request =
                new CalculateSalaryDto
                {
                    AbsentDays = 2,
                    WorkedDays = 0,
                    TypeId = 1,
                };
            var result = await _testSubject.CalculateSalary(employeeId, request);
            Assert.AreEqual(expectedSalary, result.data);
        }
        [Test]
        public async Task Calculate_Salary_Regular_Employee_With_Half_Day_Absent_WithNoErrors()
        {
            int employeeId = 1;
            decimal expectedSalary = 25718.18m;
            CalculateSalaryDto request =
                new CalculateSalaryDto
                {
                    AbsentDays = 0.5m,
                    WorkedDays = 0,
                    TypeId = 1,
                };
            var result = await _testSubject.CalculateSalary(employeeId, request);
            Assert.AreEqual(expectedSalary, result.data);
        }
        #endregion

        #region Calculate Regular Salary
        [Test]
        public async Task Calculate_Salary_Contractual_Employee_WithNoErrors()
        {
            int employeeId = 2;
            decimal expectedSalary = 30000.00m;
            CalculateSalaryDto request =
                new CalculateSalaryDto
                {
                    AbsentDays = 0,
                    WorkedDays = 30,
                    TypeId = 2,
                };
            var result = await _testSubject.CalculateSalary(employeeId, request);
            Assert.AreEqual(expectedSalary, result.data);

        }
        [Test]
        public async Task Calculate_Salary_Contractual_Employee_With_Work_Days_With_Decimal_Place_WithNoErrors()
        {
            int employeeId = 2;
            decimal expectedSalary = 20500.00m;
            CalculateSalaryDto request =
                new CalculateSalaryDto
                {
                    AbsentDays = 0,
                    WorkedDays = 20.5m,
                    TypeId = 2,
                };
            var result = await _testSubject.CalculateSalary(employeeId, request);
            Assert.AreEqual(expectedSalary, result.data);
        }

        #endregion
    }
}
