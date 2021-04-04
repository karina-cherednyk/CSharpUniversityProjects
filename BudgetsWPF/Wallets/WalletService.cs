using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetsWPF.Wallets
{
    public class WalletService
    {
        private static List<Wallet> Wallets = new List<Wallet>()
        {
            new Wallet() {Name = "wal1", Balance = 57.06m},
            new Wallet() {Name = "wal2", Balance = 157.06m},
            new Wallet() {Name = "wal3", Balance = 257.06m},
            new Wallet() {Name = "wal4", Balance = 357.06m},
            new Wallet() {Name = "wal5", Balance = 457.06m},
        };

        public List<Wallet> GetWallets()
        {
            return Wallets.ToList();
        }
    }
}
