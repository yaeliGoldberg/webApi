using  Active.Interfaces;
using user.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Active.Services
{
    public class ActiveUserService : IActiveUser
    {
        public userType? ActiveUser { get; private set; }
        public ActiveUserService(IHttpContextAccessor context)
        {
            var userId = context?.HttpContext?.User?.FindFirst("Id");
            if (userId != null)
            {
                ActiveUser = new userType
                {
                    Id = int.Parse(userId.Value),
                    Name = "test"
                };
            }
        }

    }

    public static partial class UserExtensions
    {
        public static  IServiceCollection AddActiveUser(this IServiceCollection services)
        {
            services.AddScoped<IActiveUser, ActiveUserService>();
            return services;
        }
    }
}