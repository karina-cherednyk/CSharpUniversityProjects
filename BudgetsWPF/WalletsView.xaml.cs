using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BudgetsWPF.Authentication;

namespace BudgetsWPF
{
    public partial class WalletsView : UserControl
    {
        public WalletsView(DBUser user, Action goToSignInView)
        {
            InitializeComponent();
        }
    }
}
