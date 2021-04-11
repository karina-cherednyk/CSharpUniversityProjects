using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BudgetsWPF.Transactions
{
    public class TransactionsViewModel
    {
        public ObservableCollection<TransactionDetailsViewModel> Transactions;
       public TransactionsViewModel(User user, Wallet wallet, Action goToWallets)
        {
            Transactions = new();
            foreach(var transaction in wallet.Transactions)
            {
                Transactions.Add(new TransactionDetailsViewModel(wallet, transaction));
            }
        }
    }
}
