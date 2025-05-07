using System.Reflection;

namespace NetWebApi.Config;
public static class ServiceRegistrationExtensions
{

    
    public static void AddServicesFromAttributes(this IServiceCollection services, Assembly assembly)
    {
        // 获取程序集中的所有类型
        var typesWithAttributes = assembly.GetTypes()
            .Where(type => type.GetCustomAttributes(typeof(RegisterServiceAttribute), true).Any());

        foreach (var type in typesWithAttributes)
        {
            // 获取自定义的 RegisterServiceAttribute 注解
            var attribute = (RegisterServiceAttribute)type.GetCustomAttribute(typeof(RegisterServiceAttribute));

            // 根据注解中的生命周期类型进行注册，如果未指定则使用默认的 Scoped
            switch (attribute.Lifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(type);
                    break;
                case ServiceLifetime.Transient:
                    services.AddTransient(type);
                    break;
                case ServiceLifetime.Scoped:
                default:
                    services.AddScoped(type);
                    break;
            }
        }
    }
    
}
