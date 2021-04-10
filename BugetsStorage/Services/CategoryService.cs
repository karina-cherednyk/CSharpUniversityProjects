using Budgets.BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetsStorage.Services
{
    public class CategoryService: EntityService<Category>
    {
        public static async  Task FillCategories<T>(T obj)
            where T: class, ICategorizable, IStorable
        {
            var storage = new RelationStorage<T, Category>();
            var all = await All;

            List<Guid> ids = await storage.GetAsync(obj);
            ids.ForEach(id => obj.AddCategory(all[id]));
        } 
    }
}
