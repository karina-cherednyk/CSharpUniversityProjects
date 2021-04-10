using Budgets.BusinessLayer.Entities;
using Prism.Commands;
using Prism.Mvvm;
using System;


namespace BudgetsWPF.Categories
{
    class CategoryDetailsViewModel: BindableBase
    {
        private Action<CategoryDetailsViewModel> _removeCategory;
        public DelegateCommand SaveCategoryComand;
        public DelegateCommand RemoveCategoryComand;

        public CategoryDetailsViewModel(Category category, Action<CategoryDetailsViewModel> removeCategory)
        {
            Category = category;
            RemoveCategoryComand = new DelegateCommand(() => removeCategory(this));
            
        }

        public string DisplayName => Category.Name;
        public Category Category { get; internal set; }
    }
}
