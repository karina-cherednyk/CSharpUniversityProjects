using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace BudgetsWPF.Wallets
{
    class WalletsViewModel: BindableBase
    {
        private readonly WalletService _walletService;
        private WalletDetailsViewModel _currentWallet;
        public ObservableCollection<WalletDetailsViewModel> Wallets { get; set; }




        public WalletsViewModel()
        {
            _walletService = new WalletService();
            Wallets = new ObservableCollection<WalletDetailsViewModel>();
            foreach (var wallet in _walletService.GetWallets())
            {
                Wallets.Add(new WalletDetailsViewModel(wallet));
            }
        }

        public WalletDetailsViewModel CurrentWallet
        {
            get
            {
                return _currentWallet;
            }
            set
            {
                _currentWallet = value;
                RaisePropertyChanged();
            }
        }

    }
}
