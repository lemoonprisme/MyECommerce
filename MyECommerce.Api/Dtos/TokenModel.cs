﻿namespace MyECommerce.Api.Dtos;

public class TokenModel
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}