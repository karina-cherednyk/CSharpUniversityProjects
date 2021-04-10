using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetsStorage.Services
{
    public class UserService : EntityService<User>
    {

        public new static  async Task<User> Get(Guid id)
        {
            var user = (await All)[id];
            await CategoryService.FillCategories(user);
            await WalletService.FillWallets(user);
            

            return user;
        }
        public new static async Task Add(User obj)  {
            await AddBase(obj);
            var wallets = obj.Wallets.ToList();
            var cats = obj.Categories.ToList();

            var links = new RelationStorage<User, Wallet>();
            var clinks = new RelationStorage<User, Category>();

            wallets.ForEach(async w =>  
            {
                await WalletService.Add(w);
                await links.AddAsync(obj, w);
            });

            cats.ForEach(async c =>
            {
                await clinks.AddAsync(obj, c);
            });
        }

    }
}
