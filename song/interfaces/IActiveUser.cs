using user.Models;
using Microsoft.AspNetCore.Http;


namespace Active.Interfaces
{
    public interface IActiveUser
    {
        userType ActiveUser { get; }
    }
}