using Budgets.BusinessLayer.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace BudgetsStorage.Services
{
    
    public static class RelationService<T,U>
        where T: class, IStorable
        where U: class, IStorable
    {
        private static readonly RelationStorage<T, U> _storage = new ();

        public static async Task<bool> AddConnection(T t, U u)
        {
            Thread.Sleep(1000);
            return await _storage.AddAsync(t, u);
        }

        public static async Task<bool> RemoveConnection(T t, U u)
        {
            Thread.Sleep(1000);
            return await _storage.RemoveAsync(t, u);
        }
    
    }
}
