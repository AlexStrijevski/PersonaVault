using PersonaVault.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.Responses
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
        public Guid Id { get; set; }
        public Role Role { get; set; }

        public LoginResponse(bool isSuccess, Guid id, Role role)
        {
            IsSuccess = isSuccess;
            Id = id;
            Role = role;
        }
        public LoginResponse(bool isSuccess, int statusCode, string message)
        {
            IsSuccess = isSuccess;
            ErrorMessage = message;
            StatusCode = statusCode;
        }
    }
}
