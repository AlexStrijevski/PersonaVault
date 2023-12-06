using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonaVault.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Database.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PersonaVaultContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPersonalDetailsRepository, PersonalDetailsRepository>();
            services.AddScoped<IAddressDetailsRepository, AddressDetailsRepository>();
            return services;
        }
    }
}
