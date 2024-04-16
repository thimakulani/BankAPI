using BankAPI.Models;

namespace BankAPI.Interfaces
{
    public interface IRefreshTokenService
    {
        public string GenerateJWTToken(AccountHolder accountHolder);
        public Task<RefreshToken> GenerateRefreshToken(int accountHolderId);
        public Task<RefreshToken> GetRefreshTokenByToken(string token);
        public Task RevokeRefreshToken(string token);
    }
}
