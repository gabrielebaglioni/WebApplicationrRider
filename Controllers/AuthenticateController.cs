using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplicationrRider.Controllers.Support;
using WebApplicationrRider.Domain.Services;
using WebApplicationrRider.Entity;


namespace WebApplicationrRider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : BaseController
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthenticateController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [HttpPost]
     
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model.Username == null || model.Password == null)
                return Unauthorized();
            
            var result = await _authenticateService.AuthenticateUser(model.Username, model.Password);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(result),
                expiration = result!.ValidTo
            });
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (model.Username == null || model.Email == null || model.Password == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthenticateResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            

            var result = await _authenticateService.Register(model.Username, model.Email, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthenticateResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            
            return Ok(new AuthenticateResponse { Status = "Success", Message = "User created successfully!" });

        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            if (model.Username == null || model.Email == null || model.Password == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthenticateResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            

            var result = await _authenticateService.RegisterAdmin(model.Username, model.Email, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthenticateResponse { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            
            return Ok(new AuthenticateResponse { Status = "Success", Message = "User created successfully!" });

        }
        [HttpGet("checkuser")]
        public async Task<ActionResult<bool>> CheckIfUsernameExistsAsync(string username)
        {
            var userExists = await _authenticateService.CheckIfUsernameExistsAsync(username);

            return Ok(new { exists = userExists });
        }
        [HttpGet("checkemail")]
        public async Task<ActionResult<bool>> CheckIfEmailExistsAsync(string email)
        {
            var emailExists = await _authenticateService.CheckIfEmailExistsAsync(email);

            return Ok(new { exists = emailExists });
        }


    }
}
