namespace Budgets.BusinessLayer.Entities
{
    public enum EntityState
    {
        Active,
        Deleted
    }
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public bool IsNew { get; protected set; }
        public bool HasChanges { get; protected set; }
        public bool IsValid
        {
            get
            {
                return Validate();
            }
        }
        public EntityState State { get; set; }

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
            return Id;
        }
    }
}