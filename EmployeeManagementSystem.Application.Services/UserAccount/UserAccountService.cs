using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using EmployeeManagementSystem.Application.Dtos;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Services.Repositories;
using EmployeeManagementSystem.Domain.Services.UnitOfWorks;
using EmployeeManagementSystem.Shared.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IRepository<RefreshTokenInfo> _refreshTokenInfoRepository;

        public UserAccountService(IUnitOfWork unitOfWork,
            IOptions<JwtSection> config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
            _userAccountRepository = _unitOfWork.Repository<ApplicationUser>();
            _systemRoleRepository = _unitOfWork.Repository<SystemRole>();
            _userRoleRepository = _unitOfWork.Repository<UserRole>();
            _refreshTokenInfoRepository = _unitOfWork.Repository<RefreshTokenInfo>();
        }
        public async Task<GeneralResponseDto> CreateAsync(RegisterDto? user)
        {
            ArgumentNullException.ThrowIfNull(user);

            if (user.Email != null)
            {
                var checkUser = await FindUserByEmail(user.Email);

                if (checkUser is not null) return new GeneralResponseDto(false, "User registered already");
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

                // check, create and assign roles
                var checkAdminRole = await _systemRoleRepository
                    .FindAsync(_ => _.Name!.Equals(Constants.Admin));

                if (checkAdminRole is null)
                {
                    var adminRole = await _systemRoleRepository.Add(
                        new SystemRole
                        {
                            Name = Constants.Admin
                        });

                    if (applicationUser != null && adminRole != null)
                    {
                        await _userRoleRepository.Add(new UserRole
                        {
                            RoleId = adminRole.Id,
                            UserId = applicationUser.Id
                        });
                    }

                    _unitOfWork.Commit();

                    return new GeneralResponseDto(true, "Account created");
                }

                var checkUserRole = await _systemRoleRepository
                    .FindAsync(_ => _.Name!.Equals(Constants.User));

                if (checkUserRole is null)
                {
                    var response = await _systemRoleRepository.Add(
                        new SystemRole
                        {
                            Name = Constants.User
                        });

                    if (applicationUser != null && response != null)
                    {
                        await _userRoleRepository.Add(
                            new UserRole
                            {
                                RoleId = response.Id,
                                UserId = applicationUser.Id
                            });
                    }
                }
                else
                {
                    if (applicationUser != null)
                    {
                        await _userRoleRepository.Add(
                            new UserRole
                            {
                                RoleId = checkUserRole.Id,
                                UserId = applicationUser.Id
                            });
                    }
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

        public async Task<LoginResponseDto> SignInAsync(LoginDto? user)
        {
            if (user is null) return new LoginResponseDto(false, "Model is empty");

            var applicationUser = await FindUserByEmail(user.Email!);

            if (applicationUser is null) return new LoginResponseDto(false, "User not found");

            // Verifying password
            if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Password)) return new LoginResponseDto(false, "Email/Password not valid");

            var userRole = await FindUserRole(applicationUser.Id);

            if (userRole is null) return new LoginResponseDto(false, "User role not found");

            var roleName = await FindRoleName(userRole.RoleId);

            if (roleName is null) return new LoginResponseDto(false, "User role not found");

            var jwtToken = GenerateToken(applicationUser, roleName.Name!);
            var refreshToken = GenerateRefreshToken();

            try
            {
                // Saving the refresh token to the database
                var userFounded = await _refreshTokenInfoRepository.FindAsync(_ => _.UserId == applicationUser.Id);

                if (userFounded is not null)
                {
                    userFounded!.Token = refreshToken;
                    _unitOfWork.Commit();
                }
                else
                {
                    await _refreshTokenInfoRepository.Add(new RefreshTokenInfo
                    {
                        Token = refreshToken,
                        UserId = applicationUser.Id
                    });

                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }
            
            return new LoginResponseDto(true, "Login successfully", jwtToken, refreshToken);
        }

        public async Task<LoginResponseDto> RefreshTokenAsync(RefreshTokenDto? token)
        {
            if (token is null) return new LoginResponseDto(false, "Model is empty");

            var tokenFounded = await _refreshTokenInfoRepository.FindAsync(_ => _.Token!.Equals(token.Token));

            if(tokenFounded is null) return new LoginResponseDto(false, "Refresh token is required");

            // Getting user details
            var user = await _userAccountRepository.FindAsync(_ => _.Id == tokenFounded.UserId);

            if (user is null) return new LoginResponseDto(false, "Refresh token could not be generated because user not found");

            var userRole = await FindUserRole(user.Id);
            var roleName = await FindRoleName(userRole!.RoleId);
            var jwtToken = GenerateToken(user, roleName!.Name!);
            var refreshToken = GenerateRefreshToken();

            var refreshTokenOfUser = await _refreshTokenInfoRepository.FindAsync(_ => _.UserId == user.Id);

            if(refreshTokenOfUser is null) return new LoginResponseDto(false, "Refresh token could not be generated because user not found");

            try
            {
                refreshTokenOfUser.Token = refreshToken;
                _unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw;
            }
            
            return new LoginResponseDto(true, "Token refreshed successfully", jwtToken, refreshToken);
        }

        public async Task<WeatherForecast[]> GetWeatherForecast()
        {
            return null;
        }

        private async Task<ApplicationUser?> FindUserByEmail(string email) =>
            await _userAccountRepository
                .FindAsync(_ => _.Email != null && _.Email.ToLower() == email.ToLower());

        private async Task<UserRole?> FindUserRole(Guid userId) => await _userRoleRepository.FindAsync(_ => _.UserId == userId);
        private async Task<SystemRole?> FindRoleName(Guid roleId) => await _systemRoleRepository.FindAsync(_ => _.Id == roleId);

        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private string GenerateToken(ApplicationUser user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Value.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            // Claims in JWT Token are used to store key data (ex.: username, timezone, roles, etc)
            // in the token payload, besides the IssuedAt, which is added by default.
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role!)
            };

            var token = new JwtSecurityToken(
                issuer: _config.Value.Issuer,
                audience: _config.Value.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
