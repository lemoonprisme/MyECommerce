using KafkaFlow;
using MediatR;
using MyECommerce.Domain.Events;

namespace MyECommerce.Notifications;

public class OrderCreatedMessageHandler : IMessageHandler<OrderCreatedEvent>
{
    public Task Handle(IMessageContext context, OrderCreatedEvent message)
    {
        Console.WriteLine(
            "Partition: {0} | Offset: {1} | Message: {2} {3}",
            context.ConsumerContext.Partition,
            context.ConsumerContext.Offset,
            message.OrderId,
            message.UserId);

        return Task.CompletedTask;
    }
}