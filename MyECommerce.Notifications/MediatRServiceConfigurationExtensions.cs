using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyECommerce.Notifications;

public static class MediatRServiceConfigurationExtensions
{
    public static MediatRServiceConfiguration RegisterNotificationServices(this MediatRServiceConfiguration mediatRServiceConfiguration)
    {
        return mediatRServiceConfiguration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    }
}