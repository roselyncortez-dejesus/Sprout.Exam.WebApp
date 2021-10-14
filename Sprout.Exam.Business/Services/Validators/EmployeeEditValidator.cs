using FluentValidation;
using FluentValidation.Results;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Constants;
using Sprout.Exam.DataAccess.Data;
using System;
using System.Linq;

namespace Sprout.Exam.Business.Services.Validators
{
    public class EmployeeEditValidator : AbstractValidator<EditEmployeeDto>
    {
        public EmployeeEditValidator(ApplicationDbContext dbContext)
        {
            RuleFor(request => request.Id)
               .NotEmpty().WithErrorCode(ResponseCodes.EMPTY_FIELDS);

            RuleFor(request => request.FullName)
                .NotEmpty().WithErrorCode(ResponseCodes.EMPTY_FIELDS);

            RuleFor(request => request.Birthdate)
                .NotEmpty().WithErrorCode(ResponseCodes.EMPTY_FIELDS);

            RuleFor(request => request.Salary)
                .NotEmpty().WithErrorCode(ResponseCodes.EMPTY_FIELDS);

            RuleFor(request => request.Tin)
                .NotEmpty().WithErrorCode(ResponseCodes.EMPTY_FIELDS);

            RuleFor(request => request.TypeId)
                .NotEmpty().WithErrorCode(ResponseCodes.EMPTY_FIELDS);

            RuleFor(request => request).Custom((request, context) =>
            {
                var result = dbContext.Employees.FirstOrDefault(e => e.Id.Equals(request.Id));

                if (result == null)
                {
                    context.AddFailure(new ValidationFailure("", ResponseMessages.NO_RECORD_FOUND)
                    {
                        ErrorCode = ResponseCodes.NO_RECORD_FOUND
                    });
                }
            }).When(request => request.Id > 0);
        }
    }
}
