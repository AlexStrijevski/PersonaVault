using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.Responses
{
    public class ActionResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ActionResponse(bool isSuccess, int statusCode, string errorMessage)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Message = errorMessage;
        }
        public ActionResponse(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}
