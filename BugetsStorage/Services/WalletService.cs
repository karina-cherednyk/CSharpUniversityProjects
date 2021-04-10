using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetsStorage.Services
{
    public class WalletService: EntityService<Wallet>
    {
        private static readonly RelationStorage<User, Wallet> _links = new();

        public static async Task FillWallets(User u)
        {
            List<Guid> wts = (await _links.GetAsync(u)).ToList();
            for(var i=0; i<wts.Count; ++i)
            {
                var wal = await Get(wts[i]);
                u.AddWallet(wal);
            }

        }

        public new static async Task<Wallet> Get(Guid id)
        {
            var wall = (await All)[id];
            await CategoryService.FillCategories(wall);

            return wall;
        }

        public new static async Task Add(Wallet obj)
        {
            await AddBase(obj);
            var transactions = obj.Transactions;
            var cats = obj.Categories.ToList();
            var links = new RelationStorage<Wallet, Transaction>();
            var clinks = new RelationStorage<Wallet, Category>();

            transactions.ForEach(async (t) => {
                await TransactionService.Add(t);
                await links.AddAsync(obj, t);
             });

            cats.ForEach(async c =>
            {
                await clinks.AddAsync(obj, c);
            });
        }

    }
}
