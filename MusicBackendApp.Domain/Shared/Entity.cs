namespace MusicBackendApp.Domain.Shared;

public abstract class Entity<TId> 
{
    public TId Id { get; protected set; } 
    
    protected Entity() { }

    protected Entity(TId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }
        Entity<TId> other = (Entity<TId>)obj; 
        return Id.Equals(other.Id);
    }

    public override int GetHashCode() => Id.GetHashCode(); 
    public static bool operator ==(Entity<TId>? left, Entity<TId>? right) => Equals(left, right); 
    public static bool operator !=(Entity<TId>? left, Entity<TId>? right) => !Equals(left, right);
}
