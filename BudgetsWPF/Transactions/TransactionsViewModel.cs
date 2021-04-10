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
            var transactions = new List<Transaction>();
            new TransactionDetailsViewModel(wallet, transactions[0]);
            Transactions = new();
            foreach(var transaction in wallet.Transactions)
            {
                Transactions.Add(new TransactionDetailsViewModel(wallet, transaction));
            }
        }
    }
}
