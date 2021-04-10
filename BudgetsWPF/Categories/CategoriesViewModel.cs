using Budgets.BusinessLayer.Entities;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace BudgetsWPF.Categories
{
    class CategoriesViewModel: BindableBase
    {
        private User _user;
        public ObservableCollection<CategoryDetailsViewModel> Categories { get;  }

        public DelegateCommand GoToWalletsCommand { get;  }
        public DelegateCommand AddCategoryCommand { get;  }
        private CategoryDetailsViewModel _currentCategory;

        public CategoriesViewModel(User user, Action<User> goToWalletsView)
        {
            _user = user;
            GoToWalletsCommand= new DelegateCommand(() => goToWalletsView(_user));
            AddCategoryCommand = new DelegateCommand(AddCategory);
            Categories = new();
            foreach(var category in _user.Categories)
            {
                Categories.Add(new CategoryDetailsViewModel(_user, category, RemoveCategory));
            }
        }
        public void RemoveCategory(CategoryDetailsViewModel wc)
        {
            Categories.Remove(wc);
            RaisePropertyChanged(nameof(CurrentCategory));
            RaisePropertyChanged(nameof(Categories));
        }
        public CategoryDetailsViewModel CurrentCategory
        {
            get
            {
                return _currentCategory;
            }
            set
            {
                _currentCategory = value;
                RaisePropertyChanged();
            }
        }
        public void AddCategory()
        {
            Category c = new Category("", "");
            Categories.Add(new CategoryDetailsViewModel(_user, c, RemoveCategory));
            RaisePropertyChanged(nameof(Categories));
        }
    }
}
