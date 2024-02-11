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

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    public List<IDomainEvent> PopDomainEvents()
    {
        var domainEvents = _domainEvents.ToList();
        _domainEvents.Clear();

        return domainEvents;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected Entity() { }
}