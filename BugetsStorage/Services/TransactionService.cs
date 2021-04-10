using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetsStorage.Services
{
    public class TransactionService: EntityService<Transaction>
    {
        private static readonly RelationStorage<Wallet, Transaction> _links = new();

        public static async Task FillTransactions(Wallet w)
        {
            List<Guid> wts = (await _links.GetAsync(w)).ToList();

            for (var i = 0; i < wts.Count; ++i)
            {
                var tran = await Get(wts[i]);
                w.AddTransaction(tran);
            }

        }

        public new static async Task<Transaction> Get(Guid id)
        {
            var tran = (await All)[id];
            await CategoryService.FillCategories(tran);

            return tran;
        }

        public static async Task FillUser(Transaction t)
        {
            var links = new RelationStorage<Transaction, User>();
            var users = await links.GetAsync(t);
            var uid = users[0];
            var user = await UserService.Get(uid);
            t.User = user;
        }

        public new static async Task Add(Transaction obj)
        {
            await AddBase(obj);
            User user = obj.User;
            var links = new RelationStorage<Transaction, User>();
            var clinks = new RelationStorage<Transaction, Category>();

            await links.AddAsync(obj, user);
            await clinks.AddAsync(obj, obj.Category);
        }
    }
}
