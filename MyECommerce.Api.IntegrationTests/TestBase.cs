using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyECommerce.Api.Dtos;
using MyECommerce.Api.Services;
using MyECommerce.Domain;

namespace MyECommerce.Api.IntegrationTests;

public class TestBase : IClassFixture<AppFactory>, IAsyncLifetime
{
    protected ApplicationUser User { get; private set; } = default!;


    protected HttpClient HttpClient { get; }
    protected IServiceProvider Services { get; }

    public TestBase(AppFactory appFactory)
    {
        Services = appFactory.Services;
        HttpClient = appFactory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        var user = new ApplicationUser()
        {
            UserName = $"Admin_{Guid.NewGuid()}",
            Email = $"admin{Guid.NewGuid()}@gmail.com",
            FirstName = "Admin",
            LastName = "Adminov"
        };
        using var scope = Services.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var identityResult = await userManager.CreateAsync(user);
        Assert.True(identityResult.Succeeded, identityResult.ToString());
        User = user;
        var roles = new List<IdentityRole<long>>()
        {
            new IdentityRole<long>()
            {
                Id = 2L, Name = $"{RoleConsts.Administrator}", NormalizedName = $"{RoleConsts.Administrator.ToUpper()}"
            },
            new IdentityRole<long>()
                { Id = 3L, Name = RoleConsts.ViewAllOrders, NormalizedName = RoleConsts.ViewAllOrders.ToUpper() }
        };

        var token = Services.GetRequiredService<ITokenService>().CreateToken(User, roles);
        HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}