﻿using System.ComponentModel.DataAnnotations;

namespace MyECommerce.Api.Dtos;

public class RegisterRequestDto
{
    [Required]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Дата рождения")]
    public DateTime BirthDate { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;

    [Required]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    [DataType(DataType.Password)]
    [Display(Name = "Подтвердить пароль")]
    public string PasswordConfirm { get; set; } = null!;

    [Required]
    [Display(Name = "Имя")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Фамилия")]
    public string LastName { get; set; } = null!;

}