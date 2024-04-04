using KafkaFlow;
using KafkaFlow.Serializer;
using MyECommerce.Notifications;

namespace MyECommerce.Api.DependencyInjection;

public static class ServiceCollectionExtensions
{
    
    public static IServiceCollection AddMyECommerceKafka(this IServiceCollection serviceCollection)
    {
        const string topicName = "sample-topic";
        const string TopicName = "sample-topic";
        const string ProducerName = "say-hello";
        var services = new ServiceCollection();
        serviceCollection.AddKafka(
            kafka => kafka
                .UseConsoleLog()
                .AddCluster(
                    cluster => cluster
                        .WithBrokers(new[] { "localhost:9092" })
                        .CreateTopicIfNotExists(TopicName, 1, 1)
                        .AddProducer(
                            ProducerName,
                            producer => producer
                                .DefaultTopic(TopicName)
                                .AddMiddlewares(m =>
                                    m.AddSerializer<ProtobufNetSerializer>()
                                )
                        )
                ).AddCluster(cluster => cluster
                    .WithBrokers(new[] { "localhost:9092" })
                    .CreateTopicIfNotExists(topicName, 1, 1)
                    .AddConsumer(consumer => consumer
                        .Topic(topicName)
                        .WithGroupId("sample-group")
                        .WithBufferSize(100)
                        .WithWorkersCount(10)
                        .AddMiddlewares(middlewares => middlewares
                            .AddDeserializer<ProtobufNetDeserializer>()
                            .AddTypedHandlers(h => h.AddHandler<OrderCreatedMessageHandler>())
                        )
                    )
                )
        );
        return serviceCollection;
    }
}