using MediatR;

namespace MyECommerce.Domain.Events;

public abstract class Event : INotification
{
    public Guid Id { get; set; }
    public DateTime TimeStamp { get; set; }
    
    protected Event()
    {
        TimeStamp = DateTime.UtcNow;
        Id = Guid.NewGuid();
    }
}
public abstract class OrderEvent : Event
{
    public Guid OrderId { get; set; }
    public long UserId { get; set; }
    
    protected OrderEvent(long userId, Guid orderId) : base()
    {
        UserId = userId;
        OrderId = orderId;
    }
}

public class OrderCreatedEvent : OrderEvent
{
    public OrderCreatedEvent( long userId, Guid orderId, Address address) : base(userId, orderId)
    {
        Address = address;
    }

    public Address Address { get; set; }
}