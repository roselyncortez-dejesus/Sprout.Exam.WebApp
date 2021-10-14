using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprout.Exam.Business.DataTransferObjects
{
    public class GenericResponse
    {
        public class ResultResponse<T>
        {
            public Status status { get; set; } = new Status();
            public T data { get; set; }

            public ResultResponse()
            {
                status = new Status();
            }

        }

        public class Status
        {
            public string code { get; set; }
            public string message { get; set; }

        }
    }
}
