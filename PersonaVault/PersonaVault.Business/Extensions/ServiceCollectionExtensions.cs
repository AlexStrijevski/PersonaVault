using Microsoft.Extensions.DependencyInjection;
using PersonaVault.Business.Managers;
using PersonaVault.Business.Requirements;
using PersonaVault.Business.Security;
using PersonaVault.Business.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonaVault.Business.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserManager, UserManager>();
            services.AddScoped<IAddressDetailsManager, AddressDetailsManager>();
            services.AddTransient<IHashService, HashService>();
            services.AddTransient<IUserDataRequirements, UserDataRequirements>();
            services.AddScoped<IPersonalDetailsManager, PersonalDetailsManager>();
            services.AddTransient<IImageHandler, ImageHandler>();
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IPersonalDetailsRequirementsValidator, PersonalDetailsRequirementsValidator>();
            services.AddTransient<IPersonalDetailsRequirements, PersonalDetailsRequirements>();
            services.AddTransient<IAddressDetailsRequirementsValidator, AddressDetailsRequirementsValidator>();
            return services;
        }
    }
}
