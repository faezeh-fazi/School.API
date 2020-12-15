using School.Contracts;
using School.DataTransferObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using School.Services.Main;
using School.Models;
using Microsoft.AspNetCore.Identity;
using School.DataAccess;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace School.API.Controllers
{
    [Produces("application/json")]

    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _context;
        private ILoggerManager _logger;
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private SignInManager<User> _signInManager;


        public AuthenticationController(IAuthenticationService context, ILoggerManager logger, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)


        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;

        }


        /// <returns>return authentication result model which consist of success flag and token</returns>
        /// 
        [HttpPost("/api/login")]
        [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticationReqest ar)
        {
            var staff = await _context.AuthenticateUser(ar);
            if (!staff.Success)
            {
                _logger.LogError($"Failed Login attempt for {ar.Username}");
            }

            return Ok(staff);
        }

        [HttpPost("/api/refreshToken")]
        [ProducesResponseType(typeof(AuthenticationResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest ar)
        {
            var staff = await _context.RefreshTokenAsync(ar.Token, ar.RefreshToken);
            return Ok(staff);
        }


      

        }
}
