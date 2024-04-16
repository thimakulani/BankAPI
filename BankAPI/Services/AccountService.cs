using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAPI.Data;
using BankAPI.Interfaces;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class AccountService : IAccountService
{
    private readonly IRepository<BankAccount> _accountRepository;
    private readonly BankDbContext _context;

    public AccountService(IRepository<BankAccount> accountRepository, BankDbContext context)
    {
        _accountRepository = accountRepository;
        _context = context;
    }

    public async Task<IEnumerable<BankAccount>> GetAccountsForAccountHolderAsync(int accountHolderId)
    {
        var accounts = await _accountRepository.GetAll();
        return accounts.Where(a => a.AccountHolderId == accountHolderId);
    }

    public async Task<BankAccount> GetAccountByAccountNumberAsync(string accountNumber)
    {
        var bankAccounts = await _accountRepository.GetAll();
        return bankAccounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
    }

    public async Task<bool> CreateWithdrawalAsync(string accountNumber, decimal withdrawalAmount)
    {
        var account = await GetAccountByAccountNumberAsync(accountNumber);
        if (account == null)
        {
            throw new ArgumentException("Account not found.");
        }

        if (withdrawalAmount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be greater than 0.");
        }

        if (account.Status != "Active")
        {
            throw new InvalidOperationException("Withdrawals are not allowed on inactive bank accounts.");
        }

        if (withdrawalAmount > account.AvailableBalance)
        {
            throw new InvalidOperationException("Withdrawal amount exceeds available balance.");
        }

        if (account.AccountType == "Fixed Deposit" && withdrawalAmount != account.AvailableBalance)
        {
            throw new InvalidOperationException("Only 100% withdrawal are allowed on a Fixed Deposit account type.");
        }

        account.AvailableBalance -= withdrawalAmount;
        _context.Entry(account).State = EntityState.Modified;

        var withdrawalDetails = new
        {
            AccountNumber = account.AccountNumber,
            WithdrawalAmount = withdrawalAmount,
            Timestamp = DateTime.UtcNow
        };
        var json = JsonConvert.SerializeObject(withdrawalDetails);

        var auditLog = new AuditLog
        {
            TableName = "BankAccounts",
            Action = "Withdrawal",
            NewData = $"{json}",
            Timestamp = DateTime.UtcNow
        };
        _context.AuditLogs.Add(auditLog);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string> GetAccountBalance(string accountNumber)
    {
        var account = await _context.BankAccounts.FindAsync(accountNumber);
        return $"Available Balance: R{account.AvailableBalance}";
    }

    public async Task<AccountHolder> GetAccountHolder(int accountHolderId)
    {
        var acc_holder = await _context.AccountHolders.FindAsync(accountHolderId);
        return acc_holder;
    }

    public async Task<AccountHolder> GetAccountHolderByAccountNumber(string accountNumber)
    {
       var acc = await _context.BankAccounts.Include(x=>x.AccountHolder).Where(x=>x.AccountNumber == accountNumber.Trim()).FirstOrDefaultAsync();
        return acc.AccountHolder;
    }
}
