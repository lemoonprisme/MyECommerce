using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyECommerce.Api.Services;
using MyECommerce.Domain;

namespace MyECommerce.Api.IntegrationTests;

public class TestBase : IClassFixture<AppFactory>
{
    protected HttpClient HttpClient { get; }
    
    
    public TestBase(AppFactory appFactory)
    {
        HttpClient = appFactory.CreateClient();
        ApplicationUser user = new ApplicationUser()
        {
            Id = 1L,
            UserName = "Admin",
            Email = "admin@gmail.com"
        };
        var roles = new List<IdentityRole<long>>()
            { new IdentityRole<long>() { Id = 2, Name = "Administrator", NormalizedName = "ADMINISTRATOR" } };
        var token = appFactory.Services.GetRequiredService<ITokenService>().CreateToken(user, roles); 
        HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
    }

    
}