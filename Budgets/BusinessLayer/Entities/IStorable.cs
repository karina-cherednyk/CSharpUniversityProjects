using System;

namespace Budgets.BusinessLayer.Entities
{
    public interface IStorable
    {
        Guid Guid { get; }
        public bool HasChanges { get; set; }
    }
    
}
