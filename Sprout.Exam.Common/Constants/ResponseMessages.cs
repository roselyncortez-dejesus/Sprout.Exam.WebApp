using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Common.Constants
{
    public static class ResponseMessages
    {
        public const string SUCCESS = "success";
        public const string EMPTY_FIELDS = "Please provide the following field/s: {fieldnames}";
        public const string EMPTY_FIELD = "{Property_Name} is a required field.";
        public const string EXISTING_RECORD = "This record already exists. Duplicate record not allowed";
        public const string NO_RECORD_FOUND = "This record already exists. Duplicate record not allowed";
        public const string SOMETHING_WENT_WRONG = "Something Went Wrong! Please try again later";
    }
}
