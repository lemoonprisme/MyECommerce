using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyECommerce.Api.Dtos;
using MyECommerce.Api.Services;
using MyECommerce.Domain;

namespace MyECommerce.Api.IntegrationTests;

public class TestBase : IClassFixture<AppFactory>, IAsyncLifetime
{
    protected readonly ApplicationUser User = new()
    {
        Id = 2L,
        UserName = "Admin",
        Email = "admin@gmail.com",
        FirstName = "Admin",
        LastName = "Adminov"
    };

    protected HttpClient HttpClient { get; }
    protected IServiceProvider Services { get; }

    public TestBase(AppFactory appFactory)
    {
        Services = appFactory.Services;
        HttpClient = appFactory.CreateClient();


        var roles = new List<IdentityRole<long>>()
        {
            new IdentityRole<long>()
            {
                Id = 2L, Name = $"{RoleConsts.Administrator}", NormalizedName = $"{RoleConsts.Administrator.ToUpper()}"
            },
            new IdentityRole<long>()
                { Id = 3L, Name = RoleConsts.ViewAllOrders, NormalizedName = RoleConsts.ViewAllOrders.ToUpper() }
        };
        var token = appFactory.Services.GetRequiredService<ITokenService>().CreateToken(User, roles);
        HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var user = await userManager.FindByIdAsync(User.Id.ToString());
        if (user == null)
            await userManager.CreateAsync(User, "Test-pass123");
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}