using Budgets.BusinessLayer.Entities;
using System;
using System.Threading.Tasks;

namespace BudgetsStorage.Services
{
    public class UserService : EntityService<User>
    {

        public new static async Task<User> Get(Guid id)
        {
            var user = (await All)[id];
            await CategoryService.FillCategories(user);
            await WalletService.FillWallets(user);

            return user;
        }
    }
}
