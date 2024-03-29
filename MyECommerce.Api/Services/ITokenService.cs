using Microsoft.AspNetCore.Identity;
using MyECommerce.Domain;

namespace MyECommerce.Api.Services;

public interface ITokenService
{
    string CreateToken(ApplicationUser user, List<IdentityRole<long>> role);
}
