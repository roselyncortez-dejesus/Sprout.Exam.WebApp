using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Business.CustomExceptions
{
    [Serializable]
    public partial class EmployeeException : Exception
    {
        public EmployeeException()
            : base("Something Went Wrong! Please try again later.")
        {

        }
    }

    [Serializable]
    public partial class InvalidEmployeeNameException : Exception
    {
        public InvalidEmployeeNameException()
        {

        }

        public InvalidEmployeeNameException(string name)
            : base(String.Format("Invalid Employee Name: {0}", name))
        {

        }

    }
    [Serializable]
    public partial class DuplicateEmployeeRecordException : Exception
    {
        public DuplicateEmployeeRecordException()
        {

        }

        public DuplicateEmployeeRecordException(string name)
            : base(String.Format("Employee already exists: {0}", name))
        {

        }

    }

    [Serializable]
    public partial class NoEmployeeRecordFoundException : Exception
    {
        public NoEmployeeRecordFoundException()
        {

        }

        public NoEmployeeRecordFoundException(string name)
            : base(String.Format("No employee record found: {0}", name))
        {

        }

    }

}
