using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.Requests
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "Username cannot be empty")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password cannot be empty")]
        public string Password { get; set; }
    }
}
