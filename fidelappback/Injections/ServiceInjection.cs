using fidelappback.Services;

namespace fidelappback.Injections;

public static class ServiceInjection
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}