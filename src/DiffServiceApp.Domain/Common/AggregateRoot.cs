namespace DiffServiceApp.Domain.Common;

public abstract class AggregateRoot : BaseEntity
{
    protected AggregateRoot(int id) : base(id) { }

    protected AggregateRoot() { }
}