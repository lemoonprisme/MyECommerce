

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyECommerce.Application.DependencyInjection;

public static class MediatRServiceConfigurationExtensions
{
    public static MediatRServiceConfiguration RegisterCustomApplicationServices(this MediatRServiceConfiguration mediatRServiceConfiguration)
    {
        mediatRServiceConfiguration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        mediatRServiceConfiguration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        
        return mediatRServiceConfiguration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    }
}