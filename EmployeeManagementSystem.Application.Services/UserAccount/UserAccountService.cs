using EmployeeManagementSystem.Application.Dtos;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Services.Repositories;
using EmployeeManagementSystem.Domain.Services.UnitOfWorks;
using EmployeeManagementSystem.Shared.Helpers;
using Microsoft.Extensions.Options;
using Constants = EmployeeManagementSystem.Shared.Helpers.Constants;

namespace EmployeeManagementSystem.Application.Services.UserAccount
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOptions<JwtSection> _config;
        private readonly IRepository<ApplicationUser> _userAccountRepository;
        private readonly IRepository<SystemRole> _systemRoleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public UserAccountService(IUnitOfWork unitOfWork,
            IOptions<JwtSection> config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _userAccountRepository = _unitOfWork.Repository<ApplicationUser>();
            _systemRoleRepository = _unitOfWork.Repository<SystemRole>();
            _userRoleRepository = _unitOfWork.Repository<UserRole>();
        }
        public async Task<GeneralResponseDto> CreateAsync(RegisterDto user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var checkUser = await FindUserByEmail(user.Email);

            if (checkUser != null)
            {
                return new GeneralResponseDto(false, "User registered already");
            }

            try
            {
                var applicationUser = await _userAccountRepository.Add(
                    new ApplicationUser
                    {
                        Fullname = user.Fullname,
                        Email = user.Email,
                        Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
                    });

                // check, create and assign role
                var checkAdminRole = await _systemRoleRepository
                    .FindAsync(_ => _.Name!.Equals(Constants.Admin));

                if (checkAdminRole is null)
                {
                    var adminRole = await _systemRoleRepository.Add(
                        new SystemRole
                        {
                            Name = Constants.Admin
                        });

                    await _userRoleRepository.Add(new UserRole
                    {
                        RoleId = adminRole.Id,
                        UserId = applicationUser.Id
                    });
                }

                _unitOfWork.Commit();

                return new GeneralResponseDto(true, "Account created");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<LoginResponseDto> SignInAsync(LoginDto user)
        {
            throw new NotImplementedException();
        }

        private async Task<ApplicationUser> FindUserByEmail(string email) =>
        await _userAccountRepository
            .FindAsync(_ => _.Email != null && _.Email.ToLower() == email.ToLower());
        
    }
}
