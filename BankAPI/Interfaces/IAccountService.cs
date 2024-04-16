using BankAPI.Models;

namespace BankAPI.Interfaces
{
    public interface IAccountService
    {
        public Task<IEnumerable<BankAccount>> GetAccountsForAccountHolderAsync(int accountHolderId);
        public Task<BankAccount> GetAccountByAccountNumberAsync(string accountNumber);
        public  Task<bool> CreateWithdrawalAsync(string accountNumber, decimal withdrawalAmount);
        public Task<string> GetAccountBalance(string accountNumber);
        public Task<AccountHolder> GetAccountHolder(int accountHolderid); 
        public Task<AccountHolder> GetAccountHolderByAccountNumber(string accountNumber); 
    }
}
