using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using TechnoWave.Business.Repository.Interfaces;
using TechnoWave.Core.Models.RequestModels;

namespace TechnoWave.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginRequest loginRequest)
        {
            var res = await authService.UserLogin(loginRequest);
            return Ok(res);
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]RegisterUserRequest RegRequest)
        {
            var res = await authService.Register(RegRequest);
            return Ok(res);
        }
    }
}
