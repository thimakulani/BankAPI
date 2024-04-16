namespace BankAPI.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<T> Get(int id);
        public Task<List<T>> GetAll();
        public Task Add(T entity);
        public Task Update(T entity);
        public Task Delet(T entity); 
    } 
}
