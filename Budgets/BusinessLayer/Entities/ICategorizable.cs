using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budgets.BusinessLayer.Entities
{
    public interface ICategorizable
    {
        public bool HasCategory(Category category);
        public bool AddCategory(Category category);
    }
}
