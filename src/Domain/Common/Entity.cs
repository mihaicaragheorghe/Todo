namespace Domain.Common;

public abstract class Entity
{
    public Guid Id { get; private init; }

    private readonly List<IDomainEvent> _domainEvents = [];

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public List<IDomainEvent> PopDomainEvents()
    {
        var domainEvents = _domainEvents.ToList();
        _domainEvents.Clear();

        return domainEvents;
    }

    protected Entity() { }
}