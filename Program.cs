using DependencyInjectionLifetimeSample.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.TryAddTransient<IService, PrimaryService>();
builder.Services.TryAddTransient<IService, PrimaryService>();
builder.Services.TryAddTransient<IService, SecondaryService>();

var descriptor = new ServiceDescriptor(
    typeof(IService),
    typeof(PrimaryService),
    ServiceLifetime.Transient
); 

builder.Services.TryAddEnumerable(descriptor);

builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, PrimaryService>());
builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService,  PrimaryService>());
//builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IService, SecondaryService>());

/*

    public void Test ([FromService] IService service) {
        ...
    }

*/

var app = builder.Build();


/*
using( var scope = app.Services.CreateScope())
{
     var services = scope.ServiceProvider;

     var repository = services.GetRequiredService<ICustomerRepository>();

     repository.CreateAsync(new Customer("André Aquilau"));
}
*/

/*
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        var service = context
        .HttpContext
        .RequestServices
        .GetRequiredService<ICustomerRepository>();

        var customer = service.Create(new Customer("André Aquilau"));
    }
*/

app.MapGet("/", handler: (IEnumerable<IService> service) =>
{
    return Results.Ok(service.Select( x => x.GetType().Name));
});

app.Run();

public interface IService
{

}
