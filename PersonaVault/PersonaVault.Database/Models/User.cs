using PersonaVault.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Database.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Role Role { get; set; }
        public Guid? PersonalDetailsId { get; set; }
        public PersonalDetails? PersonalDetails { get; set; }
    }
}
