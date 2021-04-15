using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetsWPF.Navigation
{
    public enum NavigatableType
    {
        SignIn,
        SignUp,
        Wallets, 
        Transactions,
        Categories,
        Share
    }
    public interface INavigatable
    {
        public NavigatableType Type { get; }
    }
}
