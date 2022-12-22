namespace CarSharingApp.Domain.Primitives
{
    public abstract class Entity
    {
        public virtual Guid Id { get; protected set; }
    }
}
