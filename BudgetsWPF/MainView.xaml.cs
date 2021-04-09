﻿using System.Windows.Controls;
using Budgets.BusinessLayer.Entities;
using BudgetsWPF.Authentication;
using BudgetsWPF.Wallets;

namespace BudgetsWPF
{
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            Content = new SignUpView(GoToSignInView, GoToWalletsView);
        }
        public void GoToSignUpView()
        {
            Content = new SignUpView(GoToSignInView, GoToWalletsView);
        }
        public void GoToSignInView()
        {
            Content = new SignInView(GoToSignUpView, GoToWalletsView);
        }
        public void GoToWalletsView(User user)
        {
            Content = new WalletsView(user, GoToSignInView);
        }
 
    }
}