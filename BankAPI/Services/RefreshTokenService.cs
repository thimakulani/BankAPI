using BankAPI.Interfaces;
using BankAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankAPI.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRepository<RefreshToken> _refreshToken;
        private readonly IConfiguration _config;
        private readonly IAccountService _accountService;
        public RefreshTokenService(IRepository<RefreshToken> refreshTokenRepository, IConfiguration config, IAccountService accountService)
        {
            _refreshToken = refreshTokenRepository;
            _config = config;
            _accountService = accountService;
        }

        public string GenerateJWTToken(AccountHolder accountHolder)
        {
            return GenerateToken(accountHolder); 
        }

        public async Task<RefreshToken> GenerateRefreshToken(int accountHolderId)
        {
            var acc = await _accountService.GetAccountHolder(accountHolderId);
            string token = GenerateToken(acc); 
            DateTime expiresAt = DateTime.UtcNow.AddDays(30);
            var refreshToken = new RefreshToken
            {
                Token = token,
                ExpiresAt = expiresAt,
                AccountHolderId = accountHolderId
            };
            await _refreshToken.Add(refreshToken);
            return refreshToken;
        }
        
        public async Task<RefreshToken> GetRefreshTokenByToken(string token)
        {
            var data = await _refreshToken.GetAll();
            return data.FirstOrDefault(x => x.Token == token);
        }
       
        public async Task RevokeRefreshToken(string token) 
        {
            var refreshToken = await GetRefreshTokenByToken(token);
            if (refreshToken != null)
            {
                await _refreshToken.Delete(refreshToken);
            }
        }
        private string GenerateToken(AccountHolder user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.IdNumber),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.GivenName, user.FirstName),
                new(ClaimTypes.Surname, user.LastName),
                new(ClaimTypes.MobilePhone, user.PhoneNumber),
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}