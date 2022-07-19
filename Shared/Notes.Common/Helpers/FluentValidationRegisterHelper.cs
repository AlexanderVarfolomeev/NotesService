using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Notes.Common.Helpers;

public static class FluentValidationRegisterHelper
{
    public static void Register(IServiceCollection services)
    {
        var validators = from type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
            where !type.IsAbstract && !type.IsGenericTypeDefinition
            let interfaces = type.GetInterfaces()
            let genericInterfaces = interfaces.Where(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidator<>))
            let matchingInterface = genericInterfaces.FirstOrDefault()
            where matchingInterface != null
            select new
            {
                InterfaceType = matchingInterface,
                ValidatorType = type
            };

        validators.ToList().ForEach(x => { services.AddSingleton(x.InterfaceType, x.ValidatorType); });
    }
}