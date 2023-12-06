using Microsoft.EntityFrameworkCore;
using PersonaVault.Contracts.Enums;
using PersonaVault.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Database
{
    internal class PersonaVaultContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PersonalDetails> PersonalDetails { get; set; }
        public DbSet<AddressDetails> AddressDetails { get; set; }

        public PersonaVaultContext(DbContextOptions<PersonaVaultContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //One-to-One User-PersonalDetails
            modelBuilder.Entity<User>()
                .HasOne<PersonalDetails>(u => u.PersonalDetails)
                .WithOne()
                .HasForeignKey<User>(u => u.PersonalDetailsId)
                .IsRequired(false);

            //One-to-One PersonalDetails-AddressDetails
            modelBuilder.Entity<PersonalDetails>()
                .HasOne<AddressDetails>(u => u.AddressDetails)
                .WithOne()
                .HasForeignKey<PersonalDetails>(u => u.AddressDetailsId)
                .IsRequired(false);

            // Store Role as String
            modelBuilder.Entity<User>()
                .Property(r => r.Role)
                .HasConversion(
                    v => v.ToString(),
                    v => (Role)Enum.Parse(typeof(Role), v));
        }
    }
}
