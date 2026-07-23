using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public AuthenticationService(IIdentityService identityService, ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        

        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDto, CancellationToken ct = default)
        {
            var UserResult = await _identityService.FindUserByEmailAsync(loginDto.Email, ct);
            if (!UserResult.IsSuccess)
            {
                return Result<UserDTO>.Fail(UserResult.Errors);
            }

            var checkPasswordResult = await _identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password, ct);
            if (!checkPasswordResult.IsSuccess)
            {
                return Result<UserDTO>.Fail(checkPasswordResult.Errors);
            }
            if(!checkPasswordResult.data)
            {
                return Result<UserDTO>.Fail(Error.Unauthorized("Invalid credentials."));
            }

            var user = UserResult.data;
            var roles = await _identityService.GetUserRolesAsync(loginDto.Email, ct);
            var token =  _tokenService.CreateToken(user.Id,user.Email,user.UserName,roles.data);

            return new UserDTO()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token
            };
        }

        public async Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDto, CancellationToken ct = default)
        {
           var userResult = await _identityService.CreateUserAsync(registerDto, ct);
           if (!userResult.IsSuccess)
           {
                return Result<UserDTO>.Fail(userResult.Errors);
           }


            var user = userResult.data;
            var roles = await _identityService.GetUserRolesAsync(user.Email, ct);
            var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, roles.data);

            return new UserDTO()
           {
                Email = userResult.data.Email,
                DisplayName = userResult.data.DisplayName,
                Token = token
           };
        }

        public async Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default)
          => await _identityService.EmailExistsAsync(email, ct);

        public async Task<Result<UserDTO>> GetCurrentUserAsync(string email, CancellationToken ct = default)
        {
            var userResult = await _identityService.FindUserByEmailAsync(email, ct);
            var user = userResult.data;
            var roles = await _identityService.GetUserRolesAsync(email, ct);
            var token = _tokenService.CreateToken(user.Id, user.Email, user.UserName, roles.data);

            return new UserDTO()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token
            };
        }

        public async Task<Result<AddressDTO>> GetUserAddressAsync(string email, CancellationToken ct = default)
        {
            return await _identityService.GetAddressByEmailAsync(email, ct);
        }

        public async Task<Result<AddressDTO>> UpSertUserAddressAsync(string email, AddressDTO address, CancellationToken ct = default)
        {
            return await _identityService.UpdateOrInsertAddressAsync(email, address, ct);
        }
    }
}
