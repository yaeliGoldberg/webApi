using Active.Interfaces;
using user.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;


namespace Active.Services
{
    public class ActiveUserService : IActiveUser
    {
        public userType? ActiveUser { get; private set; }
        // public ActiveUserService(IHttpContextAccessor context)
        // {
        //     var userId = context?.HttpContext?.User?.FindFirst("userid");
        //     if (userId != null)
        //     {
        //         ActiveUser = new userType
        //         {
        //             Id = int.Parse(userId.Value),
        //             Name = context?.HttpContext?.User?.FindFirst("username")?.Value
        //         };
        //     }
        // }
        public ActiveUserService(IHttpContextAccessor context)
        {
            var user = context?.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                ActiveUser = new userType
                {
                    Id = int.Parse(user.FindFirst("userid")?.Value),
                    Name = user.FindFirst("username")?.Value
                };
            }
        }

    }

    public static partial class UserExtensions
    {
        public static IServiceCollection AddActiveUser(this IServiceCollection services)
        {
            services.AddScoped<IActiveUser, ActiveUserService>();
            return services;
        }
    }
}