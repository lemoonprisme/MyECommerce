using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyECommerce.Api.Dtos;
using MyECommerce.Api.Services;
using MyECommerce.Domain;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace MyECommerce.Api.IntegrationTests;

public class ProductControllerTests : TestBase
{
    public ProductControllerTests(AppFactory appFactory) : base(appFactory)
    {
    }

    [Fact]
    public async Task Product_GetProduct_ReturnOk()
    {
        //Arrange

        //Act
        var response = await HttpClient.GetAsync($"api/product/4f2c013a-3dc7-4839-87d0-58fe9f9824bf");

        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task Product_PostProduct_ReturnBadRequest()
    {
        //Arrange

        var productDto = new CreateProductDto()
        {
            Category = "TooLongStriiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiing",
            Name = "Name",
            Status = Status.Available
        };

        //Act
        var response = await HttpClient.PostAsJsonAsync("api/product", productDto);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Product_PostProduct_ReturnOk()
    {
        //Arrange

        var productDto = new CreateProductDto()
        {
            Category = "Toy",
            Name = "PopIt",
            Status = Status.Sold
        };
        //Act
        var response = await HttpClient.PostAsJsonAsync("api/product", productDto);
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Product_PutExistingProduct_ReturnOk()
    {
        //Arrange

        var createProductDto = new CreateProductDto()
        {
            Category = "Toy",
            Name = "PopIt",
            Status = Status.Sold
        };
        var postResponse = await HttpClient.PostAsJsonAsync("api/product", createProductDto);
        var product = await postResponse.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(product);
        var putProductDto = new PutProductDto()
        {
            Category = "hoy",
            Id = product.Id,
            Name = "HoyToy",
            Status = Status.Available
        };
        var expected = new Product()
        {
            Category = "hoy",
            Id = product.Id,
            Name = "HoyToy",
            Status = Status.Available
        };

        //Act
        var response = await HttpClient.PutAsJsonAsync("api/product", putProductDto);
        var actual = await response.Content.ReadFromJsonAsync<Product>();
        //Assert
        Assert.Equivalent(expected, actual);
    }

    [Fact]
    public async Task Product_DeleteProduct_ReturnNoContent()
    {
        //Arrange
        var createProductDto = new CreateProductDto()
        {
            Category = "Toy",
            Name = "PopIt",
            Status = Status.Sold
        };
        var postResponse = await HttpClient.PostAsJsonAsync("api/product", createProductDto);
        var product = await postResponse.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(product);
        //Act

        var response = await HttpClient.DeleteAsync($"api/product/{product.Id.ToString()}");
        //Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Product_DeleteProduct_ReturnNotFound()
    {
        //Arrange

        //Act
        var response = await HttpClient.DeleteAsync($"api/product/{Guid.NewGuid()}");
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

    }

    [Fact]
    public async Task Product_PutProduct_ReturnOk()
    {
        //Arrange
        var productDto = new PutProductDto()
        {
            Id = Guid.NewGuid(),
            Category = "Toy",
            Name = "PoooopIt",
            Status = Status.Available
        };
        //Act
        var response = await HttpClient.PutAsJsonAsync("api/product", productDto);
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


}