using System.Net;
using System.Net.Http.Json;
using MyECommerce.Api.Dtos;
using MyECommerce.Domain;

namespace MyECommerce.Api.IntegrationTests;

public class ProductControllerTests : TestBase
{
    public ProductControllerTests(AppFactory appFactory) : base(appFactory)
    {
        
    }

    [Fact]
    public async Task Product_GetProduct_ReturnOk()
    {
        var productDto = new CreateProductDto()
        {
            Category = "Toy",
            Name = "PopIt",
            Status = Status.Sold
        };
        var postResponse = await HttpClient.PostAsJsonAsync("api/product", productDto);
        var product = await postResponse.Content.ReadFromJsonAsync<Product>();
        Assert.NotNull(product);
        
        var response = await HttpClient.GetAsync($"api/product/{product.Id.ToString()}");
        
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
        Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
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