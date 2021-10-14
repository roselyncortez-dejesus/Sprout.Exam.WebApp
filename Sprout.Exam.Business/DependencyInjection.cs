using Microsoft.Extensions.DependencyInjection;
using Sprout.Exam.Business.Interface;
using Sprout.Exam.Business.Services;
using System.Reflection;

namespace Sprout.Exam.Business
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IEmployee, EmployeeService>();
            services.AddScoped<IEmployeeSalary, EmployeeSalaryService>();

            return services;
        }
    }
}
