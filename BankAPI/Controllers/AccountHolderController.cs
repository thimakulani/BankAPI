using BankAPI.Interfaces;
using BankAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BankAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountHolderController : ControllerBase
    {
        private readonly IAccountService _accountService; 
       
        private readonly IRefreshTokenService _refreshTokenService;
        public AccountHolderController(IAccountService accountService, IRefreshTokenService refreshTokenService)
        {
            _accountService = accountService;
            
            _refreshTokenService = refreshTokenService;
        }
        [AllowAnonymous]
        [HttpPost("login/{accountNumber}")]
        public async Task<IActionResult> Login(string accountNumber)
        {
            if(string.IsNullOrWhiteSpace(accountNumber)) {  return BadRequest(); }
            var accHolder =  await _accountService.GetAccountByAccountNumberAsync(accountNumber);
            if (accHolder == null) {  return BadRequest("Invalid account number"); }

            var refreshToken = await _refreshTokenService.GenerateRefreshToken(accHolder.Id);
            var jwtToken = _refreshTokenService.GenerateJWTToken(accHolder.AccountHolder);
            return Ok(new { AccessToken = jwtToken, RefreshToken = refreshToken.Token });
        }
        [HttpGet("{accountHolderId}")]
        public async Task<IActionResult> GetAccountsForAccountHolder(int accountHolderId)
        {
            var accounts = await _accountService.GetAccountsForAccountHolderAsync(accountHolderId);
            return Ok(accounts);
        }

        [HttpGet("account/{accountNumber}")]
        public async Task<IActionResult> GetAccountByAccountNumber(string accountNumber)
        {
            var account = await _accountService.GetAccountByAccountNumberAsync(accountNumber);
            if (account == null)
                return NotFound();
            return Ok(account);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> CreateWithdrawal(WithdrawalViewModel request)
        {
            try
            {
                var withdrawal = await _accountService.CreateWithdrawalAsync(request.AccountNumber, request.WithdrawalAmount);
                return Ok(withdrawal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
