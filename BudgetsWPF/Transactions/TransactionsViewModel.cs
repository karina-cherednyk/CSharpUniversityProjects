using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;

namespace BudgetsWPF.Transactions
{
    public class TransactionsViewModel
    {
       public TransactionsViewModel(User user, Wallet wallet, Action goToWallets)
        {
            var transactions = new List<Transaction>();
            new TransactionDetailsViewModel(transactions[0]);
        }
    }
}
