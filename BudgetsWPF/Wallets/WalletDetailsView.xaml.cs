using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace BudgetsWPF.Wallets
{

    public partial class WalletDetailsView : UserControl
    {
        public WalletDetailsView()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9\\.]+");
            e.Handled = !regex.IsMatch(e.Text);
        }
        private void WalletCategories_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (CategoriesListBox.SelectedIndex == -1)
                AddCategoryBtn.Content = "Add category";
            else AddCategoryBtn.Content = "Remove category";
            
        }
    }
}
