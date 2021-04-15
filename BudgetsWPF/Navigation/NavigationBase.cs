using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetsWPF.Navigation
{

    public abstract class NavigationBase : BindableBase
    {
        public INavigatable CurrentViewModel
        {
            get; private set;
        }

        public void Navigate(INavigatable viewModel)
        {
            CurrentViewModel = viewModel;
            RaisePropertyChanged(nameof(CurrentViewModel));
        }

    }
}
