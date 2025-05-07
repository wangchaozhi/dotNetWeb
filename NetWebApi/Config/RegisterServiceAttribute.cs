
namespace NetWebApi.Config;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class RegisterServiceAttribute : Attribute
{
    public ServiceLifetime Lifetime { get; }

    // 默认生命周期为 Scoped
    public RegisterServiceAttribute(ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        Lifetime = lifetime;
    }
    
}