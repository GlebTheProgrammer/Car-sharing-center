namespace CarSharingApp.Domain.Primitives
{
    public abstract class Entity
    {
        public Entity(Guid id)
        {
            Id = id;
        }

        public virtual Guid Id { get; protected set; }
    }
}
