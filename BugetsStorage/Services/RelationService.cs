using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetsStorage.Services
{
    
    public static class RelationService<T,U>
        where T: class, IStorable
        where U: class, IStorable
    {
        private static readonly RelationStorage<T, U> _storage = new ();

        public static async Task AddConnection(T t, U u)
        {
            await _storage.AddAsync(t, u);
        }

        public static async Task RemoveConnection(T t, U u)
        {
            await _storage.RemoveAsync(t, u);
        }
    
    }
}
