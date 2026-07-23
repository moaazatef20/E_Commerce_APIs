using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDto ,CancellationToken ct = default);
        Task<Result<UserDTO>> RegisterAsync(RegisterDTO registerDto, CancellationToken ct = default);
        Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default);
        Task<Result<UserDTO>> GetCurrentUserAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> GetUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> UpSertUserAddressAsync(string email,AddressDTO address ,CancellationToken ct = default);
    }
}
