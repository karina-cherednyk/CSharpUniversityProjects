using Budgets.BusinessLayer.Entities;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BudgetsStorage.Services
{
    public abstract class EntityService<T> where T : class, IStorable
    {
        private static readonly AsyncLazy<Dictionary<Guid, T>> _all;
        protected static readonly EntityStorage<T> _storage = new();

        public static AsyncLazy<Dictionary<Guid, T>> All { get { return _all; } }

        static EntityService()
        {
            _all = new(async () =>
            {
                var vals = await _storage.GetAllAsync();
                Dictionary<Guid, T> map = new();
                vals.ForEach(x => { x.HasChanges = false; map.Add(x.Guid, x); });
                return map;
            });
        }

        public static async Task Add(T obj)  => await AddBase(obj);
        public static async Task AddBase(T obj)
        {
            await _storage.AddOrUpdateAsync(obj);
            (await All).TryAdd(obj.Guid, obj);
        }

        public static  async Task<T> Get(Guid id) => (await All)[id];

    }
}

