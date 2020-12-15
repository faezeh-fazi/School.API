using System.Threading.Tasks;
using School.DataTransferObject;
using School.Models;

namespace School.Services.Main
{
    public interface IAuthenticationService
    {
        Task<bool> AddUserAsync(User staff, string password, string role);
        Task<AuthenticationResult> AuthenticateUser(AuthenticationReqest ar);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}