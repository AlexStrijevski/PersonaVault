using Microsoft.Extensions.DependencyInjection;
using PersonaVault.Contracts.Mappers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Contracts.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddContractsServices(this IServiceCollection services)
        {
            services.AddTransient<IUserDataMapper, UserDataMapper>();
            services.AddTransient<IPersonalDetailsMapper, PersonalDetailsMapper>();
            return services;
        }
    }
}
