using Budgets.BusinessLayer.Entities;
using BudgetsStorage.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using BudgetsStorage;


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
            RemoveCategoryCommand = new DelegateCommand(RemoveCategory, CanRemoveCategory);
            SaveCategoryCommand = new DelegateCommand(SaveCategory, CanSaveCategory);
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
            await RelationService<User, Category>.RemoveConnection(_user, _category);
            _user.RemoveCategory(_category);
            _removeCategoryFromCategoriesView(this);
        }
        public async void SaveCategory() {
            await CategoryService.Add(_category);
            await RelationService<User, Category>.AddConnection(_user, _category);
            
            _category.HasChanges = false;
            _category.IsNew = false;
            RemoveCategoryCommand.RaiseCanExecuteChanged();
            SaveCategoryCommand.RaiseCanExecuteChanged();
            RaisePropertyChanged(nameof(DisplayName));
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
