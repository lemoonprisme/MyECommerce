using KafkaFlow;
using MediatR;
using MyECommerce.Domain.Events;

namespace MyECommerce.Notifications;

public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
{
    private readonly IMessageProducer<OrderCreatedEventHandler> _producer;

    public OrderCreatedEventHandler(IMessageProducer<OrderCreatedEventHandler> producer)
    {
        _producer = producer;
    }

    public async Task Handle(OrderCreatedEvent orderCreatedEvent, CancellationToken cancellationToken)
    {
        await _producer.ProduceAsync(orderCreatedEvent.OrderId, orderCreatedEvent);
    }
    
}