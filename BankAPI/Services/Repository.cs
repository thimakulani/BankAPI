using BankAPI.Data;
using BankAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BankAPI.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly BankDbContext _context;

        public Repository(BankDbContext context)
        {
            _context = context;
        }
        public async Task Add(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delet(T entity) 
        {
            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task Update(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
