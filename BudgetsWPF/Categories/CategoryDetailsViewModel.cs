using Budgets.BusinessLayer.Entities;
using BudgetsStorage.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using BudgetsStorage;
using System.Threading;

namespace BudgetsWPF.Categories
{
    class CategoryDetailsViewModel : BindableBase
    {
        
        public DelegateCommand RemoveCategoryCommand { get; }
        public DelegateCommand SaveCategoryCommand { get; }
        private User _user;
        private Category _category;
        private Action<CategoryDetailsViewModel> _removeCategoryFromCategoriesView;

        public CategoryDetailsViewModel(User user, Category category, Action<CategoryDetailsViewModel> removeCategory)
        {
            _category = category;
            _user = user;
            _removeCategoryFromCategoriesView = removeCategory;
            RemoveCategoryCommand = new DelegateCommand(() => new Thread(() => RemoveCategory()).Start(), CanRemoveCategory);
            SaveCategoryCommand = new DelegateCommand(() => new Thread(() => SaveCategory()).Start(), CanSaveCategory);
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            private set
            {
                _isEnabled = value;
                RaisePropertyChanged();
            }
        }

        public string DisplayName {
            get
            {
                if (_category.IsNew)
                    return "New category";
                else return _category.Name;
            }
            
        }
        public async void RemoveCategory()
        {
            IsEnabled = false;
            await RelationService<User, Category>.RemoveConnection(_user, _category);
            _user.RemoveCategory(_category);
            _removeCategoryFromCategoriesView(this);
            IsEnabled = true;
        }
        public async void SaveCategory() {
            IsEnabled = false;
            await CategoryService.Add(_category);
            await RelationService<User, Category>.AddConnection(_user, _category);
            _user.AddCategory(_category);

            _category.HasChanges = false;
            _category.IsNew = false;
            RemoveCategoryCommand.RaiseCanExecuteChanged();
            SaveCategoryCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(DisplayName));
            IsEnabled = true;
        }

        public bool CanSaveCategory() => _category.HasChanges && _category.IsValid;

        public bool CanRemoveCategory() => !_category.IsNew;

        public string Name
        {
            get
            {
                return _category.Name;
            }
            set
            {
                _category.Name = value;
                SaveCategoryCommand.RaiseCanExecuteChanged();
            }
        }

        public string Description
        {
            get
            {
                return _category.Description;
            }
            set
            {
                _category.Description = value;
                SaveCategoryCommand.RaiseCanExecuteChanged();
            }
        }

    }
}
