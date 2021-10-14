using AutoMapper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.DataAccess.Models;
using System;

namespace Sprout.Exam.Business.EntityMapper
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>();
            CreateMap<EditEmployeeDto, Employee>();
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeSalary, EmployeeDto>()
                .ForMember(d => d.Id, s => s.MapFrom(s => s.Employee.Id))
                .ForMember(d => d.FullName, s => s.MapFrom(s => s.Employee.FullName))
                .ForMember(d => d.Birthdate, s => s.MapFrom(s => s.Employee.Birthdate))
                .ForMember(d => d.Tin, s => s.MapFrom(s => s.Employee.Tin))
                .ForMember(d => d.TypeId, s => s.MapFrom(s => s.Employee.TypeId))                
                .ForMember(d => d.Salary, s => s.MapFrom(s => Math.Round(s.BasicRate, 2)));
                
        }
    }
}
