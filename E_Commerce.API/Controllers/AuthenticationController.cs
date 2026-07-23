using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.API.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO ,CancellationToken ct)
            => toActionResult(await _authenticationService.LoginAsync(loginDTO, ct));


        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO, CancellationToken ct)
            => toActionResult(await _authenticationService.RegisterAsync(registerDTO, ct));

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists([FromQuery] string email, CancellationToken ct)
            => toActionResult(await _authenticationService.CheckEmailExistsAsync(email, ct));

        [Authorize]
        [HttpGet("CurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser(CancellationToken ct)
        {
            var email = GetUserEmailFromToken();
            return toActionResult(await _authenticationService.GetCurrentUserAsync(email, ct));
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress(CancellationToken ct)
        {
            var email = GetUserEmailFromToken();
            return toActionResult(await _authenticationService.GetUserAddressAsync(email, ct));
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress(AddressDTO addressDTO, CancellationToken ct)
        {
            return toActionResult( await _authenticationService.UpSertUserAddressAsync(GetUserEmailFromToken(), addressDTO, ct));
        }

    }
}