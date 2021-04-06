using System;
using System.Text.Json.Serialization;

namespace Budgets.BusinessLayer.Entities
{
    public enum EntityState
    {
        Active,
        Deleted
    }
    public abstract class BaseEntity: IStorable
    {
     
        public Guid Id { get; protected set; }

        [JsonIgnore]
        public bool IsValid
        {
            get
            {
                return Validate();
            }
        }

        [JsonIgnore]
        public Guid Guid => Id;

        public abstract bool Validate();
        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                BaseEntity p = (BaseEntity)obj;
                return p.Id == Id;
            }
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}