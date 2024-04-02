using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyECommerce.Api.Dtos;
using MyECommerce.Api.IntegrationTests.Builders;
using MyECommerce.Domain;
using MyECommerce.Infrastructure;

namespace MyECommerce.Api.IntegrationTests;

public class OrderControllerTests : TestBase, IAsyncLifetime
{
    public OrderControllerTests(AppFactory appFactory) : base(appFactory)
    {
    }

    [Fact]
    public async Task Orders_GetOrder_ReturnProduct()
    {
        Order order = new OrderBuilder(User.Id);

        using var scope = Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ApplicationContext>().Add(order);
        await scope.ServiceProvider.GetRequiredService<ApplicationContext>().SaveChangesAsync();

        var response = await HttpClient.GetAsync($"api/order/{order.Id}");

        Assert.Equivalent(order, await response.Content.ReadFromJsonAsync<Order>());
    }

    [Fact]
    public async Task Orders_PostOrder_ReturnBarRequest()
    {
        var createOrderDto = new OrderBuilder(User.Id).CleanProducts().GetCreateOrderDto();

        var response = await HttpClient.PostAsJsonAsync("api/order", createOrderDto);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Orders_PostOrder_ReturnProduct()
    {
        var order = new OrderBuilder(User.Id);
        var createOrderDto = order.GetCreateOrderDto();

        var response = await HttpClient.PostAsJsonAsync("api/order", createOrderDto);
        Assert.Equivalent(order, await response.Content.ReadFromJsonAsync<Order>());
    }

    [Fact]
    public async Task Orders_DeleteOrder_ReturnNoContent()
    {
        Order order = new OrderBuilder(User.Id);
        using var scope = Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ApplicationContext>().Add(order);
        await scope.ServiceProvider.GetRequiredService<ApplicationContext>().SaveChangesAsync();


        var response = await HttpClient.DeleteAsync($"api/order/{order.Id}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Orders_DeleteOrder_ReturnNotFound()
    {
        var response = await HttpClient.DeleteAsync($"api/order/{Guid.NewGuid()}");
        
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Orders_GetUsersOrders_ReturnOrders()
    {
        Order order1 = new OrderBuilder(User.Id).AddProducts(new OrderItem(){ProductId = Guid.NewGuid(), Quantity = 2});
        Order order2 = new OrderBuilder(User.Id);
        var orders = new List<Order>() { order1, order2 };
        
        using var scope = Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
        dbContext.AddRange(orders);
        await dbContext.SaveChangesAsync();

        var response = await HttpClient.GetAsync("api/order");

        (await response.Content.ReadFromJsonAsync<List<Order>>())
            .Should().BeEquivalentTo(orders, options => options.Excluding(o => o.Id));
    }
}