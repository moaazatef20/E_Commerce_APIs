using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_Commerce.Infrastructure.Identity.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }



        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<bool>.Fail(Error.NotFound("User not found", $"User with the {email} not found"));
            }
            else
            {
                var isMatch = await _userManager.CheckPasswordAsync(user, password);
                return Result<bool>.OK(isMatch);
            }
        }


        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Result<IdentityUserResult>.Fail(Error.NotFound("User not found",$"User with the {email} not found"));
            }

            return Result<IdentityUserResult>.OK(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDTO registerDTO, CancellationToken ct = default)
        {
            var user = new ApplicationUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                PhoneNumber = registerDTO.PhoneNumber
            };
           var result = await _userManager.CreateAsync(user, registerDTO.Password);
           if (!result.Succeeded)
           {
                var errors = result.Errors.Select(e => Error.Failure(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
           }

           return Result<IdentityUserResult>.OK(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
               return Error.NotFound("User not found", $"User with the {email} not found");

            return (await _userManager.GetRolesAsync(user)).ToList();
        }

        public async Task<Result<bool>> EmailExistsAsync(string email, CancellationToken ct = default)
        {
           return await _userManager.FindByEmailAsync(email) is not null;
        }

        public async Task<Result<AddressDTO>> GetAddressByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x=>x.Email == email, ct);
            if (user == null || user.Address == null)
            {
                return Result<AddressDTO>.Fail(Error.NotFound("Address not found", $"Address for user with email {email} not found"));
            }

            return Result<AddressDTO>.OK(new AddressDTO
            {
                City = user.Address.City,
                Street = user.Address.Street,
                Country = user.Address.Country,
                FirstName = user.Address.FirstName,
                LastName = user.Address.LastName
            });
        }

        public async Task<Result<AddressDTO>> UpdateOrInsertAddressAsync(string email, AddressDTO addressDTO, CancellationToken ct = default)
        {
            var user = await _userManager.Users.Include(x => x.Address).FirstOrDefaultAsync(x => x.Email == email, ct);
            if (user?.Address == null)
            {
                user.Address = new Address()
                {
                    FirstName = addressDTO.FirstName,
                    LastName = addressDTO.LastName,
                    City = addressDTO.City,
                    Country = addressDTO.Country,
                    Street = addressDTO.Street
                };
            }
            else
            {
                user.Address.FirstName = addressDTO.FirstName;
                user.Address.LastName = addressDTO.LastName;
                user.Address.City = addressDTO.City;
                user.Address.Country = addressDTO.Country;
                user.Address.Street = addressDTO.Street;
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) 
            {
                return addressDTO;
            }
            else
            {
                return Error.Failure("Failure", string.Join(", ", result.Errors.Select(x => x.Description)));
            }
        }
    }
}
